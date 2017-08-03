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

namespace MacrosApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
            Close();
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
            Macros.LastWindowShow(Macros.ShowWindowCommands.ShowMaximized);
        }

        private void buttonMacroClose_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Macros.CloseLastWindow();
        }

        private void buttonMacroMinimize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
