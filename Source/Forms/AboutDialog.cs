using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace MySql.Notifier
{
  public partial class AboutDialog : FormBase
  {
    private string[] version
    {
      get {
        return Assembly.GetExecutingAssembly().GetName().Version.ToString().Split('.');      
      }    
    }

    public AboutDialog()
    {      
      InitializeComponent();
      lblVersionSubTitle.Text = String.Format("{0}.{1}.{2}", version[0], version[1], version[2]);    
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
