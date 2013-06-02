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
      this.timerForFiltering = new System.Windows.Forms.Timer(this.components);
      this.DialogOKButton = new System.Windows.Forms.Button();
      this.DialogCancelButton = new System.Windows.Forms.Button();
      this.DeleteButton = new System.Windows.Forms.Button();
      this.EditButton = new System.Windows.Forms.Button();
      this.FilterTextBox = new System.Windows.Forms.TextBox();
      this.FilterLabel = new System.Windows.Forms.Label();
      this.WindowsServiceInstructionsLabel = new System.Windows.Forms.Label();
      this.MachineInstructionsLabel = new System.Windows.Forms.Label();
      this.ChooseMachineLabel = new System.Windows.Forms.Label();
      this.FilterCheckBox = new System.Windows.Forms.CheckBox();
      this.ServicesListView = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.ChooseWindowsServiceLabel = new System.Windows.Forms.Label();
      this.ComputerLabel = new System.Windows.Forms.Label();
      this.MachineSelectionComboBox = new System.Windows.Forms.ComboBox();
      this.ContentAreaPanel.SuspendLayout();
      this.CommandAreaPanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // FootnoteAreaPanel
      // 
      this.FootnoteAreaPanel.BackColor = System.Drawing.SystemColors.Menu;
      this.FootnoteAreaPanel.Location = new System.Drawing.Point(0, 292);
      this.FootnoteAreaPanel.Size = new System.Drawing.Size(634, 0);
      // 
      // ContentAreaPanel
      // 
      this.ContentAreaPanel.Controls.Add(this.DeleteButton);
      this.ContentAreaPanel.Controls.Add(this.EditButton);
      this.ContentAreaPanel.Controls.Add(this.FilterTextBox);
      this.ContentAreaPanel.Controls.Add(this.FilterLabel);
      this.ContentAreaPanel.Controls.Add(this.WindowsServiceInstructionsLabel);
      this.ContentAreaPanel.Controls.Add(this.MachineInstructionsLabel);
      this.ContentAreaPanel.Controls.Add(this.ChooseMachineLabel);
      this.ContentAreaPanel.Controls.Add(this.FilterCheckBox);
      this.ContentAreaPanel.Controls.Add(this.ServicesListView);
      this.ContentAreaPanel.Controls.Add(this.ChooseWindowsServiceLabel);
      this.ContentAreaPanel.Controls.Add(this.ComputerLabel);
      this.ContentAreaPanel.Controls.Add(this.MachineSelectionComboBox);
      this.ContentAreaPanel.Size = new System.Drawing.Size(529, 542);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.Controls.Add(this.DialogOKButton);
      this.CommandAreaPanel.Controls.Add(this.DialogCancelButton);
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 497);
      this.CommandAreaPanel.Size = new System.Drawing.Size(529, 45);
      // 
      // timerForFiltering
      // 
      this.timerForFiltering.Interval = 600;
      this.timerForFiltering.Tick += new System.EventHandler(this.timerForFiltering_Tick);
      // 
      // DialogOKButton
      // 
      this.DialogOKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogOKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.DialogOKButton.Location = new System.Drawing.Point(360, 11);
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
      this.DialogCancelButton.Location = new System.Drawing.Point(441, 11);
      this.DialogCancelButton.Name = "DialogCancelButton";
      this.DialogCancelButton.Size = new System.Drawing.Size(75, 23);
      this.DialogCancelButton.TabIndex = 1;
      this.DialogCancelButton.Text = "Cancel";
      this.DialogCancelButton.UseVisualStyleBackColor = true;
      // 
      // DeleteButton
      // 
      this.DeleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.DeleteButton.Location = new System.Drawing.Point(441, 89);
      this.DeleteButton.Name = "DeleteButton";
      this.DeleteButton.Size = new System.Drawing.Size(75, 23);
      this.DeleteButton.TabIndex = 5;
      this.DeleteButton.Text = "Delete";
      this.DeleteButton.UseVisualStyleBackColor = true;
      this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
      // 
      // EditButton
      // 
      this.EditButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.EditButton.Location = new System.Drawing.Point(360, 89);
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
      this.FilterTextBox.Location = new System.Drawing.Point(64, 193);
      this.FilterTextBox.Name = "FilterTextBox";
      this.FilterTextBox.Size = new System.Drawing.Size(171, 22);
      this.FilterTextBox.TabIndex = 9;
      this.FilterTextBox.TextChanged += new System.EventHandler(this.FilterTextBox_TextChanged);
      // 
      // FilterLabel
      // 
      this.FilterLabel.AutoSize = true;
      this.FilterLabel.Location = new System.Drawing.Point(22, 198);
      this.FilterLabel.Name = "FilterLabel";
      this.FilterLabel.Size = new System.Drawing.Size(36, 13);
      this.FilterLabel.TabIndex = 8;
      this.FilterLabel.Text = "Filter:";
      // 
      // WindowsServiceInstructionsLabel
      // 
      this.WindowsServiceInstructionsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.WindowsServiceInstructionsLabel.Location = new System.Drawing.Point(22, 163);
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
      this.MachineInstructionsLabel.Location = new System.Drawing.Point(20, 53);
      this.MachineInstructionsLabel.Name = "MachineInstructionsLabel";
      this.MachineInstructionsLabel.Size = new System.Drawing.Size(496, 29);
      this.MachineInstructionsLabel.TabIndex = 1;
      this.MachineInstructionsLabel.Text = "Select the machine you want to monitor services on. The machines need to be on yo" +
    "ur local network. ";
      // 
      // ChooseMachineLabel
      // 
      this.ChooseMachineLabel.AutoSize = true;
      this.ChooseMachineLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ChooseMachineLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(73)))), ((int)(((byte)(161)))));
      this.ChooseMachineLabel.Location = new System.Drawing.Point(19, 26);
      this.ChooseMachineLabel.Name = "ChooseMachineLabel";
      this.ChooseMachineLabel.Size = new System.Drawing.Size(140, 21);
      this.ChooseMachineLabel.TabIndex = 0;
      this.ChooseMachineLabel.Text = "Choose a Machine:";
      // 
      // FilterCheckBox
      // 
      this.FilterCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.FilterCheckBox.AutoSize = true;
      this.FilterCheckBox.Location = new System.Drawing.Point(253, 198);
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
      this.ServicesListView.Location = new System.Drawing.Point(23, 221);
      this.ServicesListView.Name = "ServicesListView";
      this.ServicesListView.Size = new System.Drawing.Size(493, 250);
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
      // ChooseWindowsServiceLabel
      // 
      this.ChooseWindowsServiceLabel.AutoSize = true;
      this.ChooseWindowsServiceLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ChooseWindowsServiceLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(73)))), ((int)(((byte)(161)))));
      this.ChooseWindowsServiceLabel.Location = new System.Drawing.Point(21, 137);
      this.ChooseWindowsServiceLabel.Name = "ChooseWindowsServiceLabel";
      this.ChooseWindowsServiceLabel.Size = new System.Drawing.Size(200, 21);
      this.ChooseWindowsServiceLabel.TabIndex = 6;
      this.ChooseWindowsServiceLabel.Text = "Choose a Windows Service:";
      // 
      // ComputerLabel
      // 
      this.ComputerLabel.AutoSize = true;
      this.ComputerLabel.Location = new System.Drawing.Point(22, 92);
      this.ComputerLabel.Name = "ComputerLabel";
      this.ComputerLabel.Size = new System.Drawing.Size(61, 13);
      this.ComputerLabel.TabIndex = 2;
      this.ComputerLabel.Text = "Computer:";
      // 
      // MachineSelectionComboBox
      // 
      this.MachineSelectionComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.MachineSelectionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.MachineSelectionComboBox.FormattingEnabled = true;
      this.MachineSelectionComboBox.Items.AddRange(new object[] {
            "Local",
            "Add Remote...",
            "————————————————————————"});
      this.MachineSelectionComboBox.Location = new System.Drawing.Point(89, 89);
      this.MachineSelectionComboBox.Name = "MachineSelectionComboBox";
      this.MachineSelectionComboBox.Size = new System.Drawing.Size(265, 21);
      this.MachineSelectionComboBox.TabIndex = 3;
      this.MachineSelectionComboBox.SelectedIndexChanged += new System.EventHandler(this.MachineSelectionComboBox_SelectedIndexChanged);
      // 
      // AddServiceDialog
      // 
      this.AcceptButton = this.DialogOKButton;
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.BackColor = System.Drawing.SystemColors.Control;
      this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
      this.CancelButton = this.DialogCancelButton;
      this.ClientSize = new System.Drawing.Size(529, 542);
      this.CommandAreaVisible = true;
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FootnoteAreaColor = System.Drawing.SystemColors.Menu;
      this.FootnoteAreaHeight = 0;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimumSize = new System.Drawing.Size(504, 450);
      this.Name = "AddServiceDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Add Service";
      this.ContentAreaPanel.ResumeLayout(false);
      this.ContentAreaPanel.PerformLayout();
      this.CommandAreaPanel.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Timer timerForFiltering;
    private System.Windows.Forms.Button DeleteButton;
    private System.Windows.Forms.Button EditButton;
    private System.Windows.Forms.TextBox FilterTextBox;
    private System.Windows.Forms.Label FilterLabel;
    private System.Windows.Forms.Label WindowsServiceInstructionsLabel;
    private System.Windows.Forms.Label MachineInstructionsLabel;
    private System.Windows.Forms.Label ChooseMachineLabel;
    private System.Windows.Forms.CheckBox FilterCheckBox;
    private System.Windows.Forms.ListView ServicesListView;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.Label ChooseWindowsServiceLabel;
    private System.Windows.Forms.Label ComputerLabel;
    private System.Windows.Forms.ComboBox MachineSelectionComboBox;
    private System.Windows.Forms.Button DialogOKButton;
    private System.Windows.Forms.Button DialogCancelButton;
  }
}