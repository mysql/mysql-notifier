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
    private string lastFilter = String.Empty;

    public AddServiceDlg()
    {      
      InitializeComponent();
      server.SelectedIndex = 0;
      filterText.Text = "mysqld.exe";
      RefreshList();
    }

    public string ServiceToAdd { get; private set; }

    private void RefreshList()
    {
      try
      {
        string currentFilter = filter.Checked ? filterText.Text.Trim() : null;
        if (currentFilter == lastFilter) return;

        lastFilter = currentFilter;
        lstServices.Items.Clear();
        var services = MySqlServiceInformation.GetInstances(lastFilter);
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
      ServiceToAdd = lstServices.SelectedItems[0].SubItems[0].Text;
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
      RefreshList();
    }

    private void filter_CheckedChanged(object sender, EventArgs e)
    {
      RefreshList();
    }

    private void lstServices_SelectedIndexChanged(object sender, EventArgs e)
    {
      btnOK.Enabled = lstServices.SelectedItems.Count > 0;
    }
    
  }
}
