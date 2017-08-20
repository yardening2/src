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
            labelWiiConnectStatus.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
            labelWiiConnectStatus.Content = "";
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

        private void buttonCalibrate_Click(object sender, RoutedEventArgs e)
        {
            m_Calibrator.CalibrateFinishedEvent += onCalibrationFinished;
            m_WiiMoteWrapper.ConnectionStateChangeEvent += onConnectionStateChange;
            m_WiiMoteWrapper.ConnectToWiimote();
        }

        private void onConnectionStateChange(object i_Sender, WiiMoteWrapper.eWiiConnectivityState i_State)
        {
            switch (i_State)
            {
                case WiiMoteWrapper.eWiiConnectivityState.Connected:
                    changeLableStringFromThread(labelWiiConnectStatus,"Connected To WiiMote");
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

        public void changeLableStringFromThread(Label i_labelToChange,string i_StateString)
        {
            Dispatcher.Invoke(new Action(() => { i_labelToChange.Content = i_StateString; }));
        }

        private void onCalibrationFinished(object i_Sender, EventArgs i_EventArgs)
        {
            m_WiimoteToMouse = new WiiMoteToMouseCoverter(m_Calibrator.getCalibratedWarper(), m_WiiMoteWrapper);
        }

        private void calibrateButton_Click(object sender, RoutedEventArgs e)
        {
            m_WiiMoteWrapper.ConnectionEstablishedEvent += onConnectionSuccessfull;
            m_WiiMoteWrapper.ConnectToWiimote();
        }

        private void onConnectionSuccessfull(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() => { m_Calibrator.CalibrateScreen(m_WiiMoteWrapper); }));
        }

        public void StopWiimoteWrapper()
        {
            m_WiiMoteWrapper.DisconnectFromWiiMote();
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