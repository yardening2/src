using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WiimoteLib;
using System.Threading;

namespace WiiSyScreen.WiiMoteControlls
{

    public class WiiMoteWrapper
    {
        public delegate void WiimoteStateChangedEventHandler(object i_WiiMote, WiimoteState i_WiiMoteState);
        public delegate void WiiMoteValueChangedEventHandler(object i_WiiMote, int i_VisibleIRCount);
        
        private WiimoteState m_CurrentWiiMoteState;
        private WiimoteState m_PreviousWiiMoteState;
        private Wiimote m_WiiMote;
        private Mutex m_StateChangedMutex;

        public event WiimoteStateChangedEventHandler InfraRedAppearedEvent;
        public event WiimoteStateChangedEventHandler InfraRedDisppearedEvent;
        public event WiimoteStateChangedEventHandler InfraRedMovedEvent;
        public event WiiMoteValueChangedEventHandler BatteryStateChangedEvent;
        public event WiiMoteValueChangedEventHandler VisibleIRDotsChangedEvent;
        public event EventHandler AButtonPressed;
        public event EventHandler BButtonPressed;
        public event EventHandler MinusButtonPressed;
        public event EventHandler PlusButtonPressed;

        public WiimoteState CurrentWiiMoteState { get { return m_CurrentWiiMoteState; } }
        public int BatteryLevel { get { return (100*m_CurrentWiiMoteState.Battery)/192; } }
        public WiiMoteWrapper()
        {
            m_WiiMote = new Wiimote();
            m_StateChangedMutex = new Mutex();
        }

        public void ConnectToWiimote()
        {
            m_WiiMote.Connect();
            m_WiiMote.SetReportType(Wiimote.InputReport.IRAccel, true);
            m_WiiMote.SetLEDs(true, false, false, false);
            m_CurrentWiiMoteState = m_WiiMote.WiimoteState;
            m_PreviousWiiMoteState = copyWiiMoteState(m_CurrentWiiMoteState);
            m_WiiMote.WiimoteChanged += onWiimoteChanged;
        }

        private void onWiimoteChanged(object i_WiiMote, WiimoteChangedEventArgs i_WiimoteChangedEventArgs)
        {
            m_StateChangedMutex.WaitOne();
            m_CurrentWiiMoteState = i_WiimoteChangedEventArgs.WiimoteState;
            fireInfraRedEvents();
            fireButtonsEvents();
            fireBattteryEvents();
            m_PreviousWiiMoteState = copyWiiMoteState(m_CurrentWiiMoteState);
            m_StateChangedMutex.ReleaseMutex();
        }

        private void fireBattteryEvents ()
        {
            if (isBatteryStateChanged() && BatteryStateChangedEvent != null)
            {
                BatteryStateChangedEvent(this.m_WiiMote, this.BatteryLevel);
            }
        }

        private bool isBatteryStateChanged()
        {
            return m_PreviousWiiMoteState.Battery != m_CurrentWiiMoteState.Battery;
        }

        private void fireInfraRedEvents()
        {
            if (isInfraRedAppeard() && InfraRedAppearedEvent != null)
            {
                InfraRedAppearedEvent(this.m_WiiMote, m_CurrentWiiMoteState);
            }

            if (isInfraRedDisappeard() && InfraRedDisppearedEvent != null)
            {
                InfraRedDisppearedEvent(this.m_WiiMote, m_CurrentWiiMoteState);
            }

            if (isInfraRedMoved() && InfraRedMovedEvent != null)
            {
                InfraRedMovedEvent(this.m_WiiMote, m_CurrentWiiMoteState);
            }
            fireIRCountChanged();
        }

        private void fireIRCountChanged()
        {
            int currentVisibleIR = countVisibleIRDots(m_CurrentWiiMoteState);
            int previousVisibleIR = countVisibleIRDots(m_PreviousWiiMoteState);
            if (currentVisibleIR != previousVisibleIR && VisibleIRDotsChangedEvent != null)
            {
                VisibleIRDotsChangedEvent(this.m_WiiMote, currentVisibleIR);
            }
        }

        private int countVisibleIRDots(WiimoteState i_WiiMoteState)
        {
            int IRDotsCount = 0;
            IRDotsCount += i_WiiMoteState.IRState.Found1 ? 1 : 0;
            IRDotsCount += i_WiiMoteState.IRState.Found2 ? 1 : 0;
            IRDotsCount += i_WiiMoteState.IRState.Found3 ? 1 : 0;
            IRDotsCount += i_WiiMoteState.IRState.Found4 ? 1 : 0;
            return IRDotsCount;
        }

        private bool isInfraRedMoved()
        {
            return m_CurrentWiiMoteState.IRState.Found1 && m_PreviousWiiMoteState.IRState.Found1 && isPreviosIRLocationChanged();
        }

        private bool isPreviosIRLocationChanged()
        {
            return m_CurrentWiiMoteState.IRState.RawX1 != m_PreviousWiiMoteState.IRState.RawX1 ||
                m_CurrentWiiMoteState.IRState.RawY1 != m_PreviousWiiMoteState.IRState.RawY1;
        }

