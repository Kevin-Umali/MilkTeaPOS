using System;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DripTea.ViewForm
{
    public partial class SplashScreen : Form
    {
        protected string Apppath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase.ToString());
        protected string conn = "Data source=driptea.db;Version=3;New=False;Compress=True";
        public SplashScreen()
        {
            InitializeComponent();
            Guna.UI.Lib.GraphicsHelper.ShadowForm(this);
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {
            timer2.Start();
            SoftBlink(progressbar1, Color.Plum, Color.Black, 2000, true);
        }

        private void CheckDBFile()
        {
            if (File.Exists("driptea.db"))
            {
                progressbar1.Value = 105;
                OpenForm();
            }
            else
            {
                if(File.Exists(Apppath + @"\driptea.db"))
                {
                    progressbar1.Value = 105;
                    OpenForm();
                }
                else
                {
                    LabelText("Creating DripTea Database....");
                    CreateTable(1); 
                    CreateTable(2); 
                    CreateTable(3); 
                    CreateTable(4);
                }
            }
        }

        private void OpenForm()
        {
            this.Hide();
            LoginForm f1 = new LoginForm();
            f1.Closed += (s, args) => this.Close();
            f1.Show();
        }
        private async void LabelText(string value)
        {
            await Task.Delay(1000);
            lblloading.Text = value;
            progressbar1.Value += 5;
        }        
        private void CreateTable(int i)
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    LabelText("Openning DripTea Database....");
                    string query = string.Empty;
                    switch (i)
                    {
                        case 1:
                            LabelText("Creating Product Table....");
                            query = "CREATE TABLE Product(ID INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE " +
                                "NOT NULL, ProductName VARCHAR UNIQUE NOT NULL, Description VARCHAR NOT NULL, " +
                                "ProductGroup VARCHAR NOT NULL, MiscItem VARCHAR NOT NULL, " +
                                "LDPrice DECIMAL NOT NULL, HDPrice DECIMAL NOT NULL, ODPrice DECIMAL NOT NULL, " +
                                "Availability BOOLEAN NOT NULL); ";
                            break;
                        case 2:
                            LabelText("Creating Miscellaneous Table....");
                            query = "CREATE TABLE Misc (ID INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL, " +
                                "MiscName VARCHAR UNIQUE NOT NULL, Stocks BIGINT NOT NULL, " +
                                "Availability BOOLEAN NOT NULL );";
                            break;
                        case 3:
                            LabelText("Creating Group of Products Table....");
                            query = "CREATE TABLE ProductsGroup (ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE, " +
                                "GroupName UNIQUE NOT NULL );";
                            break;
                        case 4:
                            LabelText("Creating Account Table....");
                            query = "CREATE TABLE Users (UsersID INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL, " +
                                "Name VARCHAR NOT NULL, Username VARCHAR NOT NULL UNIQUE, Password VARCHAR NOT NULL, " +
                                "UserType VARCHAR NOT NULL, Status BOOLEAN NOT NULL );";
                            break;
                        default:
                            LabelText("Failed...");
                            break;
                    }
                     

                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        try
                        {
                            sqlcmd.ExecuteNonQuery();
                        }
                        finally
                        {
                            if (sqlcon.State == ConnectionState.Open)
                            {
                                sqlcon.Close();
                                LabelText("Successfully Creating Account Table....");

                                switch (i)
                                {
                                    case 1:
                                        LabelText("Successfully Creating Product Table....");
                                        break;
                                    case 2:
                                        LabelText("Successfully Creating Misc. Table....");
                                        break;
                                    case 3:
                                        LabelText("Successfully Creating Group Product Table....");
                                        break;
                                    case 4:
                                        LabelText("Successfully Creating Account Table....");
                                        AddAccount(1); AddAccount(2);
                                        break;
                                    default:
                                        LabelText("Failed...");
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddAccount(int i)
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = string.Empty;
                    switch (i)
                    {
                        case 1:
                            LabelText("Creating Admin Account....");
                            query = "INSERT INTO Users VALUES (null,'Admin','Admin','Admin','Administrator',True)";
                            break;
                        case 2:
                            LabelText("Creating Cashier Account....");
                            query = "INSERT INTO Users VALUES (null,'Cashier','Cashier','Cashier','Cashier',True)";
                            break;
                        default:
                            LabelText("Failed...");
                            break;
                    }
                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        try
                        {
                            sqlcmd.ExecuteNonQuery();
                        }
                        finally
                        {
                            if (sqlcon.State == ConnectionState.Open)
                            {
                                sqlcon.Close();
                                switch (i)
                                {
                                    case 1:
                                        LabelText("Successfully Creating Admin Account....");
                                        break;
                                    case 2:
                                        LabelText("Successfully Creating Cashier Account....");

                                        LabelText("Done....");
                                        break;
                                    default:
                                        LabelText("Failed...");
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bunifuCircleProgressbar1_ProgressChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (lblloading.Text == "Done....")
            {
                timer1.Stop();
                lblloading.Text = "Loading....";
                timer3.Start();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Stop();
            CheckDBFile();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            timer3.Stop();
            OpenForm();
        }

        private async void SoftBlink(Bunifu.Framework.UI.BunifuCircleProgressbar ctrl, Color c1, Color c2, short CycleTime_ms, bool BkClr)
        {
            var sw = new Stopwatch(); sw.Start();
            short halfCycle = (short)Math.Round(CycleTime_ms * 0.5);
            while (true)
            {
                await Task.Delay(1);
                var n = sw.ElapsedMilliseconds % CycleTime_ms;
                var per = (double)Math.Abs(n - halfCycle) / halfCycle;
                var red = (short)Math.Round((c2.R - c1.R) * per) + c1.R;
                var grn = (short)Math.Round((c2.G - c1.G) * per) + c1.G;
                var blw = (short)Math.Round((c2.B - c1.B) * per) + c1.B;
                var clr = Color.FromArgb(red, grn, blw);
                if (BkClr) ctrl.ProgressBackColor = clr; else ctrl.ProgressColor = clr;
            }
        }
    }
}
