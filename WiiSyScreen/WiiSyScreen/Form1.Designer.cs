namespace WiiSyScreen
{
    partial class Form1
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
            this.connectToWiiMoteButton = new System.Windows.Forms.Button();
            this.CalibrateButton = new System.Windows.Forms.Button();
            this.BatteryLevelTextLabel = new System.Windows.Forms.Label();
            this.BatteryLevelValueLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // connectToWiiMoteButton
            // 
            this.connectToWiiMoteButton.Location = new System.Drawing.Point(51, 183);
            this.connectToWiiMoteButton.Name = "connectToWiiMoteButton";
            this.connectToWiiMoteButton.Size = new System.Drawing.Size(75, 23);
            this.connectToWiiMoteButton.TabIndex = 0;
            this.connectToWiiMoteButton.Text = "Connect";
            this.connectToWiiMoteButton.UseVisualStyleBackColor = true;
            this.connectToWiiMoteButton.Click += new System.EventHandler(this.connectToWiiMoteButton_Click);
            // 
            // CalibrateButton
            // 
            this.CalibrateButton.Location = new System.Drawing.Point(51, 212);
            this.CalibrateButton.Name = "CalibrateButton";
            this.CalibrateButton.Size = new System.Drawing.Size(75, 23);
            this.CalibrateButton.TabIndex = 1;
            this.CalibrateButton.Text = "Calibrate";
            this.CalibrateButton.UseVisualStyleBackColor = true;
            this.CalibrateButton.Visible = false;
            this.CalibrateButton.Click += new System.EventHandler(this.CalibrateButton_Click);
            // 
            // BatteryLevelTextLabel
            // 
            this.BatteryLevelTextLabel.AutoSize = true;
            this.BatteryLevelTextLabel.Location = new System.Drawing.Point(13, 13);
            this.BatteryLevelTextLabel.Name = "BatteryLevelTextLabel";
            this.BatteryLevelTextLabel.Size = new System.Drawing.Size(72, 13);
            this.BatteryLevelTextLabel.TabIndex = 2;
            this.BatteryLevelTextLabel.Text = "Battery Level:";
            this.BatteryLevelTextLabel.Visible = false;
            // 
            // BatteryLevelValueLabel
            // 
            this.BatteryLevelValueLabel.AutoSize = true;
            this.BatteryLevelValueLabel.Location = new System.Drawing.Point(91, 13);
            this.BatteryLevelValueLabel.Name = "BatteryLevelValueLabel";
            this.BatteryLevelValueLabel.Size = new System.Drawing.Size(0, 13);
            this.BatteryLevelValueLabel.TabIndex = 3;
            this.BatteryLevelValueLabel.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(192, 261);
            this.Controls.Add(this.BatteryLevelValueLabel);
            this.Controls.Add(this.BatteryLevelTextLabel);
            this.Controls.Add(this.CalibrateButton);
            this.Controls.Add(this.connectToWiiMoteButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button connectToWiiMoteButton;
        private System.Windows.Forms.Button CalibrateButton;
        private System.Windows.Forms.Label BatteryLevelTextLabel;
        private System.Windows.Forms.Label BatteryLevelValueLabel;
    }
}

