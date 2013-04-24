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

  /// <summary>
  /// This class serves as a manager of machines and its services for the Notifier class
  /// </summary>
  public class MachinesList
  {
    private bool loading;

    /// <summary>
    /// List of all machines saved in the settings file.
    /// </summary>
    public List<Machine> Machines
    {
      get { return Settings.Default.MachineList; }
      set { Settings.Default.MachineList = value; }
    }

    /// <summary>
    /// List of machines to store those that were unavailable for connection during Refresh() execution and will queue for a later failover re-connection triage.
    /// </summary>
    public List<Machine> OfflineMachines { get; set; }

    // TODO: ▲ Use this! Implement!!

    public int ServicesCount
    {
      get
      {
        int servicesCount = 0;
        foreach (Machine machine in Machines)
        {
          servicesCount += machine.Services.Count;
        }
        return servicesCount;
      }
    }

    /// <summary>
    /// Default
    /// </summary>
    public MachinesList()
    {
    }

    public void Refresh()
    {
      loading = true;

      if (Machines == null)
        Machines = new List<Machine>();
      if (Settings.Default.FirstRun)
        AutoAddServices();
      foreach (Machine machine in Machines)
      {
        //TODO: Check machine connectivity or deny all services here for X time.
        machine.ServiceListChanged += new Machine.ServiceListChangedHandler(OnCompleteServiceListChanged);
        machine.ServiceStatusChanged += new Machine.ServiceStatusChangedHandler(OnCompleteServiceStatusChanged);
        machine.MachineConnectionError += new Machine.MachineConnectionErrorHandler(OnCompleteMachineConnectionError);
        OnMachineListChanged(machine, ChangeType.Add);
        machine.LoadServiceParameters();

        // TODO: ▼
        //if (machine.Name == "localhost" || machine.Name != null && machine.IsOnline)
        //{
        //
        //}
        //else
        //{
        //  // Machines.AddToFailover(machine);
        //}
      }
      loading = false;
    }

    private void AutoAddServices()
    {
      //// Verify if MySQL services are present on the local machine
      Machine machine = new Machine("localhost");
      var services = new List<ManagementObject>();
      foreach (ManagementObject mo in machine.GetServices())
        services.Add(mo);
      services = services.Where(t => t.Properties["DisplayName"].Value.ToString().ToLower().ToLower().Contains(Settings.Default.AutoAddPattern)).ToList();

      //// If we found some services we will try to add the local machine to the list...
      if (services.Count > 0)
      {
        ChangeMachine(machine, ChangeType.AutoAdd);

        //// ...then we will try to add the services we found on it.
        machine = GetMachineByID("localhost");
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

    public void ChangeMachine(Machine machine, ChangeType changeType)
    {
      switch (changeType)
      {
        case ChangeType.AutoAdd:
          ChangeMachine(machine, ChangeType.Add);
          break;

        case ChangeType.Add:
          //// Verify if this is the first machine that would be added to the list, in that case, initialize the list.
          if (Machines == null)
          {
            Machines = new List<Machine>();
          }

          //// If Machine already exists we don't need to do anything else here;
          if (MachineIsOnTheList(machine)) return;
          Machines.Add(machine);
          break;

        case ChangeType.Remove:
          if (machine.Services.Count == 0)
            Machines.Remove(machine);
          break;

        default:
          break;
      }
      if (!loading)
        Settings.Default.Save();
      OnMachineListChanged(machine, changeType);
    }

    /// <summary>
    /// GetManagement objects
    /// </summary>
    /// <returns></returns>
    public List<ManagementScope> GetManagementScopes()
    {
      List<ManagementScope> managementScopes = new List<ManagementScope>();
      foreach (Machine machine in Machines)
      {
        ManagementScope ms = machine.GetManagementScope();
        if (ms != null)
        {
          managementScopes.Add(machine.GetManagementScope());
        }
      }
      return managementScopes;
    }

    private bool MachineIsOnTheList(Machine machine)
    {
      if (Machines == null || Machines.Count == 0)
      {
        return false;
      }

      Machine newMachine = GetMachineByID(machine.Name);

      if (newMachine == null)
      {
        return false;
      }
      else
      {
        return true;
      }
    }

    public Machine GetMachineByID(string name)
    {
      foreach (Machine machine in Machines)
      {
        if (String.Compare(machine.Name, name, true) == 0)
        {
          return machine;
        }
      }
      return null;
    }

    /// <summary>
    /// Handler of the event for changes on machines list.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="machine"></param>
    /// <param name="changeType"></param>
    public delegate void MachineListChangedHandler(Machine machine, ChangeType changeType);

    public event MachineListChangedHandler MachineListChanged;

    protected virtual void OnMachineListChanged(Machine machine, ChangeType changeType)
    {
      if (MachineListChanged != null)
        MachineListChanged(machine, changeType);
    }

    /// <summary>
    /// Notifies that the status of one of the services has changed. Contains machine information for a complete trace
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void CompleteServiceStatusChangedHandler(Machine machine, MySQLService service, ServiceStatus args);

    public event CompleteServiceStatusChangedHandler CompleteServiceStatusChanged;

    protected virtual void OnCompleteServiceStatusChanged(Machine machine, MySQLService service, ServiceStatus args)
    {
      if (CompleteServiceStatusChanged != null)
        CompleteServiceStatusChanged(machine, service, args);
    }

    /// <summary>
    /// This event system handles the case where the remote machine is unavailable, and a service has failed to connect to the host.
    /// </summary>
    public delegate void CompleteMachineConnectionErrorHandler(Machine machine, MySQLService service, Exception ex);

    public event CompleteMachineConnectionErrorHandler CompleteMachineConnectionError;

    private void OnCompleteMachineConnectionError(Machine machine, MySQLService service, Exception ex)
    {
      if (CompleteMachineConnectionError != null)
        CompleteMachineConnectionError(machine, service, ex);
    }

    /// <summary>
    /// Event handler for changes on current machine services list
    /// </summary>
    /// <param name="sender">Machine instance</param>
    /// <param name="service">MySQLService instance</param>
    /// <param name="changeType">ChangeType</param>
    public delegate void CompleteServiceListChangedHandler(Machine machine, MySQLService service, ChangeType changeType);

    public event CompleteServiceListChangedHandler CompleteServiceListChanged;

    protected virtual void OnCompleteServiceListChanged(Machine machine, MySQLService service, ChangeType changeType)
    {
      if (CompleteServiceListChanged != null)
        CompleteServiceListChanged(machine, service, changeType);
    }
  }

  public enum ChangeType
  {
    Add,
    AutoAdd,
    Remove
  }
}