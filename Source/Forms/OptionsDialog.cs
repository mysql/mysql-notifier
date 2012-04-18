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

      notifyOfAutoAdd.Checked = Settings.Default.NotifyOfAutoServiceAddition;
      notifyOfStatusChange.Checked = Settings.Default.NotifyOfStatusChange;

      chkRunAtStartup.Checked = Settings.Default.RunAtStartup;
      chkAutoCheckUpdates.Checked = Settings.Default.AutoCheckForUpdates;
      numCheckUpdatesWeeks.Value = Settings.Default.CheckForUpdatesFrequency;
      chkEnabledAutoAddServices.Checked = Settings.Default.AutoAddServicesToMonitor;
      autoAddRegex.Text = Settings.Default.AutoAddPattern;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {            
      Settings.Default.NotifyOfAutoServiceAddition = notifyOfAutoAdd.Checked;
      Settings.Default.NotifyOfStatusChange = notifyOfStatusChange.Checked;
      Settings.Default.RunAtStartup = chkRunAtStartup.Checked;
      Settings.Default.AutoCheckForUpdates = chkAutoCheckUpdates.Checked;
      Settings.Default.CheckForUpdatesFrequency = Convert.ToInt32(this.numCheckUpdatesWeeks.Value);
      Settings.Default.AutoAddServicesToMonitor = chkEnabledAutoAddServices.Checked;
      Settings.Default.AutoAddPattern = autoAddRegex.Text.Trim();
      Settings.Default.Save();
    }

    private void chkAutoCheckUpdates_CheckedChanged(object sender, EventArgs e)
    {
      this.numCheckUpdatesWeeks.Enabled = this.chkAutoCheckUpdates.Checked;
    }

    private void chkEnabledAutoAddServices_CheckedChanged(object sender, EventArgs e)
    {
      autoAddRegex.Enabled = chkEnabledAutoAddServices.Checked;
    }
  }
}
