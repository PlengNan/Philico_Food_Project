using Project_philico_food.Db;
using Project_philico_food.functions;
using Project_philico_food.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_philico_food.Pages
{
    public partial class frmCustomer : Form
    {
        public frmCustomer()
        {
            InitializeComponent();

            int x = (this.Width - gbInfor.Width) / 2;
            int y = (this.Height - gbInfor.Height) / 2;
            gbInfor.Location = new Point(x, y);

            dgvList.DefaultCellStyle.ForeColor = Color.Black;
            dgvList.DefaultCellStyle.Font = new Font("Athiti", 12);
            dgvList.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvList.ColumnHeadersDefaultCellStyle.Font = new Font("Athiti", 12);

        }

        private int Id { get; set; } = 0;
        private string CustomerName { get; set; }
        private string CustomerCode { get; set; }


        void showGbInfor(bool onOff)
        {
            switch (onOff)
            {
                case true:
                    gbInfor.Visible = true;
                    gbList.Visible = false;
                    pnControl.Visible = false;
                    break;
                case false:
                    gbInfor.Visible = false;
                    gbList.Visible = true;
                    pnControl.Visible = true;
                    break;
            }
        }

        void clearText()
        {
            txtAddCustomerCode.Clear();
            txtAddCustomerName.Clear();
        }


        void addModelToGrid(List<CustomerModel> lists)
        {
            AESEncryption aESEncryption = new AESEncryption();
            foreach (CustomerModel list in lists)
            {
                dgvList.Rows.Add(list.Id, aESEncryption.Decrypt(list.CustomerCode), aESEncryption.Decrypt(list.CustomerName));
            }
        }


        void getCustomerList()
        {
            try
            {
                CustomerDb customerDb = new CustomerDb();
                List<CustomerModel> lists = customerDb.getAllCustomer();
                dgvList.Rows.Clear();
                if (lists == null)
                    return;
                addModelToGrid(lists);
            }
            catch (Exception ex)
            {


            }
        }

        void searchCustomerCode(string cusCode)
        {
            try
            {
                CustomerDb customerDb = new CustomerDb();
                List<CustomerModel> lists = customerDb.getCustomerByCustomerCode(cusCode);
                dgvList.Rows.Clear();
                if (lists == null)
                    return;
                addModelToGrid(lists);
            }
            catch (Exception ex)
            {


            }
        }

        void searchCustomerName(string cusName)
        {
            try
            {
                CustomerDb customerDb = new CustomerDb();
                List<CustomerModel> lists = customerDb.getCustomerByCustomerName(cusName);
                dgvList.Rows.Clear();
                if (lists == null)
                    return;
                addModelToGrid(lists);
            }
            catch (Exception ex)
            {


            }
        }

        void searchCodeAndName(string cusCode, string custName)
        {
            try
            {
                CustomerDb customerDb = new CustomerDb();
                List<CustomerModel> lists = customerDb.getCustomerByCustomerNameAndCode(cusCode, custName);
                dgvList.Rows.Clear();
                if (lists == null)
                    return;
                addModelToGrid(lists);
            }
            catch (Exception ex)
            {


            }
        }


        private void frmCustomer_Load(object sender, EventArgs e)
        {
            getCustomerList();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            showGbInfor(true);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            showGbInfor(false);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtAddCustomerCode.Text == "" || txtAddCustomerName.Text == "")
            {
                msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Warning;
                msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                msg.Show("Please fill information before save", "Fill information");
                return;
            }

            CustomerDb customerDb = new CustomerDb();
            AESEncryption aESEncryption = new AESEncryption();
            // check update or insert 
            if (Id == 0)
            {
                // insert 
                CustomerModel model = new CustomerModel
                {
                    CustomerName = aESEncryption.Encrypt(txtAddCustomerName.Text),
                    CustomerCode = aESEncryption.Encrypt(txtAddCustomerCode.Text),
                };

                if (customerDb.addNew(model))
                {
                    msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                    msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    msg.Show("Add new customer success", "Successfully");
                    clearText();
                    getCustomerList();
                    return;
                }
                else
                {
                    msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                    msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    msg.Show($"Error add new customer \n\nError {customerDb.Err}", "Error add new");
                }
            }
            else
            {
                // update
                CustomerModel model = new CustomerModel
                {
                    Id = Id,
                    CustomerName = aESEncryption.Encrypt(txtAddCustomerName.Text),
                    CustomerCode = aESEncryption.Encrypt(txtAddCustomerCode.Text),
                };

                if (customerDb.updateCustomer(model))
                {
                    msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                    msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    msg.Show("Update customer success", "Successfully");
                    CustomerName = null;
                    CustomerCode = null;
                    Id = 0;
                    clearText();
                    getCustomerList();
                    showGbInfor(false);
                }
                else
                {
                    msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                    msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    msg.Show($"Error update customer \n\nError {customerDb.Err}", "Error update");
                }

            }



        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            AESEncryption aESEncryption = new AESEncryption();
            if (txtSearchCode.Text == "" && txtSearchName.Text == "")
            {
                getCustomerList();
                return;
            }

            if (txtSearchCode.Text != "" && txtSearchName.Text != "")
            {
                searchCodeAndName(aESEncryption.Encrypt(txtSearchCode.Text), aESEncryption.Encrypt(txtSearchName.Text)); return;
            }

            if (txtSearchCode.Text != "")
            {
                searchCustomerCode(aESEncryption.Encrypt(txtSearchCode.Text));
                return;
            }

            if (txtSearchName.Text != "")
            {
                searchCustomerName(aESEncryption.Encrypt(txtSearchName.Text));
                return;
            }
        }

        private void dgvList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string clName = dgvList.Columns[e.ColumnIndex].Name;
                Id = int.Parse(dgvList.Rows[e.RowIndex].Cells["cl_id"].Value.ToString());
                CustomerCode = dgvList.Rows[e.RowIndex].Cells["cl_customerCode"].Value.ToString();
                CustomerName = dgvList.Rows[e.RowIndex].Cells["cl_customerName"].Value.ToString();
                switch (clName)
                {
                    case "cl_edit":
                        showGbInfor(true);
                        txtAddCustomerCode.Text = CustomerCode;
                        txtAddCustomerName.Text = CustomerName;
                        break;
                    case "cl_delete":
                        msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
                        msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
                        DialogResult result = msg.Show("Do you want delete customer ?", "Delete");
                        if ((result == DialogResult.Yes))
                        {
                            CustomerDb customerDb = new CustomerDb();
                            if (customerDb.deleteCustomerById(Id))
                            {
                                msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                                msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                                msg.Show("Delete customer success", "Delete success");
                                Id = 0;
                                CustomerCode = null;
                                CustomerName = null;
                                getCustomerList();
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {


            }
        }
    }
}
