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
  partial class MonitorMySQLServerInstancesDialog
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MonitorMySQLServerInstancesDialog));
      this.ContentAreaPanel = new System.Windows.Forms.Panel();
      this.ConnectionNameLabel = new System.Windows.Forms.Label();
      this.ShowMonitoredInstancesCheckBox = new System.Windows.Forms.CheckBox();
      this.FilterTextBox = new System.Windows.Forms.TextBox();
      this.MySQLConnectionsHelpLabel = new System.Windows.Forms.Label();
      this.MySQLConnectionsHiperTitleLabel = new System.Windows.Forms.Label();
      this.WorkbenchConnectionsListView = new System.Windows.Forms.ListView();
      this.MethodColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.ConnectionNameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.HostnameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.PortColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.MonitoredColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.ConnectionsContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.RefreshConnectionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.CommandAreaPanel = new System.Windows.Forms.Panel();
      this.DialogCancelButton = new System.Windows.Forms.Button();
      this.AddConnectionButton = new System.Windows.Forms.Button();
      this.DialogOKButton = new System.Windows.Forms.Button();
      this.FilterTimer = new System.Windows.Forms.Timer(this.components);
      this.ContentAreaPanel.SuspendLayout();
      this.ConnectionsContextMenuStrip.SuspendLayout();
      this.CommandAreaPanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // ContentAreaPanel
      // 
      this.ContentAreaPanel.BackColor = System.Drawing.SystemColors.Window;
      this.ContentAreaPanel.Controls.Add(this.ConnectionNameLabel);
      this.ContentAreaPanel.Controls.Add(this.ShowMonitoredInstancesCheckBox);
      this.ContentAreaPanel.Controls.Add(this.FilterTextBox);
      this.ContentAreaPanel.Controls.Add(this.MySQLConnectionsHelpLabel);
      this.ContentAreaPanel.Controls.Add(this.MySQLConnectionsHiperTitleLabel);
      this.ContentAreaPanel.Controls.Add(this.WorkbenchConnectionsListView);
      this.ContentAreaPanel.Controls.Add(this.CommandAreaPanel);
      this.ContentAreaPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ContentAreaPanel.Location = new System.Drawing.Point(0, 0);
      this.ContentAreaPanel.Name = "ContentAreaPanel";
      this.ContentAreaPanel.Size = new System.Drawing.Size(594, 562);
      this.ContentAreaPanel.TabIndex = 0;
      // 
      // ConnectionNameLabel
      // 
      this.ConnectionNameLabel.AutoSize = true;
      this.ConnectionNameLabel.Location = new System.Drawing.Point(25, 109);
      this.ConnectionNameLabel.Name = "ConnectionNameLabel";
      this.ConnectionNameLabel.Size = new System.Drawing.Size(36, 15);
      this.ConnectionNameLabel.TabIndex = 11;
      this.ConnectionNameLabel.Text = "Filter:";
      // 
      // ShowMonitoredInstancesCheckBox
      // 
      this.ShowMonitoredInstancesCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.ShowMonitoredInstancesCheckBox.AutoSize = true;
      this.ShowMonitoredInstancesCheckBox.Location = new System.Drawing.Point(283, 108);
      this.ShowMonitoredInstancesCheckBox.Name = "ShowMonitoredInstancesCheckBox";
      this.ShowMonitoredInstancesCheckBox.Size = new System.Drawing.Size(286, 19);
      this.ShowMonitoredInstancesCheckBox.TabIndex = 10;
      this.ShowMonitoredInstancesCheckBox.Text = "Show MySQL instances already being monitored?";
      this.ShowMonitoredInstancesCheckBox.UseVisualStyleBackColor = true;
      this.ShowMonitoredInstancesCheckBox.CheckedChanged += new System.EventHandler(this.ShowMonitoredInstancesCheckBox_CheckedChanged);
      // 
      // FilterTextBox
      // 
      this.FilterTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.FilterTextBox.Location = new System.Drawing.Point(67, 106);
      this.FilterTextBox.Name = "FilterTextBox";
      this.FilterTextBox.Size = new System.Drawing.Size(210, 23);
      this.FilterTextBox.TabIndex = 9;
      this.FilterTextBox.TextChanged += new System.EventHandler(this.FilterTextBox_TextChanged);
      this.FilterTextBox.Validated += new System.EventHandler(this.FilterTextBox_Validated);
      // 
      // MySQLConnectionsHelpLabel
      // 
      this.MySQLConnectionsHelpLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.MySQLConnectionsHelpLabel.Location = new System.Drawing.Point(24, 53);
      this.MySQLConnectionsHelpLabel.Name = "MySQLConnectionsHelpLabel";
      this.MySQLConnectionsHelpLabel.Size = new System.Drawing.Size(545, 38);
      this.MySQLConnectionsHelpLabel.TabIndex = 1;
      this.MySQLConnectionsHelpLabel.Text = "Select a connection from MySQL Workbench that you want to monitor. You can filter" +
    " the list by typing into the filter control.";
      // 
      // MySQLConnectionsHiperTitleLabel
      // 
      this.MySQLConnectionsHiperTitleLabel.AutoSize = true;
      this.MySQLConnectionsHiperTitleLabel.Location = new System.Drawing.Point(20, 23);
      this.MySQLConnectionsHiperTitleLabel.Name = "MySQLConnectionsHiperTitleLabel";
      this.MySQLConnectionsHiperTitleLabel.Size = new System.Drawing.Size(198, 15);
      this.MySQLConnectionsHiperTitleLabel.TabIndex = 0;
      this.MySQLConnectionsHiperTitleLabel.Text = "Choose a MySQL Server connection:";
      // 
      // WorkbenchConnectionsListView
      // 
      this.WorkbenchConnectionsListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.WorkbenchConnectionsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.MethodColumnHeader,
            this.ConnectionNameColumnHeader,
            this.HostnameColumnHeader,
            this.PortColumnHeader,
            this.MonitoredColumnHeader});
      this.WorkbenchConnectionsListView.ContextMenuStrip = this.ConnectionsContextMenuStrip;
      this.WorkbenchConnectionsListView.FullRowSelect = true;
      this.WorkbenchConnectionsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.WorkbenchConnectionsListView.HideSelection = false;
      this.WorkbenchConnectionsListView.Location = new System.Drawing.Point(25, 135);
      this.WorkbenchConnectionsListView.MultiSelect = false;
      this.WorkbenchConnectionsListView.Name = "WorkbenchConnectionsListView";
      this.WorkbenchConnectionsListView.Size = new System.Drawing.Size(544, 354);
      this.WorkbenchConnectionsListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
      this.WorkbenchConnectionsListView.TabIndex = 5;
      this.WorkbenchConnectionsListView.UseCompatibleStateImageBehavior = false;
      this.WorkbenchConnectionsListView.View = System.Windows.Forms.View.Details;
      this.WorkbenchConnectionsListView.SelectedIndexChanged += new System.EventHandler(this.WorkbenchConnectionsListView_SelectedIndexChanged);
      // 
      // MethodColumnHeader
      // 
      this.MethodColumnHeader.Text = "Method";
      this.MethodColumnHeader.Width = 78;
      // 
      // ConnectionNameColumnHeader
      // 
      this.ConnectionNameColumnHeader.Text = "Name";
      this.ConnectionNameColumnHeader.Width = 186;
      // 
      // HostnameColumnHeader
      // 
      this.HostnameColumnHeader.Text = "Host";
      this.HostnameColumnHeader.Width = 152;
      // 
      // PortColumnHeader
      // 
      this.PortColumnHeader.Text = "Port";
      this.PortColumnHeader.Width = 52;
      // 
      // MonitoredColumnHeader
      // 
      this.MonitoredColumnHeader.Text = "Monitored";
      this.MonitoredColumnHeader.Width = 71;
      // 
      // ConnectionsContextMenuStrip
      // 
      this.ConnectionsContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RefreshConnectionsToolStripMenuItem});
      this.ConnectionsContextMenuStrip.Name = "ConnectionsContextMenuStrip";
      this.ConnectionsContextMenuStrip.Size = new System.Drawing.Size(184, 26);
      // 
      // RefreshConnectionsToolStripMenuItem
      // 
      this.RefreshConnectionsToolStripMenuItem.Image = global::MySql.Notifier.Properties.Resources.refresh;
      this.RefreshConnectionsToolStripMenuItem.Name = "RefreshConnectionsToolStripMenuItem";
      this.RefreshConnectionsToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
      this.RefreshConnectionsToolStripMenuItem.Text = "Refresh Connections";
      this.RefreshConnectionsToolStripMenuItem.Click += new System.EventHandler(this.RefreshConnectionsToolStripMenuItem_Click);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.BackColor = System.Drawing.SystemColors.Menu;
      this.CommandAreaPanel.Controls.Add(this.DialogCancelButton);
      this.CommandAreaPanel.Controls.Add(this.AddConnectionButton);
      this.CommandAreaPanel.Controls.Add(this.DialogOKButton);
      this.CommandAreaPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.CommandAreaPanel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 512);
      this.CommandAreaPanel.Name = "CommandAreaPanel";
      this.CommandAreaPanel.Size = new System.Drawing.Size(594, 50);
      this.CommandAreaPanel.TabIndex = 6;
      // 
      // DialogCancelButton
      // 
      this.DialogCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.DialogCancelButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DialogCancelButton.Location = new System.Drawing.Point(494, 15);
      this.DialogCancelButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.DialogCancelButton.Name = "DialogCancelButton";
      this.DialogCancelButton.Size = new System.Drawing.Size(75, 23);
      this.DialogCancelButton.TabIndex = 2;
      this.DialogCancelButton.Text = "Cancel";
      this.DialogCancelButton.UseVisualStyleBackColor = true;
      // 
      // AddConnectionButton
      // 
      this.AddConnectionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.AddConnectionButton.Location = new System.Drawing.Point(25, 15);
      this.AddConnectionButton.Name = "AddConnectionButton";
      this.AddConnectionButton.Size = new System.Drawing.Size(157, 23);
      this.AddConnectionButton.TabIndex = 0;
      this.AddConnectionButton.Text = "Add New Connection...";
      this.AddConnectionButton.UseVisualStyleBackColor = true;
      this.AddConnectionButton.Click += new System.EventHandler(this.AddConnectionButton_Click);
      // 
      // DialogOKButton
      // 
      this.DialogOKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogOKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.DialogOKButton.Enabled = false;
      this.DialogOKButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DialogOKButton.Location = new System.Drawing.Point(413, 15);
      this.DialogOKButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.DialogOKButton.Name = "DialogOKButton";
      this.DialogOKButton.Size = new System.Drawing.Size(75, 23);
      this.DialogOKButton.TabIndex = 1;
      this.DialogOKButton.Text = "OK";
      this.DialogOKButton.UseVisualStyleBackColor = true;
      this.DialogOKButton.Click += new System.EventHandler(this.DialogOKButton_Click);
      // 
      // FilterTimer
      // 
      this.FilterTimer.Interval = 500;
      this.FilterTimer.Tick += new System.EventHandler(this.FilterTimer_Tick);
      // 
      // MonitorMySQLServerInstancesDialog
      // 
      this.AcceptButton = this.DialogOKButton;
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.CancelButton = this.DialogCancelButton;
      this.ClientSize = new System.Drawing.Size(594, 562);
      this.Controls.Add(this.ContentAreaPanel);
      this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(610, 600);
      this.Name = "MonitorMySQLServerInstancesDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Monitor MySQL Server Instance";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MonitorMySQLServerInstancesDialog_FormClosing);
      this.Shown += new System.EventHandler(this.MonitorMySQLServerInstancesDialog_Shown);
      this.ContentAreaPanel.ResumeLayout(false);
      this.ContentAreaPanel.PerformLayout();
      this.ConnectionsContextMenuStrip.ResumeLayout(false);
      this.CommandAreaPanel.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel ContentAreaPanel;
    private System.Windows.Forms.Panel CommandAreaPanel;
    private System.Windows.Forms.Button DialogCancelButton;
    private System.Windows.Forms.Button DialogOKButton;
    private System.Windows.Forms.ListView WorkbenchConnectionsListView;
    private System.Windows.Forms.ColumnHeader ConnectionNameColumnHeader;
    private System.Windows.Forms.ColumnHeader HostnameColumnHeader;
    private System.Windows.Forms.ColumnHeader PortColumnHeader;
    private System.Windows.Forms.Label MySQLConnectionsHelpLabel;
    private System.Windows.Forms.Label MySQLConnectionsHiperTitleLabel;
    private System.Windows.Forms.Button AddConnectionButton;
    private System.Windows.Forms.ColumnHeader MethodColumnHeader;
    private System.Windows.Forms.ColumnHeader MonitoredColumnHeader;
    private System.Windows.Forms.Label ConnectionNameLabel;
    private System.Windows.Forms.CheckBox ShowMonitoredInstancesCheckBox;
    private System.Windows.Forms.TextBox FilterTextBox;
    private System.Windows.Forms.Timer FilterTimer;
    private System.Windows.Forms.ContextMenuStrip ConnectionsContextMenuStrip;
    private System.Windows.Forms.ToolStripMenuItem RefreshConnectionsToolStripMenuItem;
  }
}