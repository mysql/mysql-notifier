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
      this.btnClose = new System.Windows.Forms.Button();
      this.btnDelete = new System.Windows.Forms.Button();
      this.btnAdd = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.lstMonitoredServices = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.chkUpdateTrayIcon = new System.Windows.Forms.CheckBox();
      this.notifyOnStatusChange = new System.Windows.Forms.CheckBox();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnClose
      // 
      this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnClose.Location = new System.Drawing.Point(414, 254);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(75, 23);
      this.btnClose.TabIndex = 14;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      // 
      // btnDelete
      // 
      this.btnDelete.Enabled = false;
      this.btnDelete.Location = new System.Drawing.Point(414, 75);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new System.Drawing.Size(75, 23);
      this.btnDelete.TabIndex = 11;
      this.btnDelete.Text = "Delete";
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
      // 
      // btnAdd
      // 
      this.btnAdd.Location = new System.Drawing.Point(414, 46);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new System.Drawing.Size(75, 23);
      this.btnAdd.TabIndex = 10;
      this.btnAdd.Text = "Add";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 18);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(98, 13);
      this.label1.TabIndex = 9;
      this.label1.Text = "Monitored Services";
      // 
      // lstMonitoredServices
      // 
      this.lstMonitoredServices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3});
      this.lstMonitoredServices.FullRowSelect = true;
      this.lstMonitoredServices.HideSelection = false;
      this.lstMonitoredServices.Location = new System.Drawing.Point(12, 43);
      this.lstMonitoredServices.MultiSelect = false;
      this.lstMonitoredServices.Name = "lstMonitoredServices";
      this.lstMonitoredServices.Size = new System.Drawing.Size(384, 152);
      this.lstMonitoredServices.TabIndex = 8;
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
      this.columnHeader3.Width = 74;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.chkUpdateTrayIcon);
      this.groupBox1.Controls.Add(this.notifyOnStatusChange);
      this.groupBox1.Location = new System.Drawing.Point(12, 201);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(384, 76);
      this.groupBox1.TabIndex = 15;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Service Options";
      // 
      // chkUpdateTrayIcon
      // 
      this.chkUpdateTrayIcon.AutoSize = true;
      this.chkUpdateTrayIcon.Enabled = false;
      this.chkUpdateTrayIcon.Location = new System.Drawing.Point(26, 42);
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
      this.notifyOnStatusChange.Location = new System.Drawing.Point(26, 19);
      this.notifyOnStatusChange.Name = "notifyOnStatusChange";
      this.notifyOnStatusChange.Size = new System.Drawing.Size(174, 17);
      this.notifyOnStatusChange.TabIndex = 0;
      this.notifyOnStatusChange.Text = "Notify me when status changes";
      this.notifyOnStatusChange.UseVisualStyleBackColor = true;
      this.notifyOnStatusChange.CheckedChanged += new System.EventHandler(this.notifyOnStatusChange_CheckedChanged);
      // 
      // ManageServicesDlg
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(502, 294);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.btnClose);
      this.Controls.Add(this.btnDelete);
      this.Controls.Add(this.btnAdd);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.lstMonitoredServices);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ManageServicesDlg";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Manage Services";
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnAdd;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ListView lstMonitoredServices;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.CheckBox notifyOnStatusChange;
    private System.Windows.Forms.CheckBox chkUpdateTrayIcon;
  }
}