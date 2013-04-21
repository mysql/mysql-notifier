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
  using System.Drawing;
  using System.Linq;
  using System.Windows.Forms;
  using MySql.Notifier.Properties;
  using MySQL.Utility;
  using MySQL.Utility.Forms;

  public partial class ManageItemsDialog : MachineAwareForm
  {
    private MachinesList machinesList;
    public static string addServiceName;
    private object selectedItem;
    private MySQLInstancesList _instancesList;

    public ManageItemsDialog(MySQLInstancesList instancesList, MachinesList machineList)
    {
      InitializeComponent();
      _instancesList = instancesList;
      this.machinesList = machineList;
      RefreshList();
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      Point screenPoint = btnAdd.PointToScreen(new Point(btnAdd.Left, btnAdd.Bottom));
      if (screenPoint.Y + AddButtonContextMenuStrip.Size.Height > Screen.PrimaryScreen.WorkingArea.Height)
      {
        AddButtonContextMenuStrip.Show(btnAdd, new Point(0, -AddButtonContextMenuStrip.Size.Height));
      }
      else
      {
        AddButtonContextMenuStrip.Show(btnAdd, new Point(0, btnAdd.Height));
      }
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      Settings.Default.Save();
    }

    /// <summary>
    /// Deletes selected item from monitored items list
    /// </summary>
    /// <param name="sender">Sender Object</param>
    /// <param name="e">Event Arguments</param>
    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (selectedItem == null)
      {
        return;
      }

      if (selectedItem is MySQLService)
      {
        MySQLService selectedService = selectedItem as MySQLService;

        //TODO: LOCATE MACHINE CLICKED!! ▼ search the right ServicesList on the right machine
        //serviceList.RemoveService(selectedService.ServiceName);
        //RefreshList();
      }
      else if (selectedItem is MySQLInstance)
      {
        MySQLInstance selectedInstance = selectedItem as MySQLInstance;
        if (_instancesList.Remove(selectedInstance))
        {
          RefreshList();
        }
      }
    }

    private void chkUpdateTrayIcon_CheckedChanged(object sender, EventArgs e)
    {
      if (selectedItem == null)
      {
        return;
      }

      if (selectedItem is MySQLService)
      {
        MySQLService selectedService = selectedItem as MySQLService;
        selectedService.UpdateTrayIconOnStatusChange = chkUpdateTrayIcon.Checked;
      }
      else if (selectedItem is MySQLInstance)
      {
        MySQLInstance selectedInstance = selectedItem as MySQLInstance;
        selectedInstance.UpdateTrayIconOnStatusChange = chkUpdateTrayIcon.Checked;
      }
    }

    private void MonitoredItemsListView_SelectedIndexChanged(object sender, EventArgs e)
    {
      btnDelete.Enabled = MonitoredItemsListView.SelectedItems.Count > 0;
      notifyOnStatusChange.Enabled = btnDelete.Enabled;
      chkUpdateTrayIcon.Enabled = btnDelete.Enabled;

      selectedItem = MonitoredItemsListView.SelectedItems.Count > 0 ? MonitoredItemsListView.SelectedItems[0].Tag : null;
      if (selectedItem == null)
      {
        notifyOnStatusChange.Checked = false;
        chkUpdateTrayIcon.Checked = false;
        InstanceMonitorIntervalNumericUpDown.Value = 0;
        InstanceMonitorIntervalNumericUpDown.Enabled = false;
        InstanceMonitorIntervalUOMComboBox.Text = string.Empty;
        InstanceMonitorIntervalUOMComboBox.Enabled = false;
      }
      else if (selectedItem is MySQLService)
      {
        MySQLService service = selectedItem as MySQLService;
        notifyOnStatusChange.Checked = service.NotifyOnStatusChange;
        chkUpdateTrayIcon.Checked = service.UpdateTrayIconOnStatusChange;
        InstanceMonitorIntervalNumericUpDown.Value = 0;
        InstanceMonitorIntervalNumericUpDown.Enabled = false;
        InstanceMonitorIntervalUOMComboBox.Text = string.Empty;
        InstanceMonitorIntervalUOMComboBox.Enabled = false;
      }
      else if (selectedItem is MySQLInstance)
      {
        MySQLInstance instance = selectedItem as MySQLInstance;
        notifyOnStatusChange.Checked = instance.MonitorAndNotifyStatus;
        chkUpdateTrayIcon.Checked = instance.UpdateTrayIconOnStatusChange;
        InstanceMonitorIntervalNumericUpDown.Enabled = true;
        InstanceMonitorIntervalNumericUpDown.Value = instance.MonitoringInterval;
        InstanceMonitorIntervalUOMComboBox.Enabled = true;
        InstanceMonitorIntervalUOMComboBox.SelectedIndex = (int)instance.MonitoringIntervalUnitOfMeasure;
      }
    }

    private void mySQLInstanceToolStripMenuItem_Click(object sender, EventArgs e)
    {
      using (var monitorInstancesDialog = new MonitorMySQLServerInstancesDialog(machinesList, _instancesList))
      {
        if (monitorInstancesDialog.ShowDialog() == DialogResult.OK)
        {
          MySqlWorkbenchConnection selectedConnection = monitorInstancesDialog.SelectedWorkbenchConnection;
          if (selectedConnection != null)
          {
            bool connectionAlreadyInInstance = false;

            //// If the selected connection exists for an already monitored instance but it is not its main connection, replace the main connection with this one.
            foreach (var instance in _instancesList)
            {
              if (instance.RelatedConnections.Exists(conn => conn.Id == selectedConnection.Id) && instance.WorkbenchConnection.Id != selectedConnection.Id)
              {
                instance.WorkbenchConnection = selectedConnection;
                connectionAlreadyInInstance = true;
                break;
              }
            }

            if (!connectionAlreadyInInstance)
            {
              _instancesList.Add(new MySQLInstance(selectedConnection));
            }

            RefreshList();
          }
        }
      }
    }

    private void notifyOnStatusChange_CheckedChanged(object sender, EventArgs e)
    {
      if (selectedItem == null)
      {
        return;
      }

      if (selectedItem is MySQLService)
      {
        MySQLService selectedService = selectedItem as MySQLService;
        selectedService.NotifyOnStatusChange = notifyOnStatusChange.Checked;
      }
      else if (selectedItem is MySQLInstance)
      {
        MySQLInstance selectedInstance = selectedItem as MySQLInstance;
        selectedInstance.MonitorAndNotifyStatus = notifyOnStatusChange.Checked;
      }
    }

    private void RefreshList()
    {
      //// Set cursor to waiting, stop painting of list view to avoid flickering and clear its items.
      Cursor.Current = Cursors.WaitCursor;
      MonitoredItemsListView.BeginUpdate();
      MonitoredItemsListView.Items.Clear();

      //// Add monitored services.
      foreach (Machine machine in machinesList.Machines)
      {
        foreach (MySQLService service in machine.Services)
        {
          if (service.Host == null)
          {
            service.Host = machine;
            service.SetService();
          }
          ListViewItem newItem = new ListViewItem(service.DisplayName);
          newItem.Tag = service;
          newItem.SubItems.Add(service.WinServiceType.ToString());
          newItem.SubItems.Add(service.Status.ToString());
          MonitoredItemsListView.Items.Add(newItem);
        }
      }

      //// Add monitored instances.
      foreach (var instance in _instancesList)
      {
        ListViewItem newItem = new ListViewItem(instance.HostIdentifier);
        newItem.Tag = instance;
        newItem.SubItems.Add(instance.WorkbenchConnection.DriverType.ToString());
        newItem.SubItems.Add(instance.ConnectionStatusText);
        MonitoredItemsListView.Items.Add(newItem);
      }

      //// Select automatically the first itemText or disable controls if no items exist.
      if (MonitoredItemsListView.Items.Count > 0)
      {
        MonitoredItemsListView.Items[0].Selected = true;
      }
      else
      {
        btnDelete.Enabled = false;
        chkUpdateTrayIcon.Enabled = false;
        notifyOnStatusChange.Enabled = false;
        InstanceMonitorIntervalNumericUpDown.Enabled = false;
        InstanceMonitorIntervalUOMComboBox.Enabled = false;
      }

      //// Revert cursor back to normal and paint changes in list.
      MonitoredItemsListView.EndUpdate();
      Cursor.Current = Cursors.Default;
    }

    private void serviceToolStripMenuItem_Click(object sender, EventArgs e)
    {
      using (AddServiceDialog dialog = new AddServiceDialog(newMachine))
      {
        if (dialog.ShowDialog() == DialogResult.OK)
        {
          machinesList.ChangeMachine(dialog.newMachine, ChangeListChangeType.Add);

          newMachine = machinesList.GetMachineByID(dialog.newMachine.Name, dialog.newMachine.User);

          foreach (MySQLService service in dialog.ServicesToAdd)
          {
            if (service.WinServiceType == ServiceMachineType.Remote && newMachine.ContainsService(service))
            {
              InfoDialog.ShowWarningDialog("Warning", "Selected Service is already in the Monitor List");
            }
            else
            {
              newMachine.ChangeService(ChangeListChangeType.Add, service);
            }
          }

          RefreshList();
        }
      }
    }

    private void InstanceMonitorIntervalUOMComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (selectedItem == null || !(selectedItem is MySQLInstance))
      {
        return;
      }

      MySQLInstance selectedInstance = selectedItem as MySQLInstance;
      selectedInstance.MonitoringIntervalUnitOfMeasure = (MySQLInstance.IntervalUnitOfMeasure)InstanceMonitorIntervalUOMComboBox.SelectedIndex;
    }

    private void InstanceMonitorIntervalNumericUpDown_ValueChanged(object sender, EventArgs e)
    {
      if (selectedItem == null || !(selectedItem is MySQLInstance))
      {
        return;
      }

      MySQLInstance selectedInstance = selectedItem as MySQLInstance;
      selectedInstance.MonitoringInterval = (uint)InstanceMonitorIntervalNumericUpDown.Value;
    }
  }
}