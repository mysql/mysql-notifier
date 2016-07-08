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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Notifier.Enumerations;
using MySql.Notifier.Properties;
using MySQL.Utility.Classes.MySQLWorkbench;

namespace MySql.Notifier.Classes
{
  /// <summary>
  /// Contains a group of ToolStripMenuItem instances for each of the corresponding MySQLService’s context menu items.
  /// </summary>
  public class ServiceMenuGroup : IDisposable
  {
    #region Fields

    private MySqlService _boundService;

    private ToolStripMenuItem _configureMenu;

    private ToolStripMenuItem _editorMenu;

    private ToolStripMenuItem _restartMenu;

    private ToolStripSeparator _separator;

    private ToolStripMenuItem _startMenu;

    private ToolStripMenuItem _statusMenu;

    private ToolStripMenuItem _stopMenu;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceMenuGroup"/> class.
    /// </summary>
    /// <param name="mySqlBoundService">The service this menu group is bound to.</param>
    public ServiceMenuGroup(MySqlService mySqlBoundService)
    {
      MenuItemsQuantity = 0;
      _boundService = mySqlBoundService;

      _statusMenu = new ToolStripMenuItem(string.Format("{0} - {1}", _boundService.DisplayName, _boundService.Status))
      {
        Tag = _boundService.ServiceId
      };

      if (MySqlWorkbench.AllowsExternalConnectionsManagement)
      {
        _configureMenu = new ToolStripMenuItem(Resources.ConfigureInstance);
        _configureMenu.Click += configureInstanceItem_Click;
        CreateEditorMenus();
      }

      _separator = new ToolStripSeparator();

      var menuItemFont = new Font(_statusMenu.Font, FontStyle.Bold);
      _statusMenu.Font = menuItemFont;

      _startMenu = new ToolStripMenuItem(Resources.StartText, Resources.play);
      _startMenu.Click += start_Click;

      _stopMenu = new ToolStripMenuItem(Resources.StopText, Resources.stop);
      _stopMenu.Click += stop_Click;

      _restartMenu = new ToolStripMenuItem(Resources.RestartText, Resources.restart);
      _restartMenu.Click += restart_Click;

      Update();
    }

    public string BoundServiceName
    {
      get { return _boundService.ServiceName; }
    }

    public int MenuItemsQuantity { get; private set; }

    /// <summary>
    /// Finds the menu item's index within a context menu strip corresponding to the menu item with the given text.
    /// </summary>
    /// <param name="menu"><see cref="ContextMenuStrip"/> containing the itemText to find.</param>
    /// <param name="menuItemId">Menu item ID.</param>
    /// <returns>Index of the dound menu itemText, -1 if  not found.</returns>
    public static int FindMenuItemWithinMenuStrip(ContextMenuStrip menu, string menuItemId)
    {
      int index = -1;

      for (int i = 0; i < menu.Items.Count; i++)
      {
        if (menu.Items[i].Tag == null || !menu.Items[i].Tag.Equals(menuItemId))
        {
          continue;
        }

        index = i;
        break;
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
    public static ToolStripMenuItem ToolStripMenuItemWithHandler(string displayText, string menuName, Image image, EventHandler eventHandler, bool enable)
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
    public static ToolStripMenuItem ToolStripMenuItemWithHandler(string displayText, string menuName, Image image, EventHandler eventHandler)
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
    public static ToolStripMenuItem ToolStripMenuItemWithHandler(string displayText, Image image, EventHandler eventHandler, bool enable)
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
    public static ToolStripMenuItem ToolStripMenuItemWithHandler(string displayText, Image image, EventHandler eventHandler)
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
        menu.Invoke(new MethodInvoker(() => AddToContextMenu(menu)));
      }
      else
      {
        int index = FindMenuItemWithinMenuStrip(menu, _boundService.Host.MachineId);
        int servicesMenusCount = _boundService.Host.Services != null ? _boundService.Host.Services.Sum(s => s != _boundService && s.MenuGroup != null ? s.MenuGroup.MenuItemsQuantity : 0) : 0;
        index += servicesMenusCount;
        if (index < 0)
        {
          return;
        }

        // Show the separator just above this new menu item if needed.
        if (index > 0 && menu.Items[index] is ToolStripSeparator)
        {
          menu.Items[index].Visible = true;
        }

        index++;
        menu.Items.Insert(index, _statusMenu);
        MenuItemsQuantity++;
        if (_boundService.IsRealMySqlService)
        {
          if (_configureMenu != null)
          {
            menu.Items.Insert(++index, _configureMenu);
            MenuItemsQuantity++;
          }

          if (_editorMenu != null)
          {
            menu.Items.Insert(++index, _editorMenu);
            MenuItemsQuantity++;
          }
        }

        menu.Items.Insert(++index, _separator);
        _separator.Visible = menu.Items[index + 1].BackColor != SystemColors.MenuText;
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
        menu.Invoke(new MethodInvoker(() => RefreshMenu(menu)));
      }
      else
      {
        if (!_boundService.IsRealMySqlService)
        {
          return;
        }

        _boundService.FindMatchingWbConnections();

        int index = FindMenuItemWithinMenuStrip(menu, _boundService.ServiceId);
        if (index < 0)
        {
          return;
        }

        // We dispose of ConfigureInstance and SQLEditor items to recreate a clear menu.
        if (menu.Items[index + 1].Text.Equals(Resources.ConfigureInstance))
        {
          menu.Items.RemoveAt(index + 1);
        }

        if (menu.Items[index + 1].Text.Equals(Resources.SQLEditor))
        {
          menu.Items.RemoveAt(index + 1);
        }

        // If Workbench is installed on the system, we add ConfigureInstance and SQLEditor items back.
        if (MySqlWorkbench.AllowsExternalConnectionsManagement)
        {
          if (_configureMenu == null)
          {
            _configureMenu = new ToolStripMenuItem(Resources.ConfigureInstance);
            _configureMenu.Click += configureInstanceItem_Click;
          }

          CreateEditorMenus();

          if (_configureMenu != null)
          {
            menu.Items.Insert(++index, _configureMenu);
          }

          if (_editorMenu != null)
          {
            menu.Items.Insert(++index, _editorMenu);
          }
        }

        menu.Refresh();
      }
    }

