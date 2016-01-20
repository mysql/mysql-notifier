// Copyright (c) 2013, 2016, Oracle and/or its affiliates. All rights reserved.
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
using System.Drawing;
using System.Linq;
using System.Management;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using MySql.Notifier.Enumerations;
using MySql.Notifier.Properties;
using MySQL.Utility.Classes;
using MySQL.Utility.Classes.MySQLWorkbench;
using MySQL.Utility.Forms;

namespace MySql.Notifier.Classes
{
  /// <summary>
  /// Contains all the information required to connect to a remote host, plus a list of of the services that will be monitored by Notifier inside it and logic to manage them.
  /// </summary>
  [Serializable]
  public class Machine : IDisposable
  {
    #region Constants

    /// <summary>
    /// Default interval for an automatic connection test.
    /// </summary>
    public const ushort DEFAULT_AUTO_TEST_CONNECTION_INTERVAL = 10;

    /// <summary>
    /// Default waiting time in milliseconds to wait for an async cancellation before disposing an object.
    /// </summary>
    public const ushort DEFAULT_CANCEL_ASYNC_WAIT = 20000;

    /// <summary>
    /// Default waiting time in milliseconds for each step of the async cancellation waiting time.
    /// </summary>
    public const ushort DEFAULT_CANCEL_ASYNC_STEP = 1000;

    /// <summary>
    /// Default interval unit of measure, set to minutes by default.
    /// </summary>
    public const TimeUtilities.IntervalUnitOfMeasure DEFAULT_AUTO_TEST_CONNECTION_UOM = TimeUtilities.IntervalUnitOfMeasure.Minutes;

    /// <summary>
    /// Represents the WMI namespace for a local computer.
    /// </summary>
    public const string WMI_LOCAL_NAMESPACE = @"root\cimv2";

    /// <summary>
    /// WMI query to retrieve all services.
    /// </summary>
    public const string WMI_QUERY_SELECT_ALL = "SELECT * FROM Win32_Service";

    /// <summary>
    /// WMI query to retrieve all services where their display name matches a given filter.
    /// </summary>
    public const string WMI_QUERY_SELECT_DISPLAY_NAME_CONTAINING = "SELECT * FROM Win32_Service WHERE DisplayName LIKE '%{0}%'";

    /// <summary>
    /// WMI query to retrieve all services where their name matches a given filter.
    /// </summary>
    public const string WMI_QUERY_SELECT_NAME_CONTAINING = "SELECT * FROM Win32_Service WHERE Name LIKE '%{0}%'";

    /// <summary>
    /// Represents the WMI namespace for a remote computer containing a placeholder for the remote machine name.
    /// </summary>
    public const string WMI_REMOTE_NAMESPACE = @"\\{0}\root\cimv2";

    #endregion Constants

    #region Fields

    /// <summary>
    /// The interval to the next automatic connection test to see if a machine connection status changed.
    /// </summary>
    private uint _autoTestConnectionInterval;

    /// <summary>
    /// The unit of measure used for this machine auotmatic connection test.
    /// </summary>
    private TimeUtilities.IntervalUnitOfMeasure _autoTestConnectionIntervalUnitOfMeasure;

    /// <summary>
    /// The current status of this machine.
    /// </summary>
    private ConnectionStatusType _connectionStatus;

    /// <summary>
    /// Flag indicating if the asynchronous connection test was cancelled.
    /// </summary>
    private bool _connectionTestCancelled;

    /// <summary>
    /// The host name for this machine.
    /// </summary>
    private string _name;

    /// <summary>
    /// The seconds remaining to the next automatic connection test to see if the machine's connection status changed.
    /// </summary>
    private double _secondsToAutoTestConnection;

    /// <summary>
    /// Contains the settings to perform a WMI connection.
    /// </summary>
    private ConnectionOptions _wmiConnectionOptions;

    /// <summary>
    /// Scope (namespace) used for WMI operations.
    /// </summary>
    private ManagementScope _wmiManagementScope;

    /// <summary>
    /// WMI watcher for creation, deletion and status changes of services related to this machine.
    /// </summary>
    private ServiceWatcher _wmiServicesWatcher;

    /// <summary>
    /// Background worker that performs an asynchronous connection test.
    /// </summary>
    private BackgroundWorker _worker;

    /// <summary>
    /// Indicates whether workbench is installed at the time the machine is created in memory.
    /// </summary>
    private bool _workbenchWasInstalled;

    #endregion Fields

    /// <summary>
    /// DO NOT REMOVE. Default constructor required for serialization-deserialization.
    /// </summary>
    public Machine()
    {
      _autoTestConnectionInterval = DEFAULT_AUTO_TEST_CONNECTION_INTERVAL;
      _autoTestConnectionIntervalUnitOfMeasure = DEFAULT_AUTO_TEST_CONNECTION_UOM;
      _name = MySqlWorkbenchConnection.DEFAULT_HOSTNAME;
      _connectionStatus = ConnectionStatusType.Unknown;
      _connectionTestCancelled = false;
      _wmiConnectionOptions = null;
      _wmiManagementScope = null;
      _wmiServicesWatcher = null;
      _worker = null;
      ConnectionTestInProgress = false;
      ConnectionProblem = ConnectionProblemType.None;
      InitialLoadDone = false;
      MachineId = Guid.NewGuid().ToString("B");
      MenuGroup = null;
      User = string.Empty;
      OldConnectionStatus = ConnectionStatusType.Unknown;
      Password = string.Empty;
      RefreshingStatus = false;
      SecondsToAutoTestConnection = AutoTestConnectionIntervalInSeconds;
      Services = new List<MySqlService>();
      UseAsynchronousWmi = true;
      WmiQueriesTimeoutInSeconds = 5;
      if (IsLocal)
      {
        _workbenchWasInstalled = MySqlWorkbench.IsInstalled;
      }
    }

