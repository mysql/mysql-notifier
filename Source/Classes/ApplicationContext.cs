// Copyright (c) 2012, 2019, Oracle and/or its affiliates. All rights reserved.
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

using System;
using System.Security.Principal;
using System.Windows.Forms;
using MySql.Notifier.Forms;
using MySql.Utility.Classes.Logging;

namespace MySql.Notifier.Classes
{
  internal class NotifierApplicationContext : ApplicationContext
  {
    private bool _disposeDone;

    /// <summary>
    /// This class should be created and passed into Application.Run( ... )
    /// </summary>
    public NotifierApplicationContext()
    {
      _disposeDone = false;
      NotifierInstance = new Notifier();
      MainForm = new DumbForm();
      NotifierInstance.Exit += NotifierApp_Exit;
    }

    #region Properties

    /// <summary>
    /// Gets an instance of the <see cref="Notifier"/> class.
    /// </summary>
    internal Notifier NotifierInstance { get; }

    /// <summary>
    /// Gets a value indicating whether the user running this application has administrator privileges.
    /// </summary>
    public bool HasAdminPrivileges
    {
      get
      {
        var isAdmin = false;
        try
        {
          var identity = WindowsIdentity.GetCurrent();
          var principal = new WindowsPrincipal(identity);
          isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        catch (Exception ex)
        {
          Logger.LogException(ex, true);
        }

        return isAdmin;
      }
    }

    #endregion Properties

    protected override void OnMainFormClosed(object sender, EventArgs e)
    {
      NotifierInstance.ForceExit();
      base.OnMainFormClosed(sender, e);
    }

    private void NotifierApp_Exit(object sender, EventArgs e)
    {
      Dispose(true);
      ExitThread();
    }

    /// <summary>
    /// If we are presently showing a form, clean it up.
    /// </summary>
    protected override void ExitThreadCore()
    {
      Dispose(true);
      base.ExitThreadCore();
    }

    /// <summary>
    /// Releases all resources used by the <see cref="NotifierApplicationContext"/> class
    /// </summary>
    protected override void Dispose(bool disposing)
    {
      if (_disposeDone)
      {
        return;
      }

      if (disposing)
      {
        // Free managed resources
        MainForm.Dispose();
        NotifierInstance.Dispose();
        GC.SuppressFinalize(this);
      }

      // Add class finalizer if unmanaged resources are added to the class
      // Free unmanaged resources if there are any
      _disposeDone = true;
    }
  }
}