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

using System;
using System.Collections.Generic;
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
using MySQL.Utility.Forms;

namespace MySql.Notifier
{
  internal class Notifier
  {
    private System.ComponentModel.IContainer components;
    private NotifyIcon notifyIcon;

    private MySQLServicesList mySQLServicesList { get; set; }
    private MySQLInstancesList mySQLInstancesList { get; set; }
    private MachinesList machinesList { get; set; }

    private List<ManagementEventWatcher> watchers = new List<ManagementEventWatcher>();

    private ToolStripMenuItem launchWorkbenchUtilitiesMenuItem;
    private ToolStripMenuItem launchInstallerMenuItem;
    private ToolStripMenuItem installAvailablelUpdatesMenuItem;
    private ToolStripMenuItem ignoreAvailableUpdateMenuItem;
    private ToolStripSeparator hasUpdatesSeparator;

    private int previousServicesAndInstancesQuantity;

    private delegate void serviceWindowsEvent(Machine machine, string servicename, string path, string state);

    public Notifier()
    {
      //// Static initializations.
      CustomizeInfoDialog();
      InitializeMySQLWorkbenchStaticSettings();

      components = new System.ComponentModel.Container();
      notifyIcon = new NotifyIcon(components);
      notifyIcon.Visible = true;
      RefreshNotifierIcon();
      notifyIcon.MouseClick += notifyIcon_MouseClick;
      notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
      notifyIcon.BalloonTipTitle = Properties.Resources.BalloonTitleTextServiceStatus;

      //// Setup instances list
      mySQLInstancesList = new MySQLInstancesList();
      mySQLInstancesList.InstanceStatusChanged += MySQLInstanceStatusChanged;
      mySQLInstancesList.InstancesListChanged += MySQLInstancesListChanged;
      mySQLInstancesList.InstanceConnectionStatusTestErrorThrown += MySQLInstanceConnectionStatusTestErrorThrown;

      machinesList = new MachinesList();
      machinesList.LoadFromSettings();
      machinesList.MachineListChanged += new MachinesList.MachineListChangedHandler(machineList_machineChanged);

      MigrateOldServices();

      RebuildMenuIfNeeded(false);
      previousServicesAndInstancesQuantity = CountServices() + mySQLInstancesList.Count;
      SetNotifyIconToolTip();

      //// Create watcher for WB files
      string applicationDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
      if (MySqlWorkbench.AllowsExternalConnectionsManagement)
      {
        string file = String.Format(@"{0}\MySQL\Workbench\connections.xml", applicationDataFolderPath);
        if (File.Exists(file))
        {
          StartWatcherForFile(file, connectionsFile_Changed);
        }

        file = String.Format(@"{0}\MySQL\Workbench\server_instances.xml", applicationDataFolderPath);
        if (File.Exists(file))
        {
          StartWatcherForFile(file, serversFile_Changed);
        }
      }

      if (Settings.Default.FirstRun && Settings.Default.AutoCheckForUpdates && Settings.Default.CheckForUpdatesFrequency > 0)
      {
        var location = Utility.GetInstallLocation("MySQL Notifier");
        if (!String.IsNullOrEmpty(location))
        {
          Utility.CreateScheduledTask("MySQLNotifierTask", location + @"MySqlNotifier.exe", "--c", Settings.Default.CheckForUpdatesFrequency, false, Utility.GetOsVersion() == Utility.OSVersion.WindowsXp);
        }
      }

      // TODO: Complete ▼ implement events for instance status changes and instances changes add old services to local machine.
      machinesList.MachineListChanged += new MachinesList.MachineListChangedHandler(machineList_machineChanged);

      //// Suscribe each service list from each machine to the corresponding event handlers.
      foreach (Machine m in machinesList.Machines)
      {
        // TODO: Support New Architecture
        //suscribe each Services list from each machine to the events.
        m.ServiceStatusChanged += mySQLServicesList_ServiceStatusChanged;
        m.ServiceListChanged += new Machine.ServiceListChangedHandler(mySQLServicesList_ServiceListChanged);
      }

      //TODO: Restore this ▼ function, handle machines unable to connect, dialogs and all...
      //UpdateListToRemoveDeletedServices();

      //// Load instances
      mySQLInstancesList.RefreshInstances(true);

      StartWatcherForFile(applicationDataFolderPath + @"\Oracle\MySQL Notifier\settings.config", settingsFile_Changed);

      WatchForServiceChanges();
      WatchForServiceDeletion();
      RefreshNotifierIcon();

      //// Migrate Notifier connections to the MySQL Workbench connections file if possible.
      MySqlWorkbench.MigrateExternalConnectionsToWorkbench();
    }

