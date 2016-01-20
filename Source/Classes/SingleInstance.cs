// Copyright (c) 2012, 2016, Oracle and/or its affiliates. All rights reserved.
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
using System.Threading;
using MySQL.Utility.Classes;

namespace MySql.Notifier.Classes
{
  /// <summary>
  /// Enforces a single instance.
  /// </summary>
  /// <remarks>
  /// Start() tries to create a mutex.
  /// If it detects that another instance is already using the mutex, then it returns FALSE.
  /// Otherwise it returns true.
  /// </remarks>
  public static class SingleInstance
  {
    public static readonly int WmShowfirstinstance = WinApi.RegisterWindowMessage("WM_SHOWFIRSTINSTANCE|{0}", AssemblyInfo.AssemblyGuid);
    private static Mutex _mutex;

    public static bool Start()
    {
      bool onlyInstance;

      // Below "Local" limits a single instance per session, if we want to limit to a single instance
      // across all sessions (multiple users and terminal services) we can change it to "Global".
      string mutexName = string.Format("Local\\{0}", AssemblyInfo.AssemblyGuid);

      _mutex = new Mutex(true, mutexName, out onlyInstance);
      return onlyInstance;
    }

    public static void ShowFirstInstance()
    {
      WinApi.PostMessage((IntPtr)WinApi.HWND_BROADCAST,
                         WmShowfirstinstance,
                         IntPtr.Zero,
                         IntPtr.Zero);
    }

    public static void Stop()
    {
      _mutex.ReleaseMutex();
    }
  }
}