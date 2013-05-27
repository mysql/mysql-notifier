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
  using System.Drawing;
  using System.Linq;
  using System.Windows.Forms;
  using MySql.Notifier.Properties;
  using MySQL.Utility;
  using MySQL.Utility.Forms;

  /// <summary>
  /// Contains a group of ToolStripMenuItem instances for each of the corresponding MySQLService’s context menu items.
  /// </summary>
  public class ServiceMenuGroup : IDisposable
  {
    #region Fields

    private MySQLService boundService;

    private ToolStripMenuItem configureMenu;

    private ToolStripMenuItem editorMenu;

    private ToolStripMenuItem restartMenu;

    private ToolStripSeparator separator;

    private ToolStripMenuItem startMenu;

    private ToolStripMenuItem statusMenu;

    private ToolStripMenuItem stopMenu;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceMenuGroup"/> class.
    /// </summary>
    /// <param name="mySQLBoundService">The service this menu group is bound to.</param>
    public ServiceMenuGroup(MySQLService mySQLBoundService)
    {
      MenuItemsQuantity = 0;
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

      startMenu = new ToolStripMenuItem(Resources.StartText, Resources.play);
      startMenu.Click += new EventHandler(start_Click);

      stopMenu = new ToolStripMenuItem(Resources.StopText, Resources.stop);
      stopMenu.Click += new EventHandler(stop_Click);

      restartMenu = new ToolStripMenuItem(Resources.RestartText, Resources.restart);
      restartMenu.Click += new EventHandler(restart_Click);

      Update();
    }

    private delegate void menuRefreshDelegate(ContextMenuStrip menu);

    public string BoundServiceName
    {
      get { return boundService.ServiceName; }
    }

    public int MenuItemsQuantity { get; private set; }

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
        if (menu.Items[i].Text.StartsWith(menuItemText))
        {
          index = i;
          break;
        }
      }

      return index;
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
      {
        menuItem.Click += eventHandler;
      }

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

    /// <summary>
    /// Adds the context menu items corresponding to the bound service.
    /// </summary>
    /// <param name="menu">The Notifier's context menu.</param>
    public void AddToContextMenu(ContextMenuStrip menu)
    {
      if (menu.InvokeRequired)
      {
        menu.Invoke(new MethodInvoker(() => { AddToContextMenu(menu); }));
      }
      else
      {
        int index = FindMenuItemWithinMenuStrip(menu, boundService.Host.Name);
        int servicesMenusCount = boundService.Host.Services != null ? boundService.Host.Services.Sum(s => s != boundService && s.MenuGroup != null ? s.MenuGroup.MenuItemsQuantity : 0) : 0;
        index += servicesMenusCount;
        if (index < 0)
        {
          return;
        }

        //// Show the separator just above this new menu item if needed.
        if (index > 0 && menu.Items[index] is ToolStripSeparator)
        {
          menu.Items[index].Visible = true;
        }

        index++;
        menu.Items.Insert(index, statusMenu);
        MenuItemsQuantity++;
        if (boundService.IsRealMySQLService)
        {
          if (configureMenu != null)
          {
            menu.Items.Insert(++index, configureMenu);
            MenuItemsQuantity++;
          }

          if (editorMenu != null)
          {
            menu.Items.Insert(++index, editorMenu);
            MenuItemsQuantity++;
          }
        }

        menu.Items.Insert(++index, separator);
        separator.Visible = menu.Items[index + 1].BackColor != SystemColors.MenuText;
        MenuItemsQuantity++;
      }
    }

    /// <summary>
    /// Releases all resources used by the <see cref="ServiceMenuGroup"/> class
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Refreshes the menu items of this menu group.
    /// </summary>
    /// <param name="menu">The Notifier's context menu.</param>
    public void RefreshMenu(ContextMenuStrip menu)
    {
      if (menu.InvokeRequired)
      {
        menu.Invoke(new MethodInvoker(() => { RefreshMenu(menu); }));
      }
      else
      {
        if (!boundService.IsRealMySQLService)
        {
          return;
        }

        boundService.FindMatchingWBConnections();
        CreateEditorMenus();

        int index = FindMenuItemWithinMenuStrip(menu, statusMenu.Text);
        if (index >= 0)
        {
          menu.Items.RemoveAt(index + 2);
          menu.Refresh();
        }

        menu.Items.Insert(index + 2, editorMenu);
      }
    }

    /// <summary>
    /// Removes all menu items related to this service menu group from the main Notifier's context menu.
    /// </summary>
    /// <param name="menu">Main Notifier's context menu.</param>
    public void RemoveFromContextMenu(ContextMenuStrip menu)
    {
      string[] menuItems = new string[4];
      int index = -1;

      if (boundService.IsRealMySQLService && MySqlWorkbench.AllowsExternalConnectionsManagement)
      {
        menuItems[0] = Resources.ConfigureInstance;
        menuItems[1] = Resources.SQLEditor;
        menuItems[2] = "Separator";
        menuItems[3] = statusMenu.Text; // the last itemText we delete is the service name itemText which is the reference for the others
      }
      else
      {
        menuItems[0] = "Separator";
        menuItems[1] = statusMenu.Text;
      }

      index = FindMenuItemWithinMenuStrip(menu, statusMenu.Text);
      if (index <= 0)
      {
        return;
      }

      //// Hide the separator just above this new menu item if needed.
      if (index > 0 && menu.Items[index - 1] is ToolStripSeparator)
      {
        menu.Items[index - 1].Visible = menu.Items[index + 1].BackColor != SystemColors.MenuText;
      }

      foreach (var item in menuItems)
      {
        if (string.IsNullOrEmpty(item))
        {
          continue;
        }

        int plusItem = !item.Equals(statusMenu.Text) ? 1 : 0;
        menu.Items.RemoveAt(index + plusItem);
      }

      menu.Refresh();
    }

    /// <summary>
    /// Enables and disables menus based on the current Service Status
    /// </summary>
    /// <param name="boundServiceName">Service Name</param>
    /// <param name="boundServiceStatus">Service Status</param>
    public void Update()
    {
      var notifierMenu = statusMenu.GetCurrentParent();
      if (notifierMenu != null && notifierMenu.InvokeRequired)
      {
        notifierMenu.Invoke(new MethodInvoker(() => { Update(); }));
      }
      else
      {
        statusMenu.Text = String.Format("{0} - {1}", boundService.DisplayName, boundService.Status);
        Image image = null;
        switch (boundService.Status)
        {
          case MySQLServiceStatus.ContinuePending:
          case MySQLServiceStatus.Paused:
          case MySQLServiceStatus.PausePending:
          case MySQLServiceStatus.StartPending:
          case MySQLServiceStatus.StopPending:
            image = Resources.NotifierIconStarting;
            break;

          case MySQLServiceStatus.Unavailable:
          case MySQLServiceStatus.Stopped:
            image = Resources.NotifierIconStopped;
            break;

          case MySQLServiceStatus.Running:
            image = Resources.NotifierIconRunning;
            break;
        }

        statusMenu.Image = image;
        startMenu.Enabled = boundService.Status == MySQLServiceStatus.Stopped;
        stopMenu.Enabled = boundService.Status != MySQLServiceStatus.Stopped;
        restartMenu.Enabled = stopMenu.Enabled;

        if (editorMenu != null)
        {
          editorMenu.Enabled = MySqlWorkbench.AllowsExternalConnectionsManagement && boundService.WorkbenchConnections != null && boundService.WorkbenchConnections.Count > 0;
        }

        if (configureMenu != null)
        {
          configureMenu.Enabled = MySqlWorkbench.AllowsExternalConnectionsManagement;
        }

        bool actionMenusAvailable = boundService.Host.IsOnline;
        if (actionMenusAvailable && statusMenu.DropDownItems.Count == 0)
        {
          statusMenu.DropDownItems.Add(startMenu);
          statusMenu.DropDownItems.Add(stopMenu);
          statusMenu.DropDownItems.Add(restartMenu);
        }
        else if (!actionMenusAvailable && statusMenu.DropDownItems.Count > 0)
        {
          statusMenu.DropDownItems.Clear();
        }
      }
    }

    /// <summary>
    /// Releases all resources used by the <see cref="ServiceMenuGroup"/> class
    /// </summary>
    /// <param name="disposing">If true this is called by Dispose(), otherwise it is called by the finalizer</param>
    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        //// Free managed resources
        if (configureMenu != null)
        {
          configureMenu.Dispose();
        }

        if (editorMenu != null)
        {
          editorMenu.Dispose();
        }

        if (restartMenu != null)
        {
          restartMenu.Dispose();
        }

        if (separator != null)
        {
          separator.Dispose();
        }

        if (startMenu != null)
        {
          startMenu.Dispose();
        }

        if (statusMenu != null)
        {
          statusMenu.Dispose();
        }

        if (stopMenu != null)
        {
          stopMenu.Dispose();
        }
      }

      //// Add class finalizer if unmanaged resources are added to the class
      //// Free unmanaged resources if there are any
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
        InfoDialog.ShowErrorDialog(Resources.ErrorTitle, Resources.FailureToLaunchWorkbench);
        MySQLSourceTrace.WriteAppErrorToLog(ex);
      }
    }

    /// <summary>
    /// Creates the SQL Editor menu item and its drop-down items for the related MySQL Workbench connections.
    /// </summary>
    private void CreateEditorMenus()
    {
      editorMenu = new ToolStripMenuItem(Resources.SQLEditor);
      editorMenu.Click -= new EventHandler(workbenchConnection_Clicked);

      //// If there are no connections then we disable the SQL Editor menu.
      editorMenu.Enabled = MySqlWorkbench.AllowsExternalConnectionsManagement && boundService.WorkbenchConnections != null && boundService.WorkbenchConnections.Count > 0;
      if (!editorMenu.Enabled)
      {
        return;
      }

      //// If there is only 1 connection then we open Workbench directly from the SQL Editor menu.
      if (boundService.WorkbenchConnections.Count == 1)
      {
        editorMenu.Click += new EventHandler(workbenchConnection_Clicked);
        return;
      }

      //// We have more than 1 connection so we create a submenu
      foreach (MySqlWorkbenchConnection c in boundService.WorkbenchConnections)
      {
        ToolStripMenuItem menu = new ToolStripMenuItem(c.Name);
        menu.Click += new EventHandler(workbenchConnection_Clicked);
        editorMenu.DropDownItems.Add(menu);
      }
    }

    private void restart_Click(object sender, EventArgs e)
    {
      boundService.Restart();
    }

    private void start_Click(object sender, EventArgs e)
    {
      boundService.Start();
    }

    private void stop_Click(object sender, EventArgs e)
    {
      boundService.Stop();
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
        InfoDialog.ShowErrorDialog(Resources.ErrorTitle, Resources.FailureToLaunchWorkbench);
        MySQLSourceTrace.WriteAppErrorToLog(ex);
      }
    }
  }
}