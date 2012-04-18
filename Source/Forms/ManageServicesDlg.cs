using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceProcess;

namespace MySql.TrayApp
{
  public partial class ManageServicesDlg : Form
  {
    private MySQLServicesList serviceList;
    public static string addServiceName;

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
        ListViewItem itemList = new ListViewItem(service.ServiceName, 0);
        itemList.SubItems.Add(service.Status.ToString());
        itemList.Checked = service.Status == ServiceControllerStatus.Running;
        lstMonitoredServices.Items.Add(itemList);
      }
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      AddServiceDlg dlg = new AddServiceDlg();
      if (dlg.ShowDialog() == DialogResult.Cancel) return;

      if (serviceList.Contains(dlg.ServiceToAdd))
      {
        MessageBox.Show("Selected Service is already in the Monitor List", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      }

      serviceList.AddService(dlg.ServiceToAdd);
      RefreshList();
    }

    /// <summary>
    /// Deletes selected service from monitor
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnDelete_Click(object sender, EventArgs e)
    {
      serviceList.RemoveService(lstMonitoredServices.SelectedItems[0].SubItems[0].Text);
      RefreshList();
    }

    private void lstMonitoredServices_SelectedIndexChanged(object sender, EventArgs e)
    {
      btnDelete.Enabled = lstMonitoredServices.SelectedItems.Count > 0;
    }
  }
}
