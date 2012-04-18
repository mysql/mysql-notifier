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

namespace MySql.TrayApp
{

  /// <summary>
  /// Contains a group of ToolStripMenuItem instances for each of the corresponding MySQLService’s context menu items.
  /// </summary>
  public class ServiceMenuGroup
  {
    private ToolStripMenuItem statusMenu;
    private ToolStripMenuItem configureMenu;
    private ToolStripMenuItem editorMenu;
    private ToolStripSeparator separator;

    public enum AvailableActions
    { Start, Stop, ReStart };


    private MySQLService boundService;
    
    private string connectionStringName
    {
      get {
        return MySqlServiceInformation.GetConnectionString(boundService.ServiceName);
      }    
    }

    private string serverName
    {
      get
      {
        return MySqlServiceInformation.GetServerName(boundService.ServiceName);      
      }
    }

    public string BoundServiceName
    {
      get { return boundService.ServiceName; }
    }

    public ServiceMenuGroup(MySQLService mySQLBoundService)
    {
      boundService = mySQLBoundService;

      //serviceMenuItems = new ToolStripMenuItem[3];
      statusMenu = new ToolStripMenuItem(String.Format("{0} - {1}", boundService.ServiceName, boundService.Status));
      configureMenu = new ToolStripMenuItem(Resources.ConfigureInstance);
      editorMenu = new ToolStripMenuItem(Resources.SQLEditor);
      
      //Enables/Disables options that require with Workbench
      editorMenu.Enabled = Utilities.IsApplicationInstalled("Workbench") && connectionStringName != String.Empty;
      configureMenu.Enabled = Utilities.IsApplicationInstalled("Workbench") && serverName != String.Empty;

      separator = new ToolStripSeparator(); 

      Font menuItemFont = new Font(statusMenu.Font, FontStyle.Bold);
      Font subMenuItemFont = new Font(statusMenu.Font, FontStyle.Regular);
      statusMenu.Font = menuItemFont;

      ToolStripMenuItem start = new ToolStripMenuItem("Start", Resources.play);
      start.Click += new EventHandler(start_Click);
      ToolStripMenuItem stop = new ToolStripMenuItem("Stop", Resources.stop);
      stop.Click += new EventHandler(stop_Click);
      ToolStripMenuItem restart = new ToolStripMenuItem("Restart");
      restart.Click += new EventHandler(restart_Click);
      statusMenu.DropDownItems.Add(start);
      statusMenu.DropDownItems.Add(stop);
      statusMenu.DropDownItems.Add(restart);

      Update();
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

      //statusMenu.Text = String.Format("{0} - {1}",
      //                                       boundServiceName,
      //                                       boundServiceStatus.ToString());
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

      foreach (ToolStripMenuItem subMenuItem in statusMenu.DropDownItems)
      {
        subMenuItem.Enabled = false;
      }

      if (boundService.HasAdminPrivileges)
      {
        switch (boundService.Status)
        {
          case ServiceControllerStatus.ContinuePending:
          case ServiceControllerStatus.Paused:
          case ServiceControllerStatus.PausePending:
          case ServiceControllerStatus.StartPending:
          case ServiceControllerStatus.StopPending:
            statusMenu.DropDownItems[1].Enabled = true;
            break;
          case ServiceControllerStatus.Stopped:
            statusMenu.DropDownItems[0].Enabled = true;
            break;
          case ServiceControllerStatus.Running:
            statusMenu.DropDownItems[1].Enabled = false; //serviceMenuItems[0].DropDownItems[2].Enabled = true;
            break;
        }
      }
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

    private void configureInstanceItem_Click(object sender, EventArgs e)
    {
      if (sender == null)
        return;

      try
      {
        ProcessStartInfo startInfo = new ProcessStartInfo();

        startInfo.FileName = Utilities.GetWorkBenchPath();
        startInfo.Arguments = "-admin " + MySqlServiceInformation.GetServerName(boundService.ServiceName);
        Process.Start(startInfo);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }      

    }

    private void sqlEditorItem_Click(object sender, EventArgs e)
    {
      if (sender == null)
        return;
      try
      {
        ProcessStartInfo startInfo = new ProcessStartInfo();

        startInfo.FileName = Utilities.GetWorkBenchPath();
        startInfo.Arguments = "-query " + MySqlServiceInformation.GetConnectionString(boundService.ServiceName);
        Process.Start(startInfo);
      }      
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }      
    }
  }
}
