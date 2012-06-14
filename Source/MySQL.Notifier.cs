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
using System.Diagnostics;
using System.ComponentModel;
using System.Linq;
using System.Management;
using MySql.Notifier.Properties;
using MySQL.Utility;
using System.IO;
using System.Configuration;



namespace MySql.Notifier
{
  class Notifier
  {
    private System.ComponentModel.IContainer components;
    private NotifyIcon notifyIcon;
    private MySQLServicesList mySQLServicesList { get; set; }
    
    private ManagementEventWatcher watcher;

    private ToolStripMenuItem launchWorkbenchUtilitiesMenuItem;
    private ToolStripMenuItem launchInstallerMenuItem;
    private ToolStripMenuItem installAvailablelUpdatesMenuItem;
    private ToolStripMenuItem ignoreAvailableUpdateMenuItem;
    private ToolStripSeparator hasUpdatesSeparator;

    private int previousTotalServicesNumber;

    private delegate void serviceWindowsEvent(string servicename, string path, string state);    

    public Notifier()
    {           
      //load splash screen
       var splashScreen = new AboutDialog();
       splashScreen.Show();
      
      components = new System.ComponentModel.Container();
      notifyIcon = new NotifyIcon(components)
                    {
                      ContextMenuStrip = new ContextMenuStrip(),
                      Icon = Icon.FromHandle(GetIconForNotifier().GetHicon()),
                      Visible = true
                    };

      notifyIcon.MouseClick += notifyIcon_MouseClick;
      notifyIcon.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);
      
      notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
      notifyIcon.BalloonTipTitle = Properties.Resources.BalloonTitleTextServiceStatus;

      // Setup our service list
      mySQLServicesList = new MySQLServicesList();
      mySQLServicesList.ServiceStatusChanged += mySQLServicesList_ServiceStatusChanged;
      mySQLServicesList.ServiceListChanged += new MySQLServicesList.ServiceListChangedHandler(mySQLServicesList_ServiceListChanged);


      // Create watcher to synchronize menus
      if (MySqlWorkbench.IsInstalled)
      {
        string file = String.Format(@"{0}\MySQL\Workbench\connections.xml", Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData));
        StartWatcherForFile(file, connectionsFile_Changed);

