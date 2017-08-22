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
            this.buttonDrag = new System.Windows.Forms.Button();
            this.labelLine = new System.Windows.Forms.Label();
            this.TitlePictureBox = new System.Windows.Forms.PictureBox();
            this.CrossPictureBox = new System.Windows.Forms.PictureBox();
            this.labelStart = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.TitlePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CrossPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonDrag
            // 
            this.buttonDrag.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDrag.Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDrag.Location = new System.Drawing.Point(257, 0);
            this.buttonDrag.Name = "buttonDrag";
            this.buttonDrag.Size = new System.Drawing.Size(397, 80);
            this.buttonDrag.TabIndex = 4;
            this.buttonDrag.Text = "Pull Here To Resize";
            this.buttonDrag.UseVisualStyleBackColor = true;
            this.buttonDrag.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonDrag_MouseDown);
            this.buttonDrag.MouseMove += new System.Windows.Forms.MouseEventHandler(this.buttonDrag_MouseMove);
            this.buttonDrag.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonDrag_MouseUp);
            // 
            // labelLine
            // 
            this.labelLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLine.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelLine.Font = new System.Drawing.Font("Miriam", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLine.Location = new System.Drawing.Point(-1, 82);
            this.labelLine.Name = "labelLine";
            this.labelLine.Size = new System.Drawing.Size(903, 1);
            this.labelLine.TabIndex = 6;
            this.labelLine.Text = "label1";
            this.labelLine.Visible = false;
            // 
            // TitlePictureBox
            // 
            this.TitlePictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TitlePictureBox.Image = global::WiiMoteConnect.Properties.Resources.logo;
            this.TitlePictureBox.Location = new System.Drawing.Point(94, 154);
            this.TitlePictureBox.Name = "TitlePictureBox";
            this.TitlePictureBox.Size = new System.Drawing.Size(709, 362);
            this.TitlePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.TitlePictureBox.TabIndex = 5;
            this.TitlePictureBox.TabStop = false;
            // 
            // CrossPictureBox
            // 
            this.CrossPictureBox.Location = new System.Drawing.Point(12, 12);
            this.CrossPictureBox.Name = "CrossPictureBox";
            this.CrossPictureBox.Size = new System.Drawing.Size(75, 41);
            this.CrossPictureBox.TabIndex = 0;
            this.CrossPictureBox.TabStop = false;
            // 
            // labelStart
            // 
            this.labelStart.AutoSize = true;
            this.labelStart.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStart.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.labelStart.Location = new System.Drawing.Point(12, 78);
            this.labelStart.Name = "labelStart";
            this.labelStart.Size = new System.Drawing.Size(117, 27);
            this.labelStart.TabIndex = 7;
            this.labelStart.Text = "Start Here";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label1.Location = new System.Drawing.Point(312, 562);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(271, 27);
            this.label1.TabIndex = 8;
            this.label1.Text = "Press Esc anytime to cancel";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(331, 125);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(198, 23);
            this.button1.TabIndex = 9;
            this.button1.TabStop = false;
            this.button1.Text = "Test Next Calibration Step";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.CaliTestBtn_Click);
            // 
            // CalibrationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(902, 661);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelStart);
            this.Controls.Add(this.labelLine);
            this.Controls.Add(this.buttonDrag);
            this.Controls.Add(this.TitlePictureBox);
            this.Controls.Add(this.CrossPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CalibrationForm";
            this.Text = "Calibrating...";
            ((System.ComponentModel.ISupportInitialize)(this.TitlePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CrossPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox CrossPictureBox;
        private System.Windows.Forms.PictureBox TitlePictureBox;
        private System.Windows.Forms.Button buttonDrag;
        private System.Windows.Forms.Label labelLine;
        private System.Windows.Forms.Label labelStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}