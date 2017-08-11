using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WiisyScreen.WiiMoteControlls;
using winMacros;


namespace WiiMoteConnect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindow wiiMoteConnect = null;
        private static readonly object sr_key = new object();
        private WiiMoteWrapper m_WiiMoteWrapper;
        private Calibrator m_Calibrator;
        private WiiMoteToMouseCoverter m_WiimoteToMouse;

        public WiiMoteToMouseCoverter ToMouseConverter { get { return m_WiimoteToMouse; } }

        public static MainWindow Instance
        {
            get
            {
                if (wiiMoteConnect == null)
                {
                    lock (sr_key)
                    {
                        if (wiiMoteConnect == null)
                        {
                            wiiMoteConnect = new MainWindow();
                        }
                    }
                }

                return wiiMoteConnect;
            }
        }

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

        private void calibrateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m_WiiMoteWrapper.ConnectToWiimote();
                this.Dispatcher.Invoke(new Action(() => { m_Calibrator.CalibrateScreen(m_WiiMoteWrapper); }));
                this.Close();
            }
            catch (Exception i_Exception)
            {
                System.Windows.MessageBox.Show(i_Exception.Message);
            }
            finally
            {
            }
        }

        //private void calibrateButton_Click(object sender, RoutedEventArgs e)
        //{
        //    this.Dispatcher.Invoke(new Action(() => { m_Calibrator.CalibrateScreen(m_WiiMoteWrapper); }));
        //}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Width = desktopWorkingArea.Width;
            this.Height = desktopWorkingArea.Height;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;

            GeneralWinUtils.SetWindowToHideFromAltTab(new WindowInteropHelper(this).Handle);
        }

        private void rectangleExit_MouseUp(object sender, MouseButtonEventArgs e)
        {
            wiiMoteConnect = null;
            this.Close();
        }
    }
}
