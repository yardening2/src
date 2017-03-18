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
            m_PreviousWiiMoteState = m_CurrentWiiMoteState = m_WiiMote.WiimoteState;
            m_WiiMote.WiimoteChanged += onWiimoteChanged;
        }

        private void onWiimoteChanged(object i_WiiMote, WiimoteChangedEventArgs i_WiimoteChangedEventArgs)
        {
            m_StateChangedMutex.WaitOne();
            m_CurrentWiiMoteState = i_WiimoteChangedEventArgs.WiimoteState;
            fireInfraRedEvents();
            fireButtonsEvents();
            fireBattteryEvents();
            m_PreviousWiiMoteState = m_CurrentWiiMoteState;
            m_StateChangedMutex.ReleaseMutex();
        }

        private void fireBattteryEvents ()
        {
            if (isBatteryStateChanged() && BatteryStateChangedEvent != null)
            {
                BatteryStateChangedEvent(this.m_WiiMote, m_CurrentWiiMoteState.Battery);
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
                fireIRCountChanged();
            }

            if (isInfraRedDisappeard() && InfraRedDisppearedEvent != null)
            {
                InfraRedDisppearedEvent(this.m_WiiMote, m_CurrentWiiMoteState);
                fireIRCountChanged();
            }

            if (isInfraRedMoved() && InfraRedMovedEvent != null)
            {
                InfraRedMovedEvent(this.m_WiiMote, m_CurrentWiiMoteState);
            }
        }

        private void fireIRCountChanged()
        {
            if (VisibleIRDotsChangedEvent != null)
            {
                VisibleIRDotsChangedEvent(this.m_WiiMote, countVisibleIRDots());
            }
        }

        private int countVisibleIRDots()
        {
            int IRDotsCount = 0;
            IRDotsCount += m_CurrentWiiMoteState.IRState.Found1 ? 1 : 0;
            IRDotsCount += m_CurrentWiiMoteState.IRState.Found2 ? 1 : 0;
            IRDotsCount += m_CurrentWiiMoteState.IRState.Found3 ? 1 : 0;
            IRDotsCount += m_CurrentWiiMoteState.IRState.Found4 ? 1 : 0;
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
        }

        private bool isAButtonPressed()
        {
            return !m_PreviousWiiMoteState.ButtonState.A && m_CurrentWiiMoteState.ButtonState.A;
        }

        private bool isBButtonPressed()
        {
            return !m_PreviousWiiMoteState.ButtonState.B && m_CurrentWiiMoteState.ButtonState.B;
        }
    }
}
