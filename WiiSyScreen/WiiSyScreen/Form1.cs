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
    public partial class Form1 : Form
    {
        WiiMoteWrapper m_WiiMoteWrapper;

        public Form1()
        {
            InitializeComponent();
            m_WiiMoteWrapper = new WiiMoteWrapper();
        }

        private void connectToWiiMoteButton_Click(object sender, EventArgs e)
        {
            try
            {
                m_WiiMoteWrapper.ConnectToWiimote();
                updateComponents();
            }
            catch (Exception i_Exception)
            {
                MessageBox.Show(i_Exception.Message);
            }
        }

        private void updateComponents()
        {
            this.Text = "Connected To Wii";
            connectToWiiMoteButton.Visible = false;
            CalibrateButton.Visible = true;
            BatteryLevelTextLabel.Visible = true;
            BatteryLevelValueLabel.Visible = true;
            m_WiiMoteWrapper.VisibleIRDotsChangedEvent += test;
            BatteryLevelValueLabel.Text = m_WiiMoteWrapper.BatteryLevel.ToString();
        }

        private void test(object sender, int lala){
            this.Invoke(new Action(() => { Text = "visibleDots = " + lala; }));
        }

        private void CalibrateButton_Click(object sender, EventArgs e)
        {
            Calibrator cal = new Calibrator();
            cal.CalibrateScreen(m_WiiMoteWrapper);
            new CalibrationForm(m_WiiMoteWrapper).Show();
        }
    }
}
