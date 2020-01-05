using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace DripTea.ViewUserControl
{
    public partial class TransactionHistoryUserControl : UserControl
    {
        protected string conn = "Data source=driptea.db;Version=3;New=False;Compress=True";
        string orderby = "ASC";
        string filter = "ORDERNO";
        public TransactionHistoryUserControl()
        {
            InitializeComponent();
        }
        private void TransactionHistoryUserControl_Load(object sender, EventArgs e)
        {
            MaximumDATE();
            string query = string.Format("SELECT * FROM TransactionHistory WHERE date(Date) " +
                " BETWEEN '{0}' AND '{1}' ORDER BY {2} {3}", dtfrom.Value.ToString("yyyy-MM-dd")
                , dtto, filter, orderby);

            LoadData(query);
        }
        private void MaximumDATE()
        {
            DateTime maxdt = DateTime.Today;
            dtto.MaxDate = maxdt;
            dtfrom.MaxDate = maxdt;
        }
        private void gunaGradientButton1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtsearch.Text))
            {
                string query = string.Format("SELECT * FROM TransactionHistory WHERE date(Date) " +
                " BETWEEN '{0}' AND '{1}' AND {2} Like '%{3}%' ORDER BY {4} {5}", dtfrom.Value.ToString("yyyy-MM-dd")
                , dtto.Value.ToString("yyyy-MM-dd"), filter, txtsearch.Text, filter, orderby);

                LoadData(query);
            }
            else
            {
                string query = string.Format("SELECT * FROM TransactionHistory WHERE date(Date) " +
                " BETWEEN '{0}' AND '{1}' ORDER BY {2} {3}", dtfrom.Value.ToString("yyyy-MM-dd")
                , dtto.Value.ToString("yyyy-MM-dd"), filter, orderby);

                LoadData(query);
            }
        }


        private void LoadData(string q)
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = q;
                    DataTable dt = new DataTable();
                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        using (SQLiteDataReader dr = sqlcmd.ExecuteReader())
                        {
                            try
                            {
                                dt.Clear();
                                dt.Dispose();

                                dt.Load(dr);
                                bunifuCustomDataGrid1.DataSource = dt;
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

        private void gunaRadioButton1_Click(object sender, EventArgs e)
        {
            Guna.UI.WinForms.GunaRadioButton grb = (Guna.UI.WinForms.GunaRadioButton)sender;
            orderby = grb.Text;
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            if (gunaShadowPanel1.Visible) gunaShadowPanel1.Visible = false; else gunaShadowPanel1.Visible = true;
        }

        private void gunaAdvenceButton1_Click(object sender, EventArgs e)
        {
            Guna.UI.WinForms.GunaAdvenceButton btn = (Guna.UI.WinForms.GunaAdvenceButton)sender;
            filter = btn.Text;
        }

    }
}
