using Project_philico_food.functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_philico_food.Pages
{
    public partial class frmSetting : Form
    {
        public frmSetting()
        {
            InitializeComponent();
        }


        void readConfig()
        {
            AESEncryption aESEncryption = new AESEncryption();
            string comport = aESEncryption.Decrypt(ConfigurationManager.AppSettings["SCALE_PORT"]);
            string bardRate = aESEncryption.Decrypt(ConfigurationManager.AppSettings["SCALE_BAUDRATE"]);
            string stationName = aESEncryption.Decrypt(ConfigurationManager.AppSettings["STATION_NAME"]);

            cbbComport.Items.Add(comport);
            cbbComport.SelectedIndex = 0;

            cbbBaudRate.Items.Add(bardRate);
            cbbBaudRate.SelectedIndex = 0;

            txtStationName.Text = stationName;
        }

        void saveConfig()
        {
            Configuration Config1 = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            AESEncryption aESEncryption = new AESEncryption();
            Config1.AppSettings.Settings["SCALE_PORT"].Value = aESEncryption.Encrypt(cbbComport.Text);
            Config1.AppSettings.Settings["SCALE_BAUDRATE"].Value = aESEncryption.Encrypt(cbbBaudRate.Text);
            Config1.AppSettings.Settings["STATION_NAME"].Value = aESEncryption.Encrypt(txtStationName.Text);

            Config1.Save(ConfigurationSaveMode.Modified);
            msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
            msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
            msg.Show("Save success please open again", "success");
            //Application.Exit();
            this.Close();
        }

        private void frmSetting_Load(object sender, EventArgs e)
        {
            readConfig();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveConfig();
        }

        private void cbbComport_DropDown(object sender, EventArgs e)
        {
            cbbComport.Items.Clear();
            string[] port = SerialPort.GetPortNames();
            foreach (string portName in port)
            {
                cbbComport.Items.Add($"{portName}");
            }
        }

        private void cbbBaudRate_DropDown(object sender, EventArgs e)
        {
            cbbBaudRate.Items.Clear();
            string[] baudrate = { "600", "1200", "2400", "4800", "9600", "19200", "56000", "57600", "115200" };
            foreach (string item in baudrate)
            {
                cbbBaudRate.Items.Add(item);
            }
        }
    }
}
