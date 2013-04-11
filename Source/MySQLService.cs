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
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Xml.Serialization;
using Microsoft.Win32;
using MySql.Notifier.Properties;
using MySQL.Utility;

namespace MySql.Notifier
{
  [Serializable]
  public class MySQLService
  {
    //private ServiceType winServiceType;
    // TODO: Verify all of these ▼ fields are required for a correct operation
    private ManagementObject managementObject;

    private string serviceName;
    private ServiceProblem serviceProblem;
    private ServiceMenuGroup menuGroup;

    /// <summary>
    /// Default constructor. DO NOT REMOVE.
    /// </summary>
    public MySQLService()
    {
    }

    public MySQLService(string serviceName, bool notificationOnChange, bool updatesTrayIcon, AccountLogin login = null)
    {
      Login = login ?? new AccountLogin();
      WinServiceType = Login.ServiceType;
      //TODO: Solve problem ▼ ...
      Host = Login.Host ?? ".";
      User = Login.User ?? string.Empty;
      Password = Login.Password;
      NotifyOnStatusChange = notificationOnChange;
      UpdateTrayIconOnStatusChange = updatesTrayIcon;
      ServiceName = serviceName;
    }

    [XmlAttribute(AttributeName = "ServiceType")]
    public ServiceType WinServiceType { get; set; }

    [XmlAttribute(AttributeName = "Host")]
    public string Host { get; set; }

    [XmlAttribute(AttributeName = "User")]
    public string User { get; set; }

    [XmlAttribute(AttributeName = "Password")]
    public string Password { get; set; }

    [XmlAttribute(AttributeName = "ServiceName")]
    public string ServiceName
    {
      get { return serviceName; }
      set { SetServiceName(value); }
    }

    [XmlAttribute(AttributeName = "UpdateTrayIconOnStatusChange")]
    public bool UpdateTrayIconOnStatusChange { get; set; }

    [XmlAttribute(AttributeName = "NotifyOnStatusChange")]
    public bool NotifyOnStatusChange { get; set; }

    [XmlIgnore]
    public ServiceProblem Problem
    {
      get
      {
        return serviceProblem;
      }
    }

    [XmlIgnore]
    public bool IsRealMySQLService { get; private set; }

    [XmlIgnore]
    public ServiceControllerStatus Status { get; set; }

    [XmlIgnore]
    public string DisplayName { get; private set; }

    [XmlIgnore]
    public ServiceMenuGroup MenuGroup //{ get; private set; }
    //TODO: Check why this implementation is correct ▼
    {
      get
      {
        if (menuGroup == null) SetService();
        return menuGroup;
      }

      set
      {
        menuGroup = value;
      }
    }

    [XmlIgnore]
    public List<MySqlWorkbenchConnection> WorkbenchConnections { get; private set; }

    [XmlIgnore]
    public bool WorkCompleted { get; private set; }

    [XmlIgnore]
    public bool FoundInSystem { get; private set; }

    [XmlIgnore]
    public AccountLogin Login { get; private set; }

    [XmlIgnore]
    public ManagementObject sManagementObject
    {
      get
      {
        return managementObject ?? ReturnServiceInstance();
      }
    }

    private void SetService()
    {
      if (sManagementObject == null) return;

      try
      {
        DisplayName = sManagementObject.Properties["DisplayName"].Value.ToString();
        FindMatchingWBConnections();
        menuGroup = new ServiceMenuGroup(this);
        switch (sManagementObject.Properties["State"].Value.ToString())
        {
          case "ContinuePending":
            Status = ServiceControllerStatus.ContinuePending;
            break;

          case "PausePending":
            Status = ServiceControllerStatus.PausePending;
            break;

          case "Paused":
            Status = ServiceControllerStatus.Paused;
            break;

          case "Running":
            Status = ServiceControllerStatus.Running;
            break;

          case "StartPending":
            Status = ServiceControllerStatus.StartPending;
            break;

          case "StopPending":
            Status = ServiceControllerStatus.StopPending;
            break;

          case "Stopped":
            Status = ServiceControllerStatus.Stopped;
            break;

          default:
            Status = ServiceControllerStatus.Stopped;
            break;
        }
      }
      catch (InvalidOperationException ioEx)
      {
        using (var errorDialog = new MessageDialog(Resources.HighSeverityError, ioEx.Message, true))
        {
          errorDialog.ShowDialog();
        }
        MySQLNotifierTrace.GetSourceTrace().WriteError(ioEx.Message, 1);
        managementObject = null;
      }
    }

