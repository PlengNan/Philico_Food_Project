using Project_philico_food.Db;
using Project_philico_food.Models;
using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;
using Project_philico_food.functions;


namespace Project_philico_food.Pages
{
    public partial class frmReport : Form
    {
        private readonly ReportDb _repo;

        private int _pageSize = 10;
        private int _currentPage = 1;
        private int _totalRows = 0;
        private ReportFilter _filter = new ReportFilter();

        public frmReport()
        {
            InitializeComponent();

            _repo = new ReportDb();
            SetupGridBindings();

            LoadReport();
        }

        private void frmReport_Load(object sender, EventArgs e)
        {
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
            btnSc.Text = $"Success : {c.Success:N0}";
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
            txbOrNum.Clear();
            txbCarId.Clear();
            txbDate.Clear();

        }

        private void dgvList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        } 
        
        private void btnSerach_Click(object sender, EventArgs e)
        {
            var aes = new AESEncryption();
            string ord = txbOrNum.Text?.Trim();

            string plateEnc = string.IsNullOrWhiteSpace(txbCarId.Text)
                ? null
                : aes.Encrypt(txbCarId.Text.Trim());

            string dateEnc = null;
            var dateRaw = txbDate.Text?.Trim();
            if (!string.IsNullOrWhiteSpace(dateRaw) && dateRaw.Length == 10)
                dateEnc = aes.Encrypt(dateRaw);

            _filter = new ReportFilter
            {
                OrderNumber = ord,    
                LicensePlate = plateEnc,  
                DatePrefix = dateEnc      
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

    }
}
