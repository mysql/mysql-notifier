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
  using System.Drawing;
  using System.Linq;
  using System.Windows.Forms;
  using MySql.Notifier.Properties;
  using MySQL.Utility;
  using MySQL.Utility.Forms;

  /// <summary>
  /// Contains a group of ToolStripMenuItem controls for each of the corresponding MySQLInstance’s context menu items.
  /// </summary>
  public class MySQLInstanceMenuGroup : IDisposable
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MySQLInstanceMenuGroup"/> class.
    /// </summary>
    /// <param name="boundInstance">The MySQL instance that this menu group is associated to.</param>
    public MySQLInstanceMenuGroup(MySQLInstance boundInstance)
    {
      BoundInstance = boundInstance;
      InstanceMenuItem = new ToolStripMenuItem();
      Font menuItemFont = new Font(InstanceMenuItem.Font, FontStyle.Bold);
      InstanceMenuItem.Font = menuItemFont;

      if (MySqlWorkbench.AllowsExternalConnectionsManagement)
      {
        ConfigureMenuItem = new ToolStripMenuItem(Resources.ConfigureInstance);
        ConfigureMenuItem.Click += new EventHandler(ConfigureMenuItem_Click);
      }

      RecreateSQLEditorMenus();
      Separator = new ToolStripSeparator();
      Update();
    }

    /// <summary>
    /// Releases all resources used by the <see cref="MySQLInstanceMenuGroup"/> class
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases all resources used by the <see cref="MySQLInstanceMenuGroup"/> class
    /// </summary>
    /// <param name="disposing">If true this is called by Dispose(), otherwise it is called by the finalizer</param>
    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        //// Free managed resources
        if (ConfigureMenuItem != null)
        {
          ConfigureMenuItem.Dispose();
        }

        if (InstanceMenuItem != null)
        {
          InstanceMenuItem.Dispose();
        }
        if (SQLEditorMenuItem != null)
        {
          SQLEditorMenuItem.Dispose();
        }

        if (Separator != null)
        {
          Separator.Dispose();
        }
      }

      //// Add class finalizer if unmanaged resources are added to the class
      //// Free unmanaged resources if there are any
    }

    /// <summary>
    /// Delegate to refresh the context menus.
    /// </summary>
    /// <param name="menu"></param>
    private delegate void MenuRefreshDelegate(ContextMenuStrip menu);

    #region Properties

    /// <summary>
    /// Gets the MySQL instance that this menu group is associated to.
    /// </summary>
    public MySQLInstance BoundInstance { get; private set; }

    /// <summary>
    /// Gets the Configure Instance menu itemText that opens the instance's configuration page in MySQL Workbench.
    /// </summary>
    public ToolStripMenuItem ConfigureMenuItem { get; private set; }

    /// <summary>
    /// Gets the main MySQL instance's menu itemText that shows the connection status.
    /// </summary>
    public ToolStripMenuItem InstanceMenuItem { get; private set; }

    /// <summary>
    /// Gets the SQL Editor menu itemText that opens the SQL Editor page in MySQL Workbench for related connections.
    /// </summary>
    public ToolStripMenuItem SQLEditorMenuItem { get; private set; }

    /// <summary>
    /// The separator menu itemText at the end of all menu items.
    /// </summary>
    private ToolStripSeparator Separator { get; set; }

    #endregion Properties

    /// <summary>
    /// Finds the menu item's index within a context menu strip corresponding to the menu item with the given text.
    /// </summary>
    /// <param name="menu"><see cref="ContextMenuStrip"/> containing the itemText to find.</param>
    /// <param name="menuItemText">Menu item text.</param>
    /// <returns>Index of the dound menu itemText, -1 if  not found.</returns>
    public static int FindMenuItemWithinMenuStrip(ContextMenuStrip menu, string menuItemText)
    {
      int index = -1;

      for (int i = 0; i < menu.Items.Count; i++)
      {
        if (menu.Items[i].Text.Equals(menuItemText))
        {
          index = i;
          break;
        }
      }

      return index;
    }

    /// <summary>
    /// Adds the main MySQL instance's menu itemText and its sub-items to the given context menu strip.
    /// </summary>
    /// <param name="menu">Context menu strip to add the MySQL instance's menu items to.</param>
    public void AddToContextMenu(ContextMenuStrip menu)
    {
      if (menu.InvokeRequired)
      {
        menu.Invoke(new MethodInvoker(() => { AddToContextMenu(menu); }));
      }
      else
      {
        int index = FindMenuItemWithinMenuStrip(menu, Resources.Actions);
        if (index < 0)
        {
          index = 0;
        }

        InstanceMenuItem.Text = BoundInstance.HostIdentifier + " - " + BoundInstance.ConnectionStatusText;
        menu.Items.Insert(index++, InstanceMenuItem);
        if (BoundInstance.WorkbenchConnection != null)
        {
          if (ConfigureMenuItem != null)
          {
            menu.Items.Insert(index++, ConfigureMenuItem);
          }

          if (SQLEditorMenuItem != null)
          {
            menu.Items.Insert(index++, SQLEditorMenuItem);
          }
        }

        menu.Items.Insert(index++, Separator);
        menu.Refresh();
      }
    }

    /// <summary>
    /// Finds the menu item's index within a context menu strip corresponding to this instance menu group.
    /// </summary>
    /// <param name="menu"><see cref="ContextMenuStrip"/> containing the itemText to find.</param>
    /// <returns>Index of the dound menu itemText, -1 if  not found.</returns>
    public int FindInstanceMenuItemWithinMenuStrip(ContextMenuStrip menu)
    {
      return FindMenuItemWithinMenuStrip(menu, InstanceMenuItem.Text);
    }

    /// <summary>
    /// Recreates the SQL Editor sub menu items.
    /// </summary>
    public void RecreateSQLEditorMenus()
    {
      if (!MySqlWorkbench.AllowsExternalConnectionsManagement)
      {
        return;
      }

      if (SQLEditorMenuItem == null)
      {
        SQLEditorMenuItem = new ToolStripMenuItem(Resources.SQLEditor);
      }
      else
      {
        SQLEditorMenuItem.DropDownItems.Clear();
      }

      //// If there are 0 or 1 connections then the single menu will suffice.
      if (BoundInstance.RelatedConnections.Count <= 1)
      {
        SQLEditorMenuItem.Enabled = true;
        SQLEditorMenuItem.Click += new EventHandler(SQLEditorMenuItem_Click);
        return;
      }
      else
      {
        SQLEditorMenuItem.Enabled = false;
      }

      //// We have more than 1 connection so we create a submenu.
      foreach (var conn in BoundInstance.RelatedConnections)
      {
        ToolStripMenuItem menu = new ToolStripMenuItem(conn.Name);
        if (conn == BoundInstance.WorkbenchConnection)
        {
          Font boldFont = new Font(menu.Font, FontStyle.Bold);
          menu.Font = boldFont;
        }

        menu.Click += new EventHandler(SQLEditorMenuItem_Click);
        SQLEditorMenuItem.DropDownItems.Add(menu);
      }
    }

    /// <summary>
    /// Removes the main MySQL instance's menu itemText and its sub-items from the given context menu strip.
    /// </summary>
    /// <param name="menu">Context menu strip to remove the MySQL instance's menu items from..</param>
    public void RemoveFromContextMenu(ContextMenuStrip menu)
    {
      string[] menuItemTexts = new string[4];
      int index = -1;

      if (MySqlWorkbench.AllowsExternalConnectionsManagement)
      {
        //// The last itemText we delete is the service name itemText which is the reference for the others.
        menuItemTexts[0] = "Configure Menu";
        menuItemTexts[1] = "Editor Menu";
        menuItemTexts[2] = "Separator";
        menuItemTexts[3] = InstanceMenuItem.Text;
      }
      else
      {
        menuItemTexts[0] = "Separator";
        menuItemTexts[1] = InstanceMenuItem.Text;
      }

      foreach (var itemText in menuItemTexts)
      {
        if (string.IsNullOrEmpty(itemText))
        {
          continue;
        }

        index = FindInstanceMenuItemWithinMenuStrip(menu);
        if (index >= 0)
        {
          if (!itemText.Equals(InstanceMenuItem.Text))
          {
            index++;
          }

          menu.Items.RemoveAt(index);
        }
      }

      menu.Refresh();
    }

    /// <summary>
    /// Enables and disables menus based on the bound MySQL instance's connection status.
    /// </summary>
    public void Update()
    {
      ToolStrip menu = InstanceMenuItem.GetCurrentParent();
      if (menu != null && menu.InvokeRequired)
      {
        menu.Invoke(new MethodInvoker(() => { Update(); }));
      }
      else
      {
        InstanceMenuItem.Text = BoundInstance.HostIdentifier + " - " + BoundInstance.ConnectionStatusText;
        switch (BoundInstance.ConnectionStatus)
        {
          case MySqlWorkbenchConnection.ConnectionStatusType.AcceptingConnections:
            InstanceMenuItem.Image = Resources.NotifierIconRunning;
            break;

          case MySqlWorkbenchConnection.ConnectionStatusType.RefusingConnections:
            InstanceMenuItem.Image = Resources.NotifierIconStopped;
            break;

          case MySqlWorkbenchConnection.ConnectionStatusType.Unknown:
            InstanceMenuItem.Image = Resources.NotifierIcon;
            break;
        }

        if (SQLEditorMenuItem != null)
        {
          SQLEditorMenuItem.Enabled = MySqlWorkbench.AllowsExternalConnectionsManagement && BoundInstance.WorkbenchConnection != null;
        }

        if (ConfigureMenuItem != null)
        {
          ConfigureMenuItem.Enabled = BoundInstance.WorkbenchServer != null;
        }
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ConfigureMenuItem"/> menu itemText is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ConfigureMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        MySqlWorkbenchServer server = null;
        if (MySqlWorkbench.Servers.Any(s => s.ConnectionId == BoundInstance.WorkbenchConnectionId))
        {
          server = MySqlWorkbench.Servers.First(s => s.ConnectionId == BoundInstance.WorkbenchConnectionId);
        }

        MySqlWorkbench.LaunchConfigure(server);
      }
      catch (Exception ex)
      {
        InfoDialog.ShowErrorDialog(Resources.ErrorTitle, string.Format(Resources.FailureToLaunchWorkbench, ex.Message));
        MySQLSourceTrace.WriteAppErrorToLog(ex);
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ConfigureMenuItem"/> menu itemText is clicked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SQLEditorMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        if (BoundInstance.RelatedConnections.Count == 0)
        {
          MySqlWorkbench.LaunchSQLEditor(null);
        }
        else if (!SQLEditorMenuItem.HasDropDownItems)
        {
          MySqlWorkbench.LaunchSQLEditor(BoundInstance.WorkbenchConnection.Name);
        }
        else
        {
          for (int i = 0; i < SQLEditorMenuItem.DropDownItems.Count; i++)
          {
            if (sender == SQLEditorMenuItem.DropDownItems[i])
            {
              MySqlWorkbench.LaunchSQLEditor(BoundInstance.RelatedConnections[i].Name);
            }
          }
        }
      }
      catch (Exception ex)
      {
        InfoDialog.ShowErrorDialog(Resources.ErrorTitle, Resources.FailureToLaunchWorkbench);
        MySQLSourceTrace.WriteAppErrorToLog(ex);
      }
    }
  }
}