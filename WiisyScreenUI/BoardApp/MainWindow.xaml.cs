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

namespace BoardApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Boolean gridMinimized = false;
        private static MainWindow boardApp = null;
        private static readonly object sr_key = new object();

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

        private MainWindow()
        {
            InitializeComponent();
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

        private void buttonEraser_Click(object sender, RoutedEventArgs e)
        {
            setBoardToScetchable();
            inkCanvasBoard.EditingMode = InkCanvasEditingMode.EraseByStroke;
        }

        private void buttonPencil_Click(object sender, RoutedEventArgs e)
        {
            setBoardToScetchable();
            inkCanvasBoard.EditingMode = InkCanvasEditingMode.Ink;
        }

        private void buttonPointer_Click(object sender, RoutedEventArgs e)
        {
            sliderOpacity.Value = sliderOpacity.Minimum = 0;
            inkCanvasBoard.EditingMode = InkCanvasEditingMode.None;
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
            setBoardToScetchable();
            inkCanvasBoard.EditingMode = InkCanvasEditingMode.GestureOnly;
        }

        private void buttonClearPage_Click(object sender, RoutedEventArgs e)
        {
            inkCanvasBoard.Strokes.Clear();
        }
    }
}
