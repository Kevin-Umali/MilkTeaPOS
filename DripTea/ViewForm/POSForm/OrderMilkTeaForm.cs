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

namespace DripTea.ViewForm.POSForm
{
    public partial class OrderMilkTeaForm : Form
    {
        protected string conn = "Data source=driptea.db;Version=3;New=False;Compress=True";
        protected string ID = string.Empty;
        double ld, hd, od = 0;
        double total = 0;
        double productprice = 0;
        string description, miscitem = string.Empty;

        string drinksize = "Lower Dose";

        public OrderMilkTeaForm(string _id, string _productname, string _ld, string _hd, 
            string _od, string _productgroup, Color c)
        {
            InitializeComponent();
            ID = _id;
            lblproductname.Text = _productname;
            lblproductgroup.Text = _productgroup;
            lblprice.Text = string.Format("LD: ₱{0}, HD: ₱{1}, OD: ₱{2}", _ld, _hd, _od);
            gunaCirclePictureBox1.BaseColor = c;

            ld = Convert.ToDouble(_ld);
            hd = Convert.ToDouble(_hd);
            od = Convert.ToDouble(_od);
        }

        private void gunaControlBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void gunaAdvenceButton2_Click(object sender, EventArgs e)
        {
            lbldescript.Text = "- " + description;
        }

        private void gunaAdvenceButton1_Click(object sender, EventArgs e)
        {
            lbldescript.Text = "- " + miscitem;
        }

        private void gunaNumeric1_ValueChanged(object sender, EventArgs e)
        {
            total = 0;
            getTotal();
        }

        private void getTotal()
        {
            if (GetCheckedRadio(flowLayoutPanel1) == "Lower Dose")
            {
                productprice = ld;
                double value = gunaNumeric1.Value * productprice;
                total = value;
                lbltotal.Text = string.Format("₱{0}", value);
            }
            else if (GetCheckedRadio(flowLayoutPanel1) == "High Dose")
            {
                productprice = hd;
                double value = gunaNumeric1.Value * productprice;
                total = value;
                lbltotal.Text = string.Format("₱{0}", value);
            }
            else if (GetCheckedRadio(flowLayoutPanel1) == "Over Dose")
            {
                productprice = od;
                double value = gunaNumeric1.Value * productprice;
                total = value;
                lbltotal.Text = string.Format("₱{0}", value);
            }
        }
        string GetCheckedRadio(Control container)
        {
            foreach (var control in container.Controls)
            {
                Guna.UI.WinForms.GunaRadioButton radio = control as Guna.UI.WinForms.GunaRadioButton;

                if (radio != null && radio.Checked)
                {
                    return radio.Text;
                }
            }

            return null;
        }

        private void gunaRadioButton1_Click(object sender, EventArgs e)
        {
            Guna.UI.WinForms.GunaRadioButton grb = (Guna.UI.WinForms.GunaRadioButton)sender;
            drinksize = grb.Text;
            getTotal();
        }

        private void LoadProductDetails()
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "SELECT Description,MiscItem FROM Product where ID=@ID";

                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        sqlcmd.Parameters.AddWithValue("@ID", ID);
                        using (SQLiteDataReader dr = sqlcmd.ExecuteReader())
                        {
                            try
                            {
                                if(dr.HasRows)
                                {
                                    while(dr.Read())
                                    {
                                        description = dr["Description"].ToString();
                                        miscitem = dr.GetString(1);
                                    }
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void OrderMilkTeaForm_Load(object sender, EventArgs e)
        {
            LoadProductDetails();
            getTotal();
            lbldescript.Text = "- " + description;
        }

        private void gunaButton2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblproductname.Text) && !string.IsNullOrEmpty(drinksize) &&
                !string.IsNullOrEmpty(gunaNumeric1.Value.ToString()) && !string.IsNullOrEmpty(total.ToString()))
            {
                if (CheckStockStateAndCart(miscitem))
                {
                    //MessageBox.Show("Done");
                    CheckCart(lblproductname.Text, drinksize);

                    POSForm f2 = (POSForm)Application.OpenForms["POSForm"];
                    f2.POSForm_Load(f2, EventArgs.Empty);
                }
            }
        }

