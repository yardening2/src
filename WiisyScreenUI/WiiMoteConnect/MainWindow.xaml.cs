using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using InTheHand;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using InTheHand.Net.Ports;
using WiisyScreen.WiiMoteControlls;

namespace WiiMoteConnect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WiiMoteWrapper m_WiiMoteWrapper;
        private Calibrator m_Calibrator;
        private WiiMoteToMouseCoverter m_WiimoteToMouse;
        private readonly Guid r_UUID = new Guid("00001000-0000-1000-8000-00805f9b34fb");
        public WiiMoteToMouseCoverter ToMouseConverter { get { return m_WiimoteToMouse; } }

        public MainWindow()
        {
            InitializeComponent();
            m_WiiMoteWrapper = new WiiMoteWrapper();
            m_Calibrator = new Calibrator(m_WiiMoteWrapper);
            m_Calibrator.CalibrateFinishedEvent += onCalibrationFinished;
        }

        private void onCalibrationFinished(object i_Sender, EventArgs i_EventArgs)
        {
            m_WiimoteToMouse = new WiiMoteToMouseCoverter(m_Calibrator.getCalibratedWarper(), m_WiiMoteWrapper);
        }

        private void SearchWiimote()
        {
            BluetoothClient btClient = new BluetoothClient();
            BluetoothDeviceInfo[] devices = btClient.DiscoverDevicesInRange();
            BluetoothDeviceInfo DeviceToConnect = getWiiMoteBTDevicec(devices);
            if (DeviceToConnect == null)
            {
                devices = btClient.DiscoverDevicesInRange();
                DeviceToConnect = getWiiMoteBTDevicec(devices);
                if (DeviceToConnect == null)
                {
                    Exception e = new Exception("Could Not Find a WiiMote");
                    throw (e);
                }
            }
            if (checkWiiMoteConnectionAbility(DeviceToConnect))
            {
                if (DeviceToConnect.InstalledServices.Length != 0)
                {
                    BluetoothSecurity.RemoveDevice(DeviceToConnect.DeviceAddress);
                }
                DeviceToConnect.Refresh();
                btClient.BeginConnect(DeviceToConnect.DeviceAddress, r_UUID, connectionCallbackFunction, btClient);
                DeviceToConnect.SetServiceState(BluetoothService.HumanInterfaceDevice, true);
                connectAsHIDDevice();
            }
            else
            {
                MessageBox.Show("Failed to connect to Wiimote");
            }
        }

        private void connectAsHIDDevice()
        {
            try
            {
                m_WiiMoteWrapper.ConnectToWiimote();
                
            }
            catch (Exception i_Exception)
            {
                System.Windows.MessageBox.Show(i_Exception.Message);
            }
        }

        public void connectionCallbackFunction(IAsyncResult i_ConnectionResult)
        {
            BluetoothClient client = i_ConnectionResult.AsyncState as BluetoothClient;
        }

        private bool checkWiiMoteConnectionAbility(BluetoothDeviceInfo i_Device)
        {
            if (!i_Device.Authenticated)
            {
                if (!BluetoothSecurity.PairRequest(i_Device.DeviceAddress, String.Empty))
                {
                    char[] chArray = i_Device.DeviceAddress.ToString().ToArray<char>();
                    chArray = hexReverse(chArray);
                    string pinCode = hexToAscii(new String(chArray));
                    if (!BluetoothSecurity.PairRequest(i_Device.DeviceAddress, pinCode))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private char[] hexReverse(char[] i_arrToReverse)
        {
            char[] resultArr = new char[i_arrToReverse.Length];
            for (int i = 0; i < i_arrToReverse.Length -1; i+=2)
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

        private BluetoothDeviceInfo getWiiMoteBTDevicec(BluetoothDeviceInfo[] i_Devices)
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

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            Thread searchWiimoteThread = new Thread(new ThreadStart(SearchWiimote));
            searchWiimoteThread.Start();
        }

        private void calibrateButton_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() => { m_Calibrator.CalibrateScreen(m_WiiMoteWrapper); }));
        }
    }
}
