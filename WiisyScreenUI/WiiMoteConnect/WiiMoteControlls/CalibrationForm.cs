﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using WiimoteLib;

namespace WiiMoteConnect.WiiMoteControlls
{
    public partial class CalibrationForm : Form
    {
        private const float k_DefaultMargin = .1f;
        private Pen m_Pen;
        private int k_CrossSize = 25;
        private WiiMoteWrapper m_WiiMoteWrapper;
        private int m_StepCounter;
        private Bitmap m_CrossBitmap;
        private float m_FormTopMargin;
        private readonly int r_ScreenHeight;
        public float CalibrationTopMargin { get { return m_FormTopMargin; } }
        public event EventHandler CalibrationHeightChangedEvent;

        public CalibrationForm(WiiMoteWrapper i_WiiMoteWrapper)
        {
            InitializeComponent();
            r_ScreenHeight = this.Height = Screen.PrimaryScreen.Bounds.Height;
            m_WiiMoteWrapper = i_WiiMoteWrapper;
            m_StepCounter = 0;
            m_FormTopMargin = 0;
            m_Pen = new Pen(Color.Blue);
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            CrossPictureBox.Left = CrossPictureBox.Top = 0;
            CalibrationSizePanel.Location = new Point(Width / 2 - (CalibrationSizePanel.Width / 2), Height - CalibrationSizePanel.Height * 2);
        }

        /// This ctor is for testing only!!
        public CalibrationForm()
        {
            InitializeComponent();
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            r_ScreenHeight = this.Height = Screen.PrimaryScreen.Bounds.Height;
            m_Pen = new Pen(Color.Blue);
            m_StepCounter = 0;
            m_FormTopMargin = 0;
            CrossPictureBox.Left = CrossPictureBox.Top = 0;
        }
        /// -----------------------------------------------------------------------------
        
        private void nextCalibrationStep(object i_WiiMoteWrapper, WiimoteState i_WiiMoteState)
        {
            this.Invoke(new Action(() => { CalibrationSizePanel.Visible = false; }));
            switch (m_StepCounter)
            {
                case 0:
                    drawCross(new Point((int)(Width * k_DefaultMargin), Height - (int)(r_ScreenHeight * k_DefaultMargin)));
                    break;
                case 1:
                    drawCross(new Point(Width - (int)(Width * k_DefaultMargin), (int)(r_ScreenHeight * k_DefaultMargin)));
                    break;
                case 2:
                    drawCross(new Point(Width - (int)(Width * k_DefaultMargin), Height - (int)(r_ScreenHeight * k_DefaultMargin)));
                    break;
                case 3:
                    this.Invoke(new Action(() => { this.Close(); }));
                    break;
            }

            m_StepCounter++;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            m_WiiMoteWrapper.InfraRedAppearedEvent += nextCalibrationStep;
            m_WiiMoteWrapper.BButtonPressed += onBButtonPressed;
            m_WiiMoteWrapper.MinusButtonPressed += SmallerCalibrationButtom_Click;
            m_WiiMoteWrapper.PlusButtonPressed += BiggerCalibrationButtom_Click;
            this.TopMost = true;
            resetCalibration();
        }

        protected override void OnClosed(EventArgs e)
        {
            m_WiiMoteWrapper.InfraRedAppearedEvent -= nextCalibrationStep;
            m_WiiMoteWrapper.BButtonPressed -= onBButtonPressed;
            m_WiiMoteWrapper.MinusButtonPressed -= SmallerCalibrationButtom_Click;
            m_WiiMoteWrapper.PlusButtonPressed -= BiggerCalibrationButtom_Click;
            base.OnClosed(e);
        }

        private void resetCalibration()
        {
            m_StepCounter = 0;
            CrossPictureBox.Size = new Size(Width, Height);
            drawCross(new Point((int)(Width * k_DefaultMargin), (int)(r_ScreenHeight * k_DefaultMargin)));
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
            this.Invoke(new Action(() =>
            {
                int HeightDelta = (int)(.1f * Screen.PrimaryScreen.Bounds.Height);
                this.Size = new Size(this.Size.Width, this.Size.Height - HeightDelta);
                Location = new Point(Location.X, Location.Y + HeightDelta);
                CalibrationSizePanel.Location = new Point(Width / 2 - (CalibrationSizePanel.Width / 2), Height - CalibrationSizePanel.Height * 2);
                m_FormTopMargin += .1f;
                resetCalibration();
                fireHeightChangeEvent();
            }));
        }

        private void fireHeightChangeEvent()
        {
            if (CalibrationHeightChangedEvent != null)
            {
                CalibrationHeightChangedEvent(this, EventArgs.Empty);
            }
        }

        private void BiggerCalibrationButtom_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                int HeightDelta = (int)(.1f * r_ScreenHeight);
                if (this.Size.Height + HeightDelta <= r_ScreenHeight)
                {
                    this.Size = new Size(this.Size.Width, this.Size.Height + HeightDelta);
                    Location = new Point(Location.X, Location.Y - HeightDelta);
                    CalibrationSizePanel.Location = new Point(Width / 2 - (CalibrationSizePanel.Width / 2), Height - CalibrationSizePanel.Height * 2);
                    m_FormTopMargin -= .1f;
                    resetCalibration();
                    fireHeightChangeEvent();
                }
            }));
        }

        private void onBButtonPressed(object i_Sender, EventArgs i_Args)
        {
            this.Invoke(new Action(() => { this.Close(); }));
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if(keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }
    }
}