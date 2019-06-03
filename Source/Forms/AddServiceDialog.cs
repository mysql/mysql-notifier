// Copyright (c) 2012, 2019, Oracle and/or its affiliates. All rights reserved.
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
using System.Linq;
using System.Management;
using System.Windows.Forms;
using MySql.Notifier.Classes;
using MySql.Notifier.Enumerations;
using MySql.Notifier.Properties;
using MySql.Utility.Classes;
using MySql.Utility.Forms;

namespace MySql.Notifier.Forms
{
  public partial class AddServiceDialog : MachineAwareForm
  {
    #region Fields

    private int _sortColumn;

    #endregion Fields

    public AddServiceDialog(MachinesList machinesList)
    {
      _sortColumn = -1;
      MachinesList = machinesList;
      HasChanges = false;

      InitializeComponent();
      ServicesListView.ColumnClick += ServicesListView_ColumnClick;
      InsertMachinesIntoComboBox();
      MachineSelectionComboBox.SelectedIndex = 0;
    }

    #region Properties

    public bool HasChanges { get; private set; }

    public Machine.LocationType MachineLocationType { get; set; }

    public List<MySqlService> ServicesToAdd { get; set; }

    #endregion Properties

    /// <summary>
    /// Event delegate method fired when the <see cref="DeleteButton"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DeleteButton_Click(object sender, EventArgs e)
    {
      var infoProperties = InfoDialogProperties.GetYesNoDialogProperties(
        InfoDialog.InfoType.Warning,
        Resources.DeleteMachineConfirmationTitle,
        Resources.DeleteMachineConfirmationText);
      infoProperties.CommandAreaProperties.DefaultButton = InfoDialog.DefaultButtonType.Button2;
      infoProperties.CommandAreaProperties.DefaultButtonTimeout = 30;
      var infoResult = InfoDialog.ShowDialog(infoProperties);
      if (infoResult.DialogResult != DialogResult.Yes)
      {
        return;
      }

      HasChanges = true;
      MachinesList.ChangeMachine(NewMachine, ListChangeType.RemoveByUser);
      var removedMachineIndex = MachineSelectionComboBox.SelectedIndex;
      MachineSelectionComboBox.SelectedIndex = 0;
      MachineSelectionComboBox.Items.RemoveAt(removedMachineIndex);
    }

    private void DialogOKButton_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      ServicesToAdd = new List<MySqlService>();
      foreach (ListViewItem lvi in ServicesListView.SelectedItems)
      {
        ServicesToAdd.Add(new MySqlService(lvi.Tag as string, true, true, NewMachine));
      }

