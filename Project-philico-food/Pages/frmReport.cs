using Guna.UI2.WinForms;
using Project_philico_food.Db;
using Project_philico_food.functions;
using Project_philico_food.Models;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;


namespace Project_philico_food.Pages
{
    public partial class frmReport : Form
    {

        private readonly ReportDb _repo;

        private int _pageSize = 10;
        private int _currentPage = 1;
        private int _totalRows = 0;
        private ReportFilter _filter = new ReportFilter();
        private class dropdownItem
        {
            public string Text { get; set; }   
            public string Value { get; set; }  
        }
        private void getCombo(Guna2ComboBox combo, string column)
        {
            var aes = new AESEncryption();
            var list = new List<dropdownItem>
            {
                new dropdownItem { Text = " ", Value = null }
            };

            var encs = _repo.GetDistinctEncrypted(column); 
            foreach (var enc in encs)
            {
                string dec;
                try { dec = aes.Decrypt(enc); } catch { dec = enc; } 
                list.Add(new dropdownItem { Text = dec, Value = enc });
            }

            combo.DisplayMember = "Text";
            combo.ValueMember = "Value";
            combo.DataSource = list;
            combo.SelectedIndex = 0;
            combo.DropDownStyle = ComboBoxStyle.DropDownList; 
        }

        public frmReport()
        {

            InitializeComponent();
            this.Load += frmReport_Load;

            _repo = new ReportDb();
            SetupGridBindings();

            _filter = new ReportFilter
            {
                DateList = BuildLast()
            };

            LoadReport();
        }

        private void DisableAllFilters()
        {
            cbbCarId.Enabled = false;
            cbbCus.Enabled = false;
            cbbProduct.Enabled = false;
            dtpFrom.Enabled = false;
            dtpTo.Enabled = false;

        }

        private bool _didInit7Days = false;
        private List<string> BuildLast()
        {
            var aes = new AESEncryption();
            var en = new System.Globalization.CultureInfo("en-US");
            var list = new List<string>();
            for (int i = 0; i <= 7; i++)
            {
                var d = DateTime.Today.AddDays(-i).ToString("yyyy-MM-dd", en);
                list.Add(aes.Encrypt(d));
            }
            return list;
        }

        private void frmReport_Load(object sender, EventArgs e)
        {
            chbCarId.Checked = false;
            chbCus.Checked = false;
            chbProduct.Checked = false;
            chbDate.Checked = false;

            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            dtpTo.ValueChanged += dtpTo_ValueChanged;

            DisableAllFilters();


            cbbCarId.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbCus.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbProduct.DropDownStyle = ComboBoxStyle.DropDownList;

            if (dgvList.DataSource == null) LoadReport();
        }

        private void SetupGridBindings()
        {
            dgvList.AutoGenerateColumns = false;
            cl_orNum.DataPropertyName = "OrderNumber";
            cl_lcP.DataPropertyName = "LicensePlate";
            cl_customerName.DataPropertyName = "CustomerName";
            cl_productName.DataPropertyName = "ProductName";
            cl_Datez.DataPropertyName = "Datez";
            cl_wg.DataPropertyName = "WeightIn";
            cl_wgO.DataPropertyName = "WeightOut";
            cl_status.DataPropertyName = "Status";
        }


        private void LoadReport()
        {
            _totalRows = _repo.CountOrders(_filter);
            _currentPage = 1;
            LoadPage(_currentPage);
            LoadStatusCounters();
        }

        private void LoadPage(int page)
        {
            var dt = _repo.GetReportPage(_filter, page, _pageSize);
            var aes = new AESEncryption();
            foreach (DataRow r in dt.Rows)
            {
               
                var lp = aes.Decrypt(r["LicensePlate"]?.ToString() ?? "");
                var cn = aes.Decrypt(r["CustomerName"]?.ToString() ?? "");
                var pn = aes.Decrypt(r["ProductName"]?.ToString() ?? "");
                var dz = aes.Decrypt(r["Datez"]?.ToString() ?? "");

                r["LicensePlate"] = lp ?? "";
                r["CustomerName"] = cn ?? "";
                r["ProductName"] = pn ?? "";
                r["Datez"] = dz ?? "";
            
            }

            dgvList.DataSource = dt;
            _currentPage = page;
            UpdateButtonState();

            LoadStatusCounters();
        }


        private void LoadStatusCounters()
        {
            var c = _repo.GetStatusProcess(_filter);
            btnSc.Text = $"Completed : {c.Success:N0}";
            btnPc.Text = $"Process : {c.Process:N0}";
            btnCc.Text = $"Cancle : {c.Cancle:N0}";

        }

        private void UpdateButtonState()
        {
            btnBack.Enabled = _currentPage > 1;
            btnNext.Enabled = (_currentPage * _pageSize) < _totalRows;
        }


        void clearText()
        {
            
            txbCarId.Clear();
            txbDate.Clear();

        }

        private void dgvList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvList.Columns[e.ColumnIndex].Name != "cl_print") return;

            string orderNumber = dgvList.Rows[e.RowIndex].Cells["cl_orNum"]?.Value?.ToString();
            if (string.IsNullOrWhiteSpace(orderNumber))
            {
                msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                msg.Show("Order number not found.");
                return;
            }

