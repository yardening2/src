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
using ScreenSaver;
using winMacros;
using System.Threading;


namespace WiisyScreen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            //this.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
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
            string dirToSaveTo = chooseFolder();
            System.Threading.Thread.Sleep(250);
            if (dirToSaveTo != null)
            {
                ScreenSaver.ScreenSaver.SaveAsImage(dirToSaveTo);
            }
        }

        private void buttonGeneratePDF_Click(object sender, RoutedEventArgs e)
        {
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
        }

        private void buttonEraser_Click(object sender, RoutedEventArgs e)
        {
            inkCanvasBoard.EditingMode = InkCanvasEditingMode.EraseByStroke;
        }

        private void buttonPencil_Click(object sender, RoutedEventArgs e)
        {
            inkCanvasBoard.EditingMode = InkCanvasEditingMode.Ink;
        }

        private void button_Click(object sender, RoutedEventArgs e)
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
            Macros.LastWindowShow(Macros.ShowWindowCommands.ShowMaximized);
            Topmost = true;
        }

        private void buttonShift0_Click(object sender, RoutedEventArgs e)
        {
            Topmost = false;
            Macros.ShiftLastWindow(0);
            Topmost = true;
        }

        private void buttonShift1_Click(object sender, RoutedEventArgs e)
        {
            Topmost = false;
            Macros.ShiftLastWindow(1);
            Topmost = true;

        }

        private void buttonShift2_Click(object sender, RoutedEventArgs e)
        {
            Topmost = false;
            Macros.ShiftLastWindow(2);
            Topmost = true;
        }
    }

}
