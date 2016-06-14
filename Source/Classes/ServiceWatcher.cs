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
using System.ComponentModel;
using System.Management;
using System.Threading;
using MySql.Notifier.Properties;
using MySQL.Utility.Classes;
using MySQL.Utility.Classes.MySQL;
using MySQL.Utility.Forms;

namespace MySql.Notifier.Classes
{
  internal class ServiceWatcher : IDisposable
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
    /// The default timeout in seconds for WMI queries, set to 5 seconds.
    /// </summary>
    public const ushort WMI_QUERIES_DEFAULT_TIMEOUT_IN_SECONDS = 5;

    /// <summary>
    /// The where clause part of WMI queries regarding services.
    /// </summary>
    public const string WMI_QUERIES_WHERE_CLAUSE = "TargetInstance isa 'Win32_Service'";

    #endregion Constants

    #region Fields

    /// <summary>
    /// Asynchronous WMI watcher for creation of services related to this machine.
    /// </summary>
    private ManagementEventWatcher _wmiAsyncCreationWatcher;

    /// <summary>
    /// Asynchronous WMI watcher for deletion of services related to this machine.
    /// </summary>
    private ManagementEventWatcher _wmiAsyncDeletionWatcher;

    /// <summary>
    /// Asynchronous WMI watcher for change of status in services related to this machine.
    /// </summary>
    private ManagementEventWatcher _wmiAsyncStatusChangeWatcher;

    /// <summary>
    /// Semi-synchronous WMI watcher for creation of services related to this machine.
    /// </summary>
    private BackgroundWorker _wmiSemiSyncCreationWatcher;

    /// <summary>
    /// Semi-synchronous WMI watcher for deletion of services related to this machine.
    /// </summary>
    private BackgroundWorker _wmiSemiSyncDeletionWatcher;

    /// <summary>
    /// Semi-synchronous WMI watcher for change of status in services related to this machine.
    /// </summary>
    private BackgroundWorker _wmiSemiSyncStatusChangeWatcher;

    #endregion Fields

    /// <summary>
    /// Instantiates a new instance of the <see cref="ServiceWatcher"/> class.
    /// </summary>
    /// <param name="watchForServiceCreation">Flag indicating whether the watcher will monitor service creation.</param>
    /// <param name="watchForServiceDeletion">Flag indicating whether the watcher will monitor service deletion.</param>
    /// <param name="watchForServiceStatusChange">Flag indicating whether the watcher will monitor service status changes.</param>
    /// <param name="asynchronous">Flag indicating if the watcher uses asnynchronous or semi-synchronous operations.</param>
    /// <param name="isMachineOnline">Flag indicating whether the consumer machine is online.</param>
    public ServiceWatcher(bool watchForServiceCreation, bool watchForServiceDeletion, bool watchForServiceStatusChange, bool asynchronous, bool isMachineOnline)
    {
      _wmiAsyncCreationWatcher = null;
      _wmiAsyncDeletionWatcher = null;
      _wmiAsyncStatusChangeWatcher = null;
      _wmiSemiSyncCreationWatcher = null;
      _wmiSemiSyncDeletionWatcher = null;
      _wmiSemiSyncStatusChangeWatcher = null;
      Asynchronous = asynchronous;
      IsMachineOnline = isMachineOnline;
      IsRunning = false;
      WatchForServiceCreation = watchForServiceCreation;
      WatchForServiceDeletion = watchForServiceDeletion;
      WatchForServiceStatusChange = watchForServiceStatusChange;
      WmiQueriesTimeoutInSeconds = WMI_QUERIES_DEFAULT_TIMEOUT_IN_SECONDS;
    }

    #region Events

    /// <summary>
    /// This event system handles the case where the remote machine is unavailable, and a service has failed to connect to the host.
    /// </summary>
    public delegate void ServiceEventHandler(ManagementBaseObject remoteService);

    /// <summary>
    /// Occurs when a service is created on a remote computer.
    /// </summary>
    public event ServiceEventHandler ServiceCreated;

    /// <summary>
    /// Occurs when a service is created on a remote computer.
    /// </summary>
    public event ServiceEventHandler ServiceDeleted;

    /// <summary>
    /// Occurs when the status of a service on a remote computer changes.
    /// </summary>
    public event ServiceEventHandler ServiceStatusChanged;