    private int CountServices()
    {
      int servicesCount = 0;
      foreach (Machine machine in machinesList.Machines)
      {
        servicesCount += machine.Services.Count;
      }
      return servicesCount;
    }

    /// <summary>
    /// Merge the old services schema into the new one
    /// </summary>
    private void MigrateOldServices()
    {
      //// Load old services schema
      mySQLServicesList = new MySQLServicesList();

      //// Attempt migration only if services were found
      if (mySQLServicesList.Services != null && mySQLServicesList.Services.Count > 0)
      {
        // TODO ▼ Unhardcode the index 0 based insertion.
        //// Create a local machine if it doesn't exist
        if (machinesList.Machines.Count <= 0 || machinesList.Machines[0].Name != "localhost")
        {
          machinesList.Machines.Insert(0, new Machine("localhost"));
        }

        //Copy services from old schema to the Local machine.
        foreach (MySQLService service in mySQLServicesList.Services)
          machinesList.Machines[0].ChangeService(ChangeListChangeType.AutoAdd, service);
        // TODO ▲ Unhardcode the index 0 based insertion.

        // Clear the old list of services to erase the duplicates on the newer schema
        mySQLServicesList.Services.Clear();

        //Persist the merged services into the newer machine.
        Settings.Default.Save();
      }
    }

    /// <summary>
    /// Initializes settings for the <see cref="MySqlWorkbenchConnectionsHelper"/> and <see cref="MySqlWorkbenchPasswordVault"/> classes.
    /// </summary>
    public static void InitializeMySQLWorkbenchStaticSettings()
    {
      string applicationDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
      MySqlWorkbench.ExternalApplicationName = AssemblyInfo.AssemblyTitle;
      MySqlWorkbenchPasswordVault.ApplicationPasswordVaultFilePath = applicationDataFolderPath + @"\Oracle\MySQL Notifier\user_data.dat";
      MySqlWorkbench.ExternalApplicationConnectionsFilePath = applicationDataFolderPath + @"\Oracle\MySQL Notifier\connections.xml";
      MySQLSourceTrace.LogFilePath = applicationDataFolderPath + @"\Oracle\MySQL Notifier";
      MySQLSourceTrace.SourceTraceClass = "MySqlNotifier";
    }

    /// <summary>
    /// Customizes the looks of the <see cref="MySQL.Utility.Forms.InfoDialog"/> form for the MySQL Notifier.
    /// </summary>
    private void CustomizeInfoDialog()
    {
      InfoDialog.ApplicationName = AssemblyInfo.AssemblyTitle;
      InfoDialog.SuccessLogo = Properties.Resources.ApplicationLogo;
      InfoDialog.ErrorLogo = Properties.Resources.NotifierErrorImage;
      InfoDialog.WarningLogo = Properties.Resources.NotifierWarningImage;
      InfoDialog.InformationLogo = Properties.Resources.ApplicationLogo;
      InfoDialog.ApplicationIcon = Properties.Resources.MySqlNotifierIcon;
    }

