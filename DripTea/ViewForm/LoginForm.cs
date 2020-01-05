using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SQLite;
using DripTea.ViewForm.AdminForm;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DripTea.ViewForm
{
    public partial class LoginForm : Form
    {
        protected string conn = "Data source=driptea.db;Version=3;New=False;Compress=True";
        static int attemptlimit = 3;
        int attempt = 0;
        public LoginForm()
        {
            InitializeComponent();
            Guna.UI.Lib.GraphicsHelper.ShadowForm(this);
            if (Properties.Settings.Default.Check)
            {
                txtusername.Text = Properties.Settings.Default.Username;
                txtpassword.Text = Properties.Settings.Default.Password;
                bunifuCheckBox1.Checked = Properties.Settings.Default.Check;
            }
        }

        int _counter = 0;
        int _len = 0;
        string _value;
        string[] _text = { "DripTea", "POS", "Point of Sale" };
        private void LoginForm_Load(object sender, EventArgs e)
        {
            _value = label4.Text;
            _len = _value.Length + 1;
            label4.Text = "";
            timer1.Start();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            _counter++;

            if (_counter >= _len)
            {
                timer1.Stop();
                timer3.Start();
            }
            else
            {
                label4.Text = _value.Substring(0, _counter) + "_";
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            _counter--;

            if (_counter <= _len)
            {
                if (_counter != 0)
                {
                    label4.Text = _value.Substring(0, _counter) + "_";
                }
                else
                {
                    label4.Text = "_";
                    Random rd = new Random();
                    _value = _text[rd.Next(0, 3)].ToString();
                    _len = _value.Length + 1;
                    label4.Text = "";
                    timer2.Stop();
                    timer1.Start();
                }
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {

            timer3.Stop();
            if (timer1.Enabled == false)
            {
                timer2.Start();
            }
        }

        private void TextChecker(string username, string password)
        {
            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                txtusername.BorderColorIdle = Color.FromArgb(220, 20, 60);
                txtpassword.BorderColorIdle = Color.FromArgb(220, 20, 60);
                txtusername.PlaceholderForeColor = Color.FromArgb(220, 20, 60);
                txtpassword.PlaceholderForeColor = Color.FromArgb(220, 20, 60);
            }
            else
            {
                if(attempt.Equals(attemptlimit))
                {
                    MessageBox.Show("Attempt Exceed. Closing Application Now");
                    Environment.Exit(1);
                }
                else
                {
                    LoginValidate(txtusername.Text, txtpassword.Text);
                }
            }
        }

        private void LoginValidate(string username, string password)
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "SELECT Name,UserType,Status from Users where Username=@Username and Password=@Password";
                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        try
                        {
                            sqlcmd.Parameters.AddWithValue("@Username", username);
                            sqlcmd.Parameters.AddWithValue("@Password", password);
                            SQLiteDataReader dr = sqlcmd.ExecuteReader();
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    string name = dr.GetString(0).ToString();
                                    string usertype = dr.GetString(1).ToString();
                                    bool status = dr.GetBoolean(2);
                                    UserTypeAndStatusChecker(name, usertype, status);
                                }
                            }
                            else
                            {
                                attempt++;
                                MessageBox.Show("Login Error");
                            }
                        }
                        finally
                        {
                            if(sqlcon.State == ConnectionState.Open)
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

        private void UserTypeAndStatusChecker(string name, string _usertype, bool _status)
        {
            if (_status == true)
            {
                Save();
                if (_usertype.Equals("Administrator"))
                {
                    MessageBox.Show("Login Successfully");
                    //Bunifu.Snackbar.Show(this, "Login Successfully", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Success);
                    attempt = 0;
                    this.Hide();
                    MainForm f1 = new MainForm(name);
                    f1.Closed += (s, args) => this.Dispose();
                    f1.Show();
                }
                else
                {
                    MessageBox.Show("Login Successfully");
                    //Bunifu.Snackbar.Show(this, "Login Successfully", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Success);
                    attempt = 0;
                    this.Hide();
                    POSForm.POSForm f1 = new POSForm.POSForm(name);
                    f1.Closed += (s, args) => this.Dispose();
                    f1.Show();
                }
            }
            else
            {
                attempt++;
                MessageBox.Show("Inactive Account");
            }
        }

        private void Save()
        {
            if(bunifuCheckBox1.Checked)
            {
                Properties.Settings.Default.Username = txtusername.Text;
                Properties.Settings.Default.Password = txtpassword.Text;
                Properties.Settings.Default.Check = bunifuCheckBox1.Checked;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.Username = string.Empty;
                Properties.Settings.Default.Password = string.Empty;
                Properties.Settings.Default.Check = bunifuCheckBox1.Checked;
                Properties.Settings.Default.Save();
            }
        }

        private void Text_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void Enter_KeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                gunaGradientButton1_Click(sender, e);
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }

        private void panel1_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            else if (this.WindowState == FormWindowState.Maximized)
                this.WindowState = FormWindowState.Normal;
        }

        private void gunaGradientButton1_Click(object sender, EventArgs e)
        {
            TextChecker(txtusername.Text, txtpassword.Text);
        }

        private void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            OpenUrl("https://www.facebook.com/dripteatagapo");
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            label6.Text = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss tt");
            label7.Text = DateTime.Now.ToString("dddd");
        }
    }
}