    /// <summary>
    /// Occurs when Workbench was installed or uninstall on the local computer.
    /// </summary>
    public event ServiceEventHandler InstallationChanged;

    #endregion Events

    #region Properties

    /// <summary>
    /// Gets a value indicating whether the watcher uses asnynchronous or semi-synchronous operations.
    /// </summary>
    public bool Asynchronous { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether the consumer machine is online.
    /// </summary>
    public bool IsMachineOnline { get; set; }

    /// <summary>
    /// Gets a value indicating whether the watcher is running.
    /// </summary>
    public bool IsRunning { get; private set; }

    /// <summary>
    /// Get or sets a value indicating whether the watcher will monitor service creation.
    /// </summary>
    public bool WatchForServiceCreation { get; private set; }

    /// <summary>
    /// Get or sets a value indicating whether the watcher will monitor service deletion.
    /// </summary>
    public bool WatchForServiceDeletion { get; private set; }

    /// <summary>
    /// Get or sets a value indicating whether the watcher will monitor service status changes.
    /// </summary>
    public bool WatchForServiceStatusChange { get; private set; }

    /// <summary>
    /// Gets or sets the timeout in seconds for WMI queries.
    /// </summary>
    public ushort WmiQueriesTimeoutInSeconds { get; set; }

    #endregion Properties

    /// <summary>
    /// Releases all resources used by the <see cref="Machine"/> class
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Attempts to start the creation, deletion and status change watchers. 
    /// </summary>
    /// <param name="wmiManagementScope">The scope (namespace) used for WMI operations.</param>
    /// <returns>true if watchers were started successfully, false otherwise.</returns>
    public bool Start(ManagementScope wmiManagementScope)
    {
      IsRunning = ServiceCreationWatcherStart(wmiManagementScope) && ServiceDeletionWatcherStart(wmiManagementScope) && ServiceStatusChangeWatcherStart(wmiManagementScope);
      return IsRunning;
    }

    /// <summary>
    /// Attempts to stop the running creation, deletion and status change watchers. 
    /// </summary>
    /// <param name="displayErrors">Flag indicating whether errors are displayed to users or just logged.</param>
    /// <returns>true if watchers were stopped successfully, false otherwise.</returns>
    public bool Stop(bool displayErrors)
    {
      bool success = ServiceCreationWatcherStop(displayErrors) && ServiceDeletionWatcherStop(displayErrors) && ServiceStatusChangeWatcherStop(displayErrors);
      IsRunning = !success;
      return success;
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
        ServiceCreationWatcherStop(false);
        if (_wmiAsyncCreationWatcher != null)
        {
          _wmiAsyncCreationWatcher.Dispose();
        }

        if (_wmiSemiSyncCreationWatcher != null)
        {
          if (_wmiSemiSyncCreationWatcher.IsBusy)
          {
            _wmiSemiSyncCreationWatcher.CancelAsync();
            ushort cancelAsyncWait = 0;
            while (_wmiSemiSyncCreationWatcher.IsBusy && cancelAsyncWait < DEFAULT_CANCEL_ASYNC_WAIT)
            {
              Thread.Sleep(DEFAULT_CANCEL_ASYNC_STEP);
              cancelAsyncWait += DEFAULT_CANCEL_ASYNC_STEP;
            }
          }

          _wmiSemiSyncCreationWatcher.DoWork -= ServiceCreationWatcherStartSemiSyncDoWork;
          _wmiSemiSyncCreationWatcher.RunWorkerCompleted -= ServiceCreationWatcherStartSemiSyncCompleted;
          _wmiSemiSyncCreationWatcher.Dispose();
        }

        ServiceDeletionWatcherStop(false);
        if (_wmiAsyncDeletionWatcher != null)
        {
          _wmiAsyncDeletionWatcher.Dispose();
        }

        if (_wmiSemiSyncDeletionWatcher != null)
        {
          if (_wmiSemiSyncDeletionWatcher.IsBusy)
          {
            _wmiSemiSyncDeletionWatcher.CancelAsync();
            ushort cancelAsyncWait = 0;
            while (_wmiSemiSyncDeletionWatcher.IsBusy && cancelAsyncWait < DEFAULT_CANCEL_ASYNC_WAIT)
            {
              Thread.Sleep(DEFAULT_CANCEL_ASYNC_STEP);
              cancelAsyncWait += DEFAULT_CANCEL_ASYNC_STEP;
            }
          }

          _wmiSemiSyncDeletionWatcher.Dispose();
        }

        ServiceStatusChangeWatcherStop(false);
        if (_wmiAsyncStatusChangeWatcher != null)
        {
          _wmiAsyncStatusChangeWatcher.Dispose();
        }

        if (_wmiSemiSyncStatusChangeWatcher != null)
        {
          if (_wmiSemiSyncStatusChangeWatcher.IsBusy)
          {
            _wmiSemiSyncStatusChangeWatcher.CancelAsync();
            ushort cancelAsyncWait = 0;
            while (_wmiSemiSyncStatusChangeWatcher.IsBusy && cancelAsyncWait < DEFAULT_CANCEL_ASYNC_WAIT)
            {
              Thread.Sleep(DEFAULT_CANCEL_ASYNC_STEP);
              cancelAsyncWait += DEFAULT_CANCEL_ASYNC_STEP;
            }
          }

          _wmiSemiSyncStatusChangeWatcher.Dispose();
        }
      }

      // Add class finalizer if unmanaged resources are added to the class
      // Free unmanaged resources if there are any
    }

