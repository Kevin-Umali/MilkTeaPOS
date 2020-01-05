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

namespace DripTea.ViewForm.AdminForm
{
    public partial class AddProductForm : Form
    {
        protected string conn = "Data source=driptea.db;Version=3;New=False;Compress=True";
        private List<string> Item = new List<string>();
        private string Group = string.Empty;
        public AddProductForm(Form parent)
        {
            InitializeComponent();
            Guna.UI.Lib.GraphicsHelper.ShadowForm(this);
            this.Size = new Size(this.Width, parent.Size.Height);
            int plusX = parent.Size.Width - this.Size.Width;
            this.Location = new Point(parent.Location.X + plusX, parent.Location.Y);
        }

        private void AddProductForm_Load(object sender, EventArgs e)
        {
            Guna.UI.Lib.GraphicsHelper.DrawLineShadow(panel4, Color.WhiteSmoke, 20, 5, Guna.UI.WinForms.VerHorAlign.HoriziontalTop);
            LoadMiscItem();
            LoadProductsGroup();
        }

        private void LoadMiscItem()
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    DataTable dt = new DataTable();
                    string query = "SELECT ID,MiscName FROM Misc where Availability=True";

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
                                    AddMiscItem(dt);
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
        private void AddMiscItem(DataTable _dt)
        {
            if (_dt.Rows.Count >= 1)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    Guna.UI.WinForms.GunaCheckBox chkbox = new Guna.UI.WinForms.GunaCheckBox();
                    chkbox.Name = dr["MiscName"].ToString();
                    chkbox.Tag = dr["ID"].ToString();
                    chkbox.Cursor = Cursors.Hand;
                    chkbox.BackColor = Color.FromArgb(51, 57, 64);
                    chkbox.FillColor = Color.White;
                    chkbox.Checked = false;
                    chkbox.CheckedOffColor = Color.FromArgb(253, 177, 195);
                    chkbox.CheckedOnColor = Color.FromArgb(253, 177, 195);
                    chkbox.Font = new Font("Verdana", 9);
                    chkbox.ForeColor = Color.FromArgb(246, 246, 246);
                    chkbox.Text = dr["MiscName"].ToString();
                    chkbox.Click += CheckBoxClickEvent;
                    flowLayoutPanel1.Controls.Add(chkbox);
                }
            }
        }
        private void CheckBoxClickEvent(object sender, EventArgs e)
        {
            var chkbox = (Guna.UI.WinForms.GunaCheckBox)sender;
            if(chkbox.Checked == true)
            {
                if (!Item.Contains(chkbox.Text))
                {
                    Item.Add(chkbox.Text);
                }
            }
            else
            {
                if (Item.Contains(chkbox.Text))
                {
                    Item.Remove(chkbox.Text);
                }
            }
        }

        private void LoadProductsGroup()
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    DataTable dt = new DataTable();
                    string query = "SELECT * FROM ProductsGroup";

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
                                    AddProductsGroup(dt);
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

        private void AddProductsGroup(DataTable _dt)
        {
            if (_dt.Rows.Count >= 1)
            {
                foreach (DataRow dr in _dt.Rows)
                {
                    Guna.UI.WinForms.GunaRadioButton rdo = new Guna.UI.WinForms.GunaRadioButton();
                    rdo.Name = dr["GroupName"].ToString();
                    rdo.Tag = dr["ID"].ToString();
                    rdo.Cursor = Cursors.Hand;
                    rdo.BackColor = Color.FromArgb(51, 57, 64);
                    rdo.FillColor = Color.White;
                    rdo.BaseColor = Color.White;
                    rdo.CheckedOffColor = Color.Gray;
                    rdo.CheckedOnColor = Color.Plum;
                    rdo.Font = new Font("Verdana", 9);
                    rdo.ForeColor = Color.FromArgb(246, 246, 246);
                    rdo.Text = dr["GroupName"].ToString();
                    rdo.Click += RadioButtonCheckChanged;
                    flowLayoutPanel3.Controls.Add(rdo);
                }
            }
        }

        private void RadioButtonCheckChanged(object sender, EventArgs e)
        {
            var rdo = (Guna.UI.WinForms.GunaRadioButton)sender;
            Group = rdo.Text;
        }

        private void gunaControlBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void CheckProductName(string yourproductname)
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "SELECT ProductName From Product WHERE ProductName=@PN";
                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        try
                        {
                            sqlcmd.Parameters.AddWithValue("@PN", yourproductname);
                            SQLiteDataReader dr = sqlcmd.ExecuteReader();
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    string productname = dr.GetString(0).ToString();
                                    if (productname.Equals(yourproductname))
                                    {
                                        Bunifu.Snackbar.Show(this, "Product Name Already Exist", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                                    }
                                    else
                                    {
                                        AddProduct();
                                    }
                                }
                            }
                            else
                            {
                                AddProduct();
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

        private void AddProduct()
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "INSERT INTO Product VALUES (@ID,@ProductName,@Description,@ProductGroup,@MiscItem,@LD,@HD,@OD,True)";
                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        try
                        {
                            sqlcmd.Parameters.AddWithValue("@ID", null);
                            sqlcmd.Parameters.AddWithValue("@ProductName", txtproductname.Text);
                            sqlcmd.Parameters.AddWithValue("@Description", txtdescription.Text);
                            sqlcmd.Parameters.AddWithValue("@ProductGroup", Group);
                            string miscitem = string.Join(",", Item);
                            sqlcmd.Parameters.AddWithValue("@MiscItem", miscitem);
                            sqlcmd.Parameters.AddWithValue("@LD", txtld.Text);
                            sqlcmd.Parameters.AddWithValue("@HD", txtod.Text);
                            sqlcmd.Parameters.AddWithValue("@OD", txthd.Text);
                            sqlcmd.ExecuteNonQuery();
                            Bunifu.Snackbar.Show(this, "Success", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Success);
                        }
                        finally
                        {
                            if (sqlcon.State == ConnectionState.Open)
                            {
                                sqlcon.Close();
                                Clear();
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
        private void Clear()
        {
            Item.Clear();
            foreach (var c in this.Controls.OfType<Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox>())
            {
                c.Clear();
            }
            foreach (var c in this.flowLayoutPanel1.Controls.OfType<Guna.UI.WinForms.GunaCheckBox>())
            {
                c.Checked = false;
            }
        }

        private void gunaGradientButton1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtproductname.Text) || string.IsNullOrEmpty(txtdescription.Text) ||
                string.IsNullOrEmpty(txtld.Text) || string.IsNullOrEmpty(txtod.Text) ||
                string.IsNullOrEmpty(txthd.Text))
            {
                foreach (var c in this.Controls.OfType<Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox>())
                {
                    c.BorderColorIdle = Color.FromArgb(220, 20, 60);
                    c.PlaceholderForeColor = Color.FromArgb(220, 20, 60);
                }
            }
            else
            {
                if(!Item.Any())
                {
                    gunaGroupBox2.LineColor = Color.FromArgb(220, 20, 60);
                }
                else
                {
                    if (!string.IsNullOrEmpty(Group))
                    {
                        CheckProductName(txtproductname.Text);
                    }
                    else
                    {
                        gunaGroupBox1.LineColor = Color.FromArgb(220, 20, 60);
                    }
                }
            }
            //if(!Item.Any())
            //{
            //MessageBox.Show(Group);
            //}
            //MessageBox.Show(string.Join(", ", Item));
        }

        private void txtshort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
            (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }

        }

        private void gunaGradientButton2_Click(object sender, EventArgs e)
        {
            Clear();
        }
    }
}
