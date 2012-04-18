using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.TrayApp.Properties;

namespace MySql.TrayApp
{
  public partial class OptionsDialog : Form
  {
    
    internal OptionsDialog()
    {
      InitializeComponent();

      chkNotifyChanges.Checked = Settings.Default.NotifyOfServiceAddition;

      chkRunAtStartup.Checked = Properties.Settings.Default.RunAtStartup;
      chkAutoCheckUpdates.Checked = Properties.Settings.Default.AutoCheckForUpdates;
      numCheckUpdatesWeeks.Value = Properties.Settings.Default.CheckForUpdatesFrequency;
      chkEnabledAutoAddServices.Checked = Properties.Settings.Default.AutoAddServicesToMonitor;
    
    }

    private void btnOK_Click(object sender, EventArgs e)
    {            
      Settings.Default.NotifyOfServiceAddition = chkNotifyChanges.Checked;      
      Settings.Default.RunAtStartup = chkRunAtStartup.Checked;
      Settings.Default.AutoCheckForUpdates = chkAutoCheckUpdates.Checked;
      Settings.Default.CheckForUpdatesFrequency = Convert.ToInt32(this.numCheckUpdatesWeeks.Value);
      Settings.Default.AutoAddServicesToMonitor = chkEnabledAutoAddServices.Checked;
      Settings.Default.Save();
    }

    private void chkAutoCheckUpdates_CheckedChanged(object sender, EventArgs e)
    {
      this.numCheckUpdatesWeeks.Enabled = this.chkAutoCheckUpdates.Checked;
    }
  }
}
