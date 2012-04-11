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
using System.Windows.Forms;
using System.ServiceProcess;
using System.Reflection;
using System.Drawing;
using System.Timers;
using System.Diagnostics;
using System.ComponentModel;

namespace MySql.TrayApp
{
  struct TrayAppSettings
  {
    public bool enableAutoRefresh;
    public string autoRefreshType;
    public int autoRefreshFrequency;
    public bool autoRefreshNotifyChanges;
    public string refreshMethod;
    public string scanForServicesType;
    public string servicesStartWith;
    public bool runAtStartup;
    public bool autoCheckForUpdates;
    public int checkForUpdatesFrequency;
    public List<string> servicesMonitor;
    public List<string> servicesInstalled;
  }

  struct TrayAppSettingValues
  {
    public string autoRefreshTypeOnDemandValue;
    public string autoRefreshTypeByTimerValue;
    public string refreshMethodHardValue;
    public string refreshMethodSoftValue;
    public string scanForServicesTypeStartsWithValue;
    public string scanForServicesTypeMysqldValue;
    public string servicesMonitorValue;
    public string servicesInstalledValue;
  }

  class TrayApp : IDisposable
  {

    private TrayAppSettings trayAppSettings;
    private TrayAppSettingValues trayAppSettingValues;

    private static readonly int MAX_TOOLTIP_LENGHT = 63; // framework constraint for notify icons


    private int timeOutMilliSec = MySQLService.DefaultTimeOut;
    private bool disposed = false;
    private bool hasAdminPrivileges { get; set;}
    private string balloonTitleForServiceStatusChanges { get; set; }
    private string balloonTextForServiceStatusChanges { get; set; }
    private string balloonTitleForServiceListChanges { get; set; }
    private string balloonTextForServiceListChanges { get; set; }

    private static Object lockObject = new Object();
    private static bool refreshingMenus = false;

    private System.ComponentModel.IContainer components;
    private ToolStripMenuItem[] staticMenuItems;
    private NotifyIcon notifyIcon;
    private MySQLServicesList mySQLServicesList {get; set;}
    private System.Timers.Timer timer {get; set;}

    private OptionsForm optionsDialog;

    private ToolStripMenuItem actionsMenuItem
    {
      set { if (staticMenuItems != null && staticMenuItems.Length > 0) staticMenuItems[0] = value; }
      get { return (staticMenuItems != null && staticMenuItems.Length > 0 ? staticMenuItems[0] : null); }
    }

    private ToolStripMenuItem optionsMenuItem
    {
      set { if (staticMenuItems != null && staticMenuItems.Length > 1) staticMenuItems[1] = value; }
      get { return (staticMenuItems != null && staticMenuItems.Length > 1 ? staticMenuItems[1] : null); }
    }

    private ToolStripMenuItem exitMenuItem
    {
      set { if (staticMenuItems != null && staticMenuItems.Length > 2) staticMenuItems[2] = value; }
      get { return (staticMenuItems != null && staticMenuItems.Length > 2 ? staticMenuItems[2] : null); }
    }

   public int ServicesCount
    {
      get { return mySQLServicesList.InstalledMySQLServicesQuantity; }
    }


