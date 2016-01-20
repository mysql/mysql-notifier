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
  /// Indicates the current state of the service.
  /// </summary>
  public enum MySqlServiceStatus
  {
    /// <summary>
    /// The service is not reachable. Verify WMI connection with the host.
    /// </summary>
    Unavailable = 0,

    /// <summary>
    /// The service is not running. This corresponds to the Win32 SERVICE_STOPPED constant, which is defined as 0x00000001.
    /// </summary>
    Stopped = 1,

    /// <summary>
    /// The service is starting. This corresponds to the Win32 SERVICE_START_PENDING constant, which is defined as 0x00000002.
    /// </summary>
    StartPending = 2,

    /// <summary>
    /// The service is stopping. This corresponds to the Win32 SERVICE_STOP_PENDING constant, which is defined as 0x00000003.
    /// </summary>
    StopPending = 3,

    /// <summary>
    /// The service is running. This corresponds to the Win32 SERVICE_RUNNING constant, which is defined as 0x00000004.
    /// </summary>
    Running = 4,

    /// <summary>
    /// The service continue is pending. This corresponds to the Win32 SERVICE_CONTINUE_PENDING constant, which is defined as 0x00000005.
    /// </summary>
    ContinuePending = 5,

    /// <summary>
    /// The service pause is pending. This corresponds to the Win32 SERVICE_PAUSE_PENDING constant, which is defined as 0x00000006.
    /// </summary>
    PausePending = 6,

    /// <summary>
    /// The service is paused. This corresponds to the Win32 SERVICE_PAUSED constant, which is defined as 0x00000007.
    /// </summary>
    Paused = 7,
  }
}