    /// <summary>
    /// Fires the <see cref="ServiceCreated"/> event.
    /// </summary>
    /// <param name="remoteService">Remote service firing the creation event.</param>
    private void OnServiceCreated(ManagementBaseObject remoteService)
    {
      if (ServiceCreated != null)
      {
        ServiceCreated(remoteService);
      }
    }

    /// <summary>
    /// Fires the <see cref="ServiceDeleted"/> event.
    /// </summary>
    /// <param name="remoteService">Remote service firing the deletion event.</param>
    private void OnServiceDeleted(ManagementBaseObject remoteService)
    {
      if (ServiceDeleted != null)
      {
        ServiceDeleted(remoteService);
      }
    }

    /// <summary>
    /// Fires the <see cref="ServiceStatusChanged"/> event.
    /// </summary>
    /// <param name="remoteService">Remote service firing the status changed event.</param>
    private void OnServiceStatusChanged(ManagementBaseObject remoteService)
    {
      if (ServiceStatusChanged != null)
      {
        ServiceStatusChanged(remoteService);
        if (remoteService["Name"].ToString().Contains("msiserver"))
        {
          OnInstallationChanged(remoteService);
        }
      }
    }

    /// <summary>
    /// Fires the <see cref="InstallationChanged"/> event.
    /// </summary>
    /// <param name="remoteService">The remote service.</param>
    private void OnInstallationChanged(ManagementBaseObject remoteService)
    {
      if (InstallationChanged != null)
      {
        InstallationChanged(remoteService);
      }
    }

    /// <summary>
    /// Event delegate method that is fired when the an event is fired by the service creation watcher.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ServiceCreationWatcher_EventArrived(object sender, EventArrivedEventArgs e)
    {
      if (e.NewEvent != null)
      {
        OnServiceCreated(e.NewEvent["TargetInstance"] as ManagementBaseObject);
      }
    }

