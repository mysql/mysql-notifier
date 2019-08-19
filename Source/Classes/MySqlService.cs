// Copyright (c) 2012, 2019, Oracle and/or its affiliates. All rights reserved.
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License as
// published by the Free Software Foundation; version 2 of the
// License.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
// 02110-1301  USA

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Timers;
using System.Xml.Serialization;
using MySql.Notifier.Enumerations;
using MySql.Notifier.Properties;
using MySql.Utility.Classes;
using MySql.Utility.Classes.Logging;
using MySql.Utility.Classes.MySql;
using MySql.Utility.Classes.MySqlRouter;
using MySql.Utility.Classes.MySqlWorkbench;

namespace MySql.Notifier.Classes
{
  [Serializable]
  public class MySqlService : IDisposable, IEquatable<MySqlService>, IComparable<MySqlService>
  {
    #region Fields

    /// <summary>
    /// The display name of the service.
    /// </summary>
    private string _displayName;

    /// <summary>
    /// Flag indicating if service is waiting for a status change while checking its status.
    /// </summary>
    private bool _isWaitingOnStatusChange;

    /// <summary>
    /// Number of cycles while waiting for a status change.
    /// </summary>
    private int _loops;

    /// <summary>
    /// The current status of this service.
    /// </summary>
    private MySqlServiceStatus _currentStatus;

    /// <summary>
    /// The name of this service (short name).
    /// </summary>
    private string _serviceName;

    /// <summary>
    /// The parameters used to initialize a MySQL product installation.
    /// </summary>
    private BaseStartupParameters _startupParameters;

    /// <summary>
    /// Timer used to wait for a status change.
    /// </summary>
    private readonly Timer _statusChangeTimer;

    /// <summary>
    /// The list of Workbench connections that connect to this MySQL service.
    /// </summary>
    private List<MySqlWorkbenchConnection> _workbenchConnections;

