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
      this.grpScanForServices = new System.Windows.Forms.GroupBox();
      this.radServicesRunning = new System.Windows.Forms.RadioButton();
      this.radInstanceName = new System.Windows.Forms.RadioButton();
      this.txtStartsWith = new System.Windows.Forms.TextBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.radSoftRefresh = new System.Windows.Forms.RadioButton();
      this.radHardRefresh = new System.Windows.Forms.RadioButton();
      this.grpOtherOptions = new System.Windows.Forms.GroupBox();
      this.lblWeeks = new System.Windows.Forms.Label();
      this.numCheckUpdatesWeeks = new System.Windows.Forms.NumericUpDown();
      this.chkAutoCheckUpdates = new System.Windows.Forms.CheckBox();
      this.chkRunAtStartup = new System.Windows.Forms.CheckBox();
      this.btnOK = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.tabPage1 = new System.Windows.Forms.TabPage();
      this.tabPage2 = new System.Windows.Forms.TabPage();
      this.AddAllServicesToMonitor = new System.Windows.Forms.Button();
      this.btnRemoveAllServices = new System.Windows.Forms.Button();
      this.btnRemovedServiceToMonitor = new System.Windows.Forms.Button();
      this.btnAddServiceToMonitor = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.lstMonitoredServices = new System.Windows.Forms.ListBox();
      this.lstExistingServices = new System.Windows.Forms.ListBox();
      this.btnRefreshAll = new System.Windows.Forms.Button();
      this.grpServicesRefresh.SuspendLayout();
      this.grpAutoRefreshType.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numAutoRefreshFrequency)).BeginInit();
      this.grpScanForServices.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.grpOtherOptions.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numCheckUpdatesWeeks)).BeginInit();
      this.tabControl1.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.tabPage2.SuspendLayout();
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
      this.grpServicesRefresh.Location = new System.Drawing.Point(19, 19);
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
      // grpScanForServices
      // 
      this.grpScanForServices.Controls.Add(this.radServicesRunning);
      this.grpScanForServices.Controls.Add(this.radInstanceName);
      this.grpScanForServices.Controls.Add(this.txtStartsWith);
      this.grpScanForServices.Location = new System.Drawing.Point(180, 383);
      this.grpScanForServices.Name = "grpScanForServices";
      this.grpScanForServices.Size = new System.Drawing.Size(285, 80);
      this.grpScanForServices.TabIndex = 3;
      this.grpScanForServices.TabStop = false;
      this.grpScanForServices.Text = "Scan for Services";
      // 
      // radServicesRunning
      // 
      this.radServicesRunning.AutoSize = true;
      this.radServicesRunning.Location = new System.Drawing.Point(16, 52);
      this.radServicesRunning.Name = "radServicesRunning";
      this.radServicesRunning.Size = new System.Drawing.Size(164, 17);
      this.radServicesRunning.TabIndex = 2;
      this.radServicesRunning.TabStop = true;
      this.radServicesRunning.Text = "Services Running mysqld.exe";
      this.radServicesRunning.UseVisualStyleBackColor = true;
      // 
      // radInstanceName
      // 
      this.radInstanceName.AutoSize = true;
      this.radInstanceName.Location = new System.Drawing.Point(16, 26);
      this.radInstanceName.Name = "radInstanceName";
      this.radInstanceName.Size = new System.Drawing.Size(155, 17);
      this.radInstanceName.TabIndex = 0;
      this.radInstanceName.TabStop = true;
      this.radInstanceName.Text = "Instance Name Starts With:";
      this.radInstanceName.UseVisualStyleBackColor = true;
      this.radInstanceName.CheckedChanged += new System.EventHandler(this.radInstanceName_CheckedChanged);
      // 
      // txtStartsWith
      // 
      this.txtStartsWith.Location = new System.Drawing.Point(193, 23);
      this.txtStartsWith.Name = "txtStartsWith";
      this.txtStartsWith.Size = new System.Drawing.Size(79, 20);
      this.txtStartsWith.TabIndex = 1;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.radSoftRefresh);
      this.groupBox1.Controls.Add(this.radHardRefresh);
      this.groupBox1.Location = new System.Drawing.Point(34, 373);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(124, 80);
      this.groupBox1.TabIndex = 2;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Refresh Method";
      // 
      // radSoftRefresh
      // 
      this.radSoftRefresh.AutoSize = true;
      this.radSoftRefresh.Location = new System.Drawing.Point(15, 52);
      this.radSoftRefresh.Name = "radSoftRefresh";
      this.radSoftRefresh.Size = new System.Drawing.Size(84, 17);
      this.radSoftRefresh.TabIndex = 1;
      this.radSoftRefresh.TabStop = true;
      this.radSoftRefresh.Text = "Soft Refresh";
      this.radSoftRefresh.UseVisualStyleBackColor = true;
      // 
      // radHardRefresh
      // 
      this.radHardRefresh.AutoSize = true;
      this.radHardRefresh.Location = new System.Drawing.Point(15, 29);
      this.radHardRefresh.Name = "radHardRefresh";
      this.radHardRefresh.Size = new System.Drawing.Size(88, 17);
      this.radHardRefresh.TabIndex = 0;
      this.radHardRefresh.TabStop = true;
      this.radHardRefresh.Text = "Hard Refresh";
      this.radHardRefresh.UseVisualStyleBackColor = true;
      // 
      // grpOtherOptions
      // 
      this.grpOtherOptions.Controls.Add(this.lblWeeks);
      this.grpOtherOptions.Controls.Add(this.numCheckUpdatesWeeks);
      this.grpOtherOptions.Controls.Add(this.chkAutoCheckUpdates);
      this.grpOtherOptions.Controls.Add(this.chkRunAtStartup);
      this.grpOtherOptions.Location = new System.Drawing.Point(19, 166);
      this.grpOtherOptions.Name = "grpOtherOptions";
      this.grpOtherOptions.Size = new System.Drawing.Size(445, 67);
      this.grpOtherOptions.TabIndex = 1;
      this.grpOtherOptions.TabStop = false;
      // 
      // lblWeeks
      // 
      this.lblWeeks.AutoSize = true;
      this.lblWeeks.Location = new System.Drawing.Point(281, 43);
      this.lblWeeks.Name = "lblWeeks";
      this.lblWeeks.Size = new System.Drawing.Size(41, 13);
      this.lblWeeks.TabIndex = 3;
      this.lblWeeks.Text = "Weeks";
      // 
      // numCheckUpdatesWeeks
      // 
      this.numCheckUpdatesWeeks.Location = new System.Drawing.Point(230, 41);
      this.numCheckUpdatesWeeks.Name = "numCheckUpdatesWeeks";
      this.numCheckUpdatesWeeks.Size = new System.Drawing.Size(45, 20);
      this.numCheckUpdatesWeeks.TabIndex = 2;
      // 
      // chkAutoCheckUpdates
      // 
      this.chkAutoCheckUpdates.AutoSize = true;
      this.chkAutoCheckUpdates.Location = new System.Drawing.Point(18, 42);
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
      this.btnOK.Location = new System.Drawing.Point(320, 339);
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
      this.btnCancel.Location = new System.Drawing.Point(401, 339);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // tabControl1
      // 
      this.tabControl1.Controls.Add(this.tabPage1);
      this.tabControl1.Controls.Add(this.tabPage2);
      this.tabControl1.Location = new System.Drawing.Point(12, 12);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new System.Drawing.Size(482, 313);
      this.tabControl1.TabIndex = 4;
      // 
      // tabPage1
      // 
      this.tabPage1.Controls.Add(this.grpServicesRefresh);
      this.tabPage1.Controls.Add(this.grpOtherOptions);
      this.tabPage1.Location = new System.Drawing.Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage1.Size = new System.Drawing.Size(474, 287);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "Monitor Control";
      this.tabPage1.UseVisualStyleBackColor = true;
      // 
      // tabPage2
      // 
      this.tabPage2.Controls.Add(this.btnRefreshAll);
      this.tabPage2.Controls.Add(this.AddAllServicesToMonitor);
      this.tabPage2.Controls.Add(this.btnRemoveAllServices);
      this.tabPage2.Controls.Add(this.btnRemovedServiceToMonitor);
      this.tabPage2.Controls.Add(this.btnAddServiceToMonitor);
      this.tabPage2.Controls.Add(this.label1);
      this.tabPage2.Controls.Add(this.lstMonitoredServices);
      this.tabPage2.Controls.Add(this.lstExistingServices);
      this.tabPage2.Location = new System.Drawing.Point(4, 22);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage2.Size = new System.Drawing.Size(474, 287);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "Monitor MySQL Instances";
      this.tabPage2.UseVisualStyleBackColor = true;
      // 
      // AddAllServicesToMonitor
      // 
      this.AddAllServicesToMonitor.Location = new System.Drawing.Point(199, 178);
      this.AddAllServicesToMonitor.Name = "AddAllServicesToMonitor";
      this.AddAllServicesToMonitor.Size = new System.Drawing.Size(65, 45);
      this.AddAllServicesToMonitor.TabIndex = 6;
      this.AddAllServicesToMonitor.Text = "Select All";
      this.AddAllServicesToMonitor.UseVisualStyleBackColor = true;
      // 
      // btnRemoveAllServices
      // 
      this.btnRemoveAllServices.Location = new System.Drawing.Point(199, 127);
      this.btnRemoveAllServices.Name = "btnRemoveAllServices";
      this.btnRemoveAllServices.Size = new System.Drawing.Size(65, 40);
      this.btnRemoveAllServices.TabIndex = 5;
      this.btnRemoveAllServices.Text = "Remove All";
      this.btnRemoveAllServices.UseVisualStyleBackColor = true;
      // 
      // btnRemovedServiceToMonitor
      // 
      this.btnRemovedServiceToMonitor.Location = new System.Drawing.Point(199, 86);
      this.btnRemovedServiceToMonitor.Name = "btnRemovedServiceToMonitor";
      this.btnRemovedServiceToMonitor.Size = new System.Drawing.Size(65, 35);
      this.btnRemovedServiceToMonitor.TabIndex = 4;
      this.btnRemovedServiceToMonitor.Text = "<<";
      this.btnRemovedServiceToMonitor.UseVisualStyleBackColor = true;
      // 
      // btnAddServiceToMonitor
      // 
      this.btnAddServiceToMonitor.Location = new System.Drawing.Point(199, 44);
      this.btnAddServiceToMonitor.Name = "btnAddServiceToMonitor";
      this.btnAddServiceToMonitor.Size = new System.Drawing.Size(65, 36);
      this.btnAddServiceToMonitor.TabIndex = 3;
      this.btnAddServiceToMonitor.Text = ">>";
      this.btnAddServiceToMonitor.UseVisualStyleBackColor = true;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(9, 16);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(128, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Select services to monitor";
      // 
      // lstMonitoredServices
      // 
      this.lstMonitoredServices.FormattingEnabled = true;
      this.lstMonitoredServices.Location = new System.Drawing.Point(283, 44);
      this.lstMonitoredServices.Name = "lstMonitoredServices";
      this.lstMonitoredServices.Size = new System.Drawing.Size(172, 199);
      this.lstMonitoredServices.TabIndex = 1;
      // 
      // lstExistingServices
      // 
      this.lstExistingServices.FormattingEnabled = true;
      this.lstExistingServices.Location = new System.Drawing.Point(12, 44);
      this.lstExistingServices.Name = "lstExistingServices";
      this.lstExistingServices.Size = new System.Drawing.Size(172, 199);
      this.lstExistingServices.TabIndex = 0;
      // 
      // btnRefreshAll
      // 
      this.btnRefreshAll.Location = new System.Drawing.Point(18, 252);
      this.btnRefreshAll.Name = "btnRefreshAll";
      this.btnRefreshAll.Size = new System.Drawing.Size(85, 25);
      this.btnRefreshAll.TabIndex = 7;
      this.btnRefreshAll.Text = "Update List";
      this.btnRefreshAll.UseVisualStyleBackColor = true;
      this.btnRefreshAll.Click += new System.EventHandler(this.btnRefreshAll_Click);
      // 
      // OptionsForm
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(505, 482);
      this.Controls.Add(this.grpScanForServices);
      this.Controls.Add(this.tabControl1);
      this.Controls.Add(this.groupBox1);
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
      this.grpScanForServices.ResumeLayout(false);
      this.grpScanForServices.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.grpOtherOptions.ResumeLayout(false);
      this.grpOtherOptions.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numCheckUpdatesWeeks)).EndInit();
      this.tabControl1.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.tabPage2.ResumeLayout(false);
      this.tabPage2.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.CheckBox chkEnableAutoRefresh;
    private System.Windows.Forms.GroupBox grpServicesRefresh;
    private System.Windows.Forms.GroupBox grpScanForServices;
    private System.Windows.Forms.RadioButton radServicesRunning;
    private System.Windows.Forms.RadioButton radInstanceName;
    private System.Windows.Forms.TextBox txtStartsWith;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.RadioButton radSoftRefresh;
    private System.Windows.Forms.RadioButton radHardRefresh;
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
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.TabPage tabPage2;
    private System.Windows.Forms.Button AddAllServicesToMonitor;
    private System.Windows.Forms.Button btnRemoveAllServices;
    private System.Windows.Forms.Button btnRemovedServiceToMonitor;
    private System.Windows.Forms.Button btnAddServiceToMonitor;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ListBox lstMonitoredServices;
    private System.Windows.Forms.ListBox lstExistingServices;
    private System.Windows.Forms.Button btnRefreshAll;
  }
}