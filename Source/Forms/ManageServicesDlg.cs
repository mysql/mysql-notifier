﻿// 
// Copyright (c) 2012, Oracle and/or its affiliates. All rights reserved.
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceProcess;
using MySql.Notifier.Properties;

namespace MySql.Notifier
{
  public partial class ManageServicesDlg : FormBase
  {
    private MySQLServicesList serviceList;
    public static string addServiceName;
    private MySQLService selectedService;

    public ManageServicesDlg(MySQLServicesList serviceList)
    {
      this.serviceList = serviceList;
      InitializeComponent();
      RefreshList();
    }

    private void RefreshList()
    {
      lstMonitoredServices.Items.Clear();
      foreach (MySQLService service in serviceList.Services)
      {
        ListViewItem itemList = new ListViewItem(service.DisplayName, 0);
        itemList.Tag = service;
        itemList.SubItems.Add(service.Status.ToString());
        lstMonitoredServices.Items.Add(itemList);
      }
      if (lstMonitoredServices.Items.Count > 0)
        lstMonitoredServices.Items[0].Selected = true;
      else
      {
        btnDelete.Enabled = false;
        chkUpdateTrayIcon.Enabled = false;
        notifyOnStatusChange.Enabled = false;
      }
    }

    private void btnAdd_Click(object sender, EventArgs e)  
    {
      AddServiceDlg dlg = new AddServiceDlg();
      if (dlg.ShowDialog() == DialogResult.Cancel) return;

      foreach (string service in dlg.ServicesToAdd)
      {
        if (serviceList.Contains(service))
        {
          using (var errorDialog = new MessageDialog("Warning", "Selected Service is already in the Monitor List", false))
          {
            errorDialog.ShowDialog(this);
          }                    
        }
        else
          serviceList.AddService(service);
      }

      RefreshList();
    }

    /// <summary>
    /// Deletes selected service from monitor
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (selectedService == null) return;

      serviceList.RemoveService(selectedService.ServiceName);
      RefreshList();
    }

    private void lstMonitoredServices_SelectedIndexChanged(object sender, EventArgs e)
    {
      btnDelete.Enabled = lstMonitoredServices.SelectedItems.Count > 0;
      notifyOnStatusChange.Enabled = btnDelete.Enabled;
      chkUpdateTrayIcon.Enabled = btnDelete.Enabled;


      selectedService = lstMonitoredServices.SelectedItems.Count > 0 ? lstMonitoredServices.SelectedItems[0].Tag as MySQLService : null;
      notifyOnStatusChange.Checked = selectedService != null && selectedService.NotifyOnStatusChange;
      chkUpdateTrayIcon.Checked = selectedService != null && selectedService.UpdateTrayIconOnStatusChange;
            
    }

    private void notifyOnStatusChange_CheckedChanged(object sender, EventArgs e)
    {
      if (selectedService == null) return;
      selectedService.NotifyOnStatusChange = notifyOnStatusChange.Checked;      
    }


    private void chkUpdateTrayIcon_CheckedChanged(object sender, EventArgs e)
    {
      if (selectedService == null) return;      
      selectedService.UpdateTrayIconOnStatusChange = chkUpdateTrayIcon.Checked;      
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      Settings.Default.Save();
    }
  }
}
