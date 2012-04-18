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

  
    private void About_Load(object sender, EventArgs e)
    {
      KeyPreview = true;
      KeyDown += new System.Windows.Forms.KeyEventHandler(About_KeyDown);
    }
   
    private void About_KeyDown(object sender, KeyEventArgs e)
    {
      if ((Keys)e.KeyValue == Keys.Escape)
      {
        Close();        
      }
    }
    
  }
}
