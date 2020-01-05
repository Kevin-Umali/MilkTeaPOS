using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DripTea.ViewForm.POSForm
{
    public partial class ReceiptPrintForm : Form
    {
        protected string conn = "Data source=driptea.db;Version=3;New=False;Compress=True";

        public ReceiptPrintForm(string orderno, string vatable, string vat, string discount,
            string discountedprice, string total, string cash, string change, string cashier)
        {
            InitializeComponent();

            lblorderno.Text = string.Format("ORDER NO: {0}", orderno);
            lblvatable.Text = string.Format("Vatable: {0}", vatable);
            lblvat.Text = string.Format("VAT (12%): {0}", vat);
            lbldiscount.Text = string.Format("Discount ({0}%): {1}", discount, discountedprice);
            lbltotal.Text = string.Format("TOTAL: {0}", total);
            lblcash.Text = string.Format("Cash Tend: ₱{0}", cash);
            lblchange.Text = string.Format("Cash Due: ₱{0}", change);
            lblcashier.Text = string.Format("Cashier: {0}", cashier);

            lbldate.Text = string.Format("Date: {0}", DateTime.Now.ToLongDateString());
            lbltime.Text = string.Format("Time: {0}", DateTime.Now.ToLongTimeString());
        }
        private void ReceiptPrintForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "Select ProductName as 'Item',Quantity as 'Qty',TotalPrice as 'Price' From Cart Order By ProductName";
                    DataTable dt = new DataTable();
                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        using (SQLiteDataReader dr = sqlcmd.ExecuteReader())
                        {
                            try
                            {
                                dt.Clear();
                                dt.Dispose();
                                //dataGridView1.Rows.Clear();
                                //dataGridView1.Dispose();


                                dt.Load(dr);
                                dataGridView1.DataSource = dt;
                            }
                            finally
                            {
                                if (sqlcon.State == ConnectionState.Open)
                                {
                                    sqlcon.Close();
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


        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if(UpdatingGrid())
            {
                //print heree...

                timer1.Start();
            }
        }

        private bool UpdatingGrid()
        {
            bool val = false;
            var height = 20;
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                height += dr.Height;
            }
            dataGridView1.Height = height;

            dataGridView1.ClearSelection();

            dataGridView1.Columns["Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["Price"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns["Qty"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView1.Columns["Qty"].Width = 37;
            dataGridView1.Columns["Item"].Width = 150;


            var format = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
            format.CurrencySymbol = "₱";
            dataGridView1.Columns["Price"].DefaultCellStyle.FormatProvider = format;
            dataGridView1.Columns["Price"].DefaultCellStyle.Format = "c";

            val = true;

            return val;
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            //e.Graphics.DrawImage(memoryImage, 0, 0);
            Bitmap bmp = new Bitmap(this.mainpanel.Width, this.mainpanel.Height);
            this.mainpanel.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
            e.Graphics.DrawImage(bmp, 0, 0);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            timer1.Stop();

            string file = (string)("DripTeaReceipt-"+ DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss.tt"));
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            string printername = Properties.Settings.Default.PrinterName;
            if (Properties.Settings.Default.PrinterName.Equals("Microsoft Print to PDF"))
            {
                printDocument1.PrinterSettings.PrinterName = printername;
                printDocument1.DefaultPageSettings.PaperSize = new PaperSize("Custom Paper", 300, mainpanel.Height);
                printDocument1.DefaultPageSettings.PrinterSettings.PrintToFile = true;
                printDocument1.DefaultPageSettings.PrinterSettings.PrintFileName = Path.Combine(directory, file + ".pdf");
                printDocument1.Print();
            }
            else
            {
                printDocument1.PrinterSettings.PrinterName = printername;
                printDocument1.Print();
            }

            this.Dispose();

        }
    }
}
