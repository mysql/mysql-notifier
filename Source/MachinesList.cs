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
  using System.Linq;
  using System.Management;
  using MySql.Notifier.Properties;
  using MySQL.Utility;

  /// <summary>
  /// This class serves as a manager of machines and its services for the Notifier class
  /// </summary>
  public class MachinesList : IDisposable
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MachinesList"/> class.
    /// </summary>
    public MachinesList()
    {
    }

    #region Events

    /// <summary>
    /// Event handler  method for changes on machines list.
    /// </summary>
    /// <param name="machine">Machine that caused the list change.</param>
    /// <param name="changeType">List change type.</param>
    public delegate void MachineListChangedHandler(Machine machine, ChangeType changeType);

    /// <summary>
    /// Occurs when a machine is added or removed from the machines list.
    /// </summary>
    public event MachineListChangedHandler MachineListChanged;

    /// <summary>
    /// Occurs when services are added or removed from the list of services of a machine in the machines list.
    /// </summary>
    public event Machine.ServiceListChangedHandler MachineServiceListChanged;

    /// <summary>
    /// Occurs when a service in the services list of a machine in the machines list has a status change.
    /// </summary>
    public event Machine.ServiceStatusChangedHandler MachineServiceStatusChanged;

    /// <summary>
    /// Occurs when an error is thrown while attempting to change the status of a service in the services list of a machine in the machines list.
    /// </summary>
    public event Machine.ServiceStatusChangeErrorHandler MachineServiceStatusChangeError;

    /// <summary>
    /// Occurs when the status of a machine in the machines list changes.
    /// </summary>
    public event Machine.MachineStatusChangedHandler MachineStatusChanged;

    #endregion Events

    /// <summary>
    /// Gets or sets a list of all machines saved in the settings file.
    /// </summary>
    public List<Machine> Machines
    {
      get
      {
        return Settings.Default.MachineList;
      }

      set
      {
        Settings.Default.MachineList = value;
      }
    }

    /// <summary>
    /// Gets the number of services monitored from all machines in the machines list.
    /// </summary>
    public int ServicesCount
    {
      get
      {
        return Machines != null ? Machines.Sum(m => m.Services.Count) : 0;
      }
    }

    /// <summary>
    /// Adds or removes machines from the machines list.
    /// </summary>
    /// <param name="machine">Machine to add or delete.</param>
    /// <param name="changeType">List change type.</param>
    public void ChangeMachine(Machine machine, ChangeType changeType)
    {
      switch (changeType)
      {
        case ChangeType.AutoAdd:
        case ChangeType.Add:
          //// Verify if this is the first machine that would be added to the list, in that case, initialize the list.
          if (Machines == null)
          {
            Machines = new List<Machine>();
          }

          //// If Machine already exists we don'_statusChangeTimer need to do anything else here;
          if (MachineIsOnTheList(machine.Name))
          {
            return;
          }
          Machines.Add(machine);
          break;

        case ChangeType.Remove:
          if (machine.Services.Count == 0)
          {
            Machines.Remove(machine);
          }
          break;
      }

      OnMachineListChanged(machine, changeType);
    }

    /// <summary>
    /// Releases all resources used by the <see cref="MachinesList"/> class
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Gets the machine corresponding to the given host name.
    /// </summary>
    /// <param name="name">Host name of the machine.</param>
    /// <returns>The machine matching the given host name.</returns>
    public Machine GetMachineByHostName(string name)
    {
      return Machines.Find(m => string.Compare(m.Name, name, true) == 0);
    }

    /// <summary>
    /// Refreshes the machines list and subscribes to machine events.
    /// </summary>
    public void Refresh()
    {
      if (Machines == null)
      {
        Machines = new List<Machine>();
      }

      if (Settings.Default.FirstRun)
      {
        AutoAddLocalServices();
      }

      foreach (Machine machine in Machines)
      {
        machine.MachineStatusChanged += OnMachineStatusChanged;
        machine.ServiceListChanged += OnMachineServiceListChanged;
        machine.ServiceStatusChanged += OnMachineServiceStatusChanged;
        machine.ServiceStatusChangeError += OnMachineServiceStatusChangeError;
        OnMachineListChanged(machine, ChangeType.Add);
        machine.LoadServicesParameters();
      }
    }

    /// <summary>
    /// Updates the machines timeout to perform an automatic connection test.
    /// </summary>
    public void UpdateMachinesConnectionTimeouts()
    {
      foreach (var machine in Machines)
      {
        machine.SecondsToAutoTestConnection--;
      }
    }

    /// <summary>
    /// Overwrites the credentials of a machine in the machines list with the ones in the given machine as long as the machine names match.
    /// </summary>
    /// <param name="newMachine">Machine with new credentials.</param>
    /// <returns>Machine with overwritten credentials if found with the same name.</returns>
    internal Machine OverwriteMachine(Machine newMachine)
    {
      Machine existingMachine = GetMachineByHostName(newMachine.Name);
      if (existingMachine != null)
      {
        existingMachine.OverwriteCredentials(newMachine.User, newMachine.UnprotectedPassword);
        Settings.Default.Save();
      }

      return existingMachine;
    }

    /// <summary>
    /// Releases all resources used by the <see cref="MachinesList"/> class
    /// </summary>
    /// <param name="disposing">If true this is called by Dispose(), otherwise it is called by the finalizer</param>
    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        //// Free managed resources
        if (Machines != null)
        {
          foreach (Machine machine in Machines)
          {
            if (machine != null)
            {
              machine.Dispose();
            }
          }
        }
      }

      //// Add class finalizer if unmanaged resources are added to the class
      //// Free unmanaged resources if there are any
    }

    /// <summary>
    /// Fires the <see cref="MachineListChanged"/> event.
    /// </summary>
    /// <param name="machine"></param>
    /// <param name="changeType"></param>
    protected virtual void OnMachineListChanged(Machine machine, ChangeType changeType)
    {
      if (MachineListChanged != null)
      {
        MachineListChanged(machine, changeType);
      }
    }

    /// <summary>
    /// Fires the <see cref="MachineServiceListChanged"/> event.
    /// </summary>
    /// <param name="machine">Machine instance.</param>
    /// <param name="service">MySQLService instance.</param>
    /// <param name="changeType">Service list change type.</param>
    protected virtual void OnMachineServiceListChanged(Machine machine, MySQLService service, ChangeType changeType)
    {
      if (MachineServiceListChanged != null)
      {
        MachineServiceListChanged(machine, service, changeType);
      }
    }

    /// <summary>
    /// Fires the <see cref="MachineServiceStatusChanged"/> event.
    /// </summary>
    /// <param name="machine">Machine instance.</param>
    /// <param name="service">MySQLService instance.</param>
    protected virtual void OnMachineServiceStatusChanged(Machine machine, MySQLService service)
    {
      if (MachineServiceStatusChanged != null)
      {
        MachineServiceStatusChanged(machine, service);
      }
    }

    /// <summary>
    /// Fires the <see cref="MachineServiceStatusChangeError"/> event.
    /// </summary>
    /// <param name="machine">Machine instance.</param>
    /// <param name="service">MySQLService instance.</param>
    /// <param name="ex">Exception thrown while trying to change the service's status.</param>
    protected virtual void OnMachineServiceStatusChangeError(Machine machine, MySQLService service, Exception ex)
    {
      if (MachineServiceStatusChangeError != null)
      {
        MachineServiceStatusChangeError(machine, service, ex);
      }
    }

    /// <summary>
    /// Fires the <see cref="MachineStatusChanged"/> event.
    /// </summary>
    /// <param name="machine">Machine instance.</param>
    /// <param name="oldConnectionStatus">Old connection status.</param>
    protected virtual void OnMachineStatusChanged(Machine machine, Machine.ConnectionStatusType oldConnectionStatus)
    {
      if (MachineStatusChanged != null)
      {
        MachineStatusChanged(machine, oldConnectionStatus);
      }
    }

    /// <summary>
    /// Adds the local computer and local services found with a specific pattern specified by users.
    /// </summary>
    private void AutoAddLocalServices()
    {
      //// Verify if MySQL services are present on the local machine
      Machine machine = new Machine();
      var services = new List<ManagementObject>();
      foreach (ManagementObject mo in machine.GetWMIServices(false))
      {
        services.Add(mo);
      }

      services = services.Where(t => t.Properties["DisplayName"].Value.ToString().ToLower().Contains(Settings.Default.AutoAddPattern)).ToList();

      //// If we found some services we will try to add the local machine to the list...
      if (services.Count > 0)
      {
        ChangeMachine(machine, ChangeType.AutoAdd);

        //// Try to add the services we found on it.
        foreach (ManagementObject mo in services)
        {
          MySQLService service = new MySQLService(mo.Properties["Name"].Value.ToString(), true, true, machine);
          service.SetServiceParameters();
          machine.ChangeService(service, ChangeType.AutoAdd);
        }
      }

      Settings.Default.FirstRun = false;
      Settings.Default.Save();
    }

    /// <summary>
    /// Checks if a machine with the given name exists in the list of machines.
    /// </summary>
    /// <param name="machineName">Name of the machine to search in the list of machines.</param>
    /// <returns>true if a machine with the given name exists in the list, false otherwise.</returns>
    private bool MachineIsOnTheList(string machineName)
    {
      if (Machines == null || Machines.Count == 0)
      {
        return false;
      }

      return GetMachineByHostName(machineName) != null;
    }
  }

  /// <summary>
  /// Specifies the type of change that a list suffered.
  /// </summary>
  public enum ChangeType
  {
    /// <summary>
    /// An element was added to the list.
    /// </summary>
    Add,

    /// <summary>
    /// An element has been added to the list automatically in an initial load.
    /// </summary>
    AutoAdd,

    /// <summary>
    /// An element was removed from the list.
    /// </summary>
    Remove
  }
}