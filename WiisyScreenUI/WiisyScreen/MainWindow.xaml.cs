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
using MacrosApp;


namespace WiisyScreen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WiiMoteWrapper m_WiiMoteWrapper;
        private Calibrator m_Calibrator;
        private Point deltaPos = new Point();
        private List<Window> openedWindows = new List<Window>();
        private bool rightToLeft = true;

        public MainWindow()
        {
            InitializeComponent();
            m_WiiMoteWrapper = new WiiMoteWrapper();
            m_Calibrator = new Calibrator(m_WiiMoteWrapper);
            m_Calibrator.CalibrateFinishedEvent += onCalibrationFinished;
            actionBubble1.setApp(runBoard, createImageForEllipse("whiteboard-icon.png"));
            actionBubble2.setApp(runMacroApp, createImageForEllipse("macroicon.png"));
        }

        private ImageBrush createImageForEllipse(string imageName)
        {
            return new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/WiisyScreen;component/Resources/" + imageName)));
        }

        private void onCalibrationFinished(object i_Sender, EventArgs i_EventArgs)
        {
            m_WiimoteToMouse = new WiiMoteToMouseCoverter(m_Calibrator.getCalibratedWarper(), m_WiiMoteWrapper);
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
            gridCalibrate.Visibility = Visibility.Visible;
            inkCanvasBoard.Opacity = 0;
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            closeOpenedWindows();
            this.Close();
        }

        private void closeOpenedWindows()
        {
            Window prevWindow = null;

            foreach (Window window in openedWindows)
            {
                if (prevWindow != null)
                    prevWindow.Close();
                prevWindow = window;
            }
            if (prevWindow != null)
            {
                prevWindow.Close();
            }
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

            if ((translate.X) > mainWindow.Width / 2)
            {
                if (rightToLeft)
                {
                    flipControllers();
                }
            }
            else
            {
                if (!rightToLeft)
                {
                    flipControllers();
                }
            }
        }

        private void centerBubble_MouseUp(object sender, MouseButtonEventArgs e)
        {
            centerBubble.ReleaseMouseCapture();
            if((translate.X) > mainWindow.Width / 2)
            {
                translate.X = (mainWindow.Width) - mainAppCanvas.ActualWidth;
            }
            else
            {
                translate.X = 0;
            }
        }

        private void flipControllers()
        {
            IEnumerable<System.Windows.Controls.Control> collection = mainAppCanvas.Children.OfType<System.Windows.Controls.Control>();

            foreach (System.Windows.Controls.Control c in collection)
            {
                c.SetValue(Canvas.LeftProperty, mainAppCanvas.Width - (double)(c.GetValue(Canvas.LeftProperty)) - c.Width); //mainAppCanvas.ActualWidth - 
            }
            centerBubble.SetValue(Canvas.LeftProperty, mainAppCanvas.Width - (double)(centerBubble.GetValue(Canvas.LeftProperty)) - centerBubble.Width);

            rightToLeft = !rightToLeft;
        }

        private void removeWindowFromOpenedWindows(object window, EventArgs e)
        {
            openedWindows.Remove(window as Window);
        }

        private void runBoard()
        {
            runApp(BoardApp.BoardAppWindow.Instance);   
        }

        private void runMacroApp()
        {
            runApp(MacrosApp.MainWindow.Instance);
        }

        private void runApp(Window windowApp)
        {
            if (!openedWindows.Exists(window => window == windowApp))
            {
                openedWindows.Add(windowApp);
                windowApp.Closed += new EventHandler(removeWindowFromOpenedWindows);
                windowApp.Show();
            }
        }
    }
}
