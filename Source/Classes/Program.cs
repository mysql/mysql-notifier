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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using MySql.Notifier.Enumerations;
using MySql.Notifier.Properties;
using MySQL.Utility.Classes;
using MySQL.Utility.Classes.MySQL;
using MySQL.Utility.Classes.MySQLInstaller;
using MySQL.Utility.Classes.MySQLWorkbench;
using MySQL.Utility.Forms;

namespace MySql.Notifier.Classes
{
  internal static class Program
  {
    #region Constants

    /// <summary>
    /// The relative path of the Notifier's connections file under the application data directory.
    /// </summary>
    public const string CONNECTIONS_FILE_RELATIVE_PATH = SETTINGS_DIRECTORY_RELATIVE_PATH + @"\connections.xml";

    /// <summary>
    /// The relative path of the Notifier's error log file under the application data directory.
    /// </summary>
    public const string ERROR_LOG_FILE_RELATIVE_PATH = SETTINGS_DIRECTORY_RELATIVE_PATH + @"\MySQLNotifier.log";

    /// <summary>
    /// The relative path of the Notifier's passwords vault file under the application data directory.
    /// </summary>
    public const string PASSWORDS_VAULT_FILE_RELATIVE_PATH = SETTINGS_DIRECTORY_RELATIVE_PATH + @"\user_data.dat";

    /// <summary>
    /// The relative path of the settings directory under the application data directory.
    /// </summary>
    public const string SETTINGS_DIRECTORY_RELATIVE_PATH = @"\Oracle\MySQL Notifier";

    /// <summary>
    /// The relative path of the Notifier's settings file under the application data directory.
    /// </summary>
    public const string SETTINGS_FILE_RELATIVE_PATH = SETTINGS_DIRECTORY_RELATIVE_PATH + @"\settings.config";

    /// <summary>
    /// The root element name of the settings.config file used in versions prior 1.1.7 which is wrong.
    /// </summary>
    private const string WRONG_SETTINGS_FILE_ROOT_ELEMENT_NAME = "MySQLForExcel";

    #endregion Constants

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
    /// Gets the environment's application data directory.
    /// </summary>
    public static string EnvironmentApplicationDataDirectory
    {
      get
      {
        return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
      }
    }

    /// <summary>
    /// Gets the installation path where the MySQL Notifier executable is located.
    /// </summary>
    public static string InstallLocation { get; private set; }

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
        infoProperties.FitTextStrategy = InfoDialog.FitTextsAction.IncreaseDialogWidth;
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
      try
      {
        // Initialize error handler settings
        MySqlSourceTrace.LogFilePath = EnvironmentApplicationDataDirectory + ERROR_LOG_FILE_RELATIVE_PATH;
        MySqlSourceTrace.SourceTraceClass = "MySqlNotifier";

        // Static initializations
        InstallLocation = Utility.GetMySqlAppInstallLocation(AssemblyInfo.AssemblyTitle);
        InitializeStaticSettings();
        CustomizeInfoDialog();

        // Update settings file
        UpdateSettingsFile();

        // In case the .exe is being run just for the sake of checking updates
        if (args.Length > 0)
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

        // Initialize app context and run
        _applicationContext = new NotifierApplicationContext();
        Application.Run(_applicationContext);
      }
      catch (Exception ex)
      {
        MySqlNotifierErrorHandler(ex, false);
      }

      SingleInstance.Stop();
    }

    private static void ChangeAutoAddPatternDefaultValue()
    {
      if (Settings.Default.AutoAddPattern != ".*mysqld.*")
      {
        return;
      }

      Settings.Default.AutoAddPattern = "mysql";
      Settings.Default.Save();
    }

    /// <summary>
    /// Customizes the looks of the <see cref="MySQL.Utility.Forms.InfoDialog"/> form for the MySQL Notifier.
    /// </summary>
    private static void CustomizeInfoDialog()
    {
      InfoDialog.ApplicationName = AssemblyInfo.AssemblyTitle;
      InfoDialog.SuccessLogo = Resources.ApplicationLogo;
      InfoDialog.ErrorLogo = Resources.NotifierErrorImage;
      InfoDialog.WarningLogo = Resources.NotifierWarningImage;
      InfoDialog.InformationLogo = Resources.ApplicationLogo;
      InfoDialog.ApplicationIcon = Resources.MySqlNotifierIcon;
    }

