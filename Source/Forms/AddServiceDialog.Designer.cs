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
      this.MachineAutoTestConnectionIntervalUOMComboBox = new System.Windows.Forms.ComboBox();
      this.MachineAutoTestConnectionLabel = new System.Windows.Forms.Label();
      this.MachineAutoTestConnectionIntervalNumericUpDown = new System.Windows.Forms.NumericUpDown();
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
      ((System.ComponentModel.ISupportInitialize)(this.MachineAutoTestConnectionIntervalNumericUpDown)).BeginInit();
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
      this.panel2.Size = new System.Drawing.Size(488, 50);
      this.panel2.TabIndex = 1;
      // 
      // DialogOKButton
      // 
      this.DialogOKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogOKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.DialogOKButton.Location = new System.Drawing.Point(319, 15);
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
      this.DialogCancelButton.Location = new System.Drawing.Point(400, 15);
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
      this.panel1.Controls.Add(this.MachineAutoTestConnectionIntervalUOMComboBox);
      this.panel1.Controls.Add(this.MachineAutoTestConnectionLabel);
      this.panel1.Controls.Add(this.MachineAutoTestConnectionIntervalNumericUpDown);
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
      this.panel1.Size = new System.Drawing.Size(488, 542);
      this.panel1.TabIndex = 0;
      // 
      // MachineAutoTestConnectionIntervalUOMComboBox
      // 
      this.MachineAutoTestConnectionIntervalUOMComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.MachineAutoTestConnectionIntervalUOMComboBox.Items.AddRange(new object[] {
            "seconds",
            "minutes",
            "hours",
            "days"});
      this.MachineAutoTestConnectionIntervalUOMComboBox.Location = new System.Drawing.Point(308, 105);
      this.MachineAutoTestConnectionIntervalUOMComboBox.Name = "MachineAutoTestConnectionIntervalUOMComboBox";
      this.MachineAutoTestConnectionIntervalUOMComboBox.Size = new System.Drawing.Size(112, 21);
      this.MachineAutoTestConnectionIntervalUOMComboBox.TabIndex = 6;
      this.MachineAutoTestConnectionIntervalUOMComboBox.SelectedIndexChanged += new System.EventHandler(this.MachineAutoTestConnectionIntervalUOMComboBox_SelectedIndexChanged);
      // 
      // MachineAutoTestConnectionLabel
      // 
      this.MachineAutoTestConnectionLabel.AutoSize = true;
      this.MachineAutoTestConnectionLabel.Location = new System.Drawing.Point(22, 108);
      this.MachineAutoTestConnectionLabel.Name = "MachineAutoTestConnectionLabel";
      this.MachineAutoTestConnectionLabel.Size = new System.Drawing.Size(214, 13);
      this.MachineAutoTestConnectionLabel.TabIndex = 4;
      this.MachineAutoTestConnectionLabel.Text = "Check computer connection status every";
      // 
      // MachineAutoTestConnectionIntervalNumericUpDown
      // 
      this.MachineAutoTestConnectionIntervalNumericUpDown.Location = new System.Drawing.Point(251, 106);
      this.MachineAutoTestConnectionIntervalNumericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
      this.MachineAutoTestConnectionIntervalNumericUpDown.Name = "MachineAutoTestConnectionIntervalNumericUpDown";
      this.MachineAutoTestConnectionIntervalNumericUpDown.Size = new System.Drawing.Size(51, 22);
      this.MachineAutoTestConnectionIntervalNumericUpDown.TabIndex = 5;
      this.MachineAutoTestConnectionIntervalNumericUpDown.ValueChanged += new System.EventHandler(this.MachineAutoTestConnectionIntervalNumericUpDown_ValueChanged);
      // 
      // FilterTextBox
      // 
      this.FilterTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.FilterTextBox.Location = new System.Drawing.Point(64, 213);
      this.FilterTextBox.Name = "FilterTextBox";
      this.FilterTextBox.Size = new System.Drawing.Size(130, 22);
      this.FilterTextBox.TabIndex = 10;
      this.FilterTextBox.TextChanged += new System.EventHandler(this.FilterTextBox_TextChanged);
      // 
      // FilterLabel
      // 
      this.FilterLabel.AutoSize = true;
      this.FilterLabel.Location = new System.Drawing.Point(22, 218);
      this.FilterLabel.Name = "FilterLabel";
      this.FilterLabel.Size = new System.Drawing.Size(36, 13);
      this.FilterLabel.TabIndex = 9;
      this.FilterLabel.Text = "Filter:";
      // 
      // WindowsServiceInstructionsLabel
      // 
      this.WindowsServiceInstructionsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.WindowsServiceInstructionsLabel.Location = new System.Drawing.Point(22, 176);
      this.WindowsServiceInstructionsLabel.Name = "WindowsServiceInstructionsLabel";
      this.WindowsServiceInstructionsLabel.Size = new System.Drawing.Size(454, 34);
      this.WindowsServiceInstructionsLabel.TabIndex = 8;
      this.WindowsServiceInstructionsLabel.Text = "Select the service you want to monitor. You can filter the list by typing into th" +
    "e filter control.";
      // 
      // MachineInstructionsLabel
      // 
      this.MachineInstructionsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.MachineInstructionsLabel.Location = new System.Drawing.Point(20, 46);
      this.MachineInstructionsLabel.Name = "MachineInstructionsLabel";
      this.MachineInstructionsLabel.Size = new System.Drawing.Size(455, 29);
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
      this.FilterCheckBox.Location = new System.Drawing.Point(212, 218);
      this.FilterCheckBox.Name = "FilterCheckBox";
      this.FilterCheckBox.Size = new System.Drawing.Size(264, 17);
      this.FilterCheckBox.TabIndex = 11;
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
      this.ServicesListView.Location = new System.Drawing.Point(23, 244);
      this.ServicesListView.Name = "ServicesListView";
      this.ServicesListView.Size = new System.Drawing.Size(452, 222);
      this.ServicesListView.TabIndex = 12;
      this.ServicesListView.UseCompatibleStateImageBehavior = false;
      this.ServicesListView.View = System.Windows.Forms.View.Details;
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Display Name";
      this.columnHeader1.Width = 337;
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "Status";
      this.columnHeader3.Width = 85;
      // 
      // WindowsServiceHyperTitleLabel
      // 
      this.WindowsServiceHyperTitleLabel.AutoSize = true;
      this.WindowsServiceHyperTitleLabel.Location = new System.Drawing.Point(22, 154);
      this.WindowsServiceHyperTitleLabel.Name = "WindowsServiceHyperTitleLabel";
      this.WindowsServiceHyperTitleLabel.Size = new System.Drawing.Size(148, 13);
      this.WindowsServiceHyperTitleLabel.TabIndex = 7;
      this.WindowsServiceHyperTitleLabel.Text = "Choose a Windows Service:";
      // 
      // ComputerLabel
      // 
      this.ComputerLabel.AutoSize = true;
      this.ComputerLabel.Location = new System.Drawing.Point(61, 81);
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
            "Remote...",
            "————————————————————————"});
      this.MachineSelectionComboBox.Location = new System.Drawing.Point(128, 78);
      this.MachineSelectionComboBox.Name = "MachineSelectionComboBox";
      this.MachineSelectionComboBox.Size = new System.Drawing.Size(292, 21);
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
      this.ClientSize = new System.Drawing.Size(488, 542);
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
      ((System.ComponentModel.ISupportInitialize)(this.MachineAutoTestConnectionIntervalNumericUpDown)).EndInit();
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
    private System.Windows.Forms.ComboBox MachineAutoTestConnectionIntervalUOMComboBox;
    private System.Windows.Forms.Label MachineAutoTestConnectionLabel;
    private System.Windows.Forms.NumericUpDown MachineAutoTestConnectionIntervalNumericUpDown;
  }
}