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
      try
      {
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
      catch
      {
        throw;
      }
    }


    /// <summary>
    /// Gets all services using mysqld or mysqld-nt
    /// </summary>
    /// <returns></returns>
    public static List<ManagementObject> GetMySqlInstances()
    {
      ManagementClass mc = new ManagementClass("Win32_Service");
      var Instances = mc.GetInstances().Cast<ManagementObject>().ToList();
      if (Instances.Count > 0)
      {
        var Services = Instances.Where(t => t.GetPropertyValue("PathName").ToString().Contains(EXE_PATH_NAME)).ToList();
        Services.AddRange(Instances.Where(t => t.GetPropertyValue("PathName").ToString().Contains(EXE_PATH_NAME_NT)));
        return Services;
      }
      return null;
    }

    
     /// <summary>
    /// Gets the first connection string that is a local connection and
    /// is related with the service
    /// </summary>
    /// <returns></returns>   
    public static String GetConnectionString(string serviceName)
    {
      //For beta version it will get only the first local connection
      //since the service name is not related or server 
      //for next version if possible
      return GetConnectionString();
    }



    /// <summary>
    /// Gets the first connection string that is a local connection
    /// </summary>
    /// <returns></returns>   
    private static String GetConnectionString()
    {     
      var version = string.Empty;
      if (!FileExists("connections.xml", out version) || string.Compare(version, WB_XMLVERSION, StringComparison.InvariantCultureIgnoreCase) != 0)
      {
          throw new Exception(Properties.Resources.UnSupportedWBXMLVersion);    
      }
      
      XmlTextReader reader = new XmlTextReader(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData)
                    + @"\MySQL\Workbench\" + "connections.xml");
      XmlDocument doc = new XmlDocument();
      doc.Load(reader);
      reader.Close();

        try
          {
            // find first local connection
            XmlElement root = doc.DocumentElement;
            XmlNodeList nodesText = root.SelectNodes("/data/value[@content-struct-name='db.mgmt.Connection']");
            foreach (XmlNode node in nodesText)
            {
              foreach (XmlNode child in node.ChildNodes)
              {
                string driver = child.SelectSingleNode("//link[@key='driver']") != null ?
                                child.SelectSingleNode("//link[@key='driver']").InnerText : string.Empty;
                string host = child.SelectSingleNode("./value[@key='hostIdentifier']") != null ?
                              child.SelectSingleNode("./value[@key='hostIdentifier']").InnerText : string.Empty;
                if (!(string.IsNullOrEmpty(driver) || string.IsNullOrEmpty(host)))
                {
                  if (driver.Contains("com.mysql.rdbms.mysql.driver.native")
                        && (String.Compare(host, "localhost", StringComparison.InvariantCultureIgnoreCase) >= 0
                        || String.Compare(host, "127.0.0.1", StringComparison.InvariantCulture) >= 0))
                  {
                    string connection = child.SelectSingleNode("./value[@key='name']") != null ?
                                        child.SelectSingleNode("./value[@key='name']").InnerText : string.Empty;
                    if (connection != null)
                    {
                      return connection;
                    }
                  }
                }
              }
            }
          }
          catch
          {
            return string.Empty;
          }
      
        return string.Empty;
    }


    public static String GetServerName(string serviceName)
    {
      var version = string.Empty;
      if (!FileExists("server_instances.xml", out version) || string.Compare(version, WB_XMLVERSION, StringComparison.InvariantCultureIgnoreCase) != 0)
      {
        throw new Exception(Properties.Resources.UnSupportedWBXMLVersion);
      }

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
