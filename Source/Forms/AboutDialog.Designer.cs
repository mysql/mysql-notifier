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

namespace MySql.Notifier.Forms
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
      this.NotifierVersionLabel = new System.Windows.Forms.Label();
      this.InstallerVersionLabel = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // NotifierVersionLabel
      // 
      this.NotifierVersionLabel.AutoSize = true;
      this.NotifierVersionLabel.BackColor = System.Drawing.Color.Transparent;
      this.NotifierVersionLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.NotifierVersionLabel.ForeColor = System.Drawing.Color.White;
      this.NotifierVersionLabel.Location = new System.Drawing.Point(105, 117);
      this.NotifierVersionLabel.Name = "NotifierVersionLabel";
      this.NotifierVersionLabel.Size = new System.Drawing.Size(114, 13);
      this.NotifierVersionLabel.TabIndex = 2;
      this.NotifierVersionLabel.Text = "MySQL Notifier 1.x.x";
      this.NotifierVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // InstallerVersionLabel
      // 
      this.InstallerVersionLabel.AutoSize = true;
      this.InstallerVersionLabel.BackColor = System.Drawing.Color.Transparent;
      this.InstallerVersionLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.InstallerVersionLabel.ForeColor = System.Drawing.Color.White;
      this.InstallerVersionLabel.Location = new System.Drawing.Point(105, 133);
      this.InstallerVersionLabel.Name = "InstallerVersionLabel";
      this.InstallerVersionLabel.Size = new System.Drawing.Size(106, 13);
      this.InstallerVersionLabel.TabIndex = 3;
      this.InstallerVersionLabel.Text = "MySQL Installer 1.3";
      this.InstallerVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // AboutDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackgroundImage = global::MySql.Notifier.Properties.Resources.AboutBackGroundDialog;
      this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
      this.ClientSize = new System.Drawing.Size(560, 322);
      this.Controls.Add(this.InstallerVersionLabel);
      this.Controls.Add(this.NotifierVersionLabel);
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

    private System.Windows.Forms.Label NotifierVersionLabel;
    private System.Windows.Forms.Label InstallerVersionLabel;


  }
}