    private void WatchForServiceChanges()
    {
      try
      {
        List<ManagementScope> ms = machinesList.GetManagementScopes();
        foreach (ManagementScope managementScope in ms)
        {
          managementScope.Connect();
          WqlEventQuery query = new WqlEventQuery("__InstanceModificationEvent", new TimeSpan(0, 0, 1), "TargetInstance isa \"Win32_Service\"");
          ManagementEventWatcher watcher = new ManagementEventWatcher(managementScope, query);
          watcher.EventArrived += new EventArrivedEventHandler(ServiceChangedHandler);
          watcher.Start();
          watchers.Add(watcher);
        }
      }
      catch (ManagementException ex)
      {
        MySQLNotifierTrace.GetSourceTrace().WriteWarning("Critical Error when adding listener for events. - " + ex.Message + " " + ex.InnerException, 1);
      }
      catch (Exception)
      {
        //TODO ▲ Check why is it breaking up with remote connections now.
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

      //if (mode == "Disabled")
      //{
      //  //TODO: ▼ search the right ServicesList on the right machine
      //  mySQLServicesList.RemoveService(serviceName);
      //  return;
      //}

      //TODO: Unhardcode ▼
      Machine machine = machinesList.Machines.FirstOrDefault(m => m.MachineIDMatch("WIN7X64VM", "Javier"));

      Control c = notifyIcon.ContextMenuStrip;
      if (c.InvokeRequired)
      {
        serviceWindowsEvent se = new serviceWindowsEvent(GetWindowsEvent);
        se.Invoke(machine, serviceName, path, state);
      }
      else
        GetWindowsEvent(machine, serviceName, path, state);
    }

    private void WatchForServiceDeletion()
    {
      try
      {
        List<ManagementScope> ms = machinesList.GetManagementScopes();
        foreach (ManagementScope managementScope in ms)
        {
          managementScope.Connect();
          WqlEventQuery query = new WqlEventQuery("__InstanceDeletionEvent", new TimeSpan(0, 0, 1), "TargetInstance isa \"Win32_Service\"");
          ManagementEventWatcher watcher = new ManagementEventWatcher(managementScope, query);
          watcher.EventArrived += new EventArrivedEventHandler(ServiceDeletedHandler);
          watcher.Start();
          watchers.Add(watcher);
        }
      }
      catch (ManagementException ex)
      {
        MySQLNotifierTrace.GetSourceTrace().WriteWarning("Critical Error when adding listener for events. - " + ex.Message + " " + ex.InnerException, 1);
      }
      catch (Exception)
      {
        //TODO ▲ Check why is it breaking up with remote connections now.
      }
    }

    private void ServiceDeletedHandler(object sender, EventArrivedEventArgs args)
    {
      var e = args.NewEvent;
      ManagementBaseObject o = ((ManagementBaseObject)e["TargetInstance"]);
      if (o == null) return;

      //TODO: Fix ▼
      string serviceName = o["Name"].ToString().Trim();
      Control c = notifyIcon.ContextMenuStrip;

      //TODO: UNharcode this ▼▼ and find something more eficient than look for servicename.
      Machine machine = machinesList.Machines.FirstOrDefault(m => m.MachineIDMatch("WIN7X64VM", "Javier"));
      MySQLService service = machine.GetServiceByName(serviceName);

      if (c.InvokeRequired)
        c.Invoke(new MethodInvoker(() =>
        {
          RemoveServiceAndNotify(machine, service);
        }));
      else
        RemoveServiceAndNotify(machine, service);
    }

    /// <summary>
    /// Remove Service and Notify the user
    /// </summary>
    private void RemoveServiceAndNotify(Machine machine, MySQLService service)
    {
      //TODO: ▼ search the right ServicesList on the right machine
      machine.ChangeService(ChangeListChangeType.Remove, service);
      RefreshNotifierIcon();
      if (!Settings.Default.NotifyOfStatusChange) return;
      SetNotifyIconToolTip();
      ShowTooltip(false, Resources.BalloonTitleTextServiceList, String.Format(Resources.ServiceRemoved, service.ServiceName), 1500);
    }

    /// <summary>
    /// Generic routine to help with showing tooltips
    /// </summary>
    private void ShowTooltip(bool error, string title, string text, int delay)
    {
      notifyIcon.BalloonTipIcon = error ? ToolTipIcon.Error : ToolTipIcon.Info;
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

      RefreshNotifierIcon();
    }

    /// <summary>
    /// Method to handle the change events in the Workbench's connections file.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void connectionsFile_Changed(object sender, FileSystemEventArgs e)
    {
      MySqlWorkbench.Servers = new MySqlWorkbenchServerCollection();
      MySqlWorkbench.LoadData();
      mySQLInstancesList.RefreshInstances(false);

      foreach (Machine machine in machinesList.Machines)
      {
        foreach (var item in machine.Services)
        {
          item.MenuGroup.RefreshMenu(notifyIcon.ContextMenuStrip);
        }
      }
    }

    /// <summary>
    /// Method to handle the change events in the Workbench's server instances file, no changes in UI.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void serversFile_Changed(object sender, FileSystemEventArgs e)
    {
      MySqlWorkbench.Servers = new MySqlWorkbenchServerCollection();
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

      if (MySqlWorkbench.AllowsExternalConnectionsManagement)
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

      if (CountServices() + mySQLInstancesList.Count > 0)
      {
        ToolStripMenuItem actionsMenu = new ToolStripMenuItem(Resources.Actions, null);
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

    /// <summary>
    /// Checks if the context menus need to be rebuilt.
    /// </summary>
    /// <param name="itemRemoved">Flag indicating if a service or instance was removed.</param>
    /// <returns>true if the menu was rebuilt, false otherwise.</returns>
    private bool RebuildMenuIfNeeded(bool itemRemoved)
    {
      bool menuWasRebuilt = false;

      if ((CountServices() + mySQLInstancesList.Count == 0 && itemRemoved) ||
         (previousServicesAndInstancesQuantity == 0 && !itemRemoved))
      {
        ReBuildMenu();
        previousServicesAndInstancesQuantity = CountServices() + mySQLInstancesList.Count;
        menuWasRebuilt = true;
      }

      return menuWasRebuilt;
    }

    private void ServiceListChanged(MySQLService service, ChangeListChangeType changeType)
    {
      bool serviceRemoved = changeType == ChangeListChangeType.Remove;
      RebuildMenuIfNeeded(serviceRemoved);
      if (serviceRemoved)
      {
        service.MenuGroup.RemoveFromContextMenu(notifyIcon.ContextMenuStrip);
      }
      else
      {
        if (service.MenuGroup != null)
          service.MenuGroup.AddToContextMenu(notifyIcon.ContextMenuStrip);
        service.StatusChangeError += new MySQLService.StatusChangeErrorHandler(service_StatusChangeError);
        if (changeType == ChangeListChangeType.AutoAdd && Settings.Default.NotifyOfAutoServiceAddition)
        {
          ShowTooltip(false, Resources.BalloonTitleTextServiceList, string.Format(Resources.BalloonTextServiceList, service.DisplayName), 1500);
        }
      }
    }

    private void MachineListChanged(Machine machine, ChangeListChangeType changeType)
    {
      //TODO: Add newer machine to the Group Menu.
    }

    private void service_StatusChangeError(object sender, Exception ex)
    {
      MySQLService service = (MySQLService)sender;
      ShowTooltip(true, Resources.BalloonTitleFailedStatusChange, string.Format(Resources.BalloonTextFailedStatusChange, service.DisplayName, Environment.NewLine + ex.Message + Environment.NewLine + Resources.AskRestartApplication), 1500);
      MySQLNotifierTrace.GetSourceTrace().WriteError("Critical Error when trying to update the service status - " + (ex.Message + " " + ex.InnerException), 1);
    }

    private void service_StatusChanged(object sender, ServiceStatus args)
    {
      MySQLService service = (MySQLService)sender;
      service.MenuGroup.Update();
      notifyIcon.ContextMenuStrip.Refresh();
    }

    private void mySQLServicesList_ServiceListChanged(object sender, MySQLService service, ChangeListChangeType changeType)
    {
      ServiceListChanged(service, changeType);
    }

    //TODO:▼▼
    private void machineList_machineChanged(object sender, Machine machine, ChangeListChangeType changeType)
    {
      MachineListChanged(machine, changeType);
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

      foreach (ManagementEventWatcher watcher in watchers)
        watcher.Stop();

      if (this.Exit != null)
        Exit(this, e);
    }

    private void mySQLServicesList_ServiceStatusChanged(object sender, ServiceStatus args)
    {
      if (!Settings.Default.NotifyOfStatusChange)
      {
        return;
      }

      //TODO: ▼ UNhardcode this.
      Machine machine = machinesList.Machines.FirstOrDefault(m => m.MachineIDMatch("WIN7X64VM", "Javier"));
      MySQLService service = machine.GetServiceByDisplayName(args.ServiceDisplayName);

      if (!service.NotifyOnStatusChange)
      {
        return;
      }

      if (service.UpdateTrayIconOnStatusChange)
      {
        RefreshNotifierIcon();
      }

      ShowTooltip(false, Resources.BalloonTitleTextServiceStatus, string.Format(Resources.BalloonTextServiceStatus, args.ServiceDisplayName, args.PreviousStatus.ToString(), args.CurrentStatus.ToString()), 1500);
    }

    private void manageServicesDialogItem_Click(object sender, EventArgs e)
    {
      ManageItemsDialog dialog = new ManageItemsDialog(mySQLInstancesList, machinesList);
      dialog.ShowDialog();

      //// Update icon
      RefreshNotifierIcon();
    }

    private void launchInstallerItem_Click(object sender, EventArgs e)
    {
      string path = MySqlInstaller.GetInstallerPath();
      if (String.IsNullOrEmpty(path)) return;  // this should not happen since our menu itemText is enabled

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

      //// If there was a change in the setting for the icons then refresh Icon
      if (usecolorfulIcons != Properties.Settings.Default.UseColorfulStatusIcons)
      {
        RefreshNotifierIcon();
      }
    }

    private void InstallAvailablelUpdates_Click(object sender, EventArgs e)
    {
      launchInstallerItem_Click(null, EventArgs.Empty);
      Settings.Default.UpdateCheck = 0;
      Settings.Default.Save();
      RefreshNotifierIcon();
    }

    private void IgnoreAvailableUpdateItem_Click(object sender, EventArgs e)
    {
      DialogResult result = MessageBox.Show("This action will completely ignore the available software updates. Would you like to continue?",
        "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
      if (result == DialogResult.Yes)
      {
        Settings.Default.UpdateCheck = 0;
        Settings.Default.Save();
        RefreshNotifierIcon();
      }
    }

    private void LaunchWorkbenchUtilities_Click(object sender, EventArgs e)
    {
      MySqlWorkbench.LaunchUtilitiesShell();
    }

    /// <summary>
    /// When the exit menu itemText is clicked, make a call to terminate the ApplicationContext.
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
                                         string.Format("{0}.{1}.{2}", version[0], version[1], version[2]),
                                         string.Format(Properties.Resources.ToolTipText, CountServices(), mySQLInstancesList.Count));
      notifyIcon.Text = (toolTipText.Length >= MAX_TOOLTIP_LENGHT ? toolTipText.Substring(0, MAX_TOOLTIP_LENGHT - 3) + "..." : toolTipText);
    }

    private void GetWindowsEvent(Machine machine, string serviceName, string path, string state)
    {
      Control c = notifyIcon.ContextMenuStrip;

      if (c.InvokeRequired)
      {
        c.Invoke(new MethodInvoker(() => { GetWindowsEvent(machine, serviceName, path, state); }));
      }
      else
      {
        // TODO: FIX THE WAY WMI Notifies for all services... :/
        //TODO: ▼ search the right ServicesList on the right machine
        //var service = mySQLServicesList.GetServiceByName(serviceName);
        var service = machine.GetServiceByName(serviceName);

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

        machine.SetServiceStatus(serviceName, path, state);

        if (service == null || service.UpdateTrayIconOnStatusChange)
        {
          RefreshNotifierIcon();
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

    //TODO: Restore this ▼ function
    private void UpdateListToRemoveDeletedServices()
    {
      if (mySQLServicesList.Services == null) return;

      // TODO: Why this comment bellow ▼ deletes the remote services up.
      // /*old call >>  */if (mySQLServicesList.Services.Count(t => t.FoundInSystem) < mySQLServicesList.Services.Count)
      /*new call >> */
      if (mySQLServicesList.Services.Count(t => (t.Problem == ServiceProblem.None)) < mySQLServicesList.Services.Count)
      {
        // /* old call >> */mySQLServicesList.Services = mySQLServicesList.Services.Where(t => t.FoundInSystem).ToList();
        /*new call >> */
        Settings.Default.ServiceList = mySQLServicesList.Services.Where(t => (t.Problem == ServiceProblem.None)).ToList();
        Settings.Default.Save();
      }
    }

    /// <summary>
    /// Adds or removes a context menu item that represents the parent of the MySQL instances menu items.
    /// </summary>
    private void SetupMySQLInstancesMainMenuItem()
    {
      int index = MySQLInstanceMenuGroup.FindMenuItemWithinMenuStrip(notifyIcon.ContextMenuStrip, Resources.MySQLInstances);
      if (index < 0 && mySQLInstancesList.Count > 0)
      {
        index = MySQLInstanceMenuGroup.FindMenuItemWithinMenuStrip(notifyIcon.ContextMenuStrip, Resources.Actions);
        if (index < 0)
        {
          index = 0;
        }

        //// Hide the separator just above this new menu item.
        if (index > 0 && notifyIcon.ContextMenuStrip.Items[index - 1] is ToolStripSeparator)
        {
          notifyIcon.ContextMenuStrip.Items[index - 1].Visible = false;
        }

        ToolStripMenuItem instancesMainMenuItem = new ToolStripMenuItem(Resources.MySQLInstances);
        Font boldFont = new Font(instancesMainMenuItem.Font, FontStyle.Bold);
        instancesMainMenuItem.Font = boldFont;
        instancesMainMenuItem.BackColor = SystemColors.MenuText;
        instancesMainMenuItem.ForeColor = SystemColors.Menu;
        notifyIcon.ContextMenuStrip.Items.Insert(index, instancesMainMenuItem);
        notifyIcon.ContextMenuStrip.Refresh();
      }
      else if (index >= 0 && mySQLInstancesList.Count == 0)
      {
        //// Show the separator just above this new menu item if it's hidden.
        if (notifyIcon.ContextMenuStrip.Items[index - 1] is ToolStripSeparator)
        {
          notifyIcon.ContextMenuStrip.Items[index - 1].Visible = true;
        }

        notifyIcon.ContextMenuStrip.Items.RemoveAt(index);
        notifyIcon.ContextMenuStrip.Refresh();
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="mySQLInstancesList"/> list changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="args">Event arguments.</param>
    private void MySQLInstancesListChanged(object sender, InstancesListChangedArgs args)
    {
      bool instanceRemoved = args.ListChange == ListChangedType.ItemDeleted;
      RebuildMenuIfNeeded(instanceRemoved);
      switch (args.ListChange)
      {
        case ListChangedType.ItemAdded:
          SetupMySQLInstancesMainMenuItem();
          args.Instance.MenuGroup.AddToContextMenu(notifyIcon.ContextMenuStrip);
          break;

        case ListChangedType.ItemDeleted:
          SetupMySQLInstancesMainMenuItem();
          args.Instance.MenuGroup.RemoveFromContextMenu(notifyIcon.ContextMenuStrip);
          RefreshNotifierIcon();
          break;

        case ListChangedType.ItemChanged:
          args.Instance.MenuGroup.Update();
          RefreshNotifierIcon();
          break;

        case ListChangedType.Reset:
          SetupMySQLInstancesMainMenuItem();
          mySQLInstancesList.RefreshInstances(false);
          RefreshNotifierIcon();
          break;
      }
    }

    /// <summary>
    /// Event delegate method fired when a MySQL Instance's status witin the <see cref="mySQLInstancesList"/> changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="args">Event arguments.</param>
    private void MySQLInstanceStatusChanged(object sender, InstanceStatusChangedArgs args)
    {
      args.Instance.MenuGroup.Update();
      notifyIcon.ContextMenuStrip.Refresh();

      if (args.OldInstanceStatus != MySqlWorkbenchConnection.ConnectionStatusType.Unknown && args.Instance.MonitorAndNotifyStatus)
      {
        ShowTooltip(false, Resources.BalloonTitleTextInstanceStatus, string.Format(Resources.BalloonTextInstanceStatus, args.Instance.HostIdentifier, args.NewInstanceStatusText), 1500);
      }

      if (args.Instance.UpdateTrayIconOnStatusChange)
      {
        RefreshNotifierIcon();
      }
    }

    /// <summary>
    /// Event delegate method fired when an error is thrown while testing a MySQL Instance's status witin the <see cref="mySQLInstancesList"/>.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="args">Event arguments.</param>
    private void MySQLInstanceConnectionStatusTestErrorThrown(object sender, InstanceConnectionStatusTestErrorThrownArgs args)
    {
      ShowTooltip(true, Resources.ErrorTitle, string.Format(Resources.BalloonTextFailedStatusCheck, args.Instance.HostIdentifier, args.ErrorException.Message), 1500);
    }

    /// <summary>
    /// Refreshes the Notifier main icon based on current services and instances statuses.
    /// </summary>
    private void RefreshNotifierIcon()
    {
      notifyIcon.Icon = Icon.FromHandle(GetIconForNotifier().GetHicon());
    }

    /// <summary>
    /// Returns a new tray icon for the Notifier based on current Services/Instances/Updates statuses.
    /// </summary>
    /// <returns>A bitmap of the updated tray icon.</returns>
    private Bitmap GetIconForNotifier()
    {
      bool hasUpdates = (Settings.Default.UpdateCheck & (int)SoftwareUpdateStaus.HasUpdates) != 0;
      bool useColorfulIcon = Settings.Default.UseColorfulStatusIcons;

      var updateIconServicesList = Settings.Default.ServiceList == null ? new List<MySQLService>() : Settings.Default.ServiceList.Where(t => t.UpdateTrayIconOnStatusChange);
      var updateIconInstancesList = Settings.Default.MySQLInstancesList == null ? new List<MySQLInstance>() : Settings.Default.MySQLInstancesList.Where(instance => instance.UpdateTrayIconOnStatusChange);

      //// Stopped or update+stopped notifier icon.
      if (updateIconServicesList.Where(t => t.Status == ServiceControllerStatus.Stopped).Count() + updateIconInstancesList.Where(i => i.ConnectionStatus == MySqlWorkbenchConnection.ConnectionStatusType.RefusingConnections).Count() > 0)
      {
        return useColorfulIcon ? (hasUpdates ? Properties.Resources.NotifierIconStoppedAlertStrong : Properties.Resources.NotifierIconStoppedStrong) : (hasUpdates ? Properties.Resources.NotifierIconStoppedAlert : Properties.Resources.NotifierIconStopped);
      }

      //// Starting or update+starting notifier icon.
      if (updateIconServicesList.Where(t => t.Status == ServiceControllerStatus.StartPending).Count() > 0)
      {
        return useColorfulIcon ? (hasUpdates ? Properties.Resources.NotifierIconStartingAlertStrong : Properties.Resources.NotifierIconStartingStrong) : (hasUpdates ? Properties.Resources.NotifierIconStartingAlert : Properties.Resources.NotifierIconStarting);
      }

      //// Running or update+running notifier icon.
      if (updateIconServicesList.Where(t => t.Status == ServiceControllerStatus.Running).Count() + updateIconInstancesList.Where(i => i.ConnectionStatus == MySqlWorkbenchConnection.ConnectionStatusType.AcceptingConnections).Count() > 0)
      {
        return hasUpdates ? Properties.Resources.NotifierIconRunningAlert : Properties.Resources.NotifierIconRunning;
      }

      //// Clean or update+clean notifier icon.
      return hasUpdates ? Properties.Resources.NotifierIconAlert : Properties.Resources.NotifierIcon;
    }
  }

  public enum ServiceListChangeType
  {
    Add,
    AutoAdd,
    Remove
  }

  public enum SoftwareUpdateStaus : int
  {
    Checking = 1,
    HasUpdates = 2,
    Notified = 4
  }
}