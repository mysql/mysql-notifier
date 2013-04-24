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
using System.Xml;
using System.Xml.Serialization;
using MySql.Notifier.Properties;
using MySQL.Utility;

namespace MySql.Notifier
{
  [Serializable]
  public class MySQLService
  {
    [XmlIgnore]
    private ManagementObject managementObject;

    [XmlIgnore]
    private ServiceProblem serviceProblem;

    [XmlIgnore]
    private ServiceMenuGroup menuGroup;

    [XmlIgnore]
    private string displayName;

    [XmlAttribute(AttributeName = "ServiceType")]
    public ServiceMachineType WinServiceType { get; set; }

    [XmlAttribute(AttributeName = "ServiceName")]
    public string ServiceName { get; set; }

    [XmlAttribute(AttributeName = "DisplayName")]
    public string DisplayName
    {
      get
      {
        return displayName;
      }
      set
      {
        if (!String.IsNullOrEmpty(value))
        {
          displayName = value;
        }
      }
    }

    [XmlAttribute(AttributeName = "UpdateTrayIconOnStatusChange")]
    public bool UpdateTrayIconOnStatusChange { get; set; }

    [XmlAttribute(AttributeName = "NotifyOnStatusChange")]
    public bool NotifyOnStatusChange { get; set; }

    [XmlIgnore]
    public Machine Host { get; set; }

    [XmlIgnore]
    public ServiceMenuGroup MenuGroup
    {
      get
      {
        if (menuGroup == null) SetServiceParameters();
        return menuGroup;
      }
      set
      {
        menuGroup = value;
      }
    }

    [XmlIgnore]
    public ServiceProblem Problem
    {
      get
      {
        return serviceProblem;
      }
    }

    [XmlIgnore]
    public ManagementObject serviceManagementObject
    {
      get
      {
        return managementObject;
      }
    }

    [XmlIgnore]
    public bool IsRealMySQLService { get; private set; }

    [XmlIgnore]
    public ServiceControllerStatus Status { get; set; }

    [XmlIgnore]
    public List<MySqlWorkbenchConnection> WorkbenchConnections { get; private set; }

    [XmlIgnore]
    public bool WorkCompleted { get; private set; }

    [XmlIgnore]
    public bool FoundInSystem { get; private set; }

    [XmlIgnore]
    private MySQLStartupParameters parameters { get; set; }

    /// <summary>
    /// DO NOT REMOVE. Default constructor required for serialization-deserialization.
    /// </summary>
    public MySQLService()
    {
    }

    public MySQLService(string serviceName, bool notificationOnChange, bool updatesTrayIcon, Machine machine = null)
      : this()
    {
      Host = machine ?? new Machine("localhost");
      WinServiceType = (Host.Name == "localhost") ? ServiceMachineType.Local : ServiceMachineType.Remote;
      NotifyOnStatusChange = notificationOnChange;
      UpdateTrayIconOnStatusChange = updatesTrayIcon;
      ServiceName = serviceName;
      displayName = String.Empty;
      SetServiceParameters();
    }

    public void SeeIfRealMySQLService(string cmd)
    {
      IsRealMySQLService = cmd.EndsWith("mysqld.exe") || cmd.EndsWith("mysqld-nt.exe") || cmd.EndsWith("mysqld") || cmd.EndsWith("mysqld-nt");
    }

