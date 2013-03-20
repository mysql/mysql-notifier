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

namespace MySQL.Notifier
{
  partial class NewWindowsConnectionDialog
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewWindowsConnectionDialog));
      this.btnOK = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.Password = new System.Windows.Forms.TextBox();
      this.lblPassword = new System.Windows.Forms.Label();
      this.lblUser = new System.Windows.Forms.Label();
      this.lblConnection = new System.Windows.Forms.Label();
      this.lblEnterPassword = new System.Windows.Forms.Label();
      this.picLogo = new System.Windows.Forms.PictureBox();
      this.Username = new System.Windows.Forms.TextBox();
      this.Hostname = new System.Windows.Forms.TextBox();
      this.SavePasswordCheck = new System.Windows.Forms.CheckBox();
      ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
      this.SuspendLayout();
      // 
      // btnOK
      // 
      this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnOK.Location = new System.Drawing.Point(217, 173);
      this.btnOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 4;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnCancel.Location = new System.Drawing.Point(298, 173);
      this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // Password
      // 
      this.Password.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Password.Location = new System.Drawing.Point(141, 108);
      this.Password.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.Password.Name = "Password";
      this.Password.Size = new System.Drawing.Size(233, 23);
      this.Password.TabIndex = 2;
      this.Password.UseSystemPasswordChar = true;
      this.Password.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
      // 
      // lblPassword
      // 
      this.lblPassword.AutoSize = true;
      this.lblPassword.BackColor = System.Drawing.Color.Transparent;
      this.lblPassword.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblPassword.Location = new System.Drawing.Point(69, 111);
      this.lblPassword.Name = "lblPassword";
      this.lblPassword.Size = new System.Drawing.Size(60, 15);
      this.lblPassword.TabIndex = 5;
      this.lblPassword.Text = "Password:";
      // 
      // lblUser
      // 
      this.lblUser.AutoSize = true;
      this.lblUser.BackColor = System.Drawing.Color.Transparent;
      this.lblUser.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblUser.Location = new System.Drawing.Point(96, 82);
      this.lblUser.Name = "lblUser";
      this.lblUser.Size = new System.Drawing.Size(33, 15);
      this.lblUser.TabIndex = 3;
      this.lblUser.Text = "User:";
      // 
      // lblConnection
      // 
      this.lblConnection.AutoSize = true;
      this.lblConnection.BackColor = System.Drawing.Color.Transparent;
      this.lblConnection.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblConnection.Location = new System.Drawing.Point(94, 54);
      this.lblConnection.Name = "lblConnection";
      this.lblConnection.Size = new System.Drawing.Size(35, 15);
      this.lblConnection.TabIndex = 1;
      this.lblConnection.Text = "Host:";
      // 
      // lblEnterPassword
      // 
      this.lblEnterPassword.AutoSize = true;
      this.lblEnterPassword.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblEnterPassword.ForeColor = System.Drawing.Color.Navy;
      this.lblEnterPassword.Location = new System.Drawing.Point(92, 10);
      this.lblEnterPassword.MaximumSize = new System.Drawing.Size(320, 0);
      this.lblEnterPassword.Name = "lblEnterPassword";
      this.lblEnterPassword.Size = new System.Drawing.Size(269, 20);
      this.lblEnterPassword.TabIndex = 0;
      this.lblEnterPassword.Text = "Please enter the requested information:";
      // 
      // picLogo
      // 
      this.picLogo.Image = global::MySql.Notifier.Properties.Resources.NotifierWarningImage;
      this.picLogo.Location = new System.Drawing.Point(12, 10);
      this.picLogo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.picLogo.Name = "picLogo";
      this.picLogo.Size = new System.Drawing.Size(64, 64);
      this.picLogo.TabIndex = 22;
      this.picLogo.TabStop = false;
      // 
      // Username
      // 
      this.Username.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Username.Location = new System.Drawing.Point(141, 79);
      this.Username.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.Username.Name = "Username";
      this.Username.Size = new System.Drawing.Size(233, 23);
      this.Username.TabIndex = 1;
      // 
      // Hostname
      // 
      this.Hostname.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Hostname.Location = new System.Drawing.Point(141, 51);
      this.Hostname.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.Hostname.Name = "Hostname";
      this.Hostname.Size = new System.Drawing.Size(233, 23);
      this.Hostname.TabIndex = 0;
      // 
      // SavePasswordCheck
      // 
      this.SavePasswordCheck.AutoSize = true;
      this.SavePasswordCheck.Location = new System.Drawing.Point(141, 138);
      this.SavePasswordCheck.Name = "SavePasswordCheck";
      this.SavePasswordCheck.Size = new System.Drawing.Size(136, 17);
      this.SavePasswordCheck.TabIndex = 3;
      this.SavePasswordCheck.Text = "Save password in vault";
      this.SavePasswordCheck.UseVisualStyleBackColor = true;
      // 
      // NewWindowsConnectionDialog
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.BackColor = System.Drawing.SystemColors.Window;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(386, 204);
      this.Controls.Add(this.SavePasswordCheck);
      this.Controls.Add(this.Hostname);
      this.Controls.Add(this.Username);
      this.Controls.Add(this.picLogo);
      this.Controls.Add(this.lblEnterPassword);
      this.Controls.Add(this.Password);
      this.Controls.Add(this.lblConnection);
      this.Controls.Add(this.lblUser);
      this.Controls.Add(this.lblPassword);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnCancel);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "NewWindowsConnectionDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Setup New Windows connection";
      ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.TextBox Password;
    private System.Windows.Forms.Label lblConnection;
    private System.Windows.Forms.Label lblUser;
    private System.Windows.Forms.Label lblPassword;
    private System.Windows.Forms.Label lblEnterPassword;
    private System.Windows.Forms.PictureBox picLogo;
    private System.Windows.Forms.TextBox Username;
    private System.Windows.Forms.TextBox Hostname;
    private System.Windows.Forms.CheckBox SavePasswordCheck;
  }
}