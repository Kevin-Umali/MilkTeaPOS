using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DripTea.Card;
using System.Data.SQLite;
using System.Threading;

namespace DripTea.ViewUserControl
{
    public partial class MilkTeaUserControl : UserControl
    {
        protected string conn = "Data source=driptea.db;Version=3;New=False;Compress=True";
        public MilkTeaUserControl()
        {
            InitializeComponent();
        }
        private void MilkTeaUserControl_Load(object sender, EventArgs e)
        {
            Guna.UI.Lib.GraphicsHelper.DrawLineShadow(panel1, Color.Black, 50, 5, Guna.UI.WinForms.VerHorAlign.HoriziontalTop);
            LoadData("Select ID,ProductName,LDPrice,HDPrice,ODPrice,ProductGroup " +
                "From Product where Availability = true ORDER BY ProductGroup");
            LoadColor();
        }
        private void LoadColor()
        {
            try
            {
                DisposableControl(flowLayoutPanel2);
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "Select * from ProductsGroup";
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
                                    LoadColor(dt);
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
        private void LoadColor(DataTable dt)
        {
            if (dt.Rows.Count >= 1)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string ProductColor = dr["Color"].ToString();
                    string[] colorsplit = ProductColor.Split(',');

                    Color clr = Color.FromArgb(
                            Convert.ToInt32(colorsplit[0]),
                            Convert.ToInt32(colorsplit[1]),
                            Convert.ToInt32(colorsplit[2]));

                    addColorCard(clr, dr["GroupName"].ToString());
                }
            }
        }
        private void addColorCard(Color c, string groupname)
        {
            GroupColorCard gcc = new GroupColorCard(c, groupname);
            flowLayoutPanel2.Controls.Add(gcc);
        }

        private void LoadData(string q)
        {
            try
            {
                DisposableControl(flowLayoutPanel1);
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
                                dt.Load(dr);
                            }
                            finally
                            {
                                if (sqlcon.State == ConnectionState.Open)
                                {
                                    sqlcon.Close();
                                    LoadProduct(dt);
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
        private void LoadProduct(DataTable dt)
        {
            if(dt.Rows.Count >= 1)
            {
                foreach(DataRow dr in dt.Rows)
                {
                    addProductCard(dr["ID"].ToString(), dr["ProductName"].ToString(),
                        dr["LDPrice"].ToString(), dr["HDPrice"].ToString(), dr["ODPrice"].ToString()
                        , dr["ProductGroup"].ToString());
                }
            }
        }

        private async void addProductCard(string id, string productname, string ld, string hd, string od, string productgroup)
        {
            ProductCard pc = new ProductCard(id, productname, ld, hd, od, productgroup);
            await Task.Run(async () =>
            {
                await Task.Delay(100);
                if (this.flowLayoutPanel1.InvokeRequired)
                {
                    this.flowLayoutPanel1.Invoke(new MethodInvoker(
                                    delegate
                                    {
                                        flowLayoutPanel1.Controls.Add(pc);
                                    }
                                    )
                        );
                }
            }); 
        }
        
        private void DisposableControl(Control ctrl)
        {
            ctrl.Controls.Clear();
            foreach (Control con in ctrl.Controls)
                con.Dispose();
        }

        private void gunaGradientButton1_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtsearch.Text))
            {
                LoadData("SELECT ID,ProductName,LDPrice,HDPrice,ODPrice,ProductGroup" +
                    " FROM Product where ProductName Like '%" + txtsearch.Text + "%' AND " +
                    "Availability = true ORDER BY ProductGroup");
            }
        }
    }
}
