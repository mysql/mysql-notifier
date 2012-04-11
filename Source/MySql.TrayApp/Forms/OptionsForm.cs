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
    
    private TrayAppSettingValues _settingValues;

    internal OptionsForm(TrayAppSettingValues settingValues)
    {
      InitializeComponent();
      _settingValues = settingValues;
    }

    private void OptionsForm_Load(object sender, EventArgs e)
    {
          
      chkEnableAutoRefresh.Checked = Properties.Settings.Default.EnableAutoRefresh;
      radOnDemand.Checked = Properties.Settings.Default.AutoRefreshType == _settingValues.autoRefreshTypeOnDemandValue;
      radByTimer.Checked = Properties.Settings.Default.AutoRefreshType == _settingValues.autoRefreshTypeByTimerValue;
      numAutoRefreshFrequency.Value = Properties.Settings.Default.AutoRefreshFrequency;
      chkNotifyChanges.Checked = Properties.Settings.Default.AutoRefreshNotifyChanges;
      radHardRefresh.Checked = Properties.Settings.Default.RefreshMethod == _settingValues.refreshMethodHardValue;
      radSoftRefresh.Checked = Properties.Settings.Default.RefreshMethod == _settingValues.refreshMethodSoftValue;
      radInstanceName.Checked = Properties.Settings.Default.ScanForServicesType == _settingValues.scanForServicesTypeStartsWithValue;
      radServicesRunning.Checked = Properties.Settings.Default.ScanForServicesType == _settingValues.scanForServicesTypeMysqldValue;
      txtStartsWith.Text = Properties.Settings.Default.ServicesStartWith;
      chkRunAtStartup.Checked = Properties.Settings.Default.RunAtStartup;
      chkAutoCheckUpdates.Checked = Properties.Settings.Default.AutoCheckForUpdates;
      numCheckUpdatesWeeks.Value = Properties.Settings.Default.CheckForUpdatesFrequency;
      lstExistingServices.Items.Add(Properties.Settings.Default.ServicesInstalled);
      lstMonitoredServices.Items.Add(Properties.Settings.Default.ServicesMonitor);

    }

    private void btnOK_Click(object sender, EventArgs e)
    {

      Properties.Settings.Default.ServicesInstalled.Clear();
      Properties.Settings.Default.ServicesMonitor.Clear();
      Properties.Settings.Default.EnableAutoRefresh = this.chkEnableAutoRefresh.Checked;
      Properties.Settings.Default.AutoRefreshType = (this.radOnDemand.Checked ? _settingValues.autoRefreshTypeOnDemandValue : _settingValues.autoRefreshTypeByTimerValue);
      Properties.Settings.Default.AutoRefreshFrequency = Convert.ToInt32(this.numAutoRefreshFrequency.Value);
      Properties.Settings.Default.AutoRefreshNotifyChanges = this.chkNotifyChanges.Checked;
      Properties.Settings.Default.RefreshMethod = (this.radHardRefresh.Checked ? _settingValues.refreshMethodHardValue : _settingValues.refreshMethodSoftValue);
      Properties.Settings.Default.ScanForServicesType = (this.radInstanceName.Checked ? _settingValues.scanForServicesTypeStartsWithValue : _settingValues.scanForServicesTypeMysqldValue);
      Properties.Settings.Default.ServicesStartWith = this.txtStartsWith.Text;
      Properties.Settings.Default.RunAtStartup = this.chkRunAtStartup.Checked;
      Properties.Settings.Default.AutoCheckForUpdates = this.chkAutoCheckUpdates.Checked;
      Properties.Settings.Default.CheckForUpdatesFrequency = Convert.ToInt32(this.numCheckUpdatesWeeks.Value);      
      Properties.Settings.Default.ServicesInstalled.AddRange(lstExistingServices.Items.Cast<string>().ToArray());
      Properties.Settings.Default.ServicesMonitor.AddRange(lstMonitoredServices.Items.Cast<string>().ToArray());
    }

    private void chkEnableAutoRefresh_CheckedChanged(object sender, EventArgs e)
    {
      bool enableAutoRefreshControls = this.chkEnableAutoRefresh.Checked;

      this.radOnDemand.Enabled = enableAutoRefreshControls;
      this.radByTimer.Enabled = enableAutoRefreshControls;
      this.chkNotifyChanges.Enabled = this.numAutoRefreshFrequency.Enabled = this.radByTimer.Checked && enableAutoRefreshControls;
    }

    private void radByTimer_CheckedChanged(object sender, EventArgs e)
    {
      this.chkNotifyChanges.Enabled = this.numAutoRefreshFrequency.Enabled = this.radByTimer.Checked && this.chkEnableAutoRefresh.Checked;
    }

    private void radInstanceName_CheckedChanged(object sender, EventArgs e)
    {
      this.txtStartsWith.Enabled = this.radInstanceName.Checked;
    }

    private void chkAutoCheckUpdates_CheckedChanged(object sender, EventArgs e)
    {
      this.numCheckUpdatesWeeks.Enabled = this.chkAutoCheckUpdates.Checked;
    }

    private void btnRefreshAll_Click(object sender, EventArgs e)
    {

    }
  }
}
