using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DripTea.ViewForm.AdminForm
{
    public partial class AddAcountForm : Form
    {
        protected string conn = "Data source=driptea.db;Version=3;New=False;Compress=True";
        public AddAcountForm(Form parent)
        {
            InitializeComponent();
            Guna.UI.Lib.GraphicsHelper.ShadowForm(this);
            this.Size = new Size(this.Width, parent.Size.Height);
            int plusX = parent.Size.Width - this.Size.Width;
            this.Location = new Point(parent.Location.X + plusX, parent.Location.Y);
        }

        private void AddAcountForm_Load(object sender, EventArgs e)
        {
            Guna.UI.Lib.GraphicsHelper.DrawLineShadow(panel4, Color.WhiteSmoke, 20, 5, Guna.UI.WinForms.VerHorAlign.HoriziontalTop);
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void gunaGradientButton1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtname.Text) || string.IsNullOrEmpty(txtusername.Text) ||
                string.IsNullOrEmpty(txtpassword.Text) || string.IsNullOrEmpty(txtconfirmpass.Text))
            {
                foreach(var c in this.Controls.OfType<Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox>())
                {
                    c.BorderColorIdle = Color.FromArgb(220, 20, 60);
                    c.PlaceholderForeColor = Color.FromArgb(220, 20, 60);
                }
            }
            else
            {
                if(txtpassword.Text == txtconfirmpass.Text)
                {
                    CheckUsername(txtusername.Text);
                }
                else
                {
                    Bunifu.Snackbar.Show(this, "Password Mismatch", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                }
            }
        }

        private void CheckUsername(string yourusername)
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "SELECT Username From Users WHERE Username=@UN";
                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        try
                        {
                            sqlcmd.Parameters.AddWithValue("@UN", yourusername);
                            SQLiteDataReader dr = sqlcmd.ExecuteReader();
                            if(dr.HasRows)
                            {
                                while(dr.Read())
                                {
                                    string username = dr.GetString(0).ToString();
                                    if(username.Equals(yourusername))
                                    {
                                        Bunifu.Snackbar.Show(this, "Username Already Exist", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Error);
                                    }
                                    else
                                    {
                                        AddAccount();
                                    }
                                }
                            }
                            else
                            {
                                AddAccount();
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

        private void AddAccount()
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "INSERT INTO Users VALUES (@ID,@Name,@Username,@Password,@Usertype,True)";
                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        try
                        {
                            sqlcmd.Parameters.AddWithValue("@ID", null);
                            sqlcmd.Parameters.AddWithValue("@Name", txtusername.Text);
                            sqlcmd.Parameters.AddWithValue("@Username", txtusername.Text);
                            sqlcmd.Parameters.AddWithValue("@Password", txtusername.Text);
                            string usertype = string.Empty;
                            if(bunifuToggleSwitch1.Value == true)
                            {
                                usertype = "Administrator";   
                            }
                            else
                            {
                                usertype = "Cashier";
                            }
                            sqlcmd.Parameters.AddWithValue("@Usertype", usertype);
                            sqlcmd.ExecuteNonQuery();
                            Bunifu.Snackbar.Show(this, "Success", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Success);
                        }
                        finally
                        {
                            if (sqlcon.State == ConnectionState.Open)
                            {
                                sqlcon.Close();
                                foreach (var c in this.Controls.OfType<Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox>())
                                {
                                    c.Clear();
                                }
                                bunifuToggleSwitch1.Value = true;
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
            foreach (var c in this.Controls.OfType<Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox>())
            {
                c.Clear();
            }
            bunifuToggleSwitch1.Value = true;
        }
    }
}