            try
            {
                var orderRepo = new OrderDb();
                int currentNo = orderRepo.getCurrentPrintNo(orderNumber);
                int previewNo = currentNo + 1;


                var printModel = new OrderDetailModel
                {
                    OrderNumber = orderNumber,
                    PrintNo = previewNo
                };


                using (var f = new frmPrintRp(printModel))
                {
                    f.StartPosition = FormStartPosition.CenterParent;
                    f.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                msg.Show("Printing failed: " + ex.Message);
            }

        } 
        
        private void btnSerach_Click(object sender, EventArgs e)
        {
            var aes = new AESEncryption();
            var en = new System.Globalization.CultureInfo("en-US");

            string plateEnc = string.IsNullOrWhiteSpace(cbbCarId.Text)
                ? null
                : aes.Encrypt(cbbCarId.Text.Trim());

            string cusEnc = string.IsNullOrWhiteSpace(cbbCus.Text)
                ? null
                : aes.Encrypt(cbbCus.Text.Trim());

            string proEnc = string.IsNullOrWhiteSpace(cbbProduct.Text)
                ? null
                : aes.Encrypt(cbbProduct.Text.Trim());

            List<string> dateListEnc = null;
            if (chbDate.Checked)
            {
                dateListEnc = new List<string>();
                var d = dtpFrom.Value.Date;
                var end = dtpTo.Value.Date;
                while (d <= end)
                {
                    var plain = d.ToString("yyyy-MM-dd", en); 
                    dateListEnc.Add(aes.Encrypt(plain));
                    d = d.AddDays(1);
                }
            }

            _filter = new ReportFilter
            {
                LicensePlate = plateEnc,
                CustomerName = cusEnc,
                ProductName = proEnc,
                DatePrefix = null,       
                DateFrom = null,       
                DateTo = null,       
                DateList = dateListEnc  
            };

            LoadReport();
            clearText();
        }


        private void btnNext_Click(object sender, EventArgs e)
        {

            if ((_currentPage * _pageSize) < _totalRows)
                LoadPage(_currentPage + 1);
        }

       

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (_currentPage > 1)
                LoadPage(_currentPage - 1);
        }

       

        private void cbbCarId_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void chbProduct_CheckedChanged(object sender, EventArgs e)
        {
            Guna2CheckBox  cb = sender as Guna2CheckBox;
            if (cb.Checked)
            {
                switch (cb.Tag)
                {
                    case "car":
                        cbbCarId.Enabled = true;
                        break;
                    case "customer":
                        cbbCus.Enabled = true;
                        break;
                    case "product":
                        cbbProduct.Enabled = true;
                        break;
                    case "date":
                        dtpFrom.Enabled = true;
                        dtpTo.Enabled = true;
                        dtpFrom.CustomFormat = "dd/MM/yyyy";
                        dtpTo.CustomFormat = "dd/MM/yyyy";

                        
                        dtpFrom.Value = DateTime.Today;
                        dtpTo.Value = DateTime.Today;
                        dtpTo.MinDate = dtpFrom.Value.Date;
                        dtpFrom.MaxDate = DateTimePicker.MaximumDateTime;
                        break;
                }
            }
            else
            {
                switch (cb.Tag)
                {
                    case "car":
                        cbbCarId.Enabled = false;

                        if (cbbCarId.DataSource != null)
                        {
                           
                            if (cbbCarId.Items.Count > 0)
                                cbbCarId.SelectedIndex = 0;    
                            else
                                cbbCarId.SelectedIndex = -1;   
                        }
                        else
                        {
                            cbbCarId.SelectedItem = null;    
                            cbbCarId.SelectedIndex = -1;
                            
                        }
                        break;

                    case "customer":
                        cbbCus.Enabled = false;
                        if (cbbCus.DataSource != null)
                        {
                            if (cbbCus.Items.Count > 0) cbbCus.SelectedIndex = 0;
                            else cbbCus.SelectedIndex = -1;
                        }
                        else
                        {
                            cbbCus.SelectedItem = null;
                            cbbCus.SelectedIndex = -1;
                        }
                        break;

                    case "product":
                        cbbProduct.Enabled = false;
                        if (cbbProduct.DataSource != null)
                        {
                            if (cbbProduct.Items.Count > 0) cbbProduct.SelectedIndex = 0;
                            else cbbProduct.SelectedIndex = -1;
                        }
                        else
                        {
                            cbbProduct.SelectedItem = null;
                            cbbProduct.SelectedIndex = -1;
                        }
                        break;

                    case "date":
                            dtpFrom.Enabled = false;
                            dtpTo.Enabled = false;

                            
                            dtpFrom.CustomFormat = " ";
                            dtpTo.CustomFormat = " ";
                        
                        break;

                }
            }
        }
        private void cbbCarId_DropDown(object sender, EventArgs e)
        {
            getCombo(cbbCarId, "LicensePlate");
        }
        private void cbbCus_DropDown(object sender, EventArgs e)
        {
            getCombo(cbbCus, "CustomerName");
        }

        private void cbbProduct_DropDown(object sender, EventArgs e)
        {
            getCombo(cbbProduct, "ProductName");
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            dtpTo.MinDate = dtpFrom.Value.Date;

            if (dtpTo.Value.Date < dtpFrom.Value.Date)
                dtpTo.Value = dtpFrom.Value.Date;
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            if (dtpTo.Value.Date < dtpFrom.Value.Date)
                dtpTo.Value = dtpFrom.Value.Date;
        }
    }
}
