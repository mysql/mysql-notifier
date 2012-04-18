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
using System.Windows.Forms;
using System.Resources;
using System.Reflection;
using System.Drawing;
using System.Security.Principal;

namespace MySql.TrayApp
{
  class TrayApplicationContext : ApplicationContext
  {
    
    private bool disposed = false;
    private readonly TrayApp trayApp;
    
    /// <summary>
    /// This class should be created and passed into Application.Run( ... )
    /// </summary>
    public TrayApplicationContext()
    {
      WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
      bool hasAdminPrivileges = principal.IsInRole(WindowsBuiltInRole.Administrator);

      this.trayApp = new TrayApp(hasAdminPrivileges);
      this.trayApp.Exit += trayApp_Exit;
    }
    

    private void trayApp_Exit(object sender, EventArgs e)
    {
      this.ExitThread();
    }

    /// <summary>
    /// If we are presently showing a form, clean it up.
    /// </summary>
    protected override void ExitThreadCore()
    {
      base.ExitThreadCore();
    }

  }
}
