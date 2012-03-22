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
  public enum AvailableActions
  { Start, Stop, ReStart };
  
  /// <summary>
  /// Event Arguments used with the StatusChanged event containing information about the Service's status change.
  /// </summary>
  class ServiceStatusChangedArgs : EventArgs
  {
    #region Private Members

    private string serviceName;
    private ServiceControllerStatus currentStatus;
    private ServiceControllerStatus previousStatus;

    #endregion Private Members

    #region Public Properties

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

    #endregion Public Properties

    public ServiceStatusChangedArgs(string serviceName, ServiceControllerStatus previousStatus, ServiceControllerStatus currentStatus)
    {
      this.serviceName = serviceName;
      this.previousStatus = previousStatus;
      this.currentStatus = currentStatus;
    }
  }

  /// <summary>
  /// Represents an installed MySQL Service
  /// </summary>
  class MySQLService : IDisposable
  {
    #region Private Members

    private const int DEFAULT_TIMEOUT = 5000;
    private int timeoutMilliseconds = DEFAULT_TIMEOUT;
    private ServiceControllerStatus previousStatus;
    private bool disposed = false;
    private readonly bool hasAdminPrivileges = false;

    private ServiceController winService;
    private ServiceMenuGroup menuGroup;

    #endregion Private Members

    #region Public Properties

    public bool HasAdminPrivileges
    {
      get { return this.hasAdminPrivileges; }
    }

    public static int DefaultTimeOut
    {
      get { return DEFAULT_TIMEOUT; }
    }

    public int TimeOutMilliseconds
    {
      set { this.timeoutMilliseconds = value; }
      get { return this.timeoutMilliseconds; }
    }

    public string ServiceName
    {
      get { return this.winService.ServiceName; }
    }

    public ServiceMenuGroup MenuGroup
    {
      get { return this.menuGroup; }
    }

    public ServiceControllerStatus RefreshStatus
    {
      get
      {
        this.winService.Refresh();
        var newStatus = this.winService.Status;
        var copyPrevStatus = this.previousStatus;
        if (!this.previousStatus.Equals(newStatus))
        {
          this.previousStatus = newStatus;
          this.OnStatusChanged(new ServiceStatusChangedArgs(this.ServiceName, copyPrevStatus, newStatus));
        }
        return newStatus;
      }
    }

    public ServiceControllerStatus CurrentStatus
    {
      get { return this.winService.Status; }
    }

    public ServiceControllerStatus PreviousStatus
    {
      get { return this.previousStatus; }
    }

    #endregion Public Properties

    public MySQLService(string serviceName, bool adminPrivileges)
    {
      this.hasAdminPrivileges = adminPrivileges;
      this.winService = new ServiceController(serviceName);
      try
      {
        this.previousStatus = this.winService.Status;
        this.menuGroup = new ServiceMenuGroup(this);
      }
      catch (InvalidOperationException ioEx)
      {
        MessageBox.Show(ioEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        this.winService = null;
      }
    }

    #region Dispose Pattern Methods

    /// <summary>
    /// Cleans-up resources
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);
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
          if (this.winService != null)
            this.winService.Dispose();
          if (this.menuGroup != null)
            this.menuGroup.Dispose();
        }
      }
      this.disposed = true;
    }

    #endregion Dispose Pattern Methods

    #region Events

    public delegate void StatusChangedHandler(object sender, ServiceStatusChangedArgs args);

    /// <summary>
    /// Notifies that the status of the Windows Service has changed
    /// </summary>
    public event StatusChangedHandler StatusChanged;

    /// <summary>
    /// Notifies that this object is in the process of being disposed
    /// </summary>
    public event EventHandler Disposing;

    /// <summary>
    /// Invokes StatusChanged event when an action causes a status change
    /// </summary>
    /// <param name="e">Event arguments</param>
    protected virtual void OnStatusChanged(ServiceStatusChangedArgs args)
    {
      if (this.StatusChanged != null)
        this.StatusChanged(this, args);
    }

    /// <summary>
    /// Invokes Disposing event just before this object is disposed
    /// </summary>
    /// <param name="e">Event arguments</param>
    protected virtual void OnDisposing(EventArgs e)
    {
      if (this.Disposing != null)
        this.Disposing(this, e);
    }

    #endregion Events

    #region Action Methods

    /// <summary>
    /// Attempts to start the current MySQL Service
    /// </summary>
    /// <returns>Flag indicating if the action completed succesfully</returns>
    public bool Start()
    {
      if (this.winService == null)
        return false;

      bool success = false;
      try
      {
        TimeSpan timeout = TimeSpan.FromMilliseconds(this.timeoutMilliseconds);
        this.winService.Start();
        this.winService.WaitForStatus(ServiceControllerStatus.Running, timeout);
        this.previousStatus = this.RefreshStatus;
        success = true;
      }
      catch (ArgumentException argEx)
      {
        MessageBox.Show(argEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      catch (System.ServiceProcess.TimeoutException toEx)
      {
        MessageBox.Show(toEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      return success;
    }

    /// <summary>
    /// Attempts to stop the current MySQL Service
    /// </summary>
    /// <returns>Flag indicating if the action completed succesfully</returns>
    public bool Stop()
    {
      if (this.winService == null || !this.winService.CanStop)
        return false;

      bool success = false;
      try
      {
        TimeSpan timeout = TimeSpan.FromMilliseconds(this.timeoutMilliseconds);
        this.winService.Stop();
        this.winService.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
        success = true;
        this.previousStatus = this.RefreshStatus;
      }
      catch (ArgumentException argEx)
      {
        MessageBox.Show(argEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      catch (System.ServiceProcess.TimeoutException toEx)
      {
        MessageBox.Show(toEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      return success;
    }

    /// <summary>
    /// Attempts to stop and then start the current MySQL Service
    /// </summary>
    /// <returns>Flag indicating if the action completed succesfully</returns>
    public bool Restart()
    {
      if (this.winService == null)
        return false;

      bool success = false;
      try
      {
        int millisec1 = Environment.TickCount;
        TimeSpan timeout = TimeSpan.FromMilliseconds(this.timeoutMilliseconds);

        this.winService.Stop();
        this.winService.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

        // count the rest of the timeout
        int millisec2 = Environment.TickCount;
        timeout = TimeSpan.FromMilliseconds(this.timeoutMilliseconds - (millisec2 - millisec1));

        this.winService.Start();
        this.winService.WaitForStatus(ServiceControllerStatus.Running, timeout);

        success = true;
        this.previousStatus = this.RefreshStatus;
      }
      catch (ArgumentException argEx)
      {
        MessageBox.Show(argEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      catch (System.ServiceProcess.TimeoutException toEx)
      {
        MessageBox.Show(toEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      return success;
    }

    #endregion Action Methods

  }

  /// <summary>
  /// Event Arguments used with the ServicesListChanged event containing information about the change.
  /// </summary>
  class ServicesListChangedArgs : EventArgs
  {
    #region Private Members

    private ArrayList addedServicesList;
    private ArrayList removedServicesList;

    #endregion Private Members

    #region Public Properties

    public ArrayList AddedServicesList
    {
      get { return this.addedServicesList; }
    }

    public ArrayList RemovedServicesList
    {
      get { return this.removedServicesList; }
    }

    public int ChangedServicesCount
    {
      get { return this.addedServicesList.Count + this.removedServicesList.Count; }
    }

    #endregion Public Properties

    public ServicesListChangedArgs(ArrayList addedServices, ArrayList removedServices)
    {
      this.addedServicesList = addedServices;
      this.removedServicesList = removedServices;
    }
  }

  /// <summary>
  /// A list of MySQL Services
  /// </summary>
  class MySQLServicesList : IDisposable
  {
    #region Private Members

    private const string EXE_PATH_NAME = "mysqld";
    private const string EXE_PATH_NAME_NT = "mysqld-nt";

    private bool disposed = false;
    private readonly bool hasAdminPrivileges = false;
    private bool mySQLPrefixServicesOnly = true;
    private string scanForServicesPrefix;
    private List<MySQLService> installedMySQLServicesList = new List<MySQLService>();

    #endregion Private Members

    #region Public Properties

    public bool HasAdminPrivileges
    {
      get { return this.hasAdminPrivileges; }
    }

    public int InstalledMySQLServicesQuantity
    { 
      get { return this.installedMySQLServicesList.Count; } 
    }

    public bool MySQLPrefixServicesOnly
    {
      set { this.mySQLPrefixServicesOnly = value; }
      get { return this.mySQLPrefixServicesOnly; }
    }

    public string ScanForServicesPrefix
    {
      set { this.scanForServicesPrefix = value; }
      get { return this.scanForServicesPrefix; }
    }

    public List<MySQLService> InstalledMySQLServicesList
    {
      get { return this.installedMySQLServicesList; }
    }

    #endregion Public Properties

    public MySQLServicesList(bool adminPrivileges)
    {
      this.hasAdminPrivileges = adminPrivileges;
    }

    #region Dispose Pattern Methods

    /// <summary>
    /// Calls the Dispose method on the passed MySQLService object
    /// </summary>
    /// <param name="service"></param>
    private void DisposeService(MySQLService service)
    {
      service.Dispose();
    }

    /// <summary>
    /// Cleans-up resources
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);
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
          if (this.installedMySQLServicesList != null)
          {
            this.installedMySQLServicesList.ForEach(DisposeService);
            this.installedMySQLServicesList.Clear();
          }
        }
      }
      this.disposed = true;
    }

    #endregion Dispose Pattern Methods

    #region Events

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
      if (this.ServicesListChanged != null)
        this.ServicesListChanged(this, args);
    }

    public delegate void ServiceStatusChangedHandler(object sender, ServiceStatusChangedArgs args);

    /// <summary>
    /// Notifies that the status of one of the services in the list has changed
    /// </summary>
    public event ServiceStatusChangedHandler ServiceStatusChanged;

    protected virtual void OnServiceStatusChanged(ServiceStatusChangedArgs args)
    {
      if (this.ServiceStatusChanged != null)
        this.ServiceStatusChanged(this, args);
    }

    private void mySQLService_StatusChanged(object sender, ServiceStatusChangedArgs args)
    {
      this.OnServiceStatusChanged(args);
    }

    #endregion Events

    #region Methods

    /// <summary>
    /// Checks if the given Windows Service is a MySQL Service
    /// </summary>
    /// <param name="service">Windows Service</param>
    /// <returns></returns>
    private bool isMySQLService(ServiceController service)
    {
      bool isMySQLServiceFlag = false;

      if (this.MySQLPrefixServicesOnly)
        isMySQLServiceFlag =  service.ServiceName.StartsWith(this.scanForServicesPrefix, true, CultureInfo.CurrentCulture);
      else
      {
        ManagementClass mc = new ManagementClass("Win32_Service");
        foreach (ManagementObject mo in mc.GetInstances())
        {
          if (mo.GetPropertyValue("Name").ToString() == service.ServiceName)
          {
            string exePath = mo.GetPropertyValue("PathName").ToString();
            isMySQLServiceFlag = exePath.Contains(EXE_PATH_NAME) || exePath.Contains(EXE_PATH_NAME_NT);
            break;
          }
        }
      }

      return isMySQLServiceFlag;
    }

    /// <summary>
    /// Gets installed MySQL Windows Services
    /// </summary>
    /// <returns>List of installed MySQL Services</returns>
    private List<ServiceController> getListOfMySQLServices()
    {
      List<ServiceController> retList = new List<ServiceController>();
      try
      {
        var windowsServices = ServiceController.GetServices();
        if (windowsServices.Length <= 0)
          throw new Exception(Properties.Resources.NoServicesExceptionMessage);

        foreach (ServiceController wService in windowsServices)
        {
          if (isMySQLService(wService))
            retList.Add(wService);
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
      return retList;
    }

    /// <summary>
    /// Checks if there are differences among the installed MySQL Services and the given list of Windows Services
    /// </summary>
    /// <param name="windowsServices">List of installed Windows Services</param>
    /// <param name="addedServices"></param>
    /// <param name="removedServices"></param>
    /// <returns>Flag indicating if there are differences</returns>
    private bool serviceListsAreDifferent(List<ServiceController> windowsServices, out ArrayList addedServices, out ArrayList removedServices)
    {
      bool differencesFound = false;
      ServiceControllerStatus currentStatus;

      addedServices = new ArrayList();
      removedServices = new ArrayList();

      // Check for removed services (i.e. MySQL Services that are no more in the Windows Services refreshed list)
      foreach (MySQLService mService in this.installedMySQLServicesList)
      {
        currentStatus = mService.RefreshStatus;
        if (!windowsServices.Exists(delegate(ServiceController service) { return service.ServiceName == mService.ServiceName; }))
        {
          differencesFound = true;
          removedServices.Add(mService.ServiceName);
        }
      }

      // Check for added services (i.e. Windows Services that aren't in the existing MySQL Services list)
      foreach (ServiceController wService in windowsServices)
      {
        if (!this.installedMySQLServicesList.Exists(delegate(MySQLService service) { return service.ServiceName == wService.ServiceName; }))
        {
          differencesFound = true;
          addedServices.Add(wService.ServiceName);
        }
      }

      return differencesFound;
    }

    /// <summary>
    /// Checks if there are differences among the installed MySQL Services and the given list of Windows Services
    /// </summary>
    /// <param name="windowsServices">List of installed Windows Services</param>
    /// <returns>Flag indicating if there are differences</returns>
    private bool serviceListsAreDifferent(List<ServiceController> windowsServices)
    {
      ArrayList dummyAddedServicesList;
      ArrayList dummyRemovedServicesList;
      return serviceListsAreDifferent(windowsServices, out dummyAddedServicesList, out dummyRemovedServicesList);
    }

    /// <summary>
    /// Clears the list of MySQL Services and recreates it the installed MySQL Services
    /// </summary>
    /// <param name="force">Forces the hard refresh even when there are no changes in services</param>
    /// <returns>Flag indicating if the services changed</returns>
    public bool HardRefreshMySQLServices(bool force)
    {
      ArrayList addedServicesList;
      ArrayList removedServicesList;
      var windowsServices = this.getListOfMySQLServices();
      var differencesFound = this.serviceListsAreDifferent(windowsServices, out addedServicesList, out removedServicesList);
      MySQLService mySQLService;

      if (!differencesFound && !force)
        return differencesFound;

      if (this.installedMySQLServicesList.Count > 0)
      {
        this.installedMySQLServicesList.ForEach(DisposeService);
        this.installedMySQLServicesList.Clear();
      }
      foreach (ServiceController wService in windowsServices)
      {
        mySQLService = new MySQLService(wService.ServiceName, this.hasAdminPrivileges);
        mySQLService.StatusChanged += mySQLService_StatusChanged;
        this.installedMySQLServicesList.Add(mySQLService);
      }
      this.installedMySQLServicesList.Sort(delegate(MySQLService serv1, MySQLService serv2) { return serv1.ServiceName.CompareTo(serv2.ServiceName); });

      // If there were changes among services we want to notify subscribers about it
      if (differencesFound)
        this.OnServicesListChanged(new ServicesListChangedArgs(addedServicesList, removedServicesList));

      return differencesFound;
    }

    /// <summary>
    /// Refreshes the list of installed MySQL Services only adding or removing items if necessary
    /// </summary>
    /// <returns>Flag indicating if the services changed</returns>
    public bool SoftRefreshMySQLServices()
    {
      ArrayList addedServicesList;
      ArrayList removedServicesList;
      var windowsServices = this.getListOfMySQLServices();
      var differencesFound = this.serviceListsAreDifferent(windowsServices, out addedServicesList, out removedServicesList);
      MySQLService mySQLService;

      if (!differencesFound)
        return differencesFound;

      // Check for deleted services to remove them from the list
      foreach (MySQLService mService in this.installedMySQLServicesList)
      {
        if (!windowsServices.Exists(delegate(ServiceController service) { return service.ServiceName == mService.ServiceName; }))
          if (this.installedMySQLServicesList.Remove(mService))
            mService.Dispose();
      }

      // Check for new services to add them to the list
      foreach (ServiceController wService in windowsServices)
      {
        if (!this.installedMySQLServicesList.Exists(delegate(MySQLService service) { return service.ServiceName == wService.ServiceName; }))
        {
          mySQLService = new MySQLService(wService.ServiceName, this.hasAdminPrivileges);
          mySQLService.StatusChanged += mySQLService_StatusChanged;
          this.installedMySQLServicesList.Add(mySQLService);
        }
      }

      this.installedMySQLServicesList.Sort(delegate(MySQLService serv1, MySQLService serv2) { return serv1.ServiceName.CompareTo(serv2.ServiceName); });
      this.OnServicesListChanged(new ServicesListChangedArgs(addedServicesList, removedServicesList));

      return differencesFound;
    }

    #endregion Methods
  }
}