        file = String.Format(@"{0}\MySQL\Workbench\server_instances.xml", Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData));
        StartWatcherForFile(file, serversFile_Changed);
      }

      if (Settings.Default.FirstRun && Settings.Default.AutoCheckForUpdates && Settings.Default.CheckForUpdatesFrequency > 0)
      {
        if (!String.IsNullOrEmpty(Utility.GetInstallLocation("MySQL Notifier")))
        {
          Utility.CreateScheduledTask("MySQLNotifierTask", @"""" + Utility.GetInstallLocation("MySQL Notifier") + @"MySql.Notifier.exe --c""",
            Settings.Default.CheckForUpdatesFrequency, false);
        }
               
      }
      
      mySQLServicesList.LoadFromSettings();
      
      previousTotalServicesNumber = mySQLServicesList.Services.Count;

      if (mySQLServicesList.Services.Count == 0) 
      {
        AddStaticMenuItems();
        UpdateStaticMenuItems();
      }
      else
      {
        notifyIcon.Icon = Icon.FromHandle(GetIconForNotifier().GetHicon());
      }
     
      SetNotifyIconToolTip();

      
      Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
      StartWatcherForFile(config.FilePath, settingsFile_Changed);

      
      var managementScope = new ManagementScope(@"root\cimv2");
      managementScope.Connect();

      // WqlEventQuery query = new WqlEventQuery("__InstanceModificationEvent", new TimeSpan(0, 0, 1), "TargetInstance isa \"Win32_Service\" AND ( TargetInstance.Name LIKE \"%MYSQL%\" OR TargetInstance.PathName LIKE \"%MYSQL%\" ) ");
      WqlEventQuery query = new WqlEventQuery("__InstanceModificationEvent", new TimeSpan(0, 0, 1), "TargetInstance isa \"Win32_Service\"");
      watcher = new ManagementEventWatcher(managementScope, query);
      watcher.EventArrived += new EventArrivedEventHandler(watcher_EventArrived);
      watcher.Start();

      splashScreen.Close();
        
    }

    /// <summary>
    /// Generic routine to help with showing tooltips
    /// </summary>
    void ShowTooltip(bool error, string title, string text, int delay)
    {
      notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
      notifyIcon.BalloonTipTitle = title;
      notifyIcon.BalloonTipText = text; 
      notifyIcon.ShowBalloonTip(delay);
    }

    /// <summary>
    /// Creates a FileSystemWatcher for the specified file
    /// </summary>
    /// <param name="filePath">File to add the file system watcher</param>
    /// <param name="method">Action method</param>
    
    private void StartWatcherForFile(string filePath, FileSystemEventHandler method)
    {      
      FileSystemWatcher watcher = new FileSystemWatcher();
      watcher.Path = Path.GetDirectoryName(filePath);
      watcher.Filter = Path.GetFileName(filePath);
      watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Attributes;
      watcher.Changed += new FileSystemEventHandler(method);
      watcher.EnableRaisingEvents = true;
    }


    void settingsFile_Changed(object sender, FileSystemEventArgs e)
    {
      Settings.Default.Reload();

      // if we have already notified our user then noting more to do
      if((Settings.Default.UpdateCheck & (int)SoftwareUpdateStaus.Notified) != 0) return;

      // let them know we are checking for updates
      if ((Settings.Default.UpdateCheck & (int)SoftwareUpdateStaus.Checking) != 0)
        ShowTooltip(false, Resources.SoftwareUpdate, Resources.CheckingForUpdates, 1500);

      else if ((Settings.Default.UpdateCheck & (int)SoftwareUpdateStaus.HasUpdates) != 0)
        ShowTooltip(false, Resources.SoftwareUpdate, Resources.HasUpdatesLaunchInstaller, 1500);

      // set that we have notified our user
      Settings.Default.UpdateCheck |= (int)SoftwareUpdateStaus.Notified;
      Settings.Default.Save();
      notifyIcon.Icon = Icon.FromHandle(GetIconForNotifier().GetHicon());
    }

    /// <summary>
    /// Method to handle the change events in the 
    /// connections file of workbench
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void connectionsFile_Changed(object sender, FileSystemEventArgs e)
    {
      MySqlWorkbench.Servers = new MySqlWorkbenchServerCollection();
      MySqlWorkbench.Connections = new MySqlWorkbenchConnectionCollection();
      MySqlWorkbench.LoadData();

      foreach (var item in mySQLServicesList.Services)
      {
        item.MenuGroup.RefreshMenu(notifyIcon.ContextMenuStrip);
      }     
    }


    /// <summary>
    /// Method to handle the change events in the 
    /// server instances file of workbench
    /// no changes in UI
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void serversFile_Changed(object sender, FileSystemEventArgs e)
    {
      MySqlWorkbench.Servers = new MySqlWorkbenchServerCollection();
      MySqlWorkbench.Connections = new MySqlWorkbenchConnectionCollection();
      MySqlWorkbench.LoadData();
    }


    void notifyIcon_MouseClick(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
        {
            MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
            mi.Invoke(notifyIcon, null);
        }
    }    

    void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
      foreach (MySQLService service in mySQLServicesList.Services)
        service.MenuGroup.Update();
      UpdateStaticMenuItems();
    }

    /// <summary>
    /// Adds the static menu items such as Options, Exit, About..
    /// </summary>
    private void AddStaticMenuItems()
    {      
      ContextMenuStrip menu = new ContextMenuStrip();      

      ToolStripMenuItem manageServices = new ToolStripMenuItem("Manage Services...");    
      manageServices.Click += new EventHandler(manageServicesDialogItem_Click);
      manageServices.Image = Resources.ManageServicesIcon;

      launchInstallerMenuItem = new ToolStripMenuItem("Launch Installer");
      launchInstallerMenuItem.Click += new EventHandler(launchInstallerItem_Click);
      launchInstallerMenuItem.Image = Resources.StartInstallerIcon;

      ToolStripMenuItem checkForUpdates = new ToolStripMenuItem("Check for updates");
      checkForUpdates.Click += new EventHandler(checkUpdatesItem_Click);
      checkForUpdates.Enabled = !String.IsNullOrEmpty(MySqlInstaller.GetInstallerPath()) && MySqlInstaller.GetInstallerVersion().Contains("1.1");
      checkForUpdates.Image = Resources.CheckForUpdatesIcon;

      launchWorkbenchUtilitiesMenuItem = new ToolStripMenuItem("MySQL Utilities Shell");
      launchWorkbenchUtilitiesMenuItem.Click += new EventHandler(LaunchWorkbenchUtilities_Click);
      launchWorkbenchUtilitiesMenuItem.Image = Resources.LaunchUtilities;

      ToolStripMenuItem optionsMenu = new ToolStripMenuItem("Options...");
      optionsMenu.Click += new EventHandler(optionsItem_Click);

      ToolStripMenuItem aboutMenu = new ToolStripMenuItem("About...");
      aboutMenu.Click += new EventHandler(aboutMenu_Click);

      ToolStripMenuItem exitMenu = new ToolStripMenuItem("Close MySQL Notifier");
      exitMenu.Click += new EventHandler(exitItem_Click);

      menu.Items.Add(manageServices);
      menu.Items.Add(launchInstallerMenuItem);
      menu.Items.Add(checkForUpdates);
      menu.Items.Add(launchWorkbenchUtilitiesMenuItem);
      menu.Items.Add(new ToolStripSeparator());
      menu.Items.Add(optionsMenu);
      menu.Items.Add(aboutMenu);
      menu.Items.Add(exitMenu);
     
      if (mySQLServicesList.Services.Count > 0)
      {
        ToolStripMenuItem actionsMenu = new ToolStripMenuItem("Actions", null);
        actionsMenu.DropDown = menu;
        notifyIcon.ContextMenuStrip.Items.Add(actionsMenu);
      }
      else
      {
        notifyIcon.ContextMenuStrip = menu;
      }

      // now we add the menu items we will show when we have updates available
      hasUpdatesSeparator = new ToolStripSeparator();

      installAvailablelUpdatesMenuItem = new ToolStripMenuItem("Install available updates...", Resources.InstallAvailableUpdatesIcon);
      installAvailablelUpdatesMenuItem.Click += new EventHandler(InstallAvailablelUpdates_Click);

      ignoreAvailableUpdateMenuItem = new ToolStripMenuItem("Ignore this update");
      ignoreAvailableUpdateMenuItem.Click += new EventHandler(IgnoreAvailableUpdateItem_Click);

      notifyIcon.ContextMenuStrip.Items.Add(hasUpdatesSeparator);
      notifyIcon.ContextMenuStrip.Items.Add(installAvailablelUpdatesMenuItem);
      notifyIcon.ContextMenuStrip.Items.Add(ignoreAvailableUpdateMenuItem);
    }

    private void UpdateStaticMenuItems()
    {      
      bool hasUpdates = (Settings.Default.UpdateCheck & (int)SoftwareUpdateStaus.HasUpdates) != 0;
      if (hasUpdatesSeparator != null) hasUpdatesSeparator.Visible = hasUpdates;
      if (installAvailablelUpdatesMenuItem != null)  installAvailablelUpdatesMenuItem.Visible = hasUpdates;
      if (ignoreAvailableUpdateMenuItem != null) ignoreAvailableUpdateMenuItem.Visible = hasUpdates;
      if (launchInstallerMenuItem != null) launchInstallerMenuItem.Enabled = MySqlInstaller.IsInstalled;
      if (launchWorkbenchUtilitiesMenuItem != null) launchWorkbenchUtilitiesMenuItem.Visible = MySqlWorkbench.IsMySQLUtilitiesInstalled();
    }

    private void ServiceListChanged(MySQLService service, ServiceListChangeType changeType)
    {
           
       if ((mySQLServicesList.Services.Count == 0 && changeType == ServiceListChangeType.Remove) ||
          (previousTotalServicesNumber == 0 &&  changeType != ServiceListChangeType.Remove))
      {      
          ReBuildMenu();
          previousTotalServicesNumber = mySQLServicesList.Services.Count;
      }

      if (changeType == ServiceListChangeType.Remove)
      {
        service.MenuGroup.RemoveFromContextMenu(notifyIcon.ContextMenuStrip);                 
      }
      else
      {                
          service.MenuGroup.AddToContextMenu(notifyIcon.ContextMenuStrip);
          service.StatusChangeError += new MySQLService.StatusChangeErrorHandler(service_StatusChangeError);
          if (changeType == ServiceListChangeType.AutoAdd && Settings.Default.NotifyOfAutoServiceAddition)
          {
            notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon.BalloonTipTitle = Resources.BalloonTitleTextServiceList;
            notifyIcon.BalloonTipText = String.Format(Resources.BalloonTextServiceList, service.ServiceName);
            notifyIcon.ShowBalloonTip(1500);            
          }
       }       
    }

    void service_StatusChangeError(object sender, Exception ex)
    {
      MySQLService service = (MySQLService)sender;
      notifyIcon.BalloonTipIcon = ToolTipIcon.Error;
      notifyIcon.BalloonTipTitle = Resources.BalloonTitleFailedStatusChange;
      notifyIcon.BalloonTipText = String.Format(Resources.BalloonTextFailedStatusChange, service.ServiceName, ex.Message);
      notifyIcon.ShowBalloonTip(1500);
    }
   
    void mySQLServicesList_ServiceListChanged(object sender, MySQLService service, ServiceListChangeType changeType)
    {
      ServiceListChanged(service, changeType);
    }

    /// <summary>
    /// Notifies that the Notifier wants to quit
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

      if (this.Exit != null)
        Exit(this, e);
    }

    private void mySQLServicesList_ServiceStatusChanged(object sender, ServiceStatus args)
    {
      if (!Settings.Default.NotifyOfStatusChange) return;

      MySQLService service = mySQLServicesList.GetServiceByName(args.ServiceName);      

      if (!service.NotifyOnStatusChange) return;

      if (service.UpdateTrayIconOnStatusChange) notifyIcon.Icon = Icon.FromHandle(GetIconForNotifier().GetHicon());

      notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
      notifyIcon.BalloonTipTitle = Resources.BalloonTitleTextServiceStatus;
      notifyIcon.BalloonTipText = String.Format(Resources.BalloonTextServiceStatus,
                                                      args.ServiceName,
                                                      args.PreviousStatus.ToString(),
                                                      args.CurrentStatus.ToString());
      notifyIcon.ShowBalloonTip(1500);
    }

   
    private void manageServicesDialogItem_Click(object sender, EventArgs e)
    {
      ManageServicesDlg dlg = new ManageServicesDlg(mySQLServicesList);
      dlg.ShowDialog();    
      //update icon 
      notifyIcon.Icon = Icon.FromHandle(GetIconForNotifier().GetHicon());
    }


    private void launchInstallerItem_Click(object sender, EventArgs e)
    {
      string path = MySqlInstaller.GetInstallerPath();
      if (String.IsNullOrEmpty(path)) return;  // this should not happen since our menu item is enabled

      Process proc = new Process();
      ProcessStartInfo startInfo = new ProcessStartInfo();
      startInfo.FileName = String.Format(@"{0}\MySQLInstaller.exe", path);
      Process.Start(startInfo);            
    }

    private void checkUpdatesItem_Click(object sender, EventArgs e)
    {
      if (!String.IsNullOrEmpty(MySqlInstaller.GetInstallerPath()))
      {
        string path = @MySqlInstaller.GetInstallerPath();
        Process proc = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = @String.Format(@"{0}\MySQLInstaller.exe", @path);
        startInfo.Arguments = "-checkforupdates";
        Process.Start(startInfo);
      }      
    }

    private void aboutMenu_Click(object sender, EventArgs e)
    {
      AboutDialog dlg = new AboutDialog();
      dlg.ShowDialog();
    }


    private void optionsItem_Click(object sender, EventArgs e)
    {
      OptionsDialog dlg = new OptionsDialog();
      dlg.ShowDialog();      
    }


    private void InstallAvailablelUpdates_Click(object sender, EventArgs e)
    {
      launchInstallerItem_Click(null, EventArgs.Empty);
    }

    private void IgnoreAvailableUpdateItem_Click(object sender, EventArgs e)
    {
      DialogResult result = MessageBox.Show("This action will completely ignore the available software updates. Would you like to continue?", 
        "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
      if (result == DialogResult.Yes)
      {
        Properties.Settings.Default.UpdateCheck = 0;
        Properties.Settings.Default.Save();        
      }
    }

    private void LaunchWorkbenchUtilities_Click(object sender, EventArgs e)
    {
      MySqlWorkbench.LaunchUtilitiesShell();    
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

    /// <summary>
    /// Sets the text displayed in the notify icon's tooltip
    /// </summary>
    public void SetNotifyIconToolTip()
    {
      int MAX_TOOLTIP_LENGHT = 63; // framework constraint for notify icons

      string toolTipText = string.Format("{0} ({1})\n{2}.",
                                         Properties.Resources.AppName,
                                         Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                                         String.Format(Properties.Resources.ToolTipText, mySQLServicesList.Services.Count));
      notifyIcon.Text = (toolTipText.Length >= MAX_TOOLTIP_LENGHT ? toolTipText.Substring(0, MAX_TOOLTIP_LENGHT - 3) + "..." : toolTipText);
    }

    public void watcher_EventArrived(object sender, EventArrivedEventArgs args)
    {
      var e = args.NewEvent;
      ManagementBaseObject o = ((ManagementBaseObject)e["TargetInstance"]);
      if (o == null) return;

      string state = o["State"].ToString().Trim();
      string serviceName = o["DisplayName"].ToString().Trim();
      string path = o["PathName"].ToString();

      if (state.Contains("Pending")) return;

      Control c = notifyIcon.ContextMenuStrip;
      if (c.InvokeRequired)
      {
        serviceWindowsEvent se = new serviceWindowsEvent(GetWindowsEvent);
        se.Invoke(serviceName, path, state);
      }     
      else      
        GetWindowsEvent(serviceName, path, state);

    }

    private void GetWindowsEvent(string serviceName, string path, string state)
    {
      var service = mySQLServicesList.GetServiceByName(serviceName);
      if (service != null)
      {
        ServiceControllerStatus copyPreviousStatus = service.Status;

        mySQLServicesList.SetServiceStatus(serviceName, path, state);
        ServiceControllerStatus newStatus = service.Status;

        SetNotifyIconToolTip();
      
        if (service.UpdateTrayIconOnStatusChange)
          notifyIcon.Icon = Icon.FromHandle(GetIconForNotifier().GetHicon());

        if (service.NotifyOnStatusChange && !copyPreviousStatus.Equals(newStatus))
        {
          var serviceStatusInfo = new ServiceStatus(service.ServiceName, copyPreviousStatus, newStatus);
          mySQLServicesList_ServiceStatusChanged(this, serviceStatusInfo);
        }
      }    
    }

    private void ReBuildMenu()
    {
      notifyIcon.ContextMenuStrip = new ContextMenuStrip();
      notifyIcon.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);
      AddStaticMenuItems();
      UpdateStaticMenuItems();
    }


    private Bitmap GetIconForNotifier()
    {
      bool hasUpdates = (Settings.Default.UpdateCheck & (int)SoftwareUpdateStaus.HasUpdates) != 0;

      if (Settings.Default.ServiceList != null)
      {
        var updateTrayIconServices = Settings.Default.ServiceList.Where(t => t.UpdateTrayIconOnStatusChange);

        if (updateTrayIconServices != null)
        {
          if (updateTrayIconServices.Where(t => t.Status == ServiceControllerStatus.Stopped).Count() > 0)
            return hasUpdates ? Properties.Resources.NotifierIconStoppedAlert : Properties.Resources.NotifierIconStopped;

          if (updateTrayIconServices.Where(t => t.Status == ServiceControllerStatus.StartPending).Count() > 0)
            return hasUpdates ? Properties.Resources.NotifierIconStartingAlert : Properties.Resources.NotifierIconStarting;

          if (updateTrayIconServices.Where(t => t.Status == ServiceControllerStatus.Running).Count() > 0)
            return hasUpdates ? Properties.Resources.NotifierIconRunningAlert : Properties.Resources.NotifierIconRunning;
        }
      }

      return hasUpdates ? Properties.Resources.NotifierIconAlert : Properties.Resources.NotifierIcon;
    }
  }    

  public enum SoftwareUpdateStaus : int
  {
    Checking = 1,
    HasUpdates = 2,
    Notified = 4
  }
}
