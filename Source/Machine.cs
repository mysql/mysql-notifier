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
  using System.Management;
  using System.Xml.Serialization;
  using System.Runtime.InteropServices;

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

    [XmlIgnore]
    public string UnprotectedPassword
    {
      get { return MySQLSecurity.DecryptPassword(Password); }
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

    public Machine(string name, string user, string password)
    {
      Name = name;
      User = user;
      Password = MySQLSecurity.EncryptPassword(password);
    }

    public bool CanConnect()
    {
      return (TestConnectionManaged() == ServiceProblem.None);
    }

    public ManagementScope GetManagementScope()
    {
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
    /// This method will handle the cases where you want test connection and have all possible exceptions handled automatically.
    /// </summary>
    /// <param name="serviceName">*Optional* Name of an specific service to verify its existance or null to test connection only.</param>
    /// <returns></returns>
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

    //TODO: Dispose when no longer needed.
    public bool MachineIDMatch(string host, string user)
    {
      return Name.Equals(host) && User.Equals(user);
    }
  }
}