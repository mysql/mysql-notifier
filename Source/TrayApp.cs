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
using System.Linq;
using System.Management;


namespace MySql.TrayApp
{
  struct TrayAppSettings
  {
    public bool enableAutoRefresh;
    public RefreshTypeEnum autoRefreshType; //Ondemand or by timer
    public int autoRefreshFrequency;
    public bool autoRefreshNotifyChanges;  // TODO  
    public bool runAtStartup;
    public bool autoCheckForUpdates;
    public int checkForUpdatesFrequency;
    public bool autoAddNewServices;
    public List<string> servicesMonitor;    
  }


  public enum RefreshTypeEnum : int
  {
    OnDemand = 0,
    ByTimer = 1
  }


  class TrayApp : IDisposable
  {

    private TrayAppSettings trayAppSettings;
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
    private ManageServicesDlg manageServicesDialog;
    private About aboutForm;
    private ManagementEventWatcher watcher;

    private delegate void AutoAddNewServiceDelegate(string ServiceName);
      
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

    private ToolStripMenuItem manageServicesDlgItem
    {
      set { if (staticMenuItems != null && staticMenuItems.Length > 2) staticMenuItems[2] = value; }
      get { return (staticMenuItems != null && staticMenuItems.Length > 2 ? staticMenuItems[2] : null); }
    }


    private ToolStripMenuItem aboutMenu
    {
      set { if (staticMenuItems != null && staticMenuItems.Length > 3) staticMenuItems[3] = value; }
      get { return (staticMenuItems != null && staticMenuItems.Length > 3 ? staticMenuItems[3] : null); }
    }

    private ToolStripMenuItem exitMenuItem
    {
      set { if (staticMenuItems != null && staticMenuItems.Length > 4) staticMenuItems[4] = value; }
      get { return (staticMenuItems != null && staticMenuItems.Length > 4 ? staticMenuItems[4] : null); }
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

      staticMenuItems = new ToolStripMenuItem[5];
      actionsMenuItem = ServiceMenuGroup.ToolStripMenuItemWithHandler(Properties.Resources.Actions, null);
      actionsMenuItem.DropDownItems.Add(ServiceMenuGroup.ToolStripMenuItemWithHandler(Properties.Resources.RefreshServices, refreshServicesItem_Click, ! trayAppSettings.enableAutoRefresh));
      manageServicesDlgItem = ServiceMenuGroup.ToolStripMenuItemWithHandler(Properties.Resources.ManageServices, manageServicesDialogItem_Click);


      var manageServicesSubMenu = ServiceMenuGroup.ToolStripMenuItemWithHandler(Properties.Resources.ManageServices, manageServicesItem_Click, !adminPrivileges);
      var shieldBitmap = SystemIcons.Shield.ToBitmap();
      shieldBitmap.SetResolution(16, 16);
      manageServicesSubMenu.Image = shieldBitmap;
      actionsMenuItem.DropDownItems.Add(manageServicesSubMenu);
      actionsMenuItem.DropDownItems.Add(ServiceMenuGroup.ToolStripMenuItemWithHandler(Properties.Resources.LaunchInstaller, launchInstallerItem_Click, false));
      actionsMenuItem.DropDownItems.Add(ServiceMenuGroup.ToolStripMenuItemWithHandler(Properties.Resources.CheckUpdates, checkUpdatesItem_Click, false));
      optionsMenuItem = ServiceMenuGroup.ToolStripMenuItemWithHandler(Properties.Resources.Options, optionsItem_Click);
      exitMenuItem = ServiceMenuGroup.ToolStripMenuItemWithHandler(Properties.Resources.Exit, exitItem_Click);
      aboutMenu = ServiceMenuGroup.ToolStripMenuItemWithHandler(Properties.Resources.About, aboutMenu_Click);

      refreshServicesMenus();

      // listener for events
      var managementScope = new ManagementScope(@"root\cimv2");
      managementScope.Connect();

      WqlEventQuery query = new WqlEventQuery("__InstanceModificationEvent", new TimeSpan(0, 0, 1), "TargetInstance isa \"Win32_Service\" AND ( TargetInstance.Name LIKE \"%MYSQL%\" OR TargetInstance.PathName LIKE \"%MYSQL%\" ) ");
      watcher = new ManagementEventWatcher(managementScope, query);
      watcher.EventArrived += new EventArrivedEventHandler(watcher_EventArrived);
      watcher.Start();
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
      if (!this.disposed)
      {
        if (disposing)
        {
          if (mySQLServicesList != null)
             mySQLServicesList.Dispose();
          if (this.actionsMenuItem != null)
          {
            int totalItems = actionsMenuItem.DropDownItems.Count;
            for (int i = 0; i < totalItems; i++)
            {
              if (actionsMenuItem.DropDownItems[actionsMenuItem.DropDownItems.Count - 1] != null && actionsMenuItem.DropDownItems[actionsMenuItem.DropDownItems.Count - 1] is ToolStripMenuItem)
                actionsMenuItem.DropDownItems[actionsMenuItem.DropDownItems.Count - 1 ].Dispose();            
            
            }            
          }

          if (staticMenuItems != null)
          {
            foreach (var item in staticMenuItems)
            {
              item.Dispose();
            }

          } 

          if (this.notifyIcon != null)
           notifyIcon.Dispose();
          if (this.timer != null)
           timer.Dispose();
          if (this.components != null)
           components.Dispose();
        }
      }
     disposed = true;
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
      notifyIcon.Visible = false;
      
      watcher.Stop();

      if (this.optionsDialog != null)
       optionsDialog.Close();
      
      if (this.Exit != null)
        Exit(this, e);

      
    }

