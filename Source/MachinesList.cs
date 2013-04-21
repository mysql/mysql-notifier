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
  using System.Management;
  using System.Linq;
  using MySql.Notifier.Properties;

  /// <summary>
  /// TODO: Refine this class ▼
  /// </summary>
  public class MachinesList
  {
    private bool loading;

    public List<Machine> Machines
    {
      get { return Settings.Default.MachineList; }
      set { Settings.Default.MachineList = value; }
    }

    public MachinesList()
    {
    }

    //TODO: ▼
    /// <summary>
    /// This list will store those machines that were unable to connect for a later re-connect triage
    /// </summary>
    public List<Machine> OfflineMachines { get; set; }

    public void LoadFromSettings()
    {
      loading = true;

      if (Machines == null)
        Machines = new List<Machine>();
      if (Settings.Default.FirstRun)
        AutoAddMachines();
      else
      {
        foreach (Machine machine in Machines)
        {
          //TODO: Check machine connectivity or deny all services here for X time.
          if (machine.Name == "localhost" || machine.Name != null && machine.IsOnline)
          {
            //machine.StatusChanged += new Machine.StatusChangedHandler(machine_StatusChanged);
            OnMachineListChanged(machine, ChangeListChangeType.Add);

            //TODO: ▼ Validate
            machine.LoadServiceParameters();
          }
          else
          {
            // Machines.AddToFailover(machine);
          }
        }
        Settings.Default.Save();
      }

      loading = false;
    }

    private void AutoAddMachines()
    {
      //TODO: ▼ Verify services. If any local MySQL server is installed, auto add localhost machine and call AutoAdd [services] from Machine scope.

      //var services = Service.GetInstances(Settings.Default.AutoAddPattern);

      //foreach (var service in Services)
      //  AddService(service, ChangeListChangeType.AutoAdd);

      //Settings.Default.FirstRun = false;
      //Settings.Default.Save();
    }

    public void ChangeMachine(Machine newMachine, ChangeListChangeType changeType)
    {
      switch (changeType)
      {
        case ChangeListChangeType.Add:

          //// Verify if this is the first machine that would be added to the list, in that case, initialize the list.
          if (Machines == null)
          {
            Machines = new List<Machine>();
          }

          //TODO: ▼ Check if this line is required.
          //newMachine.StatusChanged += new Machine.StatusChangedHandler(machine_StatusChanged);

          //// If Machine already exists we don't need to do anything else here;
          if (MachineIsOnTheList(newMachine))
          {
            return;
          }

          OnMachineListChanged(newMachine, changeType);

          if (newMachine.Name == "localhost")
          {
          Machines.Insert(0,newMachine);
          }
          else
          {
          Machines.Add(newMachine);
          }

          if (!loading)
            Settings.Default.Save();
          break;

        case ChangeListChangeType.AutoAdd:

          //???
          break;

        case ChangeListChangeType.Remove:
          {
            //TODO: Complete Removal functionality, check if machine is empty (of services), etc;
            Machine machineToDelete = null;

            foreach (Machine machine in Machines)
            {
              if (String.Compare(machine.Name, newMachine.Name, true) != 0) continue;
              machineToDelete = machine;

              //DeleteMachineServices(machineToDelete);
              Machines.Remove(machineToDelete);

              break;
            }
            if (machineToDelete == null) return;

            //TODO:
            //OnMachineListChanged(machineToDelete, ChangeListChangeType.Remove);
            //if (!loading)
            //  Settings.Default.Save();
            break;
          }
        default:
          break;
      }
    }

    /// <summary>
    /// GetManagement
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

      Machine newMachine = Machines.FirstOrDefault(m => m.MachineIDMatch(machine.Name, machine.User));

      if (newMachine == null)
      {
        return false;
      }
      else
      {
        return true;
      }
    }

    public Machine GetMachineByID(string name, string user)
    {
      foreach (Machine machine in Machines)
      {
        if (machine.MachineIDMatch(name, user))
        {
          return machine;
        }
      }
      return null;
    }

    public delegate void MachineListChangedHandler(object sender, Machine machine, ChangeListChangeType changeType);

    public event MachineListChangedHandler MachineListChanged;

    protected virtual void OnMachineListChanged(Machine machine, ChangeListChangeType changeType)
    {
      if (MachineListChanged != null)
        MachineListChanged(this, machine, changeType);
    }

    //TODO ▼▼
    //  public delegate void MachineStatusChangedHandler(object sender, MachineStatus args);

    //  /// <summary>
    //  /// Notifies that the status of one of the machines in the list has changed
    //  /// </summary>
    //  public event MachineStatusChangedHandler MachineStatusChanged;

    //  protected virtual void OnMachineStatusChanged(MachineStatus args)
    //  {
    //    if (MachineStatusChanged != null)
    //      MachineStatusChanged(this, args);
    //  }

    //  private void mySQLMachine_StatusChanged(object sender, MachineStatus args)
    //  {
    //    OnMachineStatusChanged(args);
    //  }
    //}

    //public enum MachineStatus
    //{
    //Running,
    //UnableToConnect,
    //lol
    //}
  }

  public enum ChangeListChangeType
  {
    Add,
    AutoAdd,
    Remove
  }
}