namespace MySql.TrayApp
{
  partial class OptionsDialog
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
      this.chkNotifyChanges = new System.Windows.Forms.CheckBox();
      this.grpOtherOptions = new System.Windows.Forms.GroupBox();
      this.chkEnabledAutoAddServices = new System.Windows.Forms.CheckBox();
      this.lblWeeks = new System.Windows.Forms.Label();
      this.numCheckUpdatesWeeks = new System.Windows.Forms.NumericUpDown();
      this.chkAutoCheckUpdates = new System.Windows.Forms.CheckBox();
      this.chkRunAtStartup = new System.Windows.Forms.CheckBox();
      this.btnOK = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.grpOtherOptions.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numCheckUpdatesWeeks)).BeginInit();
      this.SuspendLayout();
      // 
      // chkNotifyChanges
      // 
      this.chkNotifyChanges.AutoSize = true;
      this.chkNotifyChanges.Location = new System.Drawing.Point(18, 79);
      this.chkNotifyChanges.Name = "chkNotifyChanges";
      this.chkNotifyChanges.Size = new System.Drawing.Size(188, 17);
      this.chkNotifyChanges.TabIndex = 2;
      this.chkNotifyChanges.Text = "Notify me when a service is added";
      this.chkNotifyChanges.UseVisualStyleBackColor = true;
      // 
      // grpOtherOptions
      // 
      this.grpOtherOptions.Controls.Add(this.chkNotifyChanges);
      this.grpOtherOptions.Controls.Add(this.chkEnabledAutoAddServices);
      this.grpOtherOptions.Controls.Add(this.lblWeeks);
      this.grpOtherOptions.Controls.Add(this.numCheckUpdatesWeeks);
      this.grpOtherOptions.Controls.Add(this.chkAutoCheckUpdates);
      this.grpOtherOptions.Controls.Add(this.chkRunAtStartup);
      this.grpOtherOptions.Location = new System.Drawing.Point(12, 12);
      this.grpOtherOptions.Name = "grpOtherOptions";
      this.grpOtherOptions.Size = new System.Drawing.Size(431, 161);
      this.grpOtherOptions.TabIndex = 1;
      this.grpOtherOptions.TabStop = false;
      // 
      // chkEnabledAutoAddServices
      // 
      this.chkEnabledAutoAddServices.AutoSize = true;
      this.chkEnabledAutoAddServices.Location = new System.Drawing.Point(18, 47);
      this.chkEnabledAutoAddServices.Name = "chkEnabledAutoAddServices";
      this.chkEnabledAutoAddServices.Size = new System.Drawing.Size(265, 17);
      this.chkEnabledAutoAddServices.TabIndex = 4;
      this.chkEnabledAutoAddServices.Text = "Automatically start monitoring new MySQL services\r\n";
      this.chkEnabledAutoAddServices.UseVisualStyleBackColor = true;
      // 
      // lblWeeks
      // 
      this.lblWeeks.AutoSize = true;
      this.lblWeeks.Location = new System.Drawing.Point(281, 116);
      this.lblWeeks.Name = "lblWeeks";
      this.lblWeeks.Size = new System.Drawing.Size(41, 13);
      this.lblWeeks.TabIndex = 3;
      this.lblWeeks.Text = "Weeks";
      // 
      // numCheckUpdatesWeeks
      // 
      this.numCheckUpdatesWeeks.Location = new System.Drawing.Point(230, 114);
      this.numCheckUpdatesWeeks.Name = "numCheckUpdatesWeeks";
      this.numCheckUpdatesWeeks.Size = new System.Drawing.Size(45, 20);
      this.numCheckUpdatesWeeks.TabIndex = 2;
      // 
      // chkAutoCheckUpdates
      // 
      this.chkAutoCheckUpdates.AutoSize = true;
      this.chkAutoCheckUpdates.Location = new System.Drawing.Point(18, 115);
      this.chkAutoCheckUpdates.Name = "chkAutoCheckUpdates";
      this.chkAutoCheckUpdates.Size = new System.Drawing.Size(213, 17);
      this.chkAutoCheckUpdates.TabIndex = 1;
      this.chkAutoCheckUpdates.Text = "Automatically Check For Updates Every";
      this.chkAutoCheckUpdates.UseVisualStyleBackColor = true;
      this.chkAutoCheckUpdates.CheckedChanged += new System.EventHandler(this.chkAutoCheckUpdates_CheckedChanged);
      // 
      // chkRunAtStartup
      // 
      this.chkRunAtStartup.AutoSize = true;
      this.chkRunAtStartup.Location = new System.Drawing.Point(18, 19);
      this.chkRunAtStartup.Name = "chkRunAtStartup";
      this.chkRunAtStartup.Size = new System.Drawing.Size(142, 17);
      this.chkRunAtStartup.TabIndex = 0;
      this.chkRunAtStartup.Text = "Run at Windows Startup";
      this.chkRunAtStartup.UseVisualStyleBackColor = true;
      // 
      // btnOK
      // 
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.Location = new System.Drawing.Point(287, 188);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(368, 188);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // OptionsDialog
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(455, 227);
      this.Controls.Add(this.grpOtherOptions);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnOK);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "OptionsDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Options";
      this.grpOtherOptions.ResumeLayout(false);
      this.grpOtherOptions.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numCheckUpdatesWeeks)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.CheckBox chkNotifyChanges;
    private System.Windows.Forms.GroupBox grpOtherOptions;
    private System.Windows.Forms.Label lblWeeks;
    private System.Windows.Forms.NumericUpDown numCheckUpdatesWeeks;
    private System.Windows.Forms.CheckBox chkAutoCheckUpdates;
    private System.Windows.Forms.CheckBox chkRunAtStartup;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.CheckBox chkEnabledAutoAddServices;
  }
}