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
using DripTea.ViewForm.AdminForm;

namespace DripTea.ViewUserControl
{
    public partial class ProductUserControl : UserControl
    {
        protected string conn = "Data source=driptea.db;Version=3;New=False;Compress=True";
        string productname, description, productgroup, miscitem, ldprice, hdprice, odprice = string.Empty;
        private string ID = string.Empty;
        private int DataCount;
        private int Limit = 20;
        private double TotalPage;
        private int CurrentPage = 1;
        string filter = string.Empty;

        private bool status;
        public ProductUserControl()
        {
            InitializeComponent();
            LoadDataCount();
            LoadData("", "", Limit, CurrentPage);
        }

        private void ProductUserControl_Load(object sender, EventArgs e)
        {
            Guna.UI.Lib.GraphicsHelper.DrawLineShadow(panel1, Color.WhiteSmoke, 20, 5, Guna.UI.WinForms.VerHorAlign.VerticalLeft);
        }

        private void GunaButton1_Click(object sender, EventArgs e)
        {
            new PopupEffect.transparentBg(this.FindForm(), new AddProductForm(this.FindForm()));
            LoadDataCount();
            LoadData("", "", Limit, CurrentPage);
        }

        private void LoadDataCount()
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = Filterino1(filter, txtsearch.Text);

                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        using (SQLiteDataReader dr = sqlcmd.ExecuteReader())
                        {
                            try
                            {
                                DataTable dt = new DataTable();
                                dt.Load(dr);
                                DataCount = dt.Rows.Count;
                                lblresult.Text = dt.Rows.Count.ToString() + " Results";
                            }
                            finally
                            {
                                if (sqlcon.State == ConnectionState.Open)
                                {
                                    sqlcon.Close();
                                    createNumericPagination();
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
        private string Filterino1(string f, string s)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(f))
            {
                if (!string.IsNullOrEmpty(s))
                {
                    result = "SELECT * FROM Product where " + f + " Like '%" + s + "%'";
                }
                else
                {
                    result = "SELECT * FROM Product";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(s))
                    result = "SELECT * FROM Product where ID Like '%" + s + "%'";
                else if (string.IsNullOrEmpty(s))
                    result = "SELECT * FROM Product";
            }

            return result;
        }

        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            CurrentPage = 1;
            if (!string.IsNullOrEmpty(txtsearch.Text))
            {
                LoadDataCount();
                LoadData(filter, txtsearch.Text, Limit, CurrentPage);
            }
            else
            {
                LoadDataCount();
                LoadData("", "", Limit, CurrentPage);
            }
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            if (panel4.Visible) gunaTransition1.HideSync(panel4); else gunaTransition1.ShowSync(panel4);
        }

        private void bunifuHSlider1_Scroll(object sender, Utilities.BunifuSlider.BunifuHScrollBar.ScrollEventArgs e)
        {
            lbldatarow.Text = bunifuHSlider1.Value.ToString();
            Limit = bunifuHSlider1.Value;
        }

        private void gunaGradientCircleButton1_Click(object sender, EventArgs e)
        {
            if (CurrentPage < TotalPage)
            {
                if (CurrentPage >= 1)
                {
                    CurrentPage++;
                    if (!string.IsNullOrEmpty(txtsearch.Text))
                    {
                        LoadDataCount();
                        LoadData(filter, txtsearch.Text, Limit, CurrentPage);
                    }
                    else
                    {
                        LoadDataCount();
                        LoadData(filter, "", Limit, CurrentPage);
                    }

                    label2.Text = CurrentPage.ToString() + " / " + TotalPage.ToString();
                }
            }
        }

        private void gunaGradientCircleButton2_Click(object sender, EventArgs e)
        {
            if (CurrentPage <= TotalPage)
            {
                if (CurrentPage != 1)
                {
                    CurrentPage--;
                    if (!string.IsNullOrEmpty(txtsearch.Text))
                    {
                        LoadDataCount();
                        LoadData(filter, txtsearch.Text, Limit, CurrentPage);
                    }
                    else
                    {
                        LoadDataCount();
                        LoadData(filter, "", Limit, CurrentPage);
                    }
                    label2.Text = CurrentPage.ToString() + " / " + TotalPage.ToString();
                }
            }
        }

