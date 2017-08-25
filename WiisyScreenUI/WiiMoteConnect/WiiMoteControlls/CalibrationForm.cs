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

namespace WiiMoteConnect.WiiMoteControlls
{
    public partial class CalibrationForm : Form
    {
        private const float k_DefaultMargin = .1f;
        private const float k_MaxMargin = .6f;
        private Pen m_Pen;
        private int k_CrossSize = 25;
        private WiiMoteWrapper m_WiiMoteWrapper;
        private int m_StepCounter;
        private Bitmap m_CrossBitmap;
        private float m_FormTopMargin;
        private readonly int r_ScreenHeight;
        public float CalibrationTopMargin { get { return m_FormTopMargin; } }
        public event EventHandler CalibrationHeightChangedEvent;
        private bool isTestingMode;
        private bool v_IsDragged;
        private Timer m_BlinkingTimer;

        public CalibrationForm(WiiMoteWrapper i_WiiMoteWrapper)
        {
            InitializeComponent();
            isTestingMode = false;
            r_ScreenHeight = this.Height = Screen.PrimaryScreen.Bounds.Height;
            m_WiiMoteWrapper = i_WiiMoteWrapper;
            m_StepCounter = 0;
            m_FormTopMargin = 0;
            m_Pen = new Pen(Color.Blue);
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            CrossPictureBox.Left = CrossPictureBox.Top = 0;
            v_IsDragged = false;
        }

        /// This ctor is for testing only!!
        public CalibrationForm()
        {
            isTestingMode = true;
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
            switch (m_StepCounter)
            {
                case 0:
                    drawCross(new Point((int)(Width * k_DefaultMargin), Height - (int)(r_ScreenHeight * k_DefaultMargin)));
                    stopBlinking();
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
            if (!isTestingMode)
            {
                m_WiiMoteWrapper.InfraRedAppearedEvent += nextCalibrationStep;
                m_WiiMoteWrapper.BButtonPressed += onBButtonPressed;
                this.TopMost = true;
            }
            resetCalibration();
            labelStart.Location = new Point((int)(this.Width * k_DefaultMargin) - (labelStart.Width / 2), (int)(this.Height * k_DefaultMargin) + 30);
            startBlink();
        }

        private void startBlink()
        {
            if (m_BlinkingTimer == null)
            {
                m_BlinkingTimer = new Timer();
                m_BlinkingTimer.Interval = 500;
                m_BlinkingTimer.Tick += onBlinkTick;
            }

            m_BlinkingTimer.Start();
        }

        private void onBlinkTick(object sender, EventArgs e)
        {
            labelStart.Visible = !labelStart.Visible;
        }

        private void stopBlinking()
        {
            m_BlinkingTimer.Stop();
            labelStart.Invoke(new Action(() => { try { labelStart.Visible = false; } catch (Exception) { }; }));
        }

        protected override void OnClosed(EventArgs e)
        {
            if (!isTestingMode)
            {
                m_WiiMoteWrapper.InfraRedAppearedEvent -= nextCalibrationStep;
                m_WiiMoteWrapper.BButtonPressed -= onBButtonPressed;
            }
            base.OnClosed(e);
        }

        private void resetCalibration()
        {
            m_StepCounter = 0;
            CrossPictureBox.Size = new Size(Width, Height);
            drawCross(new Point((int)(Width * k_DefaultMargin), (int)(r_ScreenHeight * k_DefaultMargin)));
            startBlink();
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

        private void fireHeightChangeEvent()
        {
            if (CalibrationHeightChangedEvent != null)
            {
                CalibrationHeightChangedEvent(this, EventArgs.Empty);
            }
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

        private void buttonDrag_MouseDown(object sender, MouseEventArgs e)
        {
            v_IsDragged = true;
            labelLine.Visible = true;
        }

        private void buttonDrag_MouseUp(object sender, MouseEventArgs e)
        {
            if (v_IsDragged)
            {
                updateTopMargin(Cursor.Position.Y);
                this.Size = new Size(this.Size.Width, (int)((1f - m_FormTopMargin) * r_ScreenHeight));
                Location = new Point(Location.X, (int)(m_FormTopMargin * r_ScreenHeight));
            }
            labelLine.Visible = v_IsDragged = false;
            resetCalibration();
            fireHeightChangeEvent();
        }


        private void buttonDrag_MouseMove(object sender, MouseEventArgs e)
        {
            if (v_IsDragged)
            {
                updateTopMargin(e.Y);
                labelLine.Location = new Point(Location.X, (int)(m_FormTopMargin * r_ScreenHeight));
            }
        }

        private float updateTopMargin(float newY)
        {
            float WindowNewHeightPrecent = ((float)(r_ScreenHeight - newY) / r_ScreenHeight) * 100;
            if (WindowNewHeightPrecent >= 99)
            {
                WindowNewHeightPrecent = 100;
            }
            int newMargin = 100 - (int)WindowNewHeightPrecent;
            return m_FormTopMargin = Math.Min((float)newMargin / 100, k_MaxMargin);
        }
    }
}
