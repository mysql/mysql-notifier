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
  }

  struct TrayAppSettingValues
  {
    public string autoRefreshTypeOnDemandValue;
    public string autoRefreshTypeByTimerValue;
    public string refreshMethodHardValue;
    public string refreshMethodSoftValue;
    public string scanForServicesTypeStartsWithValue;
    public string scanForServicesTypeMysqldValue;
  }

  class TrayApp : IDisposable
  {
    #region Private Members

    private TrayAppSettings trayAppSettings;
    private TrayAppSettingValues trayAppSettingValues;

    private static readonly int MAX_TOOLTIP_LENGHT = 63; // framework constraint for notify icons
    private int timeOutMilliSec = MySQLService.DefaultTimeOut;
    private bool disposed = false;
    private readonly bool hasAdminPrivileges = false;
    private readonly string balloonTitleForServiceStatusChanges;
    private readonly string balloonTextForServiceStatusChanges;
    private readonly string balloonTitleForServiceListChanges;
    private readonly string balloonTextForServiceListChanges;

    private static Object lockObject = new Object();
    private static bool refreshingMenus = false;

    private System.ComponentModel.IContainer components;
    private ToolStripMenuItem[] staticMenuItems;
    private NotifyIcon notifyIcon;
    private MySQLServicesList mySQLServicesList;
    private System.Timers.Timer timer;

    private OptionsForm optionsDialog;

    #endregion Private Members

    #region Private Properties

    private ToolStripMenuItem actionsMenuItem
    {
      set { if (this.staticMenuItems != null && this.staticMenuItems.Length > 0) this.staticMenuItems[0] = value; }
      get { return (this.staticMenuItems != null && this.staticMenuItems.Length > 0 ? this.staticMenuItems[0] : null); }
    }

    private ToolStripMenuItem optionsMenuItem
    {
      set { if (this.staticMenuItems != null && this.staticMenuItems.Length > 1) this.staticMenuItems[1] = value; }
      get { return (this.staticMenuItems != null && this.staticMenuItems.Length > 1 ? this.staticMenuItems[1] : null); }
    }

    private ToolStripMenuItem exitMenuItem
    {
      set { if (this.staticMenuItems != null && this.staticMenuItems.Length > 2) this.staticMenuItems[2] = value; }
      get { return (this.staticMenuItems != null && this.staticMenuItems.Length > 2 ? this.staticMenuItems[2] : null); }
    }

    #endregion Private Properties

    #region Public Properties

    public int ServicesCount
    {
      get { return this.mySQLServicesList.InstalledMySQLServicesQuantity; }
    }

    #endregion Public Properties

    public TrayApp(bool adminPrivileges)
    {
      this.hasAdminPrivileges = adminPrivileges;
      this.mySQLServicesList = new MySQLServicesList(this.hasAdminPrivileges);
      this.mySQLServicesList.ServiceStatusChanged += mySQLServicesList_ServiceStatusChanged;
      this.mySQLServicesList.ServicesListChanged += mySQLServicesList_ServicesListChanged;

      #region Settings

      #region Load Setting Values from Resources

      this.trayAppSettingValues.autoRefreshTypeOnDemandValue = Properties.Resources.AutoRefreshTypeOnDemand;
      this.trayAppSettingValues.autoRefreshTypeByTimerValue = Properties.Resources.AutoRefreshTypeByTimer;
      this.trayAppSettingValues.refreshMethodHardValue = Properties.Resources.RefreshMethodHard;
      this.trayAppSettingValues.refreshMethodSoftValue = Properties.Resources.RefreshMethodSoft;
      this.trayAppSettingValues.scanForServicesTypeStartsWithValue = Properties.Resources.ScanForServicesTypeStartsWith;
      this.trayAppSettingValues.scanForServicesTypeMysqldValue = Properties.Resources.ScanForServicesTypeMysqld;

      #endregion Load Setting Values from Resources

      this.trayAppSettings = new TrayAppSettings();
      this.loadSettings();

      #endregion Settings
      #region Notify Icon initialization

      Bitmap iconBitmap = Properties.Resources.default_icon;

      components = new System.ComponentModel.Container();
      notifyIcon = new NotifyIcon(components)
                    {
                      ContextMenuStrip = new ContextMenuStrip(),
                      Icon = Icon.FromHandle(iconBitmap.GetHicon()),
                      Visible = true
                    };
      this.notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
      this.notifyIcon.BalloonTipTitle = Properties.Resources.BalloonTitleTextServiceStatus;
      balloonTitleForServiceStatusChanges = Properties.Resources.BalloonTitleTextServiceStatus;
      balloonTextForServiceStatusChanges = Properties.Resources.BalloonTextServiceStatus;
      balloonTitleForServiceListChanges = Properties.Resources.BalloonTitleTextServiceList;
      balloonTextForServiceListChanges = Properties.Resources.BalloonTextServiceList;

      #endregion Notify Icon initialization
      #region Context Menu Initialization

      this.notifyIcon.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

      this.staticMenuItems = new ToolStripMenuItem[3];
      this.actionsMenuItem = ServiceMenuGroup.ToolStripMenuItemWithHandler(Properties.Resources.Actions, null);
      this.actionsMenuItem.DropDownItems.Add(ServiceMenuGroup.ToolStripMenuItemWithHandler(Properties.Resources.RefreshServices, refreshServicesItem_Click, !this.trayAppSettings.enableAutoRefresh));
      var manageServicesSubMenu = ServiceMenuGroup.ToolStripMenuItemWithHandler(Properties.Resources.ManageServices, manageServicesItem_Click, !adminPrivileges);
      var shieldBitmap = SystemIcons.Shield.ToBitmap();
      shieldBitmap.SetResolution(16, 16);
      manageServicesSubMenu.Image = shieldBitmap;
      this.actionsMenuItem.DropDownItems.Add(manageServicesSubMenu);
      this.actionsMenuItem.DropDownItems.Add(ServiceMenuGroup.ToolStripMenuItemWithHandler(Properties.Resources.LaunchInstaller, launchInstallerItem_Click, false));
      this.actionsMenuItem.DropDownItems.Add(ServiceMenuGroup.ToolStripMenuItemWithHandler(Properties.Resources.CheckUpdates, checkUpdatesItem_Click, false));
      this.optionsMenuItem = ServiceMenuGroup.ToolStripMenuItemWithHandler(Properties.Resources.Options, optionsItem_Click);
      this.exitMenuItem = ServiceMenuGroup.ToolStripMenuItemWithHandler(Properties.Resources.Exit, exitItem_Click);

      #endregion Context Menu Initialization

      this.refreshServicesMenus();
    }

    #region Dispose Pattern Methods

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
          if (this.mySQLServicesList != null)
            this.mySQLServicesList.Dispose();
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

    #endregion Dispose Pattern Methods

    # region Events

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
      #region Cleanup

      this.notifyIcon.Visible = false; // should remove lingering tray icon
      if (this.optionsDialog != null)
        this.optionsDialog.Close();

      #endregion Cleanup
      
      if (this.Exit != null)
        this.Exit(this, e);
    }

    private void mySQLServicesList_ServicesListChanged(object sender, ServicesListChangedArgs args)
    {
      if (this.trayAppSettings.enableAutoRefresh && this.trayAppSettings.autoRefreshType == this.trayAppSettingValues.autoRefreshTypeByTimerValue && this.trayAppSettings.autoRefreshNotifyChanges)
      {
        this.notifyIcon.BalloonTipTitle = balloonTitleForServiceListChanges;
        this.notifyIcon.BalloonTipText = String.Format(balloonTextForServiceListChanges,
                                                       args.RemovedServicesList.Count,
                                                       args.AddedServicesList.Count);
        this.notifyIcon.ShowBalloonTip(3000);
      }
    }

    private void mySQLServicesList_ServiceStatusChanged(object sender, ServiceStatusChangedArgs args)
    {
      if (this.trayAppSettings.enableAutoRefresh && this.trayAppSettings.autoRefreshType == this.trayAppSettingValues.autoRefreshTypeByTimerValue && this.trayAppSettings.autoRefreshNotifyChanges)
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

      if (this.trayAppSettings.enableAutoRefresh && this.trayAppSettings.autoRefreshType == this.trayAppSettingValues.autoRefreshTypeOnDemandValue)
        this.refreshServicesMenus();
    }

    private void refreshServicesItem_Click(object sender, EventArgs e)
    {
      this.refreshServicesMenus();
    }

    private void manageServicesItem_Click(object sender, EventArgs e)
    {
      bool ranElevated = false;

      if (this.hasAdminPrivileges)
        return;

      #region Run Elevated

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

      #endregion Run Elevated

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
        this.optionsDialog = new OptionsForm(this.trayAppSettingValues);
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
      this.refreshServicesMenus();
    }

    #endregion Events

    # region Context Menus

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

    # endregion Context Menus

    #region Methods

    /// <summary>
    /// Loads all seetings into a local struct
    /// </summary>
    private void loadSettings()
    {
      this.trayAppSettings.enableAutoRefresh = Properties.Settings.Default.EnableAutoRefresh;
      this.trayAppSettings.autoRefreshType = Properties.Settings.Default.AutoRefreshType;
      this.trayAppSettings.autoRefreshFrequency = Properties.Settings.Default.AutoRefreshFrequency;
      this.trayAppSettings.autoRefreshNotifyChanges = Properties.Settings.Default.AutoRefreshNotifyChanges;
      this.trayAppSettings.refreshMethod = Properties.Settings.Default.RefreshMethod;
      this.trayAppSettings.scanForServicesType = Properties.Settings.Default.ScanForServicesType;
      this.trayAppSettings.servicesStartWith = Properties.Settings.Default.ServicesStartWith;
      this.trayAppSettings.runAtStartup = Properties.Settings.Default.RunAtStartup;
      this.trayAppSettings.autoCheckForUpdates = Properties.Settings.Default.AutoCheckForUpdates;
      this.trayAppSettings.checkForUpdatesFrequency = Properties.Settings.Default.CheckForUpdatesFrequency;

      #region AutoRefresh Settings

      if (this.actionsMenuItem != null && this.actionsMenuItem.DropDownItems.Count > 0)
        this.actionsMenuItem.DropDownItems[0].Enabled = !this.trayAppSettings.enableAutoRefresh;

      #endregion AutoRefresh Settings
      # region Timer Settings

      if (this.trayAppSettings.enableAutoRefresh && this.trayAppSettings.autoRefreshType == this.trayAppSettingValues.autoRefreshTypeByTimerValue)
      {
        if (this.timer == null)
        {
          this.timer = new System.Timers.Timer();
          this.timer.AutoReset = true;
          this.timer.Elapsed += timer_Elapsed;
        }
        this.timer.Interval = this.trayAppSettings.autoRefreshFrequency * 1000;
        this.timer.Enabled = true;
      }
      else
        if (this.timer != null)
          this.timer.Enabled = false;

      # endregion Timer Settings
      #region Scan For Services Settings

      this.mySQLServicesList.ScanForServicesPrefix = this.trayAppSettings.servicesStartWith;

      #endregion Scan For Services Settings
    }

    /// <summary>
    /// Refreshes services and their related menu items
    /// </summary>
    private void refreshServicesMenus()
    {
      bool changesDuringRefresh = false;
      refreshingMenus = true;

      if (this.trayAppSettings.refreshMethod == this.trayAppSettingValues.refreshMethodHardValue || this.notifyIcon.ContextMenuStrip.Items.Count == 0)
        changesDuringRefresh = this.mySQLServicesList.HardRefreshMySQLServices(false);
      else if (this.trayAppSettings.refreshMethod == this.trayAppSettingValues.refreshMethodSoftValue)
        changesDuringRefresh = this.mySQLServicesList.SoftRefreshMySQLServices();

      if (changesDuringRefresh)
      {
        this.notifyIcon.ContextMenuStrip.Items.Clear();
        foreach (MySQLService mySqlServ in this.mySQLServicesList.InstalledMySQLServicesList)
        {
          foreach (ToolStripMenuItem item in mySqlServ.MenuGroup.ServiceMenuItems)
          {
            this.notifyIcon.ContextMenuStrip.Items.Add(item);
          }
          this.notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
        }
        foreach (ToolStripMenuItem item in this.staticMenuItems)
        {
          this.notifyIcon.ContextMenuStrip.Items.Add(item);
        }
        this.SetNotifyIconToolTip();
      }

      refreshingMenus = false;
    }

    #endregion Methods

  }
}
