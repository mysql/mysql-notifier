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

namespace MySql.Notifier.Enumerations
{
  /// <summary>
  /// Specifies the type of change that a list suffered.
  /// </summary>
  public enum ListChangeType
  {
    /// <summary>
    /// An element was added to the list by a user.
    /// </summary>
    AddByUser,

    /// <summary>
    /// An element was added to the list during the initial load.
    /// </summary>
    AddByLoad,

    /// <summary>
    /// An element has been added to the list automatically by an Auto-Add or Service Migration operation.
    /// </summary>
    AutoAdd,

    /// <summary>
    /// All elements in the list have been cleared, list became empty.
    /// </summary>
    Cleared,

    /// <summary>
    /// An element was removed from the list by a user.
    /// </summary>
    RemoveByUser,

    /// <summary>
    /// An element was removed from the list by an event notification.
    /// </summary>
    RemoveByEvent,

    /// <summary>
    /// An element within the list was updated.
    /// </summary>
    Updated
  }
}
