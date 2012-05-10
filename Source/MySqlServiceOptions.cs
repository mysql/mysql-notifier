// Copyright © 2010, Oracle and/or its affiliates. All rights reserved.
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
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MySql.TrayApp
{
  [Serializable]
  public class MySqlServiceOptions
  {
    public string Name { get; set; }
    public bool NotifyOnStateChange { get; set; }
  }

  public static class MySqlServiceOptionsList
  {
    public static void Save(List<MySqlServiceOptions> serviceOptionsList)
    {
      using (MemoryStream ms = new MemoryStream())
      {
        using (StreamReader sr = new StreamReader(ms))
        {
          BinaryFormatter serviceBinaryFormatter = new BinaryFormatter();
          serviceBinaryFormatter.Serialize(ms, serviceOptionsList);
          ms.Position = 0;
          byte[] buffer = new byte[(int)ms.Length];
          ms.Read(buffer, 0, buffer.Length);
          Properties.Settings.Default.ServiceSettingsList = Convert.ToBase64String(buffer);          
        }
      }    
    }

    public static List<MySqlServiceOptions> LoadServices()
    {
      using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(Properties.Settings.Default.ServiceSettingsList)))
      {
        BinaryFormatter serviceBinaryFormatter = new BinaryFormatter();
        return (List<MySqlServiceOptions>)serviceBinaryFormatter.Deserialize(ms);
      }    
    }
  }

 
}
