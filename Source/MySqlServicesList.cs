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
using MySql.TrayApp.Properties;
using System.Text.RegularExpressions;


namespace MySql.TrayApp
{ 
  public class MySQLServicesList
  {
    private bool loading;
    public bool HasAdminPrivileges { get; private set; }

    public MySQLServicesList(bool adminPrivileges)
    {
      HasAdminPrivileges = adminPrivileges;
      Services = new List<MySQLService>();
    }

    public List<MySQLService> Services { get; private set; }

    public void LoadFromSettings()
    {
      if (Settings.Default.ServicesMonitor == null) return;

      loading = true;
      foreach (string serviceName in Settings.Default.ServicesMonitor)
        AddService(serviceName);
      loading = false;
    }

    public void AddService(string serviceName)
    {
      AddService(serviceName, ServiceListChangeType.Add);
    }

    private void AddService(string serviceName, ServiceListChangeType changeType)
    {
      foreach (MySQLService service in Services)
        if (String.Compare(service.ServiceName, serviceName, true) == 0) return;

      MySQLService newService = new MySQLService(serviceName, HasAdminPrivileges);
      newService.StatusChanged += new MySQLService.StatusChangedHandler(mySQLService_StatusChanged);
      Services.Add(newService);
      OnServiceListChanged(newService, changeType);
      if (!loading)
      {
        Settings.Default.ServicesMonitor.Add(serviceName);
        Settings.Default.Save();
      }
    }

    public void RemoveService(string serviceName)
    {
      MySQLService serviceToDelete = null;

      foreach (MySQLService service in Services)
      {
        if (String.Compare(service.ServiceName, serviceName, true) != 0) continue;
        serviceToDelete = service;
        break;
      }
      if (serviceToDelete == null) return;

      Services.Remove(serviceToDelete);
      OnServiceListChanged(serviceToDelete, ServiceListChangeType.Remove);
      if (!loading)
      {
        Settings.Default.ServicesMonitor.Remove(serviceName);
        Settings.Default.Save();
      }
    }

    public bool Contains(string name)
    {
      foreach (MySQLService service in Services)
        if (String.Compare(service.ServiceName, name, true) == 0) return true;
      return false;
    }

    public void SetServiceStatus(string serviceName, string path, string status)
    {
      foreach (MySQLService service in Services)
      {
        if (String.Compare(service.ServiceName, serviceName, true) != 0) continue;
        service.SetStatus(status);
        return;
      }
      // if we get here the service doesn't exist in the list
      // if we are not supposed to auto add then just exit
      if (!Settings.Default.AutoAddServicesToMonitor) return;

      Regex regex = new Regex(Settings.Default.AutoAddPattern, RegexOptions.IgnoreCase);
      if (regex.Match(path).Success)
        AddService(serviceName, ServiceListChangeType.AutoAdd);
    }

    /// <summary>
    /// Calls the Dispose method on the passed MySQLService object
    /// </summary>
    /// <param name="service"></param>
    private void DisposeService(MySQLService service)
    {
      //service.Dispose();
    }

    public delegate void ServiceListChangedHandler(object sender, MySQLService service, ServiceListChangeType changeType);
    public event ServiceListChangedHandler ServiceListChanged;

    protected virtual void OnServiceListChanged(MySQLService service, ServiceListChangeType changeType)
    {
      if (ServiceListChanged != null)
        ServiceListChanged(this, service, changeType);
    }

    public delegate void ServiceStatusChangedHandler(object sender, ServiceStatus args);

    /// <summary>
    /// Notifies that the status of one of the services in the list has changed
    /// </summary>
    public event ServiceStatusChangedHandler ServiceStatusChanged;

    protected virtual void OnServiceStatusChanged(ServiceStatus args)
    {
      if (ServiceStatusChanged != null)
        ServiceStatusChanged(this, args);
    }

    private void mySQLService_StatusChanged(object sender, ServiceStatus args)
    {
        OnServiceStatusChanged(args);
    }


