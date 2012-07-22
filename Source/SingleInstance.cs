// 
// Copyright (c) 2012, Oracle and/or its affiliates. All rights reserved.
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

using System;
using System.Threading;

namespace MySql.Notifier
{

    /// <summary>
    /// Enforces a single instance.
    /// </summary>
    /// <remarks>
    /// Start() tries to create a mutex.
    /// If it detects that another instance is already using the mutex, then it returns FALSE.
    /// Otherwise it returns true.
    /// </remarks>
    static public class SingleInstance
    {
      public static readonly int WM_SHOWFIRSTINSTANCE = WinAPI.RegisterWindowMessage("WM_SHOWFIRSTINSTANCE|{0}", AssemblyInfo.AssemblyGUID);
      private static Mutex mutex;

      static public bool Start()
      {
        bool onlyInstance = false;

        // Below "Local" limits a single instance per session, if we want to limit to a single instance
        // across all sessions (multiple users and terminal services) we can change it to "Global".
        string mutexName = String.Format("Local\\{0}", AssemblyInfo.AssemblyGUID);

        mutex = new Mutex(true, mutexName, out onlyInstance);
        return onlyInstance;
      }

      static public void ShowFirstInstance()
      {
        WinAPI.PostMessage((IntPtr)WinAPI.HWND_BROADCAST,
                           WM_SHOWFIRSTINSTANCE,
                           IntPtr.Zero,
                           IntPtr.Zero);
      }

      static public void Stop()
      {
        mutex.ReleaseMutex();
      }

    }
}