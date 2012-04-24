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
using System.Collections;
using System.ServiceProcess;
using System.Windows.Forms;
using System.Linq;
using System.Globalization;
using System.Management;
using System.ComponentModel;
using MySQL.Utility;
using System.Diagnostics;
using Microsoft.Win32;
using System.Net;

namespace MySql.TrayApp
{
  public class MySQLService
  {
    private ServiceController winService;


    public ServiceControllerStatus Status { get; private set; }
    public string Name { get; private set; }

    public string ServiceName
    {
      get { return winService.DisplayName; }
    }

    public ServiceMenuGroup MenuGroup { get; private set; }

    public MySQLService(string serviceName)
    {
      winService = new ServiceController(serviceName);
      Name = winService.ServiceName;
      try
      {
        Status = winService.Status;
        FindMatchingWBConnections();
        MenuGroup = new ServiceMenuGroup(this);
      }
      catch (InvalidOperationException ioEx)
      {
        MessageBox.Show(ioEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        winService = null;
      }
    }

    public List<MySqlWorkbenchConnection> WorkbenchConnections { get; private set; }

    private void FindMatchingWBConnections()
    {
      // first we discover what parameters we were started with
      MySQLStartupParameters parameters = GetStartupParameters();

      WorkbenchConnections = new List<MySqlWorkbenchConnection>();
      foreach (MySqlWorkbenchConnection c in MySqlWorkbench.GetConnections())
      {
        if (c.Host != parameters.HostIPv4) continue;
        if (c.IsNamedPipe && (!parameters.NamedPipesEnabled || String.Compare(c.Socket, parameters.PipeName, true) != 0)) continue;
        if (c.IsSocket && c.Port != parameters.Port) continue;
        WorkbenchConnections.Add(c);
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

    public void SetStatus(string statusString)
    {
      ServiceControllerStatus newStatus;
      bool parsed = Enum.TryParse(statusString, out newStatus);
      if (parsed)
      {
        if (newStatus == Status) return;
        ServiceControllerStatus copyPreviousStatus = Status;
        Status = newStatus;
        MenuGroup.Update();
        OnStatusChanged(new ServiceStatus(ServiceName, copyPreviousStatus, Status));
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
        SetStatus(winService.Status.ToString());
      }
    }

    void worker_DoWork(object sender, DoWorkEventArgs e)
    {
      TimeSpan timeout = TimeSpan.FromMilliseconds(5000);
      BackgroundWorker worker = sender as BackgroundWorker;
      int action = (int)e.Argument;

      if (action == 1)
      {
        StartOrStopService("start");
      }
      else if (action == 0)
      {
        StartOrStopService("stop");
      }
    }

    private void StartOrStopService(string action)
    {
      Process proc = new Process();
      proc.StartInfo.Verb = "runas";
      proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
      proc.StartInfo.FileName = "sc";
      proc.StartInfo.Arguments = string.Format(@" {0} {1}", action, Name);
      proc.StartInfo.UseShellExecute = true;
      proc.Start();
      winService.WaitForStatus(action == "start" ? ServiceControllerStatus.Running : ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 30));
    }

    private MySQLStartupParameters GetStartupParameters()
    {
      MySQLStartupParameters parameters = new MySQLStartupParameters();
      parameters.PipeName = "mysql";

      // get our host information
      parameters.HostName = winService.MachineName;
      parameters.HostIPv4 = Utility.GetIPv4ForHostName(parameters.HostName);

      RegistryKey key = Registry.LocalMachine.OpenSubKey(String.Format(@"SYSTEM\CurrentControlSet\Services\{0}", winService.ServiceName));
      string imagepath = (string)key.GetValue("ImagePath", null);
      key.Close();
      if (imagepath == null) return parameters;

      string[] args = Utility.SplitArgs(imagepath);

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
