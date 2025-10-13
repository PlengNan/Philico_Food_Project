using Guna.UI2.WinForms;
using Project_philico_food.Db;
using Project_philico_food.functions;
using Project_philico_food.Functions;
using Project_philico_food.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_philico_food.Pages
{
    public partial class frmWeighing : Form
    {
        private volatile bool _isClosing = false;

        public frmWeighing()
        {
            InitializeComponent();
            initial();

            dgvList.DefaultCellStyle.ForeColor = Color.Black;
            dgvList.DefaultCellStyle.Font = new Font("Athiti", 12);
            dgvList.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvList.ColumnHeadersDefaultCellStyle.Font = new Font("Athiti", 12);
        }


        int newWeight = 0;
        int lastWeight = 0;

        ScaleFunc scaleFunc = new ScaleFunc();

        void initial()
        {
            cbbCusCode.Items.Add("-- Please select --");
            cbbCusName.Items.Add("-- Please select --");
            cbbPrdCode.Items.Add("-- Please select --");
            cbbPrdName.Items.Add("-- Please select --");

            cbbCusCode.SelectedIndex = 0;
            cbbCusName.SelectedIndex = 0;
            cbbPrdCode.SelectedIndex = 0;
            cbbPrdName.SelectedIndex = 0;
        }

        string decryptData(string value)
        {
            AESEncryption aESEncryption = new AESEncryption();
            return aESEncryption.Decrypt(value);
        }


        void updateUIDataComboboxCustomer(List<CustomerModel> lists, Guna2ComboBox cbb)
        {
            cbb.Items.Clear();
            switch (cbb.Tag)
            {
                case "Code":
                    foreach (CustomerModel item in lists)
                        cbb.Items.Add(decryptData(item.CustomerCode));
                    break;
                case "Name":
                    foreach (CustomerModel item in lists)
                        cbb.Items.Add(decryptData(item.CustomerName));
                    break;
            }
        }

        void updateUIDataComboboxProduct(List<ProductModel> lists, Guna2ComboBox cbb)
        {
            cbb.Items.Clear();
            switch (cbb.Tag)
            {
                case "Code":
                    foreach (ProductModel item in lists)
                        cbb.Items.Add(decryptData(item.ProductCode));
                    break;
                case "Name":
                    foreach (ProductModel item in lists)
                        cbb.Items.Add(decryptData(item.ProductName));
                    break;
            }
        }

        void updateUIDataGridView(List<OrderModel> lists)
        {
            foreach (OrderModel item in lists)
            {
                //dgvFirstWeight.Rows.Add(null, decryptData(item.Id.ToString()), decryptData(item.OrderNumber), decryptData(item.LicensePlate), decryptData(item.);
            }
        }

        private void LoadFirstWeighGrid()
        {
            try
            {
                var detailRepo = new OrderDetailDb();
                var dt = detailRepo.GetOpenFirstWeighTable();
                dgvList.Rows.Clear();

                if (dt == null || dt.Rows.Count == 0) return;

                foreach (DataRow r in dt.Rows)
                {
                    int rowIndex = dgvList.Rows.Add();
                    var row = dgvList.Rows[rowIndex];

                    row.Cells["cl_orNum"].Value = r["OrderNumber"]?.ToString();
                    row.Cells["cl_datez"].Value = r["Datez"]?.ToString();
                    row.Cells["cl_timez"].Value = r["Timez"]?.ToString();
                    row.Cells["cl_wg"].Value = r["Weight"]?.ToString();
                    row.Cells["cl_lcP"].Value = r["LicensePlate"]?.ToString();
                }

                dgvList.ClearSelection();
            }
            catch {  }
        }

        //void getFirstWeight()
        //{
        //    OrderDb orderDb = new OrderDb();
        //    List<OrderModel> lists = orderDb.getAllOrderByStatus("Process");
        //    if (lists == null)
        //        return;

        //    dgvFirstWeight.Rows.Clear();
        //    updateUIDataGridView(lists);
        //}

        void getCustomer(Guna2ComboBox cbb)
        {
            CustomerDb customerDb = new CustomerDb();
            List<CustomerModel> lists = customerDb.getAllCustomer();
            if (lists == null)
                return;

            updateUIDataComboboxCustomer(lists, cbb);
        }

        void getProduct(Guna2ComboBox cbb)
        {
            ProductDb productDb = new ProductDb();
            List<ProductModel> lists = productDb.getAllProduct();

            if (lists == null)
                return;

            updateUIDataComboboxProduct(lists, cbb);
        }

        private List<CustomerModel> _customerList = new List<CustomerModel>();
        private List<ProductModel> _productList = new List<ProductModel>();
        private void frmWeighing_Load(object sender, EventArgs e)
        {
            var cusDb = new CustomerDb();
            _customerList = cusDb.getAllCustomer() ?? new List<CustomerModel>();
            updateUIDataComboboxCustomer(_customerList, cbbCusCode);
            updateUIDataComboboxCustomer(_customerList, cbbCusName);

            var prdDb = new ProductDb();
            _productList = prdDb.getAllProduct() ?? new List<ProductModel>();
            updateUIDataComboboxProduct(_productList, cbbPrdCode);
            updateUIDataComboboxProduct(_productList, cbbPrdName);
            LoadFirstWeighGrid();
            if (!scaleFunc.Connect(spScale))
            {
                //msg.Parent = this;
                //msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                //msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                //msg.Show("Error connect scale");
                this.Close();
            }
        }

        private void CustomerDropDown(object sender, EventArgs e)
        {
            Guna2ComboBox cbb = sender as Guna2ComboBox;
            getCustomer(cbb);
        }

        private void ProductDropDown(object sender, EventArgs e)
        {
            Guna2ComboBox cbb = sender as Guna2ComboBox;
            getProduct(cbb);
        }

        private void spScale_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {

            if (_isClosing || this.IsDisposed || this.Disposing) return;

            string value;
            try { value = spScale.ReadLine(); }
            catch { return; } 

            string _weight = scaleFunc.DataReceive(value);
            if (!int.TryParse(_weight, out newWeight)) return;

            if (!this.IsHandleCreated || lblWeight.IsDisposed || !lblWeight.IsHandleCreated) return;

            try
            {
                BeginInvoke(new MethodInvoker(() =>
                {
                    if (_isClosing || this.IsDisposed) return;

                    lblWeight.Text = _weight;

                    if (newWeight != lastWeight)
                    {
                        lastWeight = newWeight;
                        tmCheckWeight.Stop();
                        tmCheckWeight.Start();
                        btnSave.Enabled = false;
                        btnStausWeight.ForeColor = Color.FromArgb(243, 128, 0);
                        btnStausWeight.BorderColor = Color.FromArgb(243, 128, 0);
                        btnStausWeight.FillColor = Color.FromArgb(239, 250, 240);
                        btnStausWeight.Text = "WEIGHING.....";
                    }
                }));
            }
            catch (ObjectDisposedException) {  }
            catch (InvalidOperationException) {  }
            
            //string value = spScale.ReadLine();
            //string _weight = scaleFunc.DataReceive(value);
            //if (!int.TryParse(_weight, out newWeight))
            //{
            //    return;
            //}

            //BeginInvoke(new MethodInvoker(delegate ()
            //{
            //    lblWeight.Text = _weight.ToString();
            //}));

            //// ถ้าน้ำหนักแตกต่างจากน้ำหนักล่าสุด
            //if (newWeight != lastWeight)
            //{
            //    lastWeight = newWeight;
            //    BeginInvoke(new MethodInvoker(delegate ()
            //    {
            //        tmCheckWeight.Stop();
            //        tmCheckWeight.Start();
            //        btnSave.Enabled = false;
            //        btnStausWeight.ForeColor = Color.FromArgb(243, 128, 0);
            //        btnStausWeight.BorderColor = Color.FromArgb(243, 128, 0);
            //        btnStausWeight.FillColor = Color.FromArgb(239, 250, 240);
            //        btnStausWeight.Text = "Weighing.....".ToUpper();
            //    }));
            //}
        }

        private void tmCheckWeight_Tick(object sender, EventArgs e)
        {
            tmCheckWeight.Stop(); // หยุด Timer

            btnSave.Enabled = true;
            btnStausWeight.ForeColor = Color.FromArgb(46, 125, 50);
            btnStausWeight.FillColor = Color.FromArgb(239, 250, 240);
            btnStausWeight.BorderColor = Color.FromArgb(46, 125, 50);
            btnStausWeight.Text = "STABLE";
        }

        private void dgvList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvList.Columns[e.ColumnIndex].Name == "cl_delete")
            {
                string orderNumber = dgvList.Rows[e.RowIndex].Cells["cl_orNum"].Value?.ToString();
                if (string.IsNullOrEmpty(orderNumber)) return;

                msg.Parent = this;
                msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
                msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;

                DialogResult result = msg.Show("Do you want delete this user?", "Delete");

                if (result == DialogResult.Yes)
                {
                    var orderRepo = new OrderDb();
                    var detailRepo = new OrderDetailDb();

                    bool delDetail = detailRepo.DeleteByOrderNumber(orderNumber);
                    bool delOrder = orderRepo.DeleteOrder(orderNumber);

                    if (delDetail && delOrder)
                    {
                        msg.Parent = this;
                        msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                        msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                        msg.Show("Delete successfully", "Delete success");
                        LoadFirstWeighGrid();
                    }
                    else
                    {
                        msg.Parent = this;
                        msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                        msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                        msg.Show($"Error delete user\n\n{detailRepo.Err}", "Error");
                    }
                }
            }

            if (dgvList.Columns[e.ColumnIndex].Name == "cl_orNum")
            {
                string orderNumber = dgvList.Rows[e.RowIndex].Cells["cl_orNum"].Value?.ToString();
                if (string.IsNullOrWhiteSpace(orderNumber)) return;

                var orderRepo = new OrderDb();
                var order = orderRepo.getOrderByOrderNumberOrId(orderNumber);
                if (order == null) return;

                cbbCusName.Text = order.CustomerName ?? "";
                cbbPrdName.Text = order.ProductName ?? "";
                txtLicense.Text = order.LicensePlate ?? "";
                txtNote.Text = order.Note ?? "";

                lblWeight.Text = dgvList.Rows[e.RowIndex].Cells["cl_wg"].Value?.ToString() ?? "0";

                btnSave.Enabled = true;
                btnStausWeight.Text = "READY TO WEIGH OUT";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //validate weight
            if (!int.TryParse(lblWeight.Text, out int weight) || weight <= 0)
            {
                msg.Parent = this;
                msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                msg.Show ("Invalid weight value");
                return;
            }

            //read form date
            string customerName = cbbCusName.Text?.Trim();
            string productName = cbbPrdName.Text?.Trim();
            string licensePlate = txtLicense?.Text?.Trim();
            string note = txtNote?.Text?.Trim();

            if (string.IsNullOrWhiteSpace(customerName) || customerName.StartsWith("--"))
            {
                msg.Parent = this;
                msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Warning;
                msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                msg.Show("Please select a customer");
                return;
            }
            if (string.IsNullOrWhiteSpace(productName) || productName.StartsWith("--"))
            {
                msg.Parent = this;
                msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Warning;
                msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                msg.Show("Please select a productName");
                return;
            }
            if (string.IsNullOrWhiteSpace(licensePlate))
            {
                msg.Parent = this;
                msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Warning;
                msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                msg.Show("Please enter license plate");
                return;
            }

            var orderRepo = new OrderDb();
            var detailRepo = new OrderDetailDb();

            var activeOrder = orderRepo.GetActiveByPlate(licensePlate);

            if (activeOrder == null)
            {
                // weight in
                string orderNumber = orderRepo.GenerateOrderNumber();
                if (string.IsNullOrEmpty(orderNumber))
                {
                    msg.Parent = this;
                    msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                    msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    msg.Show("Failed to generate order number");
                    return;
                }

                var order = new OrderModel
                {
                    OrderNumber = orderNumber,
                    ProductName = productName,
                    CustomerName = customerName,
                    Note = note,
                    NetWeight = 0,
                    Status = "Process",
                    LicensePlate = licensePlate
                };

                if (!orderRepo.addNew(order))
                {
                    msg.Parent = this;
                    msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                    msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    msg.Show("Failed to save order");
                    return;
                }

                var detailIn = new OrderDetailModel
                {
                    OrderNumber = orderNumber,
                    Datez = DateTime.Now.ToString("yyyy-MM-dd"),
                    Timez = DateTime.Now.ToString("HH:mm:ss"),
                    Weight = weight,
                    WeightType = "IN"
                };

                if (!detailRepo.Add(detailIn))
                {
                    msg.Parent = this;
                    msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                    msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    msg.Show("Failed to save order");
                    return;
                }
                msg.Parent = this;
                msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                msg.Show("First weighing IN saved Successfully");
                ResetWeighingInputs();
            }
            else
            {
                // weight out
                string orderNumber = activeOrder.OrderNumber;

                // ดึง weight in จาก OrderDetail
                var details = detailRepo.GetByOrderNumber(orderNumber);
                if (details == null || details.All(d => !string.Equals(d.WeightType, "IN", StringComparison.OrdinalIgnoreCase)))
                {
                    msg.Parent = this;
                    msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                    msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    msg.Show("No IN record found for this order");
                    return;
                }

                int firstWeight = details.First(d => d.WeightType == "IN").Weight;

                var detailOut = new OrderDetailModel
                {
                    OrderNumber = orderNumber,
                    Datez = DateTime.Now.ToString("yyyy-MM-dd"),
                    Timez = DateTime.Now.ToString("HH:mm:ss"),
                    Weight = weight,
                    WeightType = "OUT"
                };

                if (!detailRepo.Add(detailOut))
                {
                    msg.Parent = this;
                    msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                    msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    msg.Show("Failed to save order");
                    return;
                }

                int net = Math.Abs(weight - firstWeight);

                if (!orderRepo.UpdateNetWeight(orderNumber, net, "Completed" , note))
                {
                    msg.Parent = this;
                    msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                    msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    msg.Show("Failed to update order net weight");
                    return;
                }

                msg.Parent = this;
                msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                msg.Show("First weighing Out saved Successfully");
                ResetWeighingInputs();
            }

            // refresh right grid (process list)
            //getFirstWeight();
            LoadFirstWeighGrid();

        }
        private void ResetWeighingInputs()
        {

            lblWeight.Text = "0";
            newWeight = 0;
            lastWeight = 0;

            if (cbbCusCode.Items.Count > 0) cbbCusCode.SelectedIndex = 0;
            if (cbbCusName.Items.Count > 0) cbbCusName.SelectedIndex = 0;
            if (cbbPrdCode.Items.Count > 0) cbbPrdCode.SelectedIndex = 0;
            if (cbbPrdName.Items.Count > 0) cbbPrdName.SelectedIndex = 0;

            txtLicense.Text = string.Empty;
            txtNote.Text = string.Empty;

            btnSave.Enabled = false;

            btnStausWeight.ForeColor = Color.FromArgb(243, 128, 0);
            btnStausWeight.BorderColor = Color.FromArgb(243, 128, 0);
            btnStausWeight.FillColor = Color.FromArgb(239, 250, 240);
            btnStausWeight.Text = "WEIGHING.....";

            if (dgvList.CurrentRow != null)
                dgvList.ClearSelection();
        }

        private void DetachScale()
        {
            try
            {
                if (spScale != null)
                {
                    spScale.DataReceived -= spScale_DataReceived;
                    if (spScale.IsOpen) spScale.Close();
                    spScale.Dispose();
                }
            }
            catch { }
        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _isClosing = true;
            DetachScale();
            base.OnFormClosing(e);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _isClosing = true;
            DetachScale();
            Close();
        }


        private void guna2GroupBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2GroupBox2_Click(object sender, EventArgs e)
        {

        }

        private void guna2GroupBox3_Click(object sender, EventArgs e)
        {

        }


        private void SelectInCombo(Guna2ComboBox cbb, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (cbb.Items.Count > 0) cbb.SelectedIndex = 0;
                return;
            }

            int found = -1;
            for (int i = 0; i < cbb.Items.Count; i++)
            {
                if (string.Equals(cbb.Items[i]?.ToString(), value, StringComparison.OrdinalIgnoreCase))
                {
                    found = i;
                    break;
                }
            }

            if (found >= 0)
            {
                cbb.SelectedIndex = found;
            }
            else
            {
                cbb.Items.Add(value);
                cbb.SelectedIndex = cbb.Items.Count - 1;
            }
        }
        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var row = dgvList.Rows[e.RowIndex];
            string orderNumber = row.Cells["cl_orNum"].Value?.ToString();
            if (string.IsNullOrWhiteSpace(orderNumber)) return;

            var orderRepo = new OrderDb();
            var order = orderRepo.getOrderByOrderNumberOrId(orderNumber);
            if (order == null) return;

            var customer = _customerList.FirstOrDefault(c => decryptData(c.CustomerName) == order.CustomerName);
            var product = _productList.FirstOrDefault(p => decryptData(p.ProductName) == order.ProductName);

            SelectInCombo(cbbCusCode, customer != null ? decryptData(customer.CustomerCode) : "");
            SelectInCombo(cbbCusName, order.CustomerName ?? "");

            SelectInCombo(cbbPrdCode, product != null ? decryptData(product.ProductCode) : "");
            SelectInCombo(cbbPrdName, order.ProductName ?? "");

            txtLicense.Text = order.LicensePlate ?? "";
            txtNote.Text = order.Note ?? "";
            lblWeight.Text = row.Cells["cl_wg"].Value?.ToString() ?? "0";

            btnSave.Enabled = true;
            btnStausWeight.Text = "READY TO WEIGH OUT";
        }

    }
}