    private void SetServiceName(string _serviceName)
    {
      serviceName = _serviceName;
      SetService();
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

    private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
        OnStatusChangeError(e.Error);
      else
      {
        // else no error
        // TODO Check if this ▼ call replaces exactly the managed call --> winService.Refresh() of type ServiceController.Refresh();
        managementObject = ReturnServiceInstance();
        WorkCompleted = true;
      }
    }

    private void worker_DoWork(object sender, DoWorkEventArgs e)
    {
      BackgroundWorker worker = sender as BackgroundWorker;

      if (!Service.ExistsServiceInstance(serviceName))
      {
        throw new Exception(String.Format(Resources.ServiceNotFound, serviceName));
      }

      TimeSpan timeout = TimeSpan.FromMilliseconds(5000);

      int action = (int)e.Argument;
      WorkCompleted = false;

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
        //TODO: Investigate how to replace this waiting calls ▼
        //if(WinServiceType == ServiceType.Local)
        //winService.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 30));
      }
      else
      {
        proc.StartInfo.FileName = "sc";
        proc.StartInfo.Arguments = string.Format(@" {0} {1}", action, ServiceName);
        proc.StartInfo.UseShellExecute = true;
        proc.Start();
        //TODO: Investigate how to replace this waiting calls ▼
        //if(WinServiceType == ServiceType.Local)
        //winService.WaitForStatus(action == "start" ? ServiceControllerStatus.Running : ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 30));
      }
    }

    private MySQLStartupParameters GetStartupParameters()
    {
      MySQLStartupParameters parameters = new MySQLStartupParameters();
      parameters.PipeName = "mysql";

      // get our host information
      parameters.HostName = (String.IsNullOrEmpty(Host) || Host == "." || Host == "localhost" || WinServiceType == ServiceType.Local) ? "localhost" : Host;
      parameters.HostIPv4 = Utility.GetIPv4ForHostName(parameters.HostName);

      if (WinServiceType != ServiceType.Local) return parameters;
      RegistryKey key = Registry.LocalMachine.OpenSubKey(String.Format(@"SYSTEM\CurrentControlSet\Services\{0}", ServiceName));
      if (key == null) return parameters;

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

    public ManagementObject ReturnServiceInstance()
    {
      ManagementObject result = null;
      serviceProblem = ServiceProblem.None;

      try
      {
        ManagementScope managementScope;

        if (WinServiceType == ServiceType.Local)
        {
          managementScope = new ManagementScope(@"root\cimv2");
        }
        else
        {
          Login = Login ?? new AccountLogin(Host, User, MySQLSecurity.DecryptPassword(Password));

          ManagementNamedValueCollection context = new ManagementNamedValueCollection();
          ConnectionOptions co = new ConnectionOptions();
          co.Username = User;
          co.Password = MySQLSecurity.DecryptPassword(Password);
          co.Impersonation = ImpersonationLevel.Impersonate;
          co.Authentication = AuthenticationLevel.Packet;
          co.EnablePrivileges = true;
          co.Context = context;
          co.Timeout = TimeSpan.FromSeconds(30);

          managementScope = new ManagementScope(@"\\" + Host + @"\root\cimv2", co);
        }
        managementScope.Connect();
        WqlObjectQuery query = new WqlObjectQuery(String.Format("Select * From Win32_Service Where Name = \"{0}\"", ServiceName));
        ManagementObjectSearcher searcher = new ManagementObjectSearcher(managementScope, query);
        ManagementObjectCollection retObjectCollection = searcher.Get();
        serviceProblem = (retObjectCollection.Count > 0) ? ServiceProblem.None : ServiceProblem.ServiceDoesNotExist;
        if (serviceProblem == ServiceProblem.None)
        {
          foreach (ManagementObject mo in retObjectCollection)
          {
            if (mo != null)
            {
              result = mo;
              break;
            }
          }
        }
      }
      catch (COMException)
      {
        serviceProblem = ServiceProblem.LackOfRemotePermissions;
      }
      catch (UnauthorizedAccessException)
      {
        serviceProblem = ServiceProblem.IncorrectUserOrPassword;
      }
      catch
      {
        serviceProblem = ServiceProblem.Other;
      }
      finally
      {
        FoundInSystem = (serviceProblem == ServiceProblem.ServiceDoesNotExist || serviceProblem == ServiceProblem.Other) ? false : true;
      }
      managementObject = result;
      return result;
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

  public enum ServiceProblem
  {
    None,
    LackOfRemotePermissions,
    IncorrectUserOrPassword,
    Other,
    ServiceDoesNotExist
  }
}