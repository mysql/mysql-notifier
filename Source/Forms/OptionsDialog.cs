// 
// Copyright (c) 2012-2013, Oracle and/or its affiliates. All rights reserved.
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License as
// published by the Free Software Foundation; version 2 of the
// License.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
// 02110-1301  USA
//

namespace MySql.Notifier
{
  using System;
  using System.Windows.Forms;
  using MySql.Notifier.Properties;
  using MySQL.Utility;
  using MySQL.Utility.Forms;

  public partial class OptionsDialog : AutoStyleableBaseDialog
  {
    /// <summary>
    /// Contains options for users to customize the Notifier's behavior.
    /// </summary>
    internal OptionsDialog()
    {
      InitializeComponent();

      NotifyOfAutoAddCheckBox.Checked = Settings.Default.NotifyOfAutoServiceAddition;
      NotifyOfStatusChangeCheckBox.Checked = Settings.Default.NotifyOfStatusChange;
      RunAtStartupCheckBox.Checked = Utility.GetRunAtStartUp(Application.ProductName);
      AutoCheckUpdatesCheckBox.Checked = Settings.Default.AutoCheckForUpdates;
      CheckUpdatesWeeksNumericUpDown.Value = Settings.Default.CheckForUpdatesFrequency;
      AutoAddServicesCheckBox.Checked = Settings.Default.AutoAddServicesToMonitor;
      AutoAddRegexTextBox.Text = Settings.Default.AutoAddPattern;
      UseColorfulIconsCheckBox.Checked = Settings.Default.UseColorfulStatusIcons;
    }

    private void DialogApplyButton_Click(object sender, EventArgs e)
    {
      var updateTask = AutoCheckUpdatesCheckBox.Checked != Settings.Default.AutoCheckForUpdates ? true : false;
      var deleteTask = !AutoCheckUpdatesCheckBox.Checked && Settings.Default.AutoCheckForUpdates ? true : false;
      var deleteIfPrevious = AutoCheckUpdatesCheckBox.Checked && !Settings.Default.AutoCheckForUpdates ? false : true;

      if (Settings.Default.CheckForUpdatesFrequency != Convert.ToInt32(this.CheckUpdatesWeeksNumericUpDown.Value)) updateTask = true;

      Settings.Default.NotifyOfAutoServiceAddition = NotifyOfAutoAddCheckBox.Checked;
      Settings.Default.NotifyOfStatusChange = NotifyOfStatusChangeCheckBox.Checked;
      Settings.Default.AutoCheckForUpdates = AutoCheckUpdatesCheckBox.Checked;
      Settings.Default.CheckForUpdatesFrequency = Convert.ToInt32(this.CheckUpdatesWeeksNumericUpDown.Value);
      Settings.Default.AutoAddServicesToMonitor = AutoAddServicesCheckBox.Checked;
      Settings.Default.AutoAddPattern = AutoAddRegexTextBox.Text.Trim();
      Settings.Default.UseColorfulStatusIcons = UseColorfulIconsCheckBox.Checked;
      Settings.Default.Save();
      Utility.SetRunAtStartUp(Application.ProductName, RunAtStartupCheckBox.Checked);

      if (updateTask)
      {
        if (Settings.Default.AutoCheckForUpdates)
        {
          if (!String.IsNullOrEmpty(Utility.GetInstallLocation(Application.ProductName)))
          {
            Utility.CreateScheduledTask("MySQLNotifierTask", Utility.GetInstallLocation(Application.ProductName) + "MySqlNotifier.exe", "--c", Settings.Default.CheckForUpdatesFrequency, deleteIfPrevious, Utility.GetOsVersion() == Utility.OSVersion.WindowsXp);
          }
        }
        if (deleteTask)
          Utility.DeleteScheduledTask("MySQLNotifierTask");
      }
    }

    private void AutoCheckUpdatesCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      this.CheckUpdatesWeeksNumericUpDown.Enabled = this.AutoCheckUpdatesCheckBox.Checked;
    }

    private void AutoAddServicesCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      AutoAddRegexTextBox.Enabled = AutoAddServicesCheckBox.Checked;
    }
  }
}