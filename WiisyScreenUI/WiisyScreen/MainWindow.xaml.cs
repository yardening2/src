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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Threading;
using WiisyScreen.WiiMoteControlls;
using winMacros;


namespace WiisyScreen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WiiMoteWrapper m_WiiMoteWrapper;
        private Calibrator m_Calibrator;
        private Point deltaPos= new Point();

        public MainWindow()
        {
            InitializeComponent();
            m_WiiMoteWrapper = new WiiMoteWrapper();
            m_Calibrator = new Calibrator(m_WiiMoteWrapper);
            m_Calibrator.CalibrateFinishedEvent += onCalibrationFinished;
        }

        private void onCalibrationFinished(object i_Sender, EventArgs i_EventArgs)
        {
            WiiMoteToMouseCoverter m = new WiiMoteToMouseCoverter(m_Calibrator.getCalibratedWarper(), m_WiiMoteWrapper);
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            this.Topmost = true;
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            this.Topmost = true;
            this.Activate();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Width = desktopWorkingArea.Width;
            this.Height = desktopWorkingArea.Height;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;
            
        }

        private void buttonCalibrate_Click(object sender, RoutedEventArgs e)
        {
            gridBoard.Visibility = gridMacros.Visibility = Visibility.Hidden;
            gridCalibrate.Visibility = Visibility.Visible;
            inkCanvasBoard.Opacity = 0;
        }

        private void buttonMacros_Click(object sender, RoutedEventArgs e)
        {
            gridBoard.Visibility = gridCalibrate.Visibility = Visibility.Hidden;
            gridMacros.Visibility = Visibility.Visible;
            inkCanvasBoard.Opacity = 0;
        }

        private void buttonBoard_Click(object sender, RoutedEventArgs e)
        {
            gridMacros.Visibility = gridCalibrate.Visibility = Visibility.Hidden;
            gridBoard.Visibility = Visibility.Visible;
            inkCanvasBoard.Opacity = 1;
        }

        private string chooseFolder()
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    return fbd.SelectedPath;
                }
            }

            return null;
        }

        private void ButtonSaveScreen_Click(object sender, RoutedEventArgs e)
        {
            System.IO.Directory.CreateDirectory("tmp");
            string dirToSaveTo = "tmp\\";
            /*
            string dirToSaveTo = chooseFolder();
>>>>>>> adding screenshots
            System.Threading.Thread.Sleep(250);
            if (dirToSaveTo != null)
            {
                ScreenSaver.ScreenSaver.SaveAsImage(dirToSaveTo);
            }
            */
        }

        private void buttonGeneratePDF_Click(object sender, RoutedEventArgs e)
        {
            /*
            Microsoft.Win32.OpenFileDialog dlgImagesToPDF = new Microsoft.Win32.OpenFileDialog();
            
            dlgImagesToPDF.DefaultExt = ".png";
            dlgImagesToPDF.Filter = "PNG Files (*.png)|*.png";
            dlgImagesToPDF.Multiselect = true;

            Nullable<bool> result = dlgImagesToPDF.ShowDialog();

            if (result == true)
            {
                // Open document 
                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.DefaultExt = ".pdf";
                saveFileDialog.Filter = "PDF File (*.pdf)|*.pdf";
                Nullable<bool> saveResult = saveFileDialog.ShowDialog();
                if (result == true)
                {
                    ScreenSaver.ScreenSaver.CreatePDF(saveFileDialog.FileName, dlgImagesToPDF.FileNames);
                }
            }
            */
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonWindows_Click(object sender, RoutedEventArgs e)
        {
            Macros.WindowsScreen();
        }

        private void buttonDesktop_Click(object sender, RoutedEventArgs e)
        {
            Macros.Desktop();
            //todo
            //show this window
        }

        private void buttonTskmgr_Click(object sender, RoutedEventArgs e)
        {
            Macros.Taskmgr();
        }

        private void buttonKeyboard_Click(object sender, RoutedEventArgs e)
        {
            new Thread(Macros.osk).Start();
        }

        private void buttonMaximaze_Click(object sender, RoutedEventArgs e)
        {
            Topmost = false;
            //Macros.LastWindowShow(Macros.ShowWindowCommands.ShowMaximized);
            Topmost = true;
        }

        private void buttonShift0_Click(object sender, RoutedEventArgs e)
        {
            Topmost = false;
            //Macros.ShiftLastWindow(0);
            Topmost = true;
        }

        private void buttonShift1_Click(object sender, RoutedEventArgs e)
        {
            Topmost = false;
            //Macros.ShiftLastWindow(1);
            Topmost = true;

        }

        private void buttonShift2_Click(object sender, RoutedEventArgs e)
        {
            Topmost = false;
            //Macros.ShiftLastWindow(2);
            Topmost = true;
        }

        private void ButtonConnectToWiiMote_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ButtonConnectToWiiMote.IsEnabled = false;
                ConnectingToWiiMoteLabel.Visibility = Visibility.Visible;
                this.IsEnabled = false;
                m_WiiMoteWrapper.ConnectToWiimote();
                updateComponents();
            }
            catch (Exception i_Exception)
            {
                System.Windows.MessageBox.Show(i_Exception.Message);
            }
            finally
            {
                ButtonConnectToWiiMote.IsEnabled = true;
                ConnectingToWiiMoteLabel.Visibility = Visibility.Hidden;
                this.IsEnabled = true;
            }
        }

        private void updateComponents()
        {
            ButtonConnectToWiiMote.Visibility = Visibility.Hidden;
            ButtonCalibrateWiiMote.Visibility = Visibility.Visible;
            BatteryLevelTextLabel.Visibility = Visibility.Visible;
            BatteryLevelValueLabel.Visibility = Visibility.Visible;
            VisibleIRDotsLabel.Visibility = Visibility.Visible;
            IRDotsDataLabel.Visibility = Visibility.Visible;
            m_WiiMoteWrapper.VisibleIRDotsChangedEvent += updateIRDotsCount;
            m_WiiMoteWrapper.AButtonPressed += ButtonCalibrateWiiMote_Click;
            BatteryLevelValueLabel.Content = m_WiiMoteWrapper.BatteryLevel.ToString();

        }

        private void ButtonCalibrateWiiMote_Click(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() => { m_Calibrator.CalibrateScreen(m_WiiMoteWrapper); }));
        }

        private void updateIRDotsCount(object i_Sender, int i_VisibleIRDots)
        {
            IRDotsDataLabel.Dispatcher.Invoke(new Action(() => { IRDotsDataLabel.Content = i_VisibleIRDots.ToString(); }));
        }
      

        private void centerBubble_MouseDown(object sender, MouseButtonEventArgs e)
        {
            centerBubble.CaptureMouse();
            deltaPos.X = e.GetPosition(container).X - translate.X;
            deltaPos.Y = e.GetPosition(container).Y - translate.Y;
            e.Handled = true;
        }

        private void centerBubble_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (centerBubble.IsMouseCaptured)
            {
                translate.X = e.GetPosition(container).X - deltaPos.X;
                translate.Y = e.GetPosition(container).Y - deltaPos.Y;
            }
        }

        private void centerBubble_MouseUp(object sender, MouseButtonEventArgs e)
        {
            centerBubble.ReleaseMouseCapture();
        }

        private void actionBubble1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            BoardApp.MainWindow.Instance.Show();
        }

    }

}
