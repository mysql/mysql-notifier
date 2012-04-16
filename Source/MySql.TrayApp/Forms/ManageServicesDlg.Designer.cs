namespace MySql.TrayApp
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
      this.bntOK = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnDelete = new System.Windows.Forms.Button();
      this.btnAdd = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.lstMonitoredServices = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.SuspendLayout();
      // 
      // bntOK
      // 
      this.bntOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.bntOK.Location = new System.Drawing.Point(333, 216);
      this.bntOK.Name = "bntOK";
      this.bntOK.Size = new System.Drawing.Size(75, 23);
      this.bntOK.TabIndex = 15;
      this.bntOK.Text = "OK";
      this.bntOK.UseVisualStyleBackColor = true;
      this.bntOK.Click += new System.EventHandler(this.bntOK_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(414, 216);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 14;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // btnDelete
      // 
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
      this.lstMonitoredServices.CheckBoxes = true;
      this.lstMonitoredServices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
      this.lstMonitoredServices.FullRowSelect = true;
      this.lstMonitoredServices.Location = new System.Drawing.Point(12, 43);
      this.lstMonitoredServices.MultiSelect = false;
      this.lstMonitoredServices.Name = "lstMonitoredServices";
      this.lstMonitoredServices.Size = new System.Drawing.Size(384, 152);
      this.lstMonitoredServices.TabIndex = 8;
      this.lstMonitoredServices.UseCompatibleStateImageBehavior = false;
      this.lstMonitoredServices.View = System.Windows.Forms.View.Details;
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Service Name";
      this.columnHeader1.Width = 206;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Location";
      this.columnHeader2.Width = 100;
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "Status";
      this.columnHeader3.Width = 74;
      // 
      // ManageServicesDlg
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(502, 261);
      this.Controls.Add(this.bntOK);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnDelete);
      this.Controls.Add(this.btnAdd);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.lstMonitoredServices);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ManageServicesDlg";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Manage Services";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button bntOK;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnAdd;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ListView lstMonitoredServices;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.ColumnHeader columnHeader3;
  }
}