    /// <summary>
    /// Removes all menu items related to this service menu group from the main Notifier's context menu.
    /// </summary>
    /// <param name="menu">Main Notifier's context menu.</param>
    public void RemoveFromContextMenu(ContextMenuStrip menu)
    {
      if (menu.InvokeRequired)
      {
        menu.Invoke(new MethodInvoker(() => RemoveFromContextMenu(menu)));
      }
      else
      {
        string[] menuItems = new string[4];

        if (_boundService == null || _boundService.StartupParameters == null)
        {
          return;
        }

        if (_boundService.IsRealMySqlService && MySqlWorkbench.AllowsExternalConnectionsManagement)
        {
          menuItems[0] = Resources.ConfigureInstance;
          menuItems[1] = Resources.SQLEditor;
          menuItems[2] = "Separator";
          menuItems[3] = _statusMenu.Text; // the last itemText we delete is the service name itemText which is the reference for the others
        }
        else
        {
          menuItems[0] = "Separator";
          menuItems[1] = _statusMenu.Text;
        }

        int index = FindMenuItemWithinMenuStrip(menu, _boundService.ServiceId);
        if (index <= 0)
        {
          return;
        }

        // Hide the separator just above this new menu item if needed.
        if (index > 0 && menu.Items[index - 1] is ToolStripSeparator)
        {
          menu.Items[index - 1].Visible = menu.Items[index + 1].BackColor != SystemColors.MenuText;
        }

        foreach (int plusItem in from item in menuItems where !string.IsNullOrEmpty(item) select item.Equals(_statusMenu.Text) ? 0 : 1)
        {
          menu.Items.RemoveAt(index + plusItem);
        }

        menu.Refresh();
      }
    }

