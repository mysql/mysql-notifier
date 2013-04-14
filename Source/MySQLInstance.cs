//
// Copyright (c) 2013, Oracle and/or its affiliates. All rights reserved.
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

namespace MySql.Notifier
{
  using System;
  using System.Linq;
  using System.Xml;
  using System.Xml.Serialization;
  using MySQL.Utility;

  /// <summary>
  /// A MySQL Server instance that can be reached through a <see cref="MySqlWorkbenchConnection"/>.
  /// </summary>
  [Serializable]
  public class MySQLInstance
  {
    /// <summary>
    /// Default monitoring interval in seconds for a MySQL instance, set to 10 minutes.
    /// </summary>
    public const int DEFAULT_MONITORING_INTERVAL = 600;

    #region Fields

    /// <summary>
    /// The Id of the <see cref="MySqlWorkbenchConnection"/> connection.
    /// </summary>
    private string _connectionId;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="MySQLInstance"/> class.
    /// </summary>
    public MySQLInstance()
    {
      _connectionId = string.Empty;
      MonitorAndNotifyStatus = true;
      MonitoringInterval = DEFAULT_MONITORING_INTERVAL;
      UpdateTrayIconOnStatusChange = true;
      WorkbenchConnection = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySQLInstance"/> class.
    /// </summary>
    /// <param name="workbenchConnection">A <see cref="MySqlWorkbenchConnection"/> object.</param>
    public MySQLInstance(MySqlWorkbenchConnection workbenchConnection)
      : this()
    {
      WorkbenchConnection = workbenchConnection;
      ConnectionId = workbenchConnection != null ? workbenchConnection.Id : string.Empty;
    }

    #region Properties

    /// <summary>
    /// Gets or sets the Id of the <see cref="MySqlWorkbenchConnection"/> connection.
    /// </summary>
    [XmlAttribute(AttributeName = "ConnectionId")]
    public string ConnectionId
    {
      get
      {
        return _connectionId;
      }

      set
      {
        _connectionId = value;
        if (!string.IsNullOrEmpty(_connectionId))
        {
          WorkbenchConnection = MySqlWorkbench.Connections.First(conn => conn.Id == _connectionId);
        }
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is being monitored and status changes notified to users.
    /// </summary>
    [XmlAttribute(AttributeName = "MonitorAndNotifyStatus")]
    public bool MonitorAndNotifyStatus { get; set; }

    /// <summary>
    /// Gets or sets the monitoring interval in seconds for this MySQL instance.
    /// </summary>
    [XmlAttribute(AttributeName = "MonitoringInterval")]
    public int MonitoringInterval { get; set; }

    /// <summary>
    ///Gets the name of the Workbench connection used to connect to the MySQL Server instance.
    /// </summary>
    [XmlIgnore]
    public string Name
    {
      get
      {
        return WorkbenchConnection != null ? WorkbenchConnection.Name : string.Empty;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether status changes of this instance trigger an update of the tray icon.
    /// </summary>
    [XmlAttribute(AttributeName = "UpdateTrayIconOnStatusChange")]
    public bool UpdateTrayIconOnStatusChange { get; set; }

    /// <summary>
    /// Gets a name composed of the Workbench connection name plus additional information of the connection host.
    /// </summary>
    [XmlIgnore]
    public string VerboseName
    {
      get
      {
        if (WorkbenchConnection == null)
        {
          return string.Empty;
        }

        string hostDescription = WorkbenchConnection.DriverType == MySqlWorkbenchConnectionType.NamedPipes ? MySqlWorkbenchConnection.DEFAULT_HOSTNAME : string.Format("{0}:{1}", WorkbenchConnection.Host, WorkbenchConnection.Port);
        return string.Format("{0} ({1})", WorkbenchConnection.Name, hostDescription);
      }
    }

    /// <summary>
    /// Gets a <see cref="MySqlWorkbenchConnection"/> object with the connection properties for this instance.
    /// </summary>
    [XmlIgnore]
    public MySqlWorkbenchConnection WorkbenchConnection { get; private set; }

    #endregion Properties
  }
}