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
            textBlockScreenShotsPath.Text = TheBoardApp.MainWindow.ScreenShotsFolder;
        }

        private void initBubbels()
        {
            boardBubble.clickHandler += () => MainWindow.runBoard();
            boardBubble.BubbleData = new ActionBubble.ActionBubbleData("BoardApp", eBubbleType.Board);
            MacroBubble.clickHandler += () => MainWindow.runMacroApp();
            MacroBubble.BubbleData = new ActionBubble.ActionBubbleData("MacroApp", eBubbleType.Macro);
        }



        private void addActionBubble_Click(object sender, RoutedEventArgs e)
        {
            ActionBubble ab = WiisyScreenUIHelper.CreateCustomizeActionBubble();
            if (ab != null)
            {
                addActionBubbleToRepository(ab);
            }
        }

        private void addActionBubbleToRepository(ActionBubble ab)
        {
            apllicationStackPanel.Children.Remove(addActionBubble);
            apllicationStackPanel.Children.Add(ab);
            apllicationStackPanel.Children.Add(addActionBubble);
        }

        private void buttonChooseSCFolder_Click(object sender, RoutedEventArgs e)
        {
            string scFolder = WiisyScreenUIHelper.chooseFolder(textBlockScreenShotsPath.Text);
            if (scFolder != null)
            {
                TheBoardApp.MainWindow.ScreenShotsFolder = scFolder;
                textBlockScreenShotsPath.Text = scFolder;
            }
        }

        internal void initRepository(List<ActionBubble.ActionBubbleData> repositoryData)
        {
            apllicationStackPanel.Children.Clear();
            apllicationStackPanel.Children.Add(addActionBubble);

            foreach (ActionBubble.ActionBubbleData bubbleData in repositoryData)
            {
                addActionBubbleToRepository(WiisyScreenUIHelper.CreateActionBubbleFromData(bubbleData));
            }
        }

        private void buttonGeneratePDF_Click(object sender, RoutedEventArgs e)
        {
            string[] fileNames = WiisyScreenUIHelper.ChoosePics(textBlockScreenShotsPath.Text);
            if (fileNames != null && fileNames.Length != 0)
            {
                ScreenSaver.ScreenSaver.CreatePDF(textBlockScreenShotsPath.Text, fileNames);
            }
        }

        public List<ActionBubble.ActionBubbleData> GetRepository()
        {
            List<ActionBubble.ActionBubbleData> theRepository = new List<ActionBubble.ActionBubbleData>();

            foreach (Control c in apllicationStackPanel.Children)
            {
                if (c is ActionBubble)
                {
                    theRepository.Add((c as ActionBubble).BubbleData);
                }
            }

            return theRepository;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetRepository();
        }

        /*
        private void WriteToFile_Click(object sender, RoutedEventArgs e)
        {
            List<ActionBubble.ActionBubbleData> i = new List<ActionBubble.ActionBubbleData>();
            
            i.Add(new ActionBubble.ActionBubbleData("Test", eBubbleType.Macro));
            i.Add(new ActionBubble.ActionBubbleData("234", eBubbleType.Empty));
            i.Add(new ActionBubble.ActionBubbleData());

            WiisyScreenUIHelper.WriteToBinaryFile<List<ActionBubble.ActionBubbleData>>("test", i);
        }

        private void ReadToFile_Click(object sender, RoutedEventArgs e)
        {
            List<ActionBubble.ActionBubbleData> i = WiisyScreenUIHelper.ReadFromBinaryFile< List<ActionBubble.ActionBubbleData>>("test");
        }
        */
    }
}
