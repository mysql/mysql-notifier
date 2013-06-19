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
  using System.Windows.Forms;

  /// <summary>
  /// Dumb form to workaround the fact that WIX sends a WM_CLOSE message to the application to attempt
  /// to close it upon uninstalling and the ApplicationContext needs a Form to receive the message and
  /// handle the closing of the application gracefully.
  /// </summary>
  public partial class DumbForm : Form
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="DumbForm"/> class.
    /// </summary>
    public DumbForm()
    {
      InitializeComponent();
    }

    protected override void OnLoad(System.EventArgs e)
    {
      Visible = false;
      ShowInTaskbar = false;
      base.OnLoad(e);
    }
  }
}