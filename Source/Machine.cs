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
  using System.Xml.Serialization;
  using MySql.Notifier.Properties;

  /// <summary>
  /// The machine class contains all the information required to connect to a remote host, plus a list of of the services that will be monitored by Notifier inside it and logic to manage them.
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
    public Status Status
    {
      get { return IsOnline ? Status.Online : Status.Unavailable; }
    }

    [XmlIgnore]
    public bool IsOnline
    {
      get { return CanConnect(); }
    }

    /// <summary>
    /// DO NOT REMOVE. Default constructor required for serialization-deserialization.
    /// </summary>
    public Machine()
    {
    }

    /// <summary>
    /// Constructor designed for local machines
    /// </summary>
    /// <param name="name">localhost</param>
    public Machine(string name)
    {
      Name = "localhost";
      User = String.Empty;
      Password = String.Empty;
      Services = new List<MySQLService>();
    }

    /// <summary>
    /// Constructor designed for remote machines
    /// </summary>
    /// <param name="name">Host name</param>
    /// <param name="user">User name</param>
    /// <param name="password">Password</param>
    public Machine(string name, string user, string password)
    {
      Name = (name == "localhost") ? name : name.ToUpper();
      User = user.ToUpper();
      Password = MySQLSecurity.EncryptPassword(password);
      Services = new List<MySQLService>();
    }

    public override string ToString()
    {
      return Name;
    }

    /// <summary>
    /// Returns true or false if the application is able to connect to the machine.
    /// </summary>
    /// <returns></returns>
    private bool CanConnect()
    {
      return (TestConnectionManaged() == ServiceProblem.None);
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

    /// <summary>
    /// Load Calculated, Machine dependant parameters
    /// </summary>
    internal void LoadServiceParameters()
    {
      if (Services == null)
        Services = new List<MySQLService>();

      // We have to manually call our service list changed event handler since that isn't done with how we are using settings
      foreach (MySQLService service in Services)
      {
        service.Host = this;
        service.SetServiceParameters();

        if (service.ServiceName != null && service.Problem != ServiceProblem.ServiceDoesNotExist)
        {
          service.StatusChanged += new MySQLService.StatusChangedHandler(OnServiceStatusChanged);
          service.StatusChangeError += new MySQLService.StatusChangeErrorHandler(OnMachineConnectionError);
          OnServiceListChanged(service, ChangeType.Add);
        }
        else
        {
          Services.Remove(service);
        }
      }
    }

    /// <summary>
    /// Add or Delete a Service for current machine
    /// </summary>
    /// <param name="service">MySQLService instance</param>
    /// <param name="changeType">Add/Delete</param>
    public void ChangeService(MySQLService service, ChangeType changeType)
    {
      switch (changeType)
      {
        case ChangeType.Add:
          if (GetServiceByName(service.ServiceName) == null)
            AddService(service, ChangeType.Add);
          break;

        case ChangeType.AutoAdd:
          AddService(service, ChangeType.AutoAdd);
          return;

        case ChangeType.Remove:
          RemoveService(service);
          break;
      }
      LoadServiceParameters();
    }

    /// <summary>
    /// Adds a service on the current machine.
    /// </summary>
    /// <param name="newService">MySQLService instance</param>
    /// <param name="changeType">Add/AutoAdd</param>
    private void AddService(MySQLService newService, ChangeType changeType)
    {
      newService.NotifyOnStatusChange = Settings.Default.NotifyOfStatusChange;
      newService.UpdateTrayIconOnStatusChange = true;
      Services.Add(newService);

      OnServiceListChanged(newService, changeType);
      Properties.Settings.Default.Save();
    }

    /// <summary>
    /// Removes a service on the current machine, if its the only service monitored on it also triggers machine deletion from the list.
    /// </summary>
    /// <param name="service">MySQLService instance</param>
    private void RemoveService(MySQLService service)
    {
      if (GetServiceByName(service.ServiceName) != null)
        Services.Remove(service);
      Settings.Default.Save();
      OnServiceListChanged(service, ChangeType.Remove);
    }

    /// <summary>
    /// Used to see if service is already on the list
    /// </summary>
    /// <param name="service">MySQLService instance to look for</param>
    /// <returns>True if current machine contains it already</returns>
    public bool ContainsService(MySQLService service)
    {
      if (Services == null || Services.Count == 0) return false;
      {
        return GetServiceByName(service.ServiceName) == null ? false : true;
      }
    }

    /// <summary>
    /// Returns an instance of a service if is already on the list, searching by name
    /// </summary>
    /// <param name="name">MySQLService instance name</param>
    /// <returns>MySQLService instance</returns>
    public MySQLService GetServiceByName(string name)
    {
      foreach (MySQLService service in Services)
        if (String.Compare(service.ServiceName, name, true) == 0)
        {
          return service;
        }
      return null;
    }

    // TODO ▼ Dispose of this method and attempt to use GetServiceByName instead.
    public MySQLService GetServiceByDisplayName(string displayName)
    {
      foreach (MySQLService service in Services)
        if (String.Compare(service.DisplayName, displayName, true) == 0)
        {
          return service;
        }
      return null;
    }

    internal void SetServiceStatus(MySQLService service, string state)
    {
      if (service != null)
      {
        service.SetStatus(state);
        service.MenuGroup.Update();
        return;
      }
    }

    /// <summary>
    /// Notifies that the status of one of the services in the list has changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void ServiceStatusChangedHandler(Machine machine, MySQLService sender, ServiceStatus args);

    public event ServiceStatusChangedHandler ServiceStatusChanged;

    protected virtual void OnServiceStatusChanged(MySQLService service, ServiceStatus args)
    {
      if (ServiceStatusChanged != null)
        ServiceStatusChanged(this, service, args);
    }

    /// <summary>
    /// This event system handles the case where the remote machine is unavailable, and a service has failed to connect to the host.
    /// </summary>
    public delegate void MachineConnectionErrorHandler(Machine sender, MySQLService service, Exception ex);

    public event MachineConnectionErrorHandler MachineConnectionError;

    private void OnMachineConnectionError(MySQLService service, Exception ex)
    {
      if (MachineConnectionError != null)
        MachineConnectionError(this, service, ex);
    }

    /// <summary>
    /// Event handler for changes on current machine services list
    /// </summary>
    /// <param name="sender">Machine instance</param>
    /// <param name="service">MySQLService instance</param>
    /// <param name="changeType">ChangeType</param>
    public delegate void ServiceListChangedHandler(Machine sender, MySQLService service, ChangeType changeType);

    public event ServiceListChangedHandler ServiceListChanged;

    protected virtual void OnServiceListChanged(MySQLService service, ChangeType changeType)
    {
      if (ServiceListChanged != null)
        ServiceListChanged(this, service, changeType);
    }

    internal void OverwriteCredentials(string user, string password)
    {
      User = user;
      Password = MySQLSecurity.EncryptPassword(password);
    }
  }

  public enum Status
  {
    Unavailable,
    Online
  }
}