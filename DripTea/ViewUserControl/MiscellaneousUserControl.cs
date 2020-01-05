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
    public partial class MiscellaneousUserControl : UserControl
    {
        protected string conn = "Data source=driptea.db;Version=3;New=False;Compress=True";
        string ID = string.Empty;
        string previous = string.Empty;
        private List<string> Item = new List<string>();

        public MiscellaneousUserControl()
        {
            InitializeComponent();
            LoadData();
        }

        private void ProductUserControl_Load(object sender, EventArgs e)
        {
            Guna.UI.Lib.GraphicsHelper.DrawLineShadow(panel1, Color.WhiteSmoke, 20, 5, Guna.UI.WinForms.VerHorAlign.VerticalLeft);
        }

        private void LoadData()
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "Select * From Misc";

                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
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
                                    lblresult.Text = bunifuCustomDataGrid1.Rows.Count.ToString() + " Results";
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

        private void bunifuCustomDataGrid1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(bunifuCustomDataGrid1.Rows.Count >= 1)
            {
                // All ID
                ID = bunifuCustomDataGrid1.CurrentRow.Cells["ID"].Value.ToString();
                // Update
                txtupdatename.Text = bunifuCustomDataGrid1.CurrentRow.Cells["MiscName"].Value.ToString();
                previous = bunifuCustomDataGrid1.CurrentRow.Cells["MiscName"].Value.ToString();
                bunifuToggleSwitch1.Checked = Convert.ToBoolean(bunifuCustomDataGrid1.CurrentRow.Cells["Availability"].Value);
                // Add Stocks
                txtaddname.Text = bunifuCustomDataGrid1.CurrentRow.Cells["MiscName"].Value.ToString();
                txtcurrentstocks.Text = bunifuCustomDataGrid1.CurrentRow.Cells["Stocks"].Value.ToString();


                lblselected.Text = string.Format("ID Selected : {0}, Misc. Name: {1}, " +
                    "Stocks: {2}, Availability: {3}", ID, previous, 
                    txtcurrentstocks.Text, bunifuToggleSwitch1.Checked.ToString());
            }
        }

        private void gunaGradientButton1_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtname.Text) || !string.IsNullOrEmpty(txtstocks.Text))
            {
                CheckItem(txtname.Text);
            }
            else
            {
                foreach (var c in this.gunaGroupBox1.Controls.OfType<Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox>())
                {
                    c.BorderColorIdle = Color.FromArgb(220, 20, 60);
                    c.PlaceholderForeColor = Color.FromArgb(220, 20, 60);
                }
            }
        }

        private void CheckItem(string youritem)
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "SELECT MiscName From Misc WHERE MiscName=@M";
                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        try
                        {
                            string miscname = string.Empty;
                            sqlcmd.Parameters.AddWithValue("@M", youritem);
                            SQLiteDataReader dr = sqlcmd.ExecuteReader();
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    miscname = dr.GetString(0).ToString();
                                    if (miscname.Equals(youritem))
                                    {
                                        Bunifu.Snackbar.Show(this.FindForm(), "Misc. Item Name Already Exist", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                                    }
                                    else
                                    {
                                        AddItem();
                                    }
                                }
                            }
                            else
                            {
                                if (miscname.Equals(youritem))
                                {
                                    Bunifu.Snackbar.Show(this.FindForm(), "Misc. Item Name Already Exist", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                                }
                                else
                                {
                                    AddItem();
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CheckItemNameAndID(string youritem, string yourid)
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "SELECT ID,MiscName From Misc WHERE ID=@ID AND MiscName=@M";
                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        string itemid = string.Empty;
                        string itemname = string.Empty;
                        try
                        {
                            sqlcmd.Parameters.AddWithValue("@ID", yourid);
                            sqlcmd.Parameters.AddWithValue("@M", youritem);
                            SQLiteDataReader dr = sqlcmd.ExecuteReader();

                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    itemid = Convert.ToString(dr.GetInt32(0));
                                    itemname = dr.GetString(1).ToString();
                                }
                            }
                            else
                            {
                                UpdateItem();
                            }
                        }
                        finally
                        {
                            if (sqlcon.State == ConnectionState.Open)
                            {
                                sqlcon.Close();
                                CheckItemID(itemid, itemname);
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
        private void CheckItemID(string itemid, string itemname)
        {
            if (itemname == txtupdatename.Text && itemid == ID)
            {
                UpdateItem();
            }
        }

        private void AddItem()
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "INSERT INTO Misc VALUES (@ID,@MiscName,@Stocks,True)";
                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        try
                        {
                            sqlcmd.Parameters.AddWithValue("@ID", null);
                            sqlcmd.Parameters.AddWithValue("@MiscName", txtname.Text);
                            sqlcmd.Parameters.AddWithValue("@Stocks", txtstocks.Text);
                            sqlcmd.ExecuteNonQuery();
                            Bunifu.Snackbar.Show(this.FindForm(), "Success", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Success);
                        }
                        finally
                        {
                            if (sqlcon.State == ConnectionState.Open)
                            {
                                sqlcon.Close();
                                txtname.Clear();
                                txtstocks.Clear();
                                LoadData();
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

        private void UpdateItem()
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "UPDATE Misc SET MiscName=@MiscName, Availability=@Availability WHERE ID=@ID";
                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        try
                        {
                            sqlcmd.Parameters.AddWithValue("@ID", ID);
                            sqlcmd.Parameters.AddWithValue("@MiscName", txtupdatename.Text);
                            sqlcmd.Parameters.AddWithValue("@Availability", bunifuToggleSwitch1.Checked);
                            sqlcmd.ExecuteNonQuery();
                            Bunifu.Snackbar.Show(this.FindForm(), "Success", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Success);

                            if (!string.IsNullOrEmpty(previous))
                            {
                                getProductWithMiscItem(previous);
                            }
                        }
                        finally
                        {
                            if (sqlcon.State == ConnectionState.Open)
                            {
                                sqlcon.Close();
                                LoadData();
                                txtupdatename.Clear();
                                bunifuToggleSwitch1.Checked = true;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                Bunifu.Snackbar.Show(this.FindForm(), "Misc. Item Name Already Exist", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
            }
        }

        private void getProductWithMiscItem(string youritem)
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "Select * From Product where MiscItem Like '%" + youritem + "%'";
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
                                    getDataInTable(dt);
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
        private void getDataInTable(DataTable dt)
        {
            if (dt.Rows.Count >= 1)
            {
                foreach(DataRow dr in dt.Rows)
                {
                    string id = dr["ID"].ToString();
                    string miscitem = dr["MiscItem"].ToString();
                    createNewItemList(id, miscitem);
                }
            }
        }
        private void createNewItemList(string id, string MiscData)
        {
            Item.Clear();
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(MiscData))
            {
                string[] split = MiscData.Split(',');
                foreach(string s in split)
                {
                    Item.Add(s);
                }
                UpdateProductMiscItem(id);
            }
        }

        private void UpdateProductMiscItem(string yourid)
        {
            try
            {
                if (Item.Any())
                {
                    Item.Remove(previous);
                    Item.Add(txtupdatename.Text);

                    using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                    {
                        sqlcon.Open();
                        string query = "UPDATE Product SET MiscItem=@MiscItem WHERE ID=@ID";
                        using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                        {
                            try
                            {
                                sqlcmd.Parameters.AddWithValue("@ID", yourid);
                                string newmiscitem = string.Join(",", Item);
                                sqlcmd.Parameters.AddWithValue("@MiscItem", newmiscitem);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gunaGradientButton2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtupdatename.Text))
            {
                if (!string.IsNullOrEmpty(ID))
                {
                    CheckItemNameAndID(txtupdatename.Text, ID);
                }
                else
                {
                    Bunifu.Snackbar.Show(this.FindForm(), "Select Misc. Item to Update", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Information);
                }
            }
            else
            {
                foreach (var c in this.gunaGroupBox2.Controls.OfType<Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox>())
                {
                    c.BorderColorIdle = Color.FromArgb(220, 20, 60);
                    c.PlaceholderForeColor = Color.FromArgb(220, 20, 60);
                }
            }
        }

        private void txtaddstocks_TextChanged(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtaddstocks.Text) && !string.IsNullOrEmpty(txtcurrentstocks.Text))
            {
                long cstocks = Convert.ToInt64(txtcurrentstocks.Text);
                long astocks = Convert.ToInt64(txtaddstocks.Text);
                long result = Math.Abs(cstocks + astocks);
                txttotalstocks.Text = result.ToString();
            }
            else
            {
                txttotalstocks.Clear();
            }
        }

        private void txtaddstocks_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtstocks_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtname_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void gunaGradientButton3_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txttotalstocks.Text) && !string.IsNullOrEmpty(txtaddstocks.Text))
            {
                AddStocks();
            }
            else
            {
                txtaddstocks.BorderColorIdle = Color.FromArgb(220, 20, 60);
                txtaddstocks.PlaceholderForeColor = Color.FromArgb(220, 20, 60);
            }
        }

        private void AddStocks()
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "UPDATE Misc SET Stocks=@Stocks WHERE ID=@ID AND MiscName=@MiscName";
                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        try
                        {
                            sqlcmd.Parameters.AddWithValue("@ID", ID);
                            sqlcmd.Parameters.AddWithValue("@MiscName", txtaddname.Text);
                            sqlcmd.Parameters.AddWithValue("@Stocks", txttotalstocks.Text);
                            sqlcmd.ExecuteNonQuery();
                            Bunifu.Snackbar.Show(this.FindForm(), "Success", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Success);
                        }
                        finally
                        {
                            if (sqlcon.State == ConnectionState.Open)
                            {
                                sqlcon.Close();
                                LoadData();
                                foreach (var c in this.gunaGroupBox3.Controls.OfType<Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox>())
                                {
                                    c.Clear();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                Bunifu.Snackbar.Show(this.FindForm(), "Misc. Item Name Already Exist", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
            }
        }
    }
}