    /// <summary>
    /// Enables and disables menus based on the current Service Status
    /// </summary>
    public void Update()
    {
      var notifierMenu = _statusMenu.GetCurrentParent();
      if (notifierMenu != null && notifierMenu.InvokeRequired)
      {
        notifierMenu.Invoke(new MethodInvoker(Update));
      }
      else
      {
        _statusMenu.Text = String.Format("{0} - {1}", _boundService.DisplayName, _boundService.Status);
        Image image = null;
        switch (_boundService.Status)
        {
          case MySqlServiceStatus.ContinuePending:
          case MySqlServiceStatus.Paused:
          case MySqlServiceStatus.PausePending:
          case MySqlServiceStatus.StartPending:
          case MySqlServiceStatus.StopPending:
            image = Resources.NotifierIconStarting;
            break;

          case MySqlServiceStatus.Stopped:
            image = Resources.NotifierIconStopped;
            break;

          case MySqlServiceStatus.Running:
            image = Resources.NotifierIconRunning;
            break;

          case MySqlServiceStatus.Unavailable:
            image = Resources.NotifierIcon;
            break;
        }

        _statusMenu.Image = image;
        _startMenu.Enabled = _boundService.Status == MySqlServiceStatus.Stopped;
        _stopMenu.Enabled = _boundService.Status != MySqlServiceStatus.Stopped;
        _restartMenu.Enabled = _stopMenu.Enabled;

        if (_editorMenu != null)
        {
          _editorMenu.Enabled = MySqlWorkbench.AllowsExternalConnectionsManagement && _boundService.WorkbenchConnections != null && _boundService.WorkbenchConnections.Count > 0;
        }

        if (_configureMenu != null)
        {
          _configureMenu.Enabled = MySqlWorkbench.AllowsExternalConnectionsManagement;
        }

        bool actionMenusAvailable = _boundService.Host.IsOnline;
        if (actionMenusAvailable && _statusMenu.DropDownItems.Count == 0)
        {
          _statusMenu.DropDownItems.Add(_startMenu);
          _statusMenu.DropDownItems.Add(_stopMenu);
          _statusMenu.DropDownItems.Add(_restartMenu);
        }
        else if (!actionMenusAvailable && _statusMenu.DropDownItems.Count > 0)
        {
          _statusMenu.DropDownItems.Clear();
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
        try
        {
          // Free managed resources
          if (_editorMenu != null)
          {
            _editorMenu.Dispose();
          }

          if (_restartMenu != null)
          {
            _restartMenu.Dispose();
          }

          if (_separator != null)
          {
            _separator.Dispose();
          }

          if (_startMenu != null)
          {
            _startMenu.Dispose();
          }

          if (_statusMenu != null)
          {
            _statusMenu.Dispose();
          }

          if (_stopMenu != null)
          {
            _stopMenu.Dispose();
          }

          if (_configureMenu != null)
          {
            _configureMenu.Dispose();
          }
        }
        catch
        {
          // Sometimes when the dispose is done from a thread different than the main one a cross-thread exception is thrown which is not critical
          // since these menu items will be disposed later by the garbage collector. No Exception is being actually handled or logged since we do
          // not wat to overwhelm the log with these error messages since they do not affect the Notifier's execution.
        }
      }

      // Add class finalizer if unmanaged resources are added to the class
      // Free unmanaged resources if there are any
    }

    private void configureInstanceItem_Click(object sender, EventArgs e)
    {
      try
      {
        MySqlWorkbenchServer server = MySqlWorkbench.Servers.FindByServiceName(_boundService.ServiceName);
        MySqlWorkbench.LaunchConfigure(server);
      }
      catch (Exception ex)
      {
        Program.MySqlNotifierErrorHandler(Resources.FailureToLaunchWorkbench, true, ex);
      }
    }

    /// <summary>
    /// Creates the SQL Editor menu item and its drop-down items for the related MySQL Workbench connections.
    /// </summary>
    private void CreateEditorMenus()
    {
      _editorMenu = new ToolStripMenuItem(Resources.SQLEditor);
      _editorMenu.Click -= workbenchConnection_Clicked;

      // If there are no connections then we disable the SQL Editor menu.
      _editorMenu.Enabled = MySqlWorkbench.AllowsExternalConnectionsManagement && _boundService.WorkbenchConnections != null && _boundService.WorkbenchConnections.Count > 0;
      if (!_editorMenu.Enabled || _boundService.WorkbenchConnections == null)
      {
        return;
      }

      // If there is only 1 connection then we open Workbench directly from the SQL Editor menu.
      if (_boundService.WorkbenchConnections.Count == 1)
      {
        _editorMenu.Click += workbenchConnection_Clicked;
        return;
      }

      // We have more than 1 connection so we create a submenu
      foreach (var menu in _boundService.WorkbenchConnections.Select(c => new ToolStripMenuItem(c.Name)))
      {
        menu.Click += workbenchConnection_Clicked;
        _editorMenu.DropDownItems.Add(menu);
      }
    }

    private void restart_Click(object sender, EventArgs e)
    {
      _boundService.Restart();
    }

    private void start_Click(object sender, EventArgs e)
    {
      _boundService.Start();
    }

    private void stop_Click(object sender, EventArgs e)
    {
      _boundService.Stop();
    }

    private void workbenchConnection_Clicked(object sender, EventArgs e)
    {
      try
      {
        if (_boundService.WorkbenchConnections.Count == 0)
        {
          MySqlWorkbench.LaunchSqlEditor(null);
        }
        else if (!_editorMenu.HasDropDownItems)
        {
          MySqlWorkbench.LaunchSqlEditor(_boundService.WorkbenchConnections[0].Name);
        }
        else
        {
          for (int x = 0; x < _editorMenu.DropDownItems.Count; x++)
          {
            if (sender == _editorMenu.DropDownItems[x])
            {
              MySqlWorkbench.LaunchSqlEditor(_boundService.WorkbenchConnections[x].Name);
            }
          }
        }
      }
      catch (Exception ex)
      {
        Program.MySqlNotifierErrorHandler(Resources.FailureToLaunchWorkbench, true, ex);
      }
    }
  }
}