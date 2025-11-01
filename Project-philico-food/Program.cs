using Project_philico_food.Pages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Project_philico_food
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var en = new CultureInfo("en-US");
            en.DateTimeFormat.Calendar = new GregorianCalendar();

            CultureInfo.DefaultThreadCurrentCulture = en;
            CultureInfo.DefaultThreadCurrentUICulture = en;

            Thread.CurrentThread.CurrentCulture = en;
            Thread.CurrentThread.CurrentUICulture = en;


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmLogin());
        }
    }
}
