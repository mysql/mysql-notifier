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
    private Utility.OSVersion OsVersion
    {
      get {
        return Utility.GetOsVersion();
      }
    }

    private Font smallFont
    {
      get
      {
        return new Font(OsVersion != Utility.OSVersion.WindowsXp ? "Segoe UI" : "Tahoma", 12, FontStyle.Regular, GraphicsUnit.Pixel, ((byte)(0)));       
      }      
    }
    
    private Font mediumFont
    {
      get
      {
        return new Font(OsVersion != Utility.OSVersion.WindowsXp ? "Segoe UI" : "Tahoma", 14, FontStyle.Regular, GraphicsUnit.Pixel, ((byte)(0)));       
      }     
    }

    private Font largeFont
    {
      get
      {
        return new Font(OsVersion != Utility.OSVersion.WindowsXp ? "Segoe UI" : "Tahoma", 16, FontStyle.Regular, GraphicsUnit.Pixel, ((byte)(0)));       
      }      
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      this.Font = smallFont;
      var controls = this.Controls;
      SettingControls(controls);
    }
  
    protected virtual void SettingControls(Control.ControlCollection controls)
    {
      if (controls == null || controls.Count == 0) return;
           
      foreach (Control c in controls)
      {                
        c.Font = smallFont;
        if (c is Label)
        {
          Label lbl = ((Label)c);
          lbl.BackColor = Color.Transparent;
          var name = lbl.Name.ToLower();
          if (name.Contains("hipertitle"))
          {
            lbl.Font = largeFont;
            lbl.ForeColor = Color.FromArgb(39, 73, 161);
          }
          if (name.Contains("lblsubtitle"))              
            lbl.Font = mediumFont;              
        }
        if (c is CheckBox)
        {
          CheckBox chk = (CheckBox)c;
          chk.BackColor = Color.Transparent;
        }
        SettingControls(c.Controls);
      }    
    } 
  }
}
