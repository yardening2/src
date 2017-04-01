﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WiimoteLib;
using System.Threading;

namespace WiiSyScreen.WiiMoteControlls
{

    class Calibrator
    {
        private const float k_CalibrationMargin = .1f;

        public bool IsCalibrated { get { return m_IsCalibrated; } }
        public event EventHandler CalibrateFinishedEvent;
        private float m_TopCalibrationMargin;
        private bool m_IsCalibrated;
        private WiiMoteWrapper m_WiiMoteWrapper;
        private int m_CurrentCalibrationCounter;
        private readonly int m_ScreenWidth;
        private readonly int m_ScreenHeight;
        private readonly float[] m_StaticCalibrationArrayX;
        private readonly float[] m_StaticCalibrationArrayY;
        private readonly float[] m_InfraRedCalibrationArrayX;
        private readonly float[] m_InfraRedCalibrationArrayY;
        private readonly Warper m_Warper;
        private CalibrationForm m_CalibratorForm;

        public Calibrator(WiiMoteWrapper i_WiiMoteWraper)
        {
            m_IsCalibrated = false;
            m_CurrentCalibrationCounter = 0;
            m_TopCalibrationMargin = k_CalibrationMargin;
            m_ScreenWidth = Screen.PrimaryScreen.Bounds.Width;
            m_ScreenHeight = Screen.PrimaryScreen.Bounds.Height;
            m_StaticCalibrationArrayX = new float[4];
            m_StaticCalibrationArrayY = new float[4];
            m_InfraRedCalibrationArrayX = new float[4];
            m_InfraRedCalibrationArrayY = new float[4];
            m_Warper = new Warper();
            m_WiiMoteWrapper = i_WiiMoteWraper;
            m_CalibratorForm = null;
        }

        public Warper getCalibratedWarper()
        {
            if (!m_IsCalibrated)
            {
                throw new WarperNotCalibratedException();
            }

            return m_Warper;
        }

        public void CalibrateScreen(WiiMoteWrapper i_WiiMoteWrapper)
        {
            if (m_CalibratorForm == null)
            {
                m_IsCalibrated = false;
                m_WiiMoteWrapper = i_WiiMoteWrapper;
                m_CurrentCalibrationCounter = 0;
                buildStaticCalibrationArray();
                m_CalibratorForm = new CalibrationForm(m_WiiMoteWrapper);
                m_WiiMoteWrapper.InfraRedAppearedEvent += buildInfraRedCalibrationArray;
                m_CalibratorForm.FormClosed += onCalibrationFormClosed;
                m_CalibratorForm.CalibrationHeightChangedEvent += onCalibrationAreaChanged;
                m_CalibratorForm.Show();
            }
        }

        private void onCalibrationFormClosed(object i_Sender, EventArgs i_Args)
        {
            m_WiiMoteWrapper.InfraRedAppearedEvent -= buildInfraRedCalibrationArray;
            m_CalibratorForm = null;
        }

        private void onCalibrationAreaChanged(object i_CalibrationForm, EventArgs i_EventArgs)
        {
            m_TopCalibrationMargin = m_CalibratorForm.CalibrationTopMargin;
        }

        private void buildStaticCalibrationArray()
        {
            PointF calibrationPoint = new PointF();
            switch (m_CurrentCalibrationCounter)
            {
                case 0:
                    calibrationPoint = getTopLeftCalibrationPoint();
                    break;
                case 1:
                    calibrationPoint = getBottomLeftCalibrationPoint();
                    break;
                case 2:
                    calibrationPoint = getTopRightCalibrationPoint();
                    break;
                case 3:
                    calibrationPoint = getBottomRightCalibrationPoint();
                    break;
                case 4:
                    endCalibrationProcess();
                    return;
                default:
                    break;
            }
            m_StaticCalibrationArrayX[m_CurrentCalibrationCounter] = calibrationPoint.X;
            m_StaticCalibrationArrayY[m_CurrentCalibrationCounter] = calibrationPoint.Y;
        }

        private void endCalibrationProcess()
        {
            m_CalibratorForm = null;
            m_WiiMoteWrapper.InfraRedAppearedEvent -= buildInfraRedCalibrationArray;
            setCalibratedWarperData();
            m_IsCalibrated = true;
            if (CalibrateFinishedEvent != null)
            {
                CalibrateFinishedEvent(this, EventArgs.Empty);
            }
        }

