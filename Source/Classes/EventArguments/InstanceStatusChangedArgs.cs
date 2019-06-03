// Copyright (c) 2016, 2019, Oracle and/or its affiliates. All rights reserved.
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
using MySql.Utility.Classes.MySqlWorkbench;

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

    #region Properties

    /// <summary>
    /// Gets the MySQL instance whose status changed.
    /// </summary>
    public MySqlInstance Instance { get; }

    /// <summary>
    /// Gets the new status of the instance.
    /// </summary>
    public MySqlWorkbenchConnection.ConnectionStatusType NewInstanceStatus => Instance.ConnectionStatus;

    /// <summary>
    /// Gets a description on the new status of this connection.
    /// </summary>
    public string NewInstanceStatusText => Instance.ConnectionStatusText;

    /// <summary>
    /// Gets the old status of the instance.
    /// </summary>
    public MySqlWorkbenchConnection.ConnectionStatusType OldInstanceStatus { get; }

    /// <summary>
    /// Gets a description on the old status of this connection.
    /// </summary>
    public string OldInstanceStatusText => MySqlWorkbenchConnection.GetConnectionStatusDisplayText(OldInstanceStatus);

    #endregion Properties
  }
}