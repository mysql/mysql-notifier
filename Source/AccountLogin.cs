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

  /// <summary>
  /// Credentials for remote server connection
  /// </summary>
  public class AccountLogin
  {
    //TODO: Find out how to use SecureString if possible
    private string password;

    public ServiceType ServiceType { get; set; }

    public string Host { get; set; }

    public int? Port { get; set; }

    public string User { get; set; }

    public string Password
    {
      get { return password; }
      set { password = String.IsNullOrEmpty(value) ? String.Empty : MySQLSecurity.EncryptPassword(value); }
    }

    public string DecryptedPassword
    {
      get { return String.IsNullOrEmpty(password) ? String.Empty : MySQLSecurity.DecryptPassword(password); }
    }

    /// <summary>
    /// Default constructor, used to add local machine services.
    /// </summary>
    public AccountLogin()
    {
      this.ServiceType = ServiceType.Local;
      this.Host = "localhost";
      this.User = Environment.UserName;
    }

    /// <summary>
    /// This constructor is designed for remote Windows accounts.
    /// </summary>
    /// <param name="HostName">Service's Host name or IP address</param>
    /// <param name="UserName">Admin user of remote machine</param>
    /// <param name="Password">Account's password</param>
    public AccountLogin(string HostName, string UserName, string Password)
    {
      this.ServiceType = ServiceType.RemoteWindows;
      this.Host = HostName;
      this.User = UserName;
      this.Password = Password;
    }

    /// <summary>
    /// This constructor is designed for remote Non-Windows accounts.
    /// </summary>
    /// <param name="Host">Service's Host name or IP address</param>
    /// <param name="Port">Remote MySQL server port</param>
    /// <param name="UserName">User account from remote server</param>
    /// <param name="Password">Account's password</param>
    public AccountLogin(string HostIP, int Port, string UserName, string Password)
    {
      this.ServiceType = ServiceType.RemoteNonWindows;
      this.Host = HostIP;
      this.Port = Port;
      this.User = UserName;
      this.Password = Password;
    }
  }
}