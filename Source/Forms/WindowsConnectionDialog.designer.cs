// Copyright (c) 2013, 2019, Oracle and/or its affiliates. All rights reserved.
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

namespace MySql.Notifier.Forms
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
      this.TestConnectionButton = new System.Windows.Forms.Button();
      this.DialogCancelButton = new System.Windows.Forms.Button();
      this.MachineAutoTestConnectionIntervalUOMComboBox = new System.Windows.Forms.ComboBox();
      this.MachineAutoTestConnectionLabel = new System.Windows.Forms.Label();
      this.MachineAutoTestConnectionIntervalNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.userErrorSign = new System.Windows.Forms.PictureBox();
      this.hostErrorSign = new System.Windows.Forms.PictureBox();
      this.HostTextBox = new System.Windows.Forms.TextBox();
      this.UserTextBox = new System.Windows.Forms.TextBox();
      this.LogoPictureBox = new System.Windows.Forms.PictureBox();
      this.TitleLabel = new System.Windows.Forms.Label();
      this.PasswordTextBox = new System.Windows.Forms.TextBox();
      this.HostLabel = new System.Windows.Forms.Label();
      this.UserLabel = new System.Windows.Forms.Label();
      this.PasswordLabel = new System.Windows.Forms.Label();
      this.errorToolTip = new System.Windows.Forms.ToolTip(this.components);
      this.ContentAreaPanel.SuspendLayout();
      this.CommandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.MachineAutoTestConnectionIntervalNumericUpDown)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.userErrorSign)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.hostErrorSign)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // FootnoteAreaPanel
      // 
      this.FootnoteAreaPanel.BackColor = System.Drawing.SystemColors.Control;
      this.FootnoteAreaPanel.Location = new System.Drawing.Point(0, 292);
      this.FootnoteAreaPanel.Size = new System.Drawing.Size(634, 0);
      // 
      // ContentAreaPanel
      // 
      this.ContentAreaPanel.Controls.Add(this.MachineAutoTestConnectionIntervalUOMComboBox);
      this.ContentAreaPanel.Controls.Add(this.MachineAutoTestConnectionLabel);
      this.ContentAreaPanel.Controls.Add(this.MachineAutoTestConnectionIntervalNumericUpDown);
      this.ContentAreaPanel.Controls.Add(this.userErrorSign);
      this.ContentAreaPanel.Controls.Add(this.hostErrorSign);
      this.ContentAreaPanel.Controls.Add(this.HostTextBox);
      this.ContentAreaPanel.Controls.Add(this.UserTextBox);
      this.ContentAreaPanel.Controls.Add(this.LogoPictureBox);
      this.ContentAreaPanel.Controls.Add(this.TitleLabel);
      this.ContentAreaPanel.Controls.Add(this.PasswordTextBox);
      this.ContentAreaPanel.Controls.Add(this.HostLabel);
      this.ContentAreaPanel.Controls.Add(this.UserLabel);
      this.ContentAreaPanel.Controls.Add(this.PasswordLabel);
      this.ContentAreaPanel.Size = new System.Drawing.Size(427, 237);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.BackColor = System.Drawing.SystemColors.Control;
      this.CommandAreaPanel.Controls.Add(this.TestConnectionButton);
      this.CommandAreaPanel.Controls.Add(this.DialogCancelButton);
      this.CommandAreaPanel.Controls.Add(this.DialogOKButton);
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 192);
      this.CommandAreaPanel.Size = new System.Drawing.Size(427, 45);
      // 
      // timerTextChanged
      // 
      this.timerTextChanged.Interval = 500;
      this.timerTextChanged.Tick += new System.EventHandler(this.timerTextChanged_Tick);
      // 
      // DialogOKButton
      // 
      this.DialogOKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogOKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.DialogOKButton.Enabled = false;
      this.DialogOKButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DialogOKButton.Location = new System.Drawing.Point(253, 11);
      this.DialogOKButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.DialogOKButton.Name = "DialogOKButton";
      this.DialogOKButton.Size = new System.Drawing.Size(78, 23);
      this.DialogOKButton.TabIndex = 1;
      this.DialogOKButton.Text = "OK";
      this.DialogOKButton.UseVisualStyleBackColor = true;
      // 
      // TestConnectionButton
      // 
      this.TestConnectionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.TestConnectionButton.Enabled = false;
      this.TestConnectionButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TestConnectionButton.Location = new System.Drawing.Point(12, 11);
      this.TestConnectionButton.Name = "TestConnectionButton";
      this.TestConnectionButton.Size = new System.Drawing.Size(128, 23);
      this.TestConnectionButton.TabIndex = 0;
      this.TestConnectionButton.Text = "Test Connection";
      this.TestConnectionButton.UseVisualStyleBackColor = true;
      this.TestConnectionButton.Click += new System.EventHandler(this.TestConnectionButton_Click);
      // 
      // DialogCancelButton
      // 
      this.DialogCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.DialogCancelButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DialogCancelButton.Location = new System.Drawing.Point(337, 11);
      this.DialogCancelButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.DialogCancelButton.Name = "DialogCancelButton";
      this.DialogCancelButton.Size = new System.Drawing.Size(78, 23);
      this.DialogCancelButton.TabIndex = 2;
      this.DialogCancelButton.Text = "Cancel";
      this.DialogCancelButton.UseVisualStyleBackColor = true;
      // 
      // MachineAutoTestConnectionIntervalUOMComboBox
      // 
      this.MachineAutoTestConnectionIntervalUOMComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.MachineAutoTestConnectionIntervalUOMComboBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.MachineAutoTestConnectionIntervalUOMComboBox.Items.AddRange(new object[] {
            "seconds",
            "minutes",
            "hours",
            "days"});
      this.MachineAutoTestConnectionIntervalUOMComboBox.Location = new System.Drawing.Point(307, 147);
      this.MachineAutoTestConnectionIntervalUOMComboBox.Name = "MachineAutoTestConnectionIntervalUOMComboBox";
      this.MachineAutoTestConnectionIntervalUOMComboBox.Size = new System.Drawing.Size(88, 23);
      this.MachineAutoTestConnectionIntervalUOMComboBox.TabIndex = 9;
      this.MachineAutoTestConnectionIntervalUOMComboBox.SelectedIndexChanged += new System.EventHandler(this.MachineAutoTestConnectionIntervalUOMComboBox_SelectedIndexChanged);
      // 
      // MachineAutoTestConnectionLabel
      // 
      this.MachineAutoTestConnectionLabel.AutoSize = true;
      this.MachineAutoTestConnectionLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.MachineAutoTestConnectionLabel.Location = new System.Drawing.Point(17, 150);
      this.MachineAutoTestConnectionLabel.Name = "MachineAutoTestConnectionLabel";
      this.MachineAutoTestConnectionLabel.Size = new System.Drawing.Size(223, 15);
      this.MachineAutoTestConnectionLabel.TabIndex = 7;
      this.MachineAutoTestConnectionLabel.Text = "Check computer connection status every";
      // 
      // MachineAutoTestConnectionIntervalNumericUpDown
      // 
      this.MachineAutoTestConnectionIntervalNumericUpDown.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.MachineAutoTestConnectionIntervalNumericUpDown.Location = new System.Drawing.Point(246, 147);
      this.MachineAutoTestConnectionIntervalNumericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
      this.MachineAutoTestConnectionIntervalNumericUpDown.Name = "MachineAutoTestConnectionIntervalNumericUpDown";
      this.MachineAutoTestConnectionIntervalNumericUpDown.Size = new System.Drawing.Size(51, 23);
      this.MachineAutoTestConnectionIntervalNumericUpDown.TabIndex = 8;
      this.MachineAutoTestConnectionIntervalNumericUpDown.ValueChanged += new System.EventHandler(this.MachineAutoTestConnectionIntervalNumericUpDown_ValueChanged);
      // 
      // userErrorSign
      // 
      this.userErrorSign.Image = global::MySql.Notifier.Properties.Resources.InstallAvailableUpdatesIcon;
      this.userErrorSign.Location = new System.Drawing.Point(401, 83);
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
      this.hostErrorSign.Location = new System.Drawing.Point(401, 52);
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
      this.HostTextBox.Location = new System.Drawing.Point(170, 49);
      this.HostTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.HostTextBox.Name = "HostTextBox";
      this.HostTextBox.Size = new System.Drawing.Size(225, 23);
      this.HostTextBox.TabIndex = 2;
      this.HostTextBox.TextChanged += new System.EventHandler(this.TextChangedHandler);
      this.HostTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.ValidatingHandler);
      // 
      // UserTextBox
      // 
      this.UserTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.UserTextBox.Location = new System.Drawing.Point(170, 80);
      this.UserTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.UserTextBox.Name = "UserTextBox";
      this.UserTextBox.Size = new System.Drawing.Size(225, 23);
      this.UserTextBox.TabIndex = 4;
      this.UserTextBox.TextChanged += new System.EventHandler(this.TextChangedHandler);
      this.UserTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.ValidatingHandler);
      // 
      // LogoPictureBox
      // 
      this.LogoPictureBox.Image = global::MySql.Notifier.Properties.Resources.ApplicationLogo;
      this.LogoPictureBox.Location = new System.Drawing.Point(20, 49);
      this.LogoPictureBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.LogoPictureBox.Name = "LogoPictureBox";
      this.LogoPictureBox.Size = new System.Drawing.Size(64, 64);
      this.LogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.LogoPictureBox.TabIndex = 31;
      this.LogoPictureBox.TabStop = false;
      // 
      // TitleLabel
      // 
      this.TitleLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TitleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(73)))), ((int)(((byte)(161)))));
      this.TitleLabel.Location = new System.Drawing.Point(72, 9);
      this.TitleLabel.Name = "TitleLabel";
      this.TitleLabel.Size = new System.Drawing.Size(303, 30);
      this.TitleLabel.TabIndex = 0;
      this.TitleLabel.Text = "Please enter the requested information:";
      this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // PasswordTextBox
      // 
      this.PasswordTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PasswordTextBox.Location = new System.Drawing.Point(170, 111);
      this.PasswordTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.PasswordTextBox.Name = "PasswordTextBox";
      this.PasswordTextBox.Size = new System.Drawing.Size(225, 23);
      this.PasswordTextBox.TabIndex = 6;
      this.PasswordTextBox.UseSystemPasswordChar = true;
      this.PasswordTextBox.TextChanged += new System.EventHandler(this.TextChangedHandler);
      this.PasswordTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.ValidatingHandler);
      // 
      // HostLabel
      // 
      this.HostLabel.AutoSize = true;
      this.HostLabel.BackColor = System.Drawing.Color.Transparent;
      this.HostLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.HostLabel.Location = new System.Drawing.Point(96, 52);
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
      this.UserLabel.Location = new System.Drawing.Point(98, 83);
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
      this.PasswordLabel.Location = new System.Drawing.Point(104, 114);
      this.PasswordLabel.Name = "PasswordLabel";
      this.PasswordLabel.Size = new System.Drawing.Size(60, 15);
      this.PasswordLabel.TabIndex = 5;
      this.PasswordLabel.Text = "Password:";
      // 
      // errorToolTip
      // 
      this.errorToolTip.AutoPopDelay = 5000;
      this.errorToolTip.InitialDelay = 300;
      this.errorToolTip.ReshowDelay = 100;
      // 
      // WindowsConnectionDialog
      // 
      this.AcceptButton = this.DialogOKButton;
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
      this.BackColor = System.Drawing.SystemColors.Window;
      this.CancelButton = this.DialogCancelButton;
      this.ClientSize = new System.Drawing.Size(427, 237);
      this.CommandAreaVisible = true;
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
      this.FootnoteAreaHeight = 0;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "WindowsConnectionDialog";
      this.Text = "Add New Machine";
      this.ContentAreaPanel.ResumeLayout(false);
      this.ContentAreaPanel.PerformLayout();
      this.CommandAreaPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.MachineAutoTestConnectionIntervalNumericUpDown)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.userErrorSign)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.hostErrorSign)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button DialogOKButton;
    private System.Windows.Forms.Button DialogCancelButton;
    private System.Windows.Forms.TextBox HostTextBox;
    private System.Windows.Forms.TextBox UserTextBox;
    private System.Windows.Forms.PictureBox LogoPictureBox;
    private System.Windows.Forms.Label TitleLabel;
    private System.Windows.Forms.TextBox PasswordTextBox;
    private System.Windows.Forms.Label HostLabel;
    private System.Windows.Forms.Label UserLabel;
    private System.Windows.Forms.Label PasswordLabel;
    private System.Windows.Forms.Button TestConnectionButton;
    private System.Windows.Forms.Timer timerTextChanged;
    private System.Windows.Forms.PictureBox userErrorSign;
    private System.Windows.Forms.PictureBox hostErrorSign;
    private System.Windows.Forms.ComboBox MachineAutoTestConnectionIntervalUOMComboBox;
    private System.Windows.Forms.Label MachineAutoTestConnectionLabel;
    private System.Windows.Forms.NumericUpDown MachineAutoTestConnectionIntervalNumericUpDown;
    private System.Windows.Forms.ToolTip errorToolTip;
  }
}