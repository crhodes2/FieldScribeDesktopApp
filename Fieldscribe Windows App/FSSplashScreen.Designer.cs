using System.Drawing;

namespace Fieldscribe_Windows_App
{
    partial class FSSplashScreen
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
            this.Icon = Fieldscribe_Windows_App.Properties.Resources.logo0_1_for_icon_Mxt_icon;
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FSSplashScreen));
            this.staticLogo = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.time = new System.Windows.Forms.Timer(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.staticLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // staticLogo
            // 
            this.staticLogo.Image = ((System.Drawing.Image)(resources.GetObject("staticLogo.Image")));
            this.staticLogo.Location = new System.Drawing.Point(12, 12);
            this.staticLogo.Name = "staticLogo";
            this.staticLogo.Size = new System.Drawing.Size(554, 258);
            this.staticLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.staticLogo.TabIndex = 0;
            this.staticLogo.TabStop = false;
            this.staticLogo.Click += new System.EventHandler(this.staticLogo_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(-38, -79);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(672, 373);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // time
            // 
            this.time.Interval = 2315;
            this.time.Tick += new System.EventHandler(this.time_Tick);
            // 
            // FSSplashScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.ClientSize = new System.Drawing.Size(569, 282);
            this.Controls.Add(this.staticLogo);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FSSplashScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FSSplashScreen";
            this.Load += new System.EventHandler(this.FSSplashScreen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.staticLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox staticLogo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer time;
        private System.Windows.Forms.Timer timer1;
    }
}