    /// <summary>
    /// Initializes settings for the <see cref="MySqlWorkbench"/>, <see cref="MySqlSourceTrace"/>, <see cref="MySqlWorkbenchPasswordVault"/> and <see cref="MySqlInstaller"/> classes.
    /// </summary>
    private static void InitializeStaticSettings()
    {
      MySqlWorkbench.ExternalApplicationName = AssemblyInfo.AssemblyTitle;
      MySqlWorkbenchPasswordVault.ApplicationPasswordVaultFilePath = EnvironmentApplicationDataDirectory + PASSWORDS_VAULT_FILE_RELATIVE_PATH;
      MySqlWorkbench.ExternalConnections.CreateDefaultConnections = !MySqlWorkbench.ConnectionsFileExists && MySqlWorkbench.Connections.Count == 0;
      MySqlWorkbench.ExternalApplicationsConnectionsFileRetryLoadOrRecreate = true;
      MySqlWorkbench.ExternalApplicationConnectionsFilePath = EnvironmentApplicationDataDirectory + CONNECTIONS_FILE_RELATIVE_PATH;
      MySqlWorkbench.LoadData();
      MySqlWorkbench.LoadServers();
      MySqlInstaller.InstallerLegacyDllPath = InstallLocation;
      MySqlInstaller.LoadData();
    }

    /// <summary>
    /// Reads machine services directly from the XML configuration file to update the settings object.
    /// </summary>
    /// <param name="rootElement">The root element of the configuration file.</param>
    private static void UpdateMachineServicesInSettingsFromXml(XElement rootElement)
    {
      if (rootElement == null)
      {
        return;
      }

      var machineListElement = rootElement.Element("MachineList");
      if (machineListElement == null || string.IsNullOrEmpty(machineListElement.Value))
      {
        return;
      }

      var machineListDoc = new XmlDocument();
      machineListDoc.LoadXml(machineListElement.Value);
      if (machineListDoc.DocumentElement == null)
      {
        return;
      }

      if (machineListDoc.DocumentElement.ChildNodes.Count == 0)
      {
        return;
      }

      var temporaryServicesList = new List<MySqlService>();
      var machinesListCopy = Settings.Default.MachineList.ToList();
      foreach (XmlNode machineNode in machineListDoc.DocumentElement.ChildNodes)
      {
        if (machineNode.Attributes == null)
        {
          continue;
        }

        var machineId = machineNode.Attributes["MachineId"].Value;
        var machine = machinesListCopy.FirstOrDefault(m => m.MachineId.Equals(machineId, StringComparison.Ordinal));
        if (machine == null)
        {
          continue;
        }

        var servicesListNode = machineNode.SelectSingleNode("descendant::ServicesList");
        if (servicesListNode == null || servicesListNode.ChildNodes.Count == 0)
        {
          continue;
        }

        foreach (XmlNode serviceNode in servicesListNode.ChildNodes)
        {
          var service = new MySqlService();
          if (serviceNode.Attributes == null || serviceNode.Attributes.Count == 0)
          {
            continue;
          }

          service.DisplayName = serviceNode.Attributes["DisplayName"].Value;

          bool parsedNotifyOnStatusChange;
          if (bool.TryParse(serviceNode.Attributes["NotifyOnStatusChange"].Value, out parsedNotifyOnStatusChange))
          {
            service.NotifyOnStatusChange = parsedNotifyOnStatusChange;
          }

          service.ServiceName = serviceNode.Attributes["ServiceName"].Value;

          bool parsedUpdateTrayIconOnStatusChange;
          if (bool.TryParse(serviceNode.Attributes["UpdateTrayIconOnStatusChange"].Value,
            out parsedUpdateTrayIconOnStatusChange))
          {
            service.UpdateTrayIconOnStatusChange = parsedUpdateTrayIconOnStatusChange;
          }

          temporaryServicesList.Add(service);
        }

        machine.Services = temporaryServicesList;
      }

      if (machinesListCopy.Count == 0)
      {
        return;
      }

      Settings.Default.MachineList = machinesListCopy;
      Settings.Default.Save();
    }

