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
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using System.Management;
  using System.Windows.Forms;
  using MySql.Notifier.Properties;
  using MySQL.Utility;
  using MySQL.Utility.Forms;

  public partial class AddServiceDialog : MachineAwareForm
  {
    private string lastFilter;
    private string lastTextFilter;
    private int sortColumn;
    private bool machineValuesChanged;

    public AddServiceDialog(MachinesList machineslist, Machine machine)
    {
      lastFilter = string.Empty;
      lastTextFilter = string.Empty;
      sortColumn = -1;
      machineValuesChanged = false;

      InitializeComponent();
      MachineSelectionComboBox.SelectedIndex = 0;
      ServicesListView.ColumnClick += new ColumnClickEventHandler(ServicesListView_ColumnClick);
      newMachine = machine ?? newMachine;
      machinesList = machineslist;
      InsertMachinesIntoComboBox();
    }

    public Machine.LocationType MachineLocationType { get; set; }

    public List<MySQLService> ServicesToAdd { get; set; }

    private void DialogOKButton_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      ServicesToAdd = new List<MySQLService>();
      foreach (ListViewItem lvi in ServicesListView.SelectedItems)
      {
        ServicesToAdd.Add(new MySQLService(lvi.Tag as string, true, true, newMachine));
      }

      if (machineValuesChanged)
      {
        Settings.Default.Save();
        machineValuesChanged = false;
      }
      
      Cursor.Current = Cursors.Default;
    }

    private void FilterCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      RefreshList();
    }

    private void FilterTextBox_TextChanged(object sender, EventArgs e)
    {
      if (!timerForFiltering.Enabled)
      {
        timerForFiltering.Enabled = true;
      }
    }

    private void InsertMachinesIntoComboBox()
    {
      if (machinesList == null) return;
      if (machinesList.Machines == null) return;

      foreach (Machine machine in machinesList.Machines)
      {
        if (machine.Name != "localhost")
        {
          MachineSelectionComboBox.Items.Add(machine);
        }
      }
    }

    private void MachineAutoTestConnectionIntervalNumericUpDown_ValueChanged(object sender, EventArgs e)
    {
      if (newMachine != null)
      {
        newMachine.AutoTestConnectionInterval = (uint)MachineAutoTestConnectionIntervalNumericUpDown.Value;
        machineValuesChanged = true;
      }
    }

    private void MachineAutoTestConnectionIntervalUOMComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (newMachine != null)
      {
        newMachine.AutoTestConnectionIntervalUnitOfMeasure = (TimeUtilities.IntervalUnitOfMeasure)MachineAutoTestConnectionIntervalUOMComboBox.SelectedIndex;
        machineValuesChanged = true;
      }
    }

    private void MachineSelectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      bool refreshList = true;
      DialogResult dr = DialogResult.None;
      switch (MachineSelectionComboBox.SelectedIndex)
      {
        case 0:
          MachineLocationType = Machine.LocationType.Local;
          if (newMachine != null && newMachine.IsLocal)
          {
            refreshList = false;
            break;
          }

          newMachine = new Machine();
          break;

        case 1:
          MachineLocationType = Machine.LocationType.Remote;
          using (var windowsConnectionDialog = new WindowsConnectionDialog(machinesList, newMachine))
          {
            dr = windowsConnectionDialog.ShowDialog();
            if (dr == DialogResult.Cancel)
            {
              MachineSelectionComboBox.SelectedIndex = 0;
              break;
            }
            else
            {
              newMachine = windowsConnectionDialog.newMachine;
              int index = -1;
              for (int machineIndex = 3; machineIndex < MachineSelectionComboBox.Items.Count && index < 0; machineIndex++)
              {
                string machineName = MachineSelectionComboBox.Items[machineIndex].ToString();
                if (machineName == newMachine.Name)
                {
                  index = machineIndex;
                }
              }

              if (index == -1)
              {
                MachineSelectionComboBox.Items.Add(newMachine);
                MachineSelectionComboBox.SelectedIndex = (MachineSelectionComboBox.Items.Count - 1);
              }
              else
              {
                MachineSelectionComboBox.SelectedIndex = index <= 0 ? 0 : index;
              }
            }
          }
          break;

        case 2:
          if (newMachine.IsLocal)
          {
            refreshList = false;
            MachineSelectionComboBox.SelectedIndex = 0;
            break;
          }

          for (int machineIndex = 3; machineIndex < MachineSelectionComboBox.Items.Count; machineIndex++)
          {
            string machineName = MachineSelectionComboBox.Items[machineIndex].ToString();
            if (machineName == newMachine.Name)
            {
              refreshList = false;
              break;
            }

            MachineSelectionComboBox.SelectedIndex = machineIndex >= MachineSelectionComboBox.Items.Count ? 0 : machineIndex;
          }
          break;

        default:
          Cursor.Current = Cursors.WaitCursor;
          MachineLocationType = Machine.LocationType.Remote;
          newMachine = (Machine)MachineSelectionComboBox.SelectedItem;
          if (!newMachine.IsOnline)
          {
            InfoDialog.ShowInformationDialog(Resources.HostUnavailableTitle, Resources.HostUnavailableMessage);
            ServicesListView.SelectedItems.Clear();
          }

          Cursor.Current = Cursors.Default;
          break;
      }

      SetMachineAutoTestConnectionControlsAvailability();
      ServicesListView.Enabled = newMachine.IsOnline;
      if (refreshList)
      {
        RefreshList();
      }
    }

    private void RefreshList()
    {
      ServicesListView.Items.Clear();
      if (newMachine == null || !newMachine.IsOnline)
      {
        return;
      }

      string currentFilter = FilterCheckBox.Checked ? Settings.Default.AutoAddPattern.Trim() : null;

      //TODO: Restore filtert persistance ▼
      //if (currentFilter == lastFilter && txtFilter.Text == lastTextFilter) return;

      ServicesListView.BeginUpdate();
      lastFilter = currentFilter;
      lastTextFilter = FilterTextBox.Text.ToLower();
      List<ManagementObject> services = new List<ManagementObject>();
      if (newMachine != null && MachineLocationType == Machine.LocationType.Remote)
      {
        var machineServicesCollection = newMachine.GetWMIServices(true);
        if (machineServicesCollection != null)
        {
          foreach (ManagementObject mo in machineServicesCollection)
          {
            services.Add(mo);
          }
        }
      }
      else
      {
        services = Service.GetInstances(String.Empty);
      }

      services = services.OrderBy(x => x.Properties["DisplayName"].Value).ToList();
      if (!string.IsNullOrEmpty(lastTextFilter))
      {
        services = services.Where(f => f.Properties["DisplayName"].Value.ToString().ToLower().Contains(lastTextFilter)).ToList();
      }

      foreach (ManagementObject item in services)
      {
        ListViewItem newItem = new ListViewItem();
        newItem.Text = item.Properties["DisplayName"].Value.ToString();
        newItem.Tag = item.Properties["Name"].Value.ToString();
        newItem.SubItems.Add(item.Properties["State"].Value.ToString());
        ServicesListView.Items.Add(newItem);
      }

      ServicesListView.EndUpdate();
    }

    private void ServicesListView_ColumnClick(object sender, ColumnClickEventArgs e)
    {
      if (e.Column != sortColumn)
      {
        sortColumn = e.Column;
        ServicesListView.Sorting = SortOrder.Ascending;
      }
      else
      {
        if (ServicesListView.Sorting == SortOrder.Ascending)
          ServicesListView.Sorting = SortOrder.Descending;
        else
          ServicesListView.Sorting = SortOrder.Ascending;
      }

      ServicesListView.Sort();
      ServicesListView.ListViewItemSorter = new ListViewItemComparer(e.Column, ServicesListView.Sorting);
    }

    private void SetMachineAutoTestConnectionControlsAvailability()
    {
      MachineAutoTestConnectionIntervalNumericUpDown.Value = MachineSelectionComboBox.SelectedIndex > 2 ? newMachine.AutoTestConnectionInterval : 0;
      MachineAutoTestConnectionIntervalNumericUpDown.Enabled = MachineSelectionComboBox.SelectedIndex > 2;
      MachineAutoTestConnectionIntervalUOMComboBox.Enabled = MachineSelectionComboBox.SelectedIndex > 2;
      if (MachineSelectionComboBox.SelectedIndex > 2)
      {
        MachineAutoTestConnectionIntervalUOMComboBox.SelectedIndex = (int)newMachine.AutoTestConnectionIntervalUnitOfMeasure;
      }
      else
      {
        MachineAutoTestConnectionIntervalUOMComboBox.Text = string.Empty;
      }
    }

    private void timerForFiltering_Tick(object sender, EventArgs e)
    {
      RefreshList();
      timerForFiltering.Enabled = false;
    }

    private class ListViewItemComparer : IComparer
    {
      private int col;
      private SortOrder order;

      public ListViewItemComparer()
      {
        col = 0;
        order = SortOrder.Ascending;
      }

      public ListViewItemComparer(int column, SortOrder order)
      {
        col = column;
        this.order = order;
      }

      public int Compare(object x, object y)
      {
        int returnVal = -1;

        returnVal = String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);

        if (order == SortOrder.Descending)
          returnVal *= -1;

        return returnVal;
      }
    }
  }
}