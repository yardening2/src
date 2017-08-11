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
using System.Windows.Media.Animation;
using System.Windows.Interop;
using MacrosApp;
using winMacros;
using WiisyScreen.WiiMoteControlls;


namespace WiisyScreen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        private WiiMoteToMouseCoverter m_WiimoteToMouse;
        private Point deltaPos = new Point();
        private List<Window> openedWindows = new List<Window>();
        private bool rightToLeft = true;
        private List<ActionBubble> actionBubbles = new List<ActionBubble>();
        private List<Ellipse> locationsEllipes = new List<Ellipse>();

        public MainWindow()
        {
            InitializeComponent();
            //m_Calibrator.CalibrateFinishedEvent += onCalibrationFinished;
            insertLocationEllipseToList();
            addApp(runBoard, createImageForEllipse("whiteboard-icon.png"));
            addApp(runMacroApp, createImageForEllipse("macroicon.png"));
            //actionBubble1.setApp(runBoard, createImageForEllipse("whiteboard-icon.png"));
            //actionBubble2.setApp(runMacroApp, createImageForEllipse("macroicon.png"));
        }

        private void insertLocationEllipseToList()
        {
            locationsEllipes.Add(Ellipse1);
            locationsEllipes.Add(Ellipse2);
            locationsEllipes.Add(Ellipse3);
            locationsEllipes.Add(Ellipse4);
        }
        // c.SetValue(Canvas.LeftProperty, mainAppCanvas.Width - (double)(c.GetValue(Canvas.LeftProperty)) - c.Width);
        //HorizontalAlignment="Right" Height="46" VerticalAlignment="Top" Width="46" Canvas.Left="70" Canvas.Top="46"
        private void addApp(clickedHandler runFunction, ImageBrush imageBrush)
        {
            ActionBubble newActionBubble = new ActionBubble();
            newActionBubble.Visibility = Visibility.Hidden;
            newActionBubble.Width = Ellipse1.Width;
            newActionBubble.Height = Ellipse1.Height;
            newActionBubble.VerticalAlignment = Ellipse1.VerticalAlignment;
            newActionBubble.HorizontalAlignment = Ellipse1.HorizontalAlignment;
            newActionBubble.setApp(runFunction, imageBrush);

            if (actionBubbles.Count < 4)
            {
                mainAppCanvas.Children.Add(newActionBubble);
                newActionBubble.SetValue(Canvas.TopProperty, locationsEllipes[actionBubbles.Count].GetValue(Canvas.TopProperty));
                newActionBubble.SetValue(Canvas.LeftProperty, locationsEllipes[actionBubbles.Count].GetValue(Canvas.LeftProperty));
                newActionBubble.Visibility = Visibility.Visible;
            }

            actionBubbles.Add(newActionBubble);
        }

        private ImageBrush createImageForEllipse(string imageName)
        {
            return new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/WiisyScreen;component/Resources/" + imageName)));
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

            GeneralWinUtils.SetWindowToHideFromAltTab(new WindowInteropHelper(this).Handle);


        }

        private void buttonCalibrate_Click(object sender, RoutedEventArgs e)
        {
            inkCanvasBoard.Opacity = 0;
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            closeOpenedWindows();
            this.Close();
        }

        private void closeOpenedWindows()
        {

            for(int i = openedWindows.Count - 1; i >= 0; i--)
            {
                openedWindows[i].Close();
            }
        }

        private void centerBubble_MouseDown(object sender, MouseButtonEventArgs e)
        {
            centerBubble.CaptureMouse();
            translate.BeginAnimation(TranslateTransform.XProperty, null);
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

            if ((translate.X) + (mainAppCanvas.Width/2) > mainWindow.Width / 2)
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
            double oldXLocation = translate.X;
            double newXLocation = ((translate.X) + (mainAppCanvas.Width / 2) > mainWindow.Width / 2) ? (mainWindow.Width) - mainAppCanvas.ActualWidth : 0;

            translate.X = newXLocation;
            translate.BeginAnimation(TranslateTransform.XProperty, new DoubleAnimation(oldXLocation, newXLocation, TimeSpan.FromSeconds(0.3)));
        }

        private void flipControllers()
        {
            IEnumerable<System.Windows.Controls.Control> controlsCollection = mainAppCanvas.Children.OfType<System.Windows.Controls.Control>();
            IEnumerable<Shape> shapesCollection = mainAppCanvas.Children.OfType<Shape>();

            foreach (System.Windows.Controls.Control c in controlsCollection)
            {
                c.SetValue(Canvas.LeftProperty, mainAppCanvas.Width - (double)(c.GetValue(Canvas.LeftProperty)) - c.Width);
            }
            foreach (Shape s in shapesCollection)
            {
                s.SetValue(Canvas.LeftProperty, mainAppCanvas.Width - (double)(s.GetValue(Canvas.LeftProperty)) - s.Width);
            }

            rightToLeft = !rightToLeft;
        }

        private void removeWindowFromOpenedWindows(object window, EventArgs e)
        {
            openedWindows.Remove(window as Window);
        }

        private void runBoard()
        {
            runApp(TheBoardApp.MainWindow.Instance);

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
