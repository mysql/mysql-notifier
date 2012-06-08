using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using MySQL.Utility;

namespace MySql.Notifier
{
  public class FormBase : Form
  {
    private Font smallFont;
    private Font mediumFont;
    private Font largeFont;

    public FormBase()
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
  }
}
