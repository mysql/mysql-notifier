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
using System.Collections.Generic;
using System.Collections;
using System.ServiceProcess;
using System.Windows.Forms;
using System.Linq;
using System.Globalization;
using System.Management;
using System.ComponentModel;
using MySQL.Utility;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using System.Net;
using System.Xml.Serialization;
using System.Configuration;
using MySql.Notifier.Properties;

namespace MySql.Notifier
{
  [Serializable]
  public class MySQLService 
  {
    private ServiceController winService;
    private string serviceName;

    public MySQLService()
    {
    }

    public MySQLService(string serviceName, bool notificationOnChange, bool updatesTrayIcon)
    {
      ServiceName = serviceName;
      NotifyOnStatusChange = notificationOnChange;
      UpdateTrayIconOnStatusChange = updatesTrayIcon;
    }

    [XmlAttribute(AttributeName = "ServiceName")]
    public string ServiceName
    { 
      get { return serviceName; }
      set { SetService(value); }
    }

    [XmlAttribute(AttributeName = "UpdateTrayIconOnStatusChange")]
    public bool UpdateTrayIconOnStatusChange { get; set; }

    [XmlAttribute(AttributeName = "NotifyOnStatusChange")]
    public bool NotifyOnStatusChange { get; set; }

    [XmlIgnore]
    public bool IsRealMySQLService { get; private set; }

    [XmlIgnore]
    public ServiceControllerStatus Status { get; set; }

    [XmlIgnore]
    public string DisplayName { get; private set; }

    [XmlIgnore]
    public ServiceMenuGroup MenuGroup { get; private set; }

    [XmlIgnore]
    public List<MySqlWorkbenchConnection> WorkbenchConnections { get; private set; }

    [XmlIgnore]
    public bool workCompleted { get; private set; }


    private void SetService(string serviceName)
    {
      winService = new ServiceController(serviceName);
   
      try
      {
        DisplayName = winService.DisplayName;
        this.serviceName = winService.ServiceName;
        Status = winService.Status;
        FindMatchingWBConnections();
        MenuGroup = new ServiceMenuGroup(this);
      }
      catch (InvalidOperationException ioEx)
      {
        using (var errorDialog = new MessageDialog(Resources.HighSeverityError, ioEx.Message, true))
        {
          errorDialog.ShowDialog();          
        }
        MySQLNotifierTrace.GetSourceTrace().WriteError(ioEx.Message, 1);
        winService = null;        
      }
    }


    private void SeeIfRealMySQLService(string cmd)
    {
      IsRealMySQLService = cmd.EndsWith("mysqld.exe") || cmd.EndsWith("mysqld-nt.exe") || cmd.EndsWith("mysqld") || cmd.EndsWith("mysqld-nt");
    }

    internal void FindMatchingWBConnections()
    {
            
      // first we discover what parameters we were started with
      MySQLStartupParameters parameters = GetStartupParameters();
      WorkbenchConnections = new List<MySqlWorkbenchConnection>();

      if (!IsRealMySQLService) return;
        
      var filteredConnections = MySqlWorkbench.Connections.Where(t => !String.IsNullOrEmpty(t.Name) && t.Port == parameters.Port);

      if (filteredConnections != null)
      {
        foreach (MySqlWorkbenchConnection c in filteredConnections)
        {
          switch (c.DriverType)
          {
            case MySqlWorkbenchConnectionType.NamedPipes:
              if (!parameters.NamedPipesEnabled || String.Compare(c.Socket, parameters.PipeName, true) != 0) continue;
              break;
            case MySqlWorkbenchConnectionType.Ssh:
              continue;
            case MySqlWorkbenchConnectionType.Tcp:
              if (c.Port != parameters.Port) continue;
              break;
            case MySqlWorkbenchConnectionType.Unknown:
              continue;
          }         
          
          if (!Utility.IsValidIpAddress(c.Host)) //matching connections by Ip
          {
            if (Utility.GetIPv4ForHostName(c.Host) != parameters.HostIPv4) continue;
          }
          else
          {
            if (c.Host != parameters.HostIPv4) continue;
          }
         
          WorkbenchConnections.Add(c);
        }
      }
    }

    /// <summary>
    /// This event system handles the case where the service failed to move to a proposed status
    /// </summary>
    public delegate void StatusChangeErrorHandler(object sender, Exception ex);
    public event StatusChangeErrorHandler StatusChangeError;

    private void OnStatusChangeError(Exception ex)
    {
      if (StatusChangeError != null)
        StatusChangeError(this, ex);
    }

    /// <summary>
    /// This event system handles the case where the service successfully moved to a new status
    /// </summary>
    public delegate void StatusChangedHandler(object sender, ServiceStatus args);
    public event StatusChangedHandler StatusChanged;
    protected virtual void OnStatusChanged(ServiceStatus args)
    {
      if (this.StatusChanged != null)
        this.StatusChanged(this, args);
    }

    public void UpdateMenu(string statusString)
    {              
        SetStatus(statusString);
        MenuGroup.Update();  
    }