    /// <summary>
    /// Attempts to start the service creation watcher.
    /// </summary>
    /// <param name="wmiManagementScope">The scope (namespace) used for WMI operations.</param>
    /// <returns>true if watcher was started successfull, false otherwise.</returns>
    private bool ServiceCreationWatcherStart(ManagementScope wmiManagementScope)
    {
      bool success = true;
      if (!WatchForServiceCreation)
      {
        return true;
      }

      try
      {
        if (!wmiManagementScope.IsConnected)
        {
          wmiManagementScope.Connect();
        }

        if (Asynchronous)
        {
          _wmiAsyncCreationWatcher = _wmiAsyncCreationWatcher ?? new ManagementEventWatcher(wmiManagementScope, new WqlEventQuery("__InstanceCreationEvent", TimeSpan.FromSeconds(WmiQueriesTimeoutInSeconds), WMI_QUERIES_WHERE_CLAUSE));
          _wmiAsyncCreationWatcher.EventArrived -= ServiceCreationWatcher_EventArrived;
          _wmiAsyncCreationWatcher.EventArrived += ServiceCreationWatcher_EventArrived;
          _wmiAsyncCreationWatcher.Start();
        }
        else
        {
          if (_wmiSemiSyncCreationWatcher == null)
          {
            _wmiSemiSyncCreationWatcher = new BackgroundWorker
            {
              WorkerSupportsCancellation = true,
              WorkerReportsProgress = true
            };
            _wmiSemiSyncCreationWatcher.DoWork += ServiceCreationWatcherStartSemiSyncDoWork;
            _wmiSemiSyncCreationWatcher.ProgressChanged += ServiceCreationWatcherStartSemiSynProgressChanged;
            _wmiSemiSyncCreationWatcher.RunWorkerCompleted += ServiceCreationWatcherStartSemiSyncCompleted;
          }

          if (!_wmiSemiSyncCreationWatcher.IsBusy)
          {
            _wmiSemiSyncCreationWatcher.RunWorkerAsync(wmiManagementScope);
          }
        }
      }
      catch (Exception ex)
      {
        success = false;
        MySqlSourceTrace.WriteAppErrorToLog(ex);
        using (var errorDialog = new InfoDialog(InfoDialogProperties.GetErrorDialogProperties(
          Resources.WMIEventsSubscriptionErrorTitle,
          Resources.WMIEventsSubscriptionErrorDetail,
          null,
          ex.Message)))
        {
          errorDialog.ShowDialog();
        }
      }

      return success;
    }

    /// <summary>
    /// Delegate method that reports the asynchronous operation that monitors semi-sync WMI services creation has completed.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ServiceCreationWatcherStartSemiSyncCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error == null)
      {
        return;
      }

