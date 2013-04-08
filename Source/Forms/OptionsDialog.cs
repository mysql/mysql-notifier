//
// Copyright (c) 2012, Oracle and/or its affiliates. All rights reserved.
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

using System;
using System.Windows.Forms;
using MySql.Notifier.Properties;
using MySQL.Utility;

namespace MySql.Notifier
{
  public partial class OptionsDialog : BaseForm
  {
    ServiceType serviceType = ServiceType.Local;

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
     

      //// Disable checks to monitor when service is hosted at non-windows remote machine. 
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
            Utility.CreateScheduledTask("MySQLNotifierTask", Utility.GetInstallLocation("MySQL Notifier") + "MySqlNotifier.exe", "--c", Settings.Default.CheckForUpdatesFrequency, deleteIfPrevious, Utility.GetOsVersion() == Utility.OSVersion.WindowsXp);
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