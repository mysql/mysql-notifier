//
// Copyright (c) 2012, Oracle and/or its affiliates. All rights reserved.
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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.ServiceProcess;
using System.Windows.Forms;
using MySql.Notifier.Properties;
using MySQL.Utility;

namespace MySql.Notifier
{
  internal class Notifier
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

    private bool supportedWorkbenchVersion
    {
      get
      {
        return new Version(MySqlWorkbench.ProductVersion) >= new Version(Settings.Default.SupportedWorkbenchVersion);
      }
    }

    private delegate void serviceWindowsEvent(string servicename, string path, string state);

    public Notifier()
    {
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

      // Create watcher for WB files
      if (MySqlWorkbench.IsInstalled && supportedWorkbenchVersion)
      {
        string file = String.Format(@"{0}\MySQL\Workbench\connections.xml", Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData));
        if (File.Exists(file))
          StartWatcherForFile(file, connectionsFile_Changed);

        file = String.Format(@"{0}\MySQL\Workbench\server_instances.xml", Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData));
        if (File.Exists(file))
          StartWatcherForFile(file, serversFile_Changed);
      }

      if (Settings.Default.FirstRun && Settings.Default.AutoCheckForUpdates && Settings.Default.CheckForUpdatesFrequency > 0)
      {
        var location = Utility.GetInstallLocation("MySQL Notifier");
        if (!String.IsNullOrEmpty(location))
        {
          Utility.CreateScheduledTask("MySQLNotifierTask", location + @"MySqlNotifier.exe", "--c", Settings.Default.CheckForUpdatesFrequency, false, Utility.GetOsVersion() == Utility.OSVersion.WindowsXp);
        }
      }

      UpdateListToRemoveDeletedServices();

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

      StartWatcherForFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Oracle\MySQL Notifier\settings.config", settingsFile_Changed);

      WatchForServiceChanges();
      WatchForServiceDeletion();
    }

    private void WatchForServiceChanges()
    {
      var managementScope = new ManagementScope(@"root\cimv2");
      managementScope.Connect();

      try
      {
        WqlEventQuery query = new WqlEventQuery("__InstanceModificationEvent", new TimeSpan(0, 0, 1), "TargetInstance isa \"Win32_Service\"");
        watcher = new ManagementEventWatcher(managementScope, query);
        watcher.EventArrived += new EventArrivedEventHandler(ServiceChangedHandler);
        watcher.Start();
      }
      catch (ManagementException ex)
      {
        MySQLNotifierTrace.GetSourceTrace().WriteWarning("Critical Error when adding listener for events. - " + ex.Message + " " + ex.InnerException, 1);
      }
    }

    public void ServiceChangedHandler(object sender, EventArrivedEventArgs args)
    {
      var e = args.NewEvent;
      ManagementBaseObject o = ((ManagementBaseObject)e["TargetInstance"]);
      if (o == null) return;

      string state = o["State"].ToString().Trim();
      string serviceName = o["Name"].ToString().Trim();
      string path = o["PathName"].ToString();
      string mode = o["StartMode"].ToString();

      if (state.Contains("Pending")) return;

      if (mode == "Disabled")
      {
        mySQLServicesList.RemoveService(serviceName);
        return;
      }

      Control c = notifyIcon.ContextMenuStrip;
      if (c.InvokeRequired)
      {
        serviceWindowsEvent se = new serviceWindowsEvent(GetWindowsEvent);
        se.Invoke(serviceName, path, state);
      }
      else
        GetWindowsEvent(serviceName, path, state);
    }

    private void WatchForServiceDeletion()
    {
      var managementScope = new ManagementScope(@"root\cimv2");
      managementScope.Connect();
      try
      {
        WqlEventQuery query = new WqlEventQuery("__InstanceDeletionEvent", new TimeSpan(0, 0, 1), "TargetInstance isa \"Win32_Service\"");
        watcher = new ManagementEventWatcher(managementScope, query);
        watcher.EventArrived += new EventArrivedEventHandler(ServiceDeletedHandler);
        watcher.Start();
      }
      catch (ManagementException ex)
      {
        MySQLNotifierTrace.GetSourceTrace().WriteWarning("Critical Error when adding listener for events. - " + ex.Message + " " + ex.InnerException, 1);
      }
    }

    private void ServiceDeletedHandler(object sender, EventArrivedEventArgs args)
    {
      var e = args.NewEvent;
      ManagementBaseObject o = ((ManagementBaseObject)e["TargetInstance"]);
      if (o == null) return;
      string serviceName = o["Name"].ToString().Trim();
      Control c = notifyIcon.ContextMenuStrip;
      if (c.InvokeRequired)
        c.Invoke(new MethodInvoker(() =>
        {
          RemoveServiceAndNotify(serviceName);
        }));
      else
        RemoveServiceAndNotify(serviceName);
    }

    /// <summary>
    /// Remove Service and Notify the user
    /// </summary>
    private void RemoveServiceAndNotify(string serviceName)
    {
      mySQLServicesList.RemoveService(serviceName);
      notifyIcon.Icon = Icon.FromHandle(GetIconForNotifier().GetHicon());
      if (!Settings.Default.NotifyOfStatusChange) return;
      SetNotifyIconToolTip();
      ShowTooltip(false, Resources.BalloonTitleTextServiceList, String.Format(Resources.ServiceRemoved, serviceName), 1500);
    }

    /// <summary>
    /// Generic routine to help with showing tooltips
    /// </summary>
    private void ShowTooltip(bool error, string title, string text, int delay)
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

    private int LoadUpdateCheck()
    {
      for (int i = 0; i < 3; i++)
      {
        try
        {
          Settings.Default.Reload();
          return Settings.Default.UpdateCheck;
        }
        catch (IOException ex)
        {
          MySQLNotifierTrace.GetSourceTrace().WriteWarning(Resources.SettingsFileFailedToLoad + " - " + (ex.Message + " " + ex.InnerException), 1);
          System.Threading.Thread.Sleep(1000);
        }
      }
      using (var errorDialog = new MessageDialog(Resources.HighSeverityError, Resources.SettingsFileFailedToLoad, true))
      {
        errorDialog.ShowDialog();
      }
      return -1;
    }

    private void settingsFile_Changed(object sender, FileSystemEventArgs e)
    {
      int settingsUpdateCheck = LoadUpdateCheck();

      // if we have already notified our user then noting more to do
      if (settingsUpdateCheck == 0 || (settingsUpdateCheck & (int)SoftwareUpdateStaus.Notified) != 0) return;

      // if we are supposed to check forupdates but the installer is too old then
      // notify the user and exit
      if (String.IsNullOrEmpty(MySqlInstaller.GetInstallerPath()) || !MySqlInstaller.GetInstallerVersion().StartsWith("1.1"))
      {
        ShowTooltip(false, Resources.SoftwareUpdate, Resources.ScheduledCheckRequiresInstaller11, 1500);
        settingsUpdateCheck = 0;
      }

      bool hasUpdates = true;

      // let them know we are checking for updates
      if ((settingsUpdateCheck & (int)SoftwareUpdateStaus.Checking) != 0)
      {
        ShowTooltip(false, Resources.SoftwareUpdate, Resources.CheckingForUpdates, 1500);
        hasUpdates = MySqlInstaller.HasUpdates(10 * 1000);
        Settings.Default.UpdateCheck = hasUpdates ? (int)SoftwareUpdateStaus.HasUpdates : 0;
        settingsUpdateCheck = Settings.Default.UpdateCheck;
        Settings.Default.Save();
      }

      if ((settingsUpdateCheck & (int)SoftwareUpdateStaus.HasUpdates) != 0)
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

    private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
      {
        MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
        mi.Invoke(notifyIcon, null);
      }
    }

    private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
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
      checkForUpdates.Image = Resources.CheckForUpdatesIcon;

      if (MySqlWorkbench.IsInstalled && supportedWorkbenchVersion)
      {
        launchWorkbenchUtilitiesMenuItem = new ToolStripMenuItem("MySQL Utilities Shell");
        launchWorkbenchUtilitiesMenuItem.Click += new EventHandler(LaunchWorkbenchUtilities_Click);
        launchWorkbenchUtilitiesMenuItem.Image = Resources.LaunchUtilities;
      }

      ToolStripMenuItem optionsMenu = new ToolStripMenuItem("Options...");
      optionsMenu.Click += new EventHandler(optionsItem_Click);

      ToolStripMenuItem aboutMenu = new ToolStripMenuItem("About...");
      aboutMenu.Click += new EventHandler(aboutMenu_Click);

      ToolStripMenuItem exitMenu = new ToolStripMenuItem("Close MySQL Notifier");
      exitMenu.Click += new EventHandler(exitItem_Click);

      menu.Items.Add(manageServices);
      menu.Items.Add(launchInstallerMenuItem);
      menu.Items.Add(checkForUpdates);

      if (launchWorkbenchUtilitiesMenuItem != null)
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
      if (installAvailablelUpdatesMenuItem != null) installAvailablelUpdatesMenuItem.Visible = hasUpdates;
      if (ignoreAvailableUpdateMenuItem != null) ignoreAvailableUpdateMenuItem.Visible = hasUpdates;
      if (launchInstallerMenuItem != null) launchInstallerMenuItem.Enabled = MySqlInstaller.IsInstalled;
      if (launchWorkbenchUtilitiesMenuItem != null) launchWorkbenchUtilitiesMenuItem.Visible = MySqlWorkbench.IsMySQLUtilitiesInstalled();
    }

    private void ServiceListChanged(MySQLService service, ServiceListChangeType changeType)
    {
      if ((mySQLServicesList.Services.Count == 0 && changeType == ServiceListChangeType.Remove) ||
         (previousTotalServicesNumber == 0 && changeType != ServiceListChangeType.Remove))
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
          notifyIcon.BalloonTipText = String.Format(Resources.BalloonTextServiceList, service.DisplayName);
          notifyIcon.ShowBalloonTip(1500);
        }
      }
    }

    private void service_StatusChangeError(object sender, Exception ex)
    {
      MySQLService service = (MySQLService)sender;
      notifyIcon.BalloonTipIcon = ToolTipIcon.Error;
      notifyIcon.BalloonTipTitle = Resources.BalloonTitleFailedStatusChange;
      notifyIcon.BalloonTipText = String.Format(Resources.BalloonTextFailedStatusChange, service.DisplayName, Environment.NewLine + ex.Message + Environment.NewLine + Resources.AskRestartApplication);
      notifyIcon.ShowBalloonTip(1500);
      MySQLNotifierTrace.GetSourceTrace().WriteError("Critical Error when trying to update the service status - " + (ex.Message + " " + ex.InnerException), 1);
    }

    private void service_StatusChanged(object sender, ServiceStatus args)
    {
      MySQLService service = (MySQLService)sender;
      service.MenuGroup.Update();
      notifyIcon.ContextMenuStrip.Refresh();
    }

    private void mySQLServicesList_ServiceListChanged(object sender, MySQLService service, ServiceListChangeType changeType)
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

      MySQLService service = mySQLServicesList.GetServiceByDisplayName(args.ServiceDisplayName);

      if (!service.NotifyOnStatusChange) return;

      if (service.UpdateTrayIconOnStatusChange) notifyIcon.Icon = Icon.FromHandle(GetIconForNotifier().GetHicon());

      notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
      notifyIcon.BalloonTipTitle = Resources.BalloonTitleTextServiceStatus;
      notifyIcon.BalloonTipText = String.Format(Resources.BalloonTextServiceStatus,
                                                      args.ServiceDisplayName,
                                                      args.PreviousStatus.ToString(),
                                                      args.CurrentStatus.ToString());
      notifyIcon.ShowBalloonTip(1500);
    }

    private void manageServicesDialogItem_Click(object sender, EventArgs e)
    {
      ManageItemsDialog dialog = new ManageItemsDialog(mySQLServicesList);
      dialog.ShowDialog();

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
      if (String.IsNullOrEmpty(MySqlInstaller.GetInstallerPath()) || !MySqlInstaller.GetInstallerVersion().StartsWith("1.1"))
      {
        using (var errorDialog = new MessageDialog(Resources.MissingMySQLInstaller, String.Format(Resources.Installer11RequiredForCheckForUpdates, Environment.NewLine), false))
        {
          errorDialog.ShowDialog();
          return;
        }
      }

      string path = @MySqlInstaller.GetInstallerPath();
      Process proc = new Process();
      ProcessStartInfo startInfo = new ProcessStartInfo();
      startInfo.FileName = @String.Format(@"{0}\MySQLInstaller.exe", @path);
      startInfo.Arguments = "checkforupdates";
      Process.Start(startInfo);
    }

    private void aboutMenu_Click(object sender, EventArgs e)
    {
      AboutDialog dialog = new AboutDialog();
      dialog.ShowDialog();
    }

    private void optionsItem_Click(object sender, EventArgs e)
    {
      var usecolorfulIcons = Properties.Settings.Default.UseColorfulStatusIcons;
      OptionsDialog dialog = new OptionsDialog();
      dialog.ShowDialog();

      // if there was a change in the setting for the icons then refresh Icon
      if (usecolorfulIcons != Properties.Settings.Default.UseColorfulStatusIcons)
        notifyIcon.Icon = Icon.FromHandle(GetIconForNotifier().GetHicon());
    }

    private void InstallAvailablelUpdates_Click(object sender, EventArgs e)
    {
      launchInstallerItem_Click(null, EventArgs.Empty);
      Properties.Settings.Default.UpdateCheck = 0;
      Properties.Settings.Default.Save();
      notifyIcon.Icon = Icon.FromHandle(GetIconForNotifier().GetHicon());
    }

    private void IgnoreAvailableUpdateItem_Click(object sender, EventArgs e)
    {
      DialogResult result = MessageBox.Show("This action will completely ignore the available software updates. Would you like to continue?",
        "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
      if (result == DialogResult.Yes)
      {
        Properties.Settings.Default.UpdateCheck = 0;
        Properties.Settings.Default.Save();
        notifyIcon.Icon = Icon.FromHandle(GetIconForNotifier().GetHicon());
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
      var version = Assembly.GetExecutingAssembly().GetName().Version.ToString().Split('.');

      string toolTipText = string.Format("{0} ({1})\n{2}.",
                                         Properties.Resources.AppName,
                                         String.Format("{0}.{1}.{2}", version[0], version[1], version[2]),
                                         String.Format(Properties.Resources.ToolTipText, mySQLServicesList.Services.Count));
      notifyIcon.Text = (toolTipText.Length >= MAX_TOOLTIP_LENGHT ? toolTipText.Substring(0, MAX_TOOLTIP_LENGHT - 3) + "..." : toolTipText);
    }

    private void GetWindowsEvent(string serviceName, string path, string state)
    {
      Control c = notifyIcon.ContextMenuStrip;

      if (c.InvokeRequired)
      {
        c.Invoke(new MethodInvoker(() => { GetWindowsEvent(serviceName, path, state); }));
      }
      else
      {
        // TODO: FIX THE WAY WMI Notifies for all services... :/
        var service = mySQLServicesList.GetServiceByName(serviceName);
        ServiceControllerStatus copyPreviousStatus = ServiceControllerStatus.Stopped;
        if (service != null)
        {
          // TODO: FIX THE WAY WMI Notifies for all services... :/
          service.StatusChanged -= new MySQLService.StatusChangedHandler(service_StatusChanged);
          service.StatusChanged += new MySQLService.StatusChangedHandler(service_StatusChanged);
          copyPreviousStatus = service.Status;
          service.UpdateMenu(state);

          SetNotifyIconToolTip();

          if (service.NotifyOnStatusChange && !copyPreviousStatus.Equals(service.Status))
          {
            var serviceStatusInfo = new ServiceStatus(service.DisplayName, copyPreviousStatus, service.Status);
            mySQLServicesList_ServiceStatusChanged(this, serviceStatusInfo);
          }
          service.MenuGroup.RefreshRoot(notifyIcon.ContextMenuStrip, copyPreviousStatus);
        }

        mySQLServicesList.SetServiceStatus(serviceName, path, state);

        if (service == null || service.UpdateTrayIconOnStatusChange)
          notifyIcon.Icon = Icon.FromHandle(GetIconForNotifier().GetHicon());
      }
    }

    private void ReBuildMenu()
    {
      notifyIcon.ContextMenuStrip = new ContextMenuStrip();
      notifyIcon.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);
      AddStaticMenuItems();
      UpdateStaticMenuItems();
    }

    private void UpdateListToRemoveDeletedServices()
    {
      if (mySQLServicesList.Services == null) return;

      if (mySQLServicesList.Services.Count(t => t.FoundInSystem) < mySQLServicesList.Services.Count)
      {
        mySQLServicesList.Services = mySQLServicesList.Services.Where(t => t.FoundInSystem).ToList();
        Settings.Default.Save();
      }
    }

    private Bitmap GetIconForNotifier()
    {
      bool hasUpdates = (Settings.Default.UpdateCheck & (int)SoftwareUpdateStaus.HasUpdates) != 0;
      bool useColorfulIcon = Settings.Default.UseColorfulStatusIcons;

      if (Settings.Default.ServiceList != null)
      {
        var updateTrayIconServices = Settings.Default.ServiceList.Where(t => t.UpdateTrayIconOnStatusChange);

        if (updateTrayIconServices != null)
        {
          if (updateTrayIconServices.Where(t => t.Status == ServiceControllerStatus.Stopped).Count() > 0)
            if (!useColorfulIcon)
              return hasUpdates ? Properties.Resources.NotifierIconStoppedAlert : Properties.Resources.NotifierIconStopped;
            else
              return hasUpdates ? Properties.Resources.NotifierIconStoppedAlertStrong : Properties.Resources.NotifierIconStoppedStrong;

          if (updateTrayIconServices.Where(t => t.Status == ServiceControllerStatus.StartPending).Count() > 0)
            if (!useColorfulIcon)
              return hasUpdates ? Properties.Resources.NotifierIconStartingAlert : Properties.Resources.NotifierIconStarting;
            else
              return hasUpdates ? Properties.Resources.NotifierIconStartingAlertStrong : Properties.Resources.NotifierIconStartingStrong;

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