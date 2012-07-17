using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Notifier.Properties;
using MySQL.Utility;


namespace MySql.Notifier
{
  public partial class OptionsDialog : FormBase
  {
    
    internal OptionsDialog()
    {
      InitializeComponent();     

      notifyOfAutoAdd.Checked = Settings.Default.NotifyOfAutoServiceAddition;
      notifyOfStatusChange.Checked = Settings.Default.NotifyOfStatusChange;
      chkRunAtStartup.Checked = Utility.GetRunAtStartUp(Application.ProductName);
      chkAutoCheckUpdates.Checked = Settings.Default.AutoCheckForUpdates;
      numCheckUpdatesWeeks.Value = Settings.Default.CheckForUpdatesFrequency;
      chkEnabledAutoAddServices.Checked = Settings.Default.AutoAddServicesToMonitor;
      autoAddRegex.Text = Settings.Default.AutoAddPattern;
      chkUseColorfulIcons.Checked = Settings.Default.UseColorfulStatusIcons;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
            
      var updateTask = chkAutoCheckUpdates.Checked != Settings.Default.AutoCheckForUpdates ? true : false;
      var deleteTask = !chkAutoCheckUpdates.Checked && Settings.Default.AutoCheckForUpdates ? true : false;
      var deleteIfPrevious = chkAutoCheckUpdates.Checked && !Settings.Default.AutoCheckForUpdates ? false : true;

      if (Settings.Default.CheckForUpdatesFrequency != Convert.ToInt32(this.numCheckUpdatesWeeks.Value)) updateTask = true;

      Settings.Default.NotifyOfAutoServiceAddition = notifyOfAutoAdd.Checked;
      Settings.Default.NotifyOfStatusChange = notifyOfStatusChange.Checked;
      Settings.Default.AutoCheckForUpdates = chkAutoCheckUpdates.Checked;
      Settings.Default.CheckForUpdatesFrequency = Convert.ToInt32(this.numCheckUpdatesWeeks.Value);
      Settings.Default.AutoAddServicesToMonitor = chkEnabledAutoAddServices.Checked;
      Settings.Default.AutoAddPattern = autoAddRegex.Text.Trim();
      Settings.Default.UseColorfulStatusIcons = chkUseColorfulIcons.Checked;
      Settings.Default.Save();
      Utility.SetRunAtStartUp(Application.ProductName, chkRunAtStartup.Checked);


      if (updateTask)
      {

        if (Settings.Default.AutoCheckForUpdates)
        {
          if (!String.IsNullOrEmpty(Utility.GetInstallLocation("MySQL Notifier")))
          {
            Utility.CreateScheduledTask("MySQLNotifierTask", Utility.GetInstallLocation("MySQL Notifier") + "MySqlNotifier.exe", "--c", Settings.Default.CheckForUpdatesFrequency, deleteIfPrevious);
          }
        }
        if (deleteTask)
          Utility.DeleteScheduledTask("MySQLNotifierTask");
      }
      
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
