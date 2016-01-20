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
using System.ComponentModel;

namespace MySql.Notifier.Classes.EventArguments
{
  /// <summary>
  /// Provides information for the InstancesListChanged event.
  /// </summary>
  public class InstancesListChangedArgs : EventArgs
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="InstanceStatusChangedArgs"/> class.
    /// </summary>
    /// <param name="instance">MySQL instance that caused the list change.</param>
    /// <param name="listChange">Type of change done to the list.</param>
    public InstancesListChangedArgs(MySqlInstance instance, ListChangedType listChange)
    {
      Instance = instance;
      ListChange = listChange;
    }

    /// <summary>
    /// Gets the MySQL instance that caused the list change.
    /// </summary>
    public MySqlInstance Instance { get; private set; }

    /// <summary>
    /// Gets the type of change done to the list.
    /// </summary>
    public ListChangedType ListChange { get; private set; }
  }
}
