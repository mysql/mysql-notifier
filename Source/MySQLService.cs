//
// Copyright (c) 2012-2013, Oracle and/or its affiliates. All rights reserved.
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
//

namespace MySql.Notifier
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Diagnostics;
  using System.Linq;
  using System.Management;
  using System.ServiceProcess;
  using System.Timers;
  using System.Xml;
  using System.Xml.Serialization;
  using Microsoft.Win32;
  using MySql.Notifier.Properties;
  using MySQL.Utility;
  using MySQL.Utility.Forms;

  [Serializable]
  public class MySQLService : IDisposable
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
    /// The WMI instance for this service.
    /// </summary>
    private ManagementObject _managementObject;

    /// <summary>
    /// The current status of this service.
    /// </summary>
    private MySQLServiceStatus _currentStatus;

    /// <summary>
    /// Timer used to wait for a status change.
    /// </summary>
    private Timer _statusChangeTimer;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="MySQLService"/> class.
    /// </summary>
    public MySQLService()
    {
      _currentStatus = MySQLServiceStatus.Unavailable;
      _statusChangeTimer = new Timer(100);
      _displayName = string.Empty;
      _isWaitingOnStatusChange = false;
      _loops = 0;
      _managementObject = null;
      MenuGroup = null;
      PreviousStatus = MySQLServiceStatus.Unavailable;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySQLService"/> class.
    /// </summary>
    public MySQLService(string serviceName, bool notificationOnChange, bool updatesTrayIcon, Machine machine = null)
      : this()
    {
      Host = machine ?? new Machine();
      NotifyOnStatusChange = notificationOnChange;
      UpdateTrayIconOnStatusChange = updatesTrayIcon;
      ServiceName = serviceName;
      SetServiceParameters();
    }

    /// <summary>
    /// Releases all resources used by the <see cref="MySQLService"/> class
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases all resources used by the <see cref="MySQLService"/> class
    /// </summary>
    /// <param name="disposing">If true this is called by Dispose(), otherwise it is called by the finalizer</param>
    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        //// Free managed resources
        if (_statusChangeTimer != null)
        {
          _statusChangeTimer.Dispose();
        }

        if (_managementObject != null)
        {
          _managementObject.Dispose();
        }

        if (MenuGroup != null)
        {
          MenuGroup.Dispose();
        }
      }

      //// Add class finalizer if unmanaged resources are added to the class
      //// Free unmanaged resources if there are any
    }

    #region Events

    /// <summary>
    /// Handles the case where the service successfully moved to a new status.
    /// </summary>
    /// <param name="service">MySQLService instance.</param>
    public delegate void StatusChangedHandler(MySQLService service);

    /// <summary>
    /// Handles the case where the service failed to move to a proposed status.
    /// </summary>
    /// <param name="service">MySQLService instance.</param>
    /// <param name="ex">Exception thrown while trying to change the service's status.</param>
    public delegate void StatusChangeErrorHandler(MySQLService service, Exception ex);

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
    /// Gets or sets the display name of the service.
    /// </summary>
    [XmlAttribute(AttributeName = "DisplayName")]
    public string DisplayName
    {
      get
      {
        return _displayName;
      }

      set
      {
        if (!String.IsNullOrEmpty(value))
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
    /// Gets a flag indicating if this service is bound to a MySQL server service.
    /// </summary>
    [XmlIgnore]
    public bool IsRealMySQLService { get; private set; }

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
    public MySQLServiceStatus PreviousStatus { get; private set; }

    /// <summary>
    /// Gets a value indicating if the WMI instance bound to this service exists.
    /// </summary>
    [XmlIgnore]
    public bool ServiceInstanceExists
    {
      get
      {
        return !Host.IsOnline || ServiceManagementObject != null;
      }
    }

    /// <summary>
    /// Gets the WMI instance for this service.
    /// </summary>
    [XmlIgnore]
    public ManagementObject ServiceManagementObject
    {
      get
      {
        return _managementObject;
      }
    }

    /// <summary>
    /// Gets or sets the name of this service (short name).
    /// </summary>
    [XmlAttribute(AttributeName = "ServiceName")]
    public string ServiceName { get; set; }

    /// <summary>
    /// Gets the parameters used to initialize a MySQL server instance.
    /// </summary>
    [XmlIgnore]
    public MySQLStartupParameters StartupParameters { get; private set; }

    /// <summary>
    /// Gets the current status of this service.
    /// </summary>
    [XmlIgnore]
    public MySQLServiceStatus Status
    {
      get
      {
        return _currentStatus;
      }

      private set
      {
        MySQLServiceStatus oldStatus = _currentStatus;
        _currentStatus = value;
        if (_currentStatus != oldStatus)
        {
          PreviousStatus = oldStatus;
          OnStatusChanged(this);
        }
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
    public List<MySqlWorkbenchConnection> WorkbenchConnections { get; private set; }

    /// <summary>
    /// Gets a value indicating if the service is done with a service status change operation.
    /// </summary>
    [XmlIgnore]
    public bool WorkCompleted { get; private set; }

    #endregion Properties

    /// <summary>
    /// Finds the related Workbench connections that connect to this MySQL service, only for services running on local computers.
    /// </summary>
    public void FindMatchingWBConnections()
    {
      if (!Host.IsLocal || ServiceManagementObject == null)
      {
        return;
      }

      //// Discover what StartupParameters we were started with for local connections
      MySQLStartupParameters StartupParameters = GetStartupParameters();
      if (String.IsNullOrEmpty(StartupParameters.HostName)) return;
      WorkbenchConnections = new List<MySqlWorkbenchConnection>();

      if (!IsRealMySQLService) return;

      var filteredConnections = MySqlWorkbench.WorkbenchConnections.Where(t => !String.IsNullOrEmpty(t.Name) && t.Port == StartupParameters.Port);

      if (filteredConnections != null)
      {
        foreach (MySqlWorkbenchConnection c in filteredConnections)
        {
          switch (c.DriverType)
          {
            case MySqlWorkbenchConnectionType.NamedPipes:
              if (!StartupParameters.NamedPipesEnabled || string.Compare(c.Socket, StartupParameters.PipeName, true) != 0)
              {
                continue;
              }
              break;

            case MySqlWorkbenchConnectionType.Tcp:
              if (c.Port != StartupParameters.Port)
              {
                continue;
              }
              break;

            case MySqlWorkbenchConnectionType.Ssh:
            case MySqlWorkbenchConnectionType.Unknown:
              continue;
          }

          if (!Utility.IsValidIpAddress(c.Host))
          {
            if (Utility.GetIPv4ForHostName(c.Host) != StartupParameters.HostIPv4)
            {
              continue;
            }
          }
          else
          {
            if (c.Host != StartupParameters.HostIPv4)
            {
              continue;
            }
          }

          WorkbenchConnections.Add(c);
        }
      }
    }

    /// <summary>
    /// Attempts to stop and then start the current MySQL Service
    /// </summary>
    /// <returns>Flag indicating if the action completed succesfully</returns>
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
    public void SetServiceParameters()
    {
      GetServiceInstance();
      try
      {
        FindMatchingWBConnections();
        SetStatus(ServiceManagementObject == null ? Status.ToString() : ServiceManagementObject.Properties["State"].Value.ToString());
        DisplayName = (String.IsNullOrEmpty(DisplayName) && ServiceManagementObject != null) ? ServiceManagementObject.Properties["DisplayName"].Value.ToString() : DisplayName;
        MenuGroup = new ServiceMenuGroup(this);
      }
      catch (InvalidOperationException ioEx)
      {
        InfoDialog.ShowErrorDialog(Resources.HighSeverityError, ioEx.Message);
        MySQLSourceTrace.WriteAppErrorToLog(ioEx);
        _managementObject = null;
      }
    }

    /// <summary>
    /// Sets a new status for this service given the text of the new status.
    /// </summary>
    /// <param name="statusString">Text of the new status.</param>
    public void SetStatus(string statusString)
    {
      MySQLServiceStatus newStatus;
      bool parsed = MySQLServiceStatus.TryParse(statusString, out newStatus);
      if (parsed)
      {
        Status = newStatus;
      }
    }

    /// <summary>
    /// Attempts to start the current MySQL Service
    /// </summary>
    /// <returns>Flag indicating if the action completed succesfully</returns>
    public void Start()
    {
      if (Host.IsLocal)
      {
        ChangeServiceStatus(1);
      }
      else
      {
        StartRemoteService();
      }
    }

    /// <summary>
    /// Attempts to stop the current MySQL Service
    /// </summary>
    /// <returns>Flag indicating if the action completed succesfully</returns>
    public void Stop()
    {
      if (Host.IsLocal)
      {
        ChangeServiceStatus(0);
      }
      else
      {
        StopRemoteService();
      }
    }

    /// <summary>
    /// Fires the <see cref="StatusChanged"/> event.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    protected virtual void OnStatusChanged(MySQLService sender)
    {
      if (this.StatusChanged != null)
      {
        this.StatusChanged(this);
      }
    }

    /// <summary>
    /// Fires the <see cref="StatusChangeError"/> event.
    /// </summary>
    /// <param name="ex">Exception thrown while trying to change the service's status.</param>
    protected virtual void OnStatusChangeError(Exception ex)
    {
      if (StatusChangeError != null)
      {
        StatusChangeError(this, ex);
      }
    }

    /// <summary>
    /// Changes the status of a service asynchronously.
    /// </summary>
    /// <param name="action">Action to perform on the service to change its status.</param>
    private void ChangeServiceStatus(int action)
    {
      BackgroundWorker worker = new BackgroundWorker();
      worker.WorkerSupportsCancellation = false;
      worker.WorkerReportsProgress = false;
      worker.DoWork += new DoWorkEventHandler(worker_DoWork);
      worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
      worker.RunWorkerAsync(action);
    }

    /// <summary>
    /// Gets the corresponding WMI service instance and sets it in the <see cref="ServiceManagementObject"/> property.
    /// </summary>
    private void GetServiceInstance()
    {
      _managementObject = null;
      ManagementObjectCollection retObjectCollection = Host.GetWMIServices(ServiceName, false);
      if (retObjectCollection != null && retObjectCollection.Count > 0)
      {
        foreach (ManagementObject mo in retObjectCollection)
        {
          if (mo != null)
          {
            _managementObject = mo;
            break;
          }
        }
      }
    }

    /// <summary>
    /// Gets the parameters used to initialize a MySQL server instance.
    /// </summary>
    /// <returns></returns>
    private MySQLStartupParameters GetStartupParameters()
    {
      MySQLStartupParameters parameters = new MySQLStartupParameters();
      parameters.PipeName = "mysql";

      //// Get our host information
      parameters.HostName = Host.Name == "." ? MySqlWorkbenchConnection.DEFAULT_HOSTNAME : Host.Name;
      parameters.HostIPv4 = Utility.GetIPv4ForHostName(parameters.HostName);

      RegistryKey key = Registry.LocalMachine.OpenSubKey(String.Format(@"SYSTEM\CurrentControlSet\Services\{0}", ServiceName));
      string imagepath = (string)key.GetValue("ImagePath", null);
      key.Close();
      if (imagepath == null)
      {
        return parameters;
      }

      string[] args = Utility.SplitArgs(imagepath);
      IsRealMySQLService = args[0].EndsWith("mysqld.exe") || args[0].EndsWith("mysqld-nt.exe") || args[0].EndsWith("mysqld") || args[0].EndsWith("mysqld-nt");

      //// Parse our command line args
      Mono.Options.OptionSet p = new Mono.Options.OptionSet()
        .Add("defaults-file=", "", v => parameters.DefaultsFile = v)
        .Add("port=|P=", "", v => Int32.TryParse(v, out parameters.Port))
        .Add("enable-named-pipe", v => parameters.NamedPipesEnabled = true)
        .Add("socket=", "", v => parameters.PipeName = v);
      p.Parse(args);
      if (parameters.DefaultsFile == null)
      {
        return parameters;
      }

      //// We have a valid defaults file
      IniFile f = new IniFile(parameters.DefaultsFile);
      Int32.TryParse(f.ReadValue("mysqld", "port", parameters.Port.ToString()), out parameters.Port);
      parameters.PipeName = f.ReadValue("mysqld", "socket", parameters.PipeName);

      //// Now see if named pipes are enabled
      parameters.NamedPipesEnabled = parameters.NamedPipesEnabled || f.HasKey("mysqld", "enable-named-pipe");
      return parameters;
    }

    /// <summary>
    /// Starts, stops or restarts this service.
    /// </summary>
    /// <param name="action">Action to perform on the service.</param>
    private void ProcessStatusService(string action)
    {
      ServiceController winService = new ServiceController(ServiceName);
      Process proc = new Process();
      proc.StartInfo.Verb = "runas";
      proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

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
        proc.StartInfo.Arguments = string.Format(@" {0} {1}", action, ServiceName);
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
      _statusChangeTimer.Elapsed += new ElapsedEventHandler(statusChangeTimer_ElapsedToStop);
      _statusChangeTimer.Start();
      StopRemoteService();

      while (_isWaitingOnStatusChange)
      {
        SetServiceParameters();
        System.Threading.Thread.Sleep(2000);
      }

      _statusChangeTimer.Elapsed -= new ElapsedEventHandler(statusChangeTimer_ElapsedToStop);

      if (Status != MySQLServiceStatus.Stopped && _loops >= 50)
      {
        throw new System.TimeoutException("Unable to stop service, the operation has timed out.");
      }

      _loops = 0;
      _isWaitingOnStatusChange = true;
      _statusChangeTimer.Elapsed += new ElapsedEventHandler(statusChangeTimer_ElapsedToStart);
      _statusChangeTimer.Start();
      StartRemoteService();

      while (_isWaitingOnStatusChange)
      {
        SetServiceParameters();
        System.Threading.Thread.Sleep(2000);
      }

      _statusChangeTimer.Elapsed -= new ElapsedEventHandler(statusChangeTimer_ElapsedToStart);

      if (Status != MySQLServiceStatus.Running && _loops >= 100)
      {
        throw new System.TimeoutException("Unable to restart service, the operation has timed out.");
      }
    }

    /// <summary>
    /// Executes WMI command to start the service
    /// </summary>
    private void StartRemoteService()
    {
      ServiceManagementObject.InvokeMethod("StartService", null, null);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="statusChangeTimer"/> timer elapses.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void statusChangeTimer_ElapsedToStart(object sender, ElapsedEventArgs e)
    {
      _loops++;
      if (Status == MySQLServiceStatus.Running || _loops >= 50)
      {
        _statusChangeTimer.Stop();
        _isWaitingOnStatusChange = false;
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="statusChangeTimer"/> timer elapses.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void statusChangeTimer_ElapsedToStop(object sender, ElapsedEventArgs e)
    {
      _loops++;
      if (Status == MySQLServiceStatus.Stopped || _loops >= 100)
      {
        _statusChangeTimer.Stop();
        _isWaitingOnStatusChange = false;
      }
    }

    /// <summary>
    /// Executes WMI command to stop the service
    /// </summary>
    private void StopRemoteService()
    {
      ServiceManagementObject.InvokeMethod("StopService", null, null);
    }

    /// <summary>
    /// Event delegate method that performs a service status change.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void worker_DoWork(object sender, DoWorkEventArgs e)
    {
      if (!Service.ExistsServiceInstance(ServiceName))
      {
        throw new Exception(String.Format(Resources.BaloonTextServiceNotFound, ServiceName));
      }

      int action = (int)e.Argument;
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
    private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

  /// <summary>
  /// Indicates the current state of the service.
  /// </summary>
  public enum MySQLServiceStatus
  {
    /// <summary>
    /// The service is not reachable. Verify WMI connection with the host.
    /// </summary>
    Unavailable = 0,

    /// <summary>
    /// The service is not running. This corresponds to the Win32 SERVICE_STOPPED constant, which is defined as 0x00000001.
    /// </summary>
    Stopped = 1,

    /// <summary>
    /// The service is starting. This corresponds to the Win32 SERVICE_START_PENDING constant, which is defined as 0x00000002.
    /// </summary>
    StartPending = 2,

    /// <summary>
    /// The service is stopping. This corresponds to the Win32 SERVICE_STOP_PENDING constant, which is defined as 0x00000003.
    /// </summary>
    StopPending = 3,

    /// <summary>
    /// The service is running. This corresponds to the Win32 SERVICE_RUNNING constant, which is defined as 0x00000004.
    /// </summary>
    Running = 4,

    /// <summary>
    /// The service continue is pending. This corresponds to the Win32 SERVICE_CONTINUE_PENDING constant, which is defined as 0x00000005.
    /// </summary>
    ContinuePending = 5,

    /// <summary>
    /// The service pause is pending. This corresponds to the Win32 SERVICE_PAUSE_PENDING constant, which is defined as 0x00000006.
    /// </summary>
    PausePending = 6,

    /// <summary>
    /// The service is paused. This corresponds to the Win32 SERVICE_PAUSED constant, which is defined as 0x00000007.
    /// </summary>
    Paused = 7,
  }

  /// <summary>
  /// Represents StartupParameters used to initialize a MySQL server instance.
  /// </summary>
  public struct MySQLStartupParameters
  {
    public string DefaultsFile;
    public string HostIPv4;
    public string HostName;
    public bool NamedPipesEnabled;
    public string PipeName;
    public int Port;
  }
}