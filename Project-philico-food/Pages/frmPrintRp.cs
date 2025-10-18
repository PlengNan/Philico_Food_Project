using Microsoft.Reporting.WinForms;
using Microsoft.VisualBasic.Logging;
using Project_philico_food.Db;
using Project_philico_food.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            reportViewer1.LocalReport.ReportEmbeddedResource = "TicketReport.rdlc";
            ReportDb finish_Weight = new ReportDb();
            //string ord = _orderDetailModel?.OrderNumber ?? "";
            //DataTable tb = finish_Weight.SelectQueue_no(_weightModel.queue_no);
            DataTable tb = finish_Weight.GetTicketTable(_orderDetailModel.OrderNumber);

            var any = tb.Rows[0];
            //string orderNumber = any["OrderNumber"]?.ToString() ?? "";
            //string licensePlate = any["LicensePlate"]?.ToString() ?? "";
            //string customerName = any["CustomerName"]?.ToString() ?? "";
            //string productName = any["ProductName"]?.ToString() ?? "";
            //string netWeight = any["NetWeight"] == DBNull.Value ? "0"
            //                      : Convert.ToDecimal(any["NetWeight"]).ToString("N0", CultureInfo.InvariantCulture);
            //string note = tb.Columns.Contains("Note") ? (any["Note"]?.ToString() ?? "") : "";


            //DataRow inRow = tb.AsEnumerable().FirstOrDefault(r => (r["WeightType"]?.ToString() ?? "") == "IN");
            //DataRow outRow = tb.AsEnumerable().FirstOrDefault(r => (r["WeightType"]?.ToString() ?? "") == "OUT");


            //string detailIn = inRow?["Detail"]?.ToString() ?? "Weight In";
            //string dateIn = inRow?["Datez"]?.ToString() ?? "";
            //string timeIn = inRow?["Timez"]?.ToString() ?? "";
            //string weightIn = inRow == null ? "0"
            //                 : Convert.ToDecimal(inRow["WeightValue"]).ToString("N0", CultureInfo.InvariantCulture);


            //string detailOut = outRow?["Detail"]?.ToString() ?? "Weight Out";
            //string dateOut = outRow?["Datez"]?.ToString() ?? "";
            //string timeOut = outRow?["Timez"]?.ToString() ?? "";
            //string weightOut = outRow == null ? "0"
            //                  : Convert.ToDecimal(outRow["WeightValue"]).ToString("N0", CultureInfo.InvariantCulture);

            //reportViewer1.LocalReport.SetParameters(new[]
            //{
            //    new ReportParameter("OrderNumber",  any["OrderNumber"]?.ToString() ?? ""),
            //    new ReportParameter("LicensePlate", any["Car ID"]?.ToString() ?? ""),
            //    new ReportParameter("CustomerName", any["CustomerName"]?.ToString() ?? ""),
            //    new ReportParameter("ProductName",  any["ProductName"]?.ToString() ?? ""),
            //    new ReportParameter("NetWeight",
            //        any["NetWeight"] == DBNull.Value ? "0" :
            //        Convert.ToDecimal(any["NetWeight"]).ToString("N0", CultureInfo.InvariantCulture)),
            //    new ReportParameter("Note",      dt.Columns.Contains("Note") ? (any["Note"]?.ToString() ?? "") : ""),
            //    new ReportParameter("PrintNo",   (_orderDetailModel?.PrintNo ?? 1).ToString()),
            //    new ReportParameter("PrintedAt", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")),
            //});


            //reportViewer1.LocalReport.ReportEmbeddedResource = "TicketReport.rdlc";
            //ReportDb finish_Weight = new ReportDb();
            ////string ord = _orderDetailModel?.OrderNumber ?? "";
            ////DataTable tb = finish_Weight.SelectQueue_no(_weightModel.queue_no);
            //DataTable tb = finish_Weight.GetTicketTable(_orderDetailModel.OrderNumber);

            foreach (DataRow rw in tb.Rows)
            {
                ReportParameter OrderNumber = new ReportParameter("OrderNumber", rw["OrderNumber"].ToString());
                DateTime dt = DateTime.Parse(rw["DateIn"].ToString());
                ReportParameter DateIn = new ReportParameter("DateIn",dt.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en-US")));
                ReportParameter TimeIn = new ReportParameter("TimeIn", dt.ToString("HH:mm:ss", CultureInfo.CreateSpecificCulture("en-US")));

                string weightType = rw["WeightType"]?.ToString();
                string detailText = weightType == "IN" ? "Weight In" :
                        weightType == "OUT" ? "Weight Out" : "";
                rw["Detail"] = detailText;
                ReportParameter detailParam = new ReportParameter("Detail", detailText);
                ReportParameter LicensePlate = new ReportParameter("LicensePlate", rw["Car ID"].ToString());


            }

            //var repo = new ReportDb();
            //string ord = _orderDetailModel?.OrderNumber ?? "";



            //DataRow inRow = dt.AsEnumerable().FirstOrDefault(r => (r["WeightType"]?.ToString() ?? "") == "IN");
            //DataRow outRow = dt.AsEnumerable().FirstOrDefault(r => (r["WeightType"]?.ToString() ?? "") == "OUT");
            //if (inRow == null && outRow == null)
            //    throw new Exception();

            //string orderNumber = (inRow ?? outRow)["OrderNumber"]?.ToString() ?? "";
            //string licensePlate = (inRow ?? outRow)["LicensePlate"]?.ToString() ?? "";
            //string customerName = (inRow ?? outRow)["CustomerName"]?.ToString() ?? "";
            //string productName = (inRow ?? outRow)["ProductName"]?.ToString() ?? "";
            //string netWeight = (inRow ?? outRow)["NetWeight"]?.ToString() ?? "0";

            //string dateIn = inRow?["Datez"]?.ToString() ?? "";
            //string timeIn = inRow?["Timez"]?.ToString() ?? "";
            //string wIn = inRow?["WeightIn"]?.ToString() ?? "0";

            //string dateOut = outRow?["Datez"]?.ToString() ?? "";
            //string timeOut = outRow?["Timez"]?.ToString() ?? "";
            //string wOut = outRow?["WeightOut"]?.ToString() ?? "0";

            //string note = (inRow ?? outRow).Table.Columns.Contains("Note")
            //                  ? (inRow ?? outRow)["Note"]?.ToString() ?? ""
            //                  : "";

            //        var ps = new[]
            //        {
            //    new ReportParameter("PrintedAt", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")),
            //    new ReportParameter("PrintNo",   (_orderDetailModel?.PrintNo ?? 1).ToString()),

            //    new ReportParameter("OrderNumber",  orderNumber),
            //    new ReportParameter("LicensePlate", licensePlate),
            //    new ReportParameter("CustomerName", customerName),
            //    new ReportParameter("ProductName",  productName),

            //    new ReportParameter("DateIn",   dateIn),
            //    new ReportParameter("TimeIn",   timeIn),
            //    new ReportParameter("WeightIn", wIn),

            //    new ReportParameter("DateOut",   dateOut),
            //    new ReportParameter("TimeOut",   timeOut),
            //    new ReportParameter("WeightOut", wOut),

            //    new ReportParameter("NetWeight", netWeight),
            //    new ReportParameter("Note",    note),
            //};
            reportViewer1.LocalReport.SetParameters(ps);

                reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.PageWidth;
                reportViewer1.RefreshReport();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
