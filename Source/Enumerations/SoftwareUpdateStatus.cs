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
  public enum SoftwareUpdateStatus
  {
    /// <summary>
    /// Not Available means the check for updates cannot be performed or failed when attempted.
    /// </summary>
    NotAvailable = 0,

    /// <summary>
    /// The check for updates is being performed.
    /// </summary>
    Checking = 1,

    /// <summary>
    /// There are available software updates online.
    /// </summary>
    HasUpdates = 2,

    /// <summary>
    /// The user has been notified if updates are available or if its software is up to date.
    /// </summary>
    Notified = 4
  }
}
