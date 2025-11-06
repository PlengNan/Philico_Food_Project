using Guna.UI2.WinForms;
using Project_philico_food.Service;
using Project_philico_food.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_philico_food.Pages
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();

        }
        public void SetWebVisible(bool visible)
        {
            webView21.Visible = visible;
            guna2PictureBox1.Visible = !visible;
        }

        public bool IsWebVisible
        {
            get { return webView21.Visible; }
        }
        private void btnCus_Click(object sender, EventArgs e)
        {
            frmCustomer frmCustomer = new frmCustomer();
            this.Hide();
            frmCustomer.ShowDialog();
            this.Show();
        }

        private void btnPro_Click(object sender, EventArgs e)
        {
            frmProduct frmProduct = new frmProduct();
            this.Hide();
            frmProduct.ShowDialog();
            this.Show();
        }

        private void btnWeight_Click(object sender, EventArgs e)
        {
            frmWeighing frmWeighing = new frmWeighing();
            this.Hide();
            frmWeighing.ShowDialog();
            this.Show();
        }

        private void btnSetting_Click_1(object sender, EventArgs e)
        {
            frmSetting frmSetting = new frmSetting(this);
            this.Hide();
            frmSetting.ShowDialog();
            this.Show();
        }

       

        private void btnTodayReport_Click(object sender, EventArgs e)
        {
            frmTdrp frmTdrp = new frmTdrp();
            this.Hide();
            frmTdrp.ShowDialog();
            this.Show();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            frmReport frmReport = new frmReport();
            this.Hide();
            frmReport.ShowDialog();
            this.Show();
        }
        private void btnUsers_Click(object sender, EventArgs e)
        {
            frmUsers frmUsers = new frmUsers();
            this.Hide();
            frmUsers.ShowDialog();
            this.Show();
        }

        private async void frmMain_Load_1(object sender, EventArgs e)
        {
            Ping ping = new Ping();
            PingReply pingReply = await ping.SendPingAsync("www.thaiscale.co.th", 5000);
            if (pingReply.Status == IPStatus.Success)
            {
                await webView21.EnsureCoreWebView2Async(null);
                webView21.CoreWebView2.Settings.IsScriptEnabled = true;
                webView21.Source = new Uri("https://www.thaiscale.co.th/");
            }
            else { webView21.Visible = false; }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }

        private void webView21_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            if (!e.IsSuccess)
            {
                // โหลดไม่สำเร็จ → ซ่อน WebView2
                webView21.Visible = false;
            }
            else
            {
                webView21.Visible = true; // โหลดสำเร็จ
            }
        }

        private void webView21_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