    /// <summary>
    /// Reads MySQL instances directly from the XML configuration file to update the settings object.
    /// </summary>
    /// <param name="rootElement">The root element of the configuration file.</param>
    private static void UpdateMySqlInstancesInSettingsFromXml(XElement rootElement)
    {
      if (rootElement == null)
      {
        return;
      }

      var mySqlInstancesListElement = rootElement.Element("MySQLInstancesList");
      if (mySqlInstancesListElement == null || string.IsNullOrEmpty(mySqlInstancesListElement.Value))
      {
        return;
      }

      var mySqlInstancesListDoc = new XmlDocument();
      mySqlInstancesListDoc.LoadXml(mySqlInstancesListElement.Value);
      if (mySqlInstancesListDoc.DocumentElement == null)
      {
        return;
      }

      if (mySqlInstancesListDoc.DocumentElement.ChildNodes.Count == 0)
      {
        return;
      }

      var temporaryInstancesList = new List<MySqlInstance>();
      foreach (XmlNode instanceNode in mySqlInstancesListDoc.DocumentElement.ChildNodes)
      {
        if (instanceNode.Attributes == null || instanceNode.Attributes.Count == 0)
        {
          continue;
        }

        var instance = new MySqlInstance();
        instance.HostName = instanceNode.Attributes["HostName"].Value;

        bool parsedMonitorAndNotifyStatus;
        if (bool.TryParse(instanceNode.Attributes["MonitorAndNotifyStatus"].Value, out parsedMonitorAndNotifyStatus))
        {
          instance.MonitorAndNotifyStatus = parsedMonitorAndNotifyStatus;
        }

        uint parsedInterval;
        if (uint.TryParse(instanceNode.Attributes["MonitoringInterval"].Value, out parsedInterval))
        {
          instance.MonitoringInterval = parsedInterval;
        }

        TimeUtilities.IntervalUnitOfMeasure parsedIntervalUnitOfMeasure;
        if (Enum.TryParse(instanceNode.Attributes["MonitoringIntervalUnitOfMeasure"].Value, out parsedIntervalUnitOfMeasure))
        {
          instance.MonitoringIntervalUnitOfMeasure = parsedIntervalUnitOfMeasure;
        }

        uint parsedPort;
        if (uint.TryParse(instanceNode.Attributes["Port"].Value, out parsedPort))
        {
          instance.Port = parsedPort;
        }

        bool parsedUpdateTrayIconOnStatusChange;
        if (bool.TryParse(instanceNode.Attributes["UpdateTrayIconOnStatusChange"].Value, out parsedUpdateTrayIconOnStatusChange))
        {
          instance.UpdateTrayIconOnStatusChange = parsedUpdateTrayIconOnStatusChange;
        }

        instance.WorkbenchConnectionId = instanceNode.Attributes["WorkbenchConnectionId"].Value;
        temporaryInstancesList.Add(instance);
      }

      if (temporaryInstancesList.Count == 0)
      {
        return;
      }

      Settings.Default.MySQLInstancesList = temporaryInstancesList;
      Settings.Default.Save();
    }

    /// <summary>
    /// Fixes the error where Notifier file had main element as MySQLForExcel
    /// </summary>
    private static void FixMySqlForExcelMainElement()
    {
      var settingsFilePath = NotifierSettings.SettingsFilePath;
      if (!File.Exists(settingsFilePath))
      {
        return;
      }

      var settingsCopyFileName = settingsFilePath + ".bak";
      try
      {
        // Make a copy of the configuration file before attempting to fix the root element name from MySQLForExcel to MySQLNotifier
        File.Copy(settingsFilePath, settingsCopyFileName, true);

        XDocument xdoc = XDocument.Load(settingsFilePath);
        var element = xdoc.Elements(WRONG_SETTINGS_FILE_ROOT_ELEMENT_NAME).FirstOrDefault();
        if (element == null)
        {
          return;
        }

        // Change MySQLForExcel for MySQLNotifier
        element.Name = AssemblyInfo.AssemblyTitle.Replace(" ", string.Empty);
        xdoc.Save(settingsFilePath);
        NotifierSettings.RootElementName = null;

        // For some reason collections are not loaded from the modified file, so we need to read them manually and feed them again to the settings object
        UpdateMachineServicesInSettingsFromXml(element);
        UpdateMySqlInstancesInSettingsFromXml(element);
      }
      catch (Exception ex)
      {
        MySqlNotifierErrorHandler(Resources.FixMySqlForExcelMainElementWarningText, false, ex, SourceLevels.Warning);

        // Revert back the settings.config file from the copy we made.
        NotifierSettings.RootElementName = WRONG_SETTINGS_FILE_ROOT_ELEMENT_NAME;
        if (File.Exists(settingsCopyFileName))
        {
          File.Copy(settingsCopyFileName, settingsFilePath, true);
          Settings.Default.Save();
        }
      }
      finally
      {
        // Delete the backup file.
        if (File.Exists(settingsCopyFileName))
        {
          File.Delete(settingsCopyFileName);
        }
      }
    }

    /// <summary>
    /// Updates the user's settings file for changes in newer versions of the product.
    /// </summary>
    private static void UpdateSettingsFile()
    {
      FixMySqlForExcelMainElement();
      ChangeAutoAddPatternDefaultValue();
    }

    private static void CheckForUpdates(string arg)
    {
      if (arg != "--c" || arg != "--x")
      {
        return;
      }

      Settings.Default.UpdateCheck = (int)SoftwareUpdateStatus.Checking;
      Settings.Default.Save();
    }
  }
}