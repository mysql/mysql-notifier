namespace MySql.TrayApp
{
  partial class OptionsForm
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
      this.chkEnableAutoRefresh = new System.Windows.Forms.CheckBox();
      this.grpServicesRefresh = new System.Windows.Forms.GroupBox();
      this.grpAutoRefreshType = new System.Windows.Forms.GroupBox();
      this.chkNotifyChanges = new System.Windows.Forms.CheckBox();
      this.lblAutoRefreshFrequency = new System.Windows.Forms.Label();
      this.numAutoRefreshFrequency = new System.Windows.Forms.NumericUpDown();
      this.radByTimer = new System.Windows.Forms.RadioButton();
      this.radOnDemand = new System.Windows.Forms.RadioButton();
      this.grpOtherOptions = new System.Windows.Forms.GroupBox();
      this.chkEnabledAutoAddServices = new System.Windows.Forms.CheckBox();
      this.lblWeeks = new System.Windows.Forms.Label();
      this.numCheckUpdatesWeeks = new System.Windows.Forms.NumericUpDown();
      this.chkAutoCheckUpdates = new System.Windows.Forms.CheckBox();
      this.chkRunAtStartup = new System.Windows.Forms.CheckBox();
      this.btnOK = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.grpServicesRefresh.SuspendLayout();
      this.grpAutoRefreshType.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numAutoRefreshFrequency)).BeginInit();
      this.grpOtherOptions.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numCheckUpdatesWeeks)).BeginInit();
      this.SuspendLayout();
      // 
      // chkEnableAutoRefresh
      // 
      this.chkEnableAutoRefresh.AutoSize = true;
      this.chkEnableAutoRefresh.Location = new System.Drawing.Point(18, 29);
      this.chkEnableAutoRefresh.Name = "chkEnableAutoRefresh";
      this.chkEnableAutoRefresh.Size = new System.Drawing.Size(124, 17);
      this.chkEnableAutoRefresh.TabIndex = 0;
      this.chkEnableAutoRefresh.Text = "Enable Auto-Refresh";
      this.chkEnableAutoRefresh.UseVisualStyleBackColor = true;
      this.chkEnableAutoRefresh.CheckedChanged += new System.EventHandler(this.chkEnableAutoRefresh_CheckedChanged);
      // 
      // grpServicesRefresh
      // 
      this.grpServicesRefresh.Controls.Add(this.grpAutoRefreshType);
      this.grpServicesRefresh.Controls.Add(this.chkEnableAutoRefresh);
      this.grpServicesRefresh.Location = new System.Drawing.Point(20, 21);
      this.grpServicesRefresh.Name = "grpServicesRefresh";
      this.grpServicesRefresh.Size = new System.Drawing.Size(445, 141);
      this.grpServicesRefresh.TabIndex = 0;
      this.grpServicesRefresh.TabStop = false;
      this.grpServicesRefresh.Text = "Services Refresh";
      // 
      // grpAutoRefreshType
      // 
      this.grpAutoRefreshType.Controls.Add(this.chkNotifyChanges);
      this.grpAutoRefreshType.Controls.Add(this.lblAutoRefreshFrequency);
      this.grpAutoRefreshType.Controls.Add(this.numAutoRefreshFrequency);
      this.grpAutoRefreshType.Controls.Add(this.radByTimer);
      this.grpAutoRefreshType.Controls.Add(this.radOnDemand);
      this.grpAutoRefreshType.Location = new System.Drawing.Point(18, 52);
      this.grpAutoRefreshType.Name = "grpAutoRefreshType";
      this.grpAutoRefreshType.Size = new System.Drawing.Size(415, 80);
      this.grpAutoRefreshType.TabIndex = 1;
      this.grpAutoRefreshType.TabStop = false;
      this.grpAutoRefreshType.Text = "Auto-Refresh Type";
      // 
      // chkNotifyChanges
      // 
      this.chkNotifyChanges.AutoSize = true;
      this.chkNotifyChanges.Location = new System.Drawing.Point(146, 26);
      this.chkNotifyChanges.Name = "chkNotifyChanges";
      this.chkNotifyChanges.Size = new System.Drawing.Size(142, 17);
      this.chkNotifyChanges.TabIndex = 2;
      this.chkNotifyChanges.Text = "Notify Services Changes";
      this.chkNotifyChanges.UseVisualStyleBackColor = true;
      // 
      // lblAutoRefreshFrequency
      // 
      this.lblAutoRefreshFrequency.AutoSize = true;
      this.lblAutoRefreshFrequency.Location = new System.Drawing.Point(143, 54);
      this.lblAutoRefreshFrequency.Name = "lblAutoRefreshFrequency";
      this.lblAutoRefreshFrequency.Size = new System.Drawing.Size(174, 13);
      this.lblAutoRefreshFrequency.TabIndex = 3;
      this.lblAutoRefreshFrequency.Text = "Auto-Refresh Frequency (seconds):";
      // 
      // numAutoRefreshFrequency
      // 
      this.numAutoRefreshFrequency.Location = new System.Drawing.Point(323, 52);
      this.numAutoRefreshFrequency.Name = "numAutoRefreshFrequency";
      this.numAutoRefreshFrequency.Size = new System.Drawing.Size(79, 20);
      this.numAutoRefreshFrequency.TabIndex = 4;
      // 
      // radByTimer
      // 
      this.radByTimer.AutoSize = true;
      this.radByTimer.Location = new System.Drawing.Point(16, 52);
      this.radByTimer.Name = "radByTimer";
      this.radByTimer.Size = new System.Drawing.Size(66, 17);
      this.radByTimer.TabIndex = 1;
      this.radByTimer.TabStop = true;
      this.radByTimer.Text = "By Timer";
      this.radByTimer.UseVisualStyleBackColor = true;
      this.radByTimer.CheckedChanged += new System.EventHandler(this.radByTimer_CheckedChanged);
      // 
      // radOnDemand
      // 
      this.radOnDemand.AutoSize = true;
      this.radOnDemand.Location = new System.Drawing.Point(16, 26);
      this.radOnDemand.Name = "radOnDemand";
      this.radOnDemand.Size = new System.Drawing.Size(82, 17);
      this.radOnDemand.TabIndex = 0;
      this.radOnDemand.TabStop = true;
      this.radOnDemand.Text = "On Demand";
      this.radOnDemand.UseVisualStyleBackColor = true;
      // 
      // grpOtherOptions
      // 
      this.grpOtherOptions.Controls.Add(this.chkEnabledAutoAddServices);
      this.grpOtherOptions.Controls.Add(this.lblWeeks);
      this.grpOtherOptions.Controls.Add(this.numCheckUpdatesWeeks);
      this.grpOtherOptions.Controls.Add(this.chkAutoCheckUpdates);
      this.grpOtherOptions.Controls.Add(this.chkRunAtStartup);
      this.grpOtherOptions.Location = new System.Drawing.Point(20, 177);
      this.grpOtherOptions.Name = "grpOtherOptions";
      this.grpOtherOptions.Size = new System.Drawing.Size(445, 101);
      this.grpOtherOptions.TabIndex = 1;
      this.grpOtherOptions.TabStop = false;
      // 
      // chkEnabledAutoAddServices
      // 
      this.chkEnabledAutoAddServices.AutoSize = true;
      this.chkEnabledAutoAddServices.Location = new System.Drawing.Point(18, 42);
      this.chkEnabledAutoAddServices.Name = "chkEnabledAutoAddServices";
      this.chkEnabledAutoAddServices.Size = new System.Drawing.Size(301, 17);
      this.chkEnabledAutoAddServices.TabIndex = 4;
      this.chkEnabledAutoAddServices.Text = "Automatically Add any new MySQL Service to the Monitor.";
      this.chkEnabledAutoAddServices.UseVisualStyleBackColor = true;
      // 
      // lblWeeks
      // 
      this.lblWeeks.AutoSize = true;
      this.lblWeeks.Location = new System.Drawing.Point(281, 68);
      this.lblWeeks.Name = "lblWeeks";
      this.lblWeeks.Size = new System.Drawing.Size(41, 13);
      this.lblWeeks.TabIndex = 3;
      this.lblWeeks.Text = "Weeks";
      // 
      // numCheckUpdatesWeeks
      // 
      this.numCheckUpdatesWeeks.Location = new System.Drawing.Point(230, 66);
      this.numCheckUpdatesWeeks.Name = "numCheckUpdatesWeeks";
      this.numCheckUpdatesWeeks.Size = new System.Drawing.Size(45, 20);
      this.numCheckUpdatesWeeks.TabIndex = 2;
      // 
      // chkAutoCheckUpdates
      // 
      this.chkAutoCheckUpdates.AutoSize = true;
      this.chkAutoCheckUpdates.Location = new System.Drawing.Point(18, 67);
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
      this.btnOK.Location = new System.Drawing.Point(309, 296);
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
      this.btnCancel.Location = new System.Drawing.Point(390, 296);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // OptionsForm
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(486, 342);
      this.Controls.Add(this.grpOtherOptions);
      this.Controls.Add(this.grpServicesRefresh);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnOK);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "OptionsForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Options";
      this.Load += new System.EventHandler(this.OptionsForm_Load);
      this.grpServicesRefresh.ResumeLayout(false);
      this.grpServicesRefresh.PerformLayout();
      this.grpAutoRefreshType.ResumeLayout(false);
      this.grpAutoRefreshType.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numAutoRefreshFrequency)).EndInit();
      this.grpOtherOptions.ResumeLayout(false);
      this.grpOtherOptions.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numCheckUpdatesWeeks)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.CheckBox chkEnableAutoRefresh;
    private System.Windows.Forms.GroupBox grpServicesRefresh;
    private System.Windows.Forms.GroupBox grpAutoRefreshType;
    private System.Windows.Forms.CheckBox chkNotifyChanges;
    private System.Windows.Forms.Label lblAutoRefreshFrequency;
    private System.Windows.Forms.NumericUpDown numAutoRefreshFrequency;
    private System.Windows.Forms.RadioButton radByTimer;
    private System.Windows.Forms.RadioButton radOnDemand;
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