    public TrayApp(bool adminPrivileges)
    {
      hasAdminPrivileges = adminPrivileges;
      mySQLServicesList = new MySQLServicesList(hasAdminPrivileges);
      mySQLServicesList.ServiceStatusChanged += mySQLServicesList_ServiceStatusChanged;
      mySQLServicesList.ServicesListChanged += mySQLServicesList_ServicesListChanged;


      trayAppSettingValues.autoRefreshTypeOnDemandValue = Properties.Resources.AutoRefreshTypeOnDemand;
      trayAppSettingValues.autoRefreshTypeByTimerValue = Properties.Resources.AutoRefreshTypeByTimer;
      trayAppSettingValues.refreshMethodHardValue = Properties.Resources.RefreshMethodHard;
      trayAppSettingValues.refreshMethodSoftValue = Properties.Resources.RefreshMethodSoft;
      trayAppSettingValues.scanForServicesTypeStartsWithValue = Properties.Resources.ScanForServicesTypeStartsWith;
      trayAppSettingValues.scanForServicesTypeMysqldValue = Properties.Resources.ScanForServicesTypeMysqld;

      trayAppSettings = new TrayAppSettings();
      loadSettings();


      Bitmap iconBitmap = Properties.Resources.default_icon;

      components = new System.ComponentModel.Container();
      notifyIcon = new NotifyIcon(components)
                    {
                      ContextMenuStrip = new ContextMenuStrip(),
                      Icon = Icon.FromHandle(iconBitmap.GetHicon()),
                      Visible = true
                    };
      notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
      notifyIcon.BalloonTipTitle = Properties.Resources.BalloonTitleTextServiceStatus;
      balloonTitleForServiceStatusChanges = Properties.Resources.BalloonTitleTextServiceStatus;
      balloonTextForServiceStatusChanges = Properties.Resources.BalloonTextServiceStatus;
      balloonTitleForServiceListChanges = Properties.Resources.BalloonTitleTextServiceList;
      balloonTextForServiceListChanges = Properties.Resources.BalloonTextServiceList;


      notifyIcon.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

      staticMenuItems = new ToolStripMenuItem[3];
      actionsMenuItem = ServiceMenuGroup.ToolStripMenuItemWithHandler(Properties.Resources.Actions, null);
      actionsMenuItem.DropDownItems.Add(ServiceMenuGroup.ToolStripMenuItemWithHandler(Properties.Resources.RefreshServices, refreshServicesItem_Click, ! trayAppSettings.enableAutoRefresh));
      var manageServicesSubMenu = ServiceMenuGroup.ToolStripMenuItemWithHandler(Properties.Resources.ManageServices, manageServicesItem_Click, !adminPrivileges);
      var shieldBitmap = SystemIcons.Shield.ToBitmap();
      shieldBitmap.SetResolution(16, 16);
      manageServicesSubMenu.Image = shieldBitmap;
      this.actionsMenuItem.DropDownItems.Add(manageServicesSubMenu);
      this.actionsMenuItem.DropDownItems.Add(ServiceMenuGroup.ToolStripMenuItemWithHandler(Properties.Resources.LaunchInstaller, launchInstallerItem_Click, false));
      this.actionsMenuItem.DropDownItems.Add(ServiceMenuGroup.ToolStripMenuItemWithHandler(Properties.Resources.CheckUpdates, checkUpdatesItem_Click, false));
      this.optionsMenuItem = ServiceMenuGroup.ToolStripMenuItemWithHandler(Properties.Resources.Options, optionsItem_Click);
      this.exitMenuItem = ServiceMenuGroup.ToolStripMenuItemWithHandler(Properties.Resources.Exit, exitItem_Click);
      refreshServicesMenus();
    }

