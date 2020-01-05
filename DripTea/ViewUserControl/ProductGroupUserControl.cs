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
    public partial class ProductGroupUserControl : UserControl
    {
        protected string conn = "Data source=driptea.db;Version=3;New=False;Compress=True";
        string ID = string.Empty;
        public ProductGroupUserControl()
        {
            InitializeComponent();
            LoadData();
        }

        private void ProductGroupUserControl_Load(object sender, EventArgs e)
        {
            Guna.UI.Lib.GraphicsHelper.DrawLineShadow(panel1, Color.WhiteSmoke, 20, 5, Guna.UI.WinForms.VerHorAlign.VerticalLeft);
        }



        private void gunaGradientButton2_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                string clr = RGBConverter.getColorText(cd.Color);
                txtgcolor.Text = clr;
                int r = RGBConverter.getR(cd.Color);
                int g = RGBConverter.getG(cd.Color);
                int b = RGBConverter.getB(cd.Color);
                txtgcolor.BackColor = Color.FromArgb(r, g, b);
            }
        }
        private void LoadData()
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "Select * From ProductsGroup";

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

        private void gunaGradientButton1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtgroupname.Text) || !string.IsNullOrEmpty(txtgcolor.Text))
            {
                CheckGroupName(txtgroupname.Text);
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

        private void CheckGroupName(string yourgroupname)
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "SELECT GroupName From ProductsGroup WHERE GroupName=@GN";
                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        try
                        {
                            string miscname = string.Empty;
                            sqlcmd.Parameters.AddWithValue("@GN", yourgroupname);
                            SQLiteDataReader dr = sqlcmd.ExecuteReader();
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    miscname = dr.GetString(0).ToString();
                                    if (miscname.Equals(yourgroupname))
                                    {
                                        Bunifu.Snackbar.Show(this.FindForm(), "Group Name Already Exist", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                                    }
                                    else
                                    {
                                        AddItem();
                                    }
                                }
                            }
                            else
                            {
                                if (miscname.Equals(yourgroupname))
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

        private void AddItem()
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "INSERT INTO ProductsGroup VALUES (@ID,@GroupName,@Color)";
                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        try
                        {
                            sqlcmd.Parameters.AddWithValue("@ID", null);
                            sqlcmd.Parameters.AddWithValue("@GroupName", txtgroupname.Text);
                            sqlcmd.Parameters.AddWithValue("@Color", txtgcolor.Text);
                            sqlcmd.ExecuteNonQuery();
                            Bunifu.Snackbar.Show(this.FindForm(), "Success", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Success);
                        }
                        finally
                        {
                            if (sqlcon.State == ConnectionState.Open)
                            {
                                sqlcon.Close();
                                txtgroupname.Clear();
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

        private void bunifuCustomDataGrid1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (bunifuCustomDataGrid1.Rows.Count >= 1)
            {
                foreach (DataGridViewRow dgvr in bunifuCustomDataGrid1.Rows)
                {
                    string ProductColor = dgvr.Cells["Color"].Value.ToString();
                    string[] colorsplit = ProductColor.Split(',');

                    dgvr.Cells["Color"].Style.BackColor = Color.FromArgb(
                        Convert.ToInt32(colorsplit[0]), 
                        Convert.ToInt32(colorsplit[1]),
                        Convert.ToInt32(colorsplit[2]));
                }
            }
        }

        private void gunaGradientButton3_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                string clr = RGBConverter.getColorText(cd.Color);
                txtupdategcolor.Text = clr;
                int r = RGBConverter.getR(cd.Color);
                int g = RGBConverter.getG(cd.Color);
                int b = RGBConverter.getB(cd.Color);
                txtupdategcolor.BackColor = Color.FromArgb(r, g, b);
            }
        }

        private void bunifuCustomDataGrid1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(bunifuCustomDataGrid1.Rows.Count >= 1)
            {
                ID = bunifuCustomDataGrid1.CurrentRow.Cells["ID"].Value.ToString();
                txtupdategroupname.Text = bunifuCustomDataGrid1.CurrentRow.Cells["GroupName"].Value.ToString();
                txtupdategcolor.Text = bunifuCustomDataGrid1.CurrentRow.Cells["Color"].Value.ToString();

                string ProductColor = bunifuCustomDataGrid1.CurrentRow.Cells["Color"].Value.ToString();
                string[] colorsplit = ProductColor.Split(',');

                txtupdategcolor.BackColor = Color.FromArgb(
                        Convert.ToInt32(colorsplit[0]),
                        Convert.ToInt32(colorsplit[1]),
                        Convert.ToInt32(colorsplit[2]));
            }
        }

        private void gunaGradientButton4_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtupdategroupname.Text))
            {
                if (!string.IsNullOrEmpty(ID))
                {
                    CheckGroupNameAndID(txtupdategroupname.Text, ID);
                }
                else
                {
                    Bunifu.Snackbar.Show(this.FindForm(), "Select Group to Update", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Information);
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

        private void CheckGroupNameAndID(string yourgroupname, string yourid)
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "SELECT ID,GroupName From ProductsGroup WHERE ID=@ID AND GroupName=@GN";
                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        string groupid = string.Empty;
                        string groupname = string.Empty;
                        try
                        {
                            sqlcmd.Parameters.AddWithValue("@ID", yourid);
                            sqlcmd.Parameters.AddWithValue("@GN", yourgroupname);
                            SQLiteDataReader dr = sqlcmd.ExecuteReader();

                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    groupid = Convert.ToString(dr.GetInt32(0));
                                    groupname = dr.GetString(1).ToString();
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
                                CheckGroupID(groupid, groupname);
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
        private void CheckGroupID(string _groupid, string _groupname)
        {
            if (_groupname == txtupdategroupname.Text && _groupid == ID)
            {
                if (!string.IsNullOrEmpty(ID))
                {
                    UpdateItem();
                }
            }
        }

        private void UpdateItem()
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "UPDATE ProductsGroup SET GroupName=@GroupName, Color=@Color WHERE ID=@ID";
                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        try
                        {
                            sqlcmd.Parameters.AddWithValue("@ID", ID);
                            sqlcmd.Parameters.AddWithValue("@GroupName", txtupdategroupname.Text);
                            sqlcmd.Parameters.AddWithValue("@Color", txtupdategcolor.Text);
                            sqlcmd.ExecuteNonQuery();
                            Bunifu.Snackbar.Show(this.FindForm(), "Success", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Success);
                        }
                        finally
                        {
                            if (sqlcon.State == ConnectionState.Open)
                            {
                                sqlcon.Close();
                                LoadData();
                                txtupdategroupname.Clear();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                Bunifu.Snackbar.Show(this.FindForm(), "Group Name Already Exist", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
            }
        }
    }

    public class RGBConverter
    {
        public static string getColorText(Color c)
        {
            string value = string.Empty;
            string R = string.Empty;
            string G = string.Empty;
            string B = string.Empty;
            if (!c.IsEmpty)
            {
                R = c.R.ToString();
                G = c.G.ToString();
                B = c.B.ToString();
            }
            return value = string.Format("{0}, {1}, {2}", R, G, B);
        }

        public static int getB(Color c)
        {
            int BValue = 0;
            BValue = c.B;

            return BValue;
        }

        public static int getG(Color c)
        {
            int GValue = 0;
            GValue = c.G;

            return GValue;
        }

        public static int getR(Color c)
        {
            int RValue = 0;
            RValue = c.R;

            return RValue;
        }
    }
}