    /// <summary>
    /// The list of Workbench servers related to this MySQL service.
    /// </summary>
    private List<MySqlWorkbenchServer> _workbenchServers;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlService"/> class.
    /// </summary>
    public MySqlService()
    {
      _currentStatus = MySqlServiceStatus.Unavailable;
      _statusChangeTimer = new Timer(100);
      _displayName = string.Empty;
      _isWaitingOnStatusChange = false;
      _loops = 0;
      _serviceName = null;
      _startupParameters = null;
      _workbenchConnections = null;
      _workbenchServers = null;
      ServiceManagementObject = null;
      CompareByDisplayName = false;
      MenuGroup = null;
      PreviousStatus = MySqlServiceStatus.Unavailable;
      ServiceId = Guid.NewGuid().ToString("B");
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlService"/> class.
    /// </summary>
    public MySqlService(string serviceName, bool notificationOnChange, bool updatesTrayIcon, Machine machine = null)
      : this()
    {
      Host = machine ?? new Machine();
      NotifyOnStatusChange = notificationOnChange;
      UpdateTrayIconOnStatusChange = updatesTrayIcon;
      ServiceName = serviceName;
      SetServiceParameters(false);
    }

    #region Events

    /// <summary>
    /// Handles the case where the service successfully moved to a new status.
    /// </summary>
    /// <param name="service">MySQLService instance.</param>
    public delegate void StatusChangedHandler(MySqlService service);

    /// <summary>
    /// Handles the case where the service failed to move to a proposed status.
    /// </summary>
    /// <param name="service">MySQLService instance.</param>
    /// <param name="ex">Exception thrown while trying to change the service's status.</param>
    public delegate void StatusChangeErrorHandler(MySqlService service, Exception ex);

    /// <summary>
    /// Occurs when the service has a status change.
    /// </summary>
    public event StatusChangedHandler StatusChanged;

    /// <summary>
    /// Occurs when an error is thrown while attempting to change the status of the service.
    /// </summary>
    public event StatusChangeErrorHandler StatusChangeError;

    #endregion Events

    #region Properties

    /// <summary>
    /// Gets or sets a value indicating if comparisons are done using the display name instead of the service name.
    /// </summary>
    public bool CompareByDisplayName { get; set; }

    /// <summary>
    /// Gets or sets the display name of the service.
    /// </summary>
    [XmlAttribute(AttributeName = "DisplayName")]
    public string DisplayName
    {
      get => _displayName;
      set
      {
        if (!string.IsNullOrEmpty(value))
        {
          _displayName = value;
        }
      }
    }

    /// <summary>
    /// Gets or sets the bound machine for this service.
    /// </summary>
    [XmlIgnore]
    public Machine Host { get; set; }

    /// <summary>
    /// Gets the group of ToolStripMenuItem controls for each of the corresponding instance's context menu items.
    /// </summary>
    [XmlIgnore]
    public ServiceMenuGroup MenuGroup { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether status changes of this service are notified to users.
    /// </summary>
    [XmlAttribute(AttributeName = "NotifyOnStatusChange")]
    public bool NotifyOnStatusChange { get; set; }

    /// <summary>
    /// Gets the previous status of this service.
    /// </summary>
    [XmlIgnore]
    public MySqlServiceStatus PreviousStatus { get; private set; }

    /// <summary>
    /// Gets a unique service ID.
    /// </summary>
    [XmlIgnore]
    public string ServiceId { get; }

    /// <summary>
    /// Gets a value indicating if the WMI instance bound to this service exists.
    /// </summary>
    [XmlIgnore]
    public bool ServiceInstanceExists => Host != null
                                         && (!Host.IsOnline
                                             || ServiceManagementObject != null);

    /// <summary>
    /// Gets the WMI instance for this service.
    /// </summary>
    [XmlIgnore]
    public ManagementObject ServiceManagementObject { get; private set; }

    /// <summary>
    /// Gets or sets the name of this service (short name).
    /// </summary>
    [XmlAttribute(AttributeName = "ServiceName")]
    public string ServiceName
    {
      get => _serviceName;
      set
      {
        if (!string.Equals(_serviceName, value))
        {
          _startupParameters = null;
          _workbenchConnections = null;
        }

        _serviceName = value;
      }
    }

      /// <summary>
    /// Gets the parameters used to initialize a MySQL product installation.
    /// </summary>
    [XmlIgnore]
    public BaseStartupParameters StartupParameters
    {
      get
      {
        if (_startupParameters == null)
        {
          if (ServiceName.Contains("router", StringComparison.OrdinalIgnoreCase))
          {
            _startupParameters = RouterStartupParameters.GetStartupParameters(ServiceName);
          }
          else
          {
            _startupParameters = MySqlServerStartupParameters.GetStartupParameters(ServiceName, Host.Name);
          }

          if (_startupParameters == null)
          {
            _startupParameters = new BaseStartupParameters(Host.Name);
          }
        }

        return _startupParameters;
      }
    }

    /// <summary>
    /// Gets the current status of this service.
    /// </summary>
    [XmlIgnore]
    public MySqlServiceStatus Status
    {
      get => _currentStatus;
      private set
      {
        var oldStatus = _currentStatus;
        _currentStatus = value;
        if (_currentStatus == oldStatus)
        {
          return;
        }

        PreviousStatus = oldStatus;
        OnStatusChanged(this);
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether status changes of this service trigger an update of the tray icon.
    /// </summary>
    [XmlAttribute(AttributeName = "UpdateTrayIconOnStatusChange")]
    public bool UpdateTrayIconOnStatusChange { get; set; }

    /// <summary>
    /// Gets the list of Workbench connections that connect to this MySQL service.
    /// </summary>
    [XmlIgnore]
    public List<MySqlWorkbenchConnection> WorkbenchConnections => _workbenchConnections ?? (_workbenchConnections = StartupParameters.GetRelatedWorkbenchConnections());

    /// <summary>
    /// Gets the list of Workbench servers related to this MySQL service.
    /// </summary>
    [XmlIgnore]
    public List<MySqlWorkbenchServer> WorkbenchServers => _workbenchServers ?? (_workbenchServers = MySqlWorkbench.Servers.Where(server => string.Compare(server.ServiceName, ServiceName, StringComparison.OrdinalIgnoreCase) == 0).ToList());

    /// <summary>
    /// Gets a value indicating if the service is done with a service status change operation.
    /// </summary>
    [XmlIgnore]
    public bool WorkCompleted { get; private set; }

    #endregion Properties

    /// <summary>
    /// Converts a service status text extracted from the Service's state property to a <see cref="MySqlServiceStatus"/> enumeration value.
    /// </summary>
    /// <param name="statusText">Service's state property text.</param>
    /// <param name="convertedStatus">A <see cref="MySqlServiceStatus"/> enumeration value if a matching one was found.</param>
    /// <returns>true if a matching enumeration value is found for the given status text, false otherwise.</returns>
    public static bool GetStatusFromText(string statusText, out MySqlServiceStatus convertedStatus)
    {
      statusText = statusText.Replace(" ", string.Empty);
      var parsed = Enum.TryParse(statusText, out convertedStatus);
      return parsed;
    }

    /// <summary>
    /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
    /// </summary>
    /// <param name="other">An object to compare with this instance.</param>
    /// <returns>A value that indicates the relative order of the objects being compared.</returns>
    public int CompareTo(MySqlService other)
    {
      // A null value means that this object is greater.
      if (other == null)
      {
        return 1;
      }

      return CompareByDisplayName
        ? string.Compare(DisplayName, other.DisplayName, StringComparison.Ordinal)
        : string.Compare(ServiceName, other.ServiceName, StringComparison.Ordinal);
    }

    /// <summary>
    /// Releases all resources used by the <see cref="MySqlService"/> class
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="other">The object to compare to the current object.</param>
    /// <returns><c>true</c> if both objects are equal, <c>false</c> otherwise.</returns>
    public override bool Equals(object other)
    {
      if (!(other is MySqlService otherService))
      {
        return false;
      }

      return Equals(otherService);
    }

    /// <summary>
    /// Determines whether the specified <seealso cref="MySqlService"/> instance is equal to the current one.
    /// </summary>
    /// <param name="other">The <seealso cref="MySqlService"/> instance to compare to the current object.</param>
    /// <returns><c>true</c> if both instances are equal, <c>false</c> otherwise.</returns>
    public bool Equals(MySqlService other)
    {
      if (other == null)
      {
        return false;
      }

      return CompareByDisplayName
        ? Equals(DisplayName, other.DisplayName)
        : Equals(ServiceName, other.ServiceName);
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
      unchecked
      {
        int hashCode = Utilities.HASHING_BASE;
        hashCode = (hashCode * Utilities.HASHING_MULTIPLIER) ^ (DisplayName == null ? 0 : DisplayName.GetHashCode());
        hashCode = (hashCode * Utilities.HASHING_MULTIPLIER) ^ (ServiceName == null ? 0 : ServiceName.GetHashCode());
        return hashCode;
      }
    }

    /// <summary>
    /// Fetches the real service via WMI and its current status.
    /// </summary>
    /// <param name="retryToGetServiceInstance">Flag indicating if the method will attempt to re-fetch the service instance.</param>
    public void RefreshStatusAndName(bool retryToGetServiceInstance)
    {
      // If the WMI management object is not available, attempt to fetch it.
      if (retryToGetServiceInstance)
      {
        GetServiceInstance(false);
      }

      // If the WMI management object is still not available, set the status to Unavailable.
      var newStatusText = "Unavailable";
      if (ServiceManagementObject != null)
      {
        if (Host.IsOnline)
        {
          newStatusText = ServiceManagementObject.Properties["State"].Value.ToString();
        }

        DisplayName = ServiceManagementObject.Properties["DisplayName"].Value.ToString();
      }

      GetStatusFromText(newStatusText, out var newStatus);
      if (Status != newStatus)
      {
        // Set the new status since it is different to the previous status.
        SetStatus(newStatusText);
      }
      else
      {
        // Force the update of the UI even if the previous status is the same as the new status.
        MenuGroup?.Update();
      }
    }

    /// <summary>
    /// Resets the already retrieved related Workbench connections so they are retrieved again.
    /// </summary>
    public void ResetWorkbenchConnections()
    {
      _workbenchConnections = null;
    }

    /// <summary>
    /// Resets the already retrieved related Workbench servers so they are retrieved again.
    /// </summary>
    public void ResetWorkbenchServers()
    {
      _workbenchServers = null;
    }

    /// <summary>
    /// Attempts to stop and then start the current MySQL Service
    /// </summary>
    /// <returns>Flag indicating if the action completed successfully</returns>
    public void Restart()
    {
      if (Host.IsLocal)
      {
        ChangeServiceStatus(2);
      }
      else
      {
        RestartRemoteService();
      }
    }

    /// <summary>
    /// Sets up this service status, bound WMI service and other parameters.
    /// </summary>
    /// <param name="doNotFetchInstanceIfOffline">Flag indicating whether no attempt should be made to connect to the real host if the related machine is offline.</param>
    public void SetServiceParameters(bool doNotFetchInstanceIfOffline)
    {
      GetServiceInstance(doNotFetchInstanceIfOffline);
      try
      {
        RefreshStatusAndName(false);
        if (MenuGroup == null
            && ServiceInstanceExists)
        {
          MenuGroup = new ServiceMenuGroup(this);
        }
      }
      catch (InvalidOperationException ioEx)
      {
        ServiceManagementObject = null;
        Logger.LogException(ioEx, true, string.Format(Resources.SetServiceErrorDetail, DisplayName));
      }
    }

    /// <summary>
    /// Sets a new status for this service given the text of the new status.
    /// </summary>
    /// <param name="statusString">Text of the new status.</param>
    public void SetStatus(string statusString)
    {
      var matchingStatusFound = GetStatusFromText(statusString, out var newStatus);
      if (matchingStatusFound)
      {
        Status = newStatus;
      }
    }

    /// <summary>
    /// Attempts to start the current MySQL Service
    /// </summary>
    /// <returns>Flag indicating if the action completed successfully</returns>
    public void Start()
    {
      if (Host.IsLocal)
      {
        ChangeServiceStatus(1);
      }
      else
      {
        ExecuteWmiMethod("StartService");
      }
    }

    /// <summary>
    /// Attempts to stop the current MySQL Service
    /// </summary>
    /// <returns>Flag indicating if the action completed successfully</returns>
    public void Stop()
    {
      if (Host.IsLocal)
      {
        ChangeServiceStatus(0);
      }
      else
      {
        ExecuteWmiMethod("StopService");
      }
    }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString()
    {
      return CompareByDisplayName
        ? DisplayName
        : ServiceName;
    }

    /// <summary>
    /// Releases all resources used by the <see cref="MySqlService"/> class
    /// </summary>
    /// <param name="disposing">If true this is called by Dispose(), otherwise it is called by the finalizer</param>
    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        // Free managed resources
        _statusChangeTimer?.Dispose();
        ServiceManagementObject?.Dispose();
        MenuGroup?.Dispose();
      }

      // Add class finalizer if unmanaged resources are added to the class
      // Free unmanaged resources if there are any
    }

    /// <summary>
    /// Fires the <see cref="StatusChanged"/> event.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    protected virtual void OnStatusChanged(MySqlService sender)
    {
      StatusChanged?.Invoke(this);
    }

    /// <summary>
    /// Fires the <see cref="StatusChangeError"/> event.
    /// </summary>
    /// <param name="ex">Exception thrown while trying to change the service's status.</param>
    protected virtual void OnStatusChangeError(Exception ex)
    {
      StatusChangeError?.Invoke(this, ex);
    }

    /// <summary>
    /// Changes the status of a service asynchronously.
    /// </summary>
    /// <param name="action">Action to perform on the service to change its status.</param>
    private void ChangeServiceStatus(int action)
    {
      var worker = new BackgroundWorker { WorkerSupportsCancellation = false, WorkerReportsProgress = false };
      worker.DoWork += WorkerDoWork;
      worker.RunWorkerCompleted += WorkerRunWorkerCompleted;
      worker.RunWorkerAsync(action);
    }

    /// <summary>
    /// Gets the corresponding WMI service instance and sets it in the <see cref="ServiceManagementObject"/> property.
    /// </summary>
    /// <param name="doNotFetchIfOffline">Flag indicating whether no attempt should be made to connect to the real host if the related machine is offline.</param>
    private void GetServiceInstance(bool doNotFetchIfOffline)
    {
      ServiceManagementObject = null;
      if (doNotFetchIfOffline && !Host.IsOnline)
      {
        return;
      }

      var retObjectCollection = Host.GetWmiServices(ServiceName, false, false);
      if (retObjectCollection == null
          || retObjectCollection.Count <= 0)
      {
        return;
      }

      foreach (var mo in retObjectCollection.Cast<ManagementObject>().Where(mo => mo != null))
      {
        ServiceManagementObject = mo;
        break;
      }
    }

    /// <summary>
    /// Starts, stops or restarts this service.
    /// </summary>
    /// <param name="action">Action to perform on the service.</param>
    private void ProcessStatusService(string action)
    {
      var winService = new ServiceController(ServiceName);
      var proc = new Process { StartInfo = { Verb = "runas", WindowStyle = ProcessWindowStyle.Hidden } };
      if (action == "restart")
      {
        proc.StartInfo.FileName = "cmd.exe";
        proc.StartInfo.Arguments = "/C net stop " + @"" + ServiceName + @"" + " && net start " + ServiceName + @"";
        proc.StartInfo.UseShellExecute = true;
        proc.Start();
        if (Host.IsLocal)
        {
          winService.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 30));
        }
      }
      else
      {
        proc.StartInfo.FileName = "sc";
        proc.StartInfo.Arguments = $@" {action} {ServiceName}";
        proc.StartInfo.UseShellExecute = true;
        proc.Start();
        if (Host.IsLocal)
        {
          winService.WaitForStatus(action == "start" ? ServiceControllerStatus.Running : ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 30));
        }
      }
    }

