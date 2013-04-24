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

using System;
using System.Drawing;
using System.ServiceProcess;
using System.Windows.Forms;
using MySql.Notifier.Properties;
using MySQL.Utility;

namespace MySql.Notifier
{
  /// <summary>
  /// Contains a group of ToolStripMenuItem instances for each of the corresponding MySQLService’s context menu items.
  /// </summary>
  public class ServiceMenuGroup
  {
    public ServiceMenuGroup(MySQLService mySQLBoundService)
    {
      boundService = mySQLBoundService;

      statusMenu = new ToolStripMenuItem(String.Format("{0} - {1}", boundService.DisplayName, boundService.Status));

      if (MySqlWorkbench.AllowsExternalConnectionsManagement)
      {
        configureMenu = new ToolStripMenuItem(Resources.ConfigureInstance);
        configureMenu.Click += new EventHandler(configureInstanceItem_Click);
        CreateEditorMenus();
      }

      separator = new ToolStripSeparator();

      Font menuItemFont = new Font(statusMenu.Font, FontStyle.Bold);
      Font subMenuItemFont = new Font(statusMenu.Font, FontStyle.Regular);
      statusMenu.Font = menuItemFont;

      startMenu = new ToolStripMenuItem("Start", Resources.play);
      startMenu.Click += new EventHandler(start_Click);

      stopMenu = new ToolStripMenuItem("Stop", Resources.stop);
      stopMenu.Click += new EventHandler(stop_Click);

      restartMenu = new ToolStripMenuItem("Restart", Resources.restart);
      restartMenu.Click += new EventHandler(restart_Click);

      statusMenu.DropDownItems.Add(startMenu);
      statusMenu.DropDownItems.Add(stopMenu);
      statusMenu.DropDownItems.Add(restartMenu);

      Update();
    }

    private ToolStripMenuItem statusMenu;
    private ToolStripMenuItem startMenu;
    private ToolStripMenuItem stopMenu;
    private ToolStripMenuItem restartMenu;
    private ToolStripMenuItem configureMenu;
    private ToolStripMenuItem editorMenu;
    private ToolStripSeparator separator;
    private MySQLService boundService;
    private Machine boundMachine;

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
        if (menu.Items[i].Text.ToString().StartsWith(menuItemText))
        {
          index = i;
          break;
        }
      }

