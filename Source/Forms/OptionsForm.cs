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
  public partial class OptionsForm : Form
  {
    
    internal OptionsForm(TrayAppSettings appSettingsValues)
    {
      InitializeComponent();

      chkEnableAutoRefresh.Checked = Properties.Settings.Default.EnableAutoRefresh;
      chkNotifyChanges.Checked = Properties.Settings.Default.AutoRefreshNotifyChanges;

      if (appSettingsValues.autoRefreshType == RefreshTypeEnum.OnDemand)
      {
         radOnDemand.Checked = true;
         radByTimer.Checked = false;
      }
      else
      {
         radOnDemand.Checked = false;
         radByTimer.Checked = true;
         numAutoRefreshFrequency.Value = Properties.Settings.Default.AutoRefreshFrequency;
      }
      
      chkRunAtStartup.Checked = Properties.Settings.Default.RunAtStartup;
      chkAutoCheckUpdates.Checked = Properties.Settings.Default.AutoCheckForUpdates;
      numCheckUpdatesWeeks.Value = Properties.Settings.Default.CheckForUpdatesFrequency;
      chkEnabledAutoAddServices.Checked = Properties.Settings.Default.AutoAddServicesToMonitor;
    
    }

    private void OptionsForm_Load(object sender, EventArgs e)
    {
       //all settings are loaded at the constructor 
    }

    private void btnOK_Click(object sender, EventArgs e)
    {            
      Properties.Settings.Default.EnableAutoRefresh = chkEnableAutoRefresh.Checked;
      if (radOnDemand.Checked)
      {
        Properties.Settings.Default.AutoRefreshType = System.Enum.GetName(typeof(RefreshTypeEnum), RefreshTypeEnum.OnDemand);
      }
      else
      {
        Properties.Settings.Default.AutoRefreshType = System.Enum.GetName(typeof(RefreshTypeEnum), RefreshTypeEnum.ByTimer);
        Properties.Settings.Default.AutoRefreshFrequency = Convert.ToInt32(this.numAutoRefreshFrequency.Value);
      }
      
      Properties.Settings.Default.AutoRefreshNotifyChanges = chkNotifyChanges.Checked;      
      Properties.Settings.Default.RunAtStartup = chkRunAtStartup.Checked;
      Properties.Settings.Default.AutoCheckForUpdates = chkAutoCheckUpdates.Checked;
      Properties.Settings.Default.CheckForUpdatesFrequency = Convert.ToInt32(this.numCheckUpdatesWeeks.Value);
      Properties.Settings.Default.AutoAddServicesToMonitor = chkEnabledAutoAddServices.Checked;
      Properties.Settings.Default.Save();
    }

    private void chkEnableAutoRefresh_CheckedChanged(object sender, EventArgs e)
    {
      bool enableAutoRefreshControls = this.chkEnableAutoRefresh.Checked;
      radOnDemand.Enabled = enableAutoRefreshControls;
      radByTimer.Enabled = enableAutoRefreshControls;
      chkNotifyChanges.Enabled = this.numAutoRefreshFrequency.Enabled = this.radByTimer.Checked && enableAutoRefreshControls;
    }

    private void radByTimer_CheckedChanged(object sender, EventArgs e)
    {
      chkNotifyChanges.Enabled = this.numAutoRefreshFrequency.Enabled = this.radByTimer.Checked && this.chkEnableAutoRefresh.Checked;
    }
  
    private void chkAutoCheckUpdates_CheckedChanged(object sender, EventArgs e)
    {
      this.numCheckUpdatesWeeks.Enabled = this.chkAutoCheckUpdates.Checked;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {

    }
   
  }
}
