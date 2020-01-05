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
    public partial class AccountUserControl : UserControl
    {
        protected string conn = "Data source=driptea.db;Version=3;New=False;Compress=True";
        string filter = string.Empty;
        string ID = "1";
        bool status;
        string name, username, password, usertype = string.Empty;
        private int DataCount;
        private int Limit = 10;
        private double TotalPage;
        private int CurrentPage = 1;
        public AccountUserControl()
        {
            InitializeComponent();
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
                    result = "SELECT * FROM Users where " + f + " Like '%" + s + "%'";
                }
                else
                {
                    result = "SELECT * FROM Users";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(s))
                    result = "SELECT * FROM Users where USERSID Like '%" + s + "%'";
                else if (string.IsNullOrEmpty(s))
                    result = "SELECT * FROM Users";
            }

            return result;
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
            if(p != 1)
            {
                row = p * l;
                row = row - Limit;
            }
            if (!string.IsNullOrEmpty(f))
            {
                if (!string.IsNullOrEmpty(s))
                {
                    result = "SELECT * FROM Users where " + f + " Like '%" + s + "%' LIMIT " + row + "," + Limit + "";
                }
                else
                {
                    result = "SELECT * FROM Users LIMIT " + row + "," + Limit + "";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(s))
                    result = "SELECT * FROM Users where USERSID Like '%" + s + "%' LIMIT " + row + "," + Limit + "";
                else if (string.IsNullOrEmpty(s))
                    result = "SELECT * FROM Users LIMIT " + row + "," + Limit + "";
            }

            return result;
        }
        private void AccountUserControl_Load(object sender, EventArgs e)
        {
            Guna.UI.Lib.GraphicsHelper.DrawLineShadow(panel1, Color.WhiteSmoke, 20, 5, Guna.UI.WinForms.VerHorAlign.VerticalLeft);
        }

        private void bunifuCustomDataGrid1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 3 && e.Value != null)
            {
                e.Value = new String('*', e.Value.ToString().Length);
            }
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

        private void gunaAdvenceButton1_Click(object sender, EventArgs e)
        {
            Guna.UI.WinForms.GunaAdvenceButton btn = (Guna.UI.WinForms.GunaAdvenceButton)sender;
            filter = btn.Text;
        }
        private void bunifuCustomDataGrid1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(bunifuCustomDataGrid1.Rows.Count >= 1)
            {
                ID = bunifuCustomDataGrid1.CurrentRow.Cells["UsersID"].Value.ToString();
                name = bunifuCustomDataGrid1.CurrentRow.Cells["Name"].Value.ToString();
                username = bunifuCustomDataGrid1.CurrentRow.Cells["Username"].Value.ToString();
                password = bunifuCustomDataGrid1.CurrentRow.Cells["Password"].Value.ToString();
                usertype = bunifuCustomDataGrid1.CurrentRow.Cells["UserType"].Value.ToString();
                status = Convert.ToBoolean(bunifuCustomDataGrid1.CurrentRow.Cells["Status"].Value);

                lblselected.Text = string.Format("ID Selected : {0}, Account Name: {1}, " +
                    "Username: {2}, UserType: {3}, Status: {4}", ID, name, username, 
                    usertype, status.ToString());
            }
        }

        private void gunaButton3_Click(object sender, EventArgs e)
        {
            if (ID == "1")
            {
                Bunifu.Snackbar.Show(this.FindForm(), "This is main admin you can't update the status.", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Information);
            }
            else
            {
                if (!string.IsNullOrEmpty(ID))
                {
                    InActiveAcc(ID);
                }
            }
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

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            if (panel4.Visible) gunaTransition1.HideSync(panel4); else gunaTransition1.ShowSync(panel4);
        }

        private void bunifuHSlider1_Scroll(object sender, Utilities.BunifuSlider.BunifuHScrollBar.ScrollEventArgs e)
        {
            lbldatarow.Text = bunifuHSlider1.Value.ToString();
            Limit = bunifuHSlider1.Value;
        }

        private void InActiveAcc(string yourid)
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = string.Empty;

                    if (status == true)
                        query = "UPDATE Users SET Status = False where UsersID = @ID";
                    else if(status == false)
                        query = "UPDATE Users SET Status = True where UsersID = @ID";


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

        private void GunaButton1_Click(object sender, EventArgs e)
        {
            CurrentPage = 1;
            new PopupEffect.transparentBg(this.FindForm(), new AddAcountForm(this.FindForm()));
            LoadDataCount();
            LoadData("", "", Limit, CurrentPage);
        }

        private void gunaButton2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ID))
            {
                CurrentPage = 1;
                UpdateAccountForm f1 = new UpdateAccountForm(this.FindForm());
                #region Just Public Stuff.
                f1.txtname.Text = name;
                f1.txtusername.Text = username;
                f1.txtpassword.Text = password;
                f1.txtconfirmpass.Text = password;
                f1.lblname.Text = name;
                f1.lblid.Text = ID;
                f1.lblusername.Text = username;
                f1.lblpassword.Text = password;
                f1.lblusertype.Text = usertype;
                #endregion
                new PopupEffect.transparentBg(this.FindForm(), f1);
                LoadDataCount();
                LoadData("", "", Limit, CurrentPage);
            }
        }
    }
}
