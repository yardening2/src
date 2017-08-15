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
using WiiMoteConnect.WiiMoteControlls;
using System.Diagnostics;
using System.IO;

namespace WiisyScreen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        //private WiiMoteToMouseCoverter m_WiimoteToMouse;
        private Point deltaPos = new Point();
        private static List<Window> openedWindows = new List<Window>();
        private bool rightToLeft = true;
        public static double k_bubbleDiamater = 46;
        public static int k_bubbelsAnimation = 7;


        public MainWindow()
        {
            InitializeComponent();
            addApp(runBoard, createImageForEllipse("whiteboard-icon.png"));
            addApp(runMacroApp, createImageForEllipse("macroicon.png"));
            initBubbelsAnimation();
            //actionBubble1.setApp(runBoard, createImageForEllipse("whiteboard-icon.png"));
            //actionBubble2.setApp(runMacroApp, createImageForEllipse("macroicon.png"));
            initFirstUse();
        }

        private void initFirstUse()
        {
            actionBubbleSlot1.setApp(runBoard, createImageForEllipse("whiteboard-icon.png"));
            actionBubbleSlot2.setApp(runMacroApp, createImageForEllipse("macroicon.png"));
            actionBubbleSlot1.Opacity = actionBubbleSlot2.Opacity = 1;
        }

        private void initBubbelsAnimation()
        {
            actionBubbleSlot1.InitAnimation(k_bubbelsAnimation);
            actionBubbleSlot2.InitAnimation(k_bubbelsAnimation);
            actionBubbleSlot3.InitAnimation(k_bubbelsAnimation);
            actionBubbleSlot4.InitAnimation(k_bubbelsAnimation);
        }

        private void addApp(clickedHandler runFunction, ImageBrush imageBrush)
        {
            ActionBubble newActionBubble = new ActionBubble();
            newActionBubble.Visibility = Visibility.Hidden;
            newActionBubble.Width = k_bubbleDiamater;
            newActionBubble.Height = k_bubbleDiamater;
            newActionBubble.InitAnimation(7);
            newActionBubble.setApp(runFunction, imageBrush);

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

        private static void removeWindowFromOpenedWindows(object window, EventArgs e)
        {
            openedWindows.Remove(window as Window);
        }

        public static void runBoard()
        {
            runApp(TheBoardApp.MainWindow.Instance);

        }

        public static void runMacroApp()
        {
            runApp(MacrosApp.MainWindow.Instance);
        }

        private static void runApp(Window windowApp)
        {
            if (!openedWindows.Exists(window => window == windowApp))
            {
                openedWindows.Add(windowApp);
                windowApp.Closed += new EventHandler(removeWindowFromOpenedWindows);
                windowApp.Show();
            }
        }

        private void rectangleDrag_MouseDown(object sender, MouseButtonEventArgs e)
        {
            shapeDragCanvasMouseDown(sender as Shape, e, translateSettingsCanvas);
        }

        private void rectangleDrag_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            shapeDragCanvasMouseMove(sender as Shape, e, translateSettingsCanvas);
        }

        private void rectangleDrag_MouseUp(object sender, MouseButtonEventArgs e)
        {
            shapeDragCanvasMouseUp(sender as Shape);
        }

        private void shapeDragCanvasMouseDown(Shape s, MouseButtonEventArgs e, TranslateTransform t)
        {
            s.CaptureMouse();
            deltaPos.X = e.GetPosition(container).X - t.X;
            deltaPos.Y = e.GetPosition(container).Y - t.Y;
            e.Handled = true;
        }

        private void shapeDragCanvasMouseMove(Shape s, System.Windows.Input.MouseEventArgs e, TranslateTransform t)
        {
            if (s.IsMouseCaptured)
            {
               t.X = e.GetPosition(container).X - deltaPos.X;
                t.Y = e.GetPosition(container).Y - deltaPos.Y;
            }
        }

        private void shapeDragCanvasMouseUp(Shape s)
        {
            s.ReleaseMouseCapture();
        }

        private void buttonSetting_Click(object sender, RoutedEventArgs e)
        {
            if ((translate.X) > mainWindow.Width / 2)
            {
                translateSettingsCanvas.X = translate.X - canvasSettings.Width - 50;
            }
            else
            {
                translateSettingsCanvas.X = translate.X + mainAppCanvas.Width + 50;
            }

            translateSettingsCanvas.Y = translate.Y;

            canvasSettings.Visibility = Visibility.Visible;
        }

        private void buttonExitSettings_Click(object sender, RoutedEventArgs e)
        {
            canvasSettings.Visibility = Visibility.Hidden;
        }

       private void addCustomizeActionBubble()
        {
            ActionBubble ab = new ActionBubble();
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".exe";
            dlg.Filter = "exe Files (*.exe)|*.exe";

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                ImageBrush theIcon = utils.ImageBrushFromIconConverter.createImageBrushFromIcon(GeneralWinUtils.GetLargeIcon(dlg.FileName));
                addApp(() => { Process.Start(dlg.FileName); }, theIcon);
            }

        }


    }
}
