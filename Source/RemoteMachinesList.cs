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
  using System.Collections.Generic;
  using System.Management;

  /// <summary>
  /// TODO: Refine this class ▼
  /// </summary>
  public static class RemoteMachinesList
  {
    private static List<RemoteMachine> remoteMachines = new List<RemoteMachine>();

    /// <summary>
    /// GetManagement
    /// </summary>
    /// <returns></returns>
    public static List<ManagementScope> GetManagementScopes()
    {
      List<ManagementScope> managementScopes = new List<ManagementScope>();
      managementScopes.Add(new ManagementScope(@"root\cimv2"));
      foreach (MySQLService service in (new MySQLServicesList()).Services)
      {
        if (service.WinServiceType == ServiceType.Remote)
        {
          if (IsRemoteMachineRegistered(service.Host, service.User))
          {
            remoteMachines.Add(new RemoteMachine(service.Host, service.User, MySQLSecurity.DecryptPassword(service.Password)));
            managementScopes.Add(new RemoteMachine(service.Host, service.User, MySQLSecurity.DecryptPassword(service.Password)).GetManagementScope());
          }
        }
      }
      return managementScopes;
    }

    public static bool IsRemoteMachineRegistered(string host, string user)
    {
      foreach (RemoteMachine r in remoteMachines)
      {
        if (r.MachineIDMatch(host, user))
          return true;
      }
      return false;
    }
  }
}