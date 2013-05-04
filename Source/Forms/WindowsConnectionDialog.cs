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
  using MySql.Notifier.Properties;
  using MySQL.Utility;
  using MySQL.Utility.Forms;

  public partial class WindowsConnectionDialog : MachineAwareForm
  {
    public WindowsConnectionDialog()
    {
      InitializeComponent();
    }

    public WindowsConnectionDialog(MachinesList machineslist, Machine CurrentMachine)
    {
      InitializeComponent();

      if (CurrentMachine != null)
      {
        newMachine = CurrentMachine;
        HostTextbox.Text = (string.IsNullOrEmpty(CurrentMachine.Name) || CurrentMachine.Name == "localhost") ? String.Empty : CurrentMachine.Name;
        UserTextBox.Text = CurrentMachine.User ?? String.Empty;
        PasswordTextbox.Text = CurrentMachine.UnprotectedPassword ?? String.Empty;
      }
      if (machineslist != null)
      {
        if (machineslist.Machines != null)
        {
          machinesList = machineslist;
        }
      }
    }

    private void Button_Click(object sender, EventArgs e)
    {
      Cursor = Cursors.WaitCursor;
      DialogOKButton.Enabled = TestConnectionButton.Enabled = false;
      if (ValidateConnectionAndPermissions(sender.Equals(this.TestConnectionButton)))
      {
        if (sender.Equals(this.TestConnectionButton))
        {
          InfoDialog.ShowSuccessDialog(Resources.ConnectionSuccessfulTitle, Resources.ConnectionSuccessfulMessage);
        }
        else
        {
          DialogOKButton.DialogResult = this.DialogResult = DialogResult.OK;
        }
      }
      Textbox_TextChanged(sender, e);
      Cursor = Cursors.Default;
    }

    private void Textbox_TextChanged(object sender, EventArgs e)
    {
      DialogOKButton.Enabled = TestConnectionButton.Enabled = ValidateTextEntries();
    }

    private bool ValidateTextEntries()
    {
      //// Validate all textboxes are filled properly and the Host name is either a valid IP address or a machine name
      return (!String.IsNullOrEmpty(HostTextbox.Text) && !String.IsNullOrEmpty(UserTextBox.Text) && !String.IsNullOrEmpty(PasswordTextbox.Text) &&
        Regex.IsMatch(UserTextBox.Text, @"^[\w\.\-_]{1,64}$") && (Regex.IsMatch(HostTextbox.Text, @"^[\w\.\-_]{1,64}$") ||
        Regex.IsMatch(HostTextbox.Text, @"^([01]?[\d][\d]?|2[0-4][\d]|25[0-5])(\.([01]?[\d][\d]?|2[0-4][\d]|25[0-5])){3}$")));
    }

    private bool ValidateConnectionAndPermissions(bool OnlyTest)
    {
      bool result = false, machineAlreadyExist = false;
      if (!ValidateTextEntries())
      {
        return false;
      }

      newMachine = new Machine(HostTextbox.Text, UserTextBox.Text, PasswordTextbox.Text);
      newMachine.TestConnection(true, false);
      result = newMachine.IsOnline;

      if (!OnlyTest)
      {
        machineAlreadyExist = machinesList.GetMachineByHostName(newMachine.Name) != null;
      }

      if (machineAlreadyExist)
      {
        if (InfoDialog.ShowYesNoDialog(InfoDialog.InfoType.Warning, Resources.MachineAlreadyExistTitle, Resources.MachineAlreadyExistMessage) == DialogResult.Yes)
        {
          newMachine = machinesList.OverwriteMachine(newMachine);
        }
        else
        {
          result = false;
        }

        if (!result)
        {
          HostTextbox.Focus();
          HostTextbox.SelectAll();
          newMachine = null;
        }
      }

      return result;
    }
  }
}