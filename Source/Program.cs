//
// Copyright (c) 2012-2013, Oracle and/or its affiliates. All rights reserved.
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
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using MySql.Notifier.Properties;
using MySQL.Utility;

namespace MySql.Notifier
{
  internal static class Program
  {
    private static void MySQLNotifierHandler(Exception e, bool critical)
    {
      using (var errorDialog = new MessageDialog(Resources.HighSeverityError, e.Message, critical))
      {
        errorDialog.ShowDialog();
        MySQLNotifierTrace.GetSourceTrace().WriteError("Unhandled Exception - " + (e.Message + " " + e.InnerException), 1);
      }
    }

    private static void MySQLNotifierThreadExceptionEventHandler(object sender, ThreadExceptionEventArgs args)
    {
      MySQLNotifierHandler(args.Exception, true);
    }

    private static void MySQLNotifierAppExceptionHandler(object sender, UnhandledExceptionEventArgs args)
    {
      MySQLNotifierHandler((Exception)args.ExceptionObject, true);
    }

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main(params string[] args)
    {
      if (args.Length > 0 && (args[0] == "--c" || args[0] == "--x"))
      {
        CheckForUpdates(args[0]);

        //// Migrate Notifier connections to the MySQL Workbench connections file if they have not been migrated and need migrating.
        Notifier.InitializeMySQLWorkbenchStaticSettings();
        MySqlWorkbench.MigrateExternalConnectionsToWorkbench();

        return;
      }

      if (!SingleInstance.Start()) { return; }
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      Application.ThreadException += new ThreadExceptionEventHandler(MySQLNotifierThreadExceptionEventHandler);

      // For Windows Forms errors to go through our handler.
      Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

      // For non-UI thread exceptions
      AppDomain.CurrentDomain.UnhandledException +=
          new UnhandledExceptionEventHandler(MySQLNotifierAppExceptionHandler);

      try
      {
        var applicationContext = new NotifierApplicationContext();
        Application.Run(applicationContext);
      }
      catch (Exception ex)
      {
        using (var errorDialog = new MessageDialog(Resources.HighSeverityError, ex.Message, true))
        {
          errorDialog.ShowDialog();
          MySQLNotifierTrace.GetSourceTrace().WriteError("Application Error - " + (ex.Message + " " + ex.InnerException), 1);
        }
      }
      SingleInstance.Stop();
    }

    private static void CheckForUpdates(string arg)
    {
      if (arg == "--c")
      {
        Settings.Default.UpdateCheck = (int)SoftwareUpdateStaus.Checking;
        Settings.Default.Save();
      }
      return;
    }
  }

  public static class MySQLNotifierTrace
  {
    public static MySQLSourceTrace GetSourceTrace()
    {
      var logPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Oracle\MySQL Notifier";
      return new MySQLSourceTrace("MySqlNotifier", logPath + @"\MySQLNotifier.log", "", SourceLevels.Warning);
    }
  }
}