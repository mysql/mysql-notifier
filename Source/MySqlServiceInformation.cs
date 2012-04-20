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
using System.Management;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using MySQL.Utility;

namespace MySql.TrayApp
{
  internal class MySqlServiceInformation
  {

    public const string EXE_PATH_NAME = "mysqld";
    public const string EXE_PATH_NAME_NT = "mysqld-nt";
    public const string WB_XMLVERSION = "2.0";

    public static string GetMySqlServiceInformation(string serviceName, out string location)
    {
      string status = "Unknown";
      string service = string.Format("Win32_Service.Name='{0}'", serviceName);
      ManagementObject mo = new ManagementObject(new ManagementPath(service));
      if (mo != null)
      {
        status = mo.Properties["State"].Value.ToString().Trim();
        location = Dns.GetHostName(); // by now will be only the local computer
      }
      else
        location = "Not Found";
      return status;
    }


    /// <summary>
    /// Gets all services using mysqld or mysqld-nt
    /// </summary>
    /// <returns></returns>
    public static List<ManagementObject> GetInstances(string filter)
    {
      List<ManagementObject> list = new List<ManagementObject>();

      ManagementClass mc = new ManagementClass("Win32_Service");
      var Instances = mc.GetInstances().Cast<ManagementObject>().ToList();
      foreach (ManagementObject o in Instances)
      {
        if (String.IsNullOrEmpty(filter))
          list.Add(o);
        else
        {
          object path = o.GetPropertyValue("PathName");
          if (path != null && path.ToString().Contains(filter))
            list.Add(o);
        }
      }
      return list;
    }


    public static String GetServerName(string serviceName)
    {      
      var version = string.Empty;

      if (!FileExists("server_instances.xml", out version)) return string.Empty;

      if (string.Compare(version, WB_XMLVERSION, StringComparison.InvariantCultureIgnoreCase) != 0)      
        throw new Exception(Properties.Resources.UnSupportedWBXMLVersion);
      
      XmlTextReader reader = new XmlTextReader(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData)
                    + @"\MySQL\Workbench\" + "server_instances.xml");
      XmlDocument doc = new XmlDocument();
      doc.Load(reader);
      reader.Close();
      try
      {

        XmlElement root = doc.DocumentElement;
        var nodes = root.SelectNodes("//data/value[@content-struct-name='db.mgmt.ServerInstance']/value[@struct-name='db.mgmt.ServerInstance']/value[@key='serverInfo']/value[(text() = 'Windows')]");

        foreach (var item in nodes)
        {
          var xPathServiceName = string.Format("//data/value[@content-struct-name='db.mgmt.ServerInstance']/value[@struct-name='db.mgmt.ServerInstance']/value[@key='serverInfo']/value[(text() = '{0}')]", serviceName);
          var somenode = ((XmlElement)item).ParentNode.SelectSingleNode(xPathServiceName);
          if (somenode != null)
          {
            return somenode.ParentNode.NextSibling.InnerText;
          }
        }
      }
      catch
      {
        return string.Empty;
      }
      return string.Empty;
    }

    private static bool FileExists(string name, out string version)
    {
      version = string.Empty;
      // Get path to the Application Data folder
      var appDataPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
      if (File.Exists(appDataPath + @"\MySQL\Workbench\" + name))
      {
        XmlTextReader reader = new XmlTextReader(appDataPath + @"\MySQL\Workbench\" + name);
        XmlDocument doc = new XmlDocument();
        doc.Load(reader);
        reader.Close();

        XmlElement root = doc.DocumentElement;
        version = root.SelectSingleNode("//data[@grt_format]").Attributes["grt_format"].Value;
        return true;
      }
      else
        return false;
    }


  }
}
