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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WindowsConnectionDialog));
      this.DialogOKButton = new System.Windows.Forms.Button();
      this.CommandAreaPanel = new System.Windows.Forms.Panel();
      this.TestConnectionButton = new System.Windows.Forms.Button();
      this.DialogCancelButton = new System.Windows.Forms.Button();
      this.ContentAreaPanel = new System.Windows.Forms.Panel();
      this.HostTextbox = new System.Windows.Forms.TextBox();
      this.UserTextBox = new System.Windows.Forms.TextBox();
      this.LogoPictureBox = new System.Windows.Forms.PictureBox();
      this.HipertitleLabel = new System.Windows.Forms.Label();
      this.PasswordTextbox = new System.Windows.Forms.TextBox();
      this.HostLabel = new System.Windows.Forms.Label();
      this.UserLabel = new System.Windows.Forms.Label();
      this.PasswordLabel = new System.Windows.Forms.Label();
      this.CommandAreaPanel.SuspendLayout();
      this.ContentAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // DialogOKButton
      // 
      this.DialogOKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogOKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.DialogOKButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DialogOKButton.Location = new System.Drawing.Point(217, 15);
      this.DialogOKButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.DialogOKButton.Name = "DialogOKButton";
      this.DialogOKButton.Size = new System.Drawing.Size(75, 23);
      this.DialogOKButton.TabIndex = 1;
      this.DialogOKButton.Text = "OK";
      this.DialogOKButton.UseVisualStyleBackColor = true;
      this.DialogOKButton.Click += new System.EventHandler(this.OKButton_Click);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.BackColor = System.Drawing.SystemColors.Menu;
      this.CommandAreaPanel.Controls.Add(this.TestConnectionButton);
      this.CommandAreaPanel.Controls.Add(this.DialogCancelButton);
      this.CommandAreaPanel.Controls.Add(this.DialogOKButton);
      this.CommandAreaPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.CommandAreaPanel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 161);
      this.CommandAreaPanel.Name = "CommandAreaPanel";
      this.CommandAreaPanel.Size = new System.Drawing.Size(386, 50);
      this.CommandAreaPanel.TabIndex = 1;
      // 
      // TestConnectionButton
      // 
      this.TestConnectionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.TestConnectionButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TestConnectionButton.Location = new System.Drawing.Point(13, 15);
      this.TestConnectionButton.Name = "TestConnectionButton";
      this.TestConnectionButton.Size = new System.Drawing.Size(128, 23);
      this.TestConnectionButton.TabIndex = 0;
      this.TestConnectionButton.Text = "Test Connection";
      this.TestConnectionButton.UseVisualStyleBackColor = true;
      // 
      // DialogCancelButton
      // 
      this.DialogCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.DialogCancelButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DialogCancelButton.Location = new System.Drawing.Point(298, 15);
      this.DialogCancelButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.DialogCancelButton.Name = "DialogCancelButton";
      this.DialogCancelButton.Size = new System.Drawing.Size(75, 23);
      this.DialogCancelButton.TabIndex = 2;
      this.DialogCancelButton.Text = "Cancel";
      this.DialogCancelButton.UseVisualStyleBackColor = true;
      // 
      // ContentAreaPanel
      // 
      this.ContentAreaPanel.Controls.Add(this.HostTextbox);
      this.ContentAreaPanel.Controls.Add(this.UserTextBox);
      this.ContentAreaPanel.Controls.Add(this.LogoPictureBox);
      this.ContentAreaPanel.Controls.Add(this.HipertitleLabel);
      this.ContentAreaPanel.Controls.Add(this.PasswordTextbox);
      this.ContentAreaPanel.Controls.Add(this.HostLabel);
      this.ContentAreaPanel.Controls.Add(this.UserLabel);
      this.ContentAreaPanel.Controls.Add(this.PasswordLabel);
      this.ContentAreaPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ContentAreaPanel.Location = new System.Drawing.Point(0, 0);
      this.ContentAreaPanel.Name = "ContentAreaPanel";
      this.ContentAreaPanel.Size = new System.Drawing.Size(386, 211);
      this.ContentAreaPanel.TabIndex = 0;
      // 
      // HostTextbox
      // 
      this.HostTextbox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.HostTextbox.Location = new System.Drawing.Point(142, 61);
      this.HostTextbox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.HostTextbox.Name = "HostTextbox";
      this.HostTextbox.Size = new System.Drawing.Size(233, 23);
      this.HostTextbox.TabIndex = 2;
      // 
      // UserTextBox
      // 
      this.UserTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.UserTextBox.Location = new System.Drawing.Point(142, 89);
      this.UserTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.UserTextBox.Name = "UserTextBox";
      this.UserTextBox.Size = new System.Drawing.Size(233, 23);
      this.UserTextBox.TabIndex = 4;
      // 
      // LogoPictureBox
      // 
      this.LogoPictureBox.Image = global::MySql.Notifier.Properties.Resources.NotifierWarningImage;
      this.LogoPictureBox.Location = new System.Drawing.Point(13, 20);
      this.LogoPictureBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.LogoPictureBox.Name = "LogoPictureBox";
      this.LogoPictureBox.Size = new System.Drawing.Size(64, 64);
      this.LogoPictureBox.TabIndex = 31;
      this.LogoPictureBox.TabStop = false;
      // 
      // HipertitleLabel
      // 
      this.HipertitleLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.HipertitleLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.HipertitleLabel.Location = new System.Drawing.Point(88, 20);
      this.HipertitleLabel.MaximumSize = new System.Drawing.Size(320, 0);
      this.HipertitleLabel.Name = "HipertitleLabel";
      this.HipertitleLabel.Size = new System.Drawing.Size(290, 26);
      this.HipertitleLabel.TabIndex = 0;
      this.HipertitleLabel.Text = "Please enter the requested information:";
      this.HipertitleLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // PasswordTextbox
      // 
      this.PasswordTextbox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PasswordTextbox.Location = new System.Drawing.Point(142, 118);
      this.PasswordTextbox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.PasswordTextbox.Name = "PasswordTextbox";
      this.PasswordTextbox.Size = new System.Drawing.Size(233, 23);
      this.PasswordTextbox.TabIndex = 6;
      this.PasswordTextbox.UseSystemPasswordChar = true;
      // 
      // HostLabel
      // 
      this.HostLabel.AutoSize = true;
      this.HostLabel.BackColor = System.Drawing.Color.Transparent;
      this.HostLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.HostLabel.Location = new System.Drawing.Point(95, 64);
      this.HostLabel.Name = "HostLabel";
      this.HostLabel.Size = new System.Drawing.Size(35, 15);
      this.HostLabel.TabIndex = 1;
      this.HostLabel.Text = "Host:";
      // 
      // UserLabel
      // 
      this.UserLabel.AutoSize = true;
      this.UserLabel.BackColor = System.Drawing.Color.Transparent;
      this.UserLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.UserLabel.Location = new System.Drawing.Point(97, 92);
      this.UserLabel.Name = "UserLabel";
      this.UserLabel.Size = new System.Drawing.Size(33, 15);
      this.UserLabel.TabIndex = 3;
      this.UserLabel.Text = "User:";
      // 
      // PasswordLabel
      // 
      this.PasswordLabel.AutoSize = true;
      this.PasswordLabel.BackColor = System.Drawing.Color.Transparent;
      this.PasswordLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PasswordLabel.Location = new System.Drawing.Point(70, 121);
      this.PasswordLabel.Name = "PasswordLabel";
      this.PasswordLabel.Size = new System.Drawing.Size(60, 15);
      this.PasswordLabel.TabIndex = 5;
      this.PasswordLabel.Text = "Password:";
      // 
      // WindowsConnectionDialog
      // 
      this.AcceptButton = this.DialogOKButton;
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.BackColor = System.Drawing.SystemColors.Window;
      this.ClientSize = new System.Drawing.Size(386, 211);
      this.Controls.Add(this.CommandAreaPanel);
      this.Controls.Add(this.ContentAreaPanel);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "WindowsConnectionDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Windows Connection";
      this.CommandAreaPanel.ResumeLayout(false);
      this.ContentAreaPanel.ResumeLayout(false);
      this.ContentAreaPanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button DialogOKButton;
    private System.Windows.Forms.Button DialogCancelButton;
    private System.Windows.Forms.Panel ContentAreaPanel;
    private System.Windows.Forms.TextBox HostTextbox;
    private System.Windows.Forms.TextBox UserTextBox;
    private System.Windows.Forms.PictureBox LogoPictureBox;
    private System.Windows.Forms.Label HipertitleLabel;
    private System.Windows.Forms.TextBox PasswordTextbox;
    private System.Windows.Forms.Label HostLabel;
    private System.Windows.Forms.Label UserLabel;
    private System.Windows.Forms.Label PasswordLabel;
    private System.Windows.Forms.Panel CommandAreaPanel;
    private System.Windows.Forms.Button TestConnectionButton;
  }
}