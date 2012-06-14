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
using MySql.Notifier.Properties;
using System.Text.RegularExpressions;
using MySQL.Utility;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;


namespace MySql.Notifier
{

  public enum ServiceListChangeType
  {
    Add,
    AutoAdd,
    Remove
  }

  public class MySQLServicesList
  {
    private bool loading;

    public List<MySQLService> Services
    {
      get { return Settings.Default.ServiceList; }
      set { Settings.Default.ServiceList = value; }
    }

    public void LoadFromSettings()
    {
      loading = true;

      if (Services == null)
        Services = new List<MySQLService>();
      if (Settings.Default.FirstRun)
        AutoAddServices();
      else
        // we have to manually call our service list changed event handler since that isn't done
        // with how we are using settings
        foreach (MySQLService service in Services)
        {
          service.StatusChanged += new MySQLService.StatusChangedHandler(mySQLService_StatusChanged);
          OnServiceListChanged(service, ServiceListChangeType.Add);
        }

      loading = false;
    }

    private void AutoAddServices()
    {
      var services = Service.GetInstances(Settings.Default.AutoAddPattern);
      foreach (var item in services)
        AddService(item.Properties["DisplayName"].Value.ToString());

      Settings.Default.FirstRun = false;
      Settings.Default.Save();
    }

    public void AddService(string serviceName)
    {
      AddService(serviceName, ServiceListChangeType.Add);
    }

    private void AddService(string serviceName, ServiceListChangeType changeType)
    {
      foreach (MySQLService service in Services)
        if (String.Compare(service.ServiceName, serviceName, true) == 0) return;
      
      // for now all services will have same value as the global setting for the notifications
      MySQLService newService = new MySQLService(serviceName, Settings.Default.NotifyOfStatusChange, updatesTrayIcon: true);
      newService.StatusChanged += new MySQLService.StatusChangedHandler(mySQLService_StatusChanged);
      Services.Add(newService);

      OnServiceListChanged(newService, changeType);
      if (!loading)
        Properties.Settings.Default.Save();
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
        Settings.Default.Save();
    }

    public bool Contains(string name)
    {
      return GetServiceByName(name) != null;
    }

    public MySQLService GetServiceByName(string name)
    {
      foreach (MySQLService service in Services)
        if (String.Compare(service.ServiceName, name, true) == 0) return service;
      return null;
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
  }

}