    /// <summary>
    /// Executes WMI command to stop the service
    /// </summary>
    private void RestartRemoteService()
    {
      _loops = 0;
      _isWaitingOnStatusChange = true;
      _statusChangeTimer.Elapsed += StatusChangeTimerElapsedToStop;
      _statusChangeTimer.Start();
      if (!ExecuteWmiMethod("StopService"))
      {
        return;
      }

      while (_isWaitingOnStatusChange)
      {
        SetServiceParameters(false);
        System.Threading.Thread.Sleep(2000);
      }

      _statusChangeTimer.Elapsed -= StatusChangeTimerElapsedToStop;
      if (Status != MySqlServiceStatus.Stopped
          && _loops >= 50)
      {
        throw new System.TimeoutException("Unable to stop service, the operation has timed out.");
      }

      _loops = 0;
      _isWaitingOnStatusChange = true;
      _statusChangeTimer.Elapsed += StatusChangeTimerElapsedToStart;
      _statusChangeTimer.Start();
      if (!ExecuteWmiMethod("StartService"))
      {
        return;
      }

      while (_isWaitingOnStatusChange)
      {
        SetServiceParameters(false);
        System.Threading.Thread.Sleep(2000);
      }

      _statusChangeTimer.Elapsed -= StatusChangeTimerElapsedToStart;
      if (Status != MySqlServiceStatus.Running
          && _loops >= 100)
      {
        throw new System.TimeoutException("Unable to restart service, the operation has timed out.");
      }
    }

