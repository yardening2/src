﻿using System;
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
using ScreenSaver;
using System.Threading;
using System.Windows.Interop;
using winMacros;
using System.Windows.Media.Animation;

namespace BoardApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class BoardAppWindow : Window
    {
        private Boolean gridMinimized = false;
        private static BoardAppWindow boardApp = null;
        private static readonly object sr_key = new object();
        public string ScreenShotsTempFolder { get; set; }

        public static BoardAppWindow Instance
        {
            get
            {
                if (boardApp == null)
                {
                    lock (sr_key)
                    {
                        if (boardApp == null)
                        {
                            boardApp = new BoardAppWindow();
                        }
                    }
                }

                return boardApp;
            }
        } 

        public BoardAppWindow()
        {
            InitializeComponent();
            ScreenShotsTempFolder = "tmp";
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

        private void buttonEraser_Click(object sender, RoutedEventArgs e)
        {
            markClickedModeButton(sender as Rectangle);
            setBoardToScetchable();
            inkCanvasBoard.EditingMode = InkCanvasEditingMode.EraseByStroke;
            displayNotification("Board Mode: Eraser");
        }

        private void buttonPointer_Click(object sender, RoutedEventArgs e)
        {
            markClickedModeButton(sender as Rectangle);
            sliderOpacity.Value = sliderOpacity.Minimum = 0;
            inkCanvasBoard.EditingMode = InkCanvasEditingMode.None;
            displayNotification("Board Mode: None");
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

        private void buttonMinimize_Click(object sender, RoutedEventArgs e)
        {
            if (gridMinimized)
            {
                gridBoardToolBar.Height = 100;
                gridMinimized = false;
            }
            else
            {
                gridBoardToolBar.Height = 20;
                gridMinimized = true;
            }
        }

        private void buttonLaser_Click(object sender, RoutedEventArgs e)
        {
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

            System.IO.Directory.CreateDirectory(ScreenShotsTempFolder);
            System.Threading.Thread.Sleep(250);
            if (ScreenShotsTempFolder != null)
            {
                theSavedPic = ScreenSaver.ScreenSaver.SaveAsImage(ScreenShotsTempFolder);
                addPicToScrollPanel(theSavedPic);
            }
            displayNotification("Screenshot saved");
        }

        private void addPicToScrollPanel(string theSavedPic)
        {
            Image savedPicImage = new Image();
            savedPicImage.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath(theSavedPic)));
            savedPicImage.Width = savedPicImage.Height = 150;
            savedPicImage.HorizontalAlignment = HorizontalAlignment.Left;
            savedPicImage.Margin = new Thickness(30);
            savedPicImage.MouseUp += new MouseButtonEventHandler(setDisplayImage);
            savedPicsPanel.Children.Add(savedPicImage);
        }

        
        private void setDisplayImage(object sender, MouseButtonEventArgs r)
        {
            imageCanvas.Background = new ImageBrush((sender as Image).Source);
            imageCanvas.Visibility = Visibility.Visible;
        }

        private void buttonScreenshotFiles_MouseUp(object sender, MouseButtonEventArgs e)
        {
            gridBoardToolBar.Height = 0;
            canvasScreenshotWrapper.Height = 100;
            inkCanvasBoard.Visibility = Visibility.Hidden;
        }

        private void buttonReturnFromScreenshots_MouseUp(object sender, MouseButtonEventArgs e)
        {
            canvasScreenshotWrapper.Height = 0;
            imageCanvas.Background = null;
            gridBoardToolBar.Height = 100;
            inkCanvasBoard.Visibility = Visibility.Visible;
        }

        private void buttonPencil_Click(object sender, MouseButtonEventArgs e)
        {
            markClickedModeButton(sender as Rectangle);
            setBoardToScetchable();
            inkCanvasBoard.EditingMode = InkCanvasEditingMode.Ink;
            displayNotification("Board Mode: Pencil");
        }

        private void displayNotification(string i_msg)
        {
            notificationLabel.Content = i_msg;
            notificationLabel.BeginAnimation(OpacityProperty, new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(1.8))));
        }

        private void changeColor_MouseUp(object sender, MouseButtonEventArgs e)
        {
            switch ((sender as Rectangle).Name)
            {
                case "buttonRed":
                    inkCanvasBoard.DefaultDrawingAttributes.Color = Colors.Red;
                    break;
                case "buttonBlue":
                    inkCanvasBoard.DefaultDrawingAttributes.Color = Colors.Blue;
                    break;
                case "buttonGreen":
                    inkCanvasBoard.DefaultDrawingAttributes.Color = Colors.Green;
                    break;
                default:
                    inkCanvasBoard.DefaultDrawingAttributes.Color = Colors.Black;
                    break;
            }
        }
    }
}
