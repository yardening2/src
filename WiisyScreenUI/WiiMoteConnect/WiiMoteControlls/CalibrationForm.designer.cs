namespace WiiMoteConnect.WiiMoteControlls
{
    partial class CalibrationForm
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
            this.CrossPictureBox = new System.Windows.Forms.PictureBox();
            this.SmallerCalibrationButtom = new System.Windows.Forms.Button();
            this.BiggerCalibrationButtom = new System.Windows.Forms.Button();
            this.CalibrationSizePanel = new System.Windows.Forms.Panel();
            this.TitlePictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.CrossPictureBox)).BeginInit();
            this.CalibrationSizePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TitlePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // CrossPictureBox
            // 
            this.CrossPictureBox.Location = new System.Drawing.Point(12, 12);
            this.CrossPictureBox.Name = "CrossPictureBox";
            this.CrossPictureBox.Size = new System.Drawing.Size(75, 41);
            this.CrossPictureBox.TabIndex = 0;
            this.CrossPictureBox.TabStop = false;
            // 
            // SmallerCalibrationButtom
            // 
            this.SmallerCalibrationButtom.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SmallerCalibrationButtom.Location = new System.Drawing.Point(18, 63);
            this.SmallerCalibrationButtom.Name = "SmallerCalibrationButtom";
            this.SmallerCalibrationButtom.Size = new System.Drawing.Size(40, 40);
            this.SmallerCalibrationButtom.TabIndex = 2;
            this.SmallerCalibrationButtom.Text = "-";
            this.SmallerCalibrationButtom.UseVisualStyleBackColor = true;
            this.SmallerCalibrationButtom.Click += new System.EventHandler(this.SmallerCalibrationButtom_Click);
            // 
            // BiggerCalibrationButtom
            // 
            this.BiggerCalibrationButtom.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BiggerCalibrationButtom.Location = new System.Drawing.Point(18, 16);
            this.BiggerCalibrationButtom.Name = "BiggerCalibrationButtom";
            this.BiggerCalibrationButtom.Size = new System.Drawing.Size(40, 40);
            this.BiggerCalibrationButtom.TabIndex = 3;
            this.BiggerCalibrationButtom.Text = "+";
            this.BiggerCalibrationButtom.UseVisualStyleBackColor = true;
            this.BiggerCalibrationButtom.Click += new System.EventHandler(this.BiggerCalibrationButtom_Click);
            // 
            // CalibrationSizePanel
            // 
            this.CalibrationSizePanel.Controls.Add(this.SmallerCalibrationButtom);
            this.CalibrationSizePanel.Controls.Add(this.BiggerCalibrationButtom);
            this.CalibrationSizePanel.Location = new System.Drawing.Point(361, 501);
            this.CalibrationSizePanel.Name = "CalibrationSizePanel";
            this.CalibrationSizePanel.Size = new System.Drawing.Size(75, 114);
            this.CalibrationSizePanel.TabIndex = 4;
            // 
            // TitlePictureBox
            // 
            this.TitlePictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.TitlePictureBox.Image = global::WiiMoteConnect.Properties.Resources.WiiSyScreen;
            this.TitlePictureBox.Location = new System.Drawing.Point(25, 241);
            this.TitlePictureBox.Name = "TitlePictureBox";
            this.TitlePictureBox.Size = new System.Drawing.Size(849, 195);
            this.TitlePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.TitlePictureBox.TabIndex = 5;
            this.TitlePictureBox.TabStop = false;
            // 
            // CalibrationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(902, 661);
            this.ControlBox = false;
            this.Controls.Add(this.TitlePictureBox);
            this.Controls.Add(this.CalibrationSizePanel);
            this.Controls.Add(this.CrossPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CalibrationForm";
            this.Text = "Calibrating...";
            ((System.ComponentModel.ISupportInitialize)(this.CrossPictureBox)).EndInit();
            this.CalibrationSizePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TitlePictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox CrossPictureBox;
        private System.Windows.Forms.Button SmallerCalibrationButtom;
        private System.Windows.Forms.Button BiggerCalibrationButtom;
        private System.Windows.Forms.Panel CalibrationSizePanel;
        private System.Windows.Forms.PictureBox TitlePictureBox;
    }
}