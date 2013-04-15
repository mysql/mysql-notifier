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
  using System.Text.RegularExpressions;
  using MySql.Notifier.Properties;
  using MySQL.Utility;
  using System.Management;
  using System.Diagnostics;

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
        // we have to manually call our machine list changed event handler since that isn't done with how we are using settings
        var copyofMachines = Machines;
        foreach (Machine machine in copyofMachines)
        {
          //TODO: Check machine connectivity or deny all services here for X time.         
          if (machine.Name != null ) //&& machine.Problem == None)
          {
            //machine.StatusChanged += new Machine.StatusChangedHandler(machine_StatusChanged);
            OnMachineListChanged(machine, ChangeListChangeType.Add);
          }
          //else
          //{
          //  Machines.Disable(machine);
          //}
        }
        Settings.Default.Save();
      }

      loading = false;
    }

    private void AutoAddMachines()
    {
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
          //TODO:
          if (Machines == null)
            Machines = new List<Machine>();
          if (!Machines.Contains(newMachine))
            Machines.Add(newMachine);
          //newMachine.StatusChanged += new Machine.StatusChangedHandler(machine_StatusChanged);          
           OnMachineListChanged(newMachine, changeType);
          //if (!loading)
           Settings.Default.Save();
          break;
        case ChangeListChangeType.AutoAdd:
          //???
          break;
        case ChangeListChangeType.Remove:
          {

            Machine machineToDelete = null;

            foreach (Machine machine in Machines)
            {
              if (String.Compare(machine.Name, newMachine.Name, true) != 0) continue;
              machineToDelete = machine;
              //TODO:
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

    public bool Contains(Machine machine)
    {
      if (Machines == null || Machines.Count == 0) return false;
      foreach (Machine m in Machines)
      {
        if (m == machine)
        {
          return true;
        }
      }
      return false;
    }

    public Machine GetMachineByName(string name)
    {
      foreach (Machine machine in Machines)
        if (String.Compare(machine.Name, name, true) == 0)
        {
          return machine;
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

    /// <summary>
    /// GetManagement
    /// </summary>
    /// <returns></returns>
    public List<ManagementScope> GetManagementScopes()
    {
      List<ManagementScope> managementScopes = new List<ManagementScope>();
      managementScopes.Add(new ManagementScope(@"root\cimv2"));

      foreach (Machine machine in Machines)
      {
        if (machine.IsOnline)
        {
          managementScopes.Add(machine.GetManagementScope());
        }
      }
      return managementScopes;
    }
  }

  public enum ChangeListChangeType
  {
    Add,
    AutoAdd,
    Remove
  }
}