// 
// Copyright (c) 2012-2013, Oracle and/or its affiliates. All rights reserved.
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
  partial class OptionsDialog
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsDialog));
      this.DialogCancelButton = new System.Windows.Forms.Button();
      this.DialogApplyButton = new System.Windows.Forms.Button();
      this.UseColorfulIconsCheckBox = new System.Windows.Forms.CheckBox();
      this.NotificationsOptionsLabel = new System.Windows.Forms.Label();
      this.NotifyOfStatusChangeCheckBox = new System.Windows.Forms.CheckBox();
      this.NotifyOfAutoAddCheckBox = new System.Windows.Forms.CheckBox();
      this.AutoAddRegexTextBox = new System.Windows.Forms.TextBox();
      this.AutoAddServicesCheckBox = new System.Windows.Forms.CheckBox();
      this.lblWeeks = new System.Windows.Forms.Label();
      this.CheckUpdatesWeeksNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.AutoCheckUpdatesCheckBox = new System.Windows.Forms.CheckBox();
      this.RunAtStartupCheckBox = new System.Windows.Forms.CheckBox();
      this.GeneralOptionsLabel = new System.Windows.Forms.Label();
      this.TitleLabel = new System.Windows.Forms.Label();
      this.ContentAreaPanel.SuspendLayout();
      this.CommandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.CheckUpdatesWeeksNumericUpDown)).BeginInit();
      this.SuspendLayout();
      // 
      // FootnoteAreaPanel
      // 
      this.FootnoteAreaPanel.Location = new System.Drawing.Point(0, 292);
      this.FootnoteAreaPanel.Size = new System.Drawing.Size(634, 0);
      // 
      // ContentAreaPanel
      // 
      this.ContentAreaPanel.Controls.Add(this.UseColorfulIconsCheckBox);
      this.ContentAreaPanel.Controls.Add(this.TitleLabel);
      this.ContentAreaPanel.Controls.Add(this.NotificationsOptionsLabel);
      this.ContentAreaPanel.Controls.Add(this.GeneralOptionsLabel);
      this.ContentAreaPanel.Controls.Add(this.NotifyOfStatusChangeCheckBox);
      this.ContentAreaPanel.Controls.Add(this.RunAtStartupCheckBox);
      this.ContentAreaPanel.Controls.Add(this.NotifyOfAutoAddCheckBox);
      this.ContentAreaPanel.Controls.Add(this.AutoCheckUpdatesCheckBox);
      this.ContentAreaPanel.Controls.Add(this.AutoAddRegexTextBox);
      this.ContentAreaPanel.Controls.Add(this.CheckUpdatesWeeksNumericUpDown);
      this.ContentAreaPanel.Controls.Add(this.AutoAddServicesCheckBox);
      this.ContentAreaPanel.Controls.Add(this.lblWeeks);
      this.ContentAreaPanel.Size = new System.Drawing.Size(422, 452);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.Controls.Add(this.DialogCancelButton);
      this.CommandAreaPanel.Controls.Add(this.DialogApplyButton);
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 407);
      this.CommandAreaPanel.Size = new System.Drawing.Size(422, 45);
      // 
      // DialogCancelButton
      // 
      this.DialogCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.DialogCancelButton.Location = new System.Drawing.Point(331, 10);
      this.DialogCancelButton.Name = "DialogCancelButton";
      this.DialogCancelButton.Size = new System.Drawing.Size(78, 23);
      this.DialogCancelButton.TabIndex = 1;
      this.DialogCancelButton.Text = "Cancel";
      this.DialogCancelButton.UseVisualStyleBackColor = true;
      // 
      // DialogApplyButton
      // 
      this.DialogApplyButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.DialogApplyButton.Location = new System.Drawing.Point(247, 11);
      this.DialogApplyButton.Name = "DialogApplyButton";
      this.DialogApplyButton.Size = new System.Drawing.Size(78, 23);
      this.DialogApplyButton.TabIndex = 0;
      this.DialogApplyButton.Text = "Apply";
      this.DialogApplyButton.UseVisualStyleBackColor = true;
      this.DialogApplyButton.Click += new System.EventHandler(this.DialogApplyButton_Click);
      // 
      // UseColorfulIconsCheckBox
      // 
      this.UseColorfulIconsCheckBox.AutoSize = true;
      this.UseColorfulIconsCheckBox.Location = new System.Drawing.Point(31, 101);
      this.UseColorfulIconsCheckBox.Name = "UseColorfulIconsCheckBox";
      this.UseColorfulIconsCheckBox.Size = new System.Drawing.Size(154, 19);
      this.UseColorfulIconsCheckBox.TabIndex = 2;
      this.UseColorfulIconsCheckBox.Text = "Use colorful status icons";
      this.UseColorfulIconsCheckBox.UseVisualStyleBackColor = true;
      // 
      // NotificationsOptionsLabel
      // 
      this.NotificationsOptionsLabel.AutoSize = true;
      this.NotificationsOptionsLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.NotificationsOptionsLabel.Location = new System.Drawing.Point(27, 287);
      this.NotificationsOptionsLabel.Name = "NotificationsOptionsLabel";
      this.NotificationsOptionsLabel.Size = new System.Drawing.Size(131, 17);
      this.NotificationsOptionsLabel.TabIndex = 9;
      this.NotificationsOptionsLabel.Text = "Notifications Options";
      // 
      // NotifyOfStatusChangeCheckBox
      // 
      this.NotifyOfStatusChangeCheckBox.AutoSize = true;
      this.NotifyOfStatusChangeCheckBox.Location = new System.Drawing.Point(31, 358);
      this.NotifyOfStatusChangeCheckBox.Name = "NotifyOfStatusChangeCheckBox";
      this.NotifyOfStatusChangeCheckBox.Size = new System.Drawing.Size(304, 19);
      this.NotifyOfStatusChangeCheckBox.TabIndex = 11;
      this.NotifyOfStatusChangeCheckBox.Text = "Notify me when a service or instance changes status.";
      this.NotifyOfStatusChangeCheckBox.UseVisualStyleBackColor = true;
      // 
      // NotifyOfAutoAddCheckBox
      // 
      this.NotifyOfAutoAddCheckBox.AutoSize = true;
      this.NotifyOfAutoAddCheckBox.Location = new System.Drawing.Point(31, 324);
      this.NotifyOfAutoAddCheckBox.Name = "NotifyOfAutoAddCheckBox";
      this.NotifyOfAutoAddCheckBox.Size = new System.Drawing.Size(284, 19);
      this.NotifyOfAutoAddCheckBox.TabIndex = 10;
      this.NotifyOfAutoAddCheckBox.Text = "Notify me when a service is automatically added.";
      this.NotifyOfAutoAddCheckBox.UseVisualStyleBackColor = true;
      // 
      // AutoAddRegexTextBox
      // 
      this.AutoAddRegexTextBox.Location = new System.Drawing.Point(59, 237);
      this.AutoAddRegexTextBox.Name = "AutoAddRegexTextBox";
      this.AutoAddRegexTextBox.Size = new System.Drawing.Size(300, 23);
      this.AutoAddRegexTextBox.TabIndex = 8;
      // 
      // AutoAddServicesCheckBox
      // 
      this.AutoAddServicesCheckBox.AutoSize = true;
      this.AutoAddServicesCheckBox.Location = new System.Drawing.Point(31, 211);
      this.AutoAddServicesCheckBox.Name = "AutoAddServicesCheckBox";
      this.AutoAddServicesCheckBox.Size = new System.Drawing.Size(316, 19);
      this.AutoAddServicesCheckBox.TabIndex = 7;
      this.AutoAddServicesCheckBox.Text = "Automatically add new services that match this pattern\r\n";
      this.AutoAddServicesCheckBox.UseVisualStyleBackColor = true;
      this.AutoAddServicesCheckBox.CheckedChanged += new System.EventHandler(this.AutoAddServicesCheckBox_CheckedChanged);
      // 
      // lblWeeks
      // 
      this.lblWeeks.AutoSize = true;
      this.lblWeeks.Location = new System.Drawing.Point(338, 173);
      this.lblWeeks.Name = "lblWeeks";
      this.lblWeeks.Size = new System.Drawing.Size(41, 15);
      this.lblWeeks.TabIndex = 6;
      this.lblWeeks.Text = "Weeks";
      // 
      // CheckUpdatesWeeksNumericUpDown
      // 
      this.CheckUpdatesWeeksNumericUpDown.Location = new System.Drawing.Point(278, 170);
      this.CheckUpdatesWeeksNumericUpDown.Name = "CheckUpdatesWeeksNumericUpDown";
      this.CheckUpdatesWeeksNumericUpDown.Size = new System.Drawing.Size(52, 23);
      this.CheckUpdatesWeeksNumericUpDown.TabIndex = 5;
      // 
      // AutoCheckUpdatesCheckBox
      // 
      this.AutoCheckUpdatesCheckBox.AutoSize = true;
      this.AutoCheckUpdatesCheckBox.Location = new System.Drawing.Point(31, 172);
      this.AutoCheckUpdatesCheckBox.Name = "AutoCheckUpdatesCheckBox";
      this.AutoCheckUpdatesCheckBox.Size = new System.Drawing.Size(233, 19);
      this.AutoCheckUpdatesCheckBox.TabIndex = 4;
      this.AutoCheckUpdatesCheckBox.Text = "Automatically Check For Updates Every";
      this.AutoCheckUpdatesCheckBox.UseVisualStyleBackColor = true;
      this.AutoCheckUpdatesCheckBox.CheckedChanged += new System.EventHandler(this.AutoCheckUpdatesCheckBox_CheckedChanged);
      // 
      // RunAtStartupCheckBox
      // 
      this.RunAtStartupCheckBox.AutoSize = true;
      this.RunAtStartupCheckBox.Location = new System.Drawing.Point(31, 136);
      this.RunAtStartupCheckBox.Name = "RunAtStartupCheckBox";
      this.RunAtStartupCheckBox.Size = new System.Drawing.Size(153, 19);
      this.RunAtStartupCheckBox.TabIndex = 3;
      this.RunAtStartupCheckBox.Text = "Run at Windows Startup";
      this.RunAtStartupCheckBox.UseVisualStyleBackColor = true;
      // 
      // GeneralOptionsLabel
      // 
      this.GeneralOptionsLabel.AutoSize = true;
      this.GeneralOptionsLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.GeneralOptionsLabel.Location = new System.Drawing.Point(27, 64);
      this.GeneralOptionsLabel.Name = "GeneralOptionsLabel";
      this.GeneralOptionsLabel.Size = new System.Drawing.Size(103, 17);
      this.GeneralOptionsLabel.TabIndex = 1;
      this.GeneralOptionsLabel.Text = "General Options";
      // 
      // TitleLabel
      // 
      this.TitleLabel.AutoSize = true;
      this.TitleLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TitleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(73)))), ((int)(((byte)(161)))));
      this.TitleLabel.Location = new System.Drawing.Point(24, 26);
      this.TitleLabel.Name = "TitleLabel";
      this.TitleLabel.Size = new System.Drawing.Size(177, 21);
      this.TitleLabel.TabIndex = 0;
      this.TitleLabel.Text = "MySQL Notifier Options";
      // 
      // OptionsDialog
      // 
      this.AcceptButton = this.DialogApplyButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.DialogCancelButton;
      this.ClientSize = new System.Drawing.Size(422, 452);
      this.CommandAreaVisible = true;
      this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
      this.FootnoteAreaHeight = 0;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "OptionsDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Options";
      this.ContentAreaPanel.ResumeLayout(false);
      this.ContentAreaPanel.PerformLayout();
      this.CommandAreaPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.CheckUpdatesWeeksNumericUpDown)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.CheckBox UseColorfulIconsCheckBox;
    private System.Windows.Forms.Label TitleLabel;
    private System.Windows.Forms.Label NotificationsOptionsLabel;
    private System.Windows.Forms.Label GeneralOptionsLabel;
    private System.Windows.Forms.CheckBox NotifyOfStatusChangeCheckBox;
    private System.Windows.Forms.CheckBox RunAtStartupCheckBox;
    private System.Windows.Forms.CheckBox NotifyOfAutoAddCheckBox;
    private System.Windows.Forms.CheckBox AutoCheckUpdatesCheckBox;
    private System.Windows.Forms.TextBox AutoAddRegexTextBox;
    private System.Windows.Forms.NumericUpDown CheckUpdatesWeeksNumericUpDown;
    private System.Windows.Forms.CheckBox AutoAddServicesCheckBox;
    private System.Windows.Forms.Label lblWeeks;
    private System.Windows.Forms.Button DialogCancelButton;
    private System.Windows.Forms.Button DialogApplyButton;

  }
}