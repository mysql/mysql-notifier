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
  partial class ManageItemsDialog
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageItemsDialog));
      this.panel2 = new System.Windows.Forms.Panel();
      this.btnClose = new System.Windows.Forms.Button();
      this.panel1 = new System.Windows.Forms.Panel();
      this.label2 = new System.Windows.Forms.Label();
      this.lblSubTitle1 = new System.Windows.Forms.Label();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.InstanceMonitorIntervalUOMComboBox = new System.Windows.Forms.ComboBox();
      this.InstanceMonitorIntervalLabel = new System.Windows.Forms.Label();
      this.InstanceMonitorIntervalNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.chkUpdateTrayIcon = new System.Windows.Forms.CheckBox();
      this.notifyOnStatusChange = new System.Windows.Forms.CheckBox();
      this.btnDelete = new System.Windows.Forms.Button();
      this.btnAdd = new System.Windows.Forms.Button();
      this.lblHiperTitle = new System.Windows.Forms.Label();
      this.MonitoredItemsListView = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.AddButtonContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.serviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.mySQLInstanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.panel2.SuspendLayout();
      this.panel1.SuspendLayout();
      this.groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.InstanceMonitorIntervalNumericUpDown)).BeginInit();
      this.AddButtonContextMenuStrip.SuspendLayout();
      this.SuspendLayout();
      // 
      // panel2
      // 
      this.panel2.Controls.Add(this.btnClose);
      this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panel2.ForeColor = System.Drawing.SystemColors.ControlText;
      this.panel2.Location = new System.Drawing.Point(0, 453);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(596, 48);
      this.panel2.TabIndex = 47;
      // 
      // btnClose
      // 
      this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnClose.Location = new System.Drawing.Point(497, 9);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(87, 27);
      this.btnClose.TabIndex = 15;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      // 
      // panel1
      // 
      this.panel1.AutoSize = true;
      this.panel1.BackColor = System.Drawing.Color.White;
      this.panel1.Controls.Add(this.label2);
      this.panel1.Controls.Add(this.lblSubTitle1);
      this.panel1.Controls.Add(this.groupBox1);
      this.panel1.Controls.Add(this.btnDelete);
      this.panel1.Controls.Add(this.btnAdd);
      this.panel1.Controls.Add(this.lblHiperTitle);
      this.panel1.Controls.Add(this.MonitoredItemsListView);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel1.Location = new System.Drawing.Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(596, 501);
      this.panel1.TabIndex = 48;
      // 
      // label2
      // 
      this.label2.Location = new System.Drawing.Point(32, 73);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(448, 37);
      this.label2.TabIndex = 25;
      this.label2.Text = "Local MySQL services will be automatically added. Press Add to monitor other Wind" +
    "ows services or MySQL instances.";
      // 
      // lblSubTitle1
      // 
      this.lblSubTitle1.AutoSize = true;
      this.lblSubTitle1.Location = new System.Drawing.Point(32, 45);
      this.lblSubTitle1.Name = "lblSubTitle1";
      this.lblSubTitle1.Size = new System.Drawing.Size(217, 15);
      this.lblSubTitle1.TabIndex = 24;
      this.lblSubTitle1.Text = "Windows Services and MySQL Instances";
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.InstanceMonitorIntervalUOMComboBox);
      this.groupBox1.Controls.Add(this.InstanceMonitorIntervalLabel);
      this.groupBox1.Controls.Add(this.InstanceMonitorIntervalNumericUpDown);
      this.groupBox1.Controls.Add(this.chkUpdateTrayIcon);
      this.groupBox1.Controls.Add(this.notifyOnStatusChange);
      this.groupBox1.Location = new System.Drawing.Point(32, 313);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(448, 108);
      this.groupBox1.TabIndex = 23;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Service Options";
      // 
      // InstanceMonitorIntervalUOMComboBox
      // 
      this.InstanceMonitorIntervalUOMComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.InstanceMonitorIntervalUOMComboBox.Items.AddRange(new object[] {
            "seconds",
            "minutes",
            "hours",
            "days"});
      this.InstanceMonitorIntervalUOMComboBox.Location = new System.Drawing.Point(285, 72);
      this.InstanceMonitorIntervalUOMComboBox.Name = "InstanceMonitorIntervalUOMComboBox";
      this.InstanceMonitorIntervalUOMComboBox.Size = new System.Drawing.Size(121, 23);
      this.InstanceMonitorIntervalUOMComboBox.TabIndex = 4;
      this.InstanceMonitorIntervalUOMComboBox.SelectedIndexChanged += new System.EventHandler(this.InstanceMonitorIntervalUOMComboBox_SelectedIndexChanged);
      // 
      // InstanceMonitorIntervalLabel
      // 
      this.InstanceMonitorIntervalLabel.AutoSize = true;
      this.InstanceMonitorIntervalLabel.Location = new System.Drawing.Point(69, 75);
      this.InstanceMonitorIntervalLabel.Name = "InstanceMonitorIntervalLabel";
      this.InstanceMonitorIntervalLabel.Size = new System.Drawing.Size(210, 15);
      this.InstanceMonitorIntervalLabel.TabIndex = 3;
      this.InstanceMonitorIntervalLabel.Text = "MySQL Instance monitoring interval in";
      // 
      // InstanceMonitorIntervalNumericUpDown
      // 
      this.InstanceMonitorIntervalNumericUpDown.Location = new System.Drawing.Point(15, 73);
      this.InstanceMonitorIntervalNumericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
      this.InstanceMonitorIntervalNumericUpDown.Name = "InstanceMonitorIntervalNumericUpDown";
      this.InstanceMonitorIntervalNumericUpDown.Size = new System.Drawing.Size(51, 23);
      this.InstanceMonitorIntervalNumericUpDown.TabIndex = 2;
      this.InstanceMonitorIntervalNumericUpDown.ValueChanged += new System.EventHandler(this.InstanceMonitorIntervalNumericUpDown_ValueChanged);
      // 
      // chkUpdateTrayIcon
      // 
      this.chkUpdateTrayIcon.AutoSize = true;
      this.chkUpdateTrayIcon.Enabled = false;
      this.chkUpdateTrayIcon.Location = new System.Drawing.Point(51, 48);
      this.chkUpdateTrayIcon.Name = "chkUpdateTrayIcon";
      this.chkUpdateTrayIcon.Size = new System.Drawing.Size(321, 19);
      this.chkUpdateTrayIcon.TabIndex = 1;
      this.chkUpdateTrayIcon.Text = "Update MySQL Notifier tray icon based on service status";
      this.chkUpdateTrayIcon.UseVisualStyleBackColor = true;
      this.chkUpdateTrayIcon.CheckedChanged += new System.EventHandler(this.chkUpdateTrayIcon_CheckedChanged);
      // 
      // notifyOnStatusChange
      // 
      this.notifyOnStatusChange.AutoSize = true;
      this.notifyOnStatusChange.Enabled = false;
      this.notifyOnStatusChange.Location = new System.Drawing.Point(51, 22);
      this.notifyOnStatusChange.Name = "notifyOnStatusChange";
      this.notifyOnStatusChange.Size = new System.Drawing.Size(192, 19);
      this.notifyOnStatusChange.TabIndex = 0;
      this.notifyOnStatusChange.Text = "Notify me when status changes";
      this.notifyOnStatusChange.UseVisualStyleBackColor = true;
      this.notifyOnStatusChange.CheckedChanged += new System.EventHandler(this.notifyOnStatusChange_CheckedChanged);
      // 
      // btnDelete
      // 
      this.btnDelete.Enabled = false;
      this.btnDelete.Location = new System.Drawing.Point(497, 155);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new System.Drawing.Size(87, 27);
      this.btnDelete.TabIndex = 22;
      this.btnDelete.Text = "Delete";
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
      // 
      // btnAdd
      // 
      this.btnAdd.Location = new System.Drawing.Point(497, 122);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new System.Drawing.Size(87, 27);
      this.btnAdd.TabIndex = 21;
      this.btnAdd.Text = "Add...";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
      // 
      // lblHiperTitle
      // 
      this.lblHiperTitle.AutoSize = true;
      this.lblHiperTitle.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblHiperTitle.Location = new System.Drawing.Point(12, 18);
      this.lblHiperTitle.Name = "lblHiperTitle";
      this.lblHiperTitle.Size = new System.Drawing.Size(141, 15);
      this.lblHiperTitle.TabIndex = 20;
      this.lblHiperTitle.Text = "Manage Monitored Items";
      // 
      // MonitoredItemsListView
      // 
      this.MonitoredItemsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
      this.MonitoredItemsListView.FullRowSelect = true;
      this.MonitoredItemsListView.HideSelection = false;
      this.MonitoredItemsListView.Location = new System.Drawing.Point(33, 122);
      this.MonitoredItemsListView.MultiSelect = false;
      this.MonitoredItemsListView.Name = "MonitoredItemsListView";
      this.MonitoredItemsListView.Size = new System.Drawing.Size(447, 175);
      this.MonitoredItemsListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
      this.MonitoredItemsListView.TabIndex = 19;
      this.MonitoredItemsListView.UseCompatibleStateImageBehavior = false;
      this.MonitoredItemsListView.View = System.Windows.Forms.View.Details;
      this.MonitoredItemsListView.SelectedIndexChanged += new System.EventHandler(this.MonitoredItemsListView_SelectedIndexChanged);
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Name";
      this.columnHeader1.Width = 266;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Type";
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "Status";
      this.columnHeader3.Width = 114;
      // 
      // AddButtonContextMenuStrip
      // 
      this.AddButtonContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.serviceToolStripMenuItem,
            this.mySQLInstanceToolStripMenuItem});
      this.AddButtonContextMenuStrip.Name = "contextMenuStrip1";
      this.AddButtonContextMenuStrip.Size = new System.Drawing.Size(164, 48);
      // 
      // serviceToolStripMenuItem
      // 
      this.serviceToolStripMenuItem.Name = "serviceToolStripMenuItem";
      this.serviceToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
      this.serviceToolStripMenuItem.Text = "Windows Service";
      this.serviceToolStripMenuItem.Click += new System.EventHandler(this.serviceToolStripMenuItem_Click);
      // 
      // mySQLInstanceToolStripMenuItem
      // 
      this.mySQLInstanceToolStripMenuItem.Name = "mySQLInstanceToolStripMenuItem";
      this.mySQLInstanceToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
      this.mySQLInstanceToolStripMenuItem.Text = "MySQL Instance";
      this.mySQLInstanceToolStripMenuItem.Click += new System.EventHandler(this.mySQLInstanceToolStripMenuItem_Click);
      // 
      // ManageItemsDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.ClientSize = new System.Drawing.Size(596, 501);
      this.Controls.Add(this.panel2);
      this.Controls.Add(this.panel1);
      this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ManageItemsDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Manage Items";
      this.panel2.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.InstanceMonitorIntervalNumericUpDown)).EndInit();
      this.AddButtonContextMenuStrip.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.CheckBox chkUpdateTrayIcon;
    private System.Windows.Forms.CheckBox notifyOnStatusChange;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnAdd;
    private System.Windows.Forms.ListView MonitoredItemsListView;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.ContextMenuStrip AddButtonContextMenuStrip;
    private System.Windows.Forms.ToolStripMenuItem serviceToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem mySQLInstanceToolStripMenuItem;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label lblSubTitle1;
    private System.Windows.Forms.Label lblHiperTitle;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.Label InstanceMonitorIntervalLabel;
    private System.Windows.Forms.NumericUpDown InstanceMonitorIntervalNumericUpDown;
    private System.Windows.Forms.ComboBox InstanceMonitorIntervalUOMComboBox;
  }
}