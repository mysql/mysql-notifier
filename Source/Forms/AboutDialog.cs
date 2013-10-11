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

using System;
using System.Reflection;
using System.Windows.Forms;
using MySQL.Utility.Classes;

namespace MySql.Notifier.Forms
{
  public partial class AboutDialog : Form
  {
    private string[] Version
    {
      get
      {
        return Assembly.GetExecutingAssembly().GetName().Version.ToString().Split('.');
      }
    }

    public AboutDialog()
    {
      InitializeComponent();
      NotifierVersionLabel.Text = string.Format("{0} {1}.{2}.{3}", AssemblyInfo.AssemblyTitle, Version[0], Version[1], Version[2]);
    }

    private void AboutDialog_Load(object sender, EventArgs e)
    {
      KeyPreview = true;
      KeyDown += new System.Windows.Forms.KeyEventHandler(AboutDialog_KeyDown);
    }

    private void AboutDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if ((Keys)e.KeyValue == Keys.Escape)
      {
        Close();
      }
    }

    private void AboutDialog_MouseClick(object sender, MouseEventArgs e)
    {
      Close();
    }
  }
}