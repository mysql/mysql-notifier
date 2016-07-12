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

namespace MySql.Notifier.Forms
{
  partial class MonitorMySqlServerInstancesDialog
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MonitorMySqlServerInstancesDialog));
      this.ConnectionNameLabel = new System.Windows.Forms.Label();
      this.ShowMonitoredInstancesCheckBox = new System.Windows.Forms.CheckBox();
      this.FilterTextBox = new System.Windows.Forms.TextBox();
      this.MySQLConnectionsHelpLabel = new System.Windows.Forms.Label();
      this.MySQLConnectionsHyperTitleLabel = new System.Windows.Forms.Label();
      this.WorkbenchConnectionsListView = new System.Windows.Forms.ListView();
      this.MethodColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.ConnectionNameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.HostnameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.PortColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.MonitoredColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.ConnectionsContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.DeleteConnectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.EditConnectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.RefreshConnectionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.DialogCancelButton = new System.Windows.Forms.Button();
      this.AddConnectionButton = new System.Windows.Forms.Button();
      this.DialogOKButton = new System.Windows.Forms.Button();
      this.FilterTimer = new System.Windows.Forms.Timer(this.components);
      this.ContentAreaPanel.SuspendLayout();
      this.CommandAreaPanel.SuspendLayout();
      this.ConnectionsContextMenuStrip.SuspendLayout();
      this.SuspendLayout();
      // 
      // FootnoteAreaPanel
      // 
      this.FootnoteAreaPanel.Location = new System.Drawing.Point(0, 292);
      this.FootnoteAreaPanel.Size = new System.Drawing.Size(634, 0);
      // 
      // ContentAreaPanel
      // 
      this.ContentAreaPanel.Controls.Add(this.CommandAreaPanel);
      this.ContentAreaPanel.Controls.Add(this.ConnectionNameLabel);
      this.ContentAreaPanel.Controls.Add(this.ShowMonitoredInstancesCheckBox);
      this.ContentAreaPanel.Controls.Add(this.FilterTextBox);
      this.ContentAreaPanel.Controls.Add(this.MySQLConnectionsHelpLabel);
      this.ContentAreaPanel.Controls.Add(this.MySQLConnectionsHyperTitleLabel);
      this.ContentAreaPanel.Controls.Add(this.WorkbenchConnectionsListView);
      this.ContentAreaPanel.Size = new System.Drawing.Size(709, 572);
      this.ContentAreaPanel.Controls.SetChildIndex(this.WorkbenchConnectionsListView, 0);
      this.ContentAreaPanel.Controls.SetChildIndex(this.MySQLConnectionsHyperTitleLabel, 0);
      this.ContentAreaPanel.Controls.SetChildIndex(this.MySQLConnectionsHelpLabel, 0);
      this.ContentAreaPanel.Controls.SetChildIndex(this.FilterTextBox, 0);
      this.ContentAreaPanel.Controls.SetChildIndex(this.ShowMonitoredInstancesCheckBox, 0);
      this.ContentAreaPanel.Controls.SetChildIndex(this.ConnectionNameLabel, 0);
      this.ContentAreaPanel.Controls.SetChildIndex(this.CommandAreaPanel, 0);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.BackColor = System.Drawing.SystemColors.Control;
      this.CommandAreaPanel.Controls.Add(this.DialogCancelButton);
      this.CommandAreaPanel.Controls.Add(this.AddConnectionButton);
      this.CommandAreaPanel.Controls.Add(this.DialogOKButton);
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 527);
      this.CommandAreaPanel.Size = new System.Drawing.Size(709, 45);
      this.CommandAreaPanel.TabIndex = 6;
      // 
      // ConnectionNameLabel
      // 
      this.ConnectionNameLabel.AutoSize = true;
      this.ConnectionNameLabel.Location = new System.Drawing.Point(25, 109);
      this.ConnectionNameLabel.Name = "ConnectionNameLabel";
      this.ConnectionNameLabel.Size = new System.Drawing.Size(36, 15);
      this.ConnectionNameLabel.TabIndex = 2;
      this.ConnectionNameLabel.Text = "Filter:";
      // 
      // ShowMonitoredInstancesCheckBox
      // 
      this.ShowMonitoredInstancesCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.ShowMonitoredInstancesCheckBox.AutoSize = true;
      this.ShowMonitoredInstancesCheckBox.Location = new System.Drawing.Point(398, 108);
      this.ShowMonitoredInstancesCheckBox.Name = "ShowMonitoredInstancesCheckBox";
      this.ShowMonitoredInstancesCheckBox.Size = new System.Drawing.Size(286, 19);
      this.ShowMonitoredInstancesCheckBox.TabIndex = 4;
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
      this.FilterTextBox.Size = new System.Drawing.Size(319, 23);
      this.FilterTextBox.TabIndex = 3;
      this.FilterTextBox.TextChanged += new System.EventHandler(this.FilterTextBox_TextChanged);
      this.FilterTextBox.Validated += new System.EventHandler(this.FilterTextBox_Validated);
      // 
      // MySQLConnectionsHelpLabel
      // 
      this.MySQLConnectionsHelpLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.MySQLConnectionsHelpLabel.Location = new System.Drawing.Point(24, 53);
      this.MySQLConnectionsHelpLabel.Name = "MySQLConnectionsHelpLabel";
      this.MySQLConnectionsHelpLabel.Size = new System.Drawing.Size(660, 38);
      this.MySQLConnectionsHelpLabel.TabIndex = 1;
      this.MySQLConnectionsHelpLabel.Text = "Select a connection from MySQL Workbench that you want to monitor. You can filter" +
    " the list by typing into the filter control.";
      // 
      // MySQLConnectionsHyperTitleLabel
      // 
      this.MySQLConnectionsHyperTitleLabel.AutoSize = true;
      this.MySQLConnectionsHyperTitleLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.MySQLConnectionsHyperTitleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(73)))), ((int)(((byte)(161)))));
      this.MySQLConnectionsHyperTitleLabel.Location = new System.Drawing.Point(20, 23);
      this.MySQLConnectionsHyperTitleLabel.Name = "MySQLConnectionsHyperTitleLabel";
      this.MySQLConnectionsHyperTitleLabel.Size = new System.Drawing.Size(261, 21);
      this.MySQLConnectionsHyperTitleLabel.TabIndex = 0;
      this.MySQLConnectionsHyperTitleLabel.Text = "Choose a MySQL Server connection:";
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
      this.WorkbenchConnectionsListView.Size = new System.Drawing.Size(659, 364);
      this.WorkbenchConnectionsListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
      this.WorkbenchConnectionsListView.TabIndex = 5;
      this.WorkbenchConnectionsListView.UseCompatibleStateImageBehavior = false;
      this.WorkbenchConnectionsListView.View = System.Windows.Forms.View.Details;
      this.WorkbenchConnectionsListView.SelectedIndexChanged += new System.EventHandler(this.WorkbenchConnectionsListView_SelectedIndexChanged);
      this.WorkbenchConnectionsListView.DoubleClick += new System.EventHandler(this.WorkbenchConnectionsListView_DoubleClick);
      // 
      // MethodColumnHeader
      // 
      this.MethodColumnHeader.Text = "Type";
      this.MethodColumnHeader.Width = 101;
      // 
      // ConnectionNameColumnHeader
      // 
      this.ConnectionNameColumnHeader.Text = "Name";
      this.ConnectionNameColumnHeader.Width = 235;
      // 
      // HostnameColumnHeader
      // 
      this.HostnameColumnHeader.Text = "Host";
      this.HostnameColumnHeader.Width = 195;
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
            this.DeleteConnectionToolStripMenuItem,
            this.EditConnectionToolStripMenuItem,
            this.RefreshConnectionsToolStripMenuItem});
      this.ConnectionsContextMenuStrip.Name = "ConnectionsContextMenuStrip";
      this.ConnectionsContextMenuStrip.Size = new System.Drawing.Size(184, 70);
      this.ConnectionsContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.ConnectionsContextMenuStrip_Opening);
      // 
      // DeleteConnectionToolStripMenuItem
      // 
      this.DeleteConnectionToolStripMenuItem.Image = global::MySql.Notifier.Properties.Resources.delete;
      this.DeleteConnectionToolStripMenuItem.Name = "DeleteConnectionToolStripMenuItem";
      this.DeleteConnectionToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
      this.DeleteConnectionToolStripMenuItem.Text = "Delete Connection";
      this.DeleteConnectionToolStripMenuItem.Click += new System.EventHandler(this.DeleteConnectionToolStripMenuItem_Click);
      // 
      // EditConnectionToolStripMenuItem
      // 
      this.EditConnectionToolStripMenuItem.Image = global::MySql.Notifier.Properties.Resources.edit;
      this.EditConnectionToolStripMenuItem.Name = "EditConnectionToolStripMenuItem";
      this.EditConnectionToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
      this.EditConnectionToolStripMenuItem.Text = "Edit Connection";
      this.EditConnectionToolStripMenuItem.Click += new System.EventHandler(this.EditConnectionToolStripMenuItem_Click);
      // 
      // RefreshConnectionsToolStripMenuItem
      // 
      this.RefreshConnectionsToolStripMenuItem.Image = global::MySql.Notifier.Properties.Resources.refresh;
      this.RefreshConnectionsToolStripMenuItem.Name = "RefreshConnectionsToolStripMenuItem";
      this.RefreshConnectionsToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
      this.RefreshConnectionsToolStripMenuItem.Text = "Refresh Connections";
      this.RefreshConnectionsToolStripMenuItem.Click += new System.EventHandler(this.RefreshConnectionsToolStripMenuItem_Click);
      // 
      // DialogCancelButton
      // 
      this.DialogCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.DialogCancelButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DialogCancelButton.Location = new System.Drawing.Point(622, 11);
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
      this.AddConnectionButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.AddConnectionButton.Location = new System.Drawing.Point(12, 11);
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
      this.DialogOKButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DialogOKButton.Location = new System.Drawing.Point(541, 11);
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
      // MonitorMySqlServerInstancesDialog
      // 
      this.AcceptButton = this.DialogOKButton;
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.CancelButton = this.DialogCancelButton;
      this.ClientSize = new System.Drawing.Size(709, 572);
      this.CommandAreaVisible = true;
      this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
      this.FootnoteAreaHeight = 0;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimumSize = new System.Drawing.Size(610, 600);
      this.Name = "MonitorMySqlServerInstancesDialog";
      this.Text = "Monitor MySQL Server Instance";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MonitorMySQLServerInstancesDialog_FormClosing);
      this.Shown += new System.EventHandler(this.MonitorMySQLServerInstancesDialog_Shown);
      this.Controls.SetChildIndex(this.ContentAreaPanel, 0);
      this.Controls.SetChildIndex(this.FootnoteAreaPanel, 0);
      this.ContentAreaPanel.ResumeLayout(false);
      this.ContentAreaPanel.PerformLayout();
      this.CommandAreaPanel.ResumeLayout(false);
      this.ConnectionsContextMenuStrip.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button DialogCancelButton;
    private System.Windows.Forms.Button DialogOKButton;
    private System.Windows.Forms.ListView WorkbenchConnectionsListView;
    private System.Windows.Forms.ColumnHeader ConnectionNameColumnHeader;
    private System.Windows.Forms.ColumnHeader HostnameColumnHeader;
    private System.Windows.Forms.ColumnHeader PortColumnHeader;
    private System.Windows.Forms.Label MySQLConnectionsHelpLabel;
    private System.Windows.Forms.Label MySQLConnectionsHyperTitleLabel;
    private System.Windows.Forms.Button AddConnectionButton;
    private System.Windows.Forms.ColumnHeader MethodColumnHeader;
    private System.Windows.Forms.ColumnHeader MonitoredColumnHeader;
    private System.Windows.Forms.Label ConnectionNameLabel;
    private System.Windows.Forms.CheckBox ShowMonitoredInstancesCheckBox;
    private System.Windows.Forms.TextBox FilterTextBox;
    private System.Windows.Forms.Timer FilterTimer;
    private System.Windows.Forms.ContextMenuStrip ConnectionsContextMenuStrip;
    private System.Windows.Forms.ToolStripMenuItem RefreshConnectionsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem DeleteConnectionToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem EditConnectionToolStripMenuItem;
  }
}