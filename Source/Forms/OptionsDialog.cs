// Copyright (c) 2012, 2019, Oracle and/or its affiliates. All rights reserved.
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
using MySql.Notifier.Classes;
using MySql.Notifier.Properties;
using MySql.Utility.Classes;
using MySql.Utility.Classes.MySqlWorkbench;
using MySql.Utility.Forms;

namespace MySql.Notifier.Forms
{
  public partial class OptionsDialog : AutoStyleableBaseDialog
  {
    #region Constants

    /// <summary>
    /// The spacing in pixels defined for the inner panel of the dialog from controls.
    /// </summary>
    public const int DIALOG_BORDER_WIDTH = 8;

    /// <summary>
    /// The spacing in pixels defined for the inner panel of the dialog from controls.
    /// </summary>
    public const int DIALOG_RIGHT_SPACING_TO_CONTROLS = 80;

    #endregion Constants

    #region Fields

    /// <summary>
    /// The dialog's initial width.
    /// </summary>
    private readonly int _initialWidth;

    #endregion Fields

    /// <summary>
    /// Contains options for users to customize the Notifier's behavior.
    /// </summary>
    internal OptionsDialog()
    {
      InitializeComponent();

      _initialWidth = Width;
      RefreshControlValues(false);
      SetAutomaticMigrationDelayText();
    }

    #region Properties

