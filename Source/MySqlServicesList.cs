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

namespace MySql.Notifier
{
  using System;
  using System.Collections.Generic;
  using System.Text.RegularExpressions;
  using MySql.Notifier.Properties;
  using MySql.Notifier;
  using MySQL.Utility;

  public class MySQLServicesList
  {
    private bool loading;

    public List<MySQLService> Services
    {
      get { return Settings.Default.ServiceList; }
      set { Settings.Default.ServiceList = value; }
    }

    public MySQLServicesList()
    {
    }

    public void LoadFromSettings()
    {
      loading = true;

      if (Services == null)
      {
        Services = new List<MySQLService>();
      }

      if (Settings.Default.FirstRun)
      {
        AutoAddServices();
      }
      else
      {
        // We have to manually call our service list changed event handler since that isn't done
        // with how we are using settings
        for (int serviceIndex = 0; serviceIndex < Services.Count; serviceIndex++)
        {
          var service = Services[serviceIndex];

          //TODO: Check This ▼  is correct, service.managementObject should not be null!
          if (service.ServiceName != null && service.Problem != ServiceProblem.ServiceDoesNotExist)
          //if (service.ServiceName != null && Service.ExistsServiceInstance(service.ServiceName))
          {
            service.StatusChanged += new MySQLService.StatusChangedHandler(mySQLService_StatusChanged);
            OnServiceListChanged(service, ChangeListChangeType.Add);
          }
          else
          {
            Services.Remove(service);
            serviceIndex--;
          }
        }
        Settings.Default.Save();
      }

      loading = false;
    }

    private void AutoAddServices()
    {
      var services = Service.GetInstances(Settings.Default.AutoAddPattern);

      foreach (var service in Services)
        AddService(service, ChangeListChangeType.AutoAdd);

      Settings.Default.FirstRun = false;
      Settings.Default.Save();
    }

    public void AddService(MySQLService newService, ChangeListChangeType changeType)
    {
      newService.NotifyOnStatusChange = Settings.Default.NotifyOfStatusChange;
      newService.UpdateTrayIconOnStatusChange = true;
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
      OnServiceListChanged(serviceToDelete, ChangeListChangeType.Remove);
      if (!loading)
        Settings.Default.Save();
    }

    public bool Contains(MySQLService service)
    {
      if (Services == null || Services.Count == 0) return false;
      foreach (MySQLService s in Services)
      {
        if (s == service)
        {
          return true;
        }
      }
      return false;
    }

    //TODO : Deprecated?.
    public MySQLService GetServiceByName(string name)
    {
      foreach (MySQLService service in Services)
        if (String.Compare(service.ServiceName, name, true) == 0)
        {
          return service;
        }
      return null;
    }

    public MySQLService GetServiceByDisplayName(string displayName)
    {
      foreach (MySQLService service in Services)
        if (String.Compare(service.DisplayName, displayName, true) == 0)
        {
          return service;
        }
      return null;
    }

    // TODO: DEPRECATED METHODS: IMPLEMENT CORRECTLY!!
    public void SetServiceStatus(string serviceName, string path, string status)
    {
      // if we get here the service doesn't exist in the list
      // if we are not supposed to auto add then just exit
      if (!Settings.Default.AutoAddServicesToMonitor) return;

      Regex regex = new Regex(Settings.Default.AutoAddPattern, RegexOptions.IgnoreCase);
      if (regex.Match(path).Success)

        // TODO: DEPRECATED METHODS: IMPLEMENT CORRECTLY!!
        // AddService(serviceName, ChangeListChangeType.AutoAdd);
        AddService(GetServiceByName(serviceName), ChangeListChangeType.AutoAdd);
    }

    public delegate void ServiceListChangedHandler(object sender, MySQLService service, ChangeListChangeType changeType);

    public event ServiceListChangedHandler ServiceListChanged;

    protected virtual void OnServiceListChanged(MySQLService service, ChangeListChangeType changeType)
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