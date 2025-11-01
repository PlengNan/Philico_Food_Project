namespace Project_philico_food.Pages
{
    partial class frmPrintRp
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPrintRp));
            this.report1 = new FastReport.Report();
            this.msg = new Guna.UI2.WinForms.Guna2MessageDialog();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            ((System.ComponentModel.ISupportInitialize)(this.report1)).BeginInit();
            this.SuspendLayout();
            // 
            // report1
            // 
            this.report1.ReportResourceString = resources.GetString("report1.ReportResourceString");
            // 
            // msg
            // 
            this.msg.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
            this.msg.Caption = null;
            this.msg.Icon = Guna.UI2.WinForms.MessageDialogIcon.None;
            this.msg.Parent = null;
            this.msg.Style = Guna.UI2.WinForms.MessageDialogStyle.Light;
            this.msg.Text = null;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportViewer1.Location = new System.Drawing.Point(2, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(849, 557);
            this.reportViewer1.TabIndex = 2;
            this.reportViewer1.PrintingBegin += new Microsoft.Reporting.WinForms.ReportPrintEventHandler(this.reportViewer1_PrintingBegin);
            // 
            // frmPrintRp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 569);
            this.Controls.Add(this.reportViewer1);
            this.Name = "frmPrintRp";
            this.Text = "frmPrintRp";
            this.Load += new System.EventHandler(this.frmPrintRp_Load);
            ((System.ComponentModel.ISupportInitialize)(this.report1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private FastReport.Report report1;
        private Guna.UI2.WinForms.Guna2MessageDialog msg;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
    }
}