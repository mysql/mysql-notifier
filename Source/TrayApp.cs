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
using MySql.TrayApp.Properties;
using System.Threading;
using MySQL.Utility;


namespace MySql.TrayApp
{
  class TrayApp
  {
    private bool hasAdminPrivileges { get; set; }
    private System.ComponentModel.IContainer components;
    private NotifyIcon notifyIcon;
    private MySQLServicesList mySQLServicesList { get; set; }
    private ManagementEventWatcher watcher;

    public TrayApp(bool adminPrivileges)
    {
      hasAdminPrivileges = adminPrivileges;
      Bitmap iconBitmap = Properties.Resources.default_icon;
      if (Settings.Default.ServicesMonitor == null)
        Settings.Default.ServicesMonitor = new System.Collections.Specialized.StringCollection();

      components = new System.ComponentModel.Container();
      notifyIcon = new NotifyIcon(components)
                    {
                      ContextMenuStrip = new ContextMenuStrip(),
                      Icon = Icon.FromHandle(iconBitmap.GetHicon()),
                      Visible = true
                    };
      notifyIcon.MouseClick += notifyIcon_MouseClick;
      notifyIcon.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);
      notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
      notifyIcon.BalloonTipTitle = Properties.Resources.BalloonTitleTextServiceStatus;

      // Setup our service list
      mySQLServicesList = new MySQLServicesList(hasAdminPrivileges);
      mySQLServicesList.ServiceStatusChanged += mySQLServicesList_ServiceStatusChanged;
      mySQLServicesList.ServiceListChanged += new MySQLServicesList.ServiceListChangedHandler(mySQLServicesList_ServiceListChanged);

      // loads all the services from our settings file and sets up their menus
      mySQLServicesList.LoadFromSettings();
      AddStaticMenuItems();
      SetNotifyIconToolTip();

      // listener for events
      var managementScope = new ManagementScope(@"root\cimv2");
      managementScope.Connect();

      // WqlEventQuery query = new WqlEventQuery("__InstanceModificationEvent", new TimeSpan(0, 0, 1), "TargetInstance isa \"Win32_Service\" AND ( TargetInstance.Name LIKE \"%MYSQL%\" OR TargetInstance.PathName LIKE \"%MYSQL%\" ) ");
      WqlEventQuery query = new WqlEventQuery("__InstanceModificationEvent", new TimeSpan(0, 0, 1), "TargetInstance isa \"Win32_Service\"");
      watcher = new ManagementEventWatcher(managementScope, query);
      watcher.EventArrived += new EventArrivedEventHandler(watcher_EventArrived);
      watcher.Start();
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
    }

    /// <summary>
    /// Adds the static menu items such as Options, Exit, About..
    /// </summary>
    private void AddStaticMenuItems()
    {
      var shieldBitmap = SystemIcons.Shield.ToBitmap();
      shieldBitmap.SetResolution(16, 16);
      ToolStripMenuItem manageServices = new ToolStripMenuItem("Manage Services");    
      manageServices.Click += new EventHandler(manageServicesDialogItem_Click);

      ToolStripMenuItem launchInstaller = new ToolStripMenuItem("Launch Installer");
      bool installerInstalled = MySqlInstaller.IsInstalled;
      launchInstaller.Click += new EventHandler(launchInstallerItem_Click);
      launchInstaller.Enabled = installerInstalled;

      ToolStripMenuItem checkForUpdates = new ToolStripMenuItem("Check for updates");
      checkForUpdates.Click += new EventHandler(checkUpdatesItem_Click);
      checkForUpdates.Enabled = installerInstalled;

      ToolStripMenuItem actionsMenu = new ToolStripMenuItem("Actions", null, manageServices, launchInstaller, checkForUpdates);

      ToolStripMenuItem optionsMenu = new ToolStripMenuItem("Options...");
      optionsMenu.Click += new EventHandler(optionsItem_Click);

      ToolStripMenuItem aboutMenu = new ToolStripMenuItem("About...");
      aboutMenu.Click += new EventHandler(aboutMenu_Click);

      ToolStripMenuItem exitMenu = new ToolStripMenuItem("Close MySQL Tray App");
      exitMenu.Click += new EventHandler(exitItem_Click);

      actionsMenu.DropDownItems.Add(new ToolStripSeparator());
      actionsMenu.DropDownItems.Add(optionsMenu);
      actionsMenu.DropDownItems.Add(aboutMenu);
      actionsMenu.DropDownItems.Add(exitMenu);

      notifyIcon.ContextMenuStrip.Items.Add(actionsMenu);
    }

    private void ServiceListChanged(MySQLService service, ServiceListChangeType changeType)
    {
      if (changeType == ServiceListChangeType.Remove)
      {
        service.MenuGroup.RemoveFromContextMenu(notifyIcon.ContextMenuStrip);
        return;
      }

      // the rest of this is for additions
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

      if (this.Exit != null)
        Exit(this, e);
    }

    private void mySQLServicesList_ServiceStatusChanged(object sender, ServiceStatus args)
    {
      if (!Settings.Default.NotifyOfStatusChange) return;

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
      AboutDialog dlg = new AboutDialog();
      dlg.ShowDialog();
    }


    private void optionsItem_Click(object sender, EventArgs e)
    {
      OptionsDialog dlg = new OptionsDialog();
      dlg.ShowDialog();
    }

    private void restartApp_Click(object sender, EventArgs e)
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
      
      if (ranElevated)
        OnExit(EventArgs.Empty);
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
        c.Invoke((MethodInvoker)delegate
        {
          mySQLServicesList.SetServiceStatus(serviceName, path, state);
          SetNotifyIconToolTip();
        });
      else
      {
        mySQLServicesList.SetServiceStatus(serviceName, path, state);
        SetNotifyIconToolTip();
      }
    }

  }
}