        private void gunaButton3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ID))
            {
                InActiveProduct(ID);
            }
        }
        private void InActiveProduct(string yourid)
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = string.Empty;

                    if (status == true)
                        query = "UPDATE Product SET Availability = False where ID = @ID";
                    else if (status == false)
                        query = "UPDATE Product SET Availability = True where ID = @ID";


                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        try
                        {
                            sqlcmd.Parameters.AddWithValue("@ID", yourid);
                            sqlcmd.ExecuteNonQuery();
                            Bunifu.Snackbar.Show(this.FindForm(), "Success", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Success);
                        }
                        finally
                        {
                            if (sqlcon.State == ConnectionState.Open)
                            {
                                sqlcon.Close();
                                LoadDataCount();
                                LoadData("", "", Limit, CurrentPage);
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

        private void createNumericPagination()
        {
            if (DataCount != 0)
            {
                TotalPage = DataCount / Limit;
                if (TotalPage % Limit > 0)
                {
                    TotalPage += 1;
                }

                if (TotalPage == 0)
                {
                    lblpage.Text = "1 Page";
                    label2.Text = CurrentPage.ToString() + " / 1";
                }
                else
                {
                    lblpage.Text = TotalPage.ToString() + " Pages";
                    label2.Text = CurrentPage.ToString() + " / " + TotalPage.ToString();
                }

            }
        }
        private void LoadData(string _filter, string search, int _limit, int _page)
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = Filterino(_filter, search, _limit, _page);

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

        private string Filterino(string f, string s, int l, int p)
        {
            string result = string.Empty;
            int row = 0;
            if (p != 1)
            {
                row = p * l;
                row = row - Limit;
            }
            if (!string.IsNullOrEmpty(f))
            {
                if (!string.IsNullOrEmpty(s))
                {
                    result = "SELECT * FROM Product where " + f + " Like '%" + s + "%' LIMIT " + row + "," + Limit + "";
                }
                else
                {
                    result = "SELECT * FROM Product LIMIT " + row + "," + Limit + "";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(s))
                    result = "SELECT * FROM Product where ID Like '%" + s + "%' LIMIT " + row + "," + Limit + "";
                else if (string.IsNullOrEmpty(s))
                    result = "SELECT * FROM Product LIMIT " + row + "," + Limit + "";
            }

            return result;
        }

        private void gunaButton2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ID))
            {
                UpdateProductForm f1 = new UpdateProductForm(this.FindForm());
                f1.txtproductname.Text = productname;
                f1.txtdescription.Text = description;
                f1.txthd.Text = hdprice;
                f1.txtld.Text = ldprice;
                f1.txtod.Text = odprice;
                f1.ID = ID;
                f1.MiscItem = miscitem;
                f1.ProductGroup = productgroup;

                f1.lblproductname.Text = productname;
                f1.lbldescription.Text = description;
                f1.lblhigh.Text = hdprice;
                f1.lbllower.Text = ldprice;
                f1.lblover.Text = odprice;
                f1.lblid.Text = ID;
                f1.lblmisc.Text = miscitem;
                f1.lblproductgroup.Text = productgroup;

                new PopupEffect.transparentBg(this.FindForm(), f1);

                LoadDataCount();
                LoadData("", "", Limit, CurrentPage);
            }
            
        }

        private void gunaAdvenceButton1_Click(object sender, EventArgs e)
        {
            Guna.UI.WinForms.GunaAdvenceButton btn = (Guna.UI.WinForms.GunaAdvenceButton)sender;
            filter = btn.Text;
        }

        private void bunifuCustomDataGrid1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (bunifuCustomDataGrid1.Rows.Count >= 1)
            {
                ID = bunifuCustomDataGrid1.CurrentRow.Cells["ID"].Value.ToString();
                productname = bunifuCustomDataGrid1.CurrentRow.Cells["ProductName"].Value.ToString();
                description = bunifuCustomDataGrid1.CurrentRow.Cells["Description"].Value.ToString();
                productgroup = bunifuCustomDataGrid1.CurrentRow.Cells["ProductGroup"].Value.ToString();
                miscitem = bunifuCustomDataGrid1.CurrentRow.Cells["MiscItem"].Value.ToString();
                ldprice = bunifuCustomDataGrid1.CurrentRow.Cells["LDPrice"].Value.ToString();
                hdprice = bunifuCustomDataGrid1.CurrentRow.Cells["HDPrice"].Value.ToString();
                odprice = bunifuCustomDataGrid1.CurrentRow.Cells["ODPrice"].Value.ToString();

                status = Convert.ToBoolean(bunifuCustomDataGrid1.CurrentRow.Cells["Availability"].Value);

                lblselected.Text = string.Format("ID Selected : {0}, Product Name: {1}, " +
                    "Description: {2}, Product Group: {3}, Misc Item: {4}, Lower Dose Price: {5}" +
                    ", High Dose Price: {6}, Over Dose Price: {7}, Availability: {8}", ID, productname,
                    description, productgroup, miscitem, ldprice, hdprice,
                    odprice, status.ToString());
            }
        }
    }
}
