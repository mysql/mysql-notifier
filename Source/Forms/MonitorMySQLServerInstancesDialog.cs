// Copyright (c) 2013, 2016, Oracle and/or its affiliates. All rights reserved.
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
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using MySql.Notifier.Classes;
using MySql.Notifier.Properties;
using MySQL.Utility.Classes;
using MySQL.Utility.Classes.MySQLWorkbench;
using MySQL.Utility.Forms;

namespace MySql.Notifier.Forms
{
  /// <summary>
  /// Dialog window showing MySQL Server Instances to select for monitoring.
  /// </summary>
  public partial class MonitorMySqlServerInstancesDialog : AutoStyleableBaseDialog
  {
    #region Fields

    /// <summary>
    /// Last filter for connection names to only list ones containing the filter text.
    /// </summary>
    private string _lastServicesNameFilter;

    /// <summary>
    /// Last flag indicating if Workbench connections already being monitored are listed too.
    /// </summary>
    private bool _lastShowMonitoredServices;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="MonitorMySqlServerInstancesDialog"/> class.
    /// </summary>
    public MonitorMySqlServerInstancesDialog()
      : this(null, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MonitorMySqlServerInstancesDialog"/> class.
    /// </summary>
    /// <param name="machinesList">List of <see cref="MySqlService"/> objects monitored by the Notifier.</param>
    /// <param name="instancesList">List of names of MySQL instance monitored by the Notifier.</param>
    public MonitorMySqlServerInstancesDialog(MachinesList machinesList, MySqlInstancesList instancesList)
    {
      InitializeComponent();

      _lastServicesNameFilter = FilterTextBox.Text;
      _lastShowMonitoredServices = ShowMonitoredInstancesCheckBox.Checked;
      MachinesList = machinesList;
      MySqlInstancesList = instancesList ?? new MySqlInstancesList();
      InstancesListChanged = false;
      ResetChangeCursorDelegate(true);
    }

    #region Properties

    /// <summary>
    /// Gets a value indicating whether the instances list changed.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool InstancesListChanged { get; private set; }

    /// <summary>
    /// Gets or sets the file path of the password vault file to be used.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public static string PasswordVaultFilePath
    {
      get
      {
        return Classes.Notifier.EnvironmentApplicationDataDirectory + @"\Oracle\MySQL Notifier\notifier_user_data.dat";
      }
    }

    /// <summary>
    /// Gets a list of names of MySQL instance monitored by the Notifier.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public MySqlInstancesList MySqlInstancesList { get; private set; }

    /// <summary>
    /// Gets a list of <see cref="Machine"/> objects monitored by the Notifier.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public MachinesList MachinesList { get; private set; }

    /// <summary>
    /// Gets the Workbench connection selected to be monitored.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public MySqlWorkbenchConnection SelectedWorkbenchConnection { get; private set; }

    #endregion Properties

    /// <summary>
    /// Event delegate method fired when the <see cref="AddConnectionButton"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AddConnectionButton_Click(object sender, EventArgs e)
    {
      using (var instanceConnectionDialog = new MySqlWorkbenchConnectionDialog(null, false))
      {
        instanceConnectionDialog.Icon = Resources.MySqlNotifierIcon;
        instanceConnectionDialog.ShowIcon = true;
        if (instanceConnectionDialog.ShowIfWorkbenchNotRunning() != DialogResult.OK)
        {
          return;
        }
      }

      InstancesListChanged = true;
      RefreshMySqlInstancesList(false);
    }

    /// <summary>
    /// Adds a MySQL Workbench connection to the list of connections.
    /// </summary>
    /// <param name="workbenchConnection">Workbench connection to add to the list.</param>
    /// <param name="showNonMonitoredConnections">Flag indicating if Workbench connections already being monitored are listed too.</param>
    private void AddWorkbenchConnectionToConnectionsList(MySqlWorkbenchConnection workbenchConnection, bool showNonMonitoredConnections)
    {
      bool alreadyMonitored = IsWorkbenchConnectionAlreadyMonitored(workbenchConnection);
      if (alreadyMonitored && !showNonMonitoredConnections)
      {
        return;
      }

      var newItem = new ListViewItem(workbenchConnection.ConnectionMethod.GetDescription());
      newItem.SubItems.Add(workbenchConnection.Name);
      newItem.SubItems.Add(workbenchConnection.Host);
      newItem.SubItems.Add(workbenchConnection.Port.ToString(CultureInfo.InvariantCulture));
      newItem.SubItems.Add(alreadyMonitored ? Resources.YesText : Resources.NoText);
      newItem.Tag = workbenchConnection;
      newItem.Checked = !alreadyMonitored;
      newItem.ForeColor = alreadyMonitored ? SystemColors.InactiveCaption : SystemColors.WindowText;
      WorkbenchConnectionsListView.Items.Add(newItem);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ConnectionsContextMenuStrip"/> context menu is being opened.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments</param>
    private void ConnectionsContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
      DeleteConnectionToolStripMenuItem.Visible = WorkbenchConnectionsListView.SelectedItems.Count > 0;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="DeleteConnectionToolStripMenuItem"/> context menu item is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DeleteConnectionToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (WorkbenchConnectionsListView.SelectedItems.Count == 0)
      {
        return;
      }

      var workbenchConnection = WorkbenchConnectionsListView.SelectedItems[0].Tag as MySqlWorkbenchConnection;
      if (workbenchConnection == null || !MySqlWorkbench.Connections.DeleteConnection(workbenchConnection.Id))
      {
        return;
      }

      InstancesListChanged = true;
      RefreshMySqlInstancesList(false);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="DialogOKButton"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DialogOKButton_Click(object sender, EventArgs e)
    {
      if (WorkbenchConnectionsListView.SelectedItems.Count > 0)
      {
        SelectedWorkbenchConnection = WorkbenchConnectionsListView.SelectedItems[0].Tag as MySqlWorkbenchConnection;
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="EditConnectionToolStripMenuItem"/> context menu item is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void EditConnectionToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (WorkbenchConnectionsListView.SelectedItems.Count == 0)
      {
        return;
      }

      var workbenchConnection = WorkbenchConnectionsListView.SelectedItems[0].Tag as MySqlWorkbenchConnection;
      if (workbenchConnection == null)
      {
        return;
      }

      using (var instanceConnectionDialog = new MySqlWorkbenchConnectionDialog(workbenchConnection, false))
      {
        instanceConnectionDialog.Icon = Resources.MySqlNotifierIcon;
        instanceConnectionDialog.ShowIcon = true;
        if (instanceConnectionDialog.ShowIfWorkbenchNotRunning() != DialogResult.OK)
        {
          return;
        }
      }

      InstancesListChanged = true;
      RefreshMySqlInstancesList(false);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="FilterTextBox"/> textbox's text changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void FilterTextBox_TextChanged(object sender, EventArgs e)
    {
      FilterTimer.Stop();
      FilterTimer.Start();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="FilterTextBox"/> textbox was validated.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void FilterTextBox_Validated(object sender, EventArgs e)
    {
      FilterTimer_Tick(FilterTimer, EventArgs.Empty);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="FilterTimer"/> timer's elapses.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void FilterTimer_Tick(object sender, EventArgs e)
    {
      bool filter = FilterTimer.Enabled;
      FilterTimer.Stop();
      if (filter)
      {
        RefreshMySqlInstancesList(false);
      }
    }

    /// <summary>
    /// Checks if a Workbench connection is being monitored already.
    /// </summary>
    /// <param name="connection">A Workbench connection to check for.</param>
    /// <returns><c>true</c> if the connection is already being monitored, <c>false</c> otherwise.</returns>
    private bool IsWorkbenchConnectionAlreadyMonitored(MySqlWorkbenchConnection connection)
    {
      foreach (var machine in MachinesList.Machines)
      {
        foreach (var mySqlService in machine.Services)
        {
          if (mySqlService.WorkbenchConnections == null)
          {
            continue;
          }

          if (mySqlService.WorkbenchConnections.Exists(wbConn => wbConn.Id == connection.Id))
          {
            return true;
          }
        }
      }

      return MySqlInstancesList.Any(mySqlInstance => mySqlInstance.RelatedConnections.Exists(wbConn => wbConn.Id == connection.Id));
    }

    /// <summary>
    /// Event delegate method fired before the <see cref="MonitorMySqlServerInstancesDialog"/> dialog is closed.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void MonitorMySQLServerInstancesDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (DialogResult != DialogResult.OK || SelectedWorkbenchConnection == null || WorkbenchConnectionsListView.SelectedItems.Count <= 0 || WorkbenchConnectionsListView.SelectedItems[0].Checked)
      {
        ResetChangeCursorDelegate(false);
        return;
      }

      var infoProperties = InfoDialogProperties.GetYesNoDialogProperties(
        InfoDialog.InfoType.Info,
        Resources.ConnectionAlreadyInInstancesTitle,
        Resources.ConnectionAlreadyInInstancesDetail,
        Resources.ConnectionAlreadyInInstancesSubDetail);
      infoProperties.CommandAreaProperties.DefaultButton = InfoDialog.DefaultButtonType.Button2;
      infoProperties.CommandAreaProperties.DefaultButtonTimeout = 30;
      var infoResult = InfoDialog.ShowDialog(infoProperties);
      if (infoResult.DialogResult == DialogResult.Yes)
      {
        ResetChangeCursorDelegate(false);
        return;
      }

      SelectedWorkbenchConnection = null;
      e.Cancel = true;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="MonitorMySqlServerInstancesDialog"/> dialog is shown.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void MonitorMySQLServerInstancesDialog_Shown(object sender, EventArgs e)
    {
      RefreshMySqlInstancesList(true);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="RefreshConnectionsToolStripMenuItem"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void RefreshConnectionsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      RefreshMySqlInstancesList(true);
    }

    /// <summary>
    /// Reloads the list of MySQL Server instances from the ones contained in the MySQL Workbench connections file.
    /// </summary>
    /// <param name="forceRefresh">Flag indicating if the refresh must be done although filters haven't changed.</param>
    private void RefreshMySqlInstancesList(bool forceRefresh)
    {
      if (_lastServicesNameFilter != null && _lastServicesNameFilter != FilterTextBox.Text)
      {
        _lastServicesNameFilter = FilterTextBox.Text;
      }

      _lastShowMonitoredServices = ShowMonitoredInstancesCheckBox.Checked;
      if (forceRefresh)
      {
        MySqlWorkbench.Connections.Load(MySqlWorkbench.Connections == MySqlWorkbench.ExternalConnections && MySqlWorkbench.ExternalApplicationsConnectionsFileRetryLoadOrRecreate);
      }

      RefreshMySqlInstancesList(_lastServicesNameFilter, _lastShowMonitoredServices);
    }

    /// <summary>
    /// Reloads the list of MySQL Server instances from the ones contained in the MySQL Workbench connections file.
    /// </summary>
    /// <param name="connectionNameFilter">Filter for connection names to only list ones containing the filter text.</param>
    /// <param name="showNonMonitoredConnections">Flag indicating if Workbench connections already being monitored are listed too.</param>
    private void RefreshMySqlInstancesList(string connectionNameFilter, bool showNonMonitoredConnections)
    {
      if (MySqlWorkbench.Connections.Count == 0)
      {
        return;
      }

      if (!string.IsNullOrEmpty(connectionNameFilter))
      {
        connectionNameFilter = connectionNameFilter.ToLowerInvariant();
      }

      WorkbenchConnectionsListView.Items.Clear();
      WorkbenchConnectionsListView.BeginUpdate();

      foreach (var connection in MySqlWorkbench.Connections.OrderBy(conn => conn.Name).Where(connection => connectionNameFilter != null && (string.IsNullOrEmpty(connectionNameFilter) || connection.Name.ToLowerInvariant().Contains(connectionNameFilter))))
      {
        AddWorkbenchConnectionToConnectionsList(connection, showNonMonitoredConnections);
      }

      WorkbenchConnectionsListView.EndUpdate();
      DialogOKButton.Enabled = false;
    }

    /// <summary>
    /// Sets a delegate for the MySQL Utility to change the cursor on top of caller windows.
    /// </summary>
    /// <param name="set">Flag indicating whether the delegate is set for this form, or reset to be empty.</param>
    private void ResetChangeCursorDelegate(bool set)
    {
      if (set)
      {
        MySqlWorkbench.ChangeCurrentCursor = delegate (Cursor cursor)
        {
          Cursor = cursor;
        };
      }
      else
      {
        Program.Notifier.SetChangeCursorDelegate();
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ShowMonitoredInstancesCheckBox"/> object's checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ShowMonitoredInstancesCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      RefreshMySqlInstancesList(false);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="WorkbenchConnectionsListView"/> control is double-clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void WorkbenchConnectionsListView_DoubleClick(object sender, EventArgs e)
    {
      DialogOKButton.PerformClick();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="WorkbenchConnectionsListView"/> selected itemText's index changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void WorkbenchConnectionsListView_SelectedIndexChanged(object sender, EventArgs e)
    {
      DialogOKButton.Enabled = WorkbenchConnectionsListView.SelectedItems.Count > 0;
    }
  }
}