// Copyright (c) 2012, 2019, Oracle and/or its affiliates. All rights reserved.
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
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using MySql.Notifier.Enumerations;
using MySql.Notifier.Properties;
using MySql.Utility.Classes;
using MySql.Utility.Classes.Logging;
using MySql.Utility.Classes.MySqlInstaller;
using MySql.Utility.Classes.MySqlWorkbench;
using MySql.Utility.Forms;

namespace MySql.Notifier.Classes
{
  internal static class Program
  {
    #region Constants

    /// <summary>
    /// The application name without spaces.
    /// </summary>
    public const string APP_NAME_NO_SPACES = "MySqlNotifier";

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
    public static Notifier Notifier => _applicationContext.NotifierInstance;

    /// <summary>
    /// Gets the environment's application data directory.
    /// </summary>
    public static string EnvironmentApplicationDataDirectory => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

    /// <summary>
    /// Gets the installation path where the MySQL Notifier executable is located.
    /// </summary>
    public static string InstallLocation { get; private set; }

    /// <summary>
    /// Shows an error message to the user and sends it to the application log.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="args"><see cref="ThreadExceptionEventArgs"/> arguments.</param>
    private static void MySQLNotifierThreadExceptionEventHandler(object sender, ThreadExceptionEventArgs args)
    {
      Logger.LogException(args.Exception, true, null, null, true);
    }

    /// <summary>
    /// Shows an error message to the user and sends it to the application log.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="args"><see cref="UnhandledExceptionEventArgs"/> arguments.</param>
    private static void MySQLNotifierAppExceptionHandler(object sender, UnhandledExceptionEventArgs args)
    {
      Logger.LogException(args.ExceptionObject as Exception, true, null, null, true);
    }

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main(params string[] args)
    {
      try
      {
        InitializeLogger();

        // Static initializations
        InstallLocation = Utilities.GetMySqlAppInstallLocation(AssemblyInfo.AssemblyTitle);
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
        Logger.LogException(ex, true, Resources.MainApplicationErrorDetail);
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
    /// Customizes the looks of the <see cref="InfoDialog"/> form for the MySQL Notifier.
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
    /// Initializes the <see cref="Logger"/> class with its required information.
    /// </summary>
    private static void InitializeLogger()
    {
      Logger.Initialize(EnvironmentApplicationDataDirectory + SETTINGS_DIRECTORY_RELATIVE_PATH,
                        APP_NAME_NO_SPACES,
                        false,
                        false,
                        APP_NAME_NO_SPACES,
                        true);
      Logger.PrependUserNameToLogFileName = true;
    }

    /// <summary>
    /// Initializes settings for the <see cref="MySqlWorkbench"/>, <see cref="MySqlWorkbenchPasswordVault"/> and <see cref="MySqlInstaller"/> classes.
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
      InfoDialog.ApplicationName = AssemblyInfo.AssemblyTitle;
      InfoDialog.SuccessLogo = Resources.ApplicationLogo;
      InfoDialog.ErrorLogo = Resources.NotifierErrorImage;
      InfoDialog.WarningLogo = Resources.NotifierWarningImage;
      InfoDialog.InformationLogo = Resources.ApplicationLogo;
      AutoStyleableBaseForm.HandleDpiSizeConversions = true;
      PasswordDialog.ApplicationIcon = Resources.MySqlNotifierIcon;
      PasswordDialog.SecurityLogo = Resources.NotifierSecurityImage;
    }

    /// <summary>
    /// Reads machine services directly from the XML configuration file to update the settings object.
    /// </summary>
    /// <param name="rootElement">The root element of the configuration file.</param>
    private static void UpdateMachineServicesInSettingsFromXml(XElement rootElement)
    {
      var machineListElement = rootElement?.Element("MachineList");
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

          if (bool.TryParse(serviceNode.Attributes["NotifyOnStatusChange"].Value, out var parsedNotifyOnStatusChange))
          {
            service.NotifyOnStatusChange = parsedNotifyOnStatusChange;
          }

          service.ServiceName = serviceNode.Attributes["ServiceName"].Value;
          if (bool.TryParse(serviceNode.Attributes["UpdateTrayIconOnStatusChange"].Value,
            out var parsedUpdateTrayIconOnStatusChange))
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
      var mySqlInstancesListElement = rootElement?.Element("MySQLInstancesList");
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

        var instance = new MySqlInstance
        {
          HostName = instanceNode.Attributes["HostName"].Value
        };
        if (bool.TryParse(instanceNode.Attributes["MonitorAndNotifyStatus"].Value, out var parsedMonitorAndNotifyStatus))
        {
          instance.MonitorAndNotifyStatus = parsedMonitorAndNotifyStatus;
        }

        if (uint.TryParse(instanceNode.Attributes["MonitoringInterval"].Value, out var parsedInterval))
        {
          instance.MonitoringInterval = parsedInterval;
        }

        if (Enum.TryParse(instanceNode.Attributes["MonitoringIntervalUnitOfMeasure"].Value, out TimeUtilities.IntervalUnitOfMeasure parsedIntervalUnitOfMeasure))
        {
          instance.MonitoringIntervalUnitOfMeasure = parsedIntervalUnitOfMeasure;
        }

        if (uint.TryParse(instanceNode.Attributes["Port"].Value, out var parsedPort))
        {
          instance.Port = parsedPort;
        }

        if (bool.TryParse(instanceNode.Attributes["UpdateTrayIconOnStatusChange"].Value, out var parsedUpdateTrayIconOnStatusChange))
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

        var xDocument = XDocument.Load(settingsFilePath);
        var element = xDocument.Elements(WRONG_SETTINGS_FILE_ROOT_ELEMENT_NAME).FirstOrDefault();
        if (element == null)
        {
          return;
        }

        // Change MySQLForExcel for MySQLNotifier
        element.Name = AssemblyInfo.AssemblyTitle.Replace(" ", string.Empty);
        xDocument.Save(settingsFilePath);
        NotifierSettings.RootElementName = null;

        // For some reason collections are not loaded from the modified file, so we need to read them manually and feed them again to the settings object
        UpdateMachineServicesInSettingsFromXml(element);
        UpdateMySqlInstancesInSettingsFromXml(element);
      }
      catch (Exception ex)
      {
        Logger.LogException(ex, false, Resources.FixMySqlForExcelMainElementWarningText);

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