    ///// <summary>
    ///// Refreshes the list of installed MySQL Services only adding or removing items if necessary
    ///// </summary>
    ///// <param name="monitoredServices"> Has the list of services the app is monitoring </param>
    ///// <param name="autoAddServices">If true then add new services</param>
    ///// <returns>Flag indicating if the services changed</returns>
    //public bool RefreshMySQLServices(ref List<string> monitoredServices, bool autoAddServices)
    //{      
    //  var mySqlServices = MySqlServiceInformation.GetInstances(null);
      
    //  try 
    //  { 
    //      //if (mySqlServices.Count <= 0)
    //      //{
    //      //  throw new Exception(Properties.Resources.NoServicesExceptionMessage);        
    //      //}

    //      var mysqlServicesNames = mySqlServices.Select(t => t.Properties["Name"].Value).Cast<String>().ToList();
    //      var mysqlMonitoringNames = Services.Select(t => t.ServiceName).Cast<String>().ToList();

    //      if (mysqlServicesNames.Except(monitoredServices).Any() || monitoredServices.Except(mysqlMonitoringNames).Any())
    //      {
            
    //       // Checked for removed services
    //        var copyOf = Services.ToArray();

    //        foreach (MySQLService mservice in copyOf)
    //        {
    //            if (!Services.Exists(delegate(String service) { return service == mservice.ServiceName; }))
    //            {
    //              if (Services.Where(t => t.ServiceName == mservice.ServiceName).Count() > 0)
    //              {
    //                //monitoredMySQLServicesList.Where(t => t.ServiceName == mservice.ServiceName).First().Dispose();
    //                Services.Remove(Services.Where(t => t.ServiceName == mservice.ServiceName).First());                    
    //              }
                  
    //            }
    //        }
  
    //        var copyOfMS = monitoredServices.ToArray();
    //          // Check for uninstalled services to remove them from the global list
    //        foreach (string mService in copyOfMS)
    //          {
    //            if (!mySqlServices.Exists(delegate(ManagementObject service) { return service.Properties["Name"].Value.ToString() == mService; }))
    //            {
                
    //              //if (monitoredMySQLServicesList.Remove(monitoredMySQLServicesList.Where(t => t.ServiceName == mService).First()))
    //                //monitoredMySQLServicesList.Where(t => t.ServiceName == mService).First().Dispose();

    //              monitoredServices.Remove(mService);

    //            }
    //          }

    //          //Check if new mysql services exists that are not monitored yet
    //          foreach (ManagementObject wService in mySqlServices)
    //          {
    //            if (!monitoredServices.Exists(delegate(String service) { return service == wService.Properties["Name"].Value.ToString(); }) && autoAddServices)
    //            {
    //              var mySQLService = new MySQLService(wService.Properties["Name"].Value.ToString(), HasAdminPrivileges);
    //              mySQLService.StatusChanged += mySQLService_StatusChanged;
    //              Services.Add(mySQLService);
    //              monitoredServices.Add(wService.Properties["Name"].Value.ToString());
    //            }
    //          }

    //          //updated new services in mysqlservices global list for UI
    //          foreach (string mservice in monitoredServices)
    //          {
    //            // if its missing then add it otherwise remove it
    //            if (!Services.Exists(delegate(MySQLService service) { return service.ServiceName == mservice; }))
    //            {
    //              var mySQLService = new MySQLService(mservice, HasAdminPrivileges);
    //              mySQLService.StatusChanged += mySQLService_StatusChanged;
    //              Services.Add(mySQLService);
    //            }
    //          }

    //        Services.Sort(delegate(MySQLService serv1, MySQLService serv2) { return serv1.ServiceName.CompareTo(serv2.ServiceName); });
    //        return true;
    //      }         
    //   }
    //   catch (System.ComponentModel.Win32Exception win32ex)
    //   {
    //     MessageBox.Show(win32ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //   }
    //   catch (Exception ex)
    //   {
    //     MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //   }

    //   return false;     
    //}

  }

  public enum ServiceListChangeType
  {
    Add,
    AutoAdd,
    Remove
  }
}
