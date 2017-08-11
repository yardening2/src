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
                Thread BTWiiMoteClientThread = new Thread(connectToWiiMoteClient);
                BTWiiMoteClientThread.Start(DeviceToConnect);
            }
            else
            {
                MessageBox.Show("kakaksa");
            }
        }

        private void connectToWiiMoteClient(object i_BluetoothDeviceInfo)
        {
            BluetoothDeviceInfo device = i_BluetoothDeviceInfo as BluetoothDeviceInfo;
            BluetoothClient btClient = new BluetoothClient();
            btClient.BeginConnect(device.DeviceAddress,r_UUID, connectionCallbackFunction, btClient);
        }

        public void connectionCallbackFunction(IAsyncResult i_ConnectionResult)
        {
            BluetoothClient client = i_ConnectionResult.AsyncState as BluetoothClient;
            client.EndConnect(i_ConnectionResult);
        }

        private bool checkWiiMoteConnectionAbility(BluetoothDeviceInfo i_Device)
        {
            if (!i_Device.Authenticated)
            {
                if (!BluetoothSecurity.PairRequest(i_Device.DeviceAddress, String.Empty))
                {
                    char[] chArray = i_Device.DeviceAddress.ToString().ToCharArray();
                    Array.Reverse(chArray);
                    string address = new String(chArray);
                    string pinCode = hexToAscii(address);
                    if (!BluetoothSecurity.PairRequest(i_Device.DeviceAddress, pinCode))
                    {
                        return false;
                    }
                }
            }
            return true;
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
            //try
            //{
            //    m_WiiMoteWrapper.ConnectToWiimote();
            //}
            //catch (Exception i_Exception)
            //{
            //    System.Windows.MessageBox.Show(i_Exception.Message);
            //}
            //finally
            //{
            //}
        }

        private void calibrateButton_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() => { m_Calibrator.CalibrateScreen(m_WiiMoteWrapper); }));
        }
    }
}