    private void mySQLServicesList_ServicesListChanged(object sender, ServicesListChangedArgs args)
    {
      if (trayAppSettings.enableAutoRefresh && trayAppSettings.autoRefreshType == RefreshTypeEnum.ByTimer && trayAppSettings.autoRefreshNotifyChanges)
      {
        notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
        notifyIcon.BalloonTipTitle = balloonTitleForServiceListChanges;
        notifyIcon.BalloonTipText = String.Format(balloonTextForServiceListChanges,
                                                       args.RemovedServicesList.Count,
                                                       args.AddedServicesList.Count);
        notifyIcon.ShowBalloonTip(3000);
      }
    }

    private void mySQLServicesList_ServiceStatusChanged(object sender, ServiceStatus args)
    {
      if (trayAppSettings.enableAutoRefresh && trayAppSettings.autoRefreshType == RefreshTypeEnum.ByTimer && trayAppSettings.autoRefreshNotifyChanges)
      {
        notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
        notifyIcon.BalloonTipTitle = balloonTitleForServiceStatusChanges;
        notifyIcon.BalloonTipText = String.Format(balloonTextForServiceStatusChanges,
                                                       args.ServiceName,
                                                       args.PreviousStatus.ToString(),
                                                       args.CurrentStatus.ToString());
        notifyIcon.ShowBalloonTip(3000);
      }
    }

    private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
      e.Cancel = false;
      if (trayAppSettings.enableAutoRefresh && trayAppSettings.autoRefreshType == RefreshTypeEnum.OnDemand)
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
       OnExit(EventArgs.Empty);
    }

    private void manageServicesDialogItem_Click(object sender, EventArgs e)
    {

      if (manageServicesDialog == null)
        manageServicesDialog = new ManageServicesDlg(trayAppSettings.servicesMonitor);

        DialogResult dg = manageServicesDialog.ShowDialog();
        if (dg == DialogResult.OK)
        {
          if (Properties.Settings.Default.ServicesMonitor != null)
            Properties.Settings.Default.ServicesMonitor.Clear();
          else
            Properties.Settings.Default.ServicesMonitor = new System.Collections.Specialized.StringCollection();
          Properties.Settings.Default.ServicesMonitor.AddRange(manageServicesDialog.services.ToArray());
          Properties.Settings.Default.Save();          
          trayAppSettings.servicesMonitor = manageServicesDialog.services;
          refreshServicesMenus();
        }    
    }

 
    private void launchInstallerItem_Click(object sender, EventArgs e)
    {
      //TODO
    }

    private void checkUpdatesItem_Click(object sender, EventArgs e)
    {
      //TODO
    }

    private void aboutMenu_Click(object sender, EventArgs e)
    {
      if (aboutForm == null)
        aboutForm = new About();
      aboutForm.ShowDialog();
    }


    private void optionsItem_Click(object sender, EventArgs e)
    {
      if (optionsDialog == null)
       optionsDialog = new OptionsForm(trayAppSettings);
      
      DialogResult dg =optionsDialog.ShowDialog();

      if (dg == DialogResult.OK)
          loadSettings();
    }

