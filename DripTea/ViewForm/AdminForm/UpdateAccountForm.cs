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
    public partial class UpdateAccountForm : Form
    {
        protected string conn = "Data source=driptea.db;Version=3;New=False;Compress=True";
        public UpdateAccountForm(Form parent)
        {
            InitializeComponent();
            Guna.UI.Lib.GraphicsHelper.ShadowForm(this);
            this.Size = new Size(this.Width, parent.Size.Height);
            int plusX = parent.Size.Width - this.Size.Width;
            this.Location = new Point(parent.Location.X + plusX, parent.Location.Y);
        }

        private void UpdateAccountForm_Load(object sender, EventArgs e)
        {
            Guna.UI.Lib.GraphicsHelper.DrawLineShadow(panel4, Color.WhiteSmoke, 20, 5, Guna.UI.WinForms.VerHorAlign.HoriziontalTop);
            lblpassword.Text = new String('*', lblpassword.Text.Length);
        }

        private void gunaControlBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void gunaGradientButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtname.Text) || string.IsNullOrEmpty(txtusername.Text) ||
                string.IsNullOrEmpty(txtpassword.Text) || string.IsNullOrEmpty(txtconfirmpass.Text))
            {
                foreach (var c in this.Controls.OfType<Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox>())
                {
                    c.BorderColorIdle = Color.FromArgb(220, 20, 60);
                    c.PlaceholderForeColor = Color.FromArgb(220, 20, 60);
                }
            }
            else
            {
                if (txtpassword.Text == txtconfirmpass.Text)
                {
                    CheckUsernameAndID(txtusername.Text, lblid.Text);
                }
                else
                {
                    Bunifu.Snackbar.Show(this, "Password Mismatch", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                }
            }
        }
        //private void CheckUsername(string yourusername)
        //{
        //    try
        //    {
        //        using (SQLiteConnection sqlcon1 = new SQLiteConnection(conn))
        //        {
        //            sqlcon1.Open();
        //            string query = "SELECT Username From Users WHERE Username=@UN";
        //            using (SQLiteCommand sqlcmd1 = new SQLiteCommand(query, sqlcon1))
        //            {
        //                string username = string.Empty;
        //                try
        //                {
        //                    sqlcmd1.Parameters.AddWithValue("@UN", yourusername);
        //                    SQLiteDataReader dr1 = sqlcmd1.ExecuteReader();
        //                    if (dr1.HasRows)
        //                    {
        //                        while (dr1.Read())
        //                        {
        //                            username = dr1.GetString(0).ToString();
        //                        }
        //                    }
        //                    else
        //                    {
        //                        UpdateAccount();
        //                    }
        //                }
        //                finally
        //                {
        //                    if (sqlcon1.State == ConnectionState.Open)
        //                    {
        //                        sqlcon1.Close();
        //                        if (username == yourusername)
        //                        {
        //                            CheckUsernameAndID(yourusername, lblid.Text);
        //                        }
        //                        else
        //                        {
        //                            CheckUsernameAndID(yourusername, lblid.Text);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
        private void CheckUsernameAndID(string yourusername, string yourid)
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "SELECT UsersID,Username From Users WHERE UsersID=@ID AND Username=@UN";
                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        string usersid1 = string.Empty;
                        string username = string.Empty;
                        try
                        {
                            sqlcmd.Parameters.AddWithValue("@ID", lblid.Text);
                            sqlcmd.Parameters.AddWithValue("@UN", yourusername);
                            SQLiteDataReader dr = sqlcmd.ExecuteReader();

                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    usersid1 = Convert.ToString(dr.GetInt32(0));
                                    username = dr.GetString(1).ToString();
                                }
                            }
                            else
                            {
                                UpdateAccount();
                            }
                        }
                        finally
                        {
                            if (sqlcon.State == ConnectionState.Open)
                            {
                                sqlcon.Close();
                                CheckUsersID(usersid1, username);
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
        private void CheckUsersID(string userid, string username)
        {
            if (username == txtusername.Text && userid == lblid.Text)
            {
                UpdateAccount();
            }
        }
        private void UpdateAccount()
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "UPDATE Users SET Name=@Name, Username=@Username, Password=@Password WHERE UsersID=@ID";
                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        try
                        {
                            sqlcmd.Parameters.AddWithValue("@ID", lblid.Text);
                            sqlcmd.Parameters.AddWithValue("@Name", txtname.Text);
                            sqlcmd.Parameters.AddWithValue("@Username", txtusername.Text);
                            sqlcmd.Parameters.AddWithValue("@Password", txtpassword.Text);
                            sqlcmd.ExecuteNonQuery();
                            Bunifu.Snackbar.Show(this, "Success", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Success);
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
            catch (Exception)
            {
                Bunifu.Snackbar.Show(this, "Username Already Exist", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
            }
        }

        private void gunaGradientButton2_Click(object sender, EventArgs e)
        {
            foreach (var c in this.Controls.OfType<Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox>())
            {
                c.Clear();
            }
        }
    }
}
