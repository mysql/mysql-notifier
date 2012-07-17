namespace MySql.Notifier
{
  partial class AboutDialog
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutDialog));
      this.lblVersionSubTitle = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // lblVersionSubTitle
      // 
      this.lblVersionSubTitle.AutoSize = true;
      this.lblVersionSubTitle.ForeColor = System.Drawing.Color.Gray;
      this.lblVersionSubTitle.Location = new System.Drawing.Point(517, 113);
      this.lblVersionSubTitle.Name = "lblVersionSubTitle";
      this.lblVersionSubTitle.Size = new System.Drawing.Size(31, 15);
      this.lblVersionSubTitle.TabIndex = 2;
      this.lblVersionSubTitle.Text = "1.0.2";
      // 
      // AboutDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
      this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
      this.ClientSize = new System.Drawing.Size(561, 273);
      this.Controls.Add(this.lblVersionSubTitle);
      this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
      this.ForeColor = System.Drawing.Color.Transparent;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AboutDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "MySQL Notifier";
      this.Load += new System.EventHandler(this.AboutDialog_Load);
      this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.AboutDialog_MouseClick);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label lblVersionSubTitle;


  }
}