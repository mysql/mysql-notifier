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

  /// <summary>
  /// TODO: Refine this class ▼
  /// </summary>
  public class RemoteMachine
  {
    private string Password;
    private string Host;
    private string User;

    public RemoteMachine(string host, string user, string password)
    {
      Host = host;
      User = user;
      Password = password;
    }

    public bool MachineID(string host, string user)
    {
      return (Host.Equals(host) && User.Equals(user));
    }

    public ManagementScope GetManagementScope()
    {
      ConnectionOptions co = new ConnectionOptions();
      co.Username = User;
      co.Password = Password;
      co.Impersonation = ImpersonationLevel.Impersonate;
      co.Authentication = AuthenticationLevel.Packet;
      co.EnablePrivileges = true;
      co.Context = new ManagementNamedValueCollection();
      co.Timeout = TimeSpan.FromSeconds(30);
      return new ManagementScope(@"\\" + Host + @"\root\cimv2", co);
    }

    public bool MachineIDMatch(string host, string user)
    {
      return Host.Equals(host) && User.Equals(user);
    }
  }
}