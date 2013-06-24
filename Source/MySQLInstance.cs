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
  using System.Linq;
  using System.Threading;
  using System.Windows.Forms;
  using System.Xml;
  using System.Xml.Serialization;
  using MySql.Data.MySqlClient;
  using MySQL.Utility;

  /// <summary>
  /// A MySQL Server instance that can be reached through a <see cref="MySqlWorkbenchConnection"/>.
  /// </summary>
  [Serializable]
  public class MySQLInstance : INotifyPropertyChanged, IDisposable
  {
    #region Constants

    /// <summary>
    /// Default waiting time in milliseconds to wait for an async cancellation before disposing an object.
    /// </summary>
    public const ushort DEFAULT_CANCEL_ASYNC_WAIT = 5000;

    /// <summary>
    /// Default waiting time in milliseconds for each step of the async cancellation waiting time.
    /// </summary>
    public const ushort DEFAULT_CANCEL_ASYNC_STEP = 1000;

    /// <summary>
    /// Default monitoring interval for a MySQL instance, set to 10.
    /// </summary>
    public const ushort DEFAULT_MONITORING_INTERVAL = 10;

    /// <summary>
    /// Default monitoring interval unit of measures, set to minutes by default.
    /// </summary>
    public const TimeUtilities.IntervalUnitOfMeasure DEFAULT_MONITORING_UOM = TimeUtilities.IntervalUnitOfMeasure.Minutes;

    #endregion Constants

    #region Fields

    /// <summary>
    /// Flag indicating if this instance is being monitored and status changes notified to users.
    /// </summary>
    private bool _monitorAndNotifyStatus;

    /// <summary>
    /// The monitoring interval for this MySQL instance.
    /// </summary>
    private uint _monitoringInterval;

    /// <summary>
    /// The unit of measure used for this instance's monitoring instance.
    /// </summary>
    private TimeUtilities.IntervalUnitOfMeasure _monitoringIntervalUnitOfMeasure;

    /// <summary>
    /// The connection status of the instance before testing the connection for a new status.
    /// </summary>
    private MySqlWorkbenchConnection.ConnectionStatusType _oldInstanceStatus;

    /// <summary>
    /// The list of Workbench connections that connect to this MySQL instance.
    /// </summary>
    private List<MySqlWorkbenchConnection> _relatedConnections;

    /// <summary>
    /// The seconds remaining to the next connection test for the monitoring of this instance.
    /// </summary>
    private double _secondsToMonitorInstance;

    /// <summary>
    /// Flag indicating whether status changes of this instance trigger an update of the tray icon.
    /// </summary>
    private bool _updateTrayIconOnStatusChange;

    /// <summary>
    /// A <see cref="MySqlWorkbenchConnection"/> object with the connection properties for this instance.
    /// </summary>
    private MySqlWorkbenchConnection _workbenchConnection;

    /// <summary>
    /// The Id of the <see cref="MySqlWorkbenchConnection"/> connection.
    /// </summary>
    private string _workbenchConnectionId;

    /// <summary>
    /// Background worker that performs an asynchronous connection test.
    /// </summary>
    private BackgroundWorker _worker;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="MySQLInstance"/> class.
    /// </summary>
    public MySQLInstance()
    {
      _workbenchConnectionId = string.Empty;
      _monitoringInterval = DEFAULT_MONITORING_INTERVAL;
      _monitoringIntervalUnitOfMeasure = DEFAULT_MONITORING_UOM;
      _monitorAndNotifyStatus = true;
      _relatedConnections = null;
      _oldInstanceStatus = MySqlWorkbenchConnection.ConnectionStatusType.Unknown;
      _updateTrayIconOnStatusChange = true;
      _workbenchConnection = null;
      _worker = null;
      InstanceStatusCheckInProgress = false;
      MenuGroup = null;
      RefreshingStatus = false;
      SecondsToMonitorInstance = MonitoringIntervalInSeconds;
      InstanceID = Guid.NewGuid().ToString("B");
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySQLInstance"/> class.
    /// </summary>
    /// <param name="workbenchConnection">A <see cref="MySqlWorkbenchConnection"/> object.</param>
    public MySQLInstance(MySqlWorkbenchConnection workbenchConnection)
      : this()
    {
      WorkbenchConnection = workbenchConnection;
    }

    /// <summary>
    /// Releases all resources used by the <see cref="MySQLInstance"/> class
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
      //// Free managed resources
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

        _worker.DoWork -= CheckInstanceStatusWorkerDoWork;
        _worker.RunWorkerCompleted -= CheckInstanceStatusWorkerCompleted;
        _worker.Dispose();
      }

      //// Add class finalizer if unmanaged resources are added to the class
      //// Free unmanaged resources if there are any
    }

    #region Events

    /// <summary>
    /// Event occurring when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Event handler delegate for the <see cref="InstanceStatusChanged"/> event.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="args">Event arguments.</param>
    public delegate void InstanceStatusChangedEventHandler(object sender, InstanceStatusChangedArgs args);

    /// <summary>
    /// Event ocurring when the status of the current instance changes.
    /// </summary>
    public event InstanceStatusChangedEventHandler InstanceStatusChanged;

    /// <summary>
    /// Event handler delegate for the <see cref="InstanceStatusChanged"/> event.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="args">Event arguments.</param>
    public delegate void InstanceConnectionStatusTestErrorEventHandler(object sender, InstanceConnectionStatusTestErrorThrownArgs args);

    /// <summary>
    /// Event ocurring when an error ocurred during a connection status test.
    /// </summary>
    public event InstanceConnectionStatusTestErrorEventHandler InstanceConnectionStatusTestErrorThrown;

    #endregion Events

    #region Properties

    /// <summary>
    /// Gets a unique instance ID.
    /// </summary>
    [XmlIgnore]
    public string InstanceID { get; private set; }

    /// <summary>
    /// Gets the status of this isntance's connection.
    /// </summary>
    [XmlIgnore]
    public MySqlWorkbenchConnection.ConnectionStatusType ConnectionStatus
    {
      get
      {
        return WorkbenchConnection != null ? WorkbenchConnection.ConnectionStatus : MySqlWorkbenchConnection.ConnectionStatusType.Unknown;
      }
    }

    /// <summary>
    /// Gets a description on the status of this instance's connection.
    /// </summary>
    [XmlIgnore]
    public string ConnectionStatusText
    {
      get
      {
        return WorkbenchConnection != null ? WorkbenchConnection.ConnectionStatusText : string.Empty;
      }
    }

    /// <summary>
    /// Gets or sets the name of the host of this MySQL instance.
    /// </summary>
    [XmlAttribute(AttributeName = "HostName")]
    public string HostName { get; set; }

    /// <summary>
    /// Gets an identifier for this instance composed of the host name and port normally.
    /// </summary>
    [XmlIgnore]
    public string HostIdentifier
    {
      get
      {
        return WorkbenchConnection != null ? WorkbenchConnection.HostIdentifier : string.Empty;
      }
    }

    /// <summary>
    /// Gets a value indicating whether a connection test is still ongoing.
    /// </summary>
    [XmlIgnore]
    public bool InstanceStatusCheckInProgress { get; private set; }

    /// <summary>
    /// Gets the group of ToolStripMenuItem controls for each of the corresponding instance's context menu items.
    /// </summary>
    [XmlIgnore]
    public MySQLInstanceMenuGroup MenuGroup { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is being monitored and status changes notified to users.
    /// </summary>
    [XmlAttribute(AttributeName = "MonitorAndNotifyStatus")]
    public bool MonitorAndNotifyStatus
    {
      get
      {
        return _monitorAndNotifyStatus;
      }

      set
      {
        bool lastValue = _monitorAndNotifyStatus;
        _monitorAndNotifyStatus = value;
        if (lastValue != value)
        {
          SecondsToMonitorInstance = MonitoringIntervalInSeconds;
          OnPropertyChanged("MonitorAndNotifyStatus");
        }
      }
    }

    /// <summary>
    /// Gets or sets the monitoring interval for this MySQL instance.
    /// </summary>
    [XmlAttribute(AttributeName = "MonitoringInterval")]
    public uint MonitoringInterval
    {
      get
      {
        return _monitoringInterval;
      }

      set
      {
        uint lastValue = _monitoringInterval;
        _monitoringInterval = value;
        if (lastValue != value)
        {
          SecondsToMonitorInstance = MonitoringIntervalInSeconds;
          OnPropertyChanged("MonitoringInterval");
        }
      }
    }

    /// <summary>
    /// Gets the monitoring interval in seconds for this MySQL instance.
    /// </summary>
    [XmlIgnore]
    public double MonitoringIntervalInSeconds
    {
      get
      {
        return TimeUtilities.ConvertToSeconds(_monitoringIntervalUnitOfMeasure, _monitoringInterval);
      }
    }

    /// <summary>
    /// Gets or sets the unit of measure used for this instance's monitoring instance.
    /// </summary>
    [XmlAttribute(AttributeName = "MonitoringIntervalUnitOfMeasure")]
    public TimeUtilities.IntervalUnitOfMeasure MonitoringIntervalUnitOfMeasure
    {
      get
      {
        return _monitoringIntervalUnitOfMeasure;
      }

      set
      {
        TimeUtilities.IntervalUnitOfMeasure lastValue = _monitoringIntervalUnitOfMeasure;
        _monitoringIntervalUnitOfMeasure = value;
        if (lastValue != value)
        {
          SecondsToMonitorInstance = MonitoringIntervalInSeconds;
          OnPropertyChanged("MonitoringIntervalUnitOfMeasure");
        }
      }
    }

    /// <summary>
    /// Gets or sets the connection port for this MySQL instance.
    /// </summary>
    [XmlAttribute(AttributeName = "Port")]
    public uint Port { get; set; }

    /// <summary>
    /// Gets a value indicating whether the instance is in the process of refreshing its connection status.
    /// </summary>
    [XmlIgnore]
    public bool RefreshingStatus { get; private set; }

    /// <summary>
    /// Gets the list of Workbench connections that connect to this MySQL instance.
    /// </summary>
    [XmlIgnore]
    public List<MySqlWorkbenchConnection> RelatedConnections
    {
      get
      {
        if (_relatedConnections == null)
        {
          _relatedConnections = new List<MySqlWorkbenchConnection>();
          bool isLocalInstance = MySqlWorkbenchConnection.IsHostLocal(HostName);
          foreach (var workbenchConnection in MySqlWorkbench.Connections)
          {
            if (workbenchConnection.Port != Port)
            {
              continue;
            }

            if (string.IsNullOrEmpty(workbenchConnection.Host) && string.IsNullOrEmpty(HostName))
            {
              _relatedConnections.Add(workbenchConnection);
              continue;
            }

            if ((workbenchConnection.IsLocalConnection && isLocalInstance) || (workbenchConnection.Host.ToLowerInvariant() == HostName.ToLowerInvariant()))
            {
              _relatedConnections.Add(workbenchConnection);
            }
          }
        }

        return _relatedConnections;
      }
    }

    /// <summary>
    /// Gets or sets the seconds remaining to the next connection test for the monitoring of this instance.
    /// </summary>
    [XmlIgnore]
    public double SecondsToMonitorInstance
    {
      get
      {
        return _secondsToMonitorInstance;
      }

      set
      {
        _secondsToMonitorInstance = value;
        if (_secondsToMonitorInstance <= 0)
        {
          CheckInstanceStatus(true);
        }

        OnPropertyChanged("SecondsToMonitorInstance");
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether status changes of this instance trigger an update of the tray icon.
    /// </summary>
    [XmlAttribute(AttributeName = "UpdateTrayIconOnStatusChange")]
    public bool UpdateTrayIconOnStatusChange
    {
      get
      {
        return _updateTrayIconOnStatusChange;
      }

      set
      {
        _updateTrayIconOnStatusChange = value;
        OnPropertyChanged("UpdateTrayIconOnStatusChange");
      }
    }

    /// <summary>
    /// Gets a <see cref="MySqlWorkbenchConnection"/> object with the connection properties for this instance.
    /// </summary>
    [XmlIgnore]
    public MySqlWorkbenchConnection WorkbenchConnection
    {
      get
      {
        return _workbenchConnection;
      }

      set
      {
        _workbenchConnection = value;

        //// If the connection is null maybe it was not found anymore so we fallback to use the first found related connection.
        if (_workbenchConnection == null)
        {
          _workbenchConnection = RelatedConnections.First();
        }

        if (_workbenchConnection != null)
        {
          _workbenchConnectionId = _workbenchConnection.Id;
          HostName = _workbenchConnection.Host;
          Port = _workbenchConnection.Port;
        }

        SetupMenuGroup();
        OnPropertyChanged("WorkbenchConnection");
      }
    }

    /// <summary>
    /// Gets or sets the Id of the <see cref="MySqlWorkbenchConnection"/> connection.
    /// </summary>
    [XmlAttribute(AttributeName = "WorkbenchConnectionId")]
    public string WorkbenchConnectionId
    {
      get
      {
        return _workbenchConnectionId;
      }

      set
      {
        _workbenchConnectionId = value;
        if (!string.IsNullOrEmpty(_workbenchConnectionId))
        {
          WorkbenchConnection = MySqlWorkbench.Connections.FirstOrDefault(conn => conn.Id == _workbenchConnectionId);
        }

        OnPropertyChanged("WorkbenchConnectionId");
      }
    }

    /// <summary>
    /// Gets a <see cref="MySqlWorkbenchServer"/> object related to this instance's Workbench connection.
    /// </summary>
    [XmlIgnore]
    public MySqlWorkbenchServer WorkbenchServer
    {
      get
      {
        return MySqlWorkbench.Servers.Any(s => s.ConnectionId == WorkbenchConnectionId) ? MySqlWorkbench.Servers.First(s => s.ConnectionId == WorkbenchConnectionId) : null;
      }
    }

    #endregion Properties

    /// <summary>
    /// Cancels the asynchronous status check.
    /// </summary>
    /// <returns>true if the background connection test was cancelled, false otherwise</returns>
    public void CancelAsynchronousStatusCheck()
    {
      if (_worker != null && _worker.WorkerSupportsCancellation && (InstanceStatusCheckInProgress || _worker.IsBusy))
      {
        _worker.CancelAsync();
      }
    }

    /// <summary>
    /// Checks if this instance can connect to its corresponding MySQL Server instance with its Workbench connection.
    /// </summary>
    /// <param name="asynchronous">Flag indicating if the status check is run asynchronously or synchronously.</param>
    public void CheckInstanceStatus(bool asynchronous)
    {
      _secondsToMonitorInstance = MonitoringIntervalInSeconds;

      if (WorkbenchConnection == null || InstanceStatusCheckInProgress)
      {
        return;
      }

      InstanceStatusCheckInProgress = true;
      _oldInstanceStatus = ConnectionStatus;

      if (asynchronous)
      {
        SetupInstanceStatusCheckBackgroundWorker();
        _worker.RunWorkerAsync();
      }
      else
      {
        Cursor.Current = Cursors.WaitCursor;
        CheckInstanceStatusWorkerDoWork(this, new DoWorkEventArgs(null));
        CheckInstanceStatusWorkerCompleted(this, new RunWorkerCompletedEventArgs(null, null, false));
        Cursor.Current = Cursors.Default;
      }
    }

    /// <summary>
    /// Refreshes the instance's connection status.
    /// </summary>
    /// <param name="worker"><see cref="BackgroundWorker"/> object in case that the caller means to execute this method in another thread.</param>
    /// <returns>true if the operation was cancelled by the passed background worker, false otherwise.</returns>
    public bool RefreshStatus(ref BackgroundWorker worker)
    {
      //// If user cancells before even checking the connection status, then return.
      if (worker != null && worker.CancellationPending)
      {
        return true;
      }

      MenuGroup.Update(true);
      CheckInstanceStatus(false);
      if (MenuGroup.InstanceMenuItem.Text.EndsWith(Properties.Resources.RefreshingStatusText))
      {
        MenuGroup.Update(false);
      }

      return false;
    }

    /// <summary>
    /// Resets the list of related workbench connections in <see cref="RelatedConnections"/> to be computed again.
    /// </summary>
    public void ResetRelatedWorkbenchConnections()
    {
      _relatedConnections = null;
    }

    /// <summary>
    /// Initializes the instance's menu group.
    /// </summary>
    public void SetupMenuGroup()
    {
      if (MenuGroup == null)
      {
        MenuGroup = new MySQLInstanceMenuGroup(this);
      }
    }

    /// <summary>
    /// Fires the <see cref="InstanceStatusChanged"/> event.
    /// </summary>
    /// <param name="oldInstanceStatus">Old instance status.</param>
    protected virtual void OnInstanceStatusChanged(MySqlWorkbenchConnection.ConnectionStatusType oldInstanceStatus)
    {
      if (InstanceStatusChanged != null)
      {
        InstanceStatusChanged(this, new InstanceStatusChangedArgs(this, oldInstanceStatus));
      }
    }

    /// <summary>
    /// Fires the <see cref="InstanceConnectionStatusTestErrorThrown"/> event.
    /// </summary>
    /// <param name="ex">Exception thrown by a connection status test.</param>
    protected virtual void OnInstanceStatusTestErrorThrown(Exception ex)
    {
      if (InstanceConnectionStatusTestErrorThrown != null)
      {
        InstanceConnectionStatusTestErrorThrown(this, new InstanceConnectionStatusTestErrorThrownArgs(this, ex));
      }
    }

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    protected virtual void OnPropertyChanged(string propertyName)
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    /// <summary>
    /// Delegate method that reports the asynchronous operation to test the instance's connection status has completed.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void CheckInstanceStatusWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (!e.Cancelled && ConnectionStatus != _oldInstanceStatus)
      {
        OnInstanceStatusChanged(_oldInstanceStatus);
      }

      InstanceStatusCheckInProgress = false;
    }

    /// <summary>
    /// Delegate method that asynchronously tests the instance's connection status.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void CheckInstanceStatusWorkerDoWork(object sender, DoWorkEventArgs e)
    {
      BackgroundWorker worker = sender is BackgroundWorker ? sender as BackgroundWorker : null;
      Exception ex;
      WorkbenchConnection.TestConnection(out ex);
      if (worker != null && worker.CancellationPending)
      {
        e.Cancel = true;
        return;
      }

      if (ex != null && ex is MySqlException)
      {
        MySqlException mySqlEx = ex as MySqlException;
        int errorNumber = mySqlEx.Number > 0 ? mySqlEx.Number : (mySqlEx.InnerException != null && mySqlEx.InnerException is MySqlException ? (mySqlEx.InnerException as MySqlException).Number : 0);
        switch (errorNumber)
        {
          case 1042:
            //// Unable to connect to any of the specified MySQL hosts.
            break;

          case 1045:
            //// Authentication to host {0} for user {1} using method {2} failed with message: Access denied for user '{1}'@'{0}' (using password: NO)"}
            break;

          case 0:
            //// No error number
            break;

          default:
            OnInstanceStatusTestErrorThrown(ex);
            break;
        }
      }
    }

    /// <summary>
    /// Initializes the background worker used to check instance status asynchronously.
    /// </summary>
    private void SetupInstanceStatusCheckBackgroundWorker()
    {
      if (_worker == null)
      {
        _worker = new BackgroundWorker();
        _worker.WorkerSupportsCancellation = true;
        _worker.WorkerReportsProgress = false;
        _worker.DoWork += CheckInstanceStatusWorkerDoWork;
        _worker.RunWorkerCompleted += CheckInstanceStatusWorkerCompleted;
      }
    }
  }

  /// <summary>
  /// Provides information for the <see cref="InstanceStatusChanged"/> event.
  /// </summary>
  public class InstanceStatusChangedArgs : EventArgs
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="InstanceStatusChangedArgs"/> class.
    /// </summary>
    /// <param name="instance">MySQL instance whose status changed.</param>
    /// <param name="oldInstanceStatus">Old instance status.</param>
    public InstanceStatusChangedArgs(MySQLInstance instance, MySqlWorkbenchConnection.ConnectionStatusType oldInstanceStatus)
    {
      Instance = instance;
      OldInstanceStatus = oldInstanceStatus;
    }

    /// <summary>
    /// Gets the MySQL instance whose status changed.
    /// </summary>
    public MySQLInstance Instance { get; private set; }

    /// <summary>
    /// Gets the new status of the instance.
    /// </summary>
    public MySqlWorkbenchConnection.ConnectionStatusType NewInstanceStatus
    {
      get
      {
        return Instance.ConnectionStatus;
      }
    }

    /// <summary>
    /// Gets a description on the new status of this connection.
    /// </summary>
    public string NewInstanceStatusText
    {
      get
      {
        return Instance.ConnectionStatusText;
      }
    }

    /// <summary>
    /// Gets the old status of the instance.
    /// </summary>
    public MySqlWorkbenchConnection.ConnectionStatusType OldInstanceStatus { get; private set; }

    /// <summary>
    /// Gets a description on the old status of this connection.
    /// </summary>
    public string OldInstanceStatusText
    {
      get
      {
        return MySqlWorkbenchConnection.GetConnectionStatusDisplayText(OldInstanceStatus);
      }
    }
  }

  /// <summary>
  /// Provides information for the <see cref="InstanceConnectionStatusTestErrorThrown"/> event.
  /// </summary>
  public class InstanceConnectionStatusTestErrorThrownArgs : EventArgs
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="InstanceConnectionStatusTestErrorThrownArgs"/> class.
    /// </summary>
    /// <param name="instance">MySQL instance whose status changed.</param>
    /// <param name="ex">Exception thrown during a connection status test.</param>
    public InstanceConnectionStatusTestErrorThrownArgs(MySQLInstance instance, Exception ex)
    {
      Instance = instance;
      ErrorException = ex;
    }

    /// <summary>
    /// Gets the error Exception thrown during a connection status test.
    /// </summary>
    public Exception ErrorException { get; private set; }

    /// <summary>
    /// Gets the MySQL instance whose status changed.
    /// </summary>
    public MySQLInstance Instance { get; private set; }
  }
}