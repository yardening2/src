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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WiiMoteConnect;
using WiiMoteConnect.WiiMoteControlls;
namespace WiisyScreen
{
    /// <summary>
    /// Interaction logic for SettingsCustomTabControl.xaml
    /// </summary>
    public partial class SettingsCustomTabControl : UserControl
    {

        private WiiMoteWrapper m_WiiMoteWrapper;
        private Calibrator m_Calibrator;
        private WiiMoteToMouseCoverter m_WiimoteToMouse;

        public SettingsCustomTabControl()
        {
            InitializeComponent();
            initBubbels();
            textBlockScreenShotsPath.Text = TheBoardApp.MainWindow.ScreenShotsFolder;
            m_WiiMoteWrapper = new WiiMoteWrapper();
            m_Calibrator = new Calibrator(m_WiiMoteWrapper);
            m_Calibrator.CalibrateFinishedEvent += onCalibrationFinished;
            labelWiiConnectStatus.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
            labelWiiConnectStatus.Content = "";
        }

        public bool ToSaveData()
        {
            return checkBoxSaveData.IsChecked.Value;
        }

        private void initBubbels()
        {
            apllicationStackPanel.Children.Clear();
            apllicationStackPanel.Children.Add(addActionBubble);

            addActionBubbleToRepasatory(WiisyScreenUIHelper.CreateActionBubbleFromData(new ActionBubble.ActionBubbleData("", eBubbleType.Empty)));
            addActionBubbleToRepasatory(WiisyScreenUIHelper.CreateActionBubbleFromData(new ActionBubble.ActionBubbleData("BoardApp", eBubbleType.Board)));
            addActionBubbleToRepasatory(WiisyScreenUIHelper.CreateActionBubbleFromData(new ActionBubble.ActionBubbleData("MacroApp", eBubbleType.Macro)));
            addActionBubbleToRepasatory(WiisyScreenUIHelper.CreateActionBubbleFromData(new ActionBubble.ActionBubbleData("Osk", eBubbleType.Osk)));
            addActionBubbleToRepasatory(WiisyScreenUIHelper.CreateActionBubbleFromData(new ActionBubble.ActionBubbleData("Calc", eBubbleType.Calc)));



        }


        private void addActionBubbleToRepasatory(ActionBubble i_BubbleToAdd)
        {
            if (i_BubbleToAdd != null)
            {
                apllicationStackPanel.Children.Remove(addActionBubble);
                apllicationStackPanel.Children.Add(i_BubbleToAdd);
                apllicationStackPanel.Children.Add(addActionBubble);
            }
        }

        private void addActionBubble_Click(object sender, RoutedEventArgs e)
        {
            ActionBubble ab = WiisyScreenUIHelper.CreateCustomizeActionBubble();

            if (ab != null)
            {
                if (findActionBubbleInReposeory(ab) == null)
                {
                    addActionBubbleToRepasatory(ab);
                }
                else
                {
                    notifayAppsError("App Alredy Exists");
                }
            }
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
                addActionBubbleToRepasatory(WiisyScreenUIHelper.CreateActionBubbleFromData(bubbleData));
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

        private void buttonCalibrate_Click(object sender, RoutedEventArgs e)
        {
            if (m_WiiMoteWrapper.Connected)
            {
                m_Calibrator.CalibrateScreen(m_WiiMoteWrapper);
            }
            else
            {
                m_WiiMoteWrapper.ConnectionStateChangeEvent += onConnectionStateChange;
                m_WiiMoteWrapper.ConnectToWiimote();
                m_WiiMoteWrapper.ConnectionEstablishedEvent += onConnectionSuccessfull;
            }
        }

        private void onConnectionStateChange(object i_Sender, WiiMoteWrapper.eWiiConnectivityState i_State)
        {
            switch (i_State)
            {
                case WiiMoteWrapper.eWiiConnectivityState.Connected:
                    changeLableStringFromThread(labelWiiConnectStatus, "Connected To WiiMote");
                    m_WiiMoteWrapper.ConnectionStateChangeEvent -= onConnectionStateChange;
                    m_Calibrator.CalibrateScreen(m_WiiMoteWrapper);
                    break;
                case WiiMoteWrapper.eWiiConnectivityState.Connecting:
                    changeLableStringFromThread(labelWiiConnectStatus, "Connecting...");
                    break;
                case WiiMoteWrapper.eWiiConnectivityState.Failed_To_Connect:
                    changeLableStringFromThread(labelWiiConnectStatus, "Could Not connected To WiiMote");
                    break;
                case WiiMoteWrapper.eWiiConnectivityState.Not_Found:
                    changeLableStringFromThread(labelWiiConnectStatus, "WiiMote not found");
                    break;
                case WiiMoteWrapper.eWiiConnectivityState.Searching:
                    changeLableStringFromThread(labelWiiConnectStatus, "Searching...");
                    break;
            }
        }

        public void changeLableStringFromThread(Label i_labelToChange, string i_StateString)
        {
            Dispatcher.Invoke(new Action(() => { i_labelToChange.Content = i_StateString; }));
        }

        private void onCalibrationFinished(object i_Sender, EventArgs i_EventArgs)
        {
            m_WiimoteToMouse = new WiiMoteToMouseCoverter(m_Calibrator.getCalibratedWarper(), m_WiiMoteWrapper);
        }

        private void onConnectionSuccessfull(object sender, EventArgs e)
        {
            //this.Dispatcher.Invoke(new Action(() => { m_Calibrator.CalibrateScreen(m_WiiMoteWrapper); }));
        }

        public void StopWiimoteWrapper()
        {
            m_WiiMoteWrapper.DisconnectFromWiiMote();
        }

        private void Rectangle_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("Object"))
            {
                // These Effects values are used in the drag source's
                // GiveFeedback event handler to determine which cursor to display.
                if (e.KeyStates == DragDropKeyStates.ControlKey)
                {
                    e.Effects = DragDropEffects.Copy;
                }
                else
                {
                    e.Effects = DragDropEffects.Move;
                }
            }

        }

        private ActionBubble findActionBubbleInReposeory(ActionBubble i_BubbleToFind)
        {
            ActionBubble res = null;

            if (i_BubbleToFind.BubbleData.BubbleType == eBubbleType.Exe)
            {
                foreach (UIElement item in apllicationStackPanel.Children)
                {
                    if (item is ActionBubble)
                    {
                        if ((item as ActionBubble).BubbleData.Equals(i_BubbleToFind.BubbleData))
                        {
                            res = (item as ActionBubble);
                            break;
                        }
                    }
                }
            }

            return res;
        }


        private void Rectangle_Drop(object sender, DragEventArgs e)
        {
            ActionBubble refBubble = e.Data.GetData("Object") as ActionBubble;

            if (refBubble.BubbleData.BubbleType == eBubbleType.Exe)
            {
        
                apllicationStackPanel.Children.Remove(findActionBubbleInReposeory(refBubble));
            }
            else
            {
                notifayAppsError("Cant Delete This Item");
            }
        }

        private void notifayAppsError(string i_Msg)
        {
            appsNotificationText.Text = i_Msg;
            appsNotification.BeginAnimation(OpacityProperty, new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(2.5))));

        }

        private void buttonResrote_Click(object sender, RoutedEventArgs e)
        {
            initBubbels();
        }
    }
}
