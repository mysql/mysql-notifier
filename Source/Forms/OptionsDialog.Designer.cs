namespace MySql.Notifier
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsDialog));
      this.panel2 = new System.Windows.Forms.Panel();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.panel1 = new System.Windows.Forms.Panel();
      this.lblSubTitle2 = new System.Windows.Forms.Label();
      this.notifyOfStatusChange = new System.Windows.Forms.CheckBox();
      this.notifyOfAutoAdd = new System.Windows.Forms.CheckBox();
      this.autoAddRegex = new System.Windows.Forms.TextBox();
      this.chkEnabledAutoAddServices = new System.Windows.Forms.CheckBox();
      this.lblWeeks = new System.Windows.Forms.Label();
      this.numCheckUpdatesWeeks = new System.Windows.Forms.NumericUpDown();
      this.chkAutoCheckUpdates = new System.Windows.Forms.CheckBox();
      this.chkRunAtStartup = new System.Windows.Forms.CheckBox();
      this.lblSubTitle1 = new System.Windows.Forms.Label();
      this.lblHiperTitle = new System.Windows.Forms.Label();
      this.panel2.SuspendLayout();
      this.panel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numCheckUpdatesWeeks)).BeginInit();
      this.SuspendLayout();
      // 
      // panel2
      // 
      this.panel2.Controls.Add(this.btnCancel);
      this.panel2.Controls.Add(this.btnOK);
      this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panel2.ForeColor = System.Drawing.SystemColors.ControlText;
      this.panel2.Location = new System.Drawing.Point(0, 392);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(422, 63);
      this.panel2.TabIndex = 5;
      // 
      // btnCancel
      // 
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(325, 20);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(87, 27);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // btnOK
      // 
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.Location = new System.Drawing.Point(231, 20);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(87, 27);
      this.btnOK.TabIndex = 4;
      this.btnOK.Text = "Apply";
      this.btnOK.UseVisualStyleBackColor = true;
      // 
      // panel1
      // 
      this.panel1.AutoSize = true;
      this.panel1.BackColor = System.Drawing.Color.White;
      this.panel1.Controls.Add(this.lblSubTitle2);
      this.panel1.Controls.Add(this.notifyOfStatusChange);
      this.panel1.Controls.Add(this.notifyOfAutoAdd);
      this.panel1.Controls.Add(this.autoAddRegex);
      this.panel1.Controls.Add(this.chkEnabledAutoAddServices);
      this.panel1.Controls.Add(this.lblWeeks);
      this.panel1.Controls.Add(this.numCheckUpdatesWeeks);
      this.panel1.Controls.Add(this.chkAutoCheckUpdates);
      this.panel1.Controls.Add(this.chkRunAtStartup);
      this.panel1.Controls.Add(this.lblSubTitle1);
      this.panel1.Controls.Add(this.lblHiperTitle);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel1.Location = new System.Drawing.Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(422, 455);
      this.panel1.TabIndex = 6;
      // 
      // lblSubTitle2
      // 
      this.lblSubTitle2.AutoSize = true;
      this.lblSubTitle2.Location = new System.Drawing.Point(17, 268);
      this.lblSubTitle2.Name = "lblSubTitle2";
      this.lblSubTitle2.Size = new System.Drawing.Size(120, 15);
      this.lblSubTitle2.TabIndex = 61;
      this.lblSubTitle2.Text = "Notifications Options";
      // 
      // notifyOfStatusChange
      // 
      this.notifyOfStatusChange.AutoSize = true;
      this.notifyOfStatusChange.Location = new System.Drawing.Point(21, 344);
      this.notifyOfStatusChange.Name = "notifyOfStatusChange";
      this.notifyOfStatusChange.Size = new System.Drawing.Size(243, 19);
      this.notifyOfStatusChange.TabIndex = 60;
      this.notifyOfStatusChange.Text = "Notify me when a service changes status.";
      this.notifyOfStatusChange.UseVisualStyleBackColor = true;
      // 
      // notifyOfAutoAdd
      // 
      this.notifyOfAutoAdd.AutoSize = true;
      this.notifyOfAutoAdd.Location = new System.Drawing.Point(21, 310);
      this.notifyOfAutoAdd.Name = "notifyOfAutoAdd";
      this.notifyOfAutoAdd.Size = new System.Drawing.Size(284, 19);
      this.notifyOfAutoAdd.TabIndex = 59;
      this.notifyOfAutoAdd.Text = "Notify me when a service is automatically added.";
      this.notifyOfAutoAdd.UseVisualStyleBackColor = true;
      // 
      // autoAddRegex
      // 
      this.autoAddRegex.Location = new System.Drawing.Point(49, 205);
      this.autoAddRegex.Name = "autoAddRegex";
      this.autoAddRegex.Size = new System.Drawing.Size(300, 23);
      this.autoAddRegex.TabIndex = 58;
      // 
      // chkEnabledAutoAddServices
      // 
      this.chkEnabledAutoAddServices.AutoSize = true;
      this.chkEnabledAutoAddServices.Location = new System.Drawing.Point(21, 179);
      this.chkEnabledAutoAddServices.Name = "chkEnabledAutoAddServices";
      this.chkEnabledAutoAddServices.Size = new System.Drawing.Size(316, 19);
      this.chkEnabledAutoAddServices.TabIndex = 57;
      this.chkEnabledAutoAddServices.Text = "Automatically add new services that match this pattern\r\n";
      this.chkEnabledAutoAddServices.UseVisualStyleBackColor = true;
      // 
      // lblWeeks
      // 
      this.lblWeeks.AutoSize = true;
      this.lblWeeks.Location = new System.Drawing.Point(328, 141);
      this.lblWeeks.Name = "lblWeeks";
      this.lblWeeks.Size = new System.Drawing.Size(41, 15);
      this.lblWeeks.TabIndex = 56;
      this.lblWeeks.Text = "Weeks";
      // 
      // numCheckUpdatesWeeks
      // 
      this.numCheckUpdatesWeeks.Location = new System.Drawing.Point(268, 138);
      this.numCheckUpdatesWeeks.Name = "numCheckUpdatesWeeks";
      this.numCheckUpdatesWeeks.Size = new System.Drawing.Size(52, 23);
      this.numCheckUpdatesWeeks.TabIndex = 55;
      // 
      // chkAutoCheckUpdates
      // 
      this.chkAutoCheckUpdates.AutoSize = true;
      this.chkAutoCheckUpdates.Location = new System.Drawing.Point(21, 140);
      this.chkAutoCheckUpdates.Name = "chkAutoCheckUpdates";
      this.chkAutoCheckUpdates.Size = new System.Drawing.Size(233, 19);
      this.chkAutoCheckUpdates.TabIndex = 54;
      this.chkAutoCheckUpdates.Text = "Automatically Check For Updates Every";
      this.chkAutoCheckUpdates.UseVisualStyleBackColor = true;
      // 
      // chkRunAtStartup
      // 
      this.chkRunAtStartup.AutoSize = true;
      this.chkRunAtStartup.Location = new System.Drawing.Point(21, 104);
      this.chkRunAtStartup.Name = "chkRunAtStartup";
      this.chkRunAtStartup.Size = new System.Drawing.Size(153, 19);
      this.chkRunAtStartup.TabIndex = 53;
      this.chkRunAtStartup.Text = "Run at Windows Startup";
      this.chkRunAtStartup.UseVisualStyleBackColor = true;
      // 
      // lblSubTitle1
      // 
      this.lblSubTitle1.AutoSize = true;
      this.lblSubTitle1.Location = new System.Drawing.Point(17, 65);
      this.lblSubTitle1.Name = "lblSubTitle1";
      this.lblSubTitle1.Size = new System.Drawing.Size(92, 15);
      this.lblSubTitle1.TabIndex = 52;
      this.lblSubTitle1.Text = "General Options";
      // 
      // lblHiperTitle
      // 
      this.lblHiperTitle.AutoSize = true;
      this.lblHiperTitle.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblHiperTitle.Location = new System.Drawing.Point(14, 27);
      this.lblHiperTitle.Name = "lblHiperTitle";
      this.lblHiperTitle.Size = new System.Drawing.Size(133, 15);
      this.lblHiperTitle.TabIndex = 21;
      this.lblHiperTitle.Text = "MySQL Notifier Options";
      // 
      // OptionsDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(422, 455);
      this.Controls.Add(this.panel2);
      this.Controls.Add(this.panel1);
      this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "OptionsDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Options";
      this.panel2.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numCheckUpdatesWeeks)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Label lblHiperTitle;
    private System.Windows.Forms.Label lblSubTitle2;
    private System.Windows.Forms.CheckBox notifyOfStatusChange;
    private System.Windows.Forms.CheckBox notifyOfAutoAdd;
    private System.Windows.Forms.TextBox autoAddRegex;
    private System.Windows.Forms.CheckBox chkEnabledAutoAddServices;
    private System.Windows.Forms.Label lblWeeks;
    private System.Windows.Forms.NumericUpDown numCheckUpdatesWeeks;
    private System.Windows.Forms.CheckBox chkAutoCheckUpdates;
    private System.Windows.Forms.CheckBox chkRunAtStartup;
    private System.Windows.Forms.Label lblSubTitle1;
  }
}