    public void SetStatus(string statusString)
    {
      ServiceControllerStatus newStatus;
      bool parsed = Enum.TryParse(statusString, out newStatus);
      if (parsed)
      {
        if (newStatus == Status) return;
        ServiceControllerStatus copyPreviousStatus = Status;
        Status = newStatus;
        OnStatusChanged(new ServiceStatus(DisplayName, copyPreviousStatus, Status));
      }
    }

    /// <summary>
    /// Attempts to start the current MySQL Service
    /// </summary>
    /// <returns>Flag indicating if the action completed succesfully</returns>
    public void Start()
    {
      ChangeServiceStatus(1);
    }

    /// <summary>
    /// Attempts to stop the current MySQL Service
    /// </summary>
    /// <returns>Flag indicating if the action completed succesfully</returns>
    public void Stop()
    {
      ChangeServiceStatus(0);
    }

    /// <summary>
    /// Attempts to stop and then start the current MySQL Service
    /// </summary>
    /// <returns>Flag indicating if the action completed succesfully</returns>
    public void Restart()
    {
      ChangeServiceStatus(2);
    }

    private void ChangeServiceStatus(int action)
    {
      BackgroundWorker worker = new BackgroundWorker();
      worker.WorkerSupportsCancellation = false;
      worker.WorkerReportsProgress = false;
      worker.DoWork += new DoWorkEventHandler(worker_DoWork);
      worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
      worker.RunWorkerAsync(action);
    }

    void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)    
        OnStatusChangeError(e.Error);      
      else
      {
        // else no error
        winService.Refresh();
        workCompleted = true;       
      }
    }
   

    void worker_DoWork(object sender, DoWorkEventArgs e)
    {
      TimeSpan timeout = TimeSpan.FromMilliseconds(5000);
      BackgroundWorker worker = sender as BackgroundWorker;
      int action = (int)e.Argument;
      workCompleted = false;

      switch (action)
      { 
        case 0:
          ProcessStatusService("stop");
          break;
        case 1:
          ProcessStatusService("start");
          break;
        case 2:
          ProcessStatusService("restart");
          break;                
      }
    }

    private void ProcessStatusService(string action)
    {
      Process proc = new Process();
      proc.StartInfo.Verb = "runas";
      proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

      if (action == "restart")
      {
        proc.StartInfo.FileName = "cmd.exe";
        proc.StartInfo.Arguments = "/C net stop " + @"" + serviceName + @"" + " && net start " + serviceName + @"";
        proc.StartInfo.UseShellExecute = true;
        proc.Start();
        winService.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 30));      
      }
      else
      {       
        proc.StartInfo.FileName = "sc";
        proc.StartInfo.Arguments = string.Format(@" {0} {1}", action, ServiceName);
        proc.StartInfo.UseShellExecute = true;
        proc.Start();
        winService.WaitForStatus(action == "start" ? ServiceControllerStatus.Running : ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 30));
      }
    }
    
    private MySQLStartupParameters GetStartupParameters()
    {
      MySQLStartupParameters parameters = new MySQLStartupParameters();
      parameters.PipeName = "mysql";

      // get our host information
      parameters.HostName = winService.MachineName == "." ? "localhost" : winService.MachineName;
      parameters.HostIPv4 = Utility.GetIPv4ForHostName(parameters.HostName);

      RegistryKey key = Registry.LocalMachine.OpenSubKey(String.Format(@"SYSTEM\CurrentControlSet\Services\{0}", winService.ServiceName));
      string imagepath = (string)key.GetValue("ImagePath", null);
      key.Close();
      if (imagepath == null) return parameters;

      string[] args = Utility.SplitArgs(imagepath);
      SeeIfRealMySQLService(args[0]);

      // Parse our command line args
      Mono.Options.OptionSet p = new Mono.Options.OptionSet()
        .Add("defaults-file=", "", v => parameters.DefaultsFile = v)
        .Add("port=|P=", "", v => Int32.TryParse(v, out parameters.Port))
        .Add("enable-named-pipe", v => parameters.NamedPipesEnabled = true)
        .Add("socket=", "", v => parameters.PipeName = v);
      p.Parse(args);
      if (parameters.DefaultsFile == null) return parameters;

      // we have a valid defaults file
      IniFile f = new IniFile(parameters.DefaultsFile);
      Int32.TryParse(f.ReadValue("mysqld", "port", parameters.Port.ToString()), out parameters.Port);
      parameters.PipeName = f.ReadValue("mysqld", "socket", parameters.PipeName);

      // now see if named pipes are enabled
      parameters.NamedPipesEnabled = parameters.NamedPipesEnabled || f.HasKey("mysqld", "enable-named-pipe");

      return parameters;
    }
  }

  public struct MySQLStartupParameters
  {
    public string DefaultsFile;
    public string HostName;
    public string HostIPv4;
    public int Port;
    public string PipeName;
    public bool NamedPipesEnabled;
  }
}