      return index;
    }

    private void CreateEditorMenus()
    {
      editorMenu = new ToolStripMenuItem(Resources.SQLEditor);
      editorMenu.Enabled = false;

      if (boundService.WorkbenchConnections == null) return;

      // if there are 0 or 1 connections then the single menu will suffice
      if (boundService.WorkbenchConnections.Count <= 1)
      {
        editorMenu.Click += new EventHandler(workbenchConnection_Clicked);
        return;
      }

      // we have more than 1 connection so we create a submenu
      foreach (MySqlWorkbenchConnection c in boundService.WorkbenchConnections)
      {
        ToolStripMenuItem menu = new ToolStripMenuItem(c.Name);
        menu.Click += new EventHandler(workbenchConnection_Clicked);
        editorMenu.DropDownItems.Add(menu);
      }
    }

    private void workbenchConnection_Clicked(object sender, EventArgs e)
    {
      try
      {
        if (boundService.WorkbenchConnections.Count == 0)
          MySqlWorkbench.LaunchSQLEditor(null);
        else if (!editorMenu.HasDropDownItems)
          MySqlWorkbench.LaunchSQLEditor(boundService.WorkbenchConnections[0].Name);
        else
        {
          for (int x = 0; x < editorMenu.DropDownItems.Count; x++)
            if (sender == editorMenu.DropDownItems[x])
              MySqlWorkbench.LaunchSQLEditor(boundService.WorkbenchConnections[x].Name);
        }
      }
      catch (Exception ex)
      {
        using (var errorDialog = new MessageDialog(Resources.ErrorTitle, Resources.FailureToLaunchWorkbench, false))
        {
          errorDialog.ShowDialog();
        }
        MySQLNotifierTrace.GetSourceTrace().WriteError("Application Error - " + ex.Message + " " + ex.InnerException, 1);
      }
    }

    private void configureInstanceItem_Click(object sender, EventArgs e)
    {
      try
      {
        MySqlWorkbenchServer server = MySqlWorkbench.Servers.FindByServiceName(boundService.ServiceName);
        MySqlWorkbench.LaunchConfigure(server);
      }
      catch (Exception ex)
      {
        using (var errorDialog = new MessageDialog(Resources.ErrorTitle, Resources.FailureToLaunchWorkbench, false))
        {
          errorDialog.ShowDialog();
        }
        MySQLNotifierTrace.GetSourceTrace().WriteError("Application Error - " + ex.Message + " " + ex.InnerException, 1);
      }
    }

    public string BoundServiceName
    {
      get { return boundService.ServiceName; }
    }

    private void restart_Click(object sender, EventArgs e)
    {
      boundService.Restart();
      if (boundService.WorkCompleted)
        boundService.UpdateMenu(boundService.Status.ToString());
    }

    private void stop_Click(object sender, EventArgs e)
    {
      boundService.Stop();
      if (boundService.WorkCompleted)
        boundService.UpdateMenu(boundService.Status.ToString());
    }

    private void start_Click(object sender, EventArgs e)
    {
      boundService.Start();
      if (boundService.WorkCompleted)
      {
        boundService.UpdateMenu(boundService.Status.ToString());
      }
    }

    public void AddToContextMenu(ContextMenuStrip menu, int index = 0)
    {
      if (menu.InvokeRequired)
      {
        menu.Invoke(new MethodInvoker(() => { AddToContextMenu(menu); }));
      }
      else
      {
        menu.Items.Insert(index, statusMenu);
        if (boundService.IsRealMySQLService)
        {
          if (configureMenu != null) menu.Items.Insert(index, configureMenu);
          if (editorMenu != null) menu.Items.Insert(index, editorMenu);
        }
        menu.Items.Insert(index, separator);
      }
    }

    public void RemoveFromContextMenu(ContextMenuStrip menu)
    {
      string[] menuItems = new string[4];
      int index = -1;

      if (boundService.IsRealMySQLService && MySqlWorkbench.AllowsExternalConnectionsManagement)
      {
        menuItems[0] = "Configure Menu";
        menuItems[1] = "Editor Menu";
        menuItems[2] = "Separator";
        menuItems[3] = statusMenu.Text; // the last itemText we delete is the service name itemText which is the reference for the others
      }
      else
      {
        menuItems[0] = "Separator";
        menuItems[1] = statusMenu.Text;
      }

      foreach (var item in menuItems)
      {
        if (String.IsNullOrEmpty(item)) continue;

        for (int i = 0; i < menu.Items.Count; i++)
        {
          if (menu.Items[i].Text.Equals(statusMenu.Text))
          {
            index = i;
            break;
          }
        }
        if (index >= 0 && index <= menu.Items.Count)
        {
          if (!item.Equals(statusMenu.Text))
            index++;
          menu.Items.RemoveAt(index);
          menu.Refresh();
        }
      }
    }

    /// <summary>
    /// Enables and disables menus based on the current Service Status
    /// </summary>
    /// <param name="boundServiceName">Service Name</param>
    /// <param name="boundServiceStatus">Service Status</param>
    public void Update()
    {
      statusMenu.Text = String.Format("{0} - {1}", boundService.DisplayName, boundService.Status);
      Image image = null;
      switch (boundService.Status)
      {
        case ServiceControllerStatus.ContinuePending:
        case ServiceControllerStatus.Paused:
        case ServiceControllerStatus.PausePending:
        case ServiceControllerStatus.StartPending:
        case ServiceControllerStatus.StopPending:
          image = Resources.NotifierIconStarting;
          break;

        case ServiceControllerStatus.Stopped:
          image = Resources.NotifierIconStopped;
          break;

        case ServiceControllerStatus.Running:
          image = Resources.NotifierIconRunning;
          break;
      }
      statusMenu.Image = image;

      startMenu.Enabled = boundService.Status == ServiceControllerStatus.Stopped;
      stopMenu.Enabled = boundService.Status != ServiceControllerStatus.Stopped;
      restartMenu.Enabled = stopMenu.Enabled;
      if (MySqlWorkbench.AllowsExternalConnectionsManagement && boundService.IsRealMySQLService)
      {
        if (editorMenu != null) editorMenu.Enabled = true;
        if (configureMenu != null) configureMenu.Enabled = true;
      }
    }

    private void UpdateItems(ContextMenuStrip menu)
    {
      int index = -1;
      for (int i = 0; i < menu.Items.Count; i++)
      {
        if (menu.Items[i].Text.Equals(statusMenu.Text))
        {
          index = i;
          break;
        }
      }

      if (index >= 0 && index <= menu.Items.Count)
      {
        menu.Items.RemoveAt(index + 2);
        menu.Refresh();
      }
      editorMenu.Enabled = MySqlWorkbench.IsInstalled;
      menu.Items.Insert(index + 2, editorMenu);
    }

    public void RefreshMenu(ContextMenuStrip menu)
    {
      if (boundService.IsRealMySQLService)
      {
        boundService.FindMatchingWBConnections();
        CreateEditorMenus();

        if (menu.InvokeRequired)
        {
          menuRefreshDelegate md = new menuRefreshDelegate(UpdateItems);
          menu.Invoke(md, menu);
        }
        else
        {
          UpdateItems(menu);
        }
      }
    }

    private delegate void menuRefreshDelegate(ContextMenuStrip menu);

    public void RefreshRoot(ContextMenuStrip menu, ServiceControllerStatus previousStatus)
    {
      var newStatusText = String.Format("{0} - {1}", boundService.DisplayName, boundService.Status);
      var previousStatusText = String.Format("{0} - {1}", boundService.DisplayName, previousStatus);

      for (int i = 0; i < menu.Items.Count; i++)
      {
        if (menu.Items[i].Text.Equals(previousStatusText))
        {
          menu.Items[i].Text = newStatusText;

          Image image = null;
          switch (boundService.Status)
          {
            case ServiceControllerStatus.ContinuePending:
            case ServiceControllerStatus.Paused:
            case ServiceControllerStatus.PausePending:
            case ServiceControllerStatus.StartPending:
            case ServiceControllerStatus.StopPending:
              image = Resources.NotifierIconStarting;
              break;

            case ServiceControllerStatus.Stopped:
              image = Resources.NotifierIconStopped;
              break;

            case ServiceControllerStatus.Running:
              image = Resources.NotifierIconRunning;
              break;
          }
          menu.Items[i].Image = image;
          ToolStripMenuItem menuItem = (ToolStripMenuItem)menu.Items[i];
          menuItem.DropDownItems[0].Enabled = boundService.Status == ServiceControllerStatus.Stopped;
          menuItem.DropDownItems[1].Enabled = boundService.Status != ServiceControllerStatus.Stopped;
          menuItem.DropDownItems[2].Enabled = menuItem.DropDownItems[1].Enabled;
          break;
        }
      }
    }

    /// <summary>
    /// Adds a new itemText to the Notify Icon's context menu.
    /// </summary>
    /// <param name="displayText">Menu itemText's text</param>
    /// <param name="menuName">Menu itemText object's name</param>
    /// <param name="image">Menu itemText's icon displayed at its left</param>
    /// <param name="eventHandler">Event handler method to register with the Click event</param>
    /// <param name="enable">Flag that indicates the Enabled status of the menu itemText</param>
    /// <returns>A new ToolStripMenuItem object</returns>
    public static ToolStripMenuItem ToolStripMenuItemWithHandler(string displayText, string menuName, System.Drawing.Image image, EventHandler eventHandler, bool enable)
    {
      var menuItem = new ToolStripMenuItem(displayText);

      if (eventHandler != null)
        menuItem.Click += eventHandler;
      menuItem.Image = image;
      menuItem.Name = menuName;
      menuItem.Enabled = enable;
      return menuItem;
    }

    /// <summary>
    /// Adds a new itemText to the Notify Icon's context menu.
    /// </summary>
    /// <param name="displayText">Menu itemText's text</param>
    /// <param name="menuName">Menu itemText object's name</param>
    /// <param name="image">Menu itemText's icon displayed at its left</param>
    /// <param name="eventHandler">Event handler method to register with the Click event</param>
    /// <returns>A new ToolStripMenuItem object</returns>
    public static ToolStripMenuItem ToolStripMenuItemWithHandler(string displayText, string menuName, System.Drawing.Image image, EventHandler eventHandler)
    {
      return ToolStripMenuItemWithHandler(displayText, menuName, image, eventHandler, true);
    }

    /// <summary>
    /// Adds a new itemText to the Notify Icon's context menu.
    /// </summary>
    /// <param name="displayText">Menu itemText's text</param>
    /// <param name="image">Menu itemText's icon displayed at its left</param>
    /// <param name="eventHandler">Event handler method to register with the Click event</param>
    /// <param name="enable">Flag that indicates the Enabled status of the menu itemText</param>
    /// <returns>A new ToolStripMenuItem object</returns>
    public static ToolStripMenuItem ToolStripMenuItemWithHandler(string displayText, System.Drawing.Image image, EventHandler eventHandler, bool enable)
    {
      return ToolStripMenuItemWithHandler(displayText, String.Format("mnu{0}", displayText.Replace(" ", String.Empty)), image, eventHandler, enable);
    }

    /// <summary>
    /// Adds a new itemText to the Notify Icon's context menu.
    /// </summary>
    /// <param name="displayText">Menu itemText's text</param>
    /// <param name="image">Menu itemText's icon displayed at its left</param>
    /// <param name="eventHandler">Event handler method to register with the Click event</param>
    /// <returns>A new ToolStripMenuItem object</returns>
    public static ToolStripMenuItem ToolStripMenuItemWithHandler(string displayText, System.Drawing.Image image, EventHandler eventHandler)
    {
      return ToolStripMenuItemWithHandler(displayText, image, eventHandler, true);
    }

    /// <summary>
    /// Adds a new itemText to the Notify Icon's context menu.
    /// </summary>
    /// <param name="displayText">Menu itemText's text</param>
    /// <param name="eventHandler">Event handler method to register with the Click event</param>
    /// <param name="enable">Flag that indicates the Enabled status of the menu itemText</param>
    /// <returns>A new ToolStripMenuItem object</returns>
    public static ToolStripMenuItem ToolStripMenuItemWithHandler(string displayText, EventHandler eventHandler, bool enable)
    {
      return ToolStripMenuItemWithHandler(displayText, null, eventHandler, enable);
    }

    /// <summary>
    /// Adds a new itemText to the Notify Icon's context menu.
    /// </summary>
    /// <param name="displayText">Menu itemText's text</param>
    /// <param name="eventHandler">Event handler method to register with the Click event</param>
    /// <returns>A new ToolStripMenuItem object</returns>
    public static ToolStripMenuItem ToolStripMenuItemWithHandler(string displayText, EventHandler eventHandler)
    {
      return ToolStripMenuItemWithHandler(displayText, null, eventHandler);
    }
  }
}