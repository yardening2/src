namespace WiiSyScreen
{
    partial class MainForm
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
            this.IRDotsDataLabel = new System.Windows.Forms.Label();
            this.VisibleIRDotsLabel = new System.Windows.Forms.Label();
            this.ConnectingLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // connectToWiiMoteButton
            // 
            this.connectToWiiMoteButton.Location = new System.Drawing.Point(57, 183);
            this.connectToWiiMoteButton.Name = "connectToWiiMoteButton";
            this.connectToWiiMoteButton.Size = new System.Drawing.Size(75, 23);
            this.connectToWiiMoteButton.TabIndex = 0;
            this.connectToWiiMoteButton.Text = "Connect";
            this.connectToWiiMoteButton.UseVisualStyleBackColor = true;
            this.connectToWiiMoteButton.Click += new System.EventHandler(this.connectToWiiMoteButton_Click);
            // 
            // CalibrateButton
            // 
            this.CalibrateButton.Location = new System.Drawing.Point(57, 212);
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
            // IRDotsDataLabel
            // 
            this.IRDotsDataLabel.AutoSize = true;
            this.IRDotsDataLabel.Location = new System.Drawing.Point(92, 45);
            this.IRDotsDataLabel.Name = "IRDotsDataLabel";
            this.IRDotsDataLabel.Size = new System.Drawing.Size(13, 13);
            this.IRDotsDataLabel.TabIndex = 5;
            this.IRDotsDataLabel.Text = "0";
            this.IRDotsDataLabel.Visible = false;
            // 
            // VisibleIRDotsLabel
            // 
            this.VisibleIRDotsLabel.AutoSize = true;
            this.VisibleIRDotsLabel.Location = new System.Drawing.Point(13, 45);
            this.VisibleIRDotsLabel.Name = "VisibleIRDotsLabel";
            this.VisibleIRDotsLabel.Size = new System.Drawing.Size(79, 13);
            this.VisibleIRDotsLabel.TabIndex = 4;
            this.VisibleIRDotsLabel.Text = "Visible IR Dots:";
            this.VisibleIRDotsLabel.Visible = false;
            // 
            // ConnectingLabel
            // 
            this.ConnectingLabel.AutoSize = true;
            this.ConnectingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectingLabel.Location = new System.Drawing.Point(1, 100);
            this.ConnectingLabel.Name = "ConnectingLabel";
            this.ConnectingLabel.Size = new System.Drawing.Size(189, 31);
            this.ConnectingLabel.TabIndex = 6;
            this.ConnectingLabel.Text = "Connecting...";
            this.ConnectingLabel.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(192, 261);
            this.Controls.Add(this.ConnectingLabel);
            this.Controls.Add(this.IRDotsDataLabel);
            this.Controls.Add(this.VisibleIRDotsLabel);
            this.Controls.Add(this.BatteryLevelValueLabel);
            this.Controls.Add(this.BatteryLevelTextLabel);
            this.Controls.Add(this.CalibrateButton);
            this.Controls.Add(this.connectToWiiMoteButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "WiiSyScreen";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button connectToWiiMoteButton;
        private System.Windows.Forms.Button CalibrateButton;
        private System.Windows.Forms.Label BatteryLevelTextLabel;
        private System.Windows.Forms.Label BatteryLevelValueLabel;
        private System.Windows.Forms.Label IRDotsDataLabel;
        private System.Windows.Forms.Label VisibleIRDotsLabel;
        private System.Windows.Forms.Label ConnectingLabel;
    }
}

