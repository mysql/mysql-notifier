//
// Copyright (c) 2013, Oracle and/or its affiliates. All rights reserved.
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
  using System.ComponentModel;
  using System.Drawing;
  using System.Linq;
  using System.Windows.Forms;
  using MySQL.Utility;
  using MySQL.Utility.Forms;

  /// <summary>
  /// Dialog window showing MySQL Server Instances to select for monitoring.
  /// </summary>
  public partial class MonitorMySQLServerInstancesDialog : BaseForm
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

    /// <summary>
    /// 
    /// </summary>
    private MySQLServicesList _mySQLServicesList;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="MonitorMySQLServerInstancesDialog"/> class.
    /// </summary>
    public MonitorMySQLServerInstancesDialog()
    {
      InitializeComponent();

      _mySQLServicesList = null;
      _lastServicesNameFilter = FilterTextBox.Text;
      _lastShowMonitoredServices = ShowMonitoredInstancesCheckBox.Checked;
      SelectedWorkbenchConnection = null;
    }

    #region Properties

    /// <summary>
    /// Gets a list of <see cref="MySQLService"/> objects monitored by the Notifier.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public MySQLServicesList MySQLServicesList
    {
      get
      {
        if (_mySQLServicesList == null)
        {
          _mySQLServicesList = new MySQLServicesList();
          _mySQLServicesList.LoadFromSettings();
        }

        return _mySQLServicesList;
      }
    }

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
      using (var instanceConnectionDialog = new MySQLWorkbenchConnectionDialog(null))
      {
        instanceConnectionDialog.ExternalProgramName = AssemblyInfo.AssemblyTitle;
        instanceConnectionDialog.Icon = Properties.Resources.MySqlNotifierIcon;
        instanceConnectionDialog.ShowIcon = true;
        instanceConnectionDialog.InfoDialogImageList.Images.Add(Properties.Resources.ApplicationLogo);
        instanceConnectionDialog.InfoDialogImageList.Images.Add(Properties.Resources.NotifierErrorImage);
        DialogResult dr = instanceConnectionDialog.ShowIfWorkbenchNotRunning();
        if (dr == DialogResult.OK)
        {
          RefreshMySQLInstancesList(false);
        }
      }
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

      ListViewItem newItem = new ListViewItem(workbenchConnection.DriverType.ToString());
      newItem.SubItems.Add(workbenchConnection.Name);
      newItem.SubItems.Add(workbenchConnection.Host);
      newItem.SubItems.Add(workbenchConnection.Port.ToString());
      newItem.SubItems.Add(alreadyMonitored ? Properties.Resources.YesText : Properties.Resources.NoText);
      newItem.Tag = workbenchConnection;
      newItem.Checked = !alreadyMonitored;
      newItem.ForeColor = alreadyMonitored ? SystemColors.InactiveCaption : SystemColors.WindowText;
      WorkbenchConnectionsListView.Items.Add(newItem);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="DialogOKButton"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DialogOKButton_Click(object sender, EventArgs e)
    {
      SelectedWorkbenchConnection = WorkbenchConnectionsListView.SelectedItems[0].Tag as MySqlWorkbenchConnection;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="FilterTextBox"/> textbox's text changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void FilterTextBox_TextChanged(object sender, EventArgs e)
    {
      FilterTimer.Start();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="FilterTextBox"/> textbox was validated.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void FilterTextBox_Validated(object sender, EventArgs e)
    {
      RefreshMySQLInstancesList(false);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="FilterTimer"/> timer's elapses.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void FilterTimer_Tick(object sender, EventArgs e)
    {
      FilterTimer.Stop();
      RefreshMySQLInstancesList(false);
    }

    /// <summary>
    /// Checks if a Workbench connection is being monitored already.
    /// </summary>
    /// <param name="connection">A Workbench connection to check for.</param>
    /// <returns><see cref="true"/> if the connection is already being monitored, <see cref="false"/> otherwise.</returns>
    private bool IsWorkbenchConnectionAlreadyMonitored(MySqlWorkbenchConnection connection)
    {
      bool isMonitored = false;

      foreach (var mySqlService in MySQLServicesList.Services)
      {
        isMonitored = mySqlService.WorkbenchConnections.Exists(wbConn => wbConn.Name == connection.Name);
        if (isMonitored)
        {
          break;
        }
      }

      return isMonitored;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="MonitorMySQLServerInstancesDialog"/> dialog is shown.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void MonitorMySQLServerInstancesDialog_Shown(object sender, EventArgs e)
    {
      RefreshMySQLInstancesList(true);
    }

    /// <summary>
    /// Reloads the list of MySQL Server instances from the ones contained in the MySQL Workbench connections file.
    /// </summary>
    /// <param name="forceRefresh">Flag indicating if the refresh must be done although filters haven't changed.</param>
    private void RefreshMySQLInstancesList(bool forceRefresh)
    {
      bool filterChanges = false;
      if (_lastServicesNameFilter != FilterTextBox.Text)
      {
        filterChanges = true;
        _lastServicesNameFilter = FilterTextBox.Text;
      }

      if (_lastShowMonitoredServices != ShowMonitoredInstancesCheckBox.Checked)
      {
        filterChanges = true;
        _lastShowMonitoredServices = ShowMonitoredInstancesCheckBox.Checked;
      }

      if (filterChanges || forceRefresh)
      {
        RefreshMySQLInstancesList(_lastServicesNameFilter, _lastShowMonitoredServices);
      }
    }

    /// <summary>
    /// Reloads the list of MySQL Server instances from the ones contained in the MySQL Workbench connections file.
    /// </summary>
    /// <param name="connectionNameFilter">Filter for connection names to only list ones containing the filter text.</param>
    /// <param name="showNonMonitoredConnections">Flag indicating if Workbench connections already being monitored are listed too.</param>
    private void RefreshMySQLInstancesList(string connectionNameFilter, bool showNonMonitoredConnections)
    {
      if (!string.IsNullOrEmpty(connectionNameFilter))
      {
        connectionNameFilter = connectionNameFilter.ToLowerInvariant();
      }

      WorkbenchConnectionsListView.Items.Clear();
      WorkbenchConnectionsListView.BeginUpdate();

      foreach (var connection in MySqlWorkbench.Connections.OrderBy(conn => conn.Name))
      {
        if (!string.IsNullOrEmpty(connectionNameFilter) && !connection.Name.ToLowerInvariant().Contains(connectionNameFilter))
        {
          continue;
        }

        AddWorkbenchConnectionToConnectionsList(connection, showNonMonitoredConnections);
      }

      WorkbenchConnectionsListView.EndUpdate();
      DialogOKButton.Enabled = false;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ShowMonitoredInstancesCheckBox"/> object's checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ShowMonitoredInstancesCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      RefreshMySQLInstancesList(false);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="WorkbenchConnectionsListView"/> selected item's index changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void WorkbenchConnectionsListView_SelectedIndexChanged(object sender, EventArgs e)
    {
      DialogOKButton.Enabled = WorkbenchConnectionsListView.SelectedItems.Count > 0 && WorkbenchConnectionsListView.SelectedItems[0].Checked;
    }
  }
}