using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MySql.Notifier
{
  public partial class MessageDialog : FormBase
  {
    public MessageDialog(string errorSummary, string errorDetails, bool highSeverity = false)
    {
      InitializeComponent();
      picLogo.Image = highSeverity ? Properties.Resources.NotifierErrorImage : Properties.Resources.NotifierWarningImage;
      lblOperationSummary.Text = errorSummary;
      lblOperationDetails.Text = errorDetails;
    }
  }
}
