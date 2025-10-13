namespace Project_philico_food.Pages
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.btnCus = new Guna.UI2.WinForms.Guna2Button();
            this.btnPro = new Guna.UI2.WinForms.Guna2Button();
            this.btnWeight = new Guna.UI2.WinForms.Guna2Button();
            this.btnSetting = new Guna.UI2.WinForms.Guna2Button();
            this.sqliteCommand1 = new Microsoft.Data.Sqlite.SqliteCommand();
            this.btnUsers = new Guna.UI2.WinForms.Guna2Button();
            this.btnTodayReport = new Guna.UI2.WinForms.Guna2Button();
            this.btnReports = new Guna.UI2.WinForms.Guna2Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCus
            // 
            this.btnCus.Animated = true;
            this.btnCus.AnimatedGIF = true;
            this.btnCus.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(93)))), ((int)(((byte)(151)))));
            this.btnCus.BorderRadius = 4;
            this.btnCus.BorderThickness = 1;
            this.btnCus.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCus.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCus.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnCus.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnCus.FillColor = System.Drawing.Color.White;
            this.btnCus.Font = new System.Drawing.Font("Athiti", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(93)))), ((int)(((byte)(151)))));
            this.btnCus.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(93)))), ((int)(((byte)(151)))));
            this.btnCus.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnCus.Image = ((System.Drawing.Image)(resources.GetObject("btnCus.Image")));
            this.btnCus.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnCus.ImageSize = new System.Drawing.Size(40, 40);
            this.btnCus.Location = new System.Drawing.Point(3, 3);
            this.btnCus.Name = "btnCus";
            this.btnCus.Size = new System.Drawing.Size(169, 45);
            this.btnCus.TabIndex = 19;
            this.btnCus.Text = "Customer Info";
            this.btnCus.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnCus.Click += new System.EventHandler(this.btnCus_Click);
            // 
            // btnPro
            // 
            this.btnPro.Animated = true;
            this.btnPro.AnimatedGIF = true;
            this.btnPro.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(93)))), ((int)(((byte)(151)))));
            this.btnPro.BorderRadius = 4;
            this.btnPro.BorderThickness = 1;
            this.btnPro.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnPro.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnPro.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnPro.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnPro.FillColor = System.Drawing.Color.White;
            this.btnPro.Font = new System.Drawing.Font("Athiti", 12F);
            this.btnPro.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(93)))), ((int)(((byte)(151)))));
            this.btnPro.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(93)))), ((int)(((byte)(151)))));
            this.btnPro.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnPro.Image = ((System.Drawing.Image)(resources.GetObject("btnPro.Image")));
            this.btnPro.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnPro.ImageSize = new System.Drawing.Size(40, 40);
            this.btnPro.Location = new System.Drawing.Point(178, 3);
            this.btnPro.Name = "btnPro";
            this.btnPro.Size = new System.Drawing.Size(169, 45);
            this.btnPro.TabIndex = 20;
            this.btnPro.Text = "Product Info";
            this.btnPro.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnPro.Click += new System.EventHandler(this.btnPro_Click);
            // 
            // btnWeight
            // 
            this.btnWeight.Animated = true;
            this.btnWeight.AnimatedGIF = true;
            this.btnWeight.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(93)))), ((int)(((byte)(151)))));
            this.btnWeight.BorderRadius = 4;
            this.btnWeight.BorderThickness = 1;
            this.btnWeight.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnWeight.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnWeight.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnWeight.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnWeight.FillColor = System.Drawing.Color.White;
            this.btnWeight.Font = new System.Drawing.Font("Athiti", 12F);
            this.btnWeight.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(93)))), ((int)(((byte)(151)))));
            this.btnWeight.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(93)))), ((int)(((byte)(151)))));
            this.btnWeight.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnWeight.Image = ((System.Drawing.Image)(resources.GetObject("btnWeight.Image")));
            this.btnWeight.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnWeight.ImageSize = new System.Drawing.Size(40, 40);
            this.btnWeight.Location = new System.Drawing.Point(528, 3);
            this.btnWeight.Name = "btnWeight";
            this.btnWeight.Size = new System.Drawing.Size(169, 45);
            this.btnWeight.TabIndex = 21;
            this.btnWeight.Text = "Weighing";
            this.btnWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnWeight.Click += new System.EventHandler(this.btnWeight_Click);
            // 
            // btnSetting
            // 
            this.btnSetting.Animated = true;
            this.btnSetting.AnimatedGIF = true;
            this.btnSetting.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(93)))), ((int)(((byte)(151)))));
            this.btnSetting.BorderRadius = 4;
            this.btnSetting.BorderThickness = 1;
            this.btnSetting.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSetting.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSetting.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnSetting.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnSetting.FillColor = System.Drawing.Color.White;
            this.btnSetting.Font = new System.Drawing.Font("Athiti", 12F);
            this.btnSetting.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(93)))), ((int)(((byte)(151)))));
            this.btnSetting.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(93)))), ((int)(((byte)(151)))));
            this.btnSetting.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnSetting.Image = ((System.Drawing.Image)(resources.GetObject("btnSetting.Image")));
            this.btnSetting.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnSetting.ImageSize = new System.Drawing.Size(40, 40);
            this.btnSetting.Location = new System.Drawing.Point(3, 54);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(169, 45);
            this.btnSetting.TabIndex = 22;
            this.btnSetting.Text = "Setting";
            this.btnSetting.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click_1);
            // 
            // sqliteCommand1
            // 
            this.sqliteCommand1.CommandTimeout = 30;
            this.sqliteCommand1.Connection = null;
            this.sqliteCommand1.Transaction = null;
            this.sqliteCommand1.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // btnUsers
            // 
            this.btnUsers.Animated = true;
            this.btnUsers.AnimatedGIF = true;
            this.btnUsers.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(93)))), ((int)(((byte)(151)))));
            this.btnUsers.BorderRadius = 4;
            this.btnUsers.BorderThickness = 1;
            this.btnUsers.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnUsers.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnUsers.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnUsers.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnUsers.FillColor = System.Drawing.Color.White;
            this.btnUsers.Font = new System.Drawing.Font("Athiti", 12F);
            this.btnUsers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(93)))), ((int)(((byte)(151)))));
            this.btnUsers.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(93)))), ((int)(((byte)(151)))));
            this.btnUsers.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnUsers.Image = ((System.Drawing.Image)(resources.GetObject("btnUsers.Image")));
            this.btnUsers.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnUsers.ImageSize = new System.Drawing.Size(40, 40);
            this.btnUsers.Location = new System.Drawing.Point(353, 3);
            this.btnUsers.Name = "btnUsers";
            this.btnUsers.Size = new System.Drawing.Size(169, 45);
            this.btnUsers.TabIndex = 23;
            this.btnUsers.Text = "Users Info";
            this.btnUsers.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnUsers.Click += new System.EventHandler(this.btnUsers_Click);
            // 
            // btnTodayReport
            // 
            this.btnTodayReport.Animated = true;
            this.btnTodayReport.AnimatedGIF = true;
            this.btnTodayReport.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(93)))), ((int)(((byte)(151)))));
            this.btnTodayReport.BorderRadius = 4;
            this.btnTodayReport.BorderThickness = 1;
            this.btnTodayReport.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnTodayReport.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnTodayReport.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnTodayReport.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnTodayReport.FillColor = System.Drawing.Color.White;
            this.btnTodayReport.Font = new System.Drawing.Font("Athiti", 12F);
            this.btnTodayReport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(93)))), ((int)(((byte)(151)))));
            this.btnTodayReport.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(93)))), ((int)(((byte)(151)))));
            this.btnTodayReport.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnTodayReport.Image = ((System.Drawing.Image)(resources.GetObject("btnTodayReport.Image")));
            this.btnTodayReport.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnTodayReport.ImageSize = new System.Drawing.Size(40, 40);
            this.btnTodayReport.Location = new System.Drawing.Point(703, 3);
            this.btnTodayReport.Name = "btnTodayReport";
            this.btnTodayReport.Size = new System.Drawing.Size(169, 45);
            this.btnTodayReport.TabIndex = 24;
            this.btnTodayReport.Text = "Today\'s report";
            this.btnTodayReport.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnTodayReport.Click += new System.EventHandler(this.btnTodayReport_Click);
            // 
            // btnReports
            // 
            this.btnReports.Animated = true;
            this.btnReports.AnimatedGIF = true;
            this.btnReports.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(93)))), ((int)(((byte)(151)))));
            this.btnReports.BorderRadius = 4;
            this.btnReports.BorderThickness = 1;
            this.btnReports.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnReports.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnReports.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnReports.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnReports.FillColor = System.Drawing.Color.White;
            this.btnReports.Font = new System.Drawing.Font("Athiti", 12F);
            this.btnReports.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(93)))), ((int)(((byte)(151)))));
            this.btnReports.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(93)))), ((int)(((byte)(151)))));
            this.btnReports.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnReports.Image = ((System.Drawing.Image)(resources.GetObject("btnReports.Image")));
            this.btnReports.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnReports.ImageSize = new System.Drawing.Size(40, 40);
            this.btnReports.Location = new System.Drawing.Point(878, 3);
            this.btnReports.Name = "btnReports";
            this.btnReports.Size = new System.Drawing.Size(169, 45);
            this.btnReports.TabIndex = 25;
            this.btnReports.Text = "Reports";
            this.btnReports.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.btnReports.Click += new System.EventHandler(this.btnReports_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnCus);
            this.flowLayoutPanel1.Controls.Add(this.btnPro);
            this.flowLayoutPanel1.Controls.Add(this.btnUsers);
            this.flowLayoutPanel1.Controls.Add(this.btnWeight);
            this.flowLayoutPanel1.Controls.Add(this.btnTodayReport);
            this.flowLayoutPanel1.Controls.Add(this.btnReports);
            this.flowLayoutPanel1.Controls.Add(this.btnSetting);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1187, 100);
            this.flowLayoutPanel1.TabIndex = 26;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1187, 544);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Font = new System.Drawing.Font("Athiti", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmMain";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Guna.UI2.WinForms.Guna2Button btnCus;
        private Guna.UI2.WinForms.Guna2Button btnPro;
        private Guna.UI2.WinForms.Guna2Button btnWeight;
        private Guna.UI2.WinForms.Guna2Button btnSetting;
        private Microsoft.Data.Sqlite.SqliteCommand sqliteCommand1;
        private Guna.UI2.WinForms.Guna2Button btnUsers;
        private Guna.UI2.WinForms.Guna2Button btnTodayReport;
        private Guna.UI2.WinForms.Guna2Button btnReports;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}