    /// <summary>
    /// Constructor designed for remote machines.
    /// </summary>
    /// <param name="name">Host name.</param>
    /// <param name="user">User name.</param>
    /// <param name="password">Password.</param>
    public Machine(string name, string user, string password)
      : this()
    {
      _name = MySqlWorkbenchConnection.IsHostLocal(name) ? name : name.ToUpper();
      _connectionStatus = IsLocal ? ConnectionStatusType.Online : ConnectionStatusType.Unknown;
      User = user.ToUpper();
      Password = MySqlSecurity.EncryptPassword(password);
    }

    #region Enumerations

    /// <summary>
    /// Specifies the connection problem found when the status of a connection is unavailable.
    /// </summary>
    public enum ConnectionProblemType
    {
      /// <summary>
      /// Problem may be caused by an incorrect user or password.
      /// </summary>
      IncorrectUserOrPassword,

      /// <summary>
      /// Problem may be caused by insufficient WMI access permissions on the local or remote computer.
      /// </summary>
      InsufficientAccessPermissions,

      /// <summary>
      /// No problem found, machine status is online and WMI communication possible.
      /// </summary>
      None
    }

    /// <summary>
    /// Specifies the machine status.
    /// </summary>
    public enum ConnectionStatusType
    {
      /// <summary>
      /// Trying to connect to the machine to verify its status.
      /// </summary>
      Connecting,

      /// <summary>
      /// Machine is online and accepting connections.
      /// </summary>
      Online,

      /// <summary>
      /// Machine is offline or unavailable (network or firewall misconfiguration, etc.)
      /// </summary>
      Unavailable,

      /// <summary>
      /// No status yet, this is the default when the machine is created.
      /// </summary>
      Unknown
    }

    /// <summary>
    /// Specifies the location type for this computer.
    /// </summary>
    public enum LocationType
    {
      /// <summary>
      /// Machine is the local computer (localhost or 127.0.0.1)
      /// </summary>
      Local,

      /// <summary>
      /// Machine is a remote computer.
      /// </summary>
      Remote
    }

    #endregion Enumerations

    #region Events

    /// <summary>
    /// This event system handles the case where the remote machine is unavailable, and a service has failed to connect to the host.
    /// </summary>
    /// <param name="sender">Machine instance.</param>
    /// <param name="oldConnectionStatus">Old connection status.</param>
    public delegate void MachineStatusChangedHandler(Machine sender, ConnectionStatusType oldConnectionStatus);

    /// <summary>
    /// Event handler for changes on current machine services list.
    /// </summary>
    /// <param name="machine">Machine instance.</param>
    /// <param name="service">MySQLService instance.</param>
    /// <param name="listChangeType">Service list change type.</param>
    public delegate void ServiceListChangedHandler(Machine machine, MySqlService service, ListChangeType listChangeType);

    /// <summary>
    /// Notifies that the status of one of the services in the list has changed.
    /// </summary>
    /// <param name="machine">Machine instance.</param>
    /// <param name="service">MySQLService instance.</param>
    public delegate void ServiceStatusChangedHandler(Machine machine, MySqlService service);

    /// <summary>
    /// This event system handles the case where the remote machine is unavailable, and a service has failed to connect to the host.
    /// </summary>
    /// <param name="machine">Machine instance.</param>
    /// <param name="service">MySQLService instance.</param>
    /// <param name="ex">Exception thrown while trying to change the service's status.</param>
    public delegate void ServiceStatusChangeErrorHandler(Machine machine, MySqlService service, Exception ex);

    /// <summary>
    /// This event system handles the case where Workbench was installed or uninstalled on the machine.
    /// </summary>
    public delegate void WorkbenchInstallationChangedHandler(ManagementBaseObject remoteService);

    /// <summary>
    /// Occurs when the machine status changes.
    /// </summary>
    public event MachineStatusChangedHandler MachineStatusChanged;

    /// <summary>
    /// Occurs when services are added or removed from the list of services.
    /// </summary>
    public event ServiceListChangedHandler ServiceListChanged;

    /// <summary>
    /// Occurs when a service in the services list has a status change.
    /// </summary>
    public event ServiceStatusChangedHandler ServiceStatusChanged;

    /// <summary>
    /// Occurs when an error is thrown while attempting to change the status of a service in the services list.
    /// </summary>
    public event ServiceStatusChangeErrorHandler ServiceStatusChangeError;

    /// <summary>
    /// Occurs when Workbench was installed or uninstall on the local computer.
    /// </summary>
    public event WorkbenchInstallationChangedHandler WorkbenchInstallationChanged;

    #endregion Events

    #region Properties

    /// <summary>
    /// Gets or sets the interval to the next automatic connection test to see if a machine connection status changed.
    /// </summary>
    [XmlAttribute(AttributeName = "AutoTestConnectionInterval")]
    public uint AutoTestConnectionInterval
    {
      get
      {
        return _autoTestConnectionInterval;
      }

      set
      {
        uint lastValue = _autoTestConnectionInterval;
        _autoTestConnectionInterval = value;
        if (lastValue != value)
        {
          SecondsToAutoTestConnection = AutoTestConnectionIntervalInSeconds;
        }
      }
    }

    /// <summary>
    /// Gets the interval in seconds to the next automatic connection test to see if a machine connection status changed.
    /// </summary>
    [XmlIgnore]
    public double AutoTestConnectionIntervalInSeconds
    {
      get
      {
        return TimeUtilities.ConvertToSeconds(_autoTestConnectionIntervalUnitOfMeasure, _autoTestConnectionInterval);
      }
    }

    /// <summary>
    /// Gets or sets the unit of measure used for this machine auotmatic connection test.
    /// </summary>
    [XmlAttribute(AttributeName = "AutoTestConnectionIntervalUnitOfMeasure")]
    public TimeUtilities.IntervalUnitOfMeasure AutoTestConnectionIntervalUnitOfMeasure
    {
      get
      {
        return _autoTestConnectionIntervalUnitOfMeasure;
      }

      set
      {
        TimeUtilities.IntervalUnitOfMeasure lastValue = _autoTestConnectionIntervalUnitOfMeasure;
        _autoTestConnectionIntervalUnitOfMeasure = value;
        if (lastValue != value)
        {
          SecondsToAutoTestConnection = AutoTestConnectionIntervalInSeconds;
        }
      }
    }

