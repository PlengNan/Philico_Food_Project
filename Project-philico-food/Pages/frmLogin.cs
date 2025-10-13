using Microsoft.VisualStudio.OLE.Interop;
using Project_philico_food.Db;
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

namespace Project_philico_food.Pages
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            var username = txtUsername.Text;  
            var password = txtPassword.Text;

            if (!AuthService.Login(username, password, out var err))
            {
                msg.Parent = this;
                msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                msg.Show(err, "Error");
                txtPassword.SelectAll();
                txtPassword.Focus();
                return;
            }

            using (var frm = new frmMain())
            {
                this.Hide();
                frm.ShowDialog();
                this.Show();
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

            DbConnect.connect();
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F9)
            {
                frmMain   frmMain = new frmMain();
                this.Hide();
                frmMain.ShowDialog();
                this.Show();
            }
        }
    }
}
