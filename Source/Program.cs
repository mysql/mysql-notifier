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
using System.Collections.Generic;
using System.Windows.Forms;
using MySQL.Utility;
using System.Linq;
using MySql.TrayApp.Properties;

namespace MySql.TrayApp
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(params string[] args)
    {
      if (args.Length > 0 && (args[0] == "--c" || args[0] == "--x"))
      {
        CheckForUpdates(args[0]);
        return;
      }

      if (!SingleInstance.Start()) { return; }
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      try
      {
        var applicationContext = new TrayApplicationContext();
        Application.Run(applicationContext);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Program Terminated Unexpectedly", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      SingleInstance.Stop();
    }

    private static void CheckForUpdates(string arg)
    {
      Settings.Default.UpdateCheck = (int)SoftwareUpdateStaus.Checking;
      Settings.Default.Save();

      bool hasUpdates = true;

      if (arg == "--c")
        hasUpdates = MySqlInstaller.HasUpdates(10 * 1000);
      else if (arg == "--x")               // --x is only for testing right now
        System.Threading.Thread.Sleep(5000);

      Settings.Default.UpdateCheck = hasUpdates ? (int)SoftwareUpdateStaus.HasUpdates : 0;
      Settings.Default.Save();
    }
  }
}