    /// <summary>
    /// Gets the current status of this machine, refreshed by calling the <see cref="TestConnection"/> or the <see cref="o:GetWmiServices"/> method.
    /// </summary>
    [XmlIgnore]
    public ConnectionProblemType ConnectionProblem { get; private set; }

    /// <summary>
    /// Gets a long description about the current status of this machine, refreshed by calling the <see cref="TestConnection"/> or the <see cref="o:GetWmiServices"/> method.
    /// </summary>
    [XmlIgnore]
    public string ConnectionProblemLongDescription
    {
      get
      {
        switch (ConnectionProblem)
        {
          case ConnectionProblemType.IncorrectUserOrPassword:
            return Resources.AccessDeniedMessage;

          case ConnectionProblemType.InsufficientAccessPermissions:
            return Resources.MachineUnavailableMessage;

          default:
            return string.Empty;
        }
      }
    }

    /// <summary>
    /// Gets a short description about the current status of this machine, refreshed by calling the <see cref="TestConnection"/> or the <see cref="o:GetWmiServices"/> method.
    /// </summary>
    [XmlIgnore]
    public string ConnectionProblemShortDescription
    {
      get
      {
        switch (ConnectionProblem)
        {
          case ConnectionProblemType.IncorrectUserOrPassword:
            return Resources.AccessDeniedTitle;

          case ConnectionProblemType.InsufficientAccessPermissions:
            return Resources.MachineUnavailableTitle;

          default:
            return string.Empty;
        }
      }
    }

    /// <summary>
    /// Gets the current status of this machine, refreshed by calling the <see cref="TestConnection"/> or the <see cref="o:GetWmiServices"/> method.
    /// </summary>
    [XmlIgnore]
    public ConnectionStatusType ConnectionStatus
    {
      get
      {
        return IsLocal ? ConnectionStatusType.Online : _connectionStatus;
      }

      private set
      {
        ConnectionStatusType oldConnectionStatus = _connectionStatus;
        _connectionStatus = value;
        if (oldConnectionStatus == _connectionStatus)
        {
          return;
        }

        if (oldConnectionStatus != ConnectionStatusType.Connecting && oldConnectionStatus != ConnectionStatusType.Unknown)
        {
          OldConnectionStatus = oldConnectionStatus;
        }

        OnMachineStatusChanged(oldConnectionStatus);
      }
    }

