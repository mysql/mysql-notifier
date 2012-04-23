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
  }
}
