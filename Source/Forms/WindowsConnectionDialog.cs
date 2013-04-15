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
  using System;
  using System.Management;
  using System.Runtime.InteropServices;
  using System.Text.RegularExpressions;
  using System.Windows.Forms;

  public partial class WindowsConnectionDialog : MachineAwareForm
  {
    public WindowsConnectionDialog()
    {
      InitializeComponent();
    }

    //public WindowsConnectionDialog(AccountLogin CurrentLogin)
    public WindowsConnectionDialog(Machine CurrentMachine)
    {
      InitializeComponent();
      //TODO UNhardcode this ▼
      //if (CurrentMachine != null)
      //{
      //  RemoteMachine = CurrentMachine;
      //  HostTextbox.Text = CurrentMachine.Name ?? String.Empty;
      //  UserTextBox.Text = CurrentMachine.User ?? String.Empty;
      //  PasswordTextbox.Text = CurrentMachine.Password ?? String.Empty;
      //}              
        HostTextbox.Text = "VMWIN7X64";
        UserTextBox.Text = "Javier";
        PasswordTextbox.Text = ",.";
    }

    private void Button_Click(object sender, EventArgs e)
    {
      Cursor = Cursors.WaitCursor;
      DialogOKButton.Enabled = TestConnectionButton.Enabled = false;
      if (ValidateConnectionAndPermissions())
      {
        if (sender.Equals(this.TestConnectionButton))
        {
          MessageDialog dialog = new MessageDialog("Connection Successful", "You are able to monitor services in this remote machine.", false);
          dialog.ShowDialog();
        }
        else
        {
          DialogOKButton.DialogResult = this.DialogResult = DialogResult.OK;
        }
      }
      Textbox_TextChanged(sender, e);
      Cursor = Cursors.Arrow;
    }

    private void Textbox_TextChanged(object sender, EventArgs e)
    {
      DialogOKButton.Enabled = TestConnectionButton.Enabled = ValidateTextEntries();
    }

    private bool ValidateTextEntries()
    {
      //// Validate all textboxes are filled properly and the Host name is either a valid IP address or a machine name
      return (!String.IsNullOrEmpty(HostTextbox.Text) && !String.IsNullOrEmpty(UserTextBox.Text) && !String.IsNullOrEmpty(PasswordTextbox.Text) &&
        Regex.IsMatch(UserTextBox.Text, @"^[\w\.\-_]{1,64}$") && (Regex.IsMatch(HostTextbox.Text, @"^[\w\.\-_]{1,64}$") || Regex.IsMatch(HostTextbox.Text, @"^([01]?[\d][\d]?|2[0-4][\d]|25[0-5])(\.([01]?[\d][\d]?|2[0-4][\d]|25[0-5])){3}$")));
    }

    private bool ValidateConnectionAndPermissions()
    {
      MessageDialog dialog = null;
      bool result = false;
      try
      {
        if (!ValidateTextEntries())
        {
          return false;
        }

        RemoteMachine = new Machine(HostTextbox.Text, UserTextBox.Text, PasswordTextbox.Text);
        result = RemoteMachine.TestConnectionUnManaged();
      }
      catch (COMException ex)
      {
        dialog = new MessageDialog(ex.Message, "This message can be displayed for one of the following reasons at the remote machine: "
        + "Host is Offline, Incorrect WMI permission set, Firewall or DCOM settings.", true);
      }
      catch (UnauthorizedAccessException ex)
      {
        dialog = new MessageDialog(ex.Message, "This message is displayed because of an incorrect User or Password", true);
      }
      catch (ManagementException ex)
      {
        dialog = new MessageDialog(ex.Message, "This message is displayed because you are not allowed to browse services on the remote machine.", true);
      }
      finally
      {
        if (dialog != null)
        {
          dialog.ShowDialog();
          HostTextbox.SelectAll();
          RemoteMachine = null;         
        }
      }
      return result;
    }
  }
}