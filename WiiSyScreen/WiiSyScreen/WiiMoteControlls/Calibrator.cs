using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WiimoteLib;

namespace WiiSyScreen.WiiMoteControlls
{

    class Calibrator
    {
        private bool m_IsCalibrated = false;
        private WiiMoteWrapper m_WiiMoteWrapper;
        private int m_CurrentCalibrationCounter = 0;
        private readonly int m_ScreenWidth = Screen.PrimaryScreen.Bounds.Width;
        private readonly int m_ScreenHeight = Screen.PrimaryScreen.Bounds.Height;
        private const float m_CalibrationMargin = .1f;
        private readonly float[] m_StaticCalibrationArrayX = new float[4];
        private readonly float[] m_StaticCalibrationArrayY = new float[4];
        private readonly float[] m_InfraRedCalibrationArrayX = new float[4];
        private readonly float[] m_InfraRedCalibrationArrayY = new float[4];
        private readonly Warper m_Warper = new Warper();

        public Warper getCalibratedWarper()
        {
            if (m_IsCalibrated)
            {
                return m_Warper;
            }
            throw new WarperNotCalibratedException();
        }

        public bool IsCalibrated()
        {
            return m_IsCalibrated;
        }

        public void CalibrateScreen(WiiMoteWrapper i_WiiMoteWrapper)
        {
            m_IsCalibrated = false;
            m_WiiMoteWrapper = i_WiiMoteWrapper;
            m_CurrentCalibrationCounter = 0;
            buildStaticCalibrationArray();
            i_WiiMoteWrapper.InfraRedAppearedEvent += buildInfraRedCalibrationArray;
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
                    calibrationPoint = getBottomLeftCalibrationPoint();
                    break;
                case 4:
                    m_WiiMoteWrapper.InfraRedAppearedEvent -= buildInfraRedCalibrationArray;
                    setCalibratedWarperData();
                    m_IsCalibrated = true;
                    break;
                default:
                    break;
            }
            m_StaticCalibrationArrayX[m_CurrentCalibrationCounter] = calibrationPoint.X;
            m_StaticCalibrationArrayY[m_CurrentCalibrationCounter] = calibrationPoint.Y;
        }

        private void buildInfraRedCalibrationArray(object i_WiiMote, WiimoteState i_State)
        {
            m_InfraRedCalibrationArrayX[m_CurrentCalibrationCounter] = i_State.IRState.RawX1;
            m_InfraRedCalibrationArrayY[m_CurrentCalibrationCounter] = i_State.IRState.RawY1;
            m_CurrentCalibrationCounter++;
            buildStaticCalibrationArray();
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
            point.X = (int) (m_ScreenWidth * m_CalibrationMargin);
            point.Y = (int) (m_ScreenHeight * m_CalibrationMargin);
            return point;
        }

        private PointF getBottomLeftCalibrationPoint()
        {
            PointF point = new PointF();
            point.X = (int)(m_ScreenWidth * m_CalibrationMargin);
            point.Y = m_ScreenHeight - (int)(m_ScreenHeight * m_CalibrationMargin);
            return point;
        }

        private PointF getTopRightCalibrationPoint()
        {
            PointF point = new PointF();
            point.X = m_ScreenWidth - (int)(m_ScreenWidth * m_CalibrationMargin);
            point.Y = (int)(m_ScreenHeight * m_CalibrationMargin);
            return point;
        }

        private PointF getBottomRightCalibrationPoint()
        {
            PointF point = new PointF();
            point.X = m_ScreenWidth - (int)(m_ScreenWidth * m_CalibrationMargin);
            point.Y = m_ScreenHeight - (int)(m_ScreenHeight * m_CalibrationMargin);
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
