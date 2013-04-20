//
// Copyright (c) 2013, Oracle and/or its affiliates. All rights reserved.
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
  using System.Management;
  using System.Runtime.InteropServices;
  using System.Text.RegularExpressions;
  using System.Xml.Serialization;
  using Microsoft.Win32;
  using MySql.Notifier.Properties;
  using MySQL.Utility;

  /// <summary>
  /// TODO: Refine this class ▼
  /// </summary>
  [Serializable]
  public class Machine
  {
    [XmlAttribute("Host")]
    public string Name { get; set; }

    [XmlAttribute("User")]
    public string User { get; set; }

    [XmlAttribute("Password")]
    public string Password { get; set; }

    [XmlElement(ElementName = "ServicesList", Type = typeof(List<MySQLService>))]
    public List<MySQLService> Services { get; set; }

    [XmlIgnore]
    public string UnprotectedPassword
    {
      get { return String.IsNullOrEmpty(Password) ? String.Empty : MySQLSecurity.DecryptPassword(Password); }
    }

    [XmlIgnore]
    public bool IsOnline
    {
      get { return CanConnect(); }
    }

    /// <summary>
    /// Default constructor. DO NOT REMOVE. Required for serialization-deserialization.
    /// </summary>
    public Machine()
    {
    }

    /// <summary>
    /// Constructor designed for local machines
    /// </summary>
    /// <param name="name"></param>
    public Machine(string name)
    {
      Name = (name == "localhost") ? name : name.ToUpper();
      User = String.Empty;
      Password = String.Empty;
      Services = new List<MySQLService>();
    }

    /// <summary>
    /// Constructor designed for remote machines
    /// </summary>
    /// <param name="name"></param>
    /// <param name="user"></param>
    /// <param name="password"></param>
    public Machine(string name, string user, string password)
    {
      Name = (name == "localhost") ? name : name.ToUpper();
      User = user.ToUpper();
      Password = MySQLSecurity.EncryptPassword(password);
      Services = new List<MySQLService>();
    }

    /// <summary>
    /// Returns true or false if the application is able to connect to the machine.
    /// </summary>
    /// <returns></returns>
    public bool CanConnect()
    {
      return (TestConnectionManaged() == ServiceProblem.None);
    }

    public bool MachineIDMatch(string host, string user)
    {
      return (String.Compare(Name, host, true) == 0 && String.Compare(User, user, true) == 0);
    }

    /// <summary>
    /// This method will handle the cases where you want test connection and have all possible exceptions handled automatically.
    /// </summary>
    /// <param name="serviceName">*Optional* Name of an specific service to verify its existance or null to test connection only.</param>
    /// <returns>One of the following service problems: None, ServiceDoesNotExist, LackOfRemotePermissions, IncorrectUserOrPassword, Other </returns>
    public ServiceProblem TestConnectionManaged(string serviceName = null)
    {
      ServiceProblem result;
      try
      {
        result = TestConnectionUnManaged(serviceName) ? ServiceProblem.None : ServiceProblem.ServiceDoesNotExist;
      }
      catch (COMException)
      {
        result = ServiceProblem.LackOfRemotePermissions;
      }
      catch (UnauthorizedAccessException)
      {
        result = ServiceProblem.IncorrectUserOrPassword;
      }
      catch (ManagementException)
      {
        result = ServiceProblem.Other;
      }
      return result;
    }

    /// <summary>
    /// This method will handle the cases where you want test connection and handle possible thrown exceptions and exception messages manually.
    /// </summary>
    /// <param name="serviceName">*Optional* Name of an specific service to verify its existance or null to test connection only.</param>
    /// <returns>True when connection was successfull and service(s) exist.</returns>
    /// <returns>False when connection was successfull but the service no longer exists.</returns>
    /// <returns>COMException when connection failed due to a lack of remote permissions.</returns>
    /// <returns>UnauthorizedAccessException when connection failed due to incorrect user or/and password.</returns>
    /// <returns>ManagementException when connection failed due to an unknown problem.</returns>
    public bool TestConnectionUnManaged(string serviceName = null)
    {
      try
      {
        ManagementObjectCollection moc = GetServices();
        return (moc.Count > 0);
      }
      catch
      {
        throw;
      }
    }

    /// <summary>
    ///  Gets a list of matching services for a given filter
    /// </summary>
    /// <param name="serviceName"></param>
    /// <returns></returns>
    public ManagementObjectCollection GetServices(string serviceName = null)
    {
      try
      {
        ManagementScope managementScope = GetManagementScope();
        managementScope.Connect();
        WqlObjectQuery query = string.IsNullOrEmpty(serviceName) ? new WqlObjectQuery("Select * From Win32_Service")
        : new WqlObjectQuery(String.Format("Select * From Win32_Service Where Name = \"{0}\"", serviceName));
        ManagementObjectSearcher searcher = new ManagementObjectSearcher(managementScope, query);
        return searcher.Get();
      }
      catch
      {
        throw;
      }
    }

    /// <summary>
    /// Get ManagementScope
    /// </summary>
    /// <returns></returns>
    public ManagementScope GetManagementScope()
    {
      if (Name == "localhost") return new ManagementScope(@"root\cimv2");

      ConnectionOptions co = new ConnectionOptions();
      co.Username = User;
      co.Password = MySQLSecurity.DecryptPassword(Password);
      co.Impersonation = ImpersonationLevel.Impersonate;
      co.Authentication = AuthenticationLevel.Packet;
      co.EnablePrivileges = true;
      co.Context = new ManagementNamedValueCollection();
      co.Timeout = TimeSpan.FromSeconds(30);
      return new ManagementScope(@"\\" + Name + @"\root\cimv2", co);
    }

    private MySQLStartupParameters GetStartupParameters(MySQLService mysqlService)
    {
      MySQLStartupParameters parameters = new MySQLStartupParameters();
      parameters.PipeName = "mysql";

      //// Get our host information
      parameters.HostName = (mysqlService.WinServiceType == ServiceMachineType.Local) ? "localhost" : Name;
      parameters.HostIPv4 = Utility.GetIPv4ForHostName(parameters.HostName);

      if (mysqlService.WinServiceType != ServiceMachineType.Local) return parameters;
      RegistryKey key = Registry.LocalMachine.OpenSubKey(String.Format(@"SYSTEM\CurrentControlSet\Services\{0}", mysqlService.ServiceName));
      if (key == null) return parameters;

      string imagepath = (string)key.GetValue("ImagePath", null);
      key.Close();

      if (imagepath == null) return parameters;

      string[] args = Utility.SplitArgs(imagepath);
      mysqlService.SeeIfRealMySQLService(args[0]);

      //// Parse our command line args
      Mono.Options.OptionSet p = new Mono.Options.OptionSet()
        .Add("defaults-file=", "", v => parameters.DefaultsFile = v)
        .Add("port=|P=", "", v => Int32.TryParse(v, out parameters.Port))
        .Add("enable-named-pipe", v => parameters.NamedPipesEnabled = true)
        .Add("socket=", "", v => parameters.PipeName = v);
      p.Parse(args);
      if (parameters.DefaultsFile == null) return parameters;

      //// We have a valid defaults file
      IniFile f = new IniFile(parameters.DefaultsFile);
      Int32.TryParse(f.ReadValue("mysqld", "port", parameters.Port.ToString()), out parameters.Port);
      parameters.PipeName = f.ReadValue("mysqld", "socket", parameters.PipeName);

      //// Now see if named pipes are enabled
      parameters.NamedPipesEnabled = parameters.NamedPipesEnabled || f.HasKey("mysqld", "enable-named-pipe");

      return parameters;
    }

    #region Services Management

    public void ChangeService(ChangeListChangeType changeType, MySQLService service)
    {
      //TODO: ▼ Check all this is correct
      switch (changeType)
      {
        case ChangeListChangeType.Add:
          AddServiceIfNotExist(service);
          break;

        case ChangeListChangeType.AutoAdd:
          var services = Service.GetInstances(Settings.Default.AutoAddPattern);

          foreach (var newService in Services)
            AddService(newService, ChangeListChangeType.AutoAdd);

          Settings.Default.FirstRun = false;
          Settings.Default.Save();
          break;

        case ChangeListChangeType.Remove:

          //TODO: Change definition and remove by object, no by name:
          //RemoveService(string serviceName)
          break;
      }
      LoadServiceParameters();
    }

    private void AddServiceIfNotExist(MySQLService service)
    {
      if (GetServiceByName(service.ServiceName) == null)
        AddService(service, ChangeListChangeType.Add);
    }

    private void AddService(MySQLService newService, ChangeListChangeType changeType)
    {
      newService.NotifyOnStatusChange = Settings.Default.NotifyOfStatusChange;
      newService.UpdateTrayIconOnStatusChange = true;
      newService.StatusChanged += new MySQLService.StatusChangedHandler(mySQLService_StatusChanged);
      Services.Add(newService);

      OnServiceListChanged(newService, changeType);
      Properties.Settings.Default.Save();
    }

    private void RemoveService(string serviceName)
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
      Settings.Default.Save();
    }

    public bool ContainsService(MySQLService service)
    {
      if (Services == null || Services.Count == 0) return false;
      {
        return GetService(service) == null ? false : true;
      }
    }

    public MySQLService GetService(MySQLService service)
    {
      foreach (MySQLService s in Services)
      {
        if (String.Compare(service.ServiceName, s.ServiceName, true) == 0)
        {
          return service;
        }
      }
      return null;
    }

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

    // TODO: Check spelling ▼
    /// <summary>
    /// Load Calculated, Machine dependant parameters
    /// </summary>
    internal void LoadServiceParameters()
    {
      if (Services == null)
        Services = new List<MySQLService>();
      if (Settings.Default.FirstRun)
        AutoAddServices();
      else
      {
        // we have to manually call our service list changed event handler since that isn't done
        // with how we are using settings
        foreach (MySQLService service in Services)
        {
          service.Host = this;
          service.SetService();

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
          }
        }
        Settings.Default.Save();
      }
    }

    private void AutoAddServices()
    {
      //TODO: ▼ Verify local services(or continue operation from AutoLoadMachines() At MachinesList).
      //var services = Service.GetInstances(Settings.Default.AutoAddPattern);

      //foreach (var service in Services)
      //  AddService(service, ChangeListChangeType.AutoAdd);

      //Settings.Default.FirstRun = false;
      //Settings.Default.Save();
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

    #endregion Services Management
  }
}