    /// <summary>
    /// Gets a value indicating whether the program is set to run at Windows startup.
    /// </summary>
    public bool RunAtStartUp { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the <see cref="MigrateWorkbenchConnectionsButton"/> should be enabled.
    /// </summary>
    private bool MigrateConnectionsButtonEnabled => !Settings.Default.WorkbenchMigrationSucceeded &&
                                                    Settings.Default.WorkbenchMigrationLastAttempt != DateTime.MinValue &&
                                                    Settings.Default.WorkbenchMigrationRetryDelay != 0;

    #endregion Properties

    /// <summary>
    /// Increases the width of the dialog in case the <see cref="AutomaticMigrationDelayLabel"/> gets too big.
    /// </summary>
    private void SetAutomaticMigrationDelayText()
    {
      SuspendLayout();
      AutomaticMigrationDelayValueLabel.Text = MySqlWorkbench.GetConnectionsMigrationDelayText(Program.Notifier.NextAutomaticConnectionsMigration, Settings.Default.WorkbenchMigrationSucceeded);
      MigrateWorkbenchConnectionsButton.Enabled = MigrateConnectionsButtonEnabled;
      Width = _initialWidth;
      var spacingDelta = AutomaticMigrationDelayValueLabel.Location.X + AutomaticMigrationDelayValueLabel.Size.Width + DIALOG_RIGHT_SPACING_TO_CONTROLS + (DIALOG_BORDER_WIDTH * 2) - Width;
      if (spacingDelta > 0)
      {
        Width += spacingDelta;
      }

      ResumeLayout();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="AutoCheckUpdatesCheckBox"/> is checked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AutoCheckUpdatesCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      CheckUpdatesWeeksNumericUpDown.Enabled = AutoCheckUpdatesCheckBox.Checked;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="AutoAddServicesCheckBox"/> is checked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AutoAddServicesCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      bool enableRelated = AutoAddServicesCheckBox.Checked;
      AutoAddRegexTextBox.Enabled = enableRelated;
      NotifyOfAutoAddCheckBox.Enabled = enableRelated;
      if (!enableRelated)
      {
        AutoAddRegexTextBox.Text = string.Empty;
        NotifyOfAutoAddCheckBox.Checked = false;
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="MigrateWorkbenchConnectionsButton"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void MigrateWorkbenchConnectionsButton_Click(object sender, EventArgs e)
    {
      Program.Notifier.MigrateExternalConnectionsToWorkbench(false);
      SetAutomaticMigrationDelayText();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="OptionsDialog"/> is being closed.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void OptionsDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (DialogResult == DialogResult.Cancel)
      {
        return;
      }

      var updateTask = AutoCheckUpdatesCheckBox.Checked != Settings.Default.AutoCheckForUpdates
                        || Settings.Default.CheckForUpdatesFrequency != Convert.ToInt32(CheckUpdatesWeeksNumericUpDown.Value);
      var deleteTask = !AutoCheckUpdatesCheckBox.Checked
                       && Settings.Default.AutoCheckForUpdates;

      Settings.Default.NotifyOfAutoServiceAddition = NotifyOfAutoAddCheckBox.Checked;
      Settings.Default.NotifyOfStatusChange = NotifyOfStatusChangeCheckBox.Checked;
      Settings.Default.AutoCheckForUpdates = AutoCheckUpdatesCheckBox.Checked;
      Settings.Default.CheckForUpdatesFrequency = Convert.ToInt32(CheckUpdatesWeeksNumericUpDown.Value);
      Settings.Default.PingServicesIntervalInSeconds = Convert.ToInt32(PingMonitoredInstancesNumericUpDown.Value);
      Settings.Default.AutoAddServicesToMonitor = AutoAddServicesCheckBox.Checked;
      Settings.Default.AutoAddPattern = AutoAddRegexTextBox.Text.Trim();
      Settings.Default.UseColorfulStatusIcons = UseColorfulIconsCheckBox.Checked;
      Settings.Default.Save();
      if (RunAtStartUp != RunAtStartupCheckBox.Checked)
      {
        Utilities.SetRunAtStartUp(Application.ProductName, RunAtStartupCheckBox.Checked);
      }

      if (!updateTask)
      {
        return;
      }

      if (Settings.Default.AutoCheckForUpdates && !string.IsNullOrEmpty(Program.InstallLocation))
      {
        Utilities.CreateScheduledTask(Classes.Notifier.DefaultTaskName, Classes.Notifier.DefaultTaskPath, "--c", Settings.Default.CheckForUpdatesFrequency);
      }

      if (deleteTask)
      {
        Utilities.DeleteScheduledTask(Classes.Notifier.DefaultTaskName);
      }
    }

    /// <summary>
    /// Refreshes the dialog controls' values.
    /// </summary>
    /// <param name="useDefaultValues">Controls are set to their default values if <c>true</c>. Current stored values in application settings are used otherwise.</param>
    private void RefreshControlValues(bool useDefaultValues)
    {
      var settings = Settings.Default;
      if (useDefaultValues)
      {
        NotifyOfAutoAddCheckBox.Checked = settings.GetPropertyDefaultValueByName<bool>("NotifyOfAutoServiceAddition");
        NotifyOfStatusChangeCheckBox.Checked = settings.GetPropertyDefaultValueByName<bool>("NotifyOfStatusChange");
        AutoCheckUpdatesCheckBox.Checked = settings.GetPropertyDefaultValueByName<bool>("AutoCheckForUpdates");
        CheckUpdatesWeeksNumericUpDown.Value = settings.GetPropertyDefaultValueByName<int>("CheckForUpdatesFrequency");
        PingMonitoredInstancesNumericUpDown.Value = settings.GetPropertyDefaultValueByName<int>("PingServicesIntervalInSeconds");
        AutoAddServicesCheckBox.Checked = settings.GetPropertyDefaultValueByName<bool>("AutoAddServicesToMonitor");
        AutoAddRegexTextBox.Text = settings.GetPropertyDefaultValueByName<string>("AutoAddPattern");
        UseColorfulIconsCheckBox.Checked = settings.GetPropertyDefaultValueByName<bool>("UseColorfulStatusIcons");
      }
      else
      {
        NotifyOfAutoAddCheckBox.Checked = settings.NotifyOfAutoServiceAddition;
        NotifyOfStatusChangeCheckBox.Checked = settings.NotifyOfStatusChange;
        AutoCheckUpdatesCheckBox.Checked = settings.AutoCheckForUpdates;
        CheckUpdatesWeeksNumericUpDown.Value = settings.CheckForUpdatesFrequency;
        PingMonitoredInstancesNumericUpDown.Value = settings.PingServicesIntervalInSeconds;
        AutoAddServicesCheckBox.Checked = settings.AutoAddServicesToMonitor;
        AutoAddRegexTextBox.Text = settings.AutoAddPattern;
        UseColorfulIconsCheckBox.Checked = settings.UseColorfulStatusIcons;
      }

      RunAtStartUp = Utilities.GetRunAtStartUp(Application.ProductName);
      RunAtStartupCheckBox.Checked = RunAtStartUp;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ResetToDefaultsButton"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ResetToDefaultsButton_Click(object sender, EventArgs e)
    {
      RefreshControlValues(true);
      Refresh();
    }
  }
}