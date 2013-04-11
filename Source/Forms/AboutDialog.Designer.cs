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
      this.installerVersion = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // lblVersionSubTitle
      // 
      this.lblVersionSubTitle.AutoSize = true;
      this.lblVersionSubTitle.ForeColor = System.Drawing.Color.Gray;
      this.lblVersionSubTitle.Location = new System.Drawing.Point(435, 113);
      this.lblVersionSubTitle.Name = "lblVersionSubTitle";
      this.lblVersionSubTitle.Size = new System.Drawing.Size(115, 15);
      this.lblVersionSubTitle.TabIndex = 2;
      this.lblVersionSubTitle.Text = "MySQL Notifier 1.0.2";
      this.lblVersionSubTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // installerVersion
      // 
      this.installerVersion.AutoSize = true;
      this.installerVersion.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.installerVersion.ForeColor = System.Drawing.Color.Gray;
      this.installerVersion.Location = new System.Drawing.Point(431, 129);
      this.installerVersion.Name = "installerVersion";
      this.installerVersion.Size = new System.Drawing.Size(119, 17);
      this.installerVersion.TabIndex = 3;
      this.installerVersion.Text = "MySQL Installer 1.2";
      this.installerVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // AboutDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
      this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
      this.ClientSize = new System.Drawing.Size(561, 273);
      this.Controls.Add(this.installerVersion);
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
    private System.Windows.Forms.Label installerVersion;


  }
}