        private bool CheckStockStateAndCart(string _miscitem)
        {
            bool done = false;
            if(!string.IsNullOrEmpty(_miscitem))
            {
                string[] miscarray = _miscitem.Split(',');
                foreach(string s in miscarray)
                {
                    StockAvailability(s);
                }
                done = true;
            }
            return done;
        }
        private void StockAvailability(string youritem)
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "SELECT Availability From Misc where MiscName=@MiscName LIMIT 1";
                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        bool available = false;
                        try
                        {
                            sqlcmd.Parameters.AddWithValue("@MiscName", youritem);
                            SQLiteDataReader dr = sqlcmd.ExecuteReader();

                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    available = Convert.ToBoolean(dr.GetBoolean(0));
                                }
                            }
                            else
                            {
                                UpdateStock(youritem, available);
                            }
                        }
                        finally
                        {
                            if (sqlcon.State == ConnectionState.Open)
                            {
                                sqlcon.Close();
                                UpdateStock(youritem, available);
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
        private void UpdateStock(string youritem, bool availability)
        {
            if (availability)
            {
                try
                {
                    using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                    {
                        sqlcon.Open();
                        string query = "UPDATE Misc SET Stocks=Stocks - @Stocks WHERE MiscName=@MiscName";
                        using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                        {
                            try
                            {
                                sqlcmd.Parameters.AddWithValue("@MiscName", youritem);
                                sqlcmd.Parameters.AddWithValue("@Stocks", gunaNumeric1.Value.ToString());
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
            else
            {
                Bunifu.Snackbar.Show(this, string.Format("This {0} Misc. Item are not available."
                    ,youritem), 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Warning);
            }
        }
        private void CheckCart(string yourproduct, string drsize)
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "SELECT ID From Cart where ProductName=@ProductName and Size=@Size";
                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        string id = string.Empty;
                        bool hasrows = false;
                        try
                        {
                            sqlcmd.Parameters.AddWithValue("@ProductName", yourproduct);
                            sqlcmd.Parameters.AddWithValue("@Size", drsize);
                            SQLiteDataReader dr = sqlcmd.ExecuteReader();

                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    id = Convert.ToString(dr.GetInt32(0));
                                    hasrows = true;
                                }
                            }
                            else
                            {
                                hasrows = false;
                            }
                        }
                        finally
                        {
                            if (sqlcon.State == ConnectionState.Open)
                            {
                                sqlcon.Close();
                                CheckBoolCart(id, hasrows);
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

        private void CheckBoolCart(string ID, bool hasrows)
        {
            if (hasrows) { UpdateCart(ID); }
            else { AddCart(); }
        }

        private void UpdateCart(string yourID)
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "UPDATE Cart SET Quantity=Quantity + @Quantity, TotalPrice=TotalPrice + @TotalPrice WHERE ID=@ID";
                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        try
                        {
                            sqlcmd.Parameters.AddWithValue("@ID", yourID);
                            sqlcmd.Parameters.AddWithValue("@Quantity", gunaNumeric1.Value.ToString());
                            sqlcmd.Parameters.AddWithValue("@TotalPrice", total.ToString());
                            sqlcmd.ExecuteNonQuery();
                        }
                        finally
                        {
                            if (sqlcon.State == ConnectionState.Open)
                            {
                                sqlcon.Close();
                                Bunifu.Snackbar.Show(this, "Success", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Success);
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

        private void AddCart()
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "INSERT INTO Cart VALUES (@ID,@ProductName,@MiscIitem,@Size,@Quantity,@Price,@TotalPrice)";
                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        try
                        {
                            sqlcmd.Parameters.AddWithValue("@ID", null);
                            sqlcmd.Parameters.AddWithValue("@ProductName", lblproductname.Text);
                            sqlcmd.Parameters.AddWithValue("@MiscIitem", miscitem);
                            sqlcmd.Parameters.AddWithValue("@Size", drinksize);
                            sqlcmd.Parameters.AddWithValue("@Quantity", gunaNumeric1.Value.ToString());
                            sqlcmd.Parameters.AddWithValue("@Price", productprice);
                            sqlcmd.Parameters.AddWithValue("@TotalPrice", total);
                            sqlcmd.ExecuteNonQuery();
                        }
                        finally
                        {
                            if (sqlcon.State == ConnectionState.Open)
                            {
                                sqlcon.Close();
                                Bunifu.Snackbar.Show(this, "Success", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Success);
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
