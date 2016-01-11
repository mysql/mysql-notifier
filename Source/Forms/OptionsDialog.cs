// Copyright (c) 2012, 2016, Oracle and/or its affiliates. All rights reserved.
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

using System;
using System.Windows.Forms;
using MySql.Notifier.Properties;
using MySQL.Utility.Classes;
using MySQL.Utility.Forms;

namespace MySql.Notifier.Forms
{
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
      var updateTask = AutoCheckUpdatesCheckBox.Checked != Settings.Default.AutoCheckForUpdates;
      var deleteTask = !AutoCheckUpdatesCheckBox.Checked && Settings.Default.AutoCheckForUpdates;

      if (Settings.Default.CheckForUpdatesFrequency != Convert.ToInt32(CheckUpdatesWeeksNumericUpDown.Value)) updateTask = true;

      Settings.Default.NotifyOfAutoServiceAddition = NotifyOfAutoAddCheckBox.Checked;
      Settings.Default.NotifyOfStatusChange = NotifyOfStatusChangeCheckBox.Checked;
      Settings.Default.AutoCheckForUpdates = AutoCheckUpdatesCheckBox.Checked;
      Settings.Default.CheckForUpdatesFrequency = Convert.ToInt32(CheckUpdatesWeeksNumericUpDown.Value);
      Settings.Default.AutoAddServicesToMonitor = AutoAddServicesCheckBox.Checked;
      Settings.Default.AutoAddPattern = AutoAddRegexTextBox.Text.Trim();
      Settings.Default.UseColorfulStatusIcons = UseColorfulIconsCheckBox.Checked;
      Settings.Default.Save();
      Utility.SetRunAtStartUp(Application.ProductName, RunAtStartupCheckBox.Checked);

      if (!updateTask)
      {
        return;
      }

      if (Settings.Default.AutoCheckForUpdates && !string.IsNullOrEmpty(Notifier.InstallLocation))
      {
        Utility.CreateScheduledTask(Notifier.DefaultTaskName, Notifier.DefaultTaskPath, "--c", Settings.Default.CheckForUpdatesFrequency);
      }

      if (deleteTask)
      {
        Utility.DeleteScheduledTask(Notifier.DefaultTaskName);
      }
    }

    private void AutoCheckUpdatesCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      CheckUpdatesWeeksNumericUpDown.Enabled = AutoCheckUpdatesCheckBox.Checked;
    }

    private void AutoAddServicesCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      AutoAddRegexTextBox.Enabled = AutoAddServicesCheckBox.Checked;
    }
  }
}