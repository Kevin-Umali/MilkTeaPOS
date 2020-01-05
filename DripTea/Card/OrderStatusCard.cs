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
using DripTea.ViewForm.POSForm;

namespace DripTea.Card
{
    public partial class OrderStatusCard : UserControl
    {
        protected string conn = "Data source=driptea.db;Version=3;New=False;Compress=True";
        string ID = string.Empty;
        string selectedID = string.Empty;
        string selectedOrderNo = string.Empty;
        public OrderStatusCard(string _ID)
        {
            InitializeComponent();
            ID = _ID;
        }

        private void OrderStatusCard_Load(object sender, EventArgs e)
        {
            LoadData(ID);
        }

        private void LoadData(string yourid)
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "SELECT * FROM Orders WHERE OrderNo=@OrderNo";

                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        sqlcmd.Parameters.AddWithValue("@OrderNo", ID);
                        using (SQLiteDataReader dr = sqlcmd.ExecuteReader())
                        {
                            try
                            {
                                DataTable dt = new DataTable();
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

        private void bunifuCustomDataGrid1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if(bunifuCustomDataGrid1.Rows.Count >= 1)
            {
                int xCount = bunifuCustomDataGrid1.Rows
                .Cast<DataGridViewRow>()
                .Select(row => row.Cells["Status"].Value.ToString())
                .Count(s => s == "Pending");

                lbltotal.Text = string.Format("({0})", bunifuCustomDataGrid1.Rows.Count.ToString());
                lblpending.Text = string.Format("({0})", xCount);

                lblorderno.Text = bunifuCustomDataGrid1.Rows[0].Cells["OrderNo"].Value.ToString();
            }
        }

        private void bunifuCustomDataGrid1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void gunaGradientButton1_Click(object sender, EventArgs e)
        {
            if(bunifuCustomDataGrid1.Rows.Count >= 1)
            {
                foreach(DataGridViewRow dgvr in bunifuCustomDataGrid1.Rows)
                {
                    AddToTransactionHistory(dgvr.Cells["OrderNo"].Value.ToString(),
                        dgvr.Cells["ProductID"].Value.ToString(),
                        dgvr.Cells["ProductName"].Value.ToString(),
                        dgvr.Cells["Size"].Value.ToString(),
                        dgvr.Cells["Quantity"].Value.ToString(),
                        dgvr.Cells["Price"].Value.ToString());
                }

                Bunifu.Snackbar.Show(this.FindForm(), "Success", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Success);
                DeleteOrders();
            }
        }
        private void AddToTransactionHistory(string yourorderno, string yourid,
            string yourproductname, string yoursize, string yourqty, string yourprice)
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "INSERT INTO TransactionHistory values " +
                        "(@OrderNo, @ProductID, @ProductName, @Size, @Quantity, @Price, DATE('now'))";
                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        try
                        {
                            sqlcmd.Parameters.AddWithValue("@OrderNo", yourorderno);
                            sqlcmd.Parameters.AddWithValue("@ProductID", yourid);
                            sqlcmd.Parameters.AddWithValue("@ProductName", yourproductname);
                            sqlcmd.Parameters.AddWithValue("@Size", yoursize);
                            sqlcmd.Parameters.AddWithValue("@Quantity", yourqty);
                            sqlcmd.Parameters.AddWithValue("@Price", yourprice);
                            sqlcmd.ExecuteNonQuery();
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteOrders()
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "DELETE FROM Orders WHERE OrderNo=@OrderNo";

                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        try
                        {
                            sqlcmd.Parameters.AddWithValue("@OrderNo", lblorderno.Text);
                            sqlcmd.ExecuteNonQuery();
                        }
                        finally
                        {
                            if (sqlcon.State == ConnectionState.Open)
                            {
                                sqlcon.Close();
                                if (Application.OpenForms["OrderStatusForm"] != null)
                                {
                                    OrderStatusForm f2 = (OrderStatusForm)Application.OpenForms["OrderStatusForm"];
                                    f2.OrderStatusForm_Load(f2, EventArgs.Empty);
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
    }
}
