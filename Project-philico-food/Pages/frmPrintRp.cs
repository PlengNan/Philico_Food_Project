
using FastReport;
using FastReport.Preview;
using Microsoft.Reporting.WinForms;
using Project_philico_food.Db;
using Project_philico_food.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Project_philico_food.Pages
{
    public partial class frmPrintRp : Form
    {
        public frmPrintRp(OrderDetailModel orderDetailModel)
        {
            InitializeComponent();
            _orderDetailModel = orderDetailModel;
        }
        private OrderDetailModel _orderDetailModel;

        private void frmPrintRp_Load(object sender, EventArgs e)
        {
            reportViewer1.LocalReport.DataSources.Clear();

            reportViewer1.LocalReport.ReportEmbeddedResource = "Project_philico_food.Report.TicketReport.rdlc";
            var repo = new ReportDb();
            DataTable dt = repo.GetTicketTable(_orderDetailModel.OrderNumber);

            DataRow any = dt.Rows[0];
            string OrderNumber = any["OrderNumber"]?.ToString() ?? "";
            string LicensePlate = any["LicensePlate"]?.ToString() ?? "";
            string CustomerName = any["CustomerName"]?.ToString() ?? "";
            string ProductName = any["ProductName"]?.ToString() ?? "";
            string NetWeight = any["NetWeight"]?.ToString() ?? "0";
            string Note = dt.Columns.Contains("Note") ? (any["Note"]?.ToString() ?? "") : "";
            string PrintNo = (_orderDetailModel?.PrintNo ?? 1).ToString();

            string DateIn = "", TimeIn = "", WeightIn = "0", DateOut = "", TimeOut = "", WeightOut = "0";
            foreach (DataRow r in dt.Rows)
            {
                var wt = r["WeightType"]?.ToString();
                if (string.Equals(wt, "IN", StringComparison.OrdinalIgnoreCase))
                {
                    DateIn = r.Table.Columns.Contains("DateIn") ? r["DateIn"]?.ToString() ?? "" : "";
                    TimeIn = r.Table.Columns.Contains("TimeIn") ? r["TimeIn"]?.ToString() ?? "" : "";
                    WeightIn = r.Table.Columns.Contains("WeightIn") ? r["WeightIn"]?.ToString() ?? "0" : "0";
                }
                else if (string.Equals(wt, "OUT", StringComparison.OrdinalIgnoreCase))
                {
                    DateOut = r.Table.Columns.Contains("DateOut") ? r["DateOut"]?.ToString() ?? "" : "";
                    TimeOut = r.Table.Columns.Contains("TimeOut") ? r["TimeOut"]?.ToString() ?? "" : "";
                    WeightOut = r.Table.Columns.Contains("WeightOut") ? r["WeightOut"]?.ToString() ?? "0" : "0";
                }
            }

            var prms = new[]
            {
                new ReportParameter("OrderNumber",  OrderNumber),
                new ReportParameter("LicensePlate", LicensePlate),
                new ReportParameter("CustomerName", CustomerName),
                new ReportParameter("ProductName",  ProductName),
                new ReportParameter("NetWeight",    NetWeight),
                new ReportParameter("Note",         Note),
                new ReportParameter("PrintNo",      PrintNo),

                new ReportParameter("DateIn",   DateIn),
                new ReportParameter("TimeIn",   TimeIn),
                new ReportParameter("WeightIn", WeightIn),

                new ReportParameter("DateOut",   DateOut),
                new ReportParameter("TimeOut",   TimeOut),
                new ReportParameter("WeightOut", WeightOut),
            };

            reportViewer1.LocalReport.SetParameters(prms);

            reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);

            reportViewer1.RefreshReport();
        }

        private void reportViewer1_PrintingBegin(object sender, Microsoft.Reporting.WinForms.ReportPrintEventArgs e)
        {
            try
            {

                var orderRepo = new OrderDb();
                int updatedNo = orderRepo.getPrintNo(_orderDetailModel.OrderNumber);
            }
            catch (Exception ex)
            {
                msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                msg.Show("Printing failed: " + ex.Message);

            }
        }

     
    }
}