      MySqlSourceTrace.WriteAppErrorToLog(e.Error);
      using (var errorDialog = new InfoDialog(InfoDialogProperties.GetErrorDialogProperties(
        Resources.WMISemiSyncEventsErrorTitle,
        string.Format(Resources.WMISemiSyncEventsErrorDetail, _wmiAsyncCreationWatcher.Scope.Path.Server),
        null,
        e.Error.Message)))
      {
        errorDialog.WordWrapMoreInfo = false;
        errorDialog.DefaultButton = InfoDialog.DefaultButtonType.Button1;
        errorDialog.DefaultButtonTimeout = 30;
        errorDialog.ShowDialog();
      }
    }

    /// <summary>
    /// Delegate method that asynchronously monitors semi-sync WMI services creation.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ServiceCreationWatcherStartSemiSyncDoWork(object sender, DoWorkEventArgs e)
    {
      BackgroundWorker worker = sender as BackgroundWorker;
      Exception throwException = null;
      if (worker != null && worker.CancellationPending)
      {
        e.Cancel = true;
        return;
      }

      try
      {
        int eventCount = 0;
        ManagementScope scope = e.Argument as ManagementScope;
        _wmiAsyncCreationWatcher = _wmiAsyncCreationWatcher ?? new ManagementEventWatcher(scope, new WqlEventQuery("__InstanceCreationEvent", TimeSpan.FromSeconds(WmiQueriesTimeoutInSeconds), WMI_QUERIES_WHERE_CLAUSE));
        while (worker != null && !worker.CancellationPending)
        {
          ManagementBaseObject remoteService = _wmiAsyncCreationWatcher.WaitForNextEvent();
          if (remoteService != null)
          {
            worker.ReportProgress(++eventCount, remoteService);
          }
        }
      }
      catch (Exception ex)
      {
        throwException = ex;
      }

      if (worker != null && worker.CancellationPending)
      {
        e.Cancel = true;
      }

      try
      {
        if (_wmiAsyncCreationWatcher != null)
        {
          _wmiAsyncCreationWatcher.Stop();
          _wmiAsyncCreationWatcher.Dispose();
        }

        _wmiAsyncCreationWatcher = null;
      }
      catch (Exception ex)
      {
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }

      if (throwException != null)
      {
        throw throwException;
      }
    }

    /// <summary>
    /// Delegate method that reports the progress of the operation that monitors semi-sync WMI services creation.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ServiceCreationWatcherStartSemiSynProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      ManagementBaseObject remoteService = e.UserState != null && e.UserState is ManagementBaseObject ? e.UserState as ManagementBaseObject : null;
      if (remoteService != null)
      {
        OnServiceCreated(remoteService["TargetInstance"] as ManagementBaseObject);
      }
    }

    /// <summary>
    /// Attempts to stop the service creation watcher.
    /// </summary>
    /// <param name="displayErrors">Flag indicating whether errors are displayed to users or just logged.</param>
    /// <returns>true if watcher was stopped successfull, false otherwise.</returns>
    private bool ServiceCreationWatcherStop(bool displayErrors)
    {
      bool success = true;
      if (!WatchForServiceCreation && _wmiAsyncCreationWatcher == null)
      {
        return true;
      }

      try
      {
        if (Asynchronous && _wmiAsyncCreationWatcher != null)
        {
          _wmiAsyncCreationWatcher.EventArrived -= ServiceCreationWatcher_EventArrived;
          if (IsMachineOnline)
          {
            _wmiAsyncCreationWatcher.Stop();
          }
        }
        else if (!Asynchronous && _wmiSemiSyncCreationWatcher != null && _wmiSemiSyncCreationWatcher.IsBusy)
        {
          _wmiSemiSyncCreationWatcher.CancelAsync();
          Thread.Sleep(WMI_QUERIES_DEFAULT_TIMEOUT_IN_SECONDS * 1000);
        }
      }
      catch (Exception ex)
      {
        success = false;
        MySqlSourceTrace.WriteAppErrorToLog(ex);
        if (displayErrors)
        {
          using (var errorDialog = new InfoDialog(InfoDialogProperties.GetErrorDialogProperties(
            Resources.WMIEventsSubscriptionErrorTitle,
            Resources.WMIEventsSubscriptionErrorDetail,
            null,
            ex.Message)))
          {
            errorDialog.ShowDialog();
          }
        }
      }

      return success;
    }

    /// <summary>
    /// Event delegate method that is fired when the an event is fired by the service deletion watcher.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ServiceDeletionWatcher_EventArrived(object sender, EventArrivedEventArgs e)
    {
      if (e.NewEvent != null)
      {
        OnServiceDeleted(e.NewEvent["TargetInstance"] as ManagementBaseObject);
      }
    }

    /// <summary>
    /// Attempts to start the service deletion watcher.
    /// </summary>
    /// <param name="wmiManagementScope">The scope (namespace) used for WMI operations.</param>
    /// <returns>true if watcher was started successfull, false otherwise.</returns>
    private bool ServiceDeletionWatcherStart(ManagementScope wmiManagementScope)
    {
      bool success = true;
      if (!WatchForServiceDeletion)
      {
        return true;
      }

      try
      {
        if (!wmiManagementScope.IsConnected)
        {
          wmiManagementScope.Connect();
        }

        if (Asynchronous)
        {
          _wmiAsyncDeletionWatcher = _wmiAsyncDeletionWatcher ?? new ManagementEventWatcher(wmiManagementScope, new WqlEventQuery("__InstanceDeletionEvent", TimeSpan.FromSeconds(WmiQueriesTimeoutInSeconds), WMI_QUERIES_WHERE_CLAUSE));
          _wmiAsyncDeletionWatcher.EventArrived -= ServiceDeletionWatcher_EventArrived;
          _wmiAsyncDeletionWatcher.EventArrived += ServiceDeletionWatcher_EventArrived;
          _wmiAsyncDeletionWatcher.Start();
        }
        else
        {
          if (_wmiSemiSyncDeletionWatcher == null)
          {
            _wmiSemiSyncDeletionWatcher = new BackgroundWorker
            {
              WorkerSupportsCancellation = true,
              WorkerReportsProgress = true
            };
            _wmiSemiSyncDeletionWatcher.DoWork += ServiceDeletionWatcherStartSemiSyncDoWork;
            _wmiSemiSyncDeletionWatcher.ProgressChanged += ServiceDeletionWatcherStartSemiSynProgressChanged;
            _wmiSemiSyncDeletionWatcher.RunWorkerCompleted += ServiceDeletionWatcherStartSemiSyncCompleted;
          }

          if (!_wmiSemiSyncDeletionWatcher.IsBusy)
          {
            _wmiSemiSyncDeletionWatcher.RunWorkerAsync();
          }
        }
      }
      catch (Exception ex)
      {
        success = false;
        MySqlSourceTrace.WriteAppErrorToLog(ex);
        using (var errorDialog = new InfoDialog(InfoDialogProperties.GetErrorDialogProperties(
          Resources.WMIEventsSubscriptionErrorTitle,
          Resources.WMIEventsSubscriptionErrorDetail,
          null,
          ex.Message)))
        {
          errorDialog.ShowDialog();
        }
      }

      return success;
    }

    /// <summary>
    /// Delegate method that reports the asynchronous operation that monitors semi-sync WMI services deletion has completed.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ServiceDeletionWatcherStartSemiSyncCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error == null)
      {
        return;
      }

      MySqlSourceTrace.WriteAppErrorToLog(e.Error);
      using (var errorDialog = new InfoDialog(InfoDialogProperties.GetErrorDialogProperties(
        Resources.WMISemiSyncEventsErrorTitle,
        string.Format(Resources.WMISemiSyncEventsErrorDetail, _wmiAsyncDeletionWatcher.Scope.Path.Server),
        null,
        e.Error.Message)))
      {
        errorDialog.WordWrapMoreInfo = false;
        errorDialog.DefaultButton = InfoDialog.DefaultButtonType.Button1;
        errorDialog.DefaultButtonTimeout = 30;
        errorDialog.ShowDialog();
      }
    }

    /// <summary>
    /// Delegate method that asynchronously monitors semi-sync WMI services deletion.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ServiceDeletionWatcherStartSemiSyncDoWork(object sender, DoWorkEventArgs e)
    {
      BackgroundWorker worker = sender as BackgroundWorker;
      Exception throwException = null;
      if (worker != null && worker.CancellationPending)
      {
        e.Cancel = worker.CancellationPending;
        return;
      }

      try
      {
        int eventCount = 0;
        ManagementScope scope = e.Argument as ManagementScope;
        _wmiAsyncDeletionWatcher = new ManagementEventWatcher(scope, new WqlEventQuery("__InstanceDeletionEvent", TimeSpan.FromSeconds(WmiQueriesTimeoutInSeconds), WMI_QUERIES_WHERE_CLAUSE));
        while (worker != null && !worker.CancellationPending)
        {
          ManagementBaseObject remoteService = _wmiAsyncDeletionWatcher.WaitForNextEvent();
          if (remoteService != null)
          {
            worker.ReportProgress(++eventCount, remoteService);
          }
        }
      }
      catch (Exception ex)
      {
        throwException = ex;
      }

      if (worker != null && worker.CancellationPending)
      {
        e.Cancel = true;
      }

      try
      {
        _wmiAsyncCreationWatcher.Stop();
        _wmiAsyncDeletionWatcher.Dispose();
        _wmiAsyncDeletionWatcher = null;
      }
      catch (Exception ex)
      {
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }

      if (throwException != null)
      {
        throw throwException;
      }
    }

    /// <summary>
    /// Delegate method that reports the progress of the operation that monitors semi-sync WMI services deletion.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ServiceDeletionWatcherStartSemiSynProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      ManagementBaseObject remoteService = e.UserState != null && e.UserState is ManagementBaseObject ? e.UserState as ManagementBaseObject : null;
      if (remoteService != null)
      {
        OnServiceDeleted(remoteService["TargetInstance"] as ManagementBaseObject);
      }
    }

    /// <summary>
    /// Attempts to stop the service deletion watcher.
    /// </summary>
    /// <param name="displayErrors">Flag indicating whether errors are displayed to users or just logged.</param>
    /// <returns>true if watcher was stopped successfull, false otherwise.</returns>
    private bool ServiceDeletionWatcherStop(bool displayErrors)
    {
      bool success = true;
      if (!WatchForServiceDeletion && _wmiAsyncDeletionWatcher == null)
      {
        return true;
      }

      try
      {
        if (Asynchronous && _wmiAsyncDeletionWatcher != null)
        {
          _wmiAsyncDeletionWatcher.EventArrived -= ServiceDeletionWatcher_EventArrived;
          if (IsMachineOnline)
          {
            _wmiAsyncDeletionWatcher.Stop();
          }
        }
        else if (!Asynchronous && _wmiSemiSyncDeletionWatcher != null && _wmiSemiSyncDeletionWatcher.IsBusy)
        {
          _wmiSemiSyncDeletionWatcher.CancelAsync();
          Thread.Sleep(WMI_QUERIES_DEFAULT_TIMEOUT_IN_SECONDS * 1000);
        }
      }
      catch (Exception ex)
      {
        success = false;
        MySqlSourceTrace.WriteAppErrorToLog(ex);
        if (displayErrors)
        {
          using (var errorDialog = new InfoDialog(InfoDialogProperties.GetErrorDialogProperties(
            Resources.WMIEventsSubscriptionErrorTitle,
            Resources.WMIEventsSubscriptionErrorDetail,
            null,
            ex.Message)))
          {
            errorDialog.ShowDialog();
          }
        }
      }

      return success;
    }

    /// <summary>
    /// Event delegate method that is fired when the an event is fired by the service status changed watcher.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ServiceStatusChangeWatcher_EventArrived(object sender, EventArrivedEventArgs e)
    {
      if (e.NewEvent != null)
      {
        OnServiceStatusChanged(e.NewEvent["TargetInstance"] as ManagementBaseObject);
      }
    }

    /// <summary>
    /// Attempts to start the service deletion watcher.
    /// </summary>
    /// <param name="wmiManagementScope">The scope (namespace) used for WMI operations.</param>
    /// <returns>true if watcher was started successfull, false otherwise.</returns>
    private bool ServiceStatusChangeWatcherStart(ManagementScope wmiManagementScope)
    {
      bool success = true;
      if (!WatchForServiceStatusChange)
      {
        return true;
      }

      try
      {
        if (!wmiManagementScope.IsConnected)
        {
          wmiManagementScope.Connect();
        }

        if (Asynchronous)
        {
          var queryTimeout = TimeSpan.FromSeconds(WmiQueriesTimeoutInSeconds);
          _wmiAsyncStatusChangeWatcher = _wmiAsyncStatusChangeWatcher ?? new ManagementEventWatcher(wmiManagementScope, new WqlEventQuery("__InstanceModificationEvent", queryTimeout, WMI_QUERIES_WHERE_CLAUSE));
          _wmiAsyncStatusChangeWatcher.EventArrived += ServiceStatusChangeWatcher_EventArrived;
          _wmiAsyncStatusChangeWatcher.Start();
        }
        else
        {
          if (_wmiSemiSyncStatusChangeWatcher == null)
          {
            _wmiSemiSyncStatusChangeWatcher = new BackgroundWorker
            {
              WorkerSupportsCancellation = true,
              WorkerReportsProgress = true
            };
            _wmiSemiSyncStatusChangeWatcher.DoWork += ServiceStatusChangeWatcherStartSemiSyncDoWork;
            _wmiSemiSyncStatusChangeWatcher.ProgressChanged += ServiceStatusChangeWatcherStartSemiSynProgressChanged;
            _wmiSemiSyncStatusChangeWatcher.RunWorkerCompleted += ServiceStatusChangeWatcherStartSemiSyncCompleted;
          }

          if (!_wmiSemiSyncStatusChangeWatcher.IsBusy)
          {
            _wmiSemiSyncStatusChangeWatcher.RunWorkerAsync(wmiManagementScope);
          }
        }
      }
      catch (Exception ex)
      {
        success = false;
        MySqlSourceTrace.WriteAppErrorToLog(ex);
        using (var errorDialog = new InfoDialog(InfoDialogProperties.GetErrorDialogProperties(
          Resources.WMIEventsSubscriptionErrorTitle,
          Resources.WMIEventsSubscriptionErrorDetail,
          null,
          ex.Message)))
        {
          errorDialog.ShowDialog();
        }
      }

      return success;
    }

    /// <summary>
    /// Delegate method that reports the asynchronous operation that monitors semi-sync WMI services status change has completed.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ServiceStatusChangeWatcherStartSemiSyncCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error == null)
      {
        return;
      }

      MySqlSourceTrace.WriteAppErrorToLog(e.Error);
      using (var errorDialog = new InfoDialog(InfoDialogProperties.GetErrorDialogProperties(
        Resources.WMISemiSyncEventsErrorTitle,
        string.Format(Resources.WMISemiSyncEventsErrorDetail, _wmiAsyncStatusChangeWatcher.Scope.Path.Server),
        null,
        e.Error.Message)))
      {
        errorDialog.WordWrapMoreInfo = false;
        errorDialog.DefaultButton = InfoDialog.DefaultButtonType.Button1;
        errorDialog.DefaultButtonTimeout = 30;
        errorDialog.ShowDialog();
      }
    }

    /// <summary>
    /// Delegate method that asynchronously monitors semi-sync WMI services status change.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ServiceStatusChangeWatcherStartSemiSyncDoWork(object sender, DoWorkEventArgs e)
    {
      BackgroundWorker worker = sender as BackgroundWorker;
      Exception throwException = null;
      if (worker != null && worker.CancellationPending)
      {
        e.Cancel = true;
        return;
      }

      try
      {
        int eventCount = 0;
        ManagementScope scope = e.Argument as ManagementScope;
        _wmiAsyncStatusChangeWatcher = _wmiAsyncStatusChangeWatcher ?? new ManagementEventWatcher(scope, new WqlEventQuery("__InstanceModificationEvent", TimeSpan.FromSeconds(WmiQueriesTimeoutInSeconds), WMI_QUERIES_WHERE_CLAUSE));
        while (worker != null && !worker.CancellationPending)
        {
          ManagementBaseObject remoteService = _wmiAsyncStatusChangeWatcher.WaitForNextEvent();
          if (remoteService != null)
          {
            worker.ReportProgress(++eventCount, remoteService);
          }
        }
      }
      catch (Exception ex)
      {
        throwException = ex;
      }

      if (worker != null && worker.CancellationPending)
      {
        e.Cancel = true;
      }

      try
      {
        _wmiAsyncCreationWatcher.Stop();
        _wmiAsyncCreationWatcher.Dispose();
        _wmiAsyncCreationWatcher = null;
      }
      catch (Exception ex)
      {
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }

      if (throwException != null)
      {
        throw throwException;
      }
    }

    /// <summary>
    /// Delegate method that reports the progress of the operation that monitors semi-sync WMI services status change.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ServiceStatusChangeWatcherStartSemiSynProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      ManagementBaseObject remoteService = e.UserState != null && e.UserState is ManagementBaseObject ? e.UserState as ManagementBaseObject : null;
      if (remoteService != null)
      {
        OnServiceStatusChanged(remoteService["TargetInstance"] as ManagementBaseObject);
      }
    }

    /// <summary>
    /// Attempts to stop the service status change watcher.
    /// </summary>
    /// <param name="displayErrors">Flag indicating whether errors are displayed to users or just logged.</param>
    /// <returns>true if watcher was stopped successfull, false otherwise.</returns>
    private bool ServiceStatusChangeWatcherStop(bool displayErrors)
    {
      bool success = true;
      if (!WatchForServiceStatusChange && _wmiAsyncStatusChangeWatcher == null)
      {
        return true;
      }

      try
      {
        if (Asynchronous && _wmiAsyncStatusChangeWatcher != null)
        {
          _wmiAsyncStatusChangeWatcher.EventArrived -= ServiceStatusChangeWatcher_EventArrived;
          if (IsMachineOnline)
          {
            _wmiAsyncStatusChangeWatcher.Stop();
          }
        }
        else if (!Asynchronous && _wmiSemiSyncStatusChangeWatcher != null && _wmiSemiSyncStatusChangeWatcher.IsBusy)
        {
          _wmiSemiSyncStatusChangeWatcher.CancelAsync();
          Thread.Sleep(WMI_QUERIES_DEFAULT_TIMEOUT_IN_SECONDS * 1000);
        }
      }
      catch (Exception ex)
      {
        success = false;
        MySqlSourceTrace.WriteAppErrorToLog(ex);
        if (displayErrors)
        {
          using (var errorDialog = new InfoDialog(InfoDialogProperties.GetErrorDialogProperties(
            Resources.WMIEventsSubscriptionErrorTitle,
            Resources.WMIEventsSubscriptionErrorDetail,
            null,
            ex.Message)))
          {
            errorDialog.ShowDialog();
          }
        }
      }

      return success;
    }
  }
}