    /// <summary>
    /// Gets a value indicating whether a connection test is still ongoing.
    /// </summary>
    [XmlIgnore]
    public bool ConnectionTestInProgress { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the machine has at least one service in its monitoring list.
    /// </summary>
    [XmlIgnore]
    public bool HasServices
    {
      get
      {
        return Services != null && Services.Count > 0;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the initial load of the machine has been done.
    /// </summary>
    [XmlIgnore]
    public bool InitialLoadDone { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the machine is a local host.
    /// </summary>
    [XmlIgnore]
    public bool IsLocal
    {
      get
      {
        return Location == LocationType.Local;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the machine status is online.
    /// </summary>
    [XmlIgnore]
    public bool IsOnline
    {
      get
      {
        return IsLocal || ConnectionStatus == ConnectionStatusType.Online;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the machine is a local host.
    /// </summary>
    [XmlIgnore]
    public LocationType Location
    {
      get
      {
        return MySqlWorkbenchConnection.IsHostLocal(Name) ? LocationType.Local : LocationType.Remote;
      }
    }

    /// <summary>
    /// Gets or sets a unique ID for this machine.
    /// </summary>
    [XmlAttribute("MachineId")]
    public string MachineId { get; set; }

    /// <summary>
    /// Gets the ToolStripMenuItem control corresponding to the machine.
    /// </summary>
    [XmlIgnore]
    public ToolStripMenuItem MenuGroup { get; private set; }

    /// <summary>
    /// Gets or sets the host name for this machine.
    /// </summary>
    [XmlAttribute("Host")]
    public string Name
    {
      get
      {
        return _name;
      }

      set
      {
        string oldName = _name;
        _name = value;
        if (string.Compare(oldName, _name, StringComparison.OrdinalIgnoreCase) != 0)
        {
          ConnectionStatus = IsLocal ? ConnectionStatusType.Online : ConnectionStatusType.Unknown;
        }
      }
    }

    /// <summary>
    /// Gets the previous connection status of the machine.
    /// </summary>
    [XmlIgnore]
    public ConnectionStatusType OldConnectionStatus { get; private set; }

    /// <summary>
    /// Gets or sets the password as an encrypted string for security purposes.
    /// </summary>
    [XmlAttribute("Password")]
    public string Password { get; set; }

    /// <summary>
    /// Gets a value indicating whether the machine is in the process of refreshing its status and its services statuses.
    /// </summary>
    [XmlIgnore]
    public bool RefreshingStatus { get; private set; }

    /// <summary>
    /// Gets or sets the seconds remaining to the next automatic connection test to see if a machine connection status changed.
    /// </summary>
    [XmlIgnore]
    public double SecondsToAutoTestConnection
    {
      get
      {
        return _secondsToAutoTestConnection;
      }

      set
      {
        _secondsToAutoTestConnection = value;
        if (_secondsToAutoTestConnection <= 0)
        {
          if (ConnectionStatus != ConnectionStatusType.Connecting)
          {
            TestConnection(false, true);
          }
        }
      }
    }

    /// <summary>
    /// Gets or sets the list of services associated with this machine.
    /// </summary>
    [XmlElement(ElementName = "ServicesList", Type = typeof(List<MySqlService>))]
    public List<MySqlService> Services { get; set; }

    /// <summary>
    /// Gets the password as an unencrypted string.
    /// </summary>
    [XmlIgnore]
    public string UnprotectedPassword
    {
      get
      {
        return string.IsNullOrEmpty(Password) ? string.Empty : MySqlSecurity.DecryptPassword(Password);
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether asynchronous or synchronous WMI watchers are used by the machine.
    /// </summary>
    [XmlIgnore]
    public bool UseAsynchronousWmi { get; set; }

    /// <summary>
    /// Gets or sets the name of the user to connect to this machine.
    /// </summary>
    [XmlAttribute("User")]
    public string User { get; set; }

    /// <summary>
    /// Gets or sets the timeout in seconds for WMI queries.
    /// </summary>
    [XmlAttribute("WMIQueriesTimeoutInSeconds")]
    public ushort WmiQueriesTimeoutInSeconds { get; set; }

    /// <summary>
    /// Gets an object that contains the settings to perform a WMI connection.
    /// </summary>
    private ConnectionOptions WmiConnectionOptions
    {
      get
      {
        if (_wmiConnectionOptions == null)
        {
          _wmiConnectionOptions = new ConnectionOptions();
          _wmiConnectionOptions.Impersonation = ImpersonationLevel.Impersonate;
          _wmiConnectionOptions.Authentication = AuthenticationLevel.Packet;
          _wmiConnectionOptions.EnablePrivileges = true;
          _wmiConnectionOptions.Context = null;
          _wmiConnectionOptions.Timeout = TimeSpan.FromSeconds(30);
        }

        _wmiConnectionOptions.Username = User;
        _wmiConnectionOptions.Password = MySqlSecurity.DecryptPassword(Password);
        return _wmiConnectionOptions;
      }
    }

    /// <summary>
    /// Gets the scope (namespace) used for WMI operations.
    /// </summary>
    private ManagementScope WmiManagementScope
    {
      get
      {
        if (_wmiManagementScope == null)
        {
          _wmiManagementScope = new ManagementScope();
        }

        if (IsLocal)
        {
          if (_wmiManagementScope.Path.NamespacePath != WMI_LOCAL_NAMESPACE)
          {
            _wmiManagementScope = new ManagementScope(WMI_LOCAL_NAMESPACE);
          }
        }
        else
        {
          if (_wmiManagementScope.Path.Server != Name || _wmiManagementScope.Options != WmiConnectionOptions)
          {
            _wmiManagementScope = new ManagementScope(string.Format(WMI_REMOTE_NAMESPACE, Name), WmiConnectionOptions);
          }
        }

        return _wmiManagementScope;
      }
    }

    #endregion Properties

    /// <summary>
    /// Cancels the asynchronous connection test.
    /// </summary>
    /// <returns>true if the background connection test was cancelled, false otherwise</returns>
    public void CancelAsynchronousConnectionTest()
    {
      if (_worker == null || !_worker.WorkerSupportsCancellation || (!ConnectionTestInProgress && !_worker.IsBusy))
      {
        return;
      }

      _connectionTestCancelled = true;
      _worker.CancelAsync();
    }

    /// <summary>
    /// Makes changes (addition, removal, update) to a MySQL service on the current machine.
    /// </summary>
    /// <param name="service">The MySQL service involved on the change.</param>
    /// <param name="listChangeType">Change type (addition, removal, update) related to a MySQL service.</param>
    public void ChangeService(MySqlService service, ListChangeType listChangeType)
    {
      switch (listChangeType)
      {
        case ListChangeType.AddByUser:
        case ListChangeType.AddByLoad:
        case ListChangeType.AutoAdd:
          if (listChangeType == ListChangeType.AutoAdd || (listChangeType == ListChangeType.AddByUser && GetServiceByName(service.ServiceName) == null))
          {
            service.NotifyOnStatusChange = Settings.Default.NotifyOfStatusChange;
            service.UpdateTrayIconOnStatusChange = true;
            Services.Add(service);
            OnServiceListChanged(service, listChangeType);
            if (IsLocal && Services.Count == 1 && !InitialLoadDone)
            {
              InitialLoadDone = !Settings.Default.FirstRun;
            }
          }

          LoadServiceParameters(service, listChangeType);
          break;

        case ListChangeType.Cleared:
        case ListChangeType.RemoveByUser:
        case ListChangeType.RemoveByEvent:
          Services.Remove(service);
          OnServiceListChanged(service, listChangeType);
          break;

        case ListChangeType.Updated:
          LoadServiceParameters(service, listChangeType);
          OnServiceListChanged(service, listChangeType);
          break;
      }
    }

    /// <summary>
    /// Used to see if service is already on the list.
    /// </summary>
    /// <param name="service">MySQLService instance to look for.</param>
    /// <returns>True if current machine contains it already.</returns>
    public bool ContainsService(MySqlService service)
    {
      if (Services == null || Services.Count == 0)
      {
        return false;
      }

      return GetServiceByName(service.ServiceName) != null;
    }

    /// <summary>
    /// Releases all resources used by the <see cref="Machine"/> class
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases all resources used by the <see cref="Machine"/> class
    /// </summary>
    /// <param name="disposing">If true this is called by Dispose(), otherwise it is called by the finalizer</param>
    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        // Free managed resources
        if (_wmiServicesWatcher != null)
        {
          _wmiServicesWatcher.Dispose();
        }

        if (_worker != null)
        {
          if (_worker.IsBusy)
          {
            _worker.CancelAsync();
            ushort cancelAsyncWait = 0;
            while (_worker.IsBusy && cancelAsyncWait < DEFAULT_CANCEL_ASYNC_WAIT)
            {
              Thread.Sleep(DEFAULT_CANCEL_ASYNC_STEP);
              cancelAsyncWait += DEFAULT_CANCEL_ASYNC_STEP;
            }
          }

          _worker.DoWork -= TestConnectionWorkerDoWork;
          _worker.RunWorkerCompleted -= TestConnectionWorkerCompleted;
          _worker.Dispose();
        }

        if (Services != null)
        {
          foreach (MySqlService service in Services.Where(service => service != null))
          {
            service.Dispose();
          }
        }
      }

      // Add class finalizer if unmanaged resources are added to the class
      // Free unmanaged resources if there are any
    }

    /// <summary>
    /// Returns an instance of a service if is already on the list, searching by name
    /// </summary>
    /// <param name="name">MySQLService instance name</param>
    /// <returns>MySQLService instance</returns>
    public MySqlService GetServiceByName(string name)
    {
      return Services.FirstOrDefault(service => string.Compare(service.ServiceName, name, StringComparison.OrdinalIgnoreCase) == 0);
    }

    /// <summary>
    /// Gets a list of services corresponding to this machine.
    /// </summary>
    /// <param name="displayMessageOnError">Flag indicating if an error message is displayed to users indicating the connection error.</param>
    /// <returns>A collection of management objects.</returns>
    public ManagementObjectCollection GetWmiServices(bool displayMessageOnError)
    {
      return GetWmiServices(null, false, displayMessageOnError);
    }

    /// <summary>
    /// Gets a list of services corresponding to this machine.
    /// </summary>
    /// <param name="serviceNameFilter">Text to search within the service name to get services that contain the text, if null or empty all services are returned.</param>
    /// <param name="useDisplayName">Flag indicating if the display name is used to match the name filter instead of the service name.</param>
    /// <param name="displayMessageOnError">Flag indicating if an error message is displayed to users indicating the connection error.</param>
    /// <returns>A collection of management objects.</returns>
    public ManagementObjectCollection GetWmiServices(string serviceNameFilter, bool useDisplayName, bool displayMessageOnError)
    {
      ManagementObjectCollection wmiServicesCollection = null;
      Exception connectionException = null;

      try
      {
        if (!WmiManagementScope.IsConnected)
        {
          WmiManagementScope.Connect();
        }

        if (_connectionTestCancelled)
        {
          _connectionTestCancelled = false;
          return null;
        }

        string filterQuery = useDisplayName ? WMI_QUERY_SELECT_DISPLAY_NAME_CONTAINING : WMI_QUERY_SELECT_NAME_CONTAINING;
        WqlObjectQuery query = string.IsNullOrEmpty(serviceNameFilter) ? new WqlObjectQuery(WMI_QUERY_SELECT_ALL) : new WqlObjectQuery(string.Format(filterQuery, serviceNameFilter));
        ManagementObjectSearcher searcher = new ManagementObjectSearcher(WmiManagementScope, query);
        wmiServicesCollection = searcher.Get();
        if (_connectionTestCancelled)
        {
          _connectionTestCancelled = false;
          return null;
        }

        // Verify if we can access the services within the services collection by trying to access the first element
        // that is why after the element is successfully accessed (a property in it) we exit the loop.
        foreach (var mo in wmiServicesCollection)
        {
          string serviceDisplayName = mo["DisplayName"].ToString();
          Debug.Write(serviceDisplayName);
          break;
        }

        if (_connectionTestCancelled)
        {
          _connectionTestCancelled = false;
          return null;
        }

        ConnectionProblem = ConnectionProblemType.None;
      }
      catch (UnauthorizedAccessException uaEx)
      {
        ConnectionProblem = ConnectionProblemType.IncorrectUserOrPassword;
        connectionException = uaEx;
      }
      catch (Exception ex)
      {
        ConnectionProblem = ConnectionProblemType.InsufficientAccessPermissions;
        connectionException = ex;
      }

      if (connectionException == null)
      {
        return wmiServicesCollection;
      }

      MySqlSourceTrace.WriteToLog(ConnectionProblemLongDescription, SourceLevels.Information);
      MySqlSourceTrace.WriteAppErrorToLog(connectionException);
      if (displayMessageOnError)
      {
        using (var errorDialog = new InfoDialog(InfoDialogProperties.GetErrorDialogProperties(
          ConnectionProblemShortDescription,
          ConnectionProblemLongDescription,
          null,
          ConnectionProblem == ConnectionProblemType.InsufficientAccessPermissions
            ? Resources.MachineUnavailableExtendedMessage + Environment.NewLine + Environment.NewLine +
              connectionException.Message
            : null)))
        {
          errorDialog.DefaultButton = InfoDialog.DefaultButtonType.Button1;
          errorDialog.DefaultButtonTimeout = 30;
          errorDialog.ShowDialog();
        }
      }

      return null;
    }

    /// <summary>
    /// Load Calculated, Machine dependant StartupParameters
    /// </summary>
    /// <param name="setupWmiEventsOnly">When <c>true</c> will suscribe WMI event watchers only and will skip further operations with services.</param>
    public void LoadServicesParameters(bool setupWmiEventsOnly)
    {
      if (!InitialLoadDone && IsLocal && setupWmiEventsOnly)
      {
        SetupWmiEvents();
        return;
      }

      // Set services StartupParameters and subscribe to service events.
      if (Services != null && Services.Count > 0)
      {
        var serviceNamesList = Services.ConvertAll(service => service.ServiceName);
        foreach (MySqlService service in serviceNamesList.Select(GetServiceByName).Where(service => service != null))
        {
          ChangeService(service, InitialLoadDone ? ListChangeType.Updated : ListChangeType.AddByLoad);
        }
      }

      // Test connection status if this is a remote machine during an initial load only.
      if (!InitialLoadDone && !IsLocal && ConnectionStatus != ConnectionStatusType.Online)
      {
        TestConnection(false, true);
      }

      InitialLoadDone = true;
    }

    /// <summary>
    /// Attempts to establish a connection to a machine that is currently offline.
    /// </summary>
    public void Reconnect()
    {
      TestConnection(true, true);
      _secondsToAutoTestConnection = AutoTestConnectionIntervalInSeconds;
    }

    /// <summary>
    /// Refreshes the machine's status and the status of each of its services.
    /// </summary>
    /// <param name="worker"><see cref="BackgroundWorker"/> object in case that the caller means to execute this method in another thread.</param>
    /// <returns><c>true</c> if the operation was cancelled by the passed background worker, <c>false</c> otherwise.</returns>
    public bool RefreshStatus(ref BackgroundWorker worker)
    {
      bool cancelled = false;

      // If it's the local Machine, check if it contains services since it may not have been added to the menu and it may just be monitoring for services creation.
      if (IsLocal && Services.Count == 0)
      {
        return false;
      }

      // If user cancells before even testing the connection, then return.
      if (worker != null && worker.CancellationPending)
      {
        return true;
      }

      // Update the machine's menu item stating that the machine is refreshing its status.
      RefreshingStatus = true;
      UpdateMenuGroup();

      // Attempt to test the connection only if the machine is a remote one.
      var remoteMachineStatus = ConnectionStatus;
      if (!IsLocal)
      {
        TestConnection(false, false);
      }

      // Refresh the machine services only if it's a local machine or if it was online and status did not chance,
      // this in order to fix a racing condition where services do not report back a status change after they are
      // in a Start Pending or Stop Pending status.
      if (IsLocal || (remoteMachineStatus == ConnectionStatusType.Online && IsOnline))
      {
        foreach (var remoteService in Services)
        {
          if (worker != null && worker.CancellationPending)
          {
            cancelled = true;
            break;
          }

          remoteService.RefreshStatusAndName(true);
        }
      }

      // Update the machine's menu item only reflecting its new status.
      RefreshingStatus = false;
      UpdateMenuGroup();
      return cancelled;
    }

    /// <summary>
    /// Removes all monitored services from the machine.
    /// </summary>
    /// <returns>Number of removed services.</returns>
    public int RemoveAllServices()
    {
      int removedServicesQuantity = 0;
      if (Services == null || Services.Count == 0)
      {
        return removedServicesQuantity;
      }

      var serviceNamesList = Services.ConvertAll(service => service.ServiceName);
      foreach (MySqlService service in serviceNamesList.Select(GetServiceByName))
      {
        ChangeService(service, ListChangeType.Cleared);
        removedServicesQuantity++;
      }

      return removedServicesQuantity;
    }

    /// <summary>
    /// Removes the menu group for this machine.
    /// </summary>
    /// <param name="menu">The Notifier's context menu.</param>
    public void RemoveMenuGroup(ContextMenuStrip menu)
    {
      if (menu.InvokeRequired)
      {
        menu.Invoke(new MethodInvoker(() => RemoveMenuGroup(menu)));
      }
      else
      {
        foreach (MySqlService service in Services)
        {
          service.MenuGroup.RemoveFromContextMenu(menu);
        }

        MenuGroup.DropDownItems.Clear();
        int index = ServiceMenuGroup.FindMenuItemWithinMenuStrip(menu, MachineId);
        if (index >= 0)
        {
          menu.Items.RemoveAt(index);
          menu.Refresh();
        }

        MenuGroup = null;
      }
    }

    /// <summary>
    /// Creates the menu group for the machine if it has not been initialized.
    /// </summary>
    /// <param name="menu">The Notifier's context menu.</param>
    public void SetupMenuGroup(ContextMenuStrip menu)
    {
      if (menu.InvokeRequired)
      {
        menu.Invoke(new MethodInvoker(() => SetupMenuGroup(menu)));
      }
      else
      {
        if (MenuGroup != null)
        {
          return;
        }

        MenuGroup = new ToolStripMenuItem(string.Format("{0} ({1})", Name, ConnectionStatus.ToString()))
        {
          Tag = MachineId
        };

        Font boldFont = new Font(MenuGroup.Font, FontStyle.Bold);
        MenuGroup.Font = boldFont;
        MenuGroup.BackColor = SystemColors.MenuText;
        MenuGroup.ForeColor = SystemColors.Menu;
        int index = 0;
        if (!IsLocal)
        {
          index = ServiceMenuGroup.FindMenuItemWithinMenuStrip(menu, Resources.MySQLInstances);
          if (index < 0)
          {
            index = ServiceMenuGroup.FindMenuItemWithinMenuStrip(menu, Resources.Actions);
            if (index < 0)
            {
              index = 0;
            }
          }
        }

        menu.Items.Insert(index, MenuGroup);

        // Hide the separator just above this new menu item if needed.
        if (index > 0 && menu.Items[index - 1] is ToolStripSeparator)
        {
          menu.Items[index - 1].Visible = false;
        }

        UpdateMenuGroup();
      }
    }

    /// <summary>
    /// Tests the connection for this machine and checks if services can be queried via WMI.
    /// </summary>
    /// <param name="displayMessageOnError">Flag indicating if an error message is displayed to users indicating the connection error.</param>
    /// <param name="asynchronous">Flag indicating if the status check is run asynchronously or synchronously.</param>
    public void TestConnection(bool displayMessageOnError, bool asynchronous)
    {
      _secondsToAutoTestConnection = AutoTestConnectionIntervalInSeconds;

      if (ConnectionTestInProgress)
      {
        return;
      }

      // Start with a Connecting... status
      ConnectionTestInProgress = true;
      ConnectionStatus = ConnectionStatusType.Connecting;

      if (asynchronous)
      {
        SetupConnectionTestBackgroundWorker();
        _worker.RunWorkerAsync(displayMessageOnError);
      }
      else
      {
        Cursor.Current = Cursors.WaitCursor;
        TestConnectionWorkerDoWork(this, new DoWorkEventArgs(displayMessageOnError));
        TestConnectionWorkerCompleted(this, new RunWorkerCompletedEventArgs(null, null, false));
        Cursor.Current = Cursors.Default;
      }
    }

    /// <summary>
    /// Returns the machine name.
    /// </summary>
    /// <returns>Machine name.</returns>
    public override string ToString()
    {
      return IsLocal ? "Local" : Name;
    }

    /// <summary>
    /// Updates the menu group for the machine, creates it if it has not been initialized.
    /// </summary>
    public void UpdateMenuGroup()
    {
      ToolStrip menu = MenuGroup.GetCurrentParent();
      if (menu != null && menu.InvokeRequired)
      {
        menu.Invoke(new MethodInvoker(UpdateMenuGroup));
      }
      else
      {
        MenuGroup.Text = string.Format("{0} ({1}){2}", Name, ConnectionStatus.ToString(), RefreshingStatus ? Resources.RefreshingStatusText : string.Empty);
        if (ConnectionStatus == ConnectionStatusType.Unavailable && MenuGroup.DropDownItems.Count == 0)
        {
          ToolStripMenuItem reconnectMenu = new ToolStripMenuItem(Resources.ReconnectMenuText, Resources.refresh, ReconnectMenu_Click);
          MenuGroup.DropDownItems.Add(reconnectMenu);
        }
        else if (IsOnline && MenuGroup.DropDownItems.Count > 0)
        {
          MenuGroup.DropDownItems.Clear();
        }
      }
    }

    /// <summary>
    /// Overwrites this computer's user and password with the given ones.
    /// </summary>
    /// <param name="fromMachine">Machine to copy data from.</param>
    /// <param name="loginHasChanged">Indicates whether or not the login information has changed and services have it updated.</param>
    internal void CopyMachineData(Machine fromMachine, bool loginHasChanged)
    {
      Name = fromMachine.Name;
      User = fromMachine.User;
      Password = fromMachine.Password;
      AutoTestConnectionInterval = fromMachine.AutoTestConnectionInterval;
      AutoTestConnectionIntervalUnitOfMeasure = fromMachine.AutoTestConnectionIntervalUnitOfMeasure;

      if (!loginHasChanged)
      {
        return;
      }

      foreach (MySqlService service in Services)
      {
        service.SetServiceParameters(true);
      }
    }

    /// <summary>
    /// Fires the <see cref="MachineStatusChanged"/> event.
    /// </summary>
    /// <param name="oldConnectionStatus">Old connection status.</param>
    protected virtual void OnMachineStatusChanged(ConnectionStatusType oldConnectionStatus)
    {
      if (MachineStatusChanged != null)
      {
        MachineStatusChanged(this, oldConnectionStatus);
      }

      if (OldConnectionStatus == ConnectionStatus ||
          (!IsOnline && ConnectionStatus != ConnectionStatusType.Unavailable))
      {
        return;
      }

      if (InitialLoadDone)
      {
        LoadServicesParameters(false);
      }

      SetupWmiEvents();
    }

    /// <summary>
    /// Fires the <see cref="ServiceListChanged"/> event.
    /// </summary>
    /// <param name="service">Service that caused the services list change.</param>
    /// <param name="listChangeType">List change type.</param>
    protected virtual void OnServiceListChanged(MySqlService service, ListChangeType listChangeType)
    {
      if (ServiceListChanged != null)
      {
        ServiceListChanged(this, service, listChangeType);
      }
    }

    /// <summary>
    /// Fires the <see cref="ServiceStatusChanged"/> event.
    /// </summary>
    /// <param name="service">Service whose status changed.</param>
    protected virtual void OnServiceStatusChanged(MySqlService service)
    {
      if (ServiceStatusChanged != null)
      {
        ServiceStatusChanged(this, service);
      }
    }

    /// <summary>
    /// Initializes or refreshes the given service caused by an initial load or an update on Machine status.
    /// </summary>
    /// <param name="service">Service to initialize.</param>
    /// <param name="listChangeType">Change type (addition, removal, update) related to a MySQL service.</param>
    private void LoadServiceParameters(MySqlService service, ListChangeType listChangeType)
    {
      service.Host = this;
      service.SetServiceParameters(listChangeType == ListChangeType.Updated);
      service.StatusChanged -= OnServiceStatusChanged;
      service.StatusChangeError -= OnServiceStatusChangeError;

      if (IsOnline && !service.ServiceInstanceExists)
      {
        ChangeService(service, ListChangeType.RemoveByEvent);
      }
      else
      {
        service.StatusChanged += OnServiceStatusChanged;
        service.StatusChangeError += OnServiceStatusChangeError;
        if (!InitialLoadDone)
        {
          OnServiceListChanged(service, ListChangeType.AddByLoad);
        }
      }
    }

    /// <summary>
    /// Fires the <see cref="ServiceStatusChangeError"/> event.
    /// </summary>
    /// <param name="service">Service whose status changed.</param>
    /// <param name="ex">Exception error thrown while trying to change service status.</param>
    private void OnServiceStatusChangeError(MySqlService service, Exception ex)
    {
      if (ServiceStatusChangeError != null)
      {
        ServiceStatusChangeError(this, service, ex);
      }
    }

    /// <summary>
    /// Event delegate method that is fired when a WMI service is created.
    /// </summary>
    /// <param name="remoteService">Remote service firing the status changed event.</param>
    private void OnWmiServiceCreated(ManagementBaseObject remoteService)
    {
      var serviceName = remoteService == null ? string.Empty : remoteService["Name"].ToString().ToLowerInvariant();
      if (!Settings.Default.AutoAddServicesToMonitor || GetServiceByName(serviceName) != null || !serviceName.Contains(Settings.Default.AutoAddPattern))
      {
        return;
      }

      var service = new MySqlService(serviceName, Settings.Default.NotifyOfStatusChange, Settings.Default.NotifyOfStatusChange, this);
      if (!Service.IsRealMySqlService(serviceName))
      {
        return;
      }

      service.SetServiceParameters(true);
      ChangeService(service, ListChangeType.AutoAdd);
    }

    /// <summary>
    /// Event delegate method that is fired when a WMI service is deleted.
    /// </summary>
    /// <param name="remoteService">Remote service firing the status changed event.</param>
    private void OnWmiServiceDeleted(ManagementBaseObject remoteService)
    {
      if (remoteService == null)
      {
        return;
      }

      string serviceName = remoteService["Name"].ToString().Trim();
      MySqlService service = GetServiceByName(serviceName);
      if (service != null)
      {
        ChangeService(service, ListChangeType.RemoveByEvent);
      }
    }

    /// <summary>
    /// Event delegate method that is fired when a WMI service status changes.
    /// </summary>
    /// <param name="remoteService">Remote service firing the status changed event.</param>
    private void OnWmiServiceStatusChanged(ManagementBaseObject remoteService)
    {
      if (remoteService == null)
      {
        return;
      }

      string serviceName = remoteService["Name"].ToString().Trim();
      string state = remoteService["State"].ToString();
      MySqlService service = GetServiceByName(serviceName);
      if (service != null)
      {
        service.SetStatus(state);
      }
    }

    /// <summary>
    /// Event delegate method fired when the menu item used to reconnect to the machine is clicked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ReconnectMenu_Click(object sender, EventArgs e)
    {
      if (MenuGroup == null)
      {
        return;
      }

      ToolStrip menu = MenuGroup.GetCurrentParent();
      if (menu != null && menu.InvokeRequired)
      {
        menu.Invoke(new MethodInvoker(() => ReconnectMenu_Click(sender, e)));
      }
      else
      {
        Reconnect();
      }
    }

    /// <summary>
    /// Initializes the background worker used to test connections asynchronously.
    /// </summary>
    private void SetupConnectionTestBackgroundWorker()
    {
      if (_worker != null)
      {
        return;
      }

      _worker = new BackgroundWorker { WorkerSupportsCancellation = true, WorkerReportsProgress = false };
      _worker.DoWork += TestConnectionWorkerDoWork;
      _worker.RunWorkerCompleted += TestConnectionWorkerCompleted;
    }

    /// <summary>
    /// Sets up the WMI events that monitor service deletions and service status changes.
    /// </summary>
    private void SetupWmiEvents()
    {
      _wmiServicesWatcher = _wmiServicesWatcher ?? new ServiceWatcher(true, true, true, UseAsynchronousWmi, IsOnline);
      _wmiServicesWatcher.WmiQueriesTimeoutInSeconds = WmiQueriesTimeoutInSeconds;

      if (IsOnline)
      {
        if (IsLocal)
        {
          _wmiServicesWatcher.InstallationChanged += OnInstallationChanged;
        }
        _wmiServicesWatcher.ServiceCreated += OnWmiServiceCreated;
        _wmiServicesWatcher.ServiceDeleted += OnWmiServiceDeleted;
        _wmiServicesWatcher.ServiceStatusChanged += OnWmiServiceStatusChanged;
        _wmiServicesWatcher.Start(WmiManagementScope);
      }
      else
      {
        _wmiServicesWatcher.IsMachineOnline = IsOnline;
        _wmiServicesWatcher.ServiceCreated -= OnWmiServiceCreated;
        _wmiServicesWatcher.ServiceDeleted -= OnWmiServiceDeleted;
        _wmiServicesWatcher.ServiceStatusChanged -= OnWmiServiceStatusChanged;
        _wmiServicesWatcher.Stop(true);
      }
    }

    /// <summary>
    /// Event delegate method that is fired when Workbench installation changed.
    /// </summary>
    /// <param name="remoteService">The remote service.</param>
    private void OnInstallationChanged(ManagementBaseObject remoteService)
    {
      if (WorkbenchInstallationChanged != null && MySqlWorkbench.IsInstalled != _workbenchWasInstalled)
      {
        _workbenchWasInstalled = MySqlWorkbench.IsInstalled;
        WorkbenchInstallationChanged(remoteService);
      }
    }

    /// <summary>
    /// Delegate method that reports the asynchronous operation to test the machine's connection status has completed.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void TestConnectionWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      ConnectionTestInProgress = false;
      if (e.Cancelled)
      {
        _connectionStatus = OldConnectionStatus;
        ConnectionProblem = ConnectionProblemType.None;
      }
      else
      {
        // Report the connection status based on the Connection + Services retrieval + WMI events tests
        ConnectionStatus = ConnectionProblem != ConnectionProblemType.None ? ConnectionStatusType.Unavailable : ConnectionStatusType.Online;
      }
    }

    /// <summary>
    /// Delegate method that asynchronously tests the machine's connection status.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void TestConnectionWorkerDoWork(object sender, DoWorkEventArgs e)
    {
      var worker = sender as BackgroundWorker;

      // Try to see if we can connect to the remote computer and retrieve its services
      bool displayMessageOnError = (bool)e.Argument;
      if (worker != null && worker.CancellationPending)
      {
        e.Cancel = true;
        return;
      }

      GetWmiServices("eventlog", false, displayMessageOnError);
      if (worker != null && worker.CancellationPending)
      {
        e.Cancel = true;
        return;
      }

      // If services could be retrieved, try to subscribe to WMI events using a dummy watcher (ONLY if using asynchronous mode).
      if (!UseAsynchronousWmi || ConnectionProblem != ConnectionProblemType.None)
      {
        return;
      }

      ServiceWatcher dummyWatcher = null;
      try
      {
        if (!WmiManagementScope.IsConnected)
        {
          WmiManagementScope.Connect();
        }

        if (worker != null && worker.CancellationPending)
        {
          e.Cancel = true;
          return;
        }

        dummyWatcher = new ServiceWatcher(false, false, true, UseAsynchronousWmi, true)
        {
          WmiQueriesTimeoutInSeconds = WmiQueriesTimeoutInSeconds
        };

        dummyWatcher.Start(WmiManagementScope);
      }
      catch (Exception ex)
      {
        ConnectionProblem = ConnectionProblemType.InsufficientAccessPermissions;
        MySqlSourceTrace.WriteToLog(ConnectionProblemLongDescription, SourceLevels.Information);
        MySqlSourceTrace.WriteAppErrorToLog(ex);
        if (displayMessageOnError)
        {
          using (var errorDialog = new InfoDialog(InfoDialogProperties.GetErrorDialogProperties(
            ConnectionProblemShortDescription,
            ConnectionProblemLongDescription,
            null,
            Resources.MachineUnavailableExtendedMessage + Environment.NewLine + Environment.NewLine + ex.Message)))
          {
            errorDialog.DefaultButton = InfoDialog.DefaultButtonType.Button1;
            errorDialog.DefaultButtonTimeout = 30;
            errorDialog.ShowDialog();
          }
        }
      }
      finally
      {
        if (dummyWatcher != null)
        {
          dummyWatcher.Dispose();
        }
      }
    }
  }
}