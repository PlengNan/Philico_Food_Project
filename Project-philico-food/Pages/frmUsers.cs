using Microsoft.VisualStudio.OLE.Interop;
using Project_philico_food.Db;
using Project_philico_food.functions;     
using Project_philico_food.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Project_philico_food.Pages
{
    public partial class frmUsers : Form
    {

        public frmUsers()
        {
            InitializeComponent();
            btnSave.Click += btnSave_Click;

            int x = (this.Width - gbInfo.Width) / 2;
            int y = (this.Height - gbInfo.Height) / 2;
            gbInfo.Location = new Point(x, y);
        }

        private int Id { get; set; } = 0;
        private string NameText { get; set; }
        private string UsernameText { get; set; }
        private string EmailText { get; set; }
        private string PhoneText { get; set; }
        private string ConfirmText { get; set; }

        private void frmUsers_Load(object sender, EventArgs e)
        {
            DbConnect.connect();

            dgvList.AutoGenerateColumns = false;
            dgvList.AllowUserToAddRows = false;
            if (dgvList.Columns.Contains("cl_id"))
                dgvList.Columns["cl_id"].Visible = false;  

            showGbInfo(false);
            getUsersList();
        }

        
        void showGbInfo(bool onOff)
        {
            switch (onOff)
            {
                case true:
                    gbInfo.Visible = true;
                    gbList.Visible = false;
                    pnControl.Visible = false;
                    break;
                case false:
                    gbInfo.Visible = false;
                    gbList.Visible = true;
                    pnControl.Visible = true;
                    break;
            }
        }

        void clearText()
        {
            txtAddName.Clear();
            txtAddUsername.Clear();
            txtAddEmail.Clear();
            txtAddPhone.Clear();
            txtAddPassword.Clear();
            txtConfirmPassword.Clear();
        }

        void addModelToGrid(List<UsersModel> list)
        {
            var aes = new AESEncryption();

            dgvList.DataSource = null;
            dgvList.Rows.Clear();

            foreach (var u in list)
            {
                int rowIndex = dgvList.Rows.Add();
                var row = dgvList.Rows[rowIndex];

                row.Cells["cl_id"].Value = u.Id;
                row.Cells["cl_name"].Value = aes.Decrypt(u.Name);
                row.Cells["cl_username"].Value = aes.Decrypt(u.Username);
                row.Cells["cl_email"].Value = aes.Decrypt(u.Email);
                row.Cells["cl_phone"].Value = aes.Decrypt(u.Phone);
            }
        }


        void getUsersList()
        {
            try
            {
                var repo = new UsersDb();
                var lists = repo.GetAll();
                if (lists == null) return;

                addModelToGrid(lists);
            }
            catch {  }
        }

        void searchUsers(string namePlain, string usernamePlain)
        {
            try
            {
                var aes = new AESEncryption();
                var repo = new UsersDb();

                string encName = string.IsNullOrWhiteSpace(namePlain) ? null : aes.Encrypt(namePlain.Trim());
                string encUser = string.IsNullOrWhiteSpace(usernamePlain) ? null : aes.Encrypt(usernamePlain.Trim());

                var lists = repo.SearchExactEncrypted(encName, encUser);

                dgvList.Rows.Clear();
                if (lists == null) return;
                addModelToGrid(lists);
            }
            catch { }
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" && txtUsername.Text == "")
            {
                getUsersList();
                return;
            }
            searchUsers(txtName.Text, txtUsername.Text);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Id = 0;
            clearText();
            gbInfo.Text = "Add new product";
            showGbInfo(true);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            showGbInfo(false);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAddName.Text) ||
                 string.IsNullOrWhiteSpace(txtAddUsername.Text))
            {
                msg.Parent = this;
                msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Warning;
                msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                msg.Show("Please fill information before save", "Fill information");
                return;
            }


            UsersDb usersDb = new UsersDb();
            AESEncryption aESEncryption = new AESEncryption();
            
            var model = new UsersModel
                {
                    Id = Id,
                    Name = aESEncryption.Encrypt(txtAddName.Text.Trim()),
                    Username = aESEncryption.Encrypt(txtAddUsername.Text.Trim()),
                    Email = aESEncryption.Encrypt(txtAddEmail.Text.Trim()),
                    Phone = aESEncryption.Encrypt(txtAddPhone.Text.Trim()),
                    Password = aESEncryption.Encrypt(txtAddPassword.Text)
                };

            if (Id == 0 || !string.IsNullOrWhiteSpace(txtAddPassword.Text))
            {
                if (string.IsNullOrWhiteSpace(txtAddPassword.Text))
                {
                    msg.Parent = this;
                    msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Warning;
                    msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    msg.Show("Please fill Password", "Fill information");
                    return;
                }
                if (txtAddPassword.Text != txtConfirmPassword.Text)
                {
                    msg.Parent = this;
                    msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Warning;
                    msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    msg.Show("Please fill Confirm Password", "Fill information");
                    return;

                }
                if(usersDb.Add(model))
                {
                    msg.Parent = this;
                    msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                    msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    msg.Show("Add new user success", "Successfully");
                    clearText();
                    getUsersList();
                    return;
                }
                else
                {
                    msg.Parent = this;
                    msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                    msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    msg.Show($"Error Add new user \n\nError {usersDb.Err}", "Error add new");
                }
            }

            else
            {
                if (usersDb.Update(model) || usersDb.UpdateWithPassword(model, model.Password))
                {
                    msg.Parent = this;
                    msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                    msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    msg.Show("Update user success", "Successfully");
                    clearText();
                    getUsersList();
                    return;
                }
                else
                {
                    msg.Parent = this;
                    msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                    msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    msg.Show($"Error update user \n\nError {usersDb.Err}", "Error update");
                }
            }
 
           
        }

        private void label3_Click(object sender, EventArgs e)
        {
            
        }

        private void dgvList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string clName = dgvList.Columns[e.ColumnIndex].Name;

                Id = int.Parse(dgvList.Rows[e.RowIndex].Cells["cl_id"].Value.ToString());
                NameText = dgvList.Rows[e.RowIndex].Cells["cl_name"].Value?.ToString();
                UsernameText = dgvList.Rows[e.RowIndex].Cells["cl_username"].Value?.ToString();
                EmailText = dgvList.Rows[e.RowIndex].Cells["cl_email"].Value?.ToString();
                PhoneText = dgvList.Rows[e.RowIndex].Cells["cl_phone"].Value?.ToString();

                switch (clName)
                {
                    case "cl_edit":
                        showGbInfo(true);
                        gbInfo.Text = "Edit User";
                        txtAddName.Text = NameText;
                        txtAddUsername.Text = UsernameText;
                        txtAddEmail.Text = EmailText;
                        txtAddPhone.Text = PhoneText;
                        txtAddPassword.Clear();
                        break;

                    case "cl_delete":
                        {
                            msg.Parent = this;
                            msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
                            msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;

                            DialogResult result = msg.Show("Do you want delete this user?", "Delete");
                            if (result == DialogResult.Yes)
                            {
                                var repo = new UsersDb();
                                if (repo.Delete(Id))
                                {
                                    msg.Parent = this;
                                    msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                                    msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                                    msg.Show("Delete user success", "Delete success");

                                    Id = 0;
                                    NameText = null;
                                    UsernameText = null;
                                    EmailText = null;
                                    PhoneText = null;

                                    getUsersList();
                                }
                                else
                                {
                                    msg.Parent = this;
                                    msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                                    msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                                    msg.Show($"Error delete user\n\n{repo.Err}", "Error");
                                }
                            }
                            break;
                        }

                }
            }

            catch { }
        }
    }
}