    /// <summary>
    /// Cleans-up resources
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);
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
      if (!this.disposed)
      {
        if (disposing)
        {
          if (mySQLServicesList != null)
             mySQLServicesList.Dispose();
          if (this.actionsMenuItem != null)
          {
            if (this.actionsMenuItem.DropDownItems[0] != null && this.actionsMenuItem.DropDownItems[0] is ToolStripMenuItem)
              this.actionsMenuItem.DropDownItems[0].Dispose();
            if (this.actionsMenuItem.DropDownItems[1] != null && this.actionsMenuItem.DropDownItems[1] is ToolStripMenuItem)
              this.actionsMenuItem.DropDownItems[1].Dispose();
            if (this.actionsMenuItem.DropDownItems[2] != null && this.actionsMenuItem.DropDownItems[2] is ToolStripMenuItem)
              this.actionsMenuItem.DropDownItems[2].Dispose();
            if (this.actionsMenuItem.DropDownItems[3] != null && this.actionsMenuItem.DropDownItems[3] is ToolStripMenuItem)
              this.actionsMenuItem.DropDownItems[3].Dispose();
          }
          if (this.optionsMenuItem != null)
            this.optionsMenuItem.Dispose();
          if (this.exitMenuItem != null)
            this.exitMenuItem.Dispose();
          if (this.notifyIcon != null)
            this.notifyIcon.Dispose();
          if (this.timer != null)
            this.timer.Dispose();
          if (this.components != null)
            this.components.Dispose();
        }
      }
      this.disposed = true;
    }


    /// <summary>
    /// Notifies that the TrayApp wants to quit
    /// </summary>
    public event EventHandler Exit;

    /// <summary>
    /// Invokes the Exit event
    /// </summary>
    /// <param name="e">Event arguments</param>
    protected virtual void OnExit(EventArgs e)
    {
      this.notifyIcon.Visible = false; // should remove lingering tray icon
      if (this.optionsDialog != null)
        this.optionsDialog.Close();

      if (this.Exit != null)
        this.Exit(this, e);
    }

    private void mySQLServicesList_ServicesListChanged(object sender, ServicesListChangedArgs args)
    {
      if (trayAppSettings.enableAutoRefresh && trayAppSettings.autoRefreshType == trayAppSettingValues.autoRefreshTypeByTimerValue && trayAppSettings.autoRefreshNotifyChanges)
      {
        this.notifyIcon.BalloonTipTitle = balloonTitleForServiceListChanges;
        this.notifyIcon.BalloonTipText = String.Format(balloonTextForServiceListChanges,
                                                       args.RemovedServicesList.Count,
                                                       args.AddedServicesList.Count);
        this.notifyIcon.ShowBalloonTip(3000);
      }
    }

    private void mySQLServicesList_ServiceStatusChanged(object sender, ServiceStatus args)
    {
      if (trayAppSettings.enableAutoRefresh && trayAppSettings.autoRefreshType == trayAppSettingValues.autoRefreshTypeByTimerValue && trayAppSettings.autoRefreshNotifyChanges)
      {
        this.notifyIcon.BalloonTipTitle = balloonTitleForServiceStatusChanges;
        this.notifyIcon.BalloonTipText = String.Format(balloonTextForServiceStatusChanges,
                                                       args.ServiceName,
                                                       args.PreviousStatus.ToString(),
                                                       args.CurrentStatus.ToString());
        this.notifyIcon.ShowBalloonTip(3000);
      }
    }

    private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
      e.Cancel = false;

      if (trayAppSettings.enableAutoRefresh && trayAppSettings.autoRefreshType == trayAppSettingValues.autoRefreshTypeOnDemandValue)
        refreshServicesMenus();
    }

    private void refreshServicesItem_Click(object sender, EventArgs e)
    {
      refreshServicesMenus();
    }

    private void manageServicesItem_Click(object sender, EventArgs e)
    {
      bool ranElevated = false;

      if (hasAdminPrivileges)
        return;      

      ProcessStartInfo processInfo = new ProcessStartInfo();
      processInfo.Verb = "runas";
      processInfo.FileName = Application.ExecutablePath;
      try
      {
        Process.Start(processInfo);
        ranElevated = true;
      }
      catch (Win32Exception)
      {
        //Do nothing. Probably the user canceled the UAC window
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }      

      //Close this instance because we have an elevated a second instance
      if (ranElevated)
        this.OnExit(EventArgs.Empty);
    }

    private void launchInstallerItem_Click(object sender, EventArgs e)
    {

    }

    private void checkUpdatesItem_Click(object sender, EventArgs e)
    {
      
    }

    private void optionsItem_Click(object sender, EventArgs e)
    {
      if (this.optionsDialog == null)
        this.optionsDialog = new OptionsForm(trayAppSettingValues);
      DialogResult dg = this.optionsDialog.ShowDialog();
      if (dg == DialogResult.OK)
        this.loadSettings();
    }

    /// <summary>
    /// When the exit menu item is clicked, make a call to terminate the ApplicationContext.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void exitItem_Click(object sender, EventArgs e)
    {
      this.OnExit(EventArgs.Empty);
    }

    private void timer_Elapsed(object sender, EventArgs e)
    {
      if (refreshingMenus)
        return;
      refreshServicesMenus();
    }


    /// <summary>
    /// Sets the text displayed in the notify icon's tooltip
    /// </summary>
    public void SetNotifyIconToolTip()
    {
      string toolTipText = string.Format("{0} ({1})\n{2}.",
                                         Properties.Resources.AppName,
                                         Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                                         String.Format(Properties.Resources.ToolTipText,
                                                       this.ServicesCount.ToString()));
      this.notifyIcon.Text = (toolTipText.Length >= MAX_TOOLTIP_LENGHT ? toolTipText.Substring(0, MAX_TOOLTIP_LENGHT - 3) + "..." : toolTipText);
    }


    /// <summary>
    /// Loads all seetings into a local struct
    /// </summary>
    private void loadSettings()
    {
      trayAppSettings.enableAutoRefresh = Properties.Settings.Default.EnableAutoRefresh;
      trayAppSettings.autoRefreshType = Properties.Settings.Default.AutoRefreshType;
      trayAppSettings.autoRefreshFrequency = Properties.Settings.Default.AutoRefreshFrequency;
      trayAppSettings.autoRefreshNotifyChanges = Properties.Settings.Default.AutoRefreshNotifyChanges;
      trayAppSettings.refreshMethod = Properties.Settings.Default.RefreshMethod;
      trayAppSettings.scanForServicesType = Properties.Settings.Default.ScanForServicesType;
      trayAppSettings.servicesStartWith = Properties.Settings.Default.ServicesStartWith;
      trayAppSettings.runAtStartup = Properties.Settings.Default.RunAtStartup;
      trayAppSettings.autoCheckForUpdates = Properties.Settings.Default.AutoCheckForUpdates;
      trayAppSettings.checkForUpdatesFrequency = Properties.Settings.Default.CheckForUpdatesFrequency;
              
      foreach (var item in Properties.Settings.Default.ServicesMonitor)	    
		    trayAppSettings.servicesMonitor.Add(item);
    	
      
      foreach (var item in Properties.Settings.Default.ServicesInstalled)
	       trayAppSettings.servicesInstalled.Add(item);


      if (actionsMenuItem != null && this.actionsMenuItem.DropDownItems.Count > 0)
        actionsMenuItem.DropDownItems[0].Enabled = !trayAppSettings.enableAutoRefresh;

      timer = new System.Timers.Timer();

      if (trayAppSettings.enableAutoRefresh && trayAppSettings.autoRefreshType == trayAppSettingValues.autoRefreshTypeByTimerValue)
      {      
        timer.AutoReset = true;
        timer.Elapsed += timer_Elapsed;

        if (!timer.Enabled)
        {
          timer.Interval = trayAppSettings.autoRefreshFrequency * 1000;
          timer.Enabled = true;
        }       
      }
      else
        timer.Enabled = false;      

    }

    /// <summary>
    /// Refreshes services and their related menu items
    /// </summary>
    private void refreshServicesMenus()
    {
      bool changesDuringRefresh = false;
      refreshingMenus = true;

      //if (trayAppSettings.refreshMethod == _trayAppSettingValues.refreshMethodHardValue || this.notifyIcon.ContextMenuStrip.Items.Count == 0)
      //  changesDuringRefresh = _mySQLServicesList.HardRefreshMySQLServices(false);
      //else if (trayAppSettings.refreshMethod == _trayAppSettingValues.refreshMethodSoftValue)
      //  changesDuringRefresh = _mySQLServicesList.SoftRefreshMySQLServices();

      //if (changesDuringRefresh)
      //{
      //  this.notifyIcon.ContextMenuStrip.Items.Clear();
      //  foreach (MySQLService mySqlServ in _mySQLServicesList.InstalledMySQLServicesList)
      //  {
      //    foreach (ToolStripMenuItem item in mySqlServ.MenuGroup.ServiceMenuItems)
      //    {
      //      this.notifyIcon.ContextMenuStrip.Items.Add(item);
      //    }
      //    this.notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
      //  }
      //  foreach (ToolStripMenuItem item in _staticMenuItems)
      //  {
      //    this.notifyIcon.ContextMenuStrip.Items.Add(item);
      //  }
      //  this.SetNotifyIconToolTip();
      //}
      refreshingMenus = false;
    }    
  }
}
