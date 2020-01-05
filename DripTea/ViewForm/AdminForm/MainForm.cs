using DripTea.ViewUserControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DripTea.ViewForm.AdminForm
{
    public partial class MainForm : Form
    {
        static string name = string.Empty;
        public MainForm(string _name)
        {
            InitializeComponent();
            Guna.UI.Lib.GraphicsHelper.ShadowForm(this);
            name = _name;
            Bunifu.Snackbar.Show(this, "Welcome " + _name, 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Success);
            circlename.Text = GenerateNameCircle(_name);
            lblname.Text = _name;
        }
        //private void BarChange(object sender)
        //{
        //    bar.Top = ((Bunifu.UI.WinForms.BunifuButton.BunifuButton)sender).Top;
        //}
        private string GenerateNameCircle(string yourname)
        {
            string _name = string.Empty;
            int x = 0;
            string[] getinitials = yourname.Split(' ');
            foreach (string str in getinitials)
            {
                x++;
            }

            if (x == 1)
            {
                _name = getinitials[0].ToUpper().ToString();
            }
            else if (x >= 2)
            {
                _name = getinitials[0].Substring(0, 1).ToUpper().ToString()
                    + getinitials[1].Substring(0, 1).ToUpper().ToString();
            }
            return _name;
        }
        private void DisposableAndOpenControl(UserControl uc)
        {
            foreach(Control c in mainpanel.Controls)
            {
                mainpanel.Controls.Clear();
                c.Dispose();
            }

            uc.Dock = DockStyle.Fill;
            mainpanel.Controls.Add(uc);
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            Guna.UI.Lib.GraphicsHelper.DrawLineShadow(panel3, Color.Black, 20, 5, Guna.UI.WinForms.VerHorAlign.HoriziontalTop);
        }

        private void gunaAdvenceButton1_Click(object sender, EventArgs e)
        {
            DisposableAndOpenControl(new AccountUserControl());
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {

            if (this.WindowState == FormWindowState.Maximized)
            {
                panel6.Size = new Size(199, 104);
                pictureBox1.Size = new Size(174, 98);
            }
            else
            {
                panel6.Size = new Size(199, 60);
                pictureBox1.Size = new Size(49, 41);
            }
        }

        private void gunaAdvenceButton3_Click(object sender, EventArgs e)
        {
            DisposableAndOpenControl(new ProductUserControl());
        }

        private void gunaAdvenceButton4_Click(object sender, EventArgs e)
        {

        }

        private void gunaAdvenceButton5_Click(object sender, EventArgs e)
        {
            DisposableAndOpenControl(new MiscellaneousUserControl());
        }

        private void gunaAdvenceButton2_Click(object sender, EventArgs e)
        {

        }

        private void gunaAdvenceButton6_Click(object sender, EventArgs e)
        {
            DisposableAndOpenControl(new ProductGroupUserControl());
        }

        private void gunaControlBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm f1 = new LoginForm();
            f1.Closed += (s, args) => this.Dispose();
            f1.Show();
        }
    }
}
