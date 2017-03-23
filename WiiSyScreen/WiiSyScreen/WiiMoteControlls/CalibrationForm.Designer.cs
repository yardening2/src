namespace WiiSyScreen.WiiMoteControlls
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
            this.CaliTestBtn = new System.Windows.Forms.Button();
            this.SmallerCalibrationButtom = new System.Windows.Forms.Button();
            this.BiggerCalibrationButtom = new System.Windows.Forms.Button();
            this.CalibrationSizePanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.CrossPictureBox)).BeginInit();
            this.CalibrationSizePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // CrossPictureBox
            // 
            this.CrossPictureBox.Location = new System.Drawing.Point(78, 94);
            this.CrossPictureBox.Name = "CrossPictureBox";
            this.CrossPictureBox.Size = new System.Drawing.Size(75, 41);
            this.CrossPictureBox.TabIndex = 0;
            this.CrossPictureBox.TabStop = false;
            // 
            // CaliTestBtn
            // 
            this.CaliTestBtn.Location = new System.Drawing.Point(92, 175);
            this.CaliTestBtn.Name = "CaliTestBtn";
            this.CaliTestBtn.Size = new System.Drawing.Size(75, 23);
            this.CaliTestBtn.TabIndex = 1;
            this.CaliTestBtn.Text = "nextCaliState";
            this.CaliTestBtn.UseVisualStyleBackColor = true;
            this.CaliTestBtn.Click += new System.EventHandler(this.CaliTestBtn_Click);
            // 
            // SmallerCalibrationButtom
            // 
            this.SmallerCalibrationButtom.Location = new System.Drawing.Point(18, 63);
            this.SmallerCalibrationButtom.Name = "SmallerCalibrationButtom";
            this.SmallerCalibrationButtom.Size = new System.Drawing.Size(31, 23);
            this.SmallerCalibrationButtom.TabIndex = 2;
            this.SmallerCalibrationButtom.Text = "-";
            this.SmallerCalibrationButtom.UseVisualStyleBackColor = true;
            this.SmallerCalibrationButtom.Click += new System.EventHandler(this.SmallerCalibrationButtom_Click);
            // 
            // BiggerCalibrationButtom
            // 
            this.BiggerCalibrationButtom.Location = new System.Drawing.Point(18, 16);
            this.BiggerCalibrationButtom.Name = "BiggerCalibrationButtom";
            this.BiggerCalibrationButtom.Size = new System.Drawing.Size(31, 23);
            this.BiggerCalibrationButtom.TabIndex = 3;
            this.BiggerCalibrationButtom.Text = "+";
            this.BiggerCalibrationButtom.UseVisualStyleBackColor = true;
            // 
            // CalibrationSizePanel
            // 
            this.CalibrationSizePanel.Controls.Add(this.SmallerCalibrationButtom);
            this.CalibrationSizePanel.Controls.Add(this.BiggerCalibrationButtom);
            this.CalibrationSizePanel.Location = new System.Drawing.Point(97, 149);
            this.CalibrationSizePanel.Name = "CalibrationSizePanel";
            this.CalibrationSizePanel.Size = new System.Drawing.Size(70, 100);
            this.CalibrationSizePanel.TabIndex = 4;
            // 
            // CalibrationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.ControlBox = false;
            this.Controls.Add(this.CalibrationSizePanel);
            this.Controls.Add(this.CaliTestBtn);
            this.Controls.Add(this.CrossPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CalibrationForm";
            this.Text = "Calibrating...";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.CrossPictureBox)).EndInit();
            this.CalibrationSizePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox CrossPictureBox;
        private System.Windows.Forms.Button CaliTestBtn;
        private System.Windows.Forms.Button SmallerCalibrationButtom;
        private System.Windows.Forms.Button BiggerCalibrationButtom;
        private System.Windows.Forms.Panel CalibrationSizePanel;
    }
}