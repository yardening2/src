using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WiiSyScreen.WiiMoteControlls;

namespace WiiSyScreen
{
    public partial class MainForm : Form
    {
        private WiiMoteWrapper m_WiiMoteWrapper;
        private Calibrator m_Calibrator;
        public MainForm()
        {
            InitializeComponent();
            m_WiiMoteWrapper = new WiiMoteWrapper();
            m_Calibrator = new Calibrator(m_WiiMoteWrapper);
            m_Calibrator.CalibrateFinishedEvent += onCalibrationFinished;
        }

        private void connectToWiiMoteButton_Click(object sender, EventArgs e)
        {
            try
            {
                connectToWiiMoteButton.Enabled = false;
                ConnectingLabel.Visible = true;
                this.Enabled = false;
                m_WiiMoteWrapper.ConnectToWiimote();
                updateComponents();
            }
            catch (Exception i_Exception)
            {
                MessageBox.Show(i_Exception.Message);
            }
            finally
            {
                connectToWiiMoteButton.Enabled = true;
                ConnectingLabel.Visible = false;
                this.Enabled = true;
                CalibrateButton.Focus();
            }
        }

        private void updateComponents()
        {
            this.Text = "Connected To Wii";
            connectToWiiMoteButton.Visible = false;
            CalibrateButton.Visible = true;
            BatteryLevelTextLabel.Visible = true;
            BatteryLevelValueLabel.Visible = true;
            VisibleIRDotsLabel.Visible = true;
            IRDotsDataLabel.Visible = true;
            CalibrateButton.Focus();
            m_WiiMoteWrapper.VisibleIRDotsChangedEvent += updateIRDotsCount;
            m_WiiMoteWrapper.AButtonPressed += CalibrateButton_Click;
            BatteryLevelValueLabel.Text = m_WiiMoteWrapper.BatteryLevel.ToString();

        }

        private void updateIRDotsCount(object i_Sender, int i_VisibleIRDots)
        {
            IRDotsDataLabel.Invoke(new Action(() => { IRDotsDataLabel.Text = i_VisibleIRDots.ToString(); }));
        }
        
        private void CalibrateButton_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() => { m_Calibrator.CalibrateScreen(m_WiiMoteWrapper); }));
        }

        private void onCalibrationFinished(object i_Sender, EventArgs i_EventArgs)
        {
            WiiMoteToMouseCoverter m = new WiiMoteToMouseCoverter(m_Calibrator.getCalibratedWarper(), m_WiiMoteWrapper);
        }

    }
}
