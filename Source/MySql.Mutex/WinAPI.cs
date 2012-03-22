// Copyright (c) 2006-2008 MySQL AB, 2008-2009 Sun Microsystems, Inc.
//
// MySQL Connector/NET is licensed under the terms of the GPLv2
// <http://www.gnu.org/licenses/old-licenses/gpl-2.0.html>, like most 
// MySQL Connectors. There are special exceptions to the terms and 
// conditions of the GPLv2 as it is applied to this software, see the 
// FLOSS License Exception
// <http://www.mysql.com/about/legal/licensing/foss-exception.html>.
//
// This program is free software; you can redistribute it and/or modify 
// it under the terms of the GNU General Public License as published 
// by the Free Software Foundation; version 2 of the License.
//
// This program is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License 
// for more details.
//
// You should have received a copy of the GNU General Public License along 
// with this program; if not, write to the Free Software Foundation, Inc., 
// 51 Franklin St, Fifth Floor, Boston, MA 02110-1301  USA

using System;
using System.Runtime.InteropServices;

namespace MySql.MutexHandler
{

  /// <summary>
  /// A wrapper for various WinAPI functions.
  /// </summary>
  /// <remarks>
  /// This class is just a wrapper for various WinAPI functions.
  /// </remarks>
  static public class WinAPI
  {
    [DllImport("user32")]
    public static extern int RegisterWindowMessage(string message);

    internal static int RegisterWindowMessage(string format, params object[] args)
    {
      string message = String.Format(format, args);
      return RegisterWindowMessage(message);
    }

    internal const int HWND_BROADCAST = 0xffff;
    internal const int SW_SHOWNORMAL = 1;

    [DllImport("user32")]
    public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

    [DllImportAttribute("user32.dll")]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImportAttribute("user32.dll")]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    internal static void ShowToFront(IntPtr window)
    {
      ShowWindow(window, SW_SHOWNORMAL);
      SetForegroundWindow(window);
    }
  }
}