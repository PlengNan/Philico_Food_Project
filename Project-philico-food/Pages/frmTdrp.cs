using Project_philico_food.Db;
using Project_philico_food.functions;
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
    public partial class frmTdrp : Form
    {
        private readonly ReportDb _repo;

        private int _pageSize = 10;
        private int _currentPage = 1;
        private int _totalRows = 0;
        private ReportFilter _filter = new ReportFilter();

        public frmTdrp()
        {
            InitializeComponent();

            _repo = new ReportDb();
            SetupGridBindings();

            LoadReport();
        }

        private void frmTdrp_Load(object sender, EventArgs e)
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
            cl_wg.DataPropertyName = "WeightIn";
            cl_wgO.DataPropertyName = "WeightOut";
            cl_nw.DataPropertyName = "NetWeight";
            cl_status.DataPropertyName = "Status";
        }
        private void LoadReport()
        {
            _totalRows = _repo.GetSumTotal(_filter);
            _currentPage = 1;
            LoadPage(_currentPage);
   
        }

        private void LoadPage(int page)
        {
            var dt = _repo.GetTodayReportPage(_filter, page, _pageSize);
            var aes = new AESEncryption();
            foreach (DataRow r in dt.Rows)
            {

                var lp = aes.Decrypt(r["LicensePlate"]?.ToString() ?? "");
                var cn = aes.Decrypt(r["CustomerName"]?.ToString() ?? "");
                var pn = aes.Decrypt(r["ProductName"]?.ToString() ?? "");

                r["LicensePlate"] = lp ?? "";
                r["CustomerName"] = cn ?? "";
                r["ProductName"] = pn ?? "";

            }

            dgvList.DataSource = dt;
            _currentPage = page;
            UpdateButtonState();

            var totalToday = _repo.GetSumTotal(_filter);
            var totalNetToday = _repo.GetSumNetWeightToday(_filter);

            btnTt.Text = $"Total {totalToday:n0} Trips";
            btnTtW.Text = $"Total Weight {totalNetToday:n0} kg";
        }
        private void UpdateButtonState()
        {
            btnBack.Enabled = _currentPage > 1;
            btnNext.Enabled = (_currentPage * _pageSize) < _totalRows;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (_currentPage > 1)
                LoadPage(_currentPage - 1);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if ((_currentPage * _pageSize) < _totalRows)
                LoadPage(_currentPage + 1);
        }

        private void btnTt_Click(object sender, EventArgs e)
        {

        }

        private void btnTtW_Click(object sender, EventArgs e)
        {

        }
    }
}
