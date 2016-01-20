// Copyright (c) 2016, Oracle and/or its affiliates. All rights reserved.
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

using System;
using MySQL.Utility.Classes.MySQLWorkbench;

namespace MySql.Notifier.Classes.EventArguments
{
  /// <summary>
  /// Provides information for the InstanceStatusChanged event.
  /// </summary>
  public class InstanceStatusChangedArgs : EventArgs
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="InstanceStatusChangedArgs"/> class.
    /// </summary>
    /// <param name="instance">MySQL instance whose status changed.</param>
    /// <param name="oldInstanceStatus">Old instance status.</param>
    public InstanceStatusChangedArgs(MySqlInstance instance, MySqlWorkbenchConnection.ConnectionStatusType oldInstanceStatus)
    {
      Instance = instance;
      OldInstanceStatus = oldInstanceStatus;
    }

    /// <summary>
    /// Gets the MySQL instance whose status changed.
    /// </summary>
    public MySqlInstance Instance { get; private set; }

    /// <summary>
    /// Gets the new status of the instance.
    /// </summary>
    public MySqlWorkbenchConnection.ConnectionStatusType NewInstanceStatus
    {
      get
      {
        return Instance.ConnectionStatus;
      }
    }

    /// <summary>
    /// Gets a description on the new status of this connection.
    /// </summary>
    public string NewInstanceStatusText
    {
      get
      {
        return Instance.ConnectionStatusText;
      }
    }

    /// <summary>
    /// Gets the old status of the instance.
    /// </summary>
    public MySqlWorkbenchConnection.ConnectionStatusType OldInstanceStatus { get; private set; }

    /// <summary>
    /// Gets a description on the old status of this connection.
    /// </summary>
    public string OldInstanceStatusText
    {
      get
      {
        return MySqlWorkbenchConnection.GetConnectionStatusDisplayText(OldInstanceStatus);
      }
    }
  }
}
