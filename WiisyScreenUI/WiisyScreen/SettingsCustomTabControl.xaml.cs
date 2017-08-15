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

namespace WiisyScreen
{
    /// <summary>
    /// Interaction logic for SettingsCustomTabControl.xaml
    /// </summary>
    public partial class SettingsCustomTabControl : UserControl
    {
        public SettingsCustomTabControl()
        {
            InitializeComponent();
            initBubbels();
        }

        private void initBubbels()
        {
            boardBubble.clickHandler += () => MainWindow.runBoard();
            MacroBubble.clickHandler += () => MainWindow.runMacroApp();
        }



        private void addActionBubble_Click(object sender, RoutedEventArgs e)
        {
            ActionBubble ab = WiisyScreenUIHelper.CreateCustomizeActionBubble();
            if (ab != null)
            {
                apllicationStackPanel.Children.Remove(addActionBubble);
                apllicationStackPanel.Children.Add(ab);
                apllicationStackPanel.Children.Add(addActionBubble);
            }
        }

        private void buttonChooseSCFolder_Click(object sender, RoutedEventArgs e)
        {
            string scFolder = WiisyScreenUIHelper.chooseFolder();
            if(scFolder != null)
            {
                TheBoardApp.MainWindow.ScreenShotsFolder = scFolder;
                textBlockScreenShotsPath.Text = scFolder;
            }
        }
    }
}
