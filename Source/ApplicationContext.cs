//
// Copyright (c) 2012-2013, Oracle and/or its affiliates. All rights reserved.
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
using System.Security.Principal;
using System.Windows.Forms;

namespace MySql.Notifier
{
    internal class NotifierApplicationContext : ApplicationContext
    {
        private readonly Notifier notifierApp;

        /// <summary>
        /// This class should be created and passed into Application.Run( ... )
        /// </summary>
        public NotifierApplicationContext()
        {
            WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            bool hasAdminPrivileges = principal.IsInRole(WindowsBuiltInRole.Administrator);

            this.notifierApp = new Notifier();
            this.notifierApp.Exit += NotifierApp_Exit;
        }

        private void NotifierApp_Exit(object sender, EventArgs e)
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