    public void SetServiceParameters()
    {
      GetServiceInstance();

      if (serviceProblem != ServiceProblem.ServiceDoesNotExist && serviceProblem != ServiceProblem.None)
      {
        return;
      }

      try
      {
        DisplayName = serviceManagementObject.Properties["DisplayName"].Value.ToString();
        FindMatchingWBConnections();
        switch (serviceManagementObject.Properties["State"].Value.ToString())
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
        menuGroup = new ServiceMenuGroup(this);
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

    private void GetServiceInstance()
    {
      managementObject = null;
      serviceProblem = ServiceProblem.None;

      try
      {
        ManagementObjectCollection retObjectCollection = Host.GetServices(ServiceName);
        serviceProblem = (retObjectCollection.Count > 0) ? ServiceProblem.None : ServiceProblem.ServiceDoesNotExist;
        if (serviceProblem == ServiceProblem.None)
        {
          foreach (ManagementObject mo in retObjectCollection)
          {
            if (mo != null)
            {
              managementObject = mo;
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
    }

    internal void FindMatchingWBConnections()
    {
      // first we discover what parameters we were started with
      //MySQLStartupParameters parameters = GetStartupParameters();
      if (String.IsNullOrEmpty(parameters.HostName)) return;
      WorkbenchConnections = new List<MySqlWorkbenchConnection>();

      if (!IsRealMySQLService) return;

      var filteredConnections = MySqlWorkbench.WorkbenchConnections.Where(t => !String.IsNullOrEmpty(t.Name) && t.Port == parameters.Port);

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

    public void SetStatus(string statusString)
    {
      ServiceControllerStatus newStatus;
      bool parsed = Enum.TryParse(statusString, out newStatus);
      if (parsed)
      {
        if (newStatus == Status) return;
        ServiceControllerStatus copyPreviousStatus = Status;
        Status = newStatus;
        OnStatusChanged(this, new ServiceStatus(DisplayName, copyPreviousStatus, Status));
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
        //managementObject = ReturnServiceInstance();
        WorkCompleted = true;
      }
    }

    private void worker_DoWork(object sender, DoWorkEventArgs e)
    {
      //TODO: ▼ ReEngineer this functionality ▼
      BackgroundWorker worker = sender as BackgroundWorker;

      if (!Service.ExistsServiceInstance(ServiceName))
      {
        throw new Exception(String.Format(Resources.ServiceNotFound, ServiceName));
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
      if (WinServiceType == ServiceMachineType.Local)
      {
        ServiceController winService = new ServiceController(ServiceName);
        Process proc = new Process();
        proc.StartInfo.Verb = "runas";
        proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

        if (action == "restart")
        {
          proc.StartInfo.FileName = "cmd.exe";
          proc.StartInfo.Arguments = "/C net stop " + @"" + ServiceName + @"" + " && net start " + ServiceName + @"";
          proc.StartInfo.UseShellExecute = true;
          proc.Start();

          if (WinServiceType == ServiceMachineType.Local)
            winService.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 30));
        }
        else
        {
          proc.StartInfo.FileName = "sc";
          proc.StartInfo.Arguments = string.Format(@" {0} {1}", action, ServiceName);
          proc.StartInfo.UseShellExecute = true;
          proc.Start();

          if (WinServiceType == ServiceMachineType.Local)
            winService.WaitForStatus(action == "start" ? ServiceControllerStatus.Running : ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 30));
        }
      }

      // TODO: ▼ Implement start/stop wql commands for remote services▼
      else
      {
        //if(Host == null) return;
      }
    }

    /// <summary>
    /// This event system handles the case where the service failed to move to a proposed status
    /// </summary>
    public delegate void StatusChangeErrorHandler(MySQLService sender, Exception ex);

    public event StatusChangeErrorHandler StatusChangeError;

    private void OnStatusChangeError(Exception ex)
    {
      if (StatusChangeError != null)
        StatusChangeError(this, ex);
    }

    /// <summary>
    /// This event system handles the case where the service successfully moved to a new status
    /// </summary>
    public delegate void StatusChangedHandler(MySQLService sender, ServiceStatus args);

    public event StatusChangedHandler StatusChanged;

    protected virtual void OnStatusChanged(MySQLService sender, ServiceStatus args)
    {
      if (this.StatusChanged != null)
        this.StatusChanged(this, args);
    }

    public void UpdateMenu(string statusString)
    {
      SetStatus(statusString);
      MenuGroup.Update();
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

  public enum ServiceMachineType
  {
    Local,
    Remote
  }
}