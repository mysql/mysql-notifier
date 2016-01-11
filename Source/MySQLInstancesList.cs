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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MySql.Notifier.Properties;
using MySQL.Utility.Classes.MySQLWorkbench;

namespace MySql.Notifier
{
  /// <summary>
  /// List of <see cref="MySqlInstance"/> objects used to monitor MySQL Server instances.
  /// </summary>
  public class MySqlInstancesList : IList<MySqlInstance>, IDisposable
  {
    #region Fields

    /// <summary>
    /// Flag indicating if the instances are being currently refreshed.
    /// </summary>
    private bool _instancesRefreshing;

    /// <summary>
    /// Dictionary of instance keys and their corresponding instance monitoring timeout values.
    /// </summary>
    private Dictionary<string, double> _instanceMonitoringTimeouts;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlInstancesList"/> class.
    /// </summary>
    public MySqlInstancesList()
    {
      _instancesRefreshing = false;
      InstancesList = Settings.Default.MySQLInstancesList ?? new List<MySqlInstance>();
      RefreshInstances(false);
    }

    /// <summary>
    /// Releases all resources used by the <see cref="MySqlInstancesList"/> class
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases all resources used by the <see cref="MySqlInstancesList"/> class
    /// </summary>
    /// <param name="disposing">If true this is called by Dispose(), otherwise it is called by the finalizer</param>
    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        // Free managed resources
        if (InstancesList != null)
        {
          foreach (MySqlInstance instance in InstancesList.Where(instance => instance != null))
          {
            instance.Dispose();
          }
        }
      }
    }

    #region Events

    /// <summary>
    /// Event handler delegate for the <see cref="InstancesListChanged"/> event.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="args">Event arguments.</param>
    public delegate void InstancesListChangedHandler(object sender, InstancesListChangedArgs args);

    /// <summary>
    /// Event ocurring when the list of instances changes due an addition or removal of instances.
    /// </summary>
    public event InstancesListChangedHandler InstancesListChanged;

    /// <summary>
    /// Event ocurring when the status of the current instance changes.
    /// </summary>
    public event MySqlInstance.InstanceStatusChangedEventHandler InstanceStatusChanged;

    /// <summary>
    /// Event ocurring when an error ocurred during a connection status test.
    /// </summary>
    public event MySqlInstance.InstanceConnectionStatusTestErrorEventHandler InstanceConnectionStatusTestErrorThrown;

    #endregion Events

    #region Properties

    /// <summary>
    /// Gets or sets a list of <see cref="MySqlInstance"/> objects representing instances being monitored.
    /// </summary>
    public List<MySqlInstance> InstancesList { get; private set; }

    #endregion Properties

    #region IList implementation

    /// <summary>
    /// Gets the number of elements actually contained in the list.
    /// </summary>
    public int Count
    {
      get
      {
        return (InstancesList == null) ? 0 : InstancesList.Count;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the collection is read-only
    /// </summary>
    public bool IsReadOnly
    {
      get
      {
        return false;
      }
    }

    /// <summary>
    /// Gets or sets the element at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to get or set.</param>
    /// <returns>A <see cref="MySqlInstance"/> object at the given index position.</returns>
    public MySqlInstance this[int index]
    {
      get
      {
        return InstancesList[index];
      }

      set
      {
        InstancesList[index] = value;
        OnInstancesListChanged(value, ListChangedType.ItemChanged);
      }
    }

    /// <summary>
    /// Adds a <see cref="MySqlInstance"/> object to the end of the list.
    /// </summary>
    /// <param name="item">A <see cref="MySqlInstance"/> object to add.</param>
    public void Add(MySqlInstance item)
    {
      InstancesList.Add(item);
      item.InstanceStatusChanged += SingleInstanceStatusChanged;
      item.PropertyChanged += SingleInstancePropertyChanged;
      item.InstanceConnectionStatusTestErrorThrown += SingleInstanceConnectionStatusTestErrorThrown;
      if (item.ConnectionStatus == MySqlWorkbenchConnection.ConnectionStatusType.Unknown)
      {
        item.CheckInstanceStatus(false);
      }

      SaveToFile();
      OnInstancesListChanged(item, ListChangedType.ItemAdded);
    }

    /// <summary>
    /// Removes all elements from the list.
    /// </summary>
    public void Clear()
    {
      InstancesList.Clear();
      OnInstancesListChanged(null, ListChangedType.Reset);
    }

    /// <summary>
    /// Determines whether an element is in the list.
    /// </summary>
    /// <param name="item">A <see cref="MySqlInstance"/> object.</param>
    /// <returns><c>true</c> if <seealso cref="item"/> is found in the list; otherwise, <c>false</c>.</returns>
    public bool Contains(MySqlInstance item)
    {
      return InstancesList.Contains(item);
    }

    /// <summary>
    /// Copies the entire list to a compatible one-dimensional array, starting at the specified index of the target array.
    /// </summary>
    /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from list. The <see cref="Array"/> must have zero-based indexing.</param>
    /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
    public void CopyTo(MySqlInstance[] array, int arrayIndex)
    {
      InstancesList.CopyTo(array, arrayIndex);
    }

    /// <summary>
    /// Returns an enumerator that iterates through the list.
    /// </summary>
    /// <returns>An <see cref="IEnumerator"/> for the list.</returns>
    public IEnumerator<MySqlInstance> GetEnumerator()
    {
      return InstancesList.GetEnumerator();
    }

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>An <see cref="IEnumerator"/> object that can be used to iterate through the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
      return InstancesList.GetEnumerator();
    }

    /// <summary>
    /// Determines the index of a specific itemText in the list.
    /// </summary>
    /// <param name="item">The <see cref="MySqlInstance"/> object to locate in the list.</param>
    /// <returns>The index of <seealso cref="item"/> if found in the list; otherwise, <c>-1</c>.</returns>
    public int IndexOf(MySqlInstance item)
    {
      return InstancesList.IndexOf(item);
    }

    /// <summary>
    /// Inserts an itemText to the list at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index at which <seealso cref="item"/> should be inserted.</param>
    /// <param name="item">The <see cref="MySqlInstance"/> object to insert to the list.</param>
    public void Insert(int index, MySqlInstance item)
    {
      InstancesList.Insert(index, item);
      item.InstanceStatusChanged += SingleInstanceStatusChanged;
      item.PropertyChanged += SingleInstancePropertyChanged;
      item.InstanceConnectionStatusTestErrorThrown += SingleInstanceConnectionStatusTestErrorThrown;
      SaveToFile();
      OnInstancesListChanged(item, ListChangedType.ItemAdded);
    }

    /// <summary>
    /// Removes the first occurrence of a specific <see cref="MySqlInstance"/> object from the list.
    /// </summary>
    /// <param name="item">The <see cref="MySqlInstance"/> object to remove from the list.</param>
    /// <returns><c>true</c> if <seealso cref="item"/> is successfully removed; otherwise, <c>false</c>. This method also returns <c>false</c> if <seealso cref="item"/> was not found in the list.</returns>
    public bool Remove(MySqlInstance item)
    {
      int index = IndexOf(item);
      bool success = index >= 0;
      if (!success)
      {
        return false;
      }

      try
      {
        RemoveAt(index);
      }
      catch
      {
        success = false;
      }

      return success;
    }

    /// <summary>
    /// Removes the first occurrence of a specific <see cref="MySqlInstance"/> object with the given id from the list.
    /// </summary>
    /// <param name="connectionId">Id if the connection linked to the instance to remove.</param>
    /// <returns><c>true</c> if the instance is successfully removed; otherwise, <c>false</c>. This method also returns <c>false</c> if the instance was not found in the list.</returns>
    public bool Remove(string connectionId)
    {
      int index = InstancesList.FindIndex(ins => ins.WorkbenchConnectionId == connectionId);
      bool success = index >= 0;
      if (!success)
      {
        return false;
      }

      try
      {
        RemoveAt(index);
      }
      catch
      {
        success = false;
      }

      return success;
    }

    /// <summary>
    /// Removes the element at the specified index of the list.
    /// </summary>
    /// <param name="index">The zero-based index of the element to remove.</param>
    public void RemoveAt(int index)
    {
      MySqlInstance instance = InstancesList[index];
      InstancesList.RemoveAt(index);
      SaveToFile();
      OnInstancesListChanged(instance, ListChangedType.ItemDeleted);
    }

    #endregion IList implementation

    /// <summary>
    /// Refreshes the instances list and subscribes to events.
    /// </summary>
    /// <param name="menuGroupRefresh">Flag indicating if instances list on the popup menu needs to be updated .</param>
    public void RefreshInstances(bool menuGroupRefresh)
    {
      // Initial list creation (empty actually).
      _instancesRefreshing = true;
      if (InstancesList.Count == 0)
      {
        return;
      }

      // Initialize the monitor timeouts dictionary.
      if (_instanceMonitoringTimeouts == null)
      {
        _instanceMonitoringTimeouts = new Dictionary<string, double>(InstancesList.Count);
      }
      else
      {
        _instanceMonitoringTimeouts.Clear();
      }

      // Backup the monitor timeouts.
      foreach (var instance in InstancesList)
      {
        _instanceMonitoringTimeouts.Add(instance.WorkbenchConnectionId, instance.SecondsToMonitorInstance);
      }

      for (int instanceIndex = 0; instanceIndex < InstancesList.Count; instanceIndex++)
      {
        // Unsubscribe events as a safeguard.
        var instance = InstancesList[instanceIndex];
        instance.PropertyChanged -= SingleInstancePropertyChanged;
        instance.InstanceStatusChanged -= SingleInstanceStatusChanged;
        instance.InstanceConnectionStatusTestErrorThrown -= SingleInstanceConnectionStatusTestErrorThrown;

        // Remove the instances without a Workbench connection, which means the connection no longer exists.
        if (instance.WorkbenchConnection == null)
        {
          RemoveAt(instanceIndex--);
        }
        else
        {
          MySqlWorkbenchConnection connectionInDisk = MySqlWorkbench.Connections.GetConnectionForId(instance.WorkbenchConnection.Id);
          if (connectionInDisk != null && !instance.WorkbenchConnection.Equals(connectionInDisk))
          {
            instance.WorkbenchConnection.Sync(connectionInDisk, false);
          }
        }

        // Subscribe to instance events.
        instance.PropertyChanged += SingleInstancePropertyChanged;
        instance.InstanceStatusChanged += SingleInstanceStatusChanged;
        instance.InstanceConnectionStatusTestErrorThrown += SingleInstanceConnectionStatusTestErrorThrown;

        // Check the instance's connection status now or restore the monitor timeout if possible.
        if (menuGroupRefresh)
        {
          instance.CheckInstanceStatus(true);
          instance.SetupMenuGroup();
          OnInstancesListChanged(instance, ListChangedType.ItemAdded);
        }
        else
        {
          instance.ResetRelatedWorkbenchConnections();
          instance.MenuGroup.RecreateSqlEditorMenus();
          instance.MenuGroup.Update(false);
          if (_instanceMonitoringTimeouts.ContainsKey(instance.WorkbenchConnectionId))
          {
            instance.SecondsToMonitorInstance = _instanceMonitoringTimeouts[instance.WorkbenchConnectionId];
          }
        }
      }

      _instancesRefreshing = false;
    }

    /// <summary>
    /// Saves the list of instances in the settings.config file.
    /// </summary>
    public void SaveToFile()
    {
      Settings.Default.MySQLInstancesList = InstancesList;
      Settings.Default.Save();
    }

    /// <summary>
    /// Updates instances connection timeouts.
    /// </summary>
    public void UpdateInstancesConnectionTimeouts()
    {
      if (_instancesRefreshing)
      {
        return;
      }

      var monitoredInstances = InstancesList.Where(instance => instance.MonitorAndNotifyStatus);
      foreach (var instance in monitoredInstances)
      {
        instance.SecondsToMonitorInstance--;
      }
    }

    /// <summary>
    /// Fires the <see cref="InstanceStatusChanged"/> event.
    /// </summary>
    /// <param name="instance">MySQL instance that caused the list change.</param>
    /// <param name="listChange">Type of change done to the list.</param>
    protected virtual void OnInstancesListChanged(MySqlInstance instance, ListChangedType listChange)
    {
      if (InstancesListChanged != null)
      {
        InstancesListChanged(this, new InstancesListChangedArgs(instance, listChange));
      }
    }

    /// <summary>
    /// Fires the <see cref="InstanceStatusChanged"/> event.
    /// </summary>
    /// <param name="instance">MySQL instance with a changed status.</param>
    /// <param name="oldInstanceStatus">Old instance status.</param>
    protected virtual void OnInstanceStatusChanged(MySqlInstance instance, MySqlWorkbenchConnection.ConnectionStatusType oldInstanceStatus)
    {
      if (InstanceStatusChanged != null)
      {
        InstanceStatusChanged(this, new InstanceStatusChangedArgs(instance, oldInstanceStatus));
      }
    }

    /// <summary>
    /// Fires the <see cref="InstanceConnectionStatusTestErrorThrown"/> event.
    /// </summary>
    /// <param name="instance">MySQL instance with a changed status.</param>
    /// <param name="ex">Exception thrown by a connection status test.</param>
    protected virtual void OnInstanceConnectionStatusTestErrorThrown(MySqlInstance instance, Exception ex)
    {
      if (InstanceConnectionStatusTestErrorThrown != null)
      {
        InstanceConnectionStatusTestErrorThrown(this, new InstanceConnectionStatusTestErrorThrownArgs(instance, ex));
      }
    }

    /// <summary>
    /// Event delegate method fired when a MySQL instance property value changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="args">Event arguments.</param>
    private void SingleInstancePropertyChanged(object sender, PropertyChangedEventArgs args)
    {
      MySqlInstance instance = sender as MySqlInstance;
      switch (args.PropertyName)
      {
        case "MonitorAndNotifyStatus":
          OnInstancesListChanged(instance, ListChangedType.ItemChanged);
          break;

        case "UpdateTrayIconOnStatusChange":
          OnInstancesListChanged(instance, ListChangedType.ItemChanged);
          break;
      }
    }

    /// <summary>
    /// Event delegate method fired when a MySQL instance status changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="args">Event arguments.</param>
    private void SingleInstanceStatusChanged(object sender, InstanceStatusChangedArgs args)
    {
      OnInstanceStatusChanged(args.Instance, args.OldInstanceStatus);
    }

    /// <summary>
    /// Event delegate method fired when an error is thrown while testing an instance's connection status.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SingleInstanceConnectionStatusTestErrorThrown(object sender, InstanceConnectionStatusTestErrorThrownArgs e)
    {
      OnInstanceConnectionStatusTestErrorThrown(e.Instance, e.ErrorException);
    }
  }

  /// <summary>
  /// Provides information for the InstancesListChanged event.
  /// </summary>
  public class InstancesListChangedArgs : EventArgs
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="InstanceStatusChangedArgs"/> class.
    /// </summary>
    /// <param name="instance">MySQL instance that caused the list change.</param>
    /// <param name="listChange">Type of change done to the list.</param>
    public InstancesListChangedArgs(MySqlInstance instance, ListChangedType listChange)
    {
      Instance = instance;
      ListChange = listChange;
    }

    /// <summary>
    /// Gets the MySQL instance that caused the list change.
    /// </summary>
    public MySqlInstance Instance { get; private set; }

    /// <summary>
    /// Gets the type of change done to the list.
    /// </summary>
    public ListChangedType ListChange { get; private set; }
  }
}