using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using WiimoteLib;

namespace WiiSyScreen.WiiMoteControlls
{
    public partial class CalibrationForm : Form
    {
        private const float k_DefaultMargin = .1f;
        private Pen m_Pen;
        private int k_CrossSize = 25;
        private WiiMoteWrapper m_WiiMoteWrapper;
        private int m_StepCounter;
        private Bitmap m_CrossBitmap;
        private float m_TopMargin;
        private readonly int r_ScreenHeight;

        public CalibrationForm(WiiMoteWrapper i_WiiMoteWrapper)
        {
            InitializeComponent();
            m_Pen = new Pen(Color.Blue);
            m_WiiMoteWrapper = i_WiiMoteWrapper;
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            this.Height = Screen.PrimaryScreen.Bounds.Height;
            m_StepCounter = 0;
            r_ScreenHeight = Screen.PrimaryScreen.Bounds.Height;
            m_WiiMoteWrapper.InfraRedAppearedEvent += nextCalibrationStep;
            m_TopMargin = k_DefaultMargin;
            CrossPictureBox.Left = CrossPictureBox.Top = 0;
            CalibrationSizePanel.Location = new Point(Width - (CalibrationSizePanel.Width / 2), Height - (CalibrationSizePanel.Height / 2)*2);
        }

        /// This ctor is for testing only!!
        public CalibrationForm()
        {
            InitializeComponent();
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            this.Height = Screen.PrimaryScreen.Bounds.Height;
            m_Pen = new Pen(Color.Blue);
            m_StepCounter = 0;
            m_TopMargin = k_DefaultMargin;
            CrossPictureBox.Left = CrossPictureBox.Top = 0;
            CalibrationSizePanel.Location = new Point(Width / 2 - (CalibrationSizePanel.Width / 2), Height - CalibrationSizePanel.Height*2);
        }
        /// -----------------------------------------------------------------------------
        
        private void nextCalibrationStep(object i_WiiMoteWrapper, WiimoteState i_WiiMoteState)
        {
            switch (m_StepCounter)
            {
                case 0:
                    drawCross(new Point((int)(Width * k_DefaultMargin), Height - (int)(r_ScreenHeight * m_TopMargin)));
                    break;
                case 1:
                    drawCross(new Point(Width - (int)(Width * k_DefaultMargin), (int)(r_ScreenHeight * m_TopMargin)));
                    break;
                case 2:
                    drawCross(new Point(Width - (int)(Width * k_DefaultMargin), Height - (int)(r_ScreenHeight * m_TopMargin)));
                    break;
                case 3:
                    break;
            }

            m_StepCounter++;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            resetCalibration();
        }

        private void resetCalibration()
        {
            m_StepCounter = 0;
            CrossPictureBox.Size = new Size(Width, Height);
            drawCross(new Point((int)(Width * k_DefaultMargin), (int)(Height * m_TopMargin)));
        }

        public void drawCross(Point i_Location)
        {
            Graphics graphics;
            m_CrossBitmap = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
            graphics = Graphics.FromImage(m_CrossBitmap);
            graphics.DrawEllipse(m_Pen, i_Location.X - (k_CrossSize / 2), i_Location.Y - (k_CrossSize / 2), k_CrossSize, k_CrossSize);
            graphics.DrawLine(m_Pen, i_Location.X - k_CrossSize, i_Location.Y, i_Location.X + k_CrossSize, i_Location.Y);
            graphics.DrawLine(m_Pen, i_Location.X, i_Location.Y - k_CrossSize, i_Location.X, i_Location.Y + k_CrossSize);
            m_CrossBitmap.MakeTransparent();
            CrossPictureBox.Image = m_CrossBitmap;
        }

        private void CaliTestBtn_Click(object sender, EventArgs e)
        {
            nextCalibrationStep(null, null);
        }

        private void SmallerCalibrationButtom_Click(object sender, EventArgs e)
        {
            int HeightDelta = (int)(.1f * Screen.PrimaryScreen.Bounds.Height);
            this.Size = new Size(this.Size.Width, this.Size.Height - HeightDelta);
            Location = new Point(Location.X, Location.Y + HeightDelta);
            CalibrationSizePanel.Location = new Point(Width / 2 - (CalibrationSizePanel.Width / 2), Height - CalibrationSizePanel.Height*2);
            resetCalibration();
        }
    }
}
