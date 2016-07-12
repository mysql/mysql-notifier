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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using MySql.Notifier.Enumerations;
using MySql.Notifier.Properties;
using MySQL.Utility.Classes;
using MySQL.Utility.Classes.MySQL;
using MySQL.Utility.Forms;

namespace MySql.Notifier.Classes
{
  internal static class Program
  {
    /// <summary>
    /// An instance of the <see cref="NotifierApplicationContext"/> class.
    /// </summary>
    private static NotifierApplicationContext _applicationContext;

    /// <summary>
    /// Gets an instance of the <see cref="Notifier"/> class.
    /// </summary>
    public static Notifier Notifier
    {
      get
      {
        return _applicationContext.NotifierInstance;
      }
    }

    /// <summary>
    /// Sends an error message to the application log and optionally shows it to the users.
    /// </summary>
    /// <param name="errorTitle">The title displayed on the error dialog.</param>
    /// <param name="errorMessage">A custom error message.</param>
    /// <param name="showErrorDialog">Flag indicating whether the error is shown to users.</param>
    /// <param name="exception">An <see cref="Exception"/> object.</param>
    /// <param name="errorLevel">The <see cref="SourceLevels"/> to describe the severity of the error.</param>
    public static void MySqlNotifierErrorHandler(string errorTitle, string errorMessage, bool showErrorDialog, Exception exception, SourceLevels errorLevel = SourceLevels.Error)
    {
      bool emptyErrorMessage = string.IsNullOrEmpty(errorMessage);
      if (string.IsNullOrEmpty(errorTitle))
      {
        errorTitle = errorLevel == SourceLevels.Critical || emptyErrorMessage ? Resources.HighSeverityError : Resources.ErrorTitle;
      }

      if (emptyErrorMessage)
      {
        errorMessage = Resources.UnhandledExceptionText;
      }

      string exceptionMessage = null;
      string exceptionMoreInfo = null;
      var errorBuilder = new StringBuilder(errorMessage);
      if (exception != null)
      {
        if (exception.Message.Length > 0)
        {
          exceptionMessage = exception.Message;
          errorBuilder.AppendLine(exception.Message);
        }

        if (exception.InnerException != null)
        {
          errorBuilder.AppendLine(exception.InnerException.Message);
          exceptionMoreInfo = exception.InnerException != null ? string.Format("{0}{1}{1}", exception.InnerException.Message, Environment.NewLine) : string.Empty;
        }

        exceptionMoreInfo += exception.StackTrace;
      }

      string completeErrorMessage = errorBuilder.ToString();
      if (showErrorDialog)
      {
        var infoProperties = InfoDialogProperties.GetErrorDialogProperties(errorTitle, errorMessage, exceptionMessage, exceptionMoreInfo);
        infoProperties.WordWrapMoreInfo = false;
        infoProperties.CommandAreaProperties.DefaultButton = InfoDialog.DefaultButtonType.Button1;
        infoProperties.CommandAreaProperties.DefaultButtonTimeout = 60;
        InfoDialog.ShowDialog(infoProperties);
      }

      MySqlSourceTrace.WriteToLog(completeErrorMessage, errorLevel);
    }

    /// <summary>
    /// Sends an error message to the application log and optionally shows it to the users.
    /// </summary>
    /// <param name="errorMessage">A custom error message.</param>
    /// <param name="showErrorDialog">Flag indicating whether the error is shown to users.</param>
    /// <param name="exception">An <see cref="Exception"/> object.</param>
    /// <param name="errorLevel">The <see cref="SourceLevels"/> to describe the severity of the error.</param>
    public static void MySqlNotifierErrorHandler(string errorMessage, bool showErrorDialog, Exception exception, SourceLevels errorLevel = SourceLevels.Error)
    {
      MySqlNotifierErrorHandler(null, errorMessage, showErrorDialog, exception, errorLevel);
    }

    /// <summary>
    /// Shows an error message to the user and sends it to the application log.
    /// </summary>
    /// <param name="exception">An <see cref="Exception"/> object.</param>
    /// <param name="critical">Flag indicating whether the error is treated as <see cref="SourceLevels.Critical"/> or <see cref="SourceLevels.Error"/>.</param>
    public static void MySqlNotifierErrorHandler(Exception exception, bool critical)
    {
      MySqlNotifierErrorHandler(null, true, exception, critical ? SourceLevels.Critical : SourceLevels.Error);
    }

    /// <summary>
    /// Shows an error message to the user and sends it to the application log.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="args"><see cref="ThreadExceptionEventArgs"/> arguments.</param>
    private static void MySQLNotifierThreadExceptionEventHandler(object sender, ThreadExceptionEventArgs args)
    {
      MySqlNotifierErrorHandler(args.Exception, true);
    }

    /// <summary>
    /// Shows an error message to the user and sends it to the application log.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="args"><see cref="UnhandledExceptionEventArgs"/> arguments.</param>
    private static void MySQLNotifierAppExceptionHandler(object sender, UnhandledExceptionEventArgs args)
    {
      MySqlNotifierErrorHandler((Exception)args.ExceptionObject, true);
    }

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main(params string[] args)
    {
      // Initialize error handler settings
      MySqlSourceTrace.LogFilePath = Notifier.EnvironmentApplicationDataDirectory + Notifier.ERROR_LOG_FILE_RELATIVE_PATH;
      MySqlSourceTrace.SourceTraceClass = "MySqlNotifier";

      if (args.Length > 0 && (args[0] == "--c" || args[0] == "--x"))
      {
        CheckForUpdates(args[0]);
      }

      if (!SingleInstance.Start())
      {
        return;
      }

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.ThreadException += MySQLNotifierThreadExceptionEventHandler;

      // For Windows Forms errors to go through our handler.
      Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

      // For non-UI thread exceptions
      AppDomain.CurrentDomain.UnhandledException += MySQLNotifierAppExceptionHandler;

      try
      {
        UpdateSettingsFile();
        _applicationContext = new NotifierApplicationContext();
        Application.Run(_applicationContext);
      }
      catch (Exception ex)
      {
        MySqlNotifierErrorHandler(ex, false);
      }

      SingleInstance.Stop();
    }

    private static void UpdateSettingsFile()
    {
      // Fix the error where Notifier file had main element as MySQLForExcel
      var settingsFilePath = NotifierSettings.SettingsFilePath;
      if (File.Exists(settingsFilePath))
      {
        XDocument xdoc = XDocument.Load(settingsFilePath);
        var element = xdoc.Elements("MySQLForExcel").FirstOrDefault();
        if (element != null)
        {
          element.Name = AssemblyInfo.AssemblyTitle.Replace(" ", string.Empty);
          xdoc.Save(settingsFilePath);
        }
      }

      // Change the default value for AutoAddPattern
      if (Settings.Default.AutoAddPattern == ".*mysqld.*")
      {
        Settings.Default.AutoAddPattern = "mysql";
        Settings.Default.Save();
      }
    }

    private static void CheckForUpdates(string arg)
    {
      if (arg != "--c")
      {
        return;
      }

      Settings.Default.UpdateCheck = (int)SoftwareUpdateStatus.Checking;
      Settings.Default.Save();
    }
  }
}