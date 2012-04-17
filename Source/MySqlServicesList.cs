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
using System.Collections;
using System.ServiceProcess;
using System.Windows.Forms;
using System.Linq;
using System.Globalization;
using System.Management;


namespace MySql.TrayApp
{ 
  class MySQLServicesList : IDisposable
  {   
    
    private bool disposed { get; set; }
    
    private List<MySQLService> monitoredMySQLServicesList = new List<MySQLService>();

    public bool HasAdminPrivileges { get; private set; }

    public int InstalledMySQLServicesQuantity { get { return monitoredMySQLServicesList.Count; } }

    public List<MySQLService> InstalledMySQLServicesList
    {
      get { return monitoredMySQLServicesList; }
    }
    

    public MySQLServicesList(bool adminPrivileges)
    {
      HasAdminPrivileges = adminPrivileges;
    }


    /// <summary>
    /// Calls the Dispose method on the passed MySQLService object
    /// </summary>
    /// <param name="service"></param>
    private void DisposeService(MySQLService service)
    {
      //service.Dispose();
    }

    /// <summary>
    /// Cleans-up resources
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes of managed and unmanaged resources.
    /// </summary>
    /// <param name="disposing">If true, the method has been called directly or indirectly by a user's code. Managed and unmanaged
    /// resources can be disposed. If false, the method has been called by the runtime from inside the finalizer and you should not
    /// reference other objects. Only unmanaged resources can be disposed.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (!this.disposed)
      {
        if (disposing)
        {
          if (monitoredMySQLServicesList != null)
          {
            monitoredMySQLServicesList.ForEach(DisposeService);
            monitoredMySQLServicesList.Clear();
          }
        }
      }
      this.disposed = true;
    }


    public delegate void ServicesListChangedHandler(object sender, ServicesListChangedArgs args);

    /// <summary>
    /// Notifies that there are new services added to the list or some removed
    /// </summary>
    public event ServicesListChangedHandler ServicesListChanged;

    /// <summary>
    /// Invokes the ServicesListChanged event
    /// </summary>
    /// <param name="e">Event arguments</param>
    protected virtual void OnServicesListChanged(ServicesListChangedArgs args)
    {
      if (ServicesListChanged != null)
        ServicesListChanged(this, args);
    }

    public delegate void ServiceStatusChangedHandler(object sender, ServiceStatus args);

    /// <summary>
    /// Notifies that the status of one of the services in the list has changed
    /// </summary>
    public event ServiceStatusChangedHandler ServiceStatusChanged;

    protected virtual void OnServiceStatusChanged(ServiceStatus args)
    {
      if (ServiceStatusChanged != null)
        ServiceStatusChanged(this, args);
    }

    private void mySQLService_StatusChanged(object sender, ServiceStatus args)
    {
        OnServiceStatusChanged(args);
    }


    /// <summary>
    /// Refreshes the list of installed MySQL Services only adding or removing items if necessary
    /// </summary>
    /// <param name="monitoredServices"> Has the list of services the app is monitoring </param>
    /// <param name="autoAddServices">If true then add new services</param>
    /// <returns>Flag indicating if the services changed</returns>
    public bool RefreshMySQLServices(ref List<string> monitoredServices, bool autoAddServices)
    {      
      var mySqlServices = MySqlServiceInformation.GetMySqlInstances();
      
      try 
      { 
          //if (mySqlServices.Count <= 0)
          //{
          //  throw new Exception(Properties.Resources.NoServicesExceptionMessage);        
          //}

          var mysqlServicesNames = mySqlServices.Select(t => t.Properties["Name"].Value).Cast<String>().ToList();
          var mysqlMonitoringNames = monitoredMySQLServicesList.Select(t => t.ServiceName).Cast<String>().ToList();

          if (mysqlServicesNames.Except(monitoredServices).Any() || monitoredServices.Except(mysqlMonitoringNames).Any())
          {
            
           // Checked for removed services
            var copyOf = monitoredMySQLServicesList.ToArray();

            foreach (MySQLService mservice in copyOf)
            {
                if (!monitoredServices.Exists(delegate(String service) { return service == mservice.ServiceName; }))
                {
                  if (monitoredMySQLServicesList.Where(t => t.ServiceName == mservice.ServiceName).Count() > 0)
                  {
                    //monitoredMySQLServicesList.Where(t => t.ServiceName == mservice.ServiceName).First().Dispose();
                    monitoredMySQLServicesList.Remove(monitoredMySQLServicesList.Where(t => t.ServiceName == mservice.ServiceName).First());                    
                  }
                  
                }
            }
  
            var copyOfMS = monitoredServices.ToArray();
              // Check for uninstalled services to remove them from the global list
            foreach (string mService in copyOfMS)
              {
                if (!mySqlServices.Exists(delegate(ManagementObject service) { return service.Properties["Name"].Value.ToString() == mService; }))
                {
                
                  //if (monitoredMySQLServicesList.Remove(monitoredMySQLServicesList.Where(t => t.ServiceName == mService).First()))
                    //monitoredMySQLServicesList.Where(t => t.ServiceName == mService).First().Dispose();

                  monitoredServices.Remove(mService);

                }
              }

              //Check if new mysql services exists that are not monitored yet
              foreach (ManagementObject wService in mySqlServices)
              {
                if (!monitoredServices.Exists(delegate(String service) { return service == wService.Properties["Name"].Value.ToString(); }) && autoAddServices)
                {
                  var mySQLService = new MySQLService(wService.Properties["Name"].Value.ToString(), HasAdminPrivileges);
                  mySQLService.StatusChanged += mySQLService_StatusChanged;
                  monitoredMySQLServicesList.Add(mySQLService);
                  monitoredServices.Add(wService.Properties["Name"].Value.ToString());
                }
              }

              //updated new services in mysqlservices global list for UI
              foreach (string mservice in monitoredServices)
              {
                // if its missing then add it otherwise remove it
                if (!monitoredMySQLServicesList.Exists(delegate(MySQLService service) { return service.ServiceName == mservice; }))
                {
                  var mySQLService = new MySQLService(mservice, HasAdminPrivileges);
                  mySQLService.StatusChanged += mySQLService_StatusChanged;
                  monitoredMySQLServicesList.Add(mySQLService);
                }
              }

            monitoredMySQLServicesList.Sort(delegate(MySQLService serv1, MySQLService serv2) { return serv1.ServiceName.CompareTo(serv2.ServiceName); });
            return true;
          }         
       }
       catch (System.ComponentModel.Win32Exception win32ex)
       {
         MessageBox.Show(win32ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
       }
       catch (Exception ex)
       {
         MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
       }

       return false;     
    }

  }
}
