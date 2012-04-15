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
  public partial class AddServiceDlg : Form
  {
    public AddServiceDlg()
    {      
      InitializeComponent();
      try
      {
        lstServices.Items.Clear();
        var services = MySqlServiceInformation.GetMySqlInstances();
        foreach (var item in services)
        {
          ListViewItem newItem = new ListViewItem();
          newItem.Text = item.Properties["DisplayName"].Value.ToString();
          newItem.SubItems.Add(item.Properties["Name"].Value.ToString());
          newItem.SubItems.Add(item.Properties["State"].Value.ToString());

          lstServices.Items.Add(newItem);          
        }

      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
        throw;
      }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (lstServices.SelectedItems.Count > 0 && lstServices.SelectedItems[0].Text != String.Empty)
      {
        ManageServicesDlg.addServiceName = lstServices.SelectedItems[0].SubItems[0].Text;
      }
    }
    
  }
}
