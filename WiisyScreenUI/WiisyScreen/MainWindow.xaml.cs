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
        public static WiiMoteToMouseCoverter WiimoteToMouse { get; set; }
        private Point deltaPos = new Point();
        private static List<Window> openedWindows = new List<Window>();
        private bool rightToLeft = true;
        public static readonly double k_bubbleDiamater = 46;
        public static readonly int k_bubbelsAnimation = 7;
        public static readonly string k_savedDataFile = "savedData";

        public MainWindow()
        {
            InitializeComponent();
            initBubbelsAnimation();

            //initFirstUse();
            initApps();
        }

        private void initApps()
        {
            if (File.Exists(k_savedDataFile) == false)
            {
                initFirstUse();
                //settings first use???
            }
            else
            {
                WiisyScreenSavedData savedData = WiisyScreenUIHelper.ReadFromBinaryFile<WiisyScreenSavedData>(k_savedDataFile);
                initMainBubbels(savedData.MainBubbels);
                settings.initRepository(savedData.RepositoryData);
            }
        }

        private void initMainBubbels(List<ActionBubble.ActionBubbleData> mainBubbels)
        {
            //to do in a loop
            actionBubbleSlot1.Copy(WiisyScreenUIHelper.CreateActionBubbleFromData(mainBubbels[0]));
            actionBubbleSlot2.Copy(WiisyScreenUIHelper.CreateActionBubbleFromData(mainBubbels[1]));
            actionBubbleSlot3.Copy(WiisyScreenUIHelper.CreateActionBubbleFromData(mainBubbels[2]));
            actionBubbleSlot4.Copy(WiisyScreenUIHelper.CreateActionBubbleFromData(mainBubbels[3]));

        }

        private void initFirstUse()
        {
            actionBubbleSlot1.setApp(runBoard, WiisyScreenUIHelper.createImageForEllipse("whiteboard-icon.png"));
            actionBubbleSlot1.BubbleData = new ActionBubble.ActionBubbleData("BoardApp", eBubbleType.Board);
            actionBubbleSlot2.setApp(runMacroApp, WiisyScreenUIHelper.createImageForEllipse("macroicon.png"));
            actionBubbleSlot2.BubbleData = new ActionBubble.ActionBubbleData("MacroApp", eBubbleType.Macro);
            actionBubbleSlot1.Opacity = actionBubbleSlot2.Opacity = 1;
        }

        private void initBubbelsAnimation()
        {
            actionBubbleSlot1.InitAnimation(k_bubbelsAnimation);
            actionBubbleSlot2.InitAnimation(k_bubbelsAnimation);
            actionBubbleSlot3.InitAnimation(k_bubbelsAnimation);
            actionBubbleSlot4.InitAnimation(k_bubbelsAnimation);
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

            for (int i = openedWindows.Count - 1; i >= 0; i--)
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

            if ((translate.X) + (mainAppCanvas.Width / 2) > mainWindow.Width / 2)
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
            canvasSettings.BeginAnimation(OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5)));
        }

        private void buttonExitSettings_Click(object sender, RoutedEventArgs e)
        {
            canvasSettings.Visibility = Visibility.Hidden;
        }

        public List<ActionBubble.ActionBubbleData> GetMainBubbelsData()
        {
            List<ActionBubble.ActionBubbleData> theData = new List<ActionBubble.ActionBubbleData>();
            theData.Add(actionBubbleSlot1.BubbleData);
            theData.Add(actionBubbleSlot2.BubbleData);
            theData.Add(actionBubbleSlot3.BubbleData);
            theData.Add(actionBubbleSlot4.BubbleData);

            return theData;
        }

        private void mainWindow_Closed(object sender, EventArgs e)
        {
            hundleSaveData();
            settings.StopWiimoteWrapper();
        }

        private void hundleSaveData()
        {
            File.Delete(k_savedDataFile);
;           if (settings.ToSaveData())
            {
                WiisyScreenSavedData dataToSave = new WiisyScreenSavedData();
                dataToSave.MainBubbels = GetMainBubbelsData();
                dataToSave.RepositoryData = settings.GetRepository();
                WiisyScreenUIHelper.WriteToBinaryFile<WiisyScreenSavedData>(k_savedDataFile, dataToSave);
            }
        }
    }
}
