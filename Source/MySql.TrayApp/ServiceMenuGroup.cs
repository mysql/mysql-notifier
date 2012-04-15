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

namespace MySql.TrayApp
{

  /// <summary>
  /// Contains a group of ToolStripMenuItem instances for each of the corresponding MySQLService’s context menu items.
  /// </summary>
  class ServiceMenuGroup : IDisposable
  {

    public enum AvailableActions
    { Start, Stop, ReStart };


    private bool disposed = false;
    private MySQLService boundService;
    private ToolStripMenuItem[] serviceMenuItems;
    
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

    private ToolStripMenuItem mainMenuItem
    {
      set { if (serviceMenuItems.Length > 0) serviceMenuItems[0] = value; }
      get { return (serviceMenuItems.Length > 0 ? serviceMenuItems[0] : null); }
    }

    private ToolStripMenuItem configureMenuItem
    {
      set { if (serviceMenuItems.Length > 1) serviceMenuItems[1] = value; }
      get { return (serviceMenuItems.Length > 0 ? serviceMenuItems[1] : null); }
    }

    private ToolStripMenuItem sqlEditorMenuItem
    {
      set { if (serviceMenuItems.Length > 2) serviceMenuItems[2] = value; }
      get { return (serviceMenuItems.Length > 0 ? serviceMenuItems[2] : null); }
    }

    public string BoundServiceName
    {
      get { return boundService.ServiceName; }
    }

    public ServiceControllerStatus BoundServiceStatus
    {
      get { return boundService.CurrentStatus; }
    }

    public ToolStripMenuItem[] ServiceMenuItems
    {
      get { return serviceMenuItems; }
    }

    public ServiceMenuGroup(MySQLService mySQLBoundService)
    {
      boundService = mySQLBoundService;
      boundService.StatusChanged += boundService_StatusChanged;

      serviceMenuItems = new ToolStripMenuItem[3];
      mainMenuItem = ToolStripMenuItemWithHandler(String.Empty, String.Format("mnuService{0}", boundService.ServiceName), null, null);
      configureMenuItem = ToolStripMenuItemWithHandler(Properties.Resources.ConfigureInstance, "mnuServiceConfigure", null, configureInstanceItem_Click);
      sqlEditorMenuItem = ToolStripMenuItemWithHandler(Properties.Resources.SQLEditor, "mnuServiceSQLEditor", null, sqlEditorItem_Click);
      
      //Enables/Disables options that require with Workbench
      sqlEditorMenuItem.Enabled = Utilities.IsApplicationInstalled("Workbench") && connectionStringName != String.Empty;

      configureMenuItem.Enabled = Utilities.IsApplicationInstalled("Workbench") && serverName != String.Empty;

      System.Drawing.Font menuItemFont = new System.Drawing.Font(mainMenuItem.Font, System.Drawing.FontStyle.Bold);
      System.Drawing.Font subMenuItemFont = new System.Drawing.Font(mainMenuItem.Font, System.Drawing.FontStyle.Regular);
      mainMenuItem.Font = menuItemFont;

      foreach (string action in Enum.GetNames(typeof(AvailableActions)))
      {
        var subMenuItem = ToolStripMenuItemWithHandler(action, serviceActionItem_Click);
        subMenuItem.Font = subMenuItemFont;
        switch (action)
        {
          case "Start":
            subMenuItem.Image = Properties.Resources.play;
            break;
          case "Stop":
            subMenuItem.Image = Properties.Resources.stop;
            break;
        }
        mainMenuItem.DropDownItems.Add(subMenuItem);
      }
      RefreshMenus(BoundServiceName, BoundServiceStatus);
    }

    /// <summary>
    /// Cleans-up resources
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes of managed and unmanaged resources.
    /// </summary>
    /// <param name="disposing">If true, the method has been called directly or indirectly by a user's code. Managed and unmanaged
    /// resources can be disposed. If false, the method has been called by the runtime from inside the finalizer and you should not
    /// reference other objects. Only unmanaged resources can be disposed.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (!disposed)
      {
        if (disposing)
        {

          var copyOfSM = serviceMenuItems;
          foreach (ToolStripMenuItem subMenuItem in copyOfSM)
          {
            if (subMenuItem != null)
              serviceMenuItems.First(t => t == subMenuItem).Dispose();                                
          }
        }
      }
      disposed = true;
    }

    /// <summary>
    /// Enables and disables menus based on the current Service Status
    /// </summary>
    /// <param name="boundServiceName">Service Name</param>
    /// <param name="boundServiceStatus">Service Status</param>
    public void RefreshMenus(string boundServiceName, ServiceControllerStatus boundServiceStatus)
    {      

      mainMenuItem.Text = String.Format("{0} - {1}",
                                             boundServiceName,
                                             boundServiceStatus.ToString());
      System.Drawing.Image image = null;
      switch (boundServiceStatus)
      {
        case ServiceControllerStatus.ContinuePending:
        case ServiceControllerStatus.Paused:
        case ServiceControllerStatus.PausePending:
        case ServiceControllerStatus.StartPending:
        case ServiceControllerStatus.StopPending:
          image = Properties.Resources.starting_icon;
          break;
        case ServiceControllerStatus.Stopped:
          image = Properties.Resources.stopped_icon;
          break;
        case ServiceControllerStatus.Running:
          image = Properties.Resources.running_icon;
          break;
      }
      mainMenuItem.Image = image;
      
      foreach (ToolStripMenuItem subMenuItem in mainMenuItem.DropDownItems)
      {
        subMenuItem.Enabled = false;
      }

      if (boundService.HasAdminPrivileges)
      {
        switch (boundServiceStatus)
        {
          case ServiceControllerStatus.ContinuePending:
          case ServiceControllerStatus.Paused:
          case ServiceControllerStatus.PausePending:
          case ServiceControllerStatus.StartPending:
          case ServiceControllerStatus.StopPending:
            mainMenuItem.DropDownItems[1].Enabled = true;
            break;
          case ServiceControllerStatus.Stopped:
            mainMenuItem.DropDownItems[0].Enabled = true;
            break;
          case ServiceControllerStatus.Running:
            mainMenuItem.DropDownItems[1].Enabled = serviceMenuItems[0].DropDownItems[2].Enabled = true;
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

    private void serviceActionItem_Click(object sender, EventArgs e)
    {
      if (sender == null)
        return;

      var subMenuItem = sender as ToolStripMenuItem;

      bool actionSuccesful = false;

      Cursor.Current = Cursors.WaitCursor;
      switch (subMenuItem.Text)
      {
        case "Stop":
          actionSuccesful = boundService.Stop();
          break;
        case "Start":
          actionSuccesful = boundService.Start();
          break;
        case "ReStart":
          actionSuccesful = boundService.Restart();
          break;
      }
      mainMenuItem.ForeColor = (actionSuccesful ? System.Drawing.Color.Black : System.Drawing.Color.Red);
      Cursor.Current = Cursors.Default;
    }

    void boundService_StatusChanged(object sender, ServiceStatus args)
    {
      RefreshMenus(args.ServiceName, args.CurrentStatus);
    }

  }
}
