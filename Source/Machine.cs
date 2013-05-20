//
// Copyright (c) 2013, Oracle and/or its affiliates. All rights reserved.
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
  using System.Drawing;
  using System.Management;
  using System.Windows.Forms;
  using System.Xml.Serialization;
  using MySql.Notifier.Properties;
  using MySQL.Utility;
  using MySQL.Utility.Forms;

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
    /// Default interval unit of measure, set to minutes by default.
    /// </summary>
    public const TimeUtilities.IntervalUnitOfMeasure DEFAULT_AUTO_TEST_CONNECTION_UOM = TimeUtilities.IntervalUnitOfMeasure.Minutes;

    /// <summary>
    /// Represents the WMI namespace for a local computer.
    /// </summary>
    public const string WMI_LOCAL_NAMESPACE = @"root\cimv2";

    /// <summary>
    /// The where clause part of WMI queries regarding services.
    /// </summary>
    public const string WMI_QUERIES_WHERE_CLAUSE = "TargetInstance isa 'Win32_Service'";

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
    /// WMI watcher for creation of services related to this machine.
    /// </summary>
    private ManagementEventWatcher _wmiServiceCreationWatcher;

    /// <summary>
    /// WMI watcher for deletion of services related to this machine.
    /// </summary>
    private ManagementEventWatcher _wmiServiceDeletionWatcher;

    /// <summary>
    /// WMI watcher for change of status in services related to this machine.
    /// </summary>
    private ManagementEventWatcher _wmiServiceStatusChangeWatcher;

    /// <summary>
    /// Background worker that performs an asynchronous connection test.
    /// </summary>
    private BackgroundWorker _worker;

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
      _wmiServiceDeletionWatcher = null;
      _wmiServiceStatusChangeWatcher = null;
      _worker = null;
      ConnectionTestInProgress = false;
      ConnectionProblem = ConnectionProblemType.None;
      InitialLoadDone = false;
      MachineId = Guid.NewGuid().ToString("B");
      MenuGroup = null;
      User = string.Empty;
      OldConnectionStatus = ConnectionStatusType.Unknown;
      Password = string.Empty;
      SecondsToAutoTestConnection = AutoTestConnectionIntervalInSeconds;
      Services = new List<MySQLService>();
      WMIQueriesTimeoutInSeconds = 5;
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
      Password = MySQLSecurity.EncryptPassword(password);
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
    /// <param name="changeType">Service list change type.</param>
    public delegate void ServiceListChangedHandler(Machine machine, MySQLService service, ChangeType changeType);

    /// <summary>
    /// Notifies that the status of one of the services in the list has changed.
    /// </summary>
    /// <param name="machine">Machine instance.</param>
    /// <param name="service">MySQLService instance.</param>
    public delegate void ServiceStatusChangedHandler(Machine machine, MySQLService service);

    /// <summary>
    /// This event system handles the case where the remote machine is unavailable, and a service has failed to connect to the host.
    /// </summary>
    /// <param name="machine">Machine instance.</param>
    /// <param name="service">MySQLService instance.</param>
    /// <param name="ex">Exception thrown while trying to change the service's status.</param>
    public delegate void ServiceStatusChangeErrorHandler(Machine machine, MySQLService service, Exception ex);

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
    /// Gets the current status of this machine, refreshed by calling the <see cref="TestConnection"/> or the <see cref="GetWMIServices"/> method.
    /// </summary>
    [XmlIgnore]
    public ConnectionProblemType ConnectionProblem { get; private set; }

    /// <summary>
    /// Gets a long description about the current status of this machine, refreshed by calling the <see cref="TestConnection"/> or the <see cref="GetWMIServices"/> method.
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
    /// Gets a short description about the current status of this machine, refreshed by calling the <see cref="TestConnection"/> or the <see cref="GetWMIServices"/> method.
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
    /// Gets the current status of this machine, refreshed by calling the <see cref="TestConnection"/> or the <see cref="GetWMIServices"/> method.
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
        if (oldConnectionStatus != _connectionStatus)
        {
          if (oldConnectionStatus != ConnectionStatusType.Connecting && oldConnectionStatus != ConnectionStatusType.Unknown)
          {
            OldConnectionStatus = oldConnectionStatus;
          }

          OnMachineStatusChanged(oldConnectionStatus);
        }
      }
    }

    /// <summary>
    /// Gets a value indicating whether a connection test is still ongoing.
    /// </summary>
    [XmlIgnore]
    public bool ConnectionTestInProgress { get; private set; }

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
        string _oldName = _name;
        _name = value;
        if (string.Compare(_oldName, _name, true) != 0)
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
          _secondsToAutoTestConnection = AutoTestConnectionIntervalInSeconds;
          if (ConnectionStatus != ConnectionStatusType.Online && ConnectionStatus != ConnectionStatusType.Connecting)
          {
            TestConnection(false, true);
          }
        }
      }
    }

    /// <summary>
    /// Gets or sets the list of services associated with this machine.
    /// </summary>
    [XmlElement(ElementName = "ServicesList", Type = typeof(List<MySQLService>))]
    public List<MySQLService> Services { get; set; }

    /// <summary>
    /// Gets the password as an unencrypted string.
    /// </summary>
    [XmlIgnore]
    public string UnprotectedPassword
    {
      get
      {
        return string.IsNullOrEmpty(Password) ? string.Empty : MySQLSecurity.DecryptPassword(Password);
      }
    }

    /// <summary>
    /// Gets or sets the name of the user to connect to this machine.
    /// </summary>
    [XmlAttribute("User")]
    public string User { get; set; }

    /// <summary>
    /// Gets or sets the timeout in seconds for WMI queries.
    /// </summary>
    [XmlAttribute("WMIQueriesTimeoutInSeconds")]
    public ushort WMIQueriesTimeoutInSeconds { get; set; }

    /// <summary>
    /// Gets an object that contains the settings to perform a WMI connection.
    /// </summary>
    private ConnectionOptions WMIConnectionOptions
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
        _wmiConnectionOptions.Password = MySQLSecurity.DecryptPassword(Password);
        return _wmiConnectionOptions;
      }
    }

    /// <summary>
    /// Gets the scope (namespace) used for WMI operations.
    /// </summary>
    private ManagementScope WMIManagementScope
    {
      get
      {
        if (_wmiManagementScope == null)
        {
          _wmiManagementScope = new ManagementScope();
        }

        if (IsLocal)
        {
          if (_wmiManagementScope.Path == null || _wmiManagementScope.Path.NamespacePath != WMI_LOCAL_NAMESPACE)
          {
            _wmiManagementScope = new ManagementScope(WMI_LOCAL_NAMESPACE);
          }
        }
        else
        {
          if (_wmiManagementScope.Path == null || _wmiManagementScope.Path.Server != Name || _wmiManagementScope.Options != WMIConnectionOptions)
          {
            _wmiManagementScope = new ManagementScope(string.Format(WMI_REMOTE_NAMESPACE, Name), WMIConnectionOptions);
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
    public bool CancelAsynchronousConnectionTest()
    {
      if (_worker != null && ConnectionTestInProgress)
      {
        _connectionTestCancelled = true;
        _worker.CancelAsync();
      }

      return _connectionTestCancelled;
    }

    /// <summary>
    /// Adds or Delete a Service for current machine
    /// </summary>
    /// <param name="service">MySQLService instance</param>
    /// <param name="changeType">Add/Delete</param>
    public void ChangeService(MySQLService service, ChangeType changeType)
    {
      switch (changeType)
      {
        case ChangeType.AddByUser:
        case ChangeType.AddByLoad:
        case ChangeType.AutoAdd:
          if (changeType == ChangeType.AutoAdd || (changeType == ChangeType.AddByUser && GetServiceByName(service.ServiceName) == null))
          {
            service.NotifyOnStatusChange = Settings.Default.NotifyOfStatusChange;
            service.UpdateTrayIconOnStatusChange = true;
            Services.Add(service);
            OnServiceListChanged(service, changeType);
          }

          if (changeType != ChangeType.AutoAdd)
          {
            LoadServiceParameters(service);
          }
          break;

        case ChangeType.Cleared:
        case ChangeType.RemoveByUser:
        case ChangeType.RemoveByEvent:
          Services.Remove(service);
          OnServiceListChanged(service, changeType);
          break;

        case ChangeType.Updated:
          LoadServiceParameters(service);
          OnServiceListChanged(service, changeType);
          break;
      }
    }

    /// <summary>
    /// Used to see if service is already on the list.
    /// </summary>
    /// <param name="service">MySQLService instance to look for.</param>
    /// <returns>True if current machine contains it already.</returns>
    public bool ContainsService(MySQLService service)
    {
      if (Services == null || Services.Count == 0)
      {
        return false;
      }

      return GetServiceByName(service.ServiceName) == null ? false : true;
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
        //// Free managed resources
        if (_wmiServiceCreationWatcher != null)
        {
          _wmiServiceCreationWatcher.Stop();
          _wmiServiceCreationWatcher.Dispose();
        }

        if (_wmiServiceDeletionWatcher != null)
        {
          _wmiServiceDeletionWatcher.Stop();
          _wmiServiceDeletionWatcher.Dispose();
        }

        if (_wmiServiceStatusChangeWatcher != null)
        {
          _wmiServiceStatusChangeWatcher.Stop();
          _wmiServiceStatusChangeWatcher.Dispose();
        }

        if (Services != null)
        {
          foreach (MySQLService service in Services)
          {
            if (service != null)
            {
              service.Dispose();
            }
          }
        }
      }

      //// Add class finalizer if unmanaged resources are added to the class
      //// Free unmanaged resources if there are any
    }

    /// <summary>
    /// Returns an instance of a service if is already on the list, searching by name
    /// </summary>
    /// <param name="name">MySQLService instance name</param>
    /// <returns>MySQLService instance</returns>
    public MySQLService GetServiceByName(string name)
    {
      foreach (MySQLService service in Services)
      {
        if (string.Compare(service.ServiceName, name, true) == 0)
        {
          return service;
        }
      }

      return null;
    }

    /// <summary>
    /// Gets a list of services corresponding to this machine.
    /// </summary>
    /// <param name="displayMessageOnError">Flag indicating if an error message is displayed to users indicating the connection error.</param>
    /// <returns>A collection of management objects.</returns>
    public ManagementObjectCollection GetWMIServices(bool displayMessageOnError)
    {
      return GetWMIServices(null, displayMessageOnError);
    }

    /// <summary>
    /// Gets a list of services corresponding to this machine.
    /// </summary>
    /// <param name="serviceName">Name of specific service to get, if null or empty all services are returned.</param>
    /// <param name="displayMessageOnError">Flag indicating if an error message is displayed to users indicating the connection error.</param>
    /// <returns>A collection of management objects.</returns>
    public ManagementObjectCollection GetWMIServices(string serviceName, bool displayMessageOnError)
    {
      ManagementObjectCollection wmiServicesCollection = null;
      Exception connectionException = null;

      try
      {
        string serviceDisplayName;
        WMIManagementScope.Connect();
        if (_connectionTestCancelled)
        {
          _connectionTestCancelled = false;
          return null;
        }

        WqlObjectQuery query = string.IsNullOrEmpty(serviceName) ? new WqlObjectQuery("Select * From Win32_Service")
        : new WqlObjectQuery(string.Format("Select * From Win32_Service Where Name = \"{0}\"", serviceName));
        ManagementObjectSearcher searcher = new ManagementObjectSearcher(WMIManagementScope, query);
        wmiServicesCollection = searcher.Get();
        if (_connectionTestCancelled)
        {
          _connectionTestCancelled = false;
          return null;
        }

        if (wmiServicesCollection != null)
        {
          //// Verify if we can access the services within the services collection.
          foreach (var mo in wmiServicesCollection)
          {
            serviceDisplayName = mo["DisplayName"].ToString();
            break;
          }
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

      if (connectionException != null)
      {
        MySQLSourceTrace.WriteToLog(ConnectionProblemLongDescription, System.Diagnostics.SourceLevels.Information);
        MySQLSourceTrace.WriteAppErrorToLog(connectionException);
        if (displayMessageOnError)
        {
          InfoDialog.ShowErrorDialog(
            ConnectionProblemShortDescription,
            ConnectionProblemLongDescription,
            null,
            ConnectionProblem == ConnectionProblemType.InsufficientAccessPermissions ? Resources.MachineUnavailableExtendedMessage : null,
            true,
            InfoDialog.DefaultButtonType.AcceptButton,
            30);
        }

        return null;
      }

      return wmiServicesCollection;
    }

    /// <summary>
    /// Load Calculated, Machine dependant StartupParameters
    /// </summary>
    /// <param name="setupWMIEventsOnly">When true will suscribe WMI event watchers only and will skip further operations with services.</param>
    public void LoadServicesParameters(bool setupWMIEventsOnly)
    {
      if (!InitialLoadDone)
      {
        if (IsLocal && setupWMIEventsOnly)
        {
          SetupWMIEvents();
          return;
        }
        else if (ConnectionStatus != ConnectionStatusType.Online)
        {
          //// Test connection status
          TestConnection(false, true);
        }
      }

      //// Set services StartupParameters and subscribe to service events.
      if (Services != null && Services.Count > 0)
      {
        var serviceNamesList = Services.ConvertAll<string>(service => service.ServiceName);
        foreach (string serviceName in serviceNamesList)
        {
          MySQLService service = GetServiceByName(serviceName);
          if (service != null)
          {
            ChangeService(service, InitialLoadDone ? ChangeType.Updated : ChangeType.AddByLoad);
          }
        }
      }

      InitialLoadDone = true;
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

      var serviceNamesList = Services.ConvertAll<string>(service => service.ServiceName);
      foreach (string serviceName in serviceNamesList)
      {
        MySQLService service = GetServiceByName(serviceName);
        ChangeService(service, ChangeType.Cleared);
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
      foreach (MySQLService service in Services)
      {
        service.MenuGroup.RemoveFromContextMenu(menu);
      }

      MenuGroup.DropDownItems.Clear();
      int index = ServiceMenuGroup.FindMenuItemWithinMenuStrip(menu, MenuGroup.Text);
      if (index >= 0)
      {
        menu.Items.RemoveAt(index);
        menu.Refresh();
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
        menu.Invoke(new MethodInvoker(() => { SetupMenuGroup(menu); }));
      }
      else
      {
        if (MenuGroup != null)
        {
          return;
        }

        MenuGroup = new ToolStripMenuItem(string.Format("{0} ({1})", Name, ConnectionStatus.ToString()));
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

        //// Hide the separator just above this new menu item if needed.
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
      if (ConnectionTestInProgress)
      {
        return;
      }

      ConnectionTestInProgress = true;
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
        menu.Invoke(new MethodInvoker(() => { UpdateMenuGroup(); }));
      }
      else
      {
        MenuGroup.Text = string.Format("{0} ({1})", Name, ConnectionStatus.ToString());
        if (ConnectionStatus == ConnectionStatusType.Unavailable && MenuGroup.DropDownItems.Count == 0)
        {
          ToolStripMenuItem reconnectMenu = new ToolStripMenuItem("Reconnect", Resources.refresh, ReconnectMenu_Click);
          MenuGroup.DropDownItems.Add(reconnectMenu);
        }
        else if (ConnectionStatus == ConnectionStatusType.Online && MenuGroup.DropDownItems.Count > 0)
        {
          MenuGroup.DropDownItems.Clear();
        }
      }
    }

    /// <summary>
    /// Overwrites this computer's user and password with the given ones.
    /// </summary>
    /// <param name="fromMachine">Machine to copy data from.</param>
    internal void CopyMachineData(Machine fromMachine)
    {
      Name = fromMachine.Name;
      User = fromMachine.User;
      Password = fromMachine.Password;
      AutoTestConnectionInterval = fromMachine.AutoTestConnectionInterval;
      AutoTestConnectionIntervalUnitOfMeasure = fromMachine.AutoTestConnectionIntervalUnitOfMeasure;
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

      if (OldConnectionStatus != ConnectionStatus
          && (ConnectionStatus == Machine.ConnectionStatusType.Online || ConnectionStatus == Machine.ConnectionStatusType.Unavailable))
      {
        LoadServicesParameters(false);
        SetupWMIEvents();
      }
    }

    /// <summary>
    /// Fires the <see cref="ServiceListChanged"/> event.
    /// </summary>
    /// <param name="service">Service that caused the services list change.</param>
    /// <param name="changeType">List change type.</param>
    protected virtual void OnServiceListChanged(MySQLService service, ChangeType changeType)
    {
      if (ServiceListChanged != null)
      {
        ServiceListChanged(this, service, changeType);
      }
    }

    /// <summary>
    /// Fires the <see cref="ServiceStatusChanged"/> event.
    /// </summary>
    /// <param name="service">Service whose status changed.</param>
    protected virtual void OnServiceStatusChanged(MySQLService service)
    {
      if (ServiceStatusChanged != null)
      {
        ServiceStatusChanged(this, service);
      }
    }

    /// <summary>
    /// Load Calculated, Machine dependant StartupParameters
    /// </summary>
    /// <param name="service">Service to initialize.</param>
    private void LoadServiceParameters(MySQLService service)
    {
      service.Host = this;
      service.SetServiceParameters();
      service.StatusChanged -= OnServiceStatusChanged;
      service.StatusChangeError -= OnServiceStatusChangeError;

      if (InitialLoadDone)
      {
        return;
      }

      if (IsOnline && !service.ServiceInstanceExists)
      {
        ChangeService(service, ChangeType.RemoveByEvent);
      }
      else
      {
        service.StatusChanged += OnServiceStatusChanged;
        service.StatusChangeError += OnServiceStatusChangeError;
        OnServiceListChanged(service, ChangeType.AddByLoad);
      }
    }

    /// <summary>
    /// Fires the <see cref="ServiceStatusChangeError"/> event.
    /// </summary>
    /// <param name="service">Service whose status changed.</param>
    /// <param name="ex">Exception error thrown while trying to change service status.</param>
    private void OnServiceStatusChangeError(MySQLService service, Exception ex)
    {
      if (ServiceStatusChangeError != null)
      {
        ServiceStatusChangeError(this, service, ex);
      }
    }

    /// <summary>
    /// Event delegate method that is fired when a WMI service is created.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="args">Event arguments.</param>
    private void OnWMIServiceCreated(object sender, EventArrivedEventArgs args)
    {
      if (!Settings.Default.AutoAddServicesToMonitor) return;

      ManagementBaseObject serviceObject = ((ManagementBaseObject)args.NewEvent["TargetInstance"]);
      if (serviceObject == null || !serviceObject["Name"].ToString().ToLowerInvariant().Contains(Settings.Default.AutoAddPattern))
      {
        return;
      }

      string serviceName = serviceObject["Name"].ToString().Trim();
      MySQLService service = GetServiceByName(serviceName);
      if (service == null)
      {
        service = new MySQLService(serviceName, Settings.Default.NotifyOfStatusChange, Settings.Default.NotifyOfStatusChange, this);
        service.SetServiceParameters();
        ChangeService(service, ChangeType.AutoAdd);
      }
    }

    /// <summary>
    /// Event delegate method that is fired when a WMI service is deleted.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="args">Event arguments.</param>
    private void OnWMIServiceDeleted(object sender, EventArrivedEventArgs args)
    {
      ManagementBaseObject serviceObject = ((ManagementBaseObject)args.NewEvent["TargetInstance"]);
      if (serviceObject == null)
      {
        return;
      }

      string serviceName = serviceObject["Name"].ToString().Trim();
      MySQLService service = GetServiceByName(serviceName);
      if (service != null)
      {
        ChangeService(service, ChangeType.RemoveByEvent);
      }
    }

    /// <summary>
    /// Event delegate method that is fired when a WMI service status changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="args">Event arguments.</param>
    private void OnWMIServiceStatusChanged(object sender, EventArrivedEventArgs args)
    {
      ManagementBaseObject serviceObject = ((ManagementBaseObject)args.NewEvent["TargetInstance"]);
      if (serviceObject == null)
      {
        return;
      }

      string serviceName = serviceObject["Name"].ToString().Trim();
      string state = serviceObject["State"].ToString().Trim();
      MySQLService service = GetServiceByName(serviceName);
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
        menu.Invoke(new MethodInvoker(() => { ReconnectMenu_Click(sender, e); }));
      }
      else
      {
        TestConnection(true, true);
      }
    }

    /// <summary>
    /// Initializes the background worker used to test connections asynchronously.
    /// </summary>
    private void SetupConnectionTestBackgroundWorker()
    {
      if (_worker == null)
      {
        _worker = new BackgroundWorker();
        _worker.WorkerSupportsCancellation = true;
        _worker.WorkerReportsProgress = false;
        _worker.DoWork += new DoWorkEventHandler(TestConnectionWorkerDoWork);
        _worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(TestConnectionWorkerCompleted);
      }
    }

    /// <summary>
    /// Sets up the WMI events that monitor service deletions and service status changes.
    /// </summary>
    private void SetupWMIEvents()
    {
      try
      {
        if (ConnectionStatus == ConnectionStatusType.Online)
        {
          WMIManagementScope.Connect();
          TimeSpan queryTimeout = TimeSpan.FromSeconds(WMIQueriesTimeoutInSeconds);

          if (_wmiServiceCreationWatcher == null)
          {
            _wmiServiceCreationWatcher = new ManagementEventWatcher(WMIManagementScope, new WqlEventQuery("__InstanceCreationEvent", queryTimeout, WMI_QUERIES_WHERE_CLAUSE));
            _wmiServiceCreationWatcher.EventArrived += OnWMIServiceCreated;
            _wmiServiceCreationWatcher.Start();
          }

          if (_wmiServiceDeletionWatcher == null)
          {
            _wmiServiceDeletionWatcher = new ManagementEventWatcher(WMIManagementScope, new WqlEventQuery("__InstanceDeletionEvent", queryTimeout, WMI_QUERIES_WHERE_CLAUSE));
            _wmiServiceDeletionWatcher.EventArrived += OnWMIServiceDeleted;
            _wmiServiceDeletionWatcher.Start();
          }

          if (_wmiServiceStatusChangeWatcher == null)
          {
            _wmiServiceStatusChangeWatcher = new ManagementEventWatcher(WMIManagementScope, new WqlEventQuery("__InstanceModificationEvent", queryTimeout, WMI_QUERIES_WHERE_CLAUSE));
            _wmiServiceStatusChangeWatcher.EventArrived += OnWMIServiceStatusChanged;
            _wmiServiceStatusChangeWatcher.Start();
          }
        }
        else
        {
          if (_wmiServiceCreationWatcher != null)
          {
            _wmiServiceCreationWatcher.Stop();
            _wmiServiceCreationWatcher.EventArrived -= OnWMIServiceCreated;
          }

          if (_wmiServiceDeletionWatcher != null)
          {
            _wmiServiceDeletionWatcher.Stop();
            _wmiServiceDeletionWatcher.EventArrived -= OnWMIServiceDeleted;
          }

          if (_wmiServiceStatusChangeWatcher != null)
          {
            _wmiServiceStatusChangeWatcher.Stop();
            _wmiServiceStatusChangeWatcher.EventArrived -= OnWMIServiceStatusChanged;
          }
        }
      }
      catch (Exception ex)
      {
        MySQLSourceTrace.WriteAppErrorToLog(ex);
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
    }

    /// <summary>
    /// Delegate method that asynchronously tests the machine's connection status.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void TestConnectionWorkerDoWork(object sender, DoWorkEventArgs e)
    {
      ConnectionStatus = ConnectionStatusType.Connecting;
      ManagementObjectCollection wmiServicesCollection = GetWMIServices((bool)e.Argument);
      ConnectionStatus = ConnectionProblem != ConnectionProblemType.None ? ConnectionStatusType.Unavailable : ConnectionStatusType.Online;
    }
  }
}