      Cursor.Current = Cursors.Default;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="EditButton"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void EditButton_Click(object sender, EventArgs e)
    {
      var oldUser = NewMachine.User;
      var oldPassword = NewMachine.Password;
      using (var windowsConnectionDialog = new WindowsConnectionDialog(MachinesList, NewMachine))
      {
        if (windowsConnectionDialog.ShowDialog() == DialogResult.Cancel)
        {
          return;
        }

        HasChanges = true;
        NewMachine.CopyMachineData(windowsConnectionDialog.NewMachine,
          oldUser != windowsConnectionDialog.NewMachine.User ||
          MySqlSecurity.DecryptPassword(oldPassword) != windowsConnectionDialog.NewMachine.UnprotectedPassword);
        MachinesList.ChangeMachine(NewMachine, ListChangeType.Updated);
        RefreshList();
      }
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
      if (MachinesList?.Machines == null)
      {
        return;
      }

      foreach (var machine in MachinesList.Machines)
      {
        if (machine.IsLocal)
        {
          MachineSelectionComboBox.Items[0] = machine;
        }
        else
        {
          MachineSelectionComboBox.Items.Add(machine);
        }
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="MachineSelectionComboBox"/> selected index changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void MachineSelectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      EditButton.Enabled = MachineSelectionComboBox.SelectedIndex > 2;
      DeleteButton.Enabled = MachineSelectionComboBox.SelectedIndex > 2;
      switch (MachineSelectionComboBox.SelectedIndex)
      {
        case 0:
          MachineLocationType = Machine.LocationType.Local;
          if (MachineSelectionComboBox.SelectedItem is Machine machine)
          {
            NewMachine = machine;
          }
          else
          {
            NewMachine = MachinesList.LocalMachine;
            MachineSelectionComboBox.Items[0] = NewMachine;
          }
          break;

        case 1:
          MachineLocationType = Machine.LocationType.Remote;
          using (var windowsConnectionDialog = new WindowsConnectionDialog(MachinesList, null))
          {
            if (windowsConnectionDialog.ShowDialog() == DialogResult.Cancel)
            {
              MachineSelectionComboBox.SelectedIndex = 0;
            }
            else
            {
              NewMachine = windowsConnectionDialog.NewMachine;
              NewMachine.LoadServicesParameters(false);
              var index = -1;
              for (var machineIndex = 3; machineIndex < MachineSelectionComboBox.Items.Count && index < 0; machineIndex++)
              {
                var machineName = MachineSelectionComboBox.Items[machineIndex].ToString();
                if (machineName == NewMachine.Name)
                {
                  index = machineIndex;
                }
              }

              if (index == -1)
              {
                MachineSelectionComboBox.Items.Add(NewMachine);
                MachineSelectionComboBox.SelectedIndex = MachineSelectionComboBox.Items.Count - 1;
              }
              else
              {
                MachineSelectionComboBox.SelectedIndex = index <= 0 ? 0 : index;
              }
            }
          }
          return;

        case 2:
          if (NewMachine.IsLocal)
          {
            MachineSelectionComboBox.SelectedIndex = 0;
            return;
          }

          var mIndex = -1;
          for (var machineIndex = 3; machineIndex < MachineSelectionComboBox.Items.Count; machineIndex++)
          {
            var machineName = MachineSelectionComboBox.Items[machineIndex].ToString();
            if (machineName == NewMachine.Name)
            {
              mIndex = machineIndex;
              break;
            }
          }

          MachineSelectionComboBox.SelectedIndex = mIndex < 0 ? 0 : mIndex;
          return;

        default:
          MachineLocationType = Machine.LocationType.Remote;
          NewMachine = (Machine)MachineSelectionComboBox.SelectedItem;
          if (!NewMachine.IsOnline)
          {
            var infoProperties = InfoDialogProperties.GetYesNoDialogProperties(
              InfoDialog.InfoType.Warning,
              Resources.MachineUnavailableTitle,
              Resources.MachineUnavailableYesNoDetail,
              null,
              Resources.MachineUnavailableExtendedMessage);
            infoProperties.CommandAreaProperties.DefaultButton = InfoDialog.DefaultButtonType.Button2;
            infoProperties.CommandAreaProperties.DefaultButtonTimeout = 30;
            var infoResult = InfoDialog.ShowDialog(infoProperties);
            if (infoResult.DialogResult == DialogResult.Yes)
            {
              NewMachine.TestConnection(true, false);
            }

            if (!NewMachine.IsOnline)
            {
              ServicesListView.SelectedItems.Clear();
            }
          }
          break;
      }

      ServicesListView.Enabled = NewMachine.IsOnline;
      var servicesMachine = ServicesListView.Tag as Machine;
      if (servicesMachine != NewMachine)
      {
        RefreshList();
      }
    }

    /// <summary>
    /// Regenerates the contents of the services list view.
    /// </summary>
    private void RefreshList()
    {
      // Store the machine used to browse services so we can compare it with the current value in newMachine to know if we need to call RefreshList.
      ServicesListView.Tag = NewMachine;

      ServicesListView.Items.Clear();
      if (NewMachine == null
          || !NewMachine.IsOnline)
      {
        return;
      }

      Cursor = Cursors.WaitCursor;
      try
      {
        var currentFilter = FilterCheckBox.Checked
          ? Settings.Default.AutoAddPattern.Trim()
          : FilterTextBox.Text.ToLower();
        ServicesListView.BeginUpdate();
        var services = new List<ManagementObject>();
        if (NewMachine != null
            && MachineLocationType == Machine.LocationType.Remote)
        {
          var machineServicesCollection = NewMachine.GetWmiServices(true);
          if (machineServicesCollection != null)
          {
            services.AddRange(machineServicesCollection.Cast<ManagementObject>());
          }
        }
        else
        {
          services = Service.GetInstances(string.Empty);
        }

        services = services.OrderBy(x => x.Properties["DisplayName"].Value).ToList();
        if (!string.IsNullOrEmpty(currentFilter))
        {
          services = services.Where(f => f.Properties["DisplayName"].Value.ToString().ToLowerInvariant().Contains(currentFilter)).ToList();
        }

        foreach (var item in services)
        {
          var serviceName = item.Properties["Name"].Value.ToString();
          var displayName = item.Properties["DisplayName"].Value;
          var newItem = new ListViewItem
          {
            Text = displayName != null
              ? displayName.ToString()
              : serviceName,
            Tag = serviceName
          };

          newItem.SubItems.Add(item.Properties["State"].Value.ToString());
          ServicesListView.Items.Add(newItem);
        }
      }
      finally
      {
        ServicesListView.EndUpdate();
        Cursor = Cursors.Default;
      }
    }

    private void ServicesListView_ColumnClick(object sender, ColumnClickEventArgs e)
    {
      if (e.Column != _sortColumn)
      {
        _sortColumn = e.Column;
        ServicesListView.Sorting = SortOrder.Ascending;
      }
      else
      {
        ServicesListView.Sorting = ServicesListView.Sorting == SortOrder.Ascending
          ? SortOrder.Descending
          : SortOrder.Ascending;
      }

      ServicesListView.Sort();
      ServicesListView.ListViewItemSorter = new ListViewItemComparer(e.Column, ServicesListView.Sorting);
    }

    private void timerForFiltering_Tick(object sender, EventArgs e)
    {
      RefreshList();
      timerForFiltering.Enabled = false;
    }

    private class ListViewItemComparer : IComparer
    {
      private readonly int _col;
      private readonly SortOrder _order;

      public ListViewItemComparer(int column, SortOrder order)
      {
        _col = column;
        _order = order;
      }

      public int Compare(object x, object y)
      {
        var xItem = x as ListViewItem;
        var yItem = y as ListViewItem;
        var returnVal = string.CompareOrdinal(xItem?.SubItems[_col].Text, yItem?.SubItems[_col].Text);
        return _order == SortOrder.Descending
          ? returnVal * -1
          : returnVal;
      }
    }
  }
}