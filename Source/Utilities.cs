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
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;

namespace MySql.TrayApp
{
  public static class Utilities
  {
    public static bool IsMySQLInstallerInstalled()
    {
      return IsApplicationInstalled("MySQL Installer");
    }

    public static bool IsApplicationInstalled(string p_name)
    {
      string keyName;

      // search in: CurrentUser
      keyName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
      if (ExistsInSubKey(Registry.CurrentUser, keyName, "DisplayName", p_name)) return true;


      // search in: LocalMachine_32
      keyName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
      if (ExistsInSubKey(Registry.LocalMachine, keyName, "DisplayName", p_name)) return true;


      // search in: LocalMachine_64
      keyName = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
      if (ExistsInSubKey(Registry.LocalMachine, keyName, "DisplayName", p_name)) return true;

      return false;
    }

    private static bool ExistsInSubKey(RegistryKey p_root, string p_subKeyName, string p_attributeName, string p_name)
    {
      RegistryKey subkey;
      string displayName;

      using (RegistryKey key = p_root.OpenSubKey(p_subKeyName))
      {
        if (key != null)
        {
          foreach (string kn in key.GetSubKeyNames())
          {
            using (subkey = key.OpenSubKey(kn))
            {
              displayName = subkey.GetValue(p_attributeName) as string;
              if (String.Compare(p_name, displayName, StringComparison.OrdinalIgnoreCase) >= 0)
              {
                return true;
              }
            }
          }
        }
      }
      return false;
    }


    public static string GetWorkBenchPath(bool IncludeFileName = true)
    {

      var dirPath64 = System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\MySQL";

      var dirPath32 = System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\MySQL";

      string workbenchPath = string.Empty;

      try
      {
        var dirs = (Directory.EnumerateDirectories(dirPath64, "*", SearchOption.TopDirectoryOnly).
            Where(s => s.ToLower().Contains("workbench"))).ToList();

        dirs.AddRange((Directory.EnumerateDirectories(dirPath32, "*", SearchOption.TopDirectoryOnly).
            Where(s => s.ToLower().Contains("workbench"))));

        foreach (var dir in dirs)
        {
          if (File.Exists(dir + @"\MySQLWorkbench.exe"))
          {
            workbenchPath = dir + @"\MySQLWorkbench.exe";
            return workbenchPath;
          }
        }

      }
      catch  {  }     
      return string.Empty;
    }
  }
}
