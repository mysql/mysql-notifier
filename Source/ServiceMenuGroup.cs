// Copyright (c) 2006-2008 MySQL AB, 2008-2009 Sun Microsystems, Inc.
//
// MySQL Connector/NET is licensed under the terms of the GPLv2
// <http://www.gnu.org/licenses/old-licenses/gpl-2.0.html>, like most 
// MySQL Connectors. There are special exceptions to the terms and 
// conditions of the GPLv2 as it is applied to this software, see the 
// FLOSS License Exception
// <http://www.mysql.com/about/legal/licensing/foss-exception.html>.
//
// This program is free software; you can redistribute it and/or modify 
// it under the terms of the GNU General Public License as published 
// by the Free Software Foundation; version 2 of the License.
//
// This program is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License 
// for more details.
//
// You should have received a copy of the GNU General Public License along 
// with this program; if not, write to the Free Software Foundation, Inc., 
// 51 Franklin St, Fifth Floor, Boston, MA 02110-1301  USA

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceProcess;
using System.Diagnostics;
using System.IO;
using MySql.TrayApp.Properties;
using System.Drawing;
using MySQL.Utility;

namespace MySql.TrayApp
{

  /// <summary>
  /// Contains a group of ToolStripMenuItem instances for each of the corresponding MySQLService’s context menu items.
  /// </summary>
  public class ServiceMenuGroup
  {
    private ToolStripMenuItem statusMenu;
    private ToolStripMenuItem startMenu;
    private ToolStripMenuItem stopMenu;
    private ToolStripMenuItem restartMenu;
    private ToolStripMenuItem configureMenu;
    private ToolStripMenuItem editorMenu;
    private ToolStripSeparator separator;
    private MySQLService boundService;
    
    public ServiceMenuGroup(MySQLService mySQLBoundService)
    {
      boundService = mySQLBoundService;

      statusMenu = new ToolStripMenuItem(String.Format("{0} - {1}", boundService.ServiceName, boundService.Status));
      configureMenu = new ToolStripMenuItem(Resources.ConfigureInstance);

      CreateEditorMenus();
      MySqlWorkbenchServer server = MySqlWorkbench.Servers.FindByServiceName(boundService.ServiceName);
      configureMenu.Enabled = MySqlWorkbench.IsInstalled &&  server != null;

      separator = new ToolStripSeparator(); 

      Font menuItemFont = new Font(statusMenu.Font, FontStyle.Bold);
      Font subMenuItemFont = new Font(statusMenu.Font, FontStyle.Regular);
      statusMenu.Font = menuItemFont;

      startMenu = new ToolStripMenuItem("Start", Resources.play);
      startMenu.Click += new EventHandler(start_Click);

      stopMenu = new ToolStripMenuItem("Stop", Resources.stop);
      stopMenu.Click += new EventHandler(stop_Click);

      restartMenu = new ToolStripMenuItem("Restart");
      restartMenu.Click += new EventHandler(restart_Click);

      configureMenu.Click += new EventHandler(configureInstanceItem_Click);

      statusMenu.DropDownItems.Add(startMenu);
      statusMenu.DropDownItems.Add(stopMenu);
      statusMenu.DropDownItems.Add(restartMenu);

      Update();
    }