    /// <summary>
    /// When the exit menu item is clicked, make a call to terminate the ApplicationContext.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void exitItem_Click(object sender, EventArgs e)
    {
      OnExit(EventArgs.Empty);
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
                                                      ServicesCount.ToString()));
     notifyIcon.Text = (toolTipText.Length >= MAX_TOOLTIP_LENGHT ? toolTipText.Substring(0, MAX_TOOLTIP_LENGHT - 3) + "..." : toolTipText);
    }


    /// <summary>
    /// Loads all seetings into a local struct
    /// </summary>
    private void loadSettings()
    {
      trayAppSettings.enableAutoRefresh = Properties.Settings.Default.EnableAutoRefresh;
      trayAppSettings.autoRefreshType = (RefreshTypeEnum)Enum.Parse(typeof(RefreshTypeEnum), Properties.Settings.Default.AutoRefreshType, true);
      trayAppSettings.autoRefreshFrequency = Properties.Settings.Default.AutoRefreshFrequency;
      trayAppSettings.autoRefreshNotifyChanges = Properties.Settings.Default.AutoRefreshNotifyChanges;      
      trayAppSettings.runAtStartup = Properties.Settings.Default.RunAtStartup;
      trayAppSettings.autoCheckForUpdates = Properties.Settings.Default.AutoCheckForUpdates;
      trayAppSettings.checkForUpdatesFrequency = Properties.Settings.Default.CheckForUpdatesFrequency;
      trayAppSettings.autoAddNewServices = Properties.Settings.Default.AutoAddServicesToMonitor;

      if (Properties.Settings.Default.ServicesMonitor != null)
        trayAppSettings.servicesMonitor = Properties.Settings.Default.ServicesMonitor.Cast<string>().ToList();
      else
        trayAppSettings.servicesMonitor = new List<string>();

      if (actionsMenuItem != null &&actionsMenuItem.DropDownItems.Count > 0)
        actionsMenuItem.DropDownItems[0].Enabled = !trayAppSettings.enableAutoRefresh;

      timer = new System.Timers.Timer();

      if (trayAppSettings.enableAutoRefresh && trayAppSettings.autoRefreshType == RefreshTypeEnum.ByTimer)
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
      
      refreshingMenus = true;    
      mySQLServicesList.RefreshMySQLServices(ref trayAppSettings.servicesMonitor, trayAppSettings.autoAddNewServices);
     notifyIcon.ContextMenuStrip.Items.Clear();
      foreach (MySQLService mySqlServ in mySQLServicesList.InstalledMySQLServicesList)
      {
        foreach (ToolStripMenuItem item in mySqlServ.MenuGroup.ServiceMenuItems)
        {
         notifyIcon.ContextMenuStrip.Items.Add(item);
        }
       notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
      }
      foreach (ToolStripMenuItem item in staticMenuItems)
      {
       notifyIcon.ContextMenuStrip.Items.Add(item);
      }
     SetNotifyIconToolTip();
      
      refreshingMenus = false;
    }

    public void watcher_EventArrived(object sender, EventArrivedEventArgs args)
    {
      var e = args.NewEvent;
     
      //Debug.Print(" - Service :  has changed " + ((ManagementBaseObject)e["TargetInstance"])["DisplayName"] + " , State is " + ((ManagementBaseObject)e["TargetInstance"])["State"]);
      // if auto add is enabled then add the service to the monitored list and update the changes to the UI 

      if (trayAppSettings.autoAddNewServices)
      {
          var state = ((ManagementBaseObject)e["TargetInstance"])["State"].ToString().Trim();
          string newService = ((ManagementBaseObject)e["TargetInstance"])["DisplayName"].ToString().Trim();
          switch (state)
          {
            case "Running":
              UpdateMonitoredList(newService);
              break;            
          }
      }
    }

    private void UpdateMonitoredList(string serviceName)
    {
      if (actionsMenuItem.GetCurrentParent().InvokeRequired)
      {
        AutoAddNewServiceDelegate sd = new AutoAddNewServiceDelegate(UpdateMonitoredList);
        actionsMenuItem.GetCurrentParent().Invoke(sd, new object[] { serviceName });
      }
      else
      {
        if (!trayAppSettings.servicesMonitor.Contains(serviceName))
        {
          Properties.Settings.Default.ServicesMonitor.Add(serviceName);
          Properties.Settings.Default.Save();
          trayAppSettings.servicesMonitor = Properties.Settings.Default.ServicesMonitor.Cast<string>().ToList();

           notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
           notifyIcon.BalloonTipTitle = String.Format("Service {0} has been added to the monitor list", serviceName);
           notifyIcon.ShowBalloonTip(3000);

          refreshServicesMenus();
        }
      }
    }   


  }
}
