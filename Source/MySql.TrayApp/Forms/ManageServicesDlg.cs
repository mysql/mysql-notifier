using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MySql.TrayApp
{
  public partial class ManageServicesDlg : Form
  {

    internal AddServiceDlg addServiceDlg;
    public static string addServiceName;
    public List<String> services;

    public ManageServicesDlg()
    {
      InitializeComponent();
    }


   internal ManageServicesDlg(List<String> monitoredServices)
    {
      InitializeComponent();  
      foreach (var item in monitoredServices)
      {
        ListViewItem itemList = new ListViewItem(item, 0);
        string location;
        string status = MySqlServiceInformation.GetMySqlServiceInformation(item, out location);
        if (string.Compare(status, "Running", StringComparison.InvariantCultureIgnoreCase) == 0)
               itemList.Checked = true;
        itemList.SubItems.Add(location);
        itemList.SubItems.Add(status);    
        lstMonitoredServices.Items.Add(itemList);           
      }
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      if ( addServiceDlg == null)
        addServiceDlg = new AddServiceDlg();
      DialogResult dg = addServiceDlg.ShowDialog();
      if (dg == DialogResult.OK && addServiceName != String.Empty)
      {
        if (lstMonitoredServices.FindItemWithText(addServiceName) != null)
          MessageBox.Show("Selected Service is already in the Monitor List", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        else
        {
          ListViewItem newItem = new ListViewItem(addServiceName, 0);
          string location;
          string status = MySqlServiceInformation.GetMySqlServiceInformation(addServiceName, out location);
          if (string.Compare(status, "Running", StringComparison.InvariantCultureIgnoreCase) == 0)
            newItem.Checked = true;
          newItem.SubItems.Add(location);
          newItem.SubItems.Add(status);         
          lstMonitoredServices.Items.Add(newItem);
        }
      }    
    }


    /// <summary>
    /// Deletes selected service from monitor
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (lstMonitoredServices.SelectedItems.Count > 0 && lstMonitoredServices.SelectedItems[0].Text != String.Empty)
      {
        lstMonitoredServices.SelectedItems[0].Remove();              
      }
    }

    private void bntOK_Click(object sender, EventArgs e)
    {
      services = new List<string>();      
      foreach (ListViewItem item in lstMonitoredServices.Items)      
        services.Add(item.Text);     
    }   
  }
}
