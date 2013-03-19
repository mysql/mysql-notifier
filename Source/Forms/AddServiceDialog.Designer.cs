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
  partial class AddServiceDialog
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddServiceDialog));
      this.panel2 = new System.Windows.Forms.Panel();
      this.btnOK = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.panel1 = new System.Windows.Forms.Panel();
      this.txtFilter = new System.Windows.Forms.TextBox();
      this.lblText6 = new System.Windows.Forms.Label();
      this.lblText4 = new System.Windows.Forms.Label();
      this.lblText1 = new System.Windows.Forms.Label();
      this.lblSubTitle1 = new System.Windows.Forms.Label();
      this.filter = new System.Windows.Forms.CheckBox();
      this.lstServices = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.lblSubTitle2 = new System.Windows.Forms.Label();
      this.lblText3 = new System.Windows.Forms.Label();
      this.server = new System.Windows.Forms.ComboBox();
      this.timerForFiltering = new System.Windows.Forms.Timer(this.components);
      this.panel2.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // panel2
      // 
      this.panel2.Controls.Add(this.btnOK);
      this.panel2.Controls.Add(this.btnCancel);
      this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panel2.ForeColor = System.Drawing.SystemColors.ControlText;
      this.panel2.Location = new System.Drawing.Point(0, 468);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(488, 50);
      this.panel2.TabIndex = 46;
      // 
      // btnOK
      // 
      this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.Enabled = false;
      this.btnOK.Location = new System.Drawing.Point(319, 15);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 13;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(400, 15);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 12;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // panel1
      // 
      this.panel1.AutoSize = true;
      this.panel1.BackColor = System.Drawing.Color.White;
      this.panel1.Controls.Add(this.txtFilter);
      this.panel1.Controls.Add(this.lblText6);
      this.panel1.Controls.Add(this.lblText4);
      this.panel1.Controls.Add(this.lblText1);
      this.panel1.Controls.Add(this.lblSubTitle1);
      this.panel1.Controls.Add(this.filter);
      this.panel1.Controls.Add(this.lstServices);
      this.panel1.Controls.Add(this.lblSubTitle2);
      this.panel1.Controls.Add(this.lblText3);
      this.panel1.Controls.Add(this.server);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel1.Location = new System.Drawing.Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(488, 518);
      this.panel1.TabIndex = 47;
      // 
      // txtFilter
      // 
      this.txtFilter.Location = new System.Drawing.Point(63, 179);
      this.txtFilter.Name = "txtFilter";
      this.txtFilter.Size = new System.Drawing.Size(130, 22);
      this.txtFilter.TabIndex = 57;
      this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
      // 
      // lblText6
      // 
      this.lblText6.AutoSize = true;
      this.lblText6.Location = new System.Drawing.Point(21, 184);
      this.lblText6.Name = "lblText6";
      this.lblText6.Size = new System.Drawing.Size(36, 13);
      this.lblText6.TabIndex = 56;
      this.lblText6.Text = "Filter:";
      // 
      // lblText4
      // 
      this.lblText4.Location = new System.Drawing.Point(21, 142);
      this.lblText4.Name = "lblText4";
      this.lblText4.Size = new System.Drawing.Size(454, 38);
      this.lblText4.TabIndex = 54;
      this.lblText4.Text = "Select the service you want to monitor. You can filter the list by typing into th" +
    "e filter control.";
      // 
      // lblText1
      // 
      this.lblText1.Location = new System.Drawing.Point(20, 46);
      this.lblText1.Name = "lblText1";
      this.lblText1.Size = new System.Drawing.Size(455, 29);
      this.lblText1.TabIndex = 52;
      this.lblText1.Text = "Select the machine you want to monitor services on. The machines need to be on yo" +
    "ur local network. ";
      // 
      // lblSubTitle1
      // 
      this.lblSubTitle1.AutoSize = true;
      this.lblSubTitle1.Location = new System.Drawing.Point(20, 24);
      this.lblSubTitle1.Name = "lblSubTitle1";
      this.lblSubTitle1.Size = new System.Drawing.Size(105, 13);
      this.lblSubTitle1.TabIndex = 51;
      this.lblSubTitle1.Text = "Choose a Machine:";
      // 
      // filter
      // 
      this.filter.AutoSize = true;
      this.filter.Location = new System.Drawing.Point(211, 184);
      this.filter.Name = "filter";
      this.filter.Size = new System.Drawing.Size(264, 17);
      this.filter.TabIndex = 50;
      this.filter.Text = "Only show services that match auto-add filter?\r\n";
      this.filter.UseVisualStyleBackColor = true;
      this.filter.CheckedChanged += new System.EventHandler(this.filter_CheckedChanged);
      // 
      // lstServices
      // 
      this.lstServices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lstServices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3});
      this.lstServices.FullRowSelect = true;
      this.lstServices.Location = new System.Drawing.Point(23, 216);
      this.lstServices.Name = "lstServices";
      this.lstServices.Size = new System.Drawing.Size(452, 208);
      this.lstServices.TabIndex = 49;
      this.lstServices.UseCompatibleStateImageBehavior = false;
      this.lstServices.View = System.Windows.Forms.View.Details;
      this.lstServices.SelectedIndexChanged += new System.EventHandler(this.lstServices_SelectedIndexChanged);
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Display Name";
      this.columnHeader1.Width = 350;
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "Status";
      this.columnHeader3.Width = 102;
      // 
      // lblSubTitle2
      // 
      this.lblSubTitle2.AutoSize = true;
      this.lblSubTitle2.Location = new System.Drawing.Point(21, 120);
      this.lblSubTitle2.Name = "lblSubTitle2";
      this.lblSubTitle2.Size = new System.Drawing.Size(148, 13);
      this.lblSubTitle2.TabIndex = 48;
      this.lblSubTitle2.Text = "Choose a Windows Service:";
      // 
      // lblText3
      // 
      this.lblText3.AutoSize = true;
      this.lblText3.Location = new System.Drawing.Point(77, 81);
      this.lblText3.Name = "lblText3";
      this.lblText3.Size = new System.Drawing.Size(61, 13);
      this.lblText3.TabIndex = 47;
      this.lblText3.Text = "Computer:";
      // 
      // server
      // 
      this.server.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.server.FormattingEnabled = true;
      this.server.Items.AddRange(new object[] {
            "Local"});
      this.server.Location = new System.Drawing.Point(152, 78);
      this.server.Name = "server";
      this.server.Size = new System.Drawing.Size(259, 21);
      this.server.TabIndex = 46;
      // 
      // timerForFiltering
      // 
      this.timerForFiltering.Interval = 600;
      this.timerForFiltering.Tick += new System.EventHandler(this.timerForFiltering_Tick);
      // 
      // AddServiceDlg
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.Menu;
      this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
      this.ClientSize = new System.Drawing.Size(488, 518);
      this.Controls.Add(this.panel2);
      this.Controls.Add(this.panel1);
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(449, 454);
      this.Name = "AddServiceDlg";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Add Service";
      this.panel2.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.TextBox txtFilter;
    private System.Windows.Forms.Label lblText6;
    private System.Windows.Forms.Label lblText4;
    private System.Windows.Forms.Label lblText1;
    private System.Windows.Forms.Label lblSubTitle1;
    private System.Windows.Forms.CheckBox filter;
    private System.Windows.Forms.ListView lstServices;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.Label lblSubTitle2;
    private System.Windows.Forms.Label lblText3;
    private System.Windows.Forms.ComboBox server;
    private System.Windows.Forms.Timer timerForFiltering;
  }
}