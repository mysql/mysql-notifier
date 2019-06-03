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

namespace MySql.Notifier.Classes.EventArguments
{
  /// <summary>
  /// Provides information for the InstanceConnectionStatusTestErrorThrown event.
  /// </summary>
  public class InstanceConnectionStatusTestErrorThrownArgs : EventArgs
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="InstanceConnectionStatusTestErrorThrownArgs"/> class.
    /// </summary>
    /// <param name="instance">MySQL instance whose status changed.</param>
    /// <param name="ex">Exception thrown during a connection status test.</param>
    public InstanceConnectionStatusTestErrorThrownArgs(MySqlInstance instance, Exception ex)
    {
      Instance = instance;
      ErrorException = ex;
    }

    /// <summary>
    /// Gets the error Exception thrown during a connection status test.
    /// </summary>
    public Exception ErrorException { get; }

    /// <summary>
    /// Gets the MySQL instance whose status changed.
    /// </summary>
    public MySqlInstance Instance { get; }
  }
}
