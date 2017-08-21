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
using winMacros;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Windows.Media.Animation;

namespace MacrosApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindow macroApp = null;
        private static readonly object sr_key = new object();

        public static MainWindow Instance
        {
            get
            {
                if (macroApp == null)
                {
                    lock (sr_key)
                    {
                        if (macroApp == null)
                        {
                            macroApp = new MainWindow();
                        }
                    }
                }

                return macroApp;
            }
        }

        public enum eOriention
        {
            vertical = -90,
            horizontal = 0
        }

        private eOriention shiftOriention = eOriention.horizontal;

        public MainWindow()
        {
            InitializeComponent();
        }



        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            macroApp = null;
            this.Close();
        }

        private void buttonChangeOrientation_Click(object sender, RoutedEventArgs e)
        {
            shiftOriention = shiftOriention == eOriention.vertical ? eOriention.horizontal : eOriention.vertical;
            RotateTransform rt = new RotateTransform((int)shiftOriention);
            shiftButtonsContainer.RenderTransform = rt;
        }

        private void buttonShift_Click(object sender, RoutedEventArgs e)
        {
            int width = shiftOriention == eOriention.horizontal ? (int)SystemParameters.WorkArea.Width : (int)SystemParameters.WorkArea.Width / 2;
            int height = shiftOriention == eOriention.horizontal ? (int)SystemParameters.WorkArea.Height / 2 : (int)SystemParameters.WorkArea.Height;
            int xSlot = 0;
            int ySlot = 0;

            if((sender as Button).Equals(buttonShift2))
            {
                if(shiftOriention == eOriention.horizontal)
                {
                    ySlot = 1;
                }
                else
                {
                    xSlot = 1;
                }
            }

            Topmost = false;
            Macros.ShiftLastWindow(width, height, xSlot, ySlot);
            Topmost = true;
        }

        
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        
        

        private void buttonMacroMinimize_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Topmost = false;
            Macros.LastWindowShow(Macros.ShowWindowCommands.Minimize);
            Topmost = true;
        }

        private void buttonMacroMaximize_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Topmost = false;
            Macros.LastWindowShow(Macros.ShowWindowCommands.ShowMaximized);
            Topmost = true;
        }

        private void buttonMacroClose_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Topmost = false;
            Macros.CloseLastWindow();
            Topmost = true;
        }

        private void mouseDownHandle(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void Rectangle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Macros.WindowsScreen();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;

            GeneralWinUtils.SetWindowToHideFromAltTab(new WindowInteropHelper(this).Handle);
        }

    }
}
