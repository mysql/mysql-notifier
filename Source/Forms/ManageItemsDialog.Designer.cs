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

namespace MySql.Notifier.Forms
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
      this.AddButtonContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.ServiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.MySQLInstanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.CloseButton = new System.Windows.Forms.Button();
      this.ItemsTabControl = new System.Windows.Forms.TabControl();
      this.MonitoredServicesTabPage = new System.Windows.Forms.TabPage();
      this.MonitoredServicesListView = new System.Windows.Forms.ListView();
      this.ServiceNameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.ServiceMachineColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.ServiceStatusColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.MonitoredInstancesTabPage = new System.Windows.Forms.TabPage();
      this.MonitoredInstancesListView = new System.Windows.Forms.ListView();
      this.InstanceNameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.InstanceDriverColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.InstanceStatusColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.DescriptionLabel = new System.Windows.Forms.Label();
      this.SubTitleLabel = new System.Windows.Forms.Label();
      this.OptionsGroupBox = new System.Windows.Forms.GroupBox();
      this.InstanceMonitorIntervalUOMComboBox = new System.Windows.Forms.ComboBox();
      this.InstanceMonitorIntervalLabel = new System.Windows.Forms.Label();
      this.InstanceMonitorIntervalNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.UpdateTrayIconCheckBox = new System.Windows.Forms.CheckBox();
      this.NotifyOnStatusChangeCheckBox = new System.Windows.Forms.CheckBox();
      this.DeleteButton = new System.Windows.Forms.Button();
      this.AddButton = new System.Windows.Forms.Button();
      this.MonitoredItemsLabel = new System.Windows.Forms.Label();
      this.ContentAreaPanel.SuspendLayout();
      this.CommandAreaPanel.SuspendLayout();
      this.AddButtonContextMenuStrip.SuspendLayout();
      this.ItemsTabControl.SuspendLayout();
      this.MonitoredServicesTabPage.SuspendLayout();
      this.MonitoredInstancesTabPage.SuspendLayout();
      this.OptionsGroupBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.InstanceMonitorIntervalNumericUpDown)).BeginInit();
      this.SuspendLayout();
      // 
      // FootnoteAreaPanel
      // 
      this.FootnoteAreaPanel.Location = new System.Drawing.Point(0, 292);
      this.FootnoteAreaPanel.Size = new System.Drawing.Size(634, 0);
      // 
      // ContentAreaPanel
      // 
      this.ContentAreaPanel.Controls.Add(this.ItemsTabControl);
      this.ContentAreaPanel.Controls.Add(this.AddButton);
      this.ContentAreaPanel.Controls.Add(this.DescriptionLabel);
      this.ContentAreaPanel.Controls.Add(this.MonitoredItemsLabel);
      this.ContentAreaPanel.Controls.Add(this.SubTitleLabel);
      this.ContentAreaPanel.Controls.Add(this.DeleteButton);
      this.ContentAreaPanel.Controls.Add(this.OptionsGroupBox);
      this.ContentAreaPanel.Size = new System.Drawing.Size(596, 501);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.Controls.Add(this.CloseButton);
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 456);
      this.CommandAreaPanel.Size = new System.Drawing.Size(596, 45);
      // 
      // AddButtonContextMenuStrip
      // 
      this.AddButtonContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ServiceToolStripMenuItem,
            this.MySQLInstanceToolStripMenuItem});
      this.AddButtonContextMenuStrip.Name = "contextMenuStrip1";
      this.AddButtonContextMenuStrip.Size = new System.Drawing.Size(164, 48);
      // 
      // ServiceToolStripMenuItem
      // 
      this.ServiceToolStripMenuItem.Name = "ServiceToolStripMenuItem";
      this.ServiceToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
      this.ServiceToolStripMenuItem.Text = "Windows Service";
      this.ServiceToolStripMenuItem.Click += new System.EventHandler(this.ServiceToolStripMenuItem_Click);
      // 
      // MySQLInstanceToolStripMenuItem
      // 
      this.MySQLInstanceToolStripMenuItem.Name = "MySQLInstanceToolStripMenuItem";
      this.MySQLInstanceToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
      this.MySQLInstanceToolStripMenuItem.Text = "MySQL Instance";
      this.MySQLInstanceToolStripMenuItem.Click += new System.EventHandler(this.MySQLInstanceToolStripMenuItem_Click);
      // 
      // CloseButton
      // 
      this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.CloseButton.Location = new System.Drawing.Point(497, 11);
      this.CloseButton.Name = "CloseButton";
      this.CloseButton.Size = new System.Drawing.Size(87, 23);
      this.CloseButton.TabIndex = 0;
      this.CloseButton.Text = "Close";
      this.CloseButton.UseVisualStyleBackColor = true;
      // 
      // ItemsTabControl
      // 
      this.ItemsTabControl.Controls.Add(this.MonitoredServicesTabPage);
      this.ItemsTabControl.Controls.Add(this.MonitoredInstancesTabPage);
      this.ItemsTabControl.Location = new System.Drawing.Point(33, 131);
      this.ItemsTabControl.Name = "ItemsTabControl";
      this.ItemsTabControl.SelectedIndex = 0;
      this.ItemsTabControl.Size = new System.Drawing.Size(447, 175);
      this.ItemsTabControl.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
      this.ItemsTabControl.TabIndex = 3;
      this.ItemsTabControl.SelectedIndexChanged += new System.EventHandler(this.ItemsTabControl_SelectedIndexChanged);
      // 
      // MonitoredServicesTabPage
      // 
      this.MonitoredServicesTabPage.Controls.Add(this.MonitoredServicesListView);
      this.MonitoredServicesTabPage.Location = new System.Drawing.Point(4, 24);
      this.MonitoredServicesTabPage.Name = "MonitoredServicesTabPage";
      this.MonitoredServicesTabPage.Padding = new System.Windows.Forms.Padding(3);
      this.MonitoredServicesTabPage.Size = new System.Drawing.Size(439, 147);
      this.MonitoredServicesTabPage.TabIndex = 0;
      this.MonitoredServicesTabPage.Text = "Services";
      this.MonitoredServicesTabPage.UseVisualStyleBackColor = true;
      // 
      // MonitoredServicesListView
      // 
      this.MonitoredServicesListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.MonitoredServicesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ServiceNameColumnHeader,
            this.ServiceMachineColumnHeader,
            this.ServiceStatusColumnHeader});
      this.MonitoredServicesListView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.MonitoredServicesListView.FullRowSelect = true;
      this.MonitoredServicesListView.HideSelection = false;
      this.MonitoredServicesListView.Location = new System.Drawing.Point(3, 3);
      this.MonitoredServicesListView.MultiSelect = false;
      this.MonitoredServicesListView.Name = "MonitoredServicesListView";
      this.MonitoredServicesListView.Size = new System.Drawing.Size(433, 141);
      this.MonitoredServicesListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
      this.MonitoredServicesListView.TabIndex = 0;
      this.MonitoredServicesListView.UseCompatibleStateImageBehavior = false;
      this.MonitoredServicesListView.View = System.Windows.Forms.View.Details;
      this.MonitoredServicesListView.SelectedIndexChanged += new System.EventHandler(this.MonitoredServicesListView_SelectedIndexChanged);
      // 
      // ServiceNameColumnHeader
      // 
      this.ServiceNameColumnHeader.Text = "Name";
      this.ServiceNameColumnHeader.Width = 238;
      // 
      // ServiceMachineColumnHeader
      // 
      this.ServiceMachineColumnHeader.Text = "Running On";
      this.ServiceMachineColumnHeader.Width = 111;
      // 
      // ServiceStatusColumnHeader
      // 
      this.ServiceStatusColumnHeader.Text = "Status";
      this.ServiceStatusColumnHeader.Width = 84;
      // 
      // MonitoredInstancesTabPage
      // 
      this.MonitoredInstancesTabPage.Controls.Add(this.MonitoredInstancesListView);
      this.MonitoredInstancesTabPage.Location = new System.Drawing.Point(4, 24);
      this.MonitoredInstancesTabPage.Name = "MonitoredInstancesTabPage";
      this.MonitoredInstancesTabPage.Padding = new System.Windows.Forms.Padding(3);
      this.MonitoredInstancesTabPage.Size = new System.Drawing.Size(439, 147);
      this.MonitoredInstancesTabPage.TabIndex = 1;
      this.MonitoredInstancesTabPage.Text = "Instances";
      this.MonitoredInstancesTabPage.UseVisualStyleBackColor = true;
      // 
      // MonitoredInstancesListView
      // 
      this.MonitoredInstancesListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.MonitoredInstancesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.InstanceNameColumnHeader,
            this.InstanceDriverColumnHeader,
            this.InstanceStatusColumnHeader});
      this.MonitoredInstancesListView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.MonitoredInstancesListView.FullRowSelect = true;
      this.MonitoredInstancesListView.Location = new System.Drawing.Point(3, 3);
      this.MonitoredInstancesListView.MultiSelect = false;
      this.MonitoredInstancesListView.Name = "MonitoredInstancesListView";
      this.MonitoredInstancesListView.Size = new System.Drawing.Size(433, 141);
      this.MonitoredInstancesListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
      this.MonitoredInstancesListView.TabIndex = 26;
      this.MonitoredInstancesListView.UseCompatibleStateImageBehavior = false;
      this.MonitoredInstancesListView.View = System.Windows.Forms.View.Details;
      this.MonitoredInstancesListView.SelectedIndexChanged += new System.EventHandler(this.MonitoredInstancesListView_SelectedIndexChanged);
      // 
      // InstanceNameColumnHeader
      // 
      this.InstanceNameColumnHeader.Text = "Name";
      this.InstanceNameColumnHeader.Width = 222;
      // 
      // InstanceDriverColumnHeader
      // 
      this.InstanceDriverColumnHeader.Text = "DB Driver";
      this.InstanceDriverColumnHeader.Width = 83;
      // 
      // InstanceStatusColumnHeader
      // 
      this.InstanceStatusColumnHeader.Text = "Status";
      this.InstanceStatusColumnHeader.Width = 127;
      // 
      // DescriptionLabel
      // 
      this.DescriptionLabel.Location = new System.Drawing.Point(32, 82);
      this.DescriptionLabel.Name = "DescriptionLabel";
      this.DescriptionLabel.Size = new System.Drawing.Size(448, 37);
      this.DescriptionLabel.TabIndex = 2;
      this.DescriptionLabel.Text = "Local MySQL services will be automatically added. Press Add to monitor other Wind" +
    "ows services or MySQL instances.";
      // 
      // SubTitleLabel
      // 
      this.SubTitleLabel.AutoSize = true;
      this.SubTitleLabel.Location = new System.Drawing.Point(32, 54);
      this.SubTitleLabel.Name = "SubTitleLabel";
      this.SubTitleLabel.Size = new System.Drawing.Size(217, 15);
      this.SubTitleLabel.TabIndex = 1;
      this.SubTitleLabel.Text = "Windows Services and MySQL Instances";
      // 
      // OptionsGroupBox
      // 
      this.OptionsGroupBox.Controls.Add(this.InstanceMonitorIntervalUOMComboBox);
      this.OptionsGroupBox.Controls.Add(this.InstanceMonitorIntervalLabel);
      this.OptionsGroupBox.Controls.Add(this.InstanceMonitorIntervalNumericUpDown);
      this.OptionsGroupBox.Controls.Add(this.UpdateTrayIconCheckBox);
      this.OptionsGroupBox.Controls.Add(this.NotifyOnStatusChangeCheckBox);
      this.OptionsGroupBox.Location = new System.Drawing.Point(32, 322);
      this.OptionsGroupBox.Name = "OptionsGroupBox";
      this.OptionsGroupBox.Size = new System.Drawing.Size(448, 108);
      this.OptionsGroupBox.TabIndex = 4;
      this.OptionsGroupBox.TabStop = false;
      this.OptionsGroupBox.Text = "Options";
      // 
      // InstanceMonitorIntervalUOMComboBox
      // 
      this.InstanceMonitorIntervalUOMComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.InstanceMonitorIntervalUOMComboBox.Items.AddRange(new object[] {
            "seconds",
            "minutes",
            "hours",
            "days"});
      this.InstanceMonitorIntervalUOMComboBox.Location = new System.Drawing.Point(301, 71);
      this.InstanceMonitorIntervalUOMComboBox.Name = "InstanceMonitorIntervalUOMComboBox";
      this.InstanceMonitorIntervalUOMComboBox.Size = new System.Drawing.Size(116, 23);
      this.InstanceMonitorIntervalUOMComboBox.TabIndex = 4;
      this.InstanceMonitorIntervalUOMComboBox.SelectedIndexChanged += new System.EventHandler(this.InstanceMonitorIntervalUOMComboBox_SelectedIndexChanged);
      // 
      // InstanceMonitorIntervalLabel
      // 
      this.InstanceMonitorIntervalLabel.AutoSize = true;
      this.InstanceMonitorIntervalLabel.Location = new System.Drawing.Point(30, 74);
      this.InstanceMonitorIntervalLabel.Name = "InstanceMonitorIntervalLabel";
      this.InstanceMonitorIntervalLabel.Size = new System.Drawing.Size(203, 15);
      this.InstanceMonitorIntervalLabel.TabIndex = 2;
      this.InstanceMonitorIntervalLabel.Text = "Monitor MySQL Instance status every";
      // 
      // InstanceMonitorIntervalNumericUpDown
      // 
      this.InstanceMonitorIntervalNumericUpDown.Location = new System.Drawing.Point(244, 72);
      this.InstanceMonitorIntervalNumericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
      this.InstanceMonitorIntervalNumericUpDown.Name = "InstanceMonitorIntervalNumericUpDown";
      this.InstanceMonitorIntervalNumericUpDown.Size = new System.Drawing.Size(51, 23);
      this.InstanceMonitorIntervalNumericUpDown.TabIndex = 3;
      this.InstanceMonitorIntervalNumericUpDown.ValueChanged += new System.EventHandler(this.InstanceMonitorIntervalNumericUpDown_ValueChanged);
      // 
      // UpdateTrayIconCheckBox
      // 
      this.UpdateTrayIconCheckBox.AutoSize = true;
      this.UpdateTrayIconCheckBox.Enabled = false;
      this.UpdateTrayIconCheckBox.Location = new System.Drawing.Point(33, 47);
      this.UpdateTrayIconCheckBox.Name = "UpdateTrayIconCheckBox";
      this.UpdateTrayIconCheckBox.Size = new System.Drawing.Size(321, 19);
      this.UpdateTrayIconCheckBox.TabIndex = 1;
      this.UpdateTrayIconCheckBox.Text = "Update MySQL Notifier tray icon based on service status";
      this.UpdateTrayIconCheckBox.UseVisualStyleBackColor = true;
      this.UpdateTrayIconCheckBox.CheckedChanged += new System.EventHandler(this.UpdateTrayIconCheckBox_CheckedChanged);
      // 
      // NotifyOnStatusChangeCheckBox
      // 
      this.NotifyOnStatusChangeCheckBox.AutoSize = true;
      this.NotifyOnStatusChangeCheckBox.Enabled = false;
      this.NotifyOnStatusChangeCheckBox.Location = new System.Drawing.Point(33, 22);
      this.NotifyOnStatusChangeCheckBox.Name = "NotifyOnStatusChangeCheckBox";
      this.NotifyOnStatusChangeCheckBox.Size = new System.Drawing.Size(192, 19);
      this.NotifyOnStatusChangeCheckBox.TabIndex = 0;
      this.NotifyOnStatusChangeCheckBox.Text = "Notify me when status changes";
      this.NotifyOnStatusChangeCheckBox.UseVisualStyleBackColor = true;
      this.NotifyOnStatusChangeCheckBox.CheckedChanged += new System.EventHandler(this.NotifyOnStatusChangeCheckBox_CheckedChanged);
      // 
      // DeleteButton
      // 
      this.DeleteButton.Enabled = false;
      this.DeleteButton.Location = new System.Drawing.Point(497, 184);
      this.DeleteButton.Name = "DeleteButton";
      this.DeleteButton.Size = new System.Drawing.Size(87, 23);
      this.DeleteButton.TabIndex = 6;
      this.DeleteButton.Text = "Delete";
      this.DeleteButton.UseVisualStyleBackColor = true;
      this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
      // 
      // AddButton
      // 
      this.AddButton.Location = new System.Drawing.Point(497, 155);
      this.AddButton.Name = "AddButton";
      this.AddButton.Size = new System.Drawing.Size(87, 23);
      this.AddButton.TabIndex = 5;
      this.AddButton.Text = "Add...";
      this.AddButton.UseVisualStyleBackColor = true;
      this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
      // 
      // MonitoredItemsLabel
      // 
      this.MonitoredItemsLabel.AutoSize = true;
      this.MonitoredItemsLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.MonitoredItemsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(73)))), ((int)(((byte)(161)))));
      this.MonitoredItemsLabel.Location = new System.Drawing.Point(12, 27);
      this.MonitoredItemsLabel.Name = "MonitoredItemsLabel";
      this.MonitoredItemsLabel.Size = new System.Drawing.Size(185, 21);
      this.MonitoredItemsLabel.TabIndex = 0;
      this.MonitoredItemsLabel.Text = "Manage Monitored Items";
      // 
      // ManageItemsDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.ClientSize = new System.Drawing.Size(596, 501);
      this.CommandAreaVisible = true;
      this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
      this.FootnoteAreaHeight = 0;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "ManageItemsDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Manage Items";
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ManageItemsDialog_FormClosed);
      this.Controls.SetChildIndex(this.ContentAreaPanel, 0);
      this.Controls.SetChildIndex(this.FootnoteAreaPanel, 0);
      this.Controls.SetChildIndex(this.CommandAreaPanel, 0);
      this.ContentAreaPanel.ResumeLayout(false);
      this.ContentAreaPanel.PerformLayout();
      this.CommandAreaPanel.ResumeLayout(false);
      this.AddButtonContextMenuStrip.ResumeLayout(false);
      this.ItemsTabControl.ResumeLayout(false);
      this.MonitoredServicesTabPage.ResumeLayout(false);
      this.MonitoredInstancesTabPage.ResumeLayout(false);
      this.OptionsGroupBox.ResumeLayout(false);
      this.OptionsGroupBox.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.InstanceMonitorIntervalNumericUpDown)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ContextMenuStrip AddButtonContextMenuStrip;
    private System.Windows.Forms.ToolStripMenuItem ServiceToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem MySQLInstanceToolStripMenuItem;
    private System.Windows.Forms.TabControl ItemsTabControl;
    private System.Windows.Forms.TabPage MonitoredServicesTabPage;
    private System.Windows.Forms.ListView MonitoredServicesListView;
    private System.Windows.Forms.ColumnHeader ServiceNameColumnHeader;
    private System.Windows.Forms.ColumnHeader ServiceMachineColumnHeader;
    private System.Windows.Forms.ColumnHeader ServiceStatusColumnHeader;
    private System.Windows.Forms.TabPage MonitoredInstancesTabPage;
    private System.Windows.Forms.ListView MonitoredInstancesListView;
    private System.Windows.Forms.ColumnHeader InstanceNameColumnHeader;
    private System.Windows.Forms.ColumnHeader InstanceDriverColumnHeader;
    private System.Windows.Forms.ColumnHeader InstanceStatusColumnHeader;
    private System.Windows.Forms.Button AddButton;
    private System.Windows.Forms.Label DescriptionLabel;
    private System.Windows.Forms.Label MonitoredItemsLabel;
    private System.Windows.Forms.Label SubTitleLabel;
    private System.Windows.Forms.Button DeleteButton;
    private System.Windows.Forms.GroupBox OptionsGroupBox;
    private System.Windows.Forms.ComboBox InstanceMonitorIntervalUOMComboBox;
    private System.Windows.Forms.Label InstanceMonitorIntervalLabel;
    private System.Windows.Forms.NumericUpDown InstanceMonitorIntervalNumericUpDown;
    private System.Windows.Forms.CheckBox UpdateTrayIconCheckBox;
    private System.Windows.Forms.CheckBox NotifyOnStatusChangeCheckBox;
    private System.Windows.Forms.Button CloseButton;
  }
}