        private void buildInfraRedCalibrationArray(object i_WiiMote, WiimoteState i_State)
        {
            m_InfraRedCalibrationArrayX[m_CurrentCalibrationCounter] = i_State.IRState.RawX1;
            m_InfraRedCalibrationArrayY[m_CurrentCalibrationCounter] = i_State.IRState.RawY1;
            if (m_CurrentCalibrationCounter == 1 || m_CurrentCalibrationCounter == 3)
            {
                fixTopPoint();
            }

            m_CurrentCalibrationCounter++;
            buildStaticCalibrationArray();
        }

        private void fixTopPoint()
        {
            PointF fixedPoint = new PointF();

            float ratio = (1 - 2 * k_CalibrationMargin - m_TopCalibrationMargin) / (1 - 2 * k_CalibrationMargin);
            if (m_TopCalibrationMargin >= k_CalibrationMargin)
            {
                fixedPoint.X = calculateValueByTales(m_InfraRedCalibrationArrayX[m_CurrentCalibrationCounter], m_InfraRedCalibrationArrayX[m_CurrentCalibrationCounter - 1], ratio);
                fixedPoint.Y = calculateValueByTales(m_InfraRedCalibrationArrayY[m_CurrentCalibrationCounter], m_InfraRedCalibrationArrayY[m_CurrentCalibrationCounter - 1], ratio);
                m_InfraRedCalibrationArrayX[m_CurrentCalibrationCounter - 1] = fixedPoint.X;
                m_InfraRedCalibrationArrayY[m_CurrentCalibrationCounter - 1] = fixedPoint.Y;
            }
        }

        private float calculateValueByTales(float i_Start, float i_End, float i_Ratio)
        {
            return (Math.Abs(i_Start - i_End) / i_Ratio) + i_Start;
        }

        private void setCalibratedWarperData()
        {
            m_Warper.setDestination(m_StaticCalibrationArrayX[0], m_StaticCalibrationArrayY[0], m_StaticCalibrationArrayX[1], m_StaticCalibrationArrayY[1], 
                m_StaticCalibrationArrayX[2], m_StaticCalibrationArrayY[2], m_StaticCalibrationArrayX[3], m_StaticCalibrationArrayY[3]);
            m_Warper.setSource(m_InfraRedCalibrationArrayX[0], m_InfraRedCalibrationArrayY[0], m_InfraRedCalibrationArrayX[1], m_InfraRedCalibrationArrayY[1], 
                m_InfraRedCalibrationArrayX[2], m_InfraRedCalibrationArrayY[2], m_InfraRedCalibrationArrayX[3], m_InfraRedCalibrationArrayY[3]);
            m_Warper.computeWarp();
        }

        private PointF getTopLeftCalibrationPoint()
        {
            PointF point = new PointF();
            point.X = (int) (m_ScreenWidth * k_CalibrationMargin);
            point.Y = (int)(m_ScreenHeight * k_CalibrationMargin);
            return point;
        }

        private PointF getBottomLeftCalibrationPoint()
        {
            PointF point = new PointF();
            point.X = (int)(m_ScreenWidth * k_CalibrationMargin);
            point.Y = m_ScreenHeight - (int)(m_ScreenHeight * k_CalibrationMargin);
            return point;
        }

        private PointF getTopRightCalibrationPoint()
        {
            PointF point = new PointF();
            point.X = m_ScreenWidth - (int)(m_ScreenWidth * k_CalibrationMargin);
            point.Y = (int)(m_ScreenHeight * k_CalibrationMargin);
            return point;
        }

        private PointF getBottomRightCalibrationPoint()
        {
            PointF point = new PointF();
            point.X = m_ScreenWidth - (int)(m_ScreenWidth * k_CalibrationMargin);
            point.Y = m_ScreenHeight - (int)(m_ScreenHeight * k_CalibrationMargin);
            return point;
        }
    }

    [Serializable()]
    public class WarperNotCalibratedException : System.Exception
    {
        public WarperNotCalibratedException() : base() { }
        public WarperNotCalibratedException(string i_Message) : base(i_Message) { }
        public WarperNotCalibratedException(string i_Message, System.Exception i_Inner) : base(i_Message, i_Inner) { }
    }
}
