// 
// Copyright (c) 2012, Oracle and/or its affiliates. All rights reserved.
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
  partial class ManageServicesDlg
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageServicesDlg));
      this.panel2 = new System.Windows.Forms.Panel();
      this.btnClose = new System.Windows.Forms.Button();
      this.panel1 = new System.Windows.Forms.Panel();
      this.label3 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.lblSubTitle1 = new System.Windows.Forms.Label();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.chkUpdateTrayIcon = new System.Windows.Forms.CheckBox();
      this.notifyOnStatusChange = new System.Windows.Forms.CheckBox();
      this.btnDelete = new System.Windows.Forms.Button();
      this.btnAdd = new System.Windows.Forms.Button();
      this.lblHiperTitle = new System.Windows.Forms.Label();
      this.lstMonitoredServices = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.panel2.SuspendLayout();
      this.panel1.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // panel2
      // 
      this.panel2.Controls.Add(this.btnClose);
      this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panel2.ForeColor = System.Drawing.SystemColors.ControlText;
      this.panel2.Location = new System.Drawing.Point(0, 432);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(596, 48);
      this.panel2.TabIndex = 47;
      // 
      // btnClose
      // 
      this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnClose.Location = new System.Drawing.Point(502, 9);
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
      this.panel1.Controls.Add(this.label3);
      this.panel1.Controls.Add(this.label2);
      this.panel1.Controls.Add(this.lblSubTitle1);
      this.panel1.Controls.Add(this.groupBox1);
      this.panel1.Controls.Add(this.btnDelete);
      this.panel1.Controls.Add(this.btnAdd);
      this.panel1.Controls.Add(this.lblHiperTitle);
      this.panel1.Controls.Add(this.lstMonitoredServices);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel1.Location = new System.Drawing.Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(596, 480);
      this.panel1.TabIndex = 48;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(33, 88);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(162, 15);
      this.label3.TabIndex = 26;
      this.label3.Text = "other local Windows services.";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(33, 73);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(394, 15);
      this.label2.TabIndex = 25;
      this.label2.Text = "Local MySQL instances will be automatically added. Press Add to monitor";
      // 
      // lblSubTitle1
      // 
      this.lblSubTitle1.AutoSize = true;
      this.lblSubTitle1.Location = new System.Drawing.Point(33, 48);
      this.lblSubTitle1.Name = "lblSubTitle1";
      this.lblSubTitle1.Size = new System.Drawing.Size(101, 15);
      this.lblSubTitle1.TabIndex = 24;
      this.lblSubTitle1.Text = "Windows Services";
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.chkUpdateTrayIcon);
      this.groupBox1.Controls.Add(this.notifyOnStatusChange);
      this.groupBox1.Location = new System.Drawing.Point(33, 304);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(448, 88);
      this.groupBox1.TabIndex = 23;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Service Options";
      // 
      // chkUpdateTrayIcon
      // 
      this.chkUpdateTrayIcon.AutoSize = true;
      this.chkUpdateTrayIcon.Enabled = false;
      this.chkUpdateTrayIcon.Location = new System.Drawing.Point(30, 48);
      this.chkUpdateTrayIcon.Name = "chkUpdateTrayIcon";
      this.chkUpdateTrayIcon.Size = new System.Drawing.Size(293, 17);
      this.chkUpdateTrayIcon.TabIndex = 1;
      this.chkUpdateTrayIcon.Text = "Update MySQL Notifier tray icon based on service status";
      this.chkUpdateTrayIcon.UseVisualStyleBackColor = true;
      this.chkUpdateTrayIcon.CheckedChanged += new System.EventHandler(this.chkUpdateTrayIcon_CheckedChanged);
      // 
      // notifyOnStatusChange
      // 
      this.notifyOnStatusChange.AutoSize = true;
      this.notifyOnStatusChange.Enabled = false;
      this.notifyOnStatusChange.Location = new System.Drawing.Point(30, 22);
      this.notifyOnStatusChange.Name = "notifyOnStatusChange";
      this.notifyOnStatusChange.Size = new System.Drawing.Size(174, 17);
      this.notifyOnStatusChange.TabIndex = 0;
      this.notifyOnStatusChange.Text = "Notify me when status changes";
      this.notifyOnStatusChange.UseVisualStyleBackColor = true;
      this.notifyOnStatusChange.CheckedChanged += new System.EventHandler(this.notifyOnStatusChange_CheckedChanged);
      // 
      // btnDelete
      // 
      this.btnDelete.Enabled = false;
      this.btnDelete.Location = new System.Drawing.Point(502, 159);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new System.Drawing.Size(87, 27);
      this.btnDelete.TabIndex = 22;
      this.btnDelete.Text = "Delete";
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
      // 
      // btnAdd
      // 
      this.btnAdd.Location = new System.Drawing.Point(502, 125);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new System.Drawing.Size(87, 27);
      this.btnAdd.TabIndex = 21;
      this.btnAdd.Text = "Add";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
      // 
      // lblHiperTitle
      // 
      this.lblHiperTitle.AutoSize = true;
      this.lblHiperTitle.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblHiperTitle.Location = new System.Drawing.Point(12, 19);
      this.lblHiperTitle.Name = "lblHiperTitle";
      this.lblHiperTitle.Size = new System.Drawing.Size(154, 15);
      this.lblHiperTitle.TabIndex = 20;
      this.lblHiperTitle.Text = "Manage Monitored Services";
      // 
      // lstMonitoredServices
      // 
      this.lstMonitoredServices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3});
      this.lstMonitoredServices.FullRowSelect = true;
      this.lstMonitoredServices.HideSelection = false;
      this.lstMonitoredServices.Location = new System.Drawing.Point(33, 122);
      this.lstMonitoredServices.MultiSelect = false;
      this.lstMonitoredServices.Name = "lstMonitoredServices";
      this.lstMonitoredServices.Size = new System.Drawing.Size(447, 175);
      this.lstMonitoredServices.TabIndex = 19;
      this.lstMonitoredServices.UseCompatibleStateImageBehavior = false;
      this.lstMonitoredServices.View = System.Windows.Forms.View.Details;
      this.lstMonitoredServices.SelectedIndexChanged += new System.EventHandler(this.lstMonitoredServices_SelectedIndexChanged);
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Service Name";
      this.columnHeader1.Width = 301;
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "Status";
      this.columnHeader3.Width = 127;
      // 
      // ManageServicesDlg
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.ClientSize = new System.Drawing.Size(596, 480);
      this.Controls.Add(this.panel2);
      this.Controls.Add(this.panel1);
      this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ManageServicesDlg";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Manage Services";
      this.panel2.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label lblSubTitle1;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.CheckBox chkUpdateTrayIcon;
    private System.Windows.Forms.CheckBox notifyOnStatusChange;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnAdd;
    private System.Windows.Forms.Label lblHiperTitle;
    private System.Windows.Forms.ListView lstMonitoredServices;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader3;
  }
}