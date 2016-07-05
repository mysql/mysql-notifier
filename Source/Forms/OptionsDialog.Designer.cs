// Copyright (c) 2012, 2016, Oracle and/or its affiliates. All rights reserved.
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
      if (disposing)
      {
        if (components != null)
        {
          components.Dispose();
        }
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsDialog));
      this.DialogCancelButton = new System.Windows.Forms.Button();
      this.DialogAcceptButton = new System.Windows.Forms.Button();
      this.UseColorfulIconsCheckBox = new System.Windows.Forms.CheckBox();
      this.NotificationsOptionsLabel = new System.Windows.Forms.Label();
      this.NotifyOfStatusChangeCheckBox = new System.Windows.Forms.CheckBox();
      this.NotifyOfAutoAddCheckBox = new System.Windows.Forms.CheckBox();
      this.AutoAddRegexTextBox = new System.Windows.Forms.TextBox();
      this.AutoAddServicesCheckBox = new System.Windows.Forms.CheckBox();
      this.WeeksLabel = new System.Windows.Forms.Label();
      this.CheckUpdatesWeeksNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.AutoCheckUpdatesCheckBox = new System.Windows.Forms.CheckBox();
      this.RunAtStartupCheckBox = new System.Windows.Forms.CheckBox();
      this.GeneralOptionsLabel = new System.Windows.Forms.Label();
      this.TitleLabel = new System.Windows.Forms.Label();
      this.HelpToolTip = new System.Windows.Forms.ToolTip(this.components);
      this.MigrateWorkbenchConnectionsButton = new System.Windows.Forms.Button();
      this.PingMonitoredInstancesNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.ConnectionsOptionsLabel = new System.Windows.Forms.Label();
      this.ResetToDefaultsButton = new System.Windows.Forms.Button();
      this.AutomaticMigrationDelayLabel = new System.Windows.Forms.Label();
      this.AutomaticMigrationDelayValueLabel = new System.Windows.Forms.Label();
      this.PingMonitoredInstancesLabel = new System.Windows.Forms.Label();
      this.SecondsLabel = new System.Windows.Forms.Label();
      this.ContentAreaPanel.SuspendLayout();
      this.CommandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.CheckUpdatesWeeksNumericUpDown)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.PingMonitoredInstancesNumericUpDown)).BeginInit();
      this.SuspendLayout();
      // 
      // FootnoteAreaPanel
      // 
      this.FootnoteAreaPanel.Location = new System.Drawing.Point(0, 292);
      this.FootnoteAreaPanel.Size = new System.Drawing.Size(634, 0);
      // 
      // ContentAreaPanel
      // 
      this.ContentAreaPanel.Controls.Add(this.SecondsLabel);
      this.ContentAreaPanel.Controls.Add(this.PingMonitoredInstancesLabel);
      this.ContentAreaPanel.Controls.Add(this.PingMonitoredInstancesNumericUpDown);
      this.ContentAreaPanel.Controls.Add(this.AutomaticMigrationDelayValueLabel);
      this.ContentAreaPanel.Controls.Add(this.AutomaticMigrationDelayLabel);
      this.ContentAreaPanel.Controls.Add(this.MigrateWorkbenchConnectionsButton);
      this.ContentAreaPanel.Controls.Add(this.ConnectionsOptionsLabel);
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
      this.ContentAreaPanel.Controls.Add(this.WeeksLabel);
      this.ContentAreaPanel.Size = new System.Drawing.Size(464, 501);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.Controls.Add(this.ResetToDefaultsButton);
      this.CommandAreaPanel.Controls.Add(this.DialogCancelButton);
      this.CommandAreaPanel.Controls.Add(this.DialogAcceptButton);
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 456);
      this.CommandAreaPanel.Size = new System.Drawing.Size(464, 45);
      // 
      // DialogCancelButton
      // 
      this.DialogCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.DialogCancelButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DialogCancelButton.Location = new System.Drawing.Point(374, 10);
      this.DialogCancelButton.Name = "DialogCancelButton";
      this.DialogCancelButton.Size = new System.Drawing.Size(78, 23);
      this.DialogCancelButton.TabIndex = 1;
      this.DialogCancelButton.Text = "Cancel";
      this.DialogCancelButton.UseVisualStyleBackColor = true;
      // 
      // DialogAcceptButton
      // 
      this.DialogAcceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogAcceptButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.DialogAcceptButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DialogAcceptButton.Location = new System.Drawing.Point(290, 11);
      this.DialogAcceptButton.Name = "DialogAcceptButton";
      this.DialogAcceptButton.Size = new System.Drawing.Size(78, 23);
      this.DialogAcceptButton.TabIndex = 0;
      this.DialogAcceptButton.Text = "Accept";
      this.DialogAcceptButton.UseVisualStyleBackColor = true;
      // 
      // UseColorfulIconsCheckBox
      // 
      this.UseColorfulIconsCheckBox.AutoSize = true;
      this.UseColorfulIconsCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.UseColorfulIconsCheckBox.Location = new System.Drawing.Point(50, 85);
      this.UseColorfulIconsCheckBox.Name = "UseColorfulIconsCheckBox";
      this.UseColorfulIconsCheckBox.Size = new System.Drawing.Size(157, 19);
      this.UseColorfulIconsCheckBox.TabIndex = 2;
      this.UseColorfulIconsCheckBox.Text = "Use colorful status icons.";
      this.HelpToolTip.SetToolTip(this.UseColorfulIconsCheckBox, resources.GetString("UseColorfulIconsCheckBox.ToolTip"));
      this.UseColorfulIconsCheckBox.UseVisualStyleBackColor = true;
      // 
      // NotificationsOptionsLabel
      // 
      this.NotificationsOptionsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.NotificationsOptionsLabel.AutoSize = true;
      this.NotificationsOptionsLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.NotificationsOptionsLabel.Location = new System.Drawing.Point(24, 253);
      this.NotificationsOptionsLabel.Name = "NotificationsOptionsLabel";
      this.NotificationsOptionsLabel.Size = new System.Drawing.Size(131, 17);
      this.NotificationsOptionsLabel.TabIndex = 12;
      this.NotificationsOptionsLabel.Text = "Notifications Options";
      // 
      // NotifyOfStatusChangeCheckBox
      // 
      this.NotifyOfStatusChangeCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.NotifyOfStatusChangeCheckBox.AutoSize = true;
      this.NotifyOfStatusChangeCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.NotifyOfStatusChangeCheckBox.Location = new System.Drawing.Point(50, 307);
      this.NotifyOfStatusChangeCheckBox.Name = "NotifyOfStatusChangeCheckBox";
      this.NotifyOfStatusChangeCheckBox.Size = new System.Drawing.Size(304, 19);
      this.NotifyOfStatusChangeCheckBox.TabIndex = 14;
      this.NotifyOfStatusChangeCheckBox.Text = "Notify me when a service or instance changes status.";
      this.HelpToolTip.SetToolTip(this.NotifyOfStatusChangeCheckBox, "When checked, a notification balloon is shown when the status of a monitored Wind" +
        "ows Service or MySQL Server instance changes.");
      this.NotifyOfStatusChangeCheckBox.UseVisualStyleBackColor = true;
      // 
      // NotifyOfAutoAddCheckBox
      // 
      this.NotifyOfAutoAddCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.NotifyOfAutoAddCheckBox.AutoSize = true;
      this.NotifyOfAutoAddCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.NotifyOfAutoAddCheckBox.Location = new System.Drawing.Point(51, 282);
      this.NotifyOfAutoAddCheckBox.Name = "NotifyOfAutoAddCheckBox";
      this.NotifyOfAutoAddCheckBox.Size = new System.Drawing.Size(284, 19);
      this.NotifyOfAutoAddCheckBox.TabIndex = 13;
      this.NotifyOfAutoAddCheckBox.Text = "Notify me when a service is automatically added.";
      this.HelpToolTip.SetToolTip(this.NotifyOfAutoAddCheckBox, "When checked, a notification balloon is shown when a Windows Service is automatic" +
        "ally added to the list of monitored services.");
      this.NotifyOfAutoAddCheckBox.UseVisualStyleBackColor = true;
      // 
      // AutoAddRegexTextBox
      // 
      this.AutoAddRegexTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.AutoAddRegexTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.AutoAddRegexTextBox.Location = new System.Drawing.Point(67, 185);
      this.AutoAddRegexTextBox.Name = "AutoAddRegexTextBox";
      this.AutoAddRegexTextBox.Size = new System.Drawing.Size(342, 23);
      this.AutoAddRegexTextBox.TabIndex = 8;
      // 
      // AutoAddServicesCheckBox
      // 
      this.AutoAddServicesCheckBox.AutoSize = true;
      this.AutoAddServicesCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.AutoAddServicesCheckBox.Location = new System.Drawing.Point(51, 160);
      this.AutoAddServicesCheckBox.Name = "AutoAddServicesCheckBox";
      this.AutoAddServicesCheckBox.Size = new System.Drawing.Size(313, 19);
      this.AutoAddServicesCheckBox.TabIndex = 7;
      this.AutoAddServicesCheckBox.Text = "Automatically add new services whose name contains:";
      this.HelpToolTip.SetToolTip(this.AutoAddServicesCheckBox, "When checked, newly created Windows Services containing the given text are added " +
        "automatically to the list of monitored services.");
      this.AutoAddServicesCheckBox.UseVisualStyleBackColor = true;
      this.AutoAddServicesCheckBox.CheckedChanged += new System.EventHandler(this.AutoAddServicesCheckBox_CheckedChanged);
      // 
      // WeeksLabel
      // 
      this.WeeksLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.WeeksLabel.AutoSize = true;
      this.WeeksLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.WeeksLabel.Location = new System.Drawing.Point(366, 136);
      this.WeeksLabel.Name = "WeeksLabel";
      this.WeeksLabel.Size = new System.Drawing.Size(42, 15);
      this.WeeksLabel.TabIndex = 6;
      this.WeeksLabel.Text = "weeks.";
      // 
      // CheckUpdatesWeeksNumericUpDown
      // 
      this.CheckUpdatesWeeksNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.CheckUpdatesWeeksNumericUpDown.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.CheckUpdatesWeeksNumericUpDown.Location = new System.Drawing.Point(322, 134);
      this.CheckUpdatesWeeksNumericUpDown.Name = "CheckUpdatesWeeksNumericUpDown";
      this.CheckUpdatesWeeksNumericUpDown.Size = new System.Drawing.Size(42, 23);
      this.CheckUpdatesWeeksNumericUpDown.TabIndex = 5;
      this.HelpToolTip.SetToolTip(this.CheckUpdatesWeeksNumericUpDown, "Number of weeks to check for updates for installed MySQL products.");
      // 
      // AutoCheckUpdatesCheckBox
      // 
      this.AutoCheckUpdatesCheckBox.AutoSize = true;
      this.AutoCheckUpdatesCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.AutoCheckUpdatesCheckBox.Location = new System.Drawing.Point(51, 135);
      this.AutoCheckUpdatesCheckBox.Name = "AutoCheckUpdatesCheckBox";
      this.AutoCheckUpdatesCheckBox.Size = new System.Drawing.Size(269, 19);
      this.AutoCheckUpdatesCheckBox.TabIndex = 4;
      this.AutoCheckUpdatesCheckBox.Text = "Automatically check for MySQL updates every";
      this.HelpToolTip.SetToolTip(this.AutoCheckUpdatesCheckBox, "When checked updates for installed MySQL products are checked periodically.");
      this.AutoCheckUpdatesCheckBox.UseVisualStyleBackColor = true;
      this.AutoCheckUpdatesCheckBox.CheckedChanged += new System.EventHandler(this.AutoCheckUpdatesCheckBox_CheckedChanged);
      // 
      // RunAtStartupCheckBox
      // 
      this.RunAtStartupCheckBox.AutoSize = true;
      this.RunAtStartupCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.RunAtStartupCheckBox.Location = new System.Drawing.Point(51, 110);
      this.RunAtStartupCheckBox.Name = "RunAtStartupCheckBox";
      this.RunAtStartupCheckBox.Size = new System.Drawing.Size(155, 19);
      this.RunAtStartupCheckBox.TabIndex = 3;
      this.RunAtStartupCheckBox.Text = "Run at Windows startup.";
      this.HelpToolTip.SetToolTip(this.RunAtStartupCheckBox, "Determines if MySQL Notifier starts automatically on Windows startup.");
      this.RunAtStartupCheckBox.UseVisualStyleBackColor = true;
      // 
      // GeneralOptionsLabel
      // 
      this.GeneralOptionsLabel.AutoSize = true;
      this.GeneralOptionsLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.GeneralOptionsLabel.Location = new System.Drawing.Point(24, 56);
      this.GeneralOptionsLabel.Name = "GeneralOptionsLabel";
      this.GeneralOptionsLabel.Size = new System.Drawing.Size(103, 17);
      this.GeneralOptionsLabel.TabIndex = 1;
      this.GeneralOptionsLabel.Text = "General Options";
      // 
      // TitleLabel
      // 
      this.TitleLabel.AutoSize = true;
      this.TitleLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TitleLabel.ForeColor = System.Drawing.Color.Navy;
      this.TitleLabel.Location = new System.Drawing.Point(17, 17);
      this.TitleLabel.Name = "TitleLabel";
      this.TitleLabel.Size = new System.Drawing.Size(166, 20);
      this.TitleLabel.TabIndex = 0;
      this.TitleLabel.Text = "MySQL Notifier Options";
      // 
      // HelpToolTip
      // 
      this.HelpToolTip.AutoPopDelay = 5000;
      this.HelpToolTip.InitialDelay = 1000;
      this.HelpToolTip.ReshowDelay = 100;
      // 
      // MigrateWorkbenchConnectionsButton
      // 
      this.MigrateWorkbenchConnectionsButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.MigrateWorkbenchConnectionsButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.MigrateWorkbenchConnectionsButton.Location = new System.Drawing.Point(50, 393);
      this.MigrateWorkbenchConnectionsButton.Name = "MigrateWorkbenchConnectionsButton";
      this.MigrateWorkbenchConnectionsButton.Size = new System.Drawing.Size(358, 23);
      this.MigrateWorkbenchConnectionsButton.TabIndex = 18;
      this.MigrateWorkbenchConnectionsButton.Text = "Migrate stored connections to MySQL Workbench now";
      this.HelpToolTip.SetToolTip(this.MigrateWorkbenchConnectionsButton, resources.GetString("MigrateWorkbenchConnectionsButton.ToolTip"));
      this.MigrateWorkbenchConnectionsButton.UseVisualStyleBackColor = true;
      this.MigrateWorkbenchConnectionsButton.Click += new System.EventHandler(this.MigrateWorkbenchConnectionsButton_Click);
      // 
      // PingMonitoredInstancesNumericUpDown
      // 
      this.PingMonitoredInstancesNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.PingMonitoredInstancesNumericUpDown.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PingMonitoredInstancesNumericUpDown.Location = new System.Drawing.Point(301, 214);
      this.PingMonitoredInstancesNumericUpDown.Maximum = new decimal(new int[] {
            2419200,
            0,
            0,
            0});
      this.PingMonitoredInstancesNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.PingMonitoredInstancesNumericUpDown.Name = "PingMonitoredInstancesNumericUpDown";
      this.PingMonitoredInstancesNumericUpDown.Size = new System.Drawing.Size(42, 23);
      this.PingMonitoredInstancesNumericUpDown.TabIndex = 10;
      this.HelpToolTip.SetToolTip(this.PingMonitoredInstancesNumericUpDown, "Interval for pinging monitored MySQL Server instances to check for status updates" +
        ".");
      this.PingMonitoredInstancesNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // ConnectionsOptionsLabel
      // 
      this.ConnectionsOptionsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.ConnectionsOptionsLabel.AutoSize = true;
      this.ConnectionsOptionsLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ConnectionsOptionsLabel.Location = new System.Drawing.Point(24, 346);
      this.ConnectionsOptionsLabel.Name = "ConnectionsOptionsLabel";
      this.ConnectionsOptionsLabel.Size = new System.Drawing.Size(215, 17);
      this.ConnectionsOptionsLabel.TabIndex = 15;
      this.ConnectionsOptionsLabel.Text = "MySQL Server Connections Options";
      // 
      // ResetToDefaultsButton
      // 
      this.ResetToDefaultsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.ResetToDefaultsButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ResetToDefaultsButton.Location = new System.Drawing.Point(12, 10);
      this.ResetToDefaultsButton.Name = "ResetToDefaultsButton";
      this.ResetToDefaultsButton.Size = new System.Drawing.Size(128, 23);
      this.ResetToDefaultsButton.TabIndex = 2;
      this.ResetToDefaultsButton.Text = "Reset to Defaults";
      this.ResetToDefaultsButton.UseVisualStyleBackColor = true;
      this.ResetToDefaultsButton.Click += new System.EventHandler(this.ResetToDefaultsButton_Click);
      // 
      // AutomaticMigrationDelayLabel
      // 
      this.AutomaticMigrationDelayLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.AutomaticMigrationDelayLabel.AutoSize = true;
      this.AutomaticMigrationDelayLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.AutomaticMigrationDelayLabel.Location = new System.Drawing.Point(47, 375);
      this.AutomaticMigrationDelayLabel.Name = "AutomaticMigrationDelayLabel";
      this.AutomaticMigrationDelayLabel.Size = new System.Drawing.Size(263, 15);
      this.AutomaticMigrationDelayLabel.TabIndex = 16;
      this.AutomaticMigrationDelayLabel.Text = "Automatic connections migration delayed until: ";
      // 
      // AutomaticMigrationDelayValueLabel
      // 
      this.AutomaticMigrationDelayValueLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.AutomaticMigrationDelayValueLabel.AutoSize = true;
      this.AutomaticMigrationDelayValueLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.AutomaticMigrationDelayValueLabel.ForeColor = System.Drawing.SystemColors.GrayText;
      this.AutomaticMigrationDelayValueLabel.Location = new System.Drawing.Point(311, 375);
      this.AutomaticMigrationDelayValueLabel.Name = "AutomaticMigrationDelayValueLabel";
      this.AutomaticMigrationDelayValueLabel.Size = new System.Drawing.Size(67, 15);
      this.AutomaticMigrationDelayValueLabel.TabIndex = 17;
      this.AutomaticMigrationDelayValueLabel.Text = "Delay Value";
      // 
      // PingMonitoredInstancesLabel
      // 
      this.PingMonitoredInstancesLabel.AutoSize = true;
      this.PingMonitoredInstancesLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PingMonitoredInstancesLabel.Location = new System.Drawing.Point(48, 216);
      this.PingMonitoredInstancesLabel.Name = "PingMonitoredInstancesLabel";
      this.PingMonitoredInstancesLabel.Size = new System.Drawing.Size(249, 15);
      this.PingMonitoredInstancesLabel.TabIndex = 9;
      this.PingMonitoredInstancesLabel.Text = "Ping monitored MySQL Server instances every";
      // 
      // SecondsLabel
      // 
      this.SecondsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.SecondsLabel.AutoSize = true;
      this.SecondsLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.SecondsLabel.Location = new System.Drawing.Point(346, 216);
      this.SecondsLabel.Name = "SecondsLabel";
      this.SecondsLabel.Size = new System.Drawing.Size(53, 15);
      this.SecondsLabel.TabIndex = 11;
      this.SecondsLabel.Text = "seconds.";
      // 
      // OptionsDialog
      // 
      this.AcceptButton = this.DialogAcceptButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.DialogCancelButton;
      this.ClientSize = new System.Drawing.Size(464, 501);
      this.CommandAreaVisible = true;
      this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
      this.FootnoteAreaHeight = 0;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "OptionsDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Options";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OptionsDialog_FormClosing);
      this.ContentAreaPanel.ResumeLayout(false);
      this.ContentAreaPanel.PerformLayout();
      this.CommandAreaPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.CheckUpdatesWeeksNumericUpDown)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.PingMonitoredInstancesNumericUpDown)).EndInit();
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
    private System.Windows.Forms.Label WeeksLabel;
    private System.Windows.Forms.Button DialogCancelButton;
    private System.Windows.Forms.Button DialogAcceptButton;
    private System.Windows.Forms.ToolTip HelpToolTip;
    private System.Windows.Forms.Label ConnectionsOptionsLabel;
    private System.Windows.Forms.Button MigrateWorkbenchConnectionsButton;
    private System.Windows.Forms.Button ResetToDefaultsButton;
    private System.Windows.Forms.Label AutomaticMigrationDelayLabel;
    private System.Windows.Forms.Label AutomaticMigrationDelayValueLabel;
    private System.Windows.Forms.Label SecondsLabel;
    private System.Windows.Forms.Label PingMonitoredInstancesLabel;
    private System.Windows.Forms.NumericUpDown PingMonitoredInstancesNumericUpDown;
  }
}