// 
// Copyright (c) 2012, Oracle and/or its affiliates. All rights reserved.
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

using System;
using System.Collections.Generic;
using System.Collections;
using System.ServiceProcess;
using System.Windows.Forms;
using System.Linq;
using System.Globalization;
using System.Management;

namespace MySql.Notifier
{
  /// <summary>
  /// Event Arguments used with the StatusChanged event containing information about the Service's status change.
  /// </summary>
  public class ServiceStatus : EventArgs
  {
    private string serviceName;    
    private ServiceControllerStatus currentStatus;
    private ServiceControllerStatus previousStatus;


    public string ServiceName
    {
      get { return this.serviceName; }
    }
  
    public ServiceControllerStatus CurrentStatus
    {
      get { return this.currentStatus; }
    }

    public ServiceControllerStatus PreviousStatus
    {
      get { return this.previousStatus; }
    }

    public ServiceStatus(string serviceName, ServiceControllerStatus previousStatus, ServiceControllerStatus currentStatus)
    {
      this.serviceName = serviceName;      
      this.previousStatus = previousStatus;
      this.currentStatus = currentStatus;
    }
  }
}