    private void CreateEditorMenus()
    {
      editorMenu = new ToolStripMenuItem(Resources.SQLEditor);
      editorMenu.Enabled = false;
      if (!MySqlWorkbench.IsInstalled) return;

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

    void workbenchConnection_Clicked(object sender, EventArgs e)
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
        MessageBox.Show(String.Format(Resources.FailureToLaunchWorkbench, ex.Message), Resources.ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }


    private void configureInstanceItem_Click(object sender, EventArgs e)
    {
      try
      {
        MySqlWorkbenchServer server = MySqlWorkbench.Servers.FindByServiceName(boundService.ServiceName);
        server.Configure();
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

    }

    public string BoundServiceName
    {
      get { return boundService.ServiceName; }
    }

    void restart_Click(object sender, EventArgs e)
    {
      boundService.Restart();
    }

    void stop_Click(object sender, EventArgs e)
    {
      boundService.Stop();
    }

    void start_Click(object sender, EventArgs e)
    {
      boundService.Start();
    }

    public void AddToContextMenu(ContextMenuStrip menu)
    {
      menu.Items.Insert(0, statusMenu);
      menu.Items.Insert(1, configureMenu);
      menu.Items.Insert(2, editorMenu);
      menu.Items.Insert(3, separator);
    }

    public void RemoveFromContextMenu(ContextMenuStrip menu)
    {
      menu.Items.Remove(statusMenu);
      menu.Items.Remove(configureMenu);
      menu.Items.Remove(editorMenu);
      menu.Items.Remove(separator);
    }

    /// <summary>
    /// Enables and disables menus based on the current Service Status
    /// </summary>
    /// <param name="boundServiceName">Service Name</param>
    /// <param name="boundServiceStatus">Service Status</param>
    public void Update()
    {      
      statusMenu.Text = String.Format("{0} - {1}", boundService.ServiceName, boundService.Status);
      Image image = null;
      switch (boundService.Status)
      {
        case ServiceControllerStatus.ContinuePending:
        case ServiceControllerStatus.Paused:
        case ServiceControllerStatus.PausePending:
        case ServiceControllerStatus.StartPending:
        case ServiceControllerStatus.StopPending:
          image = Resources.starting_icon;
          break;
        case ServiceControllerStatus.Stopped:
          image = Resources.stopped_icon;
          break;
        case ServiceControllerStatus.Running:
          image = Resources.running_icon;
          break;
      }
      statusMenu.Image = image;

      startMenu.Enabled = boundService.Status == ServiceControllerStatus.Stopped;
      stopMenu.Enabled = boundService.Status != ServiceControllerStatus.Stopped;

      bool wbInstalled = MySqlWorkbench.IsInstalled;
      editorMenu.Enabled = wbInstalled;
      configureMenu.Enabled = wbInstalled;
    }


    /// <summary>
    /// Adds a new item to the Notify Icon's context menu.
    /// </summary>
    /// <param name="displayText">Menu item's text</param>
    /// <param name="menuName">Menu item object's name</param>
    /// <param name="image">Menu item's icon displayed at its left</param>
    /// <param name="eventHandler">Event handler method to register with the Click event</param>
    /// <param name="enable">Flag that indicates the Enabled status of the menu item</param>
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
    /// Adds a new item to the Notify Icon's context menu.
    /// </summary>
    /// <param name="displayText">Menu item's text</param>
    /// <param name="menuName">Menu item object's name</param>
    /// <param name="image">Menu item's icon displayed at its left</param>
    /// <param name="eventHandler">Event handler method to register with the Click event</param>
    /// <returns>A new ToolStripMenuItem object</returns>
    public static ToolStripMenuItem ToolStripMenuItemWithHandler(string displayText, string menuName, System.Drawing.Image image, EventHandler eventHandler)
    {
      return ToolStripMenuItemWithHandler(displayText, menuName, image, eventHandler, true);
    }

    /// <summary>
    /// Adds a new item to the Notify Icon's context menu.
    /// </summary>
    /// <param name="displayText">Menu item's text</param>
    /// <param name="image">Menu item's icon displayed at its left</param>
    /// <param name="eventHandler">Event handler method to register with the Click event</param>
    /// <param name="enable">Flag that indicates the Enabled status of the menu item</param>
    /// <returns>A new ToolStripMenuItem object</returns>
    public static ToolStripMenuItem ToolStripMenuItemWithHandler(string displayText, System.Drawing.Image image, EventHandler eventHandler, bool enable)
    {
      return ToolStripMenuItemWithHandler(displayText, String.Format("mnu{0}", displayText.Replace(" ", String.Empty)), image, eventHandler, enable);
    }

    /// <summary>
    /// Adds a new item to the Notify Icon's context menu.
    /// </summary>
    /// <param name="displayText">Menu item's text</param>
    /// <param name="image">Menu item's icon displayed at its left</param>
    /// <param name="eventHandler">Event handler method to register with the Click event</param>
    /// <returns>A new ToolStripMenuItem object</returns>
    public static ToolStripMenuItem ToolStripMenuItemWithHandler(string displayText, System.Drawing.Image image, EventHandler eventHandler)
    {
      return ToolStripMenuItemWithHandler(displayText, image, eventHandler, true);
    }

    /// <summary>
    /// Adds a new item to the Notify Icon's context menu.
    /// </summary>
    /// <param name="displayText">Menu item's text</param>
    /// <param name="eventHandler">Event handler method to register with the Click event</param>
    /// <param name="enable">Flag that indicates the Enabled status of the menu item</param>
    /// <returns>A new ToolStripMenuItem object</returns>
    public static ToolStripMenuItem ToolStripMenuItemWithHandler(string displayText, EventHandler eventHandler, bool enable)
    {
      return ToolStripMenuItemWithHandler(displayText, null, eventHandler, enable);
    }

    /// <summary>
    /// Adds a new item to the Notify Icon's context menu.
    /// </summary>
    /// <param name="displayText">Menu item's text</param>
    /// <param name="eventHandler">Event handler method to register with the Click event</param>
    /// <returns>A new ToolStripMenuItem object</returns>
    public static ToolStripMenuItem ToolStripMenuItemWithHandler(string displayText, EventHandler eventHandler)
    {
      return ToolStripMenuItemWithHandler(displayText, null, eventHandler);
    }

  

  }
}
