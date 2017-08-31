using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using winMacros;

namespace TheBoardApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Boolean gridMinimized = false;
        private static MainWindow boardApp = null;
        private static readonly object sr_key = new object();
        static public string ScreenShotsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        private int markerSize = 1;
        private DispatcherTimer timerCommandGrid = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
        private bool isPinned = false;


        public static MainWindow Instance
        {
            get
            {
                if (boardApp == null)
                {
                    lock (sr_key)
                    {
                        if (boardApp == null)
                        {
                            boardApp = new MainWindow();
                        }
                    }
                }

                return boardApp;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            initCommandTimer();
        }

        private void initCommandTimer()
        {
            timerCommandGrid.Interval = TimeSpan.FromSeconds(6);
            timerCommandGrid.Tick += (sender, args) => autoScroll();
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
            timerCommandGrid.Start();

        }

        private void buttonEraser_Click(object sender, RoutedEventArgs e)
        {
            hideColorsChooser();
            markClickedModeButton(sender as Rectangle);
            setBoardToScetchable();
            inkCanvasBoard.EditingMode = InkCanvasEditingMode.EraseByStroke;
            displayNotification("Board Mode: Eraser");
        }

        private void buttonPointer_Click(object sender, RoutedEventArgs e)
        {
            hideColorsChooser();
            markClickedModeButton(sender as Rectangle);
            sliderOpacity.Value = sliderOpacity.Minimum = 0;
            inkCanvasBoard.EditingMode = InkCanvasEditingMode.None;
            displayNotification("Board Mode: None");
        }

        private void hideColorsChooser()
        {
            colorsPanel.Opacity = 0;
            colorsPanel.IsEnabled = false;
            colorsPanel.BeginAnimation(OpacityProperty, new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(0.8))));
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            boardApp = null;
            this.Close();
        }

        private void setBoardToScetchable()
        {
            sliderOpacity.Minimum = 0.01;
        }

        private void buttonLaser_Click(object sender, RoutedEventArgs e)
        {
            showColorsChooser();
            markClickedModeButton(sender as Rectangle);
            setBoardToScetchable();
            inkCanvasBoard.EditingMode = InkCanvasEditingMode.GestureOnly;
            displayNotification("Board Mode: Laser");
        }

        private void buttonClearPage_Click(object sender, RoutedEventArgs e)
        {
            inkCanvasBoard.Strokes.Clear();
            displayNotification("Screen Cleard");

        }

        private void markClickedModeButton(Rectangle sender)
        {
            removeStrokeFromModeIcons();
            sender.Stroke.Opacity = 100;
        }

        private void removeStrokeFromModeIcons()
        {
            buttonEraser.Stroke.Opacity = 0;
            buttonLaser.Stroke.Opacity = 0;
            buttonPencil.Stroke.Opacity = 0;
            buttonPointer.Stroke.Opacity = 0;
        }

        private void buttonSaveScreen_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string theSavedPic;
            
            System.Threading.Thread.Sleep(250);
            if (ScreenShotsFolder != null)
            {
                theSavedPic = ScreenSaver.ScreenSaver.SaveAsImage(MainWindow.ScreenShotsFolder);
                animatePicTaken(addPicToScrollPanel(theSavedPic));
                updatePicsAmount();
            }
            displayNotification("Screenshot saved");
        }

        private void updatePicsAmount()
        {
            string picsAmount = savedPicsPanel.Children.Count <= 50 ? savedPicsPanel.Children.Count.ToString() : "50+";

            textboxPicsAmount.BeginAnimation(OpacityProperty, null);
            wraperPicsAmount.Opacity = 1;
            textboxPicsAmount.Text = picsAmount;
            textboxPicsAmount.BeginAnimation(OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1.7)));
        }

        private void animatePicTaken(Image i_Pic)
        {
            animateCanvas.Background = new ImageBrush(i_Pic.Source);
            animateCanvas.BeginAnimation(OpacityProperty, new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(0.5))));
            animateCanvasST.BeginAnimation(ScaleTransform.ScaleXProperty, new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(0.5))));
            animateCanvasST.BeginAnimation(ScaleTransform.ScaleYProperty, new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(0.5))));
        }

        private Image addPicToScrollPanel(string theSavedPic)
        {
            Image savedPicImage = new Image();
            savedPicImage.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath(theSavedPic)));
            savedPicImage.HorizontalAlignment = HorizontalAlignment.Left;
            savedPicImage.Margin = new Thickness(8);
            savedPicImage.MouseUp += new MouseButtonEventHandler(setDisplayImage);
            savedPicsPanel.Children.Add(savedPicImage);
            return savedPicImage;
        }


        private void setDisplayImage(object sender, MouseButtonEventArgs r)
        {
            imageCanvas.Background = new ImageBrush((sender as Image).Source);
            imageCanvas.Visibility = Visibility.Visible;
        }

        private void buttonScreenshotFiles_MouseUp(object sender, MouseButtonEventArgs e)
        {
            gridBoardToolBar.BeginAnimation(HeightProperty, null);
            gridBoardToolBar.Height = 0;
            canvasScreenshotWrapper.Height = 100;
            inkCanvasBoard.Visibility = Visibility.Hidden;
            timerCommandGrid.Stop();
        }

        private void buttonReturnFromScreenshots_MouseUp(object sender, MouseButtonEventArgs e)
        {
            canvasScreenshotWrapper.Height = 0;
            imageCanvas.Background = null;
            gridBoardToolBar.Height = 100;
            inkCanvasBoard.Visibility = Visibility.Visible;
            timerCommandGrid.Start();
        }

        private void buttonPencil_Click(object sender, MouseButtonEventArgs e)
        {
            showColorsChooser();
            markClickedModeButton(sender as Rectangle);
            setBoardToScetchable();
            inkCanvasBoard.EditingMode = InkCanvasEditingMode.Ink;
            displayNotification("Board Mode: Pencil");
        }

        private void showColorsChooser()
        {
            colorsPanel.Opacity = 1;
            colorsPanel.IsEnabled = true;
            colorsPanel.BeginAnimation(OpacityProperty, new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(0.8))));
        }

        private void displayNotification(string i_msg)
        {
            notificationLabel.Content = i_msg;
            notificationLabel.BeginAnimation(OpacityProperty, new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(1.8))));
        }

        private void changeColor_MouseUp(object sender, MouseButtonEventArgs e)
        {
            switch ((sender as Ellipse).Name)
            {
                case "redBucketEllipse":
                    inkCanvasBoard.DefaultDrawingAttributes.Color = Colors.Red;
                    buttonPencil.Fill = getImgFromResource("RedMarker.png");
                    buttonLaser.Fill = getImgFromResource("laserRed.png");
                    break;
                case "blueBucketEllipse":
                    inkCanvasBoard.DefaultDrawingAttributes.Color = Colors.Blue;
                    buttonPencil.Fill = getImgFromResource("BlueMarker.png");
                    buttonLaser.Fill = getImgFromResource("laserBlue.png");
                    break;
                case "greenBucketEllipse":
                    inkCanvasBoard.DefaultDrawingAttributes.Color = Colors.Green;
                    buttonPencil.Fill = getImgFromResource("GreenMarker.png");
                    buttonLaser.Fill = getImgFromResource("laserGreen.png");
                    break;
                default:
                    inkCanvasBoard.DefaultDrawingAttributes.Color = Colors.Black;
                    buttonPencil.Fill = getImgFromResource("BlackMarker.png");
                    buttonLaser.Fill = getImgFromResource("laserBlack.png");
                    break;
            }
        }

        private ImageBrush getImgFromResource(string i_imgName)
        {
            return new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/TheBoardApp;component/Resources/" + i_imgName)));
        }


        private void fontSizeUpButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (inkCanvasBoard.DefaultDrawingAttributes.Width < 50 )
            {
                inkCanvasBoard.DefaultDrawingAttributes.Width = inkCanvasBoard.DefaultDrawingAttributes.Height += 5;
                markerSize++;
            }
            displayNotification("Marker Size: " + markerSize);
        }

        private void fontSizeDownButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (inkCanvasBoard.DefaultDrawingAttributes.Width > 5)
            {
                inkCanvasBoard.DefaultDrawingAttributes.Width = inkCanvasBoard.DefaultDrawingAttributes.Height -= 5;
                markerSize--;
            }
            displayNotification("Marker Size: " + markerSize);
        }

        private async void inkCanvasBoard_Gesture(object sender, InkCanvasGestureEventArgs e)
        {
            inkCanvasBoard.Strokes.Add(e.Strokes);
            await Task.Delay(TimeSpan.FromSeconds(1));
            inkCanvasBoard.Strokes.Remove(e.Strokes);
        }

        private void gridBoardToolBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (gridMinimized == false)
            {
                timerCommandGrid.Stop();
                timerCommandGrid.Start();
            }
        }

        private void toggleCommandGrid()
        {
            if (gridMinimized)
            {
                //gridBoardToolBar.Height = 100;
                gridBoardToolBar.BeginAnimation(HeightProperty, new DoubleAnimation(100, TimeSpan.FromSeconds(0.18)));
                gridMinimized = false;
                timerCommandGrid.Start();
            }
            else
            {
                //gridBoardToolBar.Height = 20;
                gridBoardToolBar.BeginAnimation(HeightProperty, new DoubleAnimation(20, TimeSpan.FromSeconds(0.18)));
                gridMinimized = true;
                timerCommandGrid.Stop();
            }
        }

        private void buttonMinimize_Click(object sender, RoutedEventArgs e)
        {
            toggleCommandGrid();
        }

        private void buttonSaveScreen_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DoubleAnimation clickAnimation = new DoubleAnimation(1, 0.3, TimeSpan.FromSeconds(0.4));
            clickAnimation.AutoReverse = true;
            buttonSaveScreen.BeginAnimation(OpacityProperty, clickAnimation);
        }

        private void rectanglePin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isPinned = !isPinned;
            rectanglePin.Fill = isPinned ? getImgFromResource("pinned.png") : getImgFromResource("unpinned.png");
        }

        private void autoScroll()
        {
            if (!isPinned)
            {
                toggleCommandGrid();
            }
        }
    }
}
