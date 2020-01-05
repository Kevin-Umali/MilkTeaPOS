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
    public partial class CartOrderCard : UserControl
    {
        protected string conn = "Data source=driptea.db;Version=3;New=False;Compress=True";
        protected string ID = string.Empty;
        string productname, size = string.Empty;
        string miscitem = string.Empty;
        int qty = 0;
        double price = 0;
        decimal productprice = 0;
        decimal newprice = 0;
        public CartOrderCard(string _ID, string _productname, string _miscitem, string _size,
            string _qty, string _price, string _productprice)
        {
            InitializeComponent();

            ID = _ID;
            productname = _productname;
            size = _size;
            miscitem = _miscitem;
            size = _size;
            qty = Convert.ToInt32(_qty);
            price = Convert.ToDouble(_price);
            productprice = Convert.ToDecimal(_productprice);

            lblproductname.Text = _productname;
            lblmisc.Text = _miscitem;
            lblsize.Text = _size;
            lblqty.Text = _qty;
            lbltotal.Text = "₱" + _price;

            txtqty.Text = _qty;
            lblcurrentqty.Text = _qty;

            //int newsize = _newsize.Width + this.Size.Width;
            //this.Size = new Size(newsize, this.Size.Height);
            ////this.Size = new Size(newwidth, this.Size.Height);
        }
        private void gunaLinePanel1_MouseEnter(object sender, EventArgs e)
        {
            if (!gunaButton2.Visible)
                gunaButton2.Visible = true;
        }

        private void gunaButton2_Click(object sender, EventArgs e)
        {
            panel1.BringToFront();
            gunaButton2.Visible = false;
        }

        private void CartOrderCard_MouseLeave(object sender, EventArgs e)
        {
            if (gunaButton2.Visible)
                gunaButton2.Visible = false;
        }

        private void gunaButton4_Click(object sender, EventArgs e)
        {
            panel1.SendToBack();
        }

        private void CartOrderCard_Load(object sender, EventArgs e)
        {

        }

        private void txtqty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void gunaButton3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblqty.Text) && !string.IsNullOrEmpty(txtqty.Text))
            {
                if (CheckAndGetMiscID(miscitem))
                {
                    if (CheckQty())
                    {
                        POSForm f2 = (POSForm)Application.OpenForms["POSForm"];
                        f2.POSForm_Load(f2, EventArgs.Empty);
                    }
                }
            }
        }
        private void gunaButton1_Click(object sender, EventArgs e)
        {
            txtqty.Text = lblcurrentqty.Text;
            if (CheckAndGetMiscID(miscitem))
            {
                if (CheckQty())
                {
                    POSForm f2 = (POSForm)Application.OpenForms["POSForm"];
                    f2.POSForm_Load(f2, EventArgs.Empty);
                }
            }
        }

        private bool CheckQty()
        {
            bool YesorNo = false;
            if (!string.IsNullOrEmpty(lblcurrentqty.Text) && !string.IsNullOrEmpty(txtqty.Text))
            {
                int currentqty = Convert.ToInt32(lblcurrentqty.Text);
                int newqty = 0;

                if (!string.IsNullOrEmpty(txtqty.Text))
                    newqty = Convert.ToInt32(txtqty.Text);

                if (newqty == currentqty)
                {
                    UpdateCartOrDelete(ID, false);
                    YesorNo = true;
                }
                else
                {
                    if (newqty == 0)
                    {
                        txtqty.BorderColorIdle = Color.FromArgb(220, 20, 60);
                        YesorNo = false;
                    }
                    else if (newqty < currentqty && newqty > 0)
                    {
                        UpdateCartOrDelete(ID, true);
                        YesorNo = true;
                    }
                }
                
            }
            return YesorNo;
        }
        private bool CheckAndGetMiscID(string _miscitem)
        {
            bool done = false;
            if (!string.IsNullOrEmpty(_miscitem))
            {
                string[] miscarray = _miscitem.Split(',');
                foreach (string s in miscarray)
                {
                    getMISCID(s);
                }
                done = true;
            }
            return done;
        }

        private void getMISCID(string youritem)
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "SELECT ID From Misc where MiscName=@MiscName LIMIT 1";
                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        string yourid = string.Empty;
                        try
                        {
                            sqlcmd.Parameters.AddWithValue("@MiscName", youritem);
                            SQLiteDataReader dr = sqlcmd.ExecuteReader();

                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    yourid = dr["ID"].ToString();
                                }
                            }
                        }
                        finally
                        {
                            if (sqlcon.State == ConnectionState.Open)
                            {
                                sqlcon.Close();
                                UpdateStock(yourid);
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

        private void UpdateStock(string yourid)
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "UPDATE Misc SET Stocks=Stocks + @Stocks WHERE ID=@ID";
                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        try
                        {
                            sqlcmd.Parameters.AddWithValue("@ID", yourid);
                            sqlcmd.Parameters.AddWithValue("@Stocks", txtqty.Text);
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

        private void txtqty_TextChanged(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtqty.Text))
            {
                int currentqty = Convert.ToInt32(lblcurrentqty.Text);
                int newqty = Convert.ToInt32(txtqty.Text);
                if(newqty > currentqty)
                {
                    txtqty.Text = lblcurrentqty.Text;
                    newprice = CalculatePrice(productprice, Convert.ToInt32(txtqty.Text));
                }
                else
                {
                    newprice = CalculatePrice(productprice, Convert.ToInt32(txtqty.Text));
                }
            }
            else
            {
                txtqty.Text = "1";
                newprice = CalculatePrice(productprice, Convert.ToInt32(txtqty.Text));
            }
        }
        private decimal CalculatePrice(decimal _price, int multiplyTo)
        {
            decimal price = _price;
                price = Math.Round(_price * multiplyTo, 2, MidpointRounding.AwayFromZero);
            return price;
        }
        private void UpdateCartOrDelete(string yourID, bool checker)
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = string.Empty;
                    if (checker)
                    {
                         query = "UPDATE Cart SET Quantity=@Quantity, TotalPrice=@TotalPrice WHERE ID=@ID";
                    }
                    else
                    {
                        query = "DELETE FROM Cart WHERE ID=@ID";
                    }
                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        try
                        {
                            if (checker)
                            {
                                sqlcmd.Parameters.AddWithValue("@Quantity", txtqty.Text);
                                sqlcmd.Parameters.AddWithValue("@TotalPrice", newprice);
                                sqlcmd.Parameters.AddWithValue("@ID", yourID);
                                sqlcmd.ExecuteNonQuery();
                            }
                            else
                            {
                                sqlcmd.Parameters.AddWithValue("@ID", yourID);
                                sqlcmd.ExecuteNonQuery();
                            }
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
    }
}
