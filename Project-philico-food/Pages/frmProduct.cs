using Project_philico_food.Db;
using Project_philico_food.functions;
using Project_philico_food.Models;
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
    public partial class frmProduct : Form
    {
        public frmProduct()
        {
            InitializeComponent();
            int x = (this.Width - gbInfor.Width) / 2;
            int y = (this.Height - gbInfor.Height) / 2;
            gbInfor.Location = new Point(x, y);
        }

        private int Id { get; set; } = 0;
        private string ProductNames { get; set; }
        private string ProductCode { get; set; }


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
            txtAddProductCode.Clear();
            txtAddProductName.Clear();
        }
        void addModelToGrid(List<ProductModel> lists)
        {
            AESEncryption aESEncryption = new AESEncryption();
            foreach (ProductModel list in lists)
            {
                dgvList.Rows.Add(list.Id, aESEncryption.Decrypt(list.ProductCode), aESEncryption.Decrypt(list.ProductName));
            }
        }
        void getProductList()
        {
            try
            {
                ProductDb productDb = new ProductDb();
                List<ProductModel> lists = productDb.getAllProduct();
                dgvList.Rows.Clear();
                if (lists == null)
                    return;
                addModelToGrid(lists);
            }
            catch (Exception ex)
            {


            }
        }

        void searchProductCode(string cusCode)
        {
            try
            {
                ProductDb productDb = new ProductDb();
                List<ProductModel> lists = productDb.getProductByCode(cusCode);
                dgvList.Rows.Clear();
                if (lists == null)
                    return;
                addModelToGrid(lists);
            }
            catch
            {


            }
        }

        void searchProductName(string cusName)
        {
            try
            {
                ProductDb productDb = new ProductDb();
                List<ProductModel> lists = productDb.getProductByNames(cusName);
                dgvList.Rows.Clear();
                if (lists == null)
                    return;
                addModelToGrid(lists);
            }
            catch 
            {


            }
        }

        void searchCodeAndName(string prdName, string prdCode)
        {
            try
            {
                ProductDb productDb = new ProductDb();
                List<ProductModel> lists = productDb.getProductByNamesAndCode(prdName, prdCode);
                dgvList.Rows.Clear();
                if (lists == null)
                    return;
                addModelToGrid(lists);
            }
            catch (Exception ex)
            {


            }
        }

        private void frmProduct_Load(object sender, EventArgs e)
        {
            getProductList();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtAddProductName.Text == "" || txtAddProductCode.Text == "")
            {
                msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Warning;
                msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                msg.Show("Please fill information before save", "Fill information");
                return;
            }

            ProductDb productDb = new ProductDb();
            AESEncryption aESEncryption = new AESEncryption();
            // check update or insert 
            if (Id == 0)
            {
                // insert 
                ProductModel model = new ProductModel
                {
                    ProductCode = aESEncryption.Encrypt(txtAddProductCode.Text),
                    ProductName = aESEncryption.Encrypt(txtAddProductName.Text),
                };

                if (productDb.addNew(model))
                {
                    msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                    msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    msg.Show("Add new product success", "Successfully");
                    clearText();
                    getProductList();
                    return;
                }
                else
                {
                    msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                    msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    msg.Show($"Error add new product \n\nError {productDb.Err}", "Error add new");
                }
            }
            else
            {
                // update
                ProductModel model = new ProductModel
                {
                    Id = Id,
                    ProductCode = aESEncryption.Encrypt(txtAddProductCode.Text),
                    ProductName = aESEncryption.Encrypt(txtAddProductName.Text),
                };

                if (productDb.updateProduct(model))
                {
                    msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                    msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    msg.Show("Update product success", "Successfully");
                    ProductNames = null;
                    ProductCode = null;
                    Id = 0;
                    clearText();
                    getProductList();
                    showGbInfor(false);
                }
                else
                {
                    msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                    msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    msg.Show($"Error update product \n\nError {productDb.Err}", "Error update");
                }

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            showGbInfor(false);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            showGbInfor(true);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            AESEncryption aESEncryption = new AESEncryption();
            if (txtProductCode.Text == "" && txtProductName.Text == "")
            {
                getProductList();
                return;
            }

            if (txtProductCode.Text != "" && txtProductName.Text != "")
            {
                searchCodeAndName(aESEncryption.Encrypt(txtProductName.Text), aESEncryption.Encrypt(txtProductCode.Text));
                return;
            }

            if (txtProductCode.Text != "")
            {
                searchProductCode(aESEncryption.Encrypt(txtProductCode.Text));
                return;
            }

            if (txtProductName.Text != "")
            {
                searchProductName(aESEncryption.Encrypt(txtProductName.Text));
                return;
            }
        }

        private void dgvList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string clName = dgvList.Columns[e.ColumnIndex].Name;
                Id = int.Parse(dgvList.Rows[e.RowIndex].Cells["cl_id"].Value.ToString());
                ProductCode = dgvList.Rows[e.RowIndex].Cells["cl_productCode"].Value.ToString();
                ProductNames = dgvList.Rows[e.RowIndex].Cells["cl_productName"].Value.ToString();
                switch (clName)
                {
                    case "cl_edit":
                        showGbInfor(true);
                        txtAddProductCode.Text = ProductCode;
                        txtAddProductName.Text = ProductNames;
                        break;
                    case "cl_delete":
                        msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
                        msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
                        DialogResult result = msg.Show("Do you want delete product ?", "Delete");
                        if ((result == DialogResult.Yes))
                        {
                            ProductDb productDb = new ProductDb();
                            if (productDb.deleteProductById(Id))
                            {
                                msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                                msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                                msg.Show("Delete product success", "Delete success");
                                Id = 0;
                                ProductNames = null;
                                ProductCode = null;
                                getProductList();
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
