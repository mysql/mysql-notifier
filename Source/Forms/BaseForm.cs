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
using System.Drawing;
using System.Windows.Forms;
using MySQL.Utility;

namespace MySql.Notifier
{
  public class BaseForm : Form
  {
    private Font smallFont;
    private Font mediumFont;
    private Font largeFont;

    public BaseForm()
    {
      Utility.OSVersion v = Utility.GetOsVersion();
      string fontName = v != Utility.OSVersion.WindowsXp ? "Segoe UI" : "Tahoma";
      smallFont = new Font(fontName, 12, FontStyle.Regular, GraphicsUnit.Pixel, ((byte)(0)));
      mediumFont = new Font(fontName, 14, FontStyle.Regular, GraphicsUnit.Pixel, ((byte)(0)));
      largeFont = new Font(fontName, 16, FontStyle.Regular, GraphicsUnit.Pixel, ((byte)(0)));
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      Font = smallFont;
      SettingControls(Controls);
    }

    protected virtual void SettingControls(Control.ControlCollection controls)
    {
      if (controls == null || controls.Count == 0) return;

      foreach (Control c in controls)
      {
        c.Font = smallFont;
        if (c is CheckBox)
          c.BackColor = Color.Transparent;
        else if (c is Label)
        {
          c.BackColor = Color.Transparent;
          string name = c.Name.ToLower();
          if (name.Contains("hipertitle"))
          {
            c.Font = largeFont;
            c.ForeColor = Color.FromArgb(39, 73, 161);
          }
          else if (name.Contains("lblsubtitle"))
            c.Font = mediumFont;
        }
        SettingControls(c.Controls);
      }
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();
      // 
      // BaseForm
      // 
      this.ClientSize = new System.Drawing.Size(284, 262);
      this.Name = "BaseForm";
      this.ResumeLayout(false);

    }
  }

  public class LoginGathererForm : BaseForm
  {
    public AccountLogin Login { get; set; }
  }
}