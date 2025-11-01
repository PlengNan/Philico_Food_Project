using Guna.UI2.WinForms;
using Project_philico_food.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Project_philico_food.Service;

namespace Project_philico_food.Pages
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();

        }


    private void frmMain_Load(object sender, EventArgs e)
    {
        var u = AuthService.CurrentUser;
        if (u != null)
        {
            this.Text = $"Project_philico_food -Hello {u.Name ?? u.Username}";
        }
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
            frmSetting frmSetting = new frmSetting();
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

        private void frmMain_Load_1(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }
    }
}
