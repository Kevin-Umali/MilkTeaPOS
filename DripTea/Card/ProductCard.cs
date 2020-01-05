using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data;
using DripTea.ViewForm.POSForm;
using System.Linq;

namespace DripTea.Card
{
    public partial class ProductCard : UserControl
    {
        protected string conn = "Data source=driptea.db;Version=3;New=False;Compress=True";
        protected string ID,productname,ld,hd,od = string.Empty;
        protected string ProductGroup = string.Empty;
        Color clr;
        public ProductCard(string _id, string _productname, string _ld, string _hd, string _od, string _productgroup)
        {
            InitializeComponent();
            lblproductname.Text = _productname;
            lblld.Text = "₱" + _ld;
            lblhd.Text = "₱" + _hd;
            lblod.Text = "₱" + _od;

            ID = _id;
            ProductGroup = _productgroup;

            productname = _productname;
            ld = _ld;
            hd = _hd;
            od = _od;
        }

        private void gunaShadowPanel1_MouseEnter(object sender, EventArgs e)
        {
            LineControlPanel(true);
        }

        private void LineControlPanel(bool b)
        {
            if (b)
            {
                gunaLinePanel1.LineLeft = 1; gunaLinePanel1.LineRight = 1;
                gunaLinePanel1.LineTop = 1; gunaLinePanel1.LineBottom = 1;
            }
            else
            {
                gunaLinePanel1.LineLeft = 0; gunaLinePanel1.LineRight = 0;
                gunaLinePanel1.LineTop = 0; gunaLinePanel1.LineBottom = 0;
            }


        }
        private void lblod_Click(object sender, EventArgs e)
        {
            new PopupEffect.transparentBg(this.FindForm(),
                new OrderMilkTeaForm(ID, productname, ld, hd, od, ProductGroup, clr));
        }

        private void gunaLinePanel1_MouseLeave(object sender, EventArgs e)
        {
            LineControlPanel(false);
        }

        private void LoadColor()
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "Select Color from ProductsGroup where GroupName=@GN";
                    DataTable dt = new DataTable();
                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        sqlcmd.Parameters.AddWithValue("@GN", ProductGroup);
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
                                    dr.Close();
                                    ColorLoad(dt);
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

        private void ColorLoad(DataTable dt)
        {
            try
            {
                if (dt.Rows.Count >= 1)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string ProductColor = dr["Color"].ToString();
                        string[] colorsplit = ProductColor.Split(',');

                        clr = Color.FromArgb(
                               Convert.ToInt32(colorsplit[0]),
                               Convert.ToInt32(colorsplit[1]),
                               Convert.ToInt32(colorsplit[2]));
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        string[] milkpicture = { "m1", "m2", "m3", "m4", "m5", "m6" };
        private void ProductCard_Load(object sender, EventArgs e)
        {
            LoadColor();
            gunaShadowPanel1.BaseColor = clr;
            Random rdn = new Random();
            object O = Properties.Resources.ResourceManager.GetObject(milkpicture[rdn.Next(0,5)]); //Return an object from the image chan1.png in the project
            pictureBox1.Image = (Image)O;

            foreach (var c in this.gunaShadowPanel1.Controls.OfType<Label>())
            {
                if (c.Name == "lblproductname")
                    c.ForeColor = GetReadableForeColor(clr, true);
                else
                    c.ForeColor = GetReadableForeColor(clr, false);
            }
        }

        private static Color GetReadableForeColor(Color c, bool y)
        {
            Color clr;
            if (y)
            {
                clr = (((c.R + c.B + c.G) / 3) > 128) ? Color.Black : Color.White;
            }
            else
            {
                clr = (((c.R + c.B + c.G) / 3) > 128) ? Color.FromArgb(80, 80, 80) : Color.FromArgb(200, 200, 200);
            }
            return clr;
        }
    }
}
