using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using DripTea.Card;

namespace DripTea.ViewForm.POSForm
{
    public partial class OrderStatusForm : Form
    {
        protected string conn = "Data source=driptea.db;Version=3;New=False;Compress=True";
        public OrderStatusForm()
        {
            InitializeComponent();
            Guna.UI.Lib.GraphicsHelper.ShadowForm(this);
        }

        public void OrderStatusForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "SELECT OrderNo FROM Orders GROUP BY OrderNo";
                    DataTable dt = new DataTable();
                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        using (SQLiteDataReader dr = sqlcmd.ExecuteReader())
                        {
                            try
                            {
                                dt.Load(dr);
                            }
                            finally
                            {
                                if (sqlcon.State == ConnectionState.Open)
                                {
                                    sqlcon.Close();

                                    flowLayoutPanel1.Controls.Clear();
                                    foreach (Control ctrl in flowLayoutPanel1.Controls)
                                        ctrl.Dispose();

                                    LoadDataTable(dt);
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

        private void LoadDataTable(DataTable dt)
        {
            if(dt.Rows.Count >= 1)
            {
                foreach(DataRow dr in dt.Rows)
                {
                    addOrderCard(dr["OrderNo"].ToString());
                }

                lblcount.Text = string.Format("({0})", dt.Rows.Count.ToString());
            }

        }

        private async void addOrderCard(string id)
        {

            OrderStatusCard osc = new OrderStatusCard(id);
            await Task.Run(() =>
            {
                if (this.flowLayoutPanel1.InvokeRequired)
                {
                    this.flowLayoutPanel1.Invoke(new MethodInvoker(
                                    delegate
                                    {
                                        flowLayoutPanel1.Controls.Add(osc);
                                    }
                                    )
                        );
                }
            });

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void OrderStatusForm_Resize(object sender, EventArgs e)
        {
            if(this.WindowState == FormWindowState.Maximized)
            {
                pictureBox1.Visible = true;
                int previouswidth = 1200;
                int newwidth = this.Size.Width;
                int plusX = Math.Abs(previouswidth - newwidth);
                int mywidth = 380;
                this.pictureBox1.Width = mywidth + plusX;
            }
            else if(this.WindowState == FormWindowState.Normal)
            {
                pictureBox1.Visible = false;
                this.pictureBox1.Width = 10;
            }
        }

        private void OrderStatusForm_Activated(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Show();
        }

        private void gunaControlBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
