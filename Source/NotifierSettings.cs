using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySQL.Utility;
using MySql.Notifier.Properties;

namespace MySql.Notifier
{
  public class NotifierSettings : CustomSettingsProvider
  {
    public override string ApplicationName
    {
      get { return Resources.AppName; }
      set { } 
    }

    public override string SettingsPath
    {
      get { return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Oracle\MySQL Notifier\settings.config"; }
    }
  }
}