    /// <summary>
    /// Executes the given WMI method on the service.
    /// </summary>
    /// <param name="methodName">WMI method name to execute.</param>
    /// <returns>true if the method executed successfully, false otherwise.</returns>
    private bool ExecuteWmiMethod(string methodName)
    {
      if (string.IsNullOrEmpty(methodName))
      {
        return false;
      }

      var serviceUnavailable = !Host.IsOnline
                               || ServiceManagementObject == null;
      Exception errorException = null;
      if (!serviceUnavailable)
      {
        try
        {
          ServiceManagementObject.InvokeMethod(methodName, null, null);
        }
        catch (Exception ex)
        {
          serviceUnavailable = true;
          errorException = ex;
          Logger.LogException(ex);
        }
      }

      if (!serviceUnavailable)
      {
        return true;
      }

      if (errorException == null)
      {
        errorException = new Exception(string.Format(Resources.ServiceActionErrorDetail, methodName));
      }

      OnStatusChangeError(errorException);
      return false;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="_statusChangeTimer"/> timer elapses.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void StatusChangeTimerElapsedToStart(object sender, ElapsedEventArgs e)
    {
      _loops++;
      if (Status != MySqlServiceStatus.Running && _loops < 50)
      {
        return;
      }

      _statusChangeTimer.Stop();
      _isWaitingOnStatusChange = false;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="_statusChangeTimer"/> timer elapses.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void StatusChangeTimerElapsedToStop(object sender, ElapsedEventArgs e)
    {
      _loops++;
      if (Status != MySqlServiceStatus.Stopped
          && _loops < 100)
      {
        return;
      }

      _statusChangeTimer.Stop();
      _isWaitingOnStatusChange = false;
    }

    /// <summary>
    /// Event delegate method that performs a service status change.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void WorkerDoWork(object sender, DoWorkEventArgs e)
    {
      if (!Service.ExistsServiceInstance(ServiceName))
      {
        throw new Exception(string.Format(Resources.BalloonTextServiceNotFound, ServiceName));
      }

      var action = (int)e.Argument;
      WorkCompleted = false;

      switch (action)
      {
        case 0:
          ProcessStatusService("stop");
          break;

        case 1:
          ProcessStatusService("start");
          break;

        case 2:
          ProcessStatusService("restart");
          break;
      }
    }

    /// <summary>
    /// Event delegate method that signals the service status change as complete.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void WorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
      {
        OnStatusChangeError(e.Error);
      }
      else
      {
        WorkCompleted = true;
      }
    }
  }
}