using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WiimoteLib;
using System.Threading;
using System.IO;
using InTheHand;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using InTheHand.Net.Ports;

namespace WiiMoteConnect.WiiMoteControlls
{

    public class WiiMoteWrapper
    {
        public delegate void WiimoteStateChangedEventHandler(object i_WiiMote, WiimoteState i_WiiMoteState);
        public delegate void WiiMoteValueChangedEventHandler(object i_WiiMote, int i_VisibleIRCount);
        public delegate void WiiMoteConnectionStatusChangedEventHandler(object i_WiiMote, eWiiConnectivityState i_state);
        public enum eWiiConnectivityState
        {
            Searching, Not_Found, Connecting, Failed_To_Connect,Connected

        }

        private WiimoteState m_CurrentWiiMoteState;
        private WiimoteState m_PreviousWiiMoteState;
        private Wiimote m_WiiMote;
        private Mutex m_StateChangedMutex;
        private bool m_Connected;
        private readonly Guid r_UUID = new Guid("00001000-0000-1000-8000-00805f9b34fb");
        public event WiiMoteConnectionStatusChangedEventHandler ConnectionStateChangeEvent;
        public event EventHandler ConnectionEstablishedEvent;
        public event WiimoteStateChangedEventHandler InfraRedAppearedEvent;
        public event WiimoteStateChangedEventHandler InfraRedDisppearedEvent;
        public event WiimoteStateChangedEventHandler InfraRedMovedEvent;
        public event WiiMoteValueChangedEventHandler BatteryStateChangedEvent;
        public event WiiMoteValueChangedEventHandler VisibleIRDotsChangedEvent;
        public event EventHandler AButtonPressed;
        public event EventHandler BButtonPressed;
        public event EventHandler MinusButtonPressed;
        public event EventHandler PlusButtonPressed;
        public bool Connected { get { return m_Connected; } }
        public WiimoteState CurrentWiiMoteState { get { return m_CurrentWiiMoteState; } }
        public int BatteryLevel { get { return (100 * m_CurrentWiiMoteState.Battery) / 192; } }

        public WiiMoteWrapper()
        {
            m_Connected = false;
            m_WiiMote = new Wiimote();
            m_StateChangedMutex = new Mutex();
        }
        public void DisconnectFromWiiMote()
        {
            m_WiiMote.Disconnect();
        }

        public void ConnectToWiimote()
        {
            fireConnectionStateChangeEvent(eWiiConnectivityState.Searching);
            try
            {
                finelizeWiiMoteConnection();
            }
            catch (Exception)
            {
                Thread searchWiimoteThread = new Thread(new ThreadStart(SearchWiimote));
                searchWiimoteThread.Start();
            }
        }

        private void finelizeWiiMoteConnection()
        {
            if (!m_Connected)
            {
                m_WiiMote.Connect();
                m_WiiMote.SetReportType(Wiimote.InputReport.IRAccel, true);
                m_WiiMote.SetLEDs(true, false, false, false);
                m_CurrentWiiMoteState = m_WiiMote.WiimoteState;
                m_PreviousWiiMoteState = copyWiiMoteState(m_CurrentWiiMoteState);
                m_WiiMote.WiimoteChanged += onWiimoteChanged;
                m_Connected = true;
                fireConnectionStateChangeEvent(eWiiConnectivityState.Connected);
            }

            if (ConnectionEstablishedEvent != null)
            {
                ConnectionEstablishedEvent.Invoke(this, null);
            }
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

        //--------------Blue Tooth Connection Section-------------------
        private void SearchWiimote()
        {
            if (m_Connected)
            {
                finelizeWiiMoteConnection();
            }
            else
            {
                BluetoothClient btClient = new BluetoothClient();
                BluetoothDeviceInfo[] devices = btClient.DiscoverDevicesInRange();
                BluetoothDeviceInfo DeviceToConnect = getWiiMoteBTDevice(devices);
                if (DeviceToConnect == null)
                {
                    devices = btClient.DiscoverDevicesInRange();
                    DeviceToConnect = getWiiMoteBTDevice(devices);
                    if (DeviceToConnect == null)
                    {
                        fireConnectionStateChangeEvent(eWiiConnectivityState.Not_Found);
                    }
                }
                else if (checkWiiMoteConnectionAbility(DeviceToConnect))
                {
                    fireConnectionStateChangeEvent(eWiiConnectivityState.Connecting);
                    if (DeviceToConnect.InstalledServices.Length != 0)
                    {
                        BluetoothSecurity.RemoveDevice(DeviceToConnect.DeviceAddress);
                    }

                    if (!DeviceToConnect.Remembered)
                    {
                        DeviceToConnect.Refresh();
                        btClient.BeginConnect(DeviceToConnect.DeviceAddress, r_UUID, connectionCallbackFunction, btClient);
                        DeviceToConnect.SetServiceState(BluetoothService.HumanInterfaceDevice, true);
                    }

                    try
                    {
                        finelizeWiiMoteConnection();
                    }
                    catch (Exception)
                    {
                        fireConnectionStateChangeEvent(eWiiConnectivityState.Failed_To_Connect);
                    }
                }
                else
                {
                    fireConnectionStateChangeEvent(eWiiConnectivityState.Failed_To_Connect);
                }
            }
        }

        public void connectionCallbackFunction(IAsyncResult i_ConnectionResult)
        {
            BluetoothClient client = i_ConnectionResult.AsyncState as BluetoothClient;
        }

        private bool checkWiiMoteConnectionAbility(BluetoothDeviceInfo i_Device)
        {
            if (i_Device != null && !i_Device.Authenticated && !i_Device.Remembered)
            {
                char[] chArray = i_Device.DeviceAddress.ToString().ToArray<char>();
                chArray = hexReverse(chArray);
                string pinCode = hexToAscii(new String(chArray));
                if (!BluetoothSecurity.PairRequest(i_Device.DeviceAddress, pinCode))
                {
                    return false;
                }
            }
            return i_Device != null;
        }
        private char[] hexReverse(char[] i_arrToReverse)
        {
            char[] resultArr = new char[i_arrToReverse.Length];
            for (int i = 0; i < i_arrToReverse.Length - 1; i += 2)
            {
                resultArr[i_arrToReverse.Length - 1 - i] = i_arrToReverse[i + 1];
                resultArr[i_arrToReverse.Length - 2 - i] = i_arrToReverse[i];
            }
            return resultArr;
        }

        private string hexToAscii(string i_hexString)
        {
            StringBuilder resultString = new StringBuilder();
            for (int i = 0; i < i_hexString.Length; i += 2)
            {
                string hs = i_hexString.Substring(i, 2);
                resultString.Append(Convert.ToChar(Convert.ToUInt32(hs, 16)));
            }

            return resultString.ToString();
        }

        private void fireConnectionStateChangeEvent(eWiiConnectivityState i_State)
        {
            if (ConnectionStateChangeEvent != null)
            {
                ConnectionStateChangeEvent.Invoke(this, i_State);
            }

        }

        private BluetoothDeviceInfo getWiiMoteBTDevice(BluetoothDeviceInfo[] i_Devices)
        {
            BluetoothDeviceInfo returnDevice = null;
            foreach (BluetoothDeviceInfo device in i_Devices)
            {
                if (device.DeviceName.Contains("Nintendo"))
                {
                    returnDevice = device;
                }
            }
            return returnDevice;
        }
    }
}