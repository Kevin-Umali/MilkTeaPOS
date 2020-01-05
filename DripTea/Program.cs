using DripTea.ViewForm;
using DripTea.ViewForm.AdminForm;
using DripTea.ViewForm.POSForm;
using System;
using System.Windows.Forms;

namespace DripTea
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (new AppSingleInstance(1000)) //1000ms timeout on global lock
            {
                //Only 1 of these runs at a time
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new LoginForm());
            }

        }
    }
}
