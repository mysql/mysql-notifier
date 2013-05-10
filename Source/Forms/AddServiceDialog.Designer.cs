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
      this.DialogOKButton = new System.Windows.Forms.Button();
      this.DialogCancelButton = new System.Windows.Forms.Button();
      this.panel1 = new System.Windows.Forms.Panel();
      this.DeleteButton = new System.Windows.Forms.Button();
      this.EditButton = new System.Windows.Forms.Button();
      this.FilterTextBox = new System.Windows.Forms.TextBox();
      this.FilterLabel = new System.Windows.Forms.Label();
      this.WindowsServiceInstructionsLabel = new System.Windows.Forms.Label();
      this.MachineInstructionsLabel = new System.Windows.Forms.Label();
      this.MachineHyperTitleLabel = new System.Windows.Forms.Label();
      this.FilterCheckBox = new System.Windows.Forms.CheckBox();
      this.ServicesListView = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.WindowsServiceHyperTitleLabel = new System.Windows.Forms.Label();
      this.ComputerLabel = new System.Windows.Forms.Label();
      this.MachineSelectionComboBox = new System.Windows.Forms.ComboBox();
      this.timerForFiltering = new System.Windows.Forms.Timer(this.components);
      this.panel2.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // panel2
      // 
      this.panel2.Controls.Add(this.DialogOKButton);
      this.panel2.Controls.Add(this.DialogCancelButton);
      this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panel2.ForeColor = System.Drawing.SystemColors.ControlText;
      this.panel2.Location = new System.Drawing.Point(0, 492);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(529, 50);
      this.panel2.TabIndex = 1;
      // 
      // DialogOKButton
      // 
      this.DialogOKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogOKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.DialogOKButton.Location = new System.Drawing.Point(360, 15);
      this.DialogOKButton.Name = "DialogOKButton";
      this.DialogOKButton.Size = new System.Drawing.Size(75, 23);
      this.DialogOKButton.TabIndex = 0;
      this.DialogOKButton.Text = "OK";
      this.DialogOKButton.UseVisualStyleBackColor = true;
      this.DialogOKButton.Click += new System.EventHandler(this.DialogOKButton_Click);
      // 
      // DialogCancelButton
      // 
      this.DialogCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.DialogCancelButton.Location = new System.Drawing.Point(441, 15);
      this.DialogCancelButton.Name = "DialogCancelButton";
      this.DialogCancelButton.Size = new System.Drawing.Size(75, 23);
      this.DialogCancelButton.TabIndex = 1;
      this.DialogCancelButton.Text = "Cancel";
      this.DialogCancelButton.UseVisualStyleBackColor = true;
      // 
      // panel1
      // 
      this.panel1.AutoSize = true;
      this.panel1.BackColor = System.Drawing.Color.White;
      this.panel1.Controls.Add(this.DeleteButton);
      this.panel1.Controls.Add(this.EditButton);
      this.panel1.Controls.Add(this.FilterTextBox);
      this.panel1.Controls.Add(this.FilterLabel);
      this.panel1.Controls.Add(this.WindowsServiceInstructionsLabel);
      this.panel1.Controls.Add(this.MachineInstructionsLabel);
      this.panel1.Controls.Add(this.MachineHyperTitleLabel);
      this.panel1.Controls.Add(this.FilterCheckBox);
      this.panel1.Controls.Add(this.ServicesListView);
      this.panel1.Controls.Add(this.WindowsServiceHyperTitleLabel);
      this.panel1.Controls.Add(this.ComputerLabel);
      this.panel1.Controls.Add(this.MachineSelectionComboBox);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel1.Location = new System.Drawing.Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(529, 542);
      this.panel1.TabIndex = 0;
      // 
      // DeleteButton
      // 
      this.DeleteButton.Location = new System.Drawing.Point(441, 82);
      this.DeleteButton.Name = "DeleteButton";
      this.DeleteButton.Size = new System.Drawing.Size(75, 23);
      this.DeleteButton.TabIndex = 5;
      this.DeleteButton.Text = "Delete";
      this.DeleteButton.UseVisualStyleBackColor = true;
      this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
      // 
      // EditButton
      // 
      this.EditButton.Location = new System.Drawing.Point(360, 82);
      this.EditButton.Name = "EditButton";
      this.EditButton.Size = new System.Drawing.Size(75, 23);
      this.EditButton.TabIndex = 4;
      this.EditButton.Text = "Edit";
      this.EditButton.UseVisualStyleBackColor = true;
      this.EditButton.Click += new System.EventHandler(this.EditButton_Click);
      // 
      // FilterTextBox
      // 
      this.FilterTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.FilterTextBox.Location = new System.Drawing.Point(64, 186);
      this.FilterTextBox.Name = "FilterTextBox";
      this.FilterTextBox.Size = new System.Drawing.Size(171, 22);
      this.FilterTextBox.TabIndex = 9;
      this.FilterTextBox.TextChanged += new System.EventHandler(this.FilterTextBox_TextChanged);
      // 
      // FilterLabel
      // 
      this.FilterLabel.AutoSize = true;
      this.FilterLabel.Location = new System.Drawing.Point(22, 191);
      this.FilterLabel.Name = "FilterLabel";
      this.FilterLabel.Size = new System.Drawing.Size(36, 13);
      this.FilterLabel.TabIndex = 8;
      this.FilterLabel.Text = "Filter:";
      // 
      // WindowsServiceInstructionsLabel
      // 
      this.WindowsServiceInstructionsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.WindowsServiceInstructionsLabel.Location = new System.Drawing.Point(22, 156);
      this.WindowsServiceInstructionsLabel.Name = "WindowsServiceInstructionsLabel";
      this.WindowsServiceInstructionsLabel.Size = new System.Drawing.Size(495, 17);
      this.WindowsServiceInstructionsLabel.TabIndex = 7;
      this.WindowsServiceInstructionsLabel.Text = "Select the service you want to monitor. You can filter the list by typing into th" +
    "e filter control.";
      // 
      // MachineInstructionsLabel
      // 
      this.MachineInstructionsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.MachineInstructionsLabel.Location = new System.Drawing.Point(20, 46);
      this.MachineInstructionsLabel.Name = "MachineInstructionsLabel";
      this.MachineInstructionsLabel.Size = new System.Drawing.Size(496, 29);
      this.MachineInstructionsLabel.TabIndex = 1;
      this.MachineInstructionsLabel.Text = "Select the machine you want to monitor services on. The machines need to be on yo" +
    "ur local network. ";
      // 
      // MachineHyperTitleLabel
      // 
      this.MachineHyperTitleLabel.AutoSize = true;
      this.MachineHyperTitleLabel.Location = new System.Drawing.Point(20, 24);
      this.MachineHyperTitleLabel.Name = "MachineHyperTitleLabel";
      this.MachineHyperTitleLabel.Size = new System.Drawing.Size(105, 13);
      this.MachineHyperTitleLabel.TabIndex = 0;
      this.MachineHyperTitleLabel.Text = "Choose a Machine:";
      // 
      // FilterCheckBox
      // 
      this.FilterCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.FilterCheckBox.AutoSize = true;
      this.FilterCheckBox.Location = new System.Drawing.Point(253, 191);
      this.FilterCheckBox.Name = "FilterCheckBox";
      this.FilterCheckBox.Size = new System.Drawing.Size(264, 17);
      this.FilterCheckBox.TabIndex = 10;
      this.FilterCheckBox.Text = "Only show services that match auto-add filter?\r\n";
      this.FilterCheckBox.UseVisualStyleBackColor = true;
      this.FilterCheckBox.CheckedChanged += new System.EventHandler(this.FilterCheckBox_CheckedChanged);
      // 
      // ServicesListView
      // 
      this.ServicesListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.ServicesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3});
      this.ServicesListView.FullRowSelect = true;
      this.ServicesListView.Location = new System.Drawing.Point(23, 214);
      this.ServicesListView.Name = "ServicesListView";
      this.ServicesListView.Size = new System.Drawing.Size(493, 251);
      this.ServicesListView.TabIndex = 11;
      this.ServicesListView.UseCompatibleStateImageBehavior = false;
      this.ServicesListView.View = System.Windows.Forms.View.Details;
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Display Name";
      this.columnHeader1.Width = 367;
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "Status";
      this.columnHeader3.Width = 105;
      // 
      // WindowsServiceHyperTitleLabel
      // 
      this.WindowsServiceHyperTitleLabel.AutoSize = true;
      this.WindowsServiceHyperTitleLabel.Location = new System.Drawing.Point(22, 134);
      this.WindowsServiceHyperTitleLabel.Name = "WindowsServiceHyperTitleLabel";
      this.WindowsServiceHyperTitleLabel.Size = new System.Drawing.Size(148, 13);
      this.WindowsServiceHyperTitleLabel.TabIndex = 6;
      this.WindowsServiceHyperTitleLabel.Text = "Choose a Windows Service:";
      // 
      // ComputerLabel
      // 
      this.ComputerLabel.AutoSize = true;
      this.ComputerLabel.Location = new System.Drawing.Point(22, 85);
      this.ComputerLabel.Name = "ComputerLabel";
      this.ComputerLabel.Size = new System.Drawing.Size(61, 13);
      this.ComputerLabel.TabIndex = 2;
      this.ComputerLabel.Text = "Computer:";
      // 
      // MachineSelectionComboBox
      // 
      this.MachineSelectionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.MachineSelectionComboBox.FormattingEnabled = true;
      this.MachineSelectionComboBox.Items.AddRange(new object[] {
            "Local",
            "Add Remote...",
            "————————————————————————"});
      this.MachineSelectionComboBox.Location = new System.Drawing.Point(89, 82);
      this.MachineSelectionComboBox.Name = "MachineSelectionComboBox";
      this.MachineSelectionComboBox.Size = new System.Drawing.Size(265, 21);
      this.MachineSelectionComboBox.TabIndex = 3;
      this.MachineSelectionComboBox.SelectedIndexChanged += new System.EventHandler(this.MachineSelectionComboBox_SelectedIndexChanged);
      // 
      // timerForFiltering
      // 
      this.timerForFiltering.Interval = 600;
      this.timerForFiltering.Tick += new System.EventHandler(this.timerForFiltering_Tick);
      // 
      // AddServiceDialog
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.BackColor = System.Drawing.SystemColors.Menu;
      this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
      this.CancelButton = this.DialogCancelButton;
      this.ClientSize = new System.Drawing.Size(529, 542);
      this.Controls.Add(this.panel2);
      this.Controls.Add(this.panel1);
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(504, 450);
      this.Name = "AddServiceDialog";
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
    private System.Windows.Forms.Button DialogOKButton;
    private System.Windows.Forms.Button DialogCancelButton;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.TextBox FilterTextBox;
    private System.Windows.Forms.Label FilterLabel;
    private System.Windows.Forms.Label WindowsServiceInstructionsLabel;
    private System.Windows.Forms.Label MachineInstructionsLabel;
    private System.Windows.Forms.Label MachineHyperTitleLabel;
    private System.Windows.Forms.CheckBox FilterCheckBox;
    private System.Windows.Forms.ListView ServicesListView;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.Label WindowsServiceHyperTitleLabel;
    private System.Windows.Forms.Label ComputerLabel;
    private System.Windows.Forms.ComboBox MachineSelectionComboBox;
    private System.Windows.Forms.Timer timerForFiltering;
    private System.Windows.Forms.Button DeleteButton;
    private System.Windows.Forms.Button EditButton;
  }
}