        private bool isInfraRedAppeard()
        {
            return m_CurrentWiiMoteState.IRState.Found1 && !m_PreviousWiiMoteState.IRState.Found1;
        }

        private bool isInfraRedDisappeard()
        {
            return !m_CurrentWiiMoteState.IRState.Found1 && m_PreviousWiiMoteState.IRState.Found1;
        }

        private void fireButtonsEvents()
        {
            if (isAButtonPressed() && AButtonPressed != null)
            {
                AButtonPressed(this.m_WiiMote, EventArgs.Empty);
            }

            if (isBButtonPressed() && BButtonPressed != null)
            {
                BButtonPressed(this.m_WiiMote, EventArgs.Empty);
            }

            if (isMinusButtonPressed() && MinusButtonPressed != null)
            {
                MinusButtonPressed(this.m_WiiMote, EventArgs.Empty);
            }

            if (isPlusButtonPressed() && PlusButtonPressed!= null)
            {
                PlusButtonPressed(this.m_WiiMote, EventArgs.Empty);
            }
        }

        private bool isMinusButtonPressed()
        {
            return !m_PreviousWiiMoteState.ButtonState.Minus && m_CurrentWiiMoteState.ButtonState.Minus;
        }

        private bool isPlusButtonPressed()
        {
            return !m_PreviousWiiMoteState.ButtonState.Plus && m_CurrentWiiMoteState.ButtonState.Plus;
        }

        private bool isAButtonPressed()
        {
            return !m_PreviousWiiMoteState.ButtonState.A && m_CurrentWiiMoteState.ButtonState.A;
        }

        private bool isBButtonPressed()
        {
            return !m_PreviousWiiMoteState.ButtonState.B && m_CurrentWiiMoteState.ButtonState.B;
        }

        private WiimoteState copyWiiMoteState(WiimoteState i_WiiMoteState)
        {
            WiimoteState resultState = new WiimoteState();
            resultState.AccelCalibrationInfo.X0 = i_WiiMoteState.AccelCalibrationInfo.X0;
            resultState.AccelCalibrationInfo.XG = i_WiiMoteState.AccelCalibrationInfo.XG;
            resultState.AccelCalibrationInfo.Y0 = i_WiiMoteState.AccelCalibrationInfo.Y0;
            resultState.AccelCalibrationInfo.YG = i_WiiMoteState.AccelCalibrationInfo.Y0;
            resultState.AccelState.RawX = i_WiiMoteState.AccelState.RawX;
            resultState.AccelState.RawY = i_WiiMoteState.AccelState.RawY;
            resultState.AccelState.RawZ = i_WiiMoteState.AccelState.RawZ;
            resultState.AccelState.X = i_WiiMoteState.AccelState.X;
            resultState.AccelState.Y = i_WiiMoteState.AccelState.Y;
            resultState.AccelState.Z = i_WiiMoteState.AccelState.Z;
            resultState.Battery = i_WiiMoteState.Battery;
            resultState.ButtonState.A = i_WiiMoteState.ButtonState.A;
            resultState.ButtonState.B = i_WiiMoteState.ButtonState.B;
            resultState.ButtonState.Down = i_WiiMoteState.ButtonState.Down;
            resultState.ButtonState.Up = i_WiiMoteState.ButtonState.Up;
            resultState.ButtonState.Left = i_WiiMoteState.ButtonState.Left;
            resultState.ButtonState.Right = i_WiiMoteState.ButtonState.Right;
            resultState.ButtonState.Minus = i_WiiMoteState.ButtonState.Minus;
            resultState.ButtonState.Plus = i_WiiMoteState.ButtonState.Plus;
            resultState.ButtonState.Home = i_WiiMoteState.ButtonState.Home;
            resultState.ButtonState.One = i_WiiMoteState.ButtonState.One;
            resultState.ButtonState.Two = i_WiiMoteState.ButtonState.Two;
            resultState.Extension = i_WiiMoteState.Extension;
            resultState.ExtensionType = i_WiiMoteState.ExtensionType;
            resultState.IRState.Found1 = i_WiiMoteState.IRState.Found1;
            resultState.IRState.RawX1 = i_WiiMoteState.IRState.RawX1;
            resultState.IRState.RawY1 = i_WiiMoteState.IRState.RawY1;
            resultState.IRState.Found2 = i_WiiMoteState.IRState.Found2;
            resultState.IRState.RawX2 = i_WiiMoteState.IRState.RawX2;
            resultState.IRState.RawY2 = i_WiiMoteState.IRState.RawY2;
            resultState.IRState.Found3 = i_WiiMoteState.IRState.Found3;
            resultState.IRState.RawX3 = i_WiiMoteState.IRState.RawX3;
            resultState.IRState.RawY3 = i_WiiMoteState.IRState.RawY3;
            resultState.IRState.Found4 = i_WiiMoteState.IRState.Found4;
            resultState.IRState.RawX4 = i_WiiMoteState.IRState.RawX4;
            resultState.IRState.RawY4 = i_WiiMoteState.IRState.RawY4;
            return resultState;
        }
    }
}
