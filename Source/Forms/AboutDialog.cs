using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MySql.TrayApp
{
  public partial class AboutDialog : Form
  {
    public AboutDialog()
    {
      InitializeComponent();
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
