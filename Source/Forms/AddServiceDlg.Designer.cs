namespace MySql.TrayApp
{
  partial class AddServiceDlg
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
      this.btnOK = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.server = new System.Windows.Forms.ComboBox();
      this.lstServices = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.filter = new System.Windows.Forms.CheckBox();
      this.filterText = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // btnOK
      // 
      this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.Enabled = false;
      this.btnOK.Location = new System.Drawing.Point(251, 372);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 11;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(332, 372);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 10;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(12, 107);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(95, 13);
      this.label2.TabIndex = 9;
      this.label2.Text = "Windows Services";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 28);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(55, 13);
      this.label1.TabIndex = 7;
      this.label1.Text = "Computer:";
      // 
      // server
      // 
      this.server.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.server.FormattingEnabled = true;
      this.server.Items.AddRange(new object[] {
            "Local"});
      this.server.Location = new System.Drawing.Point(73, 25);
      this.server.Name = "server";
      this.server.Size = new System.Drawing.Size(339, 21);
      this.server.TabIndex = 6;
      // 
      // lstServices
      // 
      this.lstServices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lstServices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
      this.lstServices.FullRowSelect = true;
      this.lstServices.Location = new System.Drawing.Point(20, 133);
      this.lstServices.MultiSelect = false;
      this.lstServices.Name = "lstServices";
      this.lstServices.Size = new System.Drawing.Size(387, 223);
      this.lstServices.TabIndex = 12;
      this.lstServices.UseCompatibleStateImageBehavior = false;
      this.lstServices.View = System.Windows.Forms.View.Details;
      this.lstServices.SelectedIndexChanged += new System.EventHandler(this.lstServices_SelectedIndexChanged);
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Display Name";
      this.columnHeader1.Width = 206;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Service Name";
      this.columnHeader2.Width = 100;
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "Status";
      this.columnHeader3.Width = 74;
      // 
      // filter
      // 
      this.filter.AutoSize = true;
      this.filter.Location = new System.Drawing.Point(75, 65);
      this.filter.Name = "filter";
      this.filter.Size = new System.Drawing.Size(239, 17);
      this.filter.TabIndex = 14;
      this.filter.Text = "Show services with executable that contains:";
      this.filter.UseVisualStyleBackColor = true;
      this.filter.CheckedChanged += new System.EventHandler(this.filter_CheckedChanged);
      // 
      // filterText
      // 
      this.filterText.Location = new System.Drawing.Point(312, 61);
      this.filterText.Name = "filterText";
      this.filterText.Size = new System.Drawing.Size(100, 20);
      this.filterText.TabIndex = 15;
      this.filterText.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
      // 
      // AddServiceDlg
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(433, 416);
      this.Controls.Add(this.filterText);
      this.Controls.Add(this.filter);
      this.Controls.Add(this.lstServices);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.server);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(449, 454);
      this.Name = "AddServiceDlg";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Add Service";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox server;
    private System.Windows.Forms.ListView lstServices;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.CheckBox filter;
    private System.Windows.Forms.TextBox filterText;
  }
}