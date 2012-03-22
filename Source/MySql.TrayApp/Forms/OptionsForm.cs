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
    #region Private Members

    private TrayAppSettingValues settingValues;

    #endregion Private Members

    internal OptionsForm(TrayAppSettingValues settingValues)
    {
      InitializeComponent();
      this.settingValues = settingValues;
    }

    private void OptionsForm_Load(object sender, EventArgs e)
    {
      #region Load Settings
      
      this.chkEnableAutoRefresh.Checked = Properties.Settings.Default.EnableAutoRefresh;
      this.radOnDemand.Checked = Properties.Settings.Default.AutoRefreshType == this.settingValues.autoRefreshTypeOnDemandValue;
      this.radByTimer.Checked = Properties.Settings.Default.AutoRefreshType == this.settingValues.autoRefreshTypeByTimerValue;
      this.numAutoRefreshFrequency.Value = Properties.Settings.Default.AutoRefreshFrequency;
      this.chkNotifyChanges.Checked = Properties.Settings.Default.AutoRefreshNotifyChanges;
      this.radHardRefresh.Checked = Properties.Settings.Default.RefreshMethod == this.settingValues.refreshMethodHardValue;
      this.radSoftRefresh.Checked = Properties.Settings.Default.RefreshMethod == this.settingValues.refreshMethodSoftValue;
      this.radInstanceName.Checked = Properties.Settings.Default.ScanForServicesType == this.settingValues.scanForServicesTypeStartsWithValue;
      this.radServicesRunning.Checked = Properties.Settings.Default.ScanForServicesType == this.settingValues.scanForServicesTypeMysqldValue;
      this.txtStartsWith.Text = Properties.Settings.Default.ServicesStartWith;
      this.chkRunAtStartup.Checked = Properties.Settings.Default.RunAtStartup;
      this.chkAutoCheckUpdates.Checked = Properties.Settings.Default.AutoCheckForUpdates;
      this.numCheckUpdatesWeeks.Value = Properties.Settings.Default.CheckForUpdatesFrequency;

      #endregion Load Settings
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      #region Save Settings

      Properties.Settings.Default.EnableAutoRefresh = this.chkEnableAutoRefresh.Checked;
      Properties.Settings.Default.AutoRefreshType = (this.radOnDemand.Checked ? this.settingValues.autoRefreshTypeOnDemandValue : this.settingValues.autoRefreshTypeByTimerValue);
      Properties.Settings.Default.AutoRefreshFrequency = Convert.ToInt32(this.numAutoRefreshFrequency.Value);
      Properties.Settings.Default.AutoRefreshNotifyChanges = this.chkNotifyChanges.Checked;
      Properties.Settings.Default.RefreshMethod = (this.radHardRefresh.Checked ? this.settingValues.refreshMethodHardValue : this.settingValues.refreshMethodSoftValue);
      Properties.Settings.Default.ScanForServicesType = (this.radInstanceName.Checked ? this.settingValues.scanForServicesTypeStartsWithValue : this.settingValues.scanForServicesTypeMysqldValue);
      Properties.Settings.Default.ServicesStartWith = this.txtStartsWith.Text;
      Properties.Settings.Default.RunAtStartup = this.chkRunAtStartup.Checked;
      Properties.Settings.Default.AutoCheckForUpdates = this.chkAutoCheckUpdates.Checked;
      Properties.Settings.Default.CheckForUpdatesFrequency = Convert.ToInt32(this.numCheckUpdatesWeeks.Value);

      #endregion Save Settings
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
  }
}
