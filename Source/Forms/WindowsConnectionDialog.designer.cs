// 
// Copyright (c) 2013 Oracle and/or its affiliates. All rights reserved.
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
  partial class WindowsConnectionDialog
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
        timerTextChanged.Dispose();
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WindowsConnectionDialog));
      this.timerTextChanged = new System.Windows.Forms.Timer(this.components);
      this.DialogOKButton = new System.Windows.Forms.Button();
      this.CommandAreaPanel = new System.Windows.Forms.Panel();
      this.TestConnectionButton = new System.Windows.Forms.Button();
      this.DialogCancelButton = new System.Windows.Forms.Button();
      this.ContentAreaPanel = new System.Windows.Forms.Panel();
      this.userErrorSign = new System.Windows.Forms.PictureBox();
      this.hostErrorSign = new System.Windows.Forms.PictureBox();
      this.HostTextBox = new System.Windows.Forms.TextBox();
      this.UserTextBox = new System.Windows.Forms.TextBox();
      this.LogoPictureBox = new System.Windows.Forms.PictureBox();
      this.HypertitleLabel = new System.Windows.Forms.Label();
      this.PasswordTextBox = new System.Windows.Forms.TextBox();
      this.HostLabel = new System.Windows.Forms.Label();
      this.UserLabel = new System.Windows.Forms.Label();
      this.PasswordLabel = new System.Windows.Forms.Label();
      this.CommandAreaPanel.SuspendLayout();
      this.ContentAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.userErrorSign)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.hostErrorSign)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // timerTextChanged
      // 
      this.timerTextChanged.Interval = 500;
      this.timerTextChanged.Tick += new System.EventHandler(this.timerTextChanged_Tick);
      // 
      // DialogOKButton
      // 
      this.DialogOKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogOKButton.Enabled = false;
      this.DialogOKButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DialogOKButton.Location = new System.Drawing.Point(256, 14);
      this.DialogOKButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.DialogOKButton.Name = "DialogOKButton";
      this.DialogOKButton.Size = new System.Drawing.Size(78, 23);
      this.DialogOKButton.TabIndex = 1;
      this.DialogOKButton.Text = "OK";
      this.DialogOKButton.UseVisualStyleBackColor = true;
      this.DialogOKButton.Click += new System.EventHandler(this.Button_Click);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.BackColor = System.Drawing.SystemColors.Menu;
      this.CommandAreaPanel.Controls.Add(this.TestConnectionButton);
      this.CommandAreaPanel.Controls.Add(this.DialogCancelButton);
      this.CommandAreaPanel.Controls.Add(this.DialogOKButton);
      this.CommandAreaPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.CommandAreaPanel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 157);
      this.CommandAreaPanel.Name = "CommandAreaPanel";
      this.CommandAreaPanel.Size = new System.Drawing.Size(425, 50);
      this.CommandAreaPanel.TabIndex = 1;
      // 
      // TestConnectionButton
      // 
      this.TestConnectionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.TestConnectionButton.Enabled = false;
      this.TestConnectionButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TestConnectionButton.Location = new System.Drawing.Point(12, 15);
      this.TestConnectionButton.Name = "TestConnectionButton";
      this.TestConnectionButton.Size = new System.Drawing.Size(128, 23);
      this.TestConnectionButton.TabIndex = 0;
      this.TestConnectionButton.Text = "Test Connection";
      this.TestConnectionButton.UseVisualStyleBackColor = true;
      this.TestConnectionButton.Click += new System.EventHandler(this.Button_Click);
      // 
      // DialogCancelButton
      // 
      this.DialogCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.DialogCancelButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DialogCancelButton.Location = new System.Drawing.Point(337, 14);
      this.DialogCancelButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.DialogCancelButton.Name = "DialogCancelButton";
      this.DialogCancelButton.Size = new System.Drawing.Size(78, 23);
      this.DialogCancelButton.TabIndex = 2;
      this.DialogCancelButton.Text = "Cancel";
      this.DialogCancelButton.UseVisualStyleBackColor = true;
      // 
      // ContentAreaPanel
      // 
      this.ContentAreaPanel.Controls.Add(this.userErrorSign);
      this.ContentAreaPanel.Controls.Add(this.hostErrorSign);
      this.ContentAreaPanel.Controls.Add(this.HostTextBox);
      this.ContentAreaPanel.Controls.Add(this.UserTextBox);
      this.ContentAreaPanel.Controls.Add(this.LogoPictureBox);
      this.ContentAreaPanel.Controls.Add(this.HypertitleLabel);
      this.ContentAreaPanel.Controls.Add(this.PasswordTextBox);
      this.ContentAreaPanel.Controls.Add(this.HostLabel);
      this.ContentAreaPanel.Controls.Add(this.UserLabel);
      this.ContentAreaPanel.Controls.Add(this.PasswordLabel);
      this.ContentAreaPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ContentAreaPanel.Location = new System.Drawing.Point(0, 0);
      this.ContentAreaPanel.Name = "ContentAreaPanel";
      this.ContentAreaPanel.Size = new System.Drawing.Size(425, 207);
      this.ContentAreaPanel.TabIndex = 0;
      // 
      // userErrorSign
      // 
      this.userErrorSign.Image = global::MySql.Notifier.Properties.Resources.InstallAvailableUpdatesIcon;
      this.userErrorSign.Location = new System.Drawing.Point(397, 83);
      this.userErrorSign.Name = "userErrorSign";
      this.userErrorSign.Size = new System.Drawing.Size(16, 16);
      this.userErrorSign.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.userErrorSign.TabIndex = 57;
      this.userErrorSign.TabStop = false;
      this.userErrorSign.Visible = false;
      // 
      // hostErrorSign
      // 
      this.hostErrorSign.Image = global::MySql.Notifier.Properties.Resources.InstallAvailableUpdatesIcon;
      this.hostErrorSign.Location = new System.Drawing.Point(397, 52);
      this.hostErrorSign.Name = "hostErrorSign";
      this.hostErrorSign.Size = new System.Drawing.Size(16, 16);
      this.hostErrorSign.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.hostErrorSign.TabIndex = 56;
      this.hostErrorSign.TabStop = false;
      this.hostErrorSign.Visible = false;
      // 
      // HostTextBox
      // 
      this.HostTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.HostTextBox.Location = new System.Drawing.Point(166, 49);
      this.HostTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.HostTextBox.Name = "HostTextBox";
      this.HostTextBox.Size = new System.Drawing.Size(225, 23);
      this.HostTextBox.TabIndex = 2;
      this.HostTextBox.TextChanged += new System.EventHandler(this.Textbox_TextChanged);
      this.HostTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.TextBox_Validating);
      // 
      // UserTextBox
      // 
      this.UserTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.UserTextBox.Location = new System.Drawing.Point(166, 80);
      this.UserTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.UserTextBox.Name = "UserTextBox";
      this.UserTextBox.Size = new System.Drawing.Size(225, 23);
      this.UserTextBox.TabIndex = 4;
      this.UserTextBox.TextChanged += new System.EventHandler(this.Textbox_TextChanged);
      this.UserTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.TextBox_Validating);
      // 
      // LogoPictureBox
      // 
      this.LogoPictureBox.Image = global::MySql.Notifier.Properties.Resources.ApplicationLogo;
      this.LogoPictureBox.Location = new System.Drawing.Point(19, 49);
      this.LogoPictureBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.LogoPictureBox.Name = "LogoPictureBox";
      this.LogoPictureBox.Size = new System.Drawing.Size(60, 60);
      this.LogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.LogoPictureBox.TabIndex = 31;
      this.LogoPictureBox.TabStop = false;
      // 
      // HypertitleLabel
      // 
      this.HypertitleLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.HypertitleLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.HypertitleLabel.Location = new System.Drawing.Point(92, 9);
      this.HypertitleLabel.Name = "HypertitleLabel";
      this.HypertitleLabel.Size = new System.Drawing.Size(303, 30);
      this.HypertitleLabel.TabIndex = 0;
      this.HypertitleLabel.Text = "Please enter the requested information:";
      this.HypertitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // PasswordTextBox
      // 
      this.PasswordTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PasswordTextBox.Location = new System.Drawing.Point(166, 111);
      this.PasswordTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.PasswordTextBox.Name = "PasswordTextBox";
      this.PasswordTextBox.Size = new System.Drawing.Size(225, 23);
      this.PasswordTextBox.TabIndex = 6;
      this.PasswordTextBox.UseSystemPasswordChar = true;
      this.PasswordTextBox.TextChanged += new System.EventHandler(this.Textbox_TextChanged);
      this.PasswordTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.TextBox_Validating);
      // 
      // HostLabel
      // 
      this.HostLabel.AutoSize = true;
      this.HostLabel.BackColor = System.Drawing.Color.Transparent;
      this.HostLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.HostLabel.Location = new System.Drawing.Point(92, 52);
      this.HostLabel.Name = "HostLabel";
      this.HostLabel.Size = new System.Drawing.Size(68, 15);
      this.HostLabel.TabIndex = 1;
      this.HostLabel.Text = "Host name:";
      // 
      // UserLabel
      // 
      this.UserLabel.AutoSize = true;
      this.UserLabel.BackColor = System.Drawing.Color.Transparent;
      this.UserLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.UserLabel.Location = new System.Drawing.Point(94, 83);
      this.UserLabel.Name = "UserLabel";
      this.UserLabel.Size = new System.Drawing.Size(66, 15);
      this.UserLabel.TabIndex = 3;
      this.UserLabel.Text = "User name:";
      // 
      // PasswordLabel
      // 
      this.PasswordLabel.AutoSize = true;
      this.PasswordLabel.BackColor = System.Drawing.Color.Transparent;
      this.PasswordLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PasswordLabel.Location = new System.Drawing.Point(100, 114);
      this.PasswordLabel.Name = "PasswordLabel";
      this.PasswordLabel.Size = new System.Drawing.Size(60, 15);
      this.PasswordLabel.TabIndex = 5;
      this.PasswordLabel.Text = "Password:";
      // 
      // WindowsConnectionDialog
      // 
      this.AcceptButton = this.DialogOKButton;
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
      this.BackColor = System.Drawing.SystemColors.Window;
      this.ClientSize = new System.Drawing.Size(425, 207);
      this.Controls.Add(this.CommandAreaPanel);
      this.Controls.Add(this.ContentAreaPanel);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "WindowsConnectionDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Add New Machine";
      this.CommandAreaPanel.ResumeLayout(false);
      this.ContentAreaPanel.ResumeLayout(false);
      this.ContentAreaPanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.userErrorSign)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.hostErrorSign)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button DialogOKButton;
    private System.Windows.Forms.Button DialogCancelButton;
    private System.Windows.Forms.Panel ContentAreaPanel;
    private System.Windows.Forms.TextBox HostTextBox;
    private System.Windows.Forms.TextBox UserTextBox;
    private System.Windows.Forms.PictureBox LogoPictureBox;
    private System.Windows.Forms.Label HypertitleLabel;
    private System.Windows.Forms.TextBox PasswordTextBox;
    private System.Windows.Forms.Label HostLabel;
    private System.Windows.Forms.Label UserLabel;
    private System.Windows.Forms.Label PasswordLabel;
    private System.Windows.Forms.Panel CommandAreaPanel;
    private System.Windows.Forms.Button TestConnectionButton;
    private System.Windows.Forms.Timer timerTextChanged;
    private System.Windows.Forms.PictureBox userErrorSign;
    private System.Windows.Forms.PictureBox hostErrorSign;
  }
}