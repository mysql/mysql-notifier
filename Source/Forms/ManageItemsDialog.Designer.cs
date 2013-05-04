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
      this.CloseButton = new System.Windows.Forms.Button();
      this.panel1 = new System.Windows.Forms.Panel();
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
      this.MonitoredItemsHyperTitleLabel = new System.Windows.Forms.Label();
      this.AddButtonContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.ServiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.MySQLInstanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.panel2.SuspendLayout();
      this.panel1.SuspendLayout();
      this.ItemsTabControl.SuspendLayout();
      this.MonitoredServicesTabPage.SuspendLayout();
      this.MonitoredInstancesTabPage.SuspendLayout();
      this.OptionsGroupBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.InstanceMonitorIntervalNumericUpDown)).BeginInit();
      this.AddButtonContextMenuStrip.SuspendLayout();
      this.SuspendLayout();
      // 
      // panel2
      // 
      this.panel2.Controls.Add(this.CloseButton);
      this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panel2.ForeColor = System.Drawing.SystemColors.ControlText;
      this.panel2.Location = new System.Drawing.Point(0, 453);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(596, 48);
      this.panel2.TabIndex = 47;
      // 
      // CloseButton
      // 
      this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.CloseButton.Location = new System.Drawing.Point(497, 9);
      this.CloseButton.Name = "CloseButton";
      this.CloseButton.Size = new System.Drawing.Size(87, 27);
      this.CloseButton.TabIndex = 15;
      this.CloseButton.Text = "Close";
      this.CloseButton.UseVisualStyleBackColor = true;
      this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
      // 
      // panel1
      // 
      this.panel1.AutoSize = true;
      this.panel1.BackColor = System.Drawing.Color.White;
      this.panel1.Controls.Add(this.ItemsTabControl);
      this.panel1.Controls.Add(this.DescriptionLabel);
      this.panel1.Controls.Add(this.SubTitleLabel);
      this.panel1.Controls.Add(this.OptionsGroupBox);
      this.panel1.Controls.Add(this.DeleteButton);
      this.panel1.Controls.Add(this.AddButton);
      this.panel1.Controls.Add(this.MonitoredItemsHyperTitleLabel);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel1.Location = new System.Drawing.Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(596, 501);
      this.panel1.TabIndex = 48;
      // 
      // ItemsTabControl
      // 
      this.ItemsTabControl.Controls.Add(this.MonitoredServicesTabPage);
      this.ItemsTabControl.Controls.Add(this.MonitoredInstancesTabPage);
      this.ItemsTabControl.Location = new System.Drawing.Point(33, 122);
      this.ItemsTabControl.Name = "ItemsTabControl";
      this.ItemsTabControl.SelectedIndex = 0;
      this.ItemsTabControl.Size = new System.Drawing.Size(447, 175);
      this.ItemsTabControl.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
      this.ItemsTabControl.TabIndex = 27;
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
      this.MonitoredServicesListView.TabIndex = 19;
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
      this.DescriptionLabel.Location = new System.Drawing.Point(32, 73);
      this.DescriptionLabel.Name = "DescriptionLabel";
      this.DescriptionLabel.Size = new System.Drawing.Size(448, 37);
      this.DescriptionLabel.TabIndex = 25;
      this.DescriptionLabel.Text = "Local MySQL services will be automatically added. Press Add to monitor other Wind" +
    "ows services or MySQL instances.";
      // 
      // SubTitleLabel
      // 
      this.SubTitleLabel.AutoSize = true;
      this.SubTitleLabel.Location = new System.Drawing.Point(32, 45);
      this.SubTitleLabel.Name = "SubTitleLabel";
      this.SubTitleLabel.Size = new System.Drawing.Size(217, 15);
      this.SubTitleLabel.TabIndex = 24;
      this.SubTitleLabel.Text = "Windows Services and MySQL Instances";
      // 
      // OptionsGroupBox
      // 
      this.OptionsGroupBox.Controls.Add(this.InstanceMonitorIntervalUOMComboBox);
      this.OptionsGroupBox.Controls.Add(this.InstanceMonitorIntervalLabel);
      this.OptionsGroupBox.Controls.Add(this.InstanceMonitorIntervalNumericUpDown);
      this.OptionsGroupBox.Controls.Add(this.UpdateTrayIconCheckBox);
      this.OptionsGroupBox.Controls.Add(this.NotifyOnStatusChangeCheckBox);
      this.OptionsGroupBox.Location = new System.Drawing.Point(32, 313);
      this.OptionsGroupBox.Name = "OptionsGroupBox";
      this.OptionsGroupBox.Size = new System.Drawing.Size(448, 108);
      this.OptionsGroupBox.TabIndex = 23;
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
      this.InstanceMonitorIntervalUOMComboBox.Location = new System.Drawing.Point(296, 71);
      this.InstanceMonitorIntervalUOMComboBox.Name = "InstanceMonitorIntervalUOMComboBox";
      this.InstanceMonitorIntervalUOMComboBox.Size = new System.Drawing.Size(121, 23);
      this.InstanceMonitorIntervalUOMComboBox.TabIndex = 4;
      this.InstanceMonitorIntervalUOMComboBox.SelectedIndexChanged += new System.EventHandler(this.InstanceMonitorIntervalUOMComboBox_SelectedIndexChanged);
      // 
      // InstanceMonitorIntervalLabel
      // 
      this.InstanceMonitorIntervalLabel.AutoSize = true;
      this.InstanceMonitorIntervalLabel.Location = new System.Drawing.Point(30, 74);
      this.InstanceMonitorIntervalLabel.Name = "InstanceMonitorIntervalLabel";
      this.InstanceMonitorIntervalLabel.Size = new System.Drawing.Size(203, 15);
      this.InstanceMonitorIntervalLabel.TabIndex = 3;
      this.InstanceMonitorIntervalLabel.Text = "Monitor MySQL Instance status every";
      // 
      // InstanceMonitorIntervalNumericUpDown
      // 
      this.InstanceMonitorIntervalNumericUpDown.Location = new System.Drawing.Point(239, 72);
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
      this.DeleteButton.Location = new System.Drawing.Point(497, 179);
      this.DeleteButton.Name = "DeleteButton";
      this.DeleteButton.Size = new System.Drawing.Size(87, 27);
      this.DeleteButton.TabIndex = 22;
      this.DeleteButton.Text = "Delete";
      this.DeleteButton.UseVisualStyleBackColor = true;
      this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
      // 
      // AddButton
      // 
      this.AddButton.Location = new System.Drawing.Point(497, 146);
      this.AddButton.Name = "AddButton";
      this.AddButton.Size = new System.Drawing.Size(87, 27);
      this.AddButton.TabIndex = 21;
      this.AddButton.Text = "Add...";
      this.AddButton.UseVisualStyleBackColor = true;
      this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
      // 
      // MonitoredItemsHyperTitleLabel
      // 
      this.MonitoredItemsHyperTitleLabel.AutoSize = true;
      this.MonitoredItemsHyperTitleLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.MonitoredItemsHyperTitleLabel.Location = new System.Drawing.Point(12, 18);
      this.MonitoredItemsHyperTitleLabel.Name = "MonitoredItemsHyperTitleLabel";
      this.MonitoredItemsHyperTitleLabel.Size = new System.Drawing.Size(141, 15);
      this.MonitoredItemsHyperTitleLabel.TabIndex = 20;
      this.MonitoredItemsHyperTitleLabel.Text = "Manage Monitored Items";
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
      this.ItemsTabControl.ResumeLayout(false);
      this.MonitoredServicesTabPage.ResumeLayout(false);
      this.MonitoredInstancesTabPage.ResumeLayout(false);
      this.OptionsGroupBox.ResumeLayout(false);
      this.OptionsGroupBox.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.InstanceMonitorIntervalNumericUpDown)).EndInit();
      this.AddButtonContextMenuStrip.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Button CloseButton;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.GroupBox OptionsGroupBox;
    private System.Windows.Forms.CheckBox UpdateTrayIconCheckBox;
    private System.Windows.Forms.CheckBox NotifyOnStatusChangeCheckBox;
    private System.Windows.Forms.Button DeleteButton;
    private System.Windows.Forms.Button AddButton;
    private System.Windows.Forms.ListView MonitoredServicesListView;
    private System.Windows.Forms.ColumnHeader ServiceNameColumnHeader;
    private System.Windows.Forms.ColumnHeader ServiceStatusColumnHeader;
    private System.Windows.Forms.ContextMenuStrip AddButtonContextMenuStrip;
    private System.Windows.Forms.ToolStripMenuItem ServiceToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem MySQLInstanceToolStripMenuItem;
    private System.Windows.Forms.Label DescriptionLabel;
    private System.Windows.Forms.Label SubTitleLabel;
    private System.Windows.Forms.Label MonitoredItemsHyperTitleLabel;
    private System.Windows.Forms.ColumnHeader ServiceMachineColumnHeader;
    private System.Windows.Forms.Label InstanceMonitorIntervalLabel;
    private System.Windows.Forms.NumericUpDown InstanceMonitorIntervalNumericUpDown;
    private System.Windows.Forms.ComboBox InstanceMonitorIntervalUOMComboBox;
    private System.Windows.Forms.TabControl ItemsTabControl;
    private System.Windows.Forms.TabPage MonitoredServicesTabPage;
    private System.Windows.Forms.TabPage MonitoredInstancesTabPage;
    private System.Windows.Forms.ListView MonitoredInstancesListView;
    private System.Windows.Forms.ColumnHeader InstanceNameColumnHeader;
    private System.Windows.Forms.ColumnHeader InstanceDriverColumnHeader;
    private System.Windows.Forms.ColumnHeader InstanceStatusColumnHeader;
  }
}