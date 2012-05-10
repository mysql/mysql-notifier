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
using MySQL.Utility;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace MySql.TrayApp
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

    public MySQLServicesList()
    {
      Services = new List<MySQLService>();
    }

    public List<MySQLService> Services { get; private set; }    

    public void LoadFromSettings()
    {
      if (Settings.Default.FirstRun)
      {
        LoadFirstRun();
        return;
      }

      if (Properties.Settings.Default.ServiceSettingsList == null) return;
      
      loading = true;
      //load saved settings for monitored services
      LoadServicesFromSettings();

      loading = false;
      
    }

    private void LoadFirstRun()
    {
      Settings.Default.FirstRun = false;
      Settings.Default.Save();

      var services = Service.GetInstances(Settings.Default.AutoAddPattern);
      foreach (var item in services)
        AddService(item.Properties["DisplayName"].Value.ToString(), true);
    }

    public void AddService(string serviceName, bool notifyOnStateChange)
    {
      AddService(serviceName, notifyOnStateChange, ServiceListChangeType.Add);
    }

    private void AddService(string serviceName, bool notifyOnStateChange, ServiceListChangeType changeType)
    {
      foreach (MySQLService service in Services)
        if (String.Compare(service.ServiceName, serviceName, true) == 0) return;
      
      // for now all services will have same value as the global setting for the notifications
      MySQLService newService = new MySQLService(serviceName, Settings.Default.NotifyOfStatusChange);
      newService.StatusChanged += new MySQLService.StatusChangedHandler(mySQLService_StatusChanged);
      Services.Add(newService);

      OnServiceListChanged(newService, changeType);
      if (!loading)
      {                
        SaveServicesOnSettings();        
        Properties.Settings.Default.Save();
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
        SaveServicesOnSettings();
        Settings.Default.Save();
      }
    }

    public bool Contains(string name)
    {
      foreach (MySQLService service in Services)
        if (String.Compare(service.ServiceName, name, true) == 0) return true;
      return false;
    }

    public void SetServiceStatus(string serviceName, bool notifyOnStateChange, string path, string status)
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
        AddService(serviceName, notifyOnStateChange, ServiceListChangeType.AutoAdd);
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


    public void SaveServicesOnSettings()
    {
      List<MySqlServiceSettings> serviceSettingsList = new List<MySqlServiceSettings>();

      foreach (var item in Services)
      {
        var serviceSettings = new MySqlServiceSettings { Name = item.Name, NotifyOnStateChange = item.notifyChangesEnabled };
        serviceSettingsList.Add(serviceSettings);
      }

      using (MemoryStream ms = new MemoryStream())
      {
        using (StreamReader sr = new StreamReader(ms))
        {
          BinaryFormatter serviceBinaryFormatter = new BinaryFormatter();
          serviceBinaryFormatter.Serialize(ms, serviceSettingsList);
          ms.Position = 0;
          byte[] buffer = new byte[(int)ms.Length];
          ms.Read(buffer, 0, buffer.Length);
          Properties.Settings.Default.ServiceSettingsList = Convert.ToBase64String(buffer);
        }
      }
    }

    public void LoadServicesFromSettings()
    {
     
      if (String.IsNullOrEmpty(Properties.Settings.Default.ServiceSettingsList)) return;

      List<MySqlServiceSettings> serviceSettingsList = new List<MySqlServiceSettings>();

      using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(Properties.Settings.Default.ServiceSettingsList)))
      {
        BinaryFormatter serviceBinaryFormatter = new BinaryFormatter();
        serviceSettingsList = (List<MySqlServiceSettings>)serviceBinaryFormatter.Deserialize(ms);
      }
      
      foreach (var item in serviceSettingsList)
      {        
        AddService(item.Name, item.NotifyOnStateChange);       
      }      
    }

  }

}
