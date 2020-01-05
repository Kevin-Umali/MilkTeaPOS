using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DripTea.ViewForm.POSForm
{
    public partial class EnterCashForm : Form
    {
        public POSForm _posform;
        decimal Total = 0;
        public EnterCashForm(POSForm posform, bool visibility)
        {
            InitializeComponent();
            _posform = posform;
            //this.Size = new Size(this.Width, posform.Size.Height);
            //int plusX = posform.Size.Width - this.Size.Width;
            //int minusY = posform.Size.Height - this.Size.Height;
            //this.Location = new Point(posform.Location.X + plusX, posform.Location.Y + minusY);
            bunifuSeparator2.Visible = visibility;

            if (visibility)
                panel1.Dispose();
            else
                this.BackColor = Color.White;
        }

        private void EnterCashForm_Load(object sender, EventArgs e)
        {
            Total = Convert.ToDecimal(_posform.lbltotal.Text.Substring(1));
            txtcash.Focus();
        }

        private void EnterCashForm_Activated(object sender, EventArgs e)
        {
            Total = Convert.ToDecimal(_posform.lbltotal.Text.Substring(1));
            this.WindowState = FormWindowState.Normal;
            this.Show();
        }

        private void txtcash_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtcash.Text))
            {
                if (txtcash.Text.Length <= 16)
                {
                    if (Convert.ToDouble(txtcash.Text) <= 9999999999999999)
                    {
                        if (txtcash.Text.Contains(".."))
                        {
                            txtcash.Text = txtcash.Text.Substring(0, txtcash.Text.Length - 1);
                        }

                        _posform.lblcash.Text = txtcash.Text;
                    }
                    else
                    {
                        txtcash.Text = txtcash.Text.Substring(0, txtcash.Text.Length - 1);
                    }
                }
                else
                {
                    txtcash.Text = txtcash.Text.Substring(0, txtcash.Text.Length - 1);
                }
            }
            else
            {
                _posform.lblcash.Text = "0";
            }
        }

        private void txtcash_KeyPress(object sender, KeyPressEventArgs e)
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

        private void gunaGradientButton9_Click(object sender, EventArgs e)
        {
            Guna.UI.WinForms.GunaGradientButton btn = (Guna.UI.WinForms.GunaGradientButton)sender;
            txtcash.Text += btn.Text;
        }

        private void gunaGradientButton11_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtcash.Text))
            {
                txtcash.Text = txtcash.Text.Substring(0, txtcash.Text.Length - 1);
            }
        }

        private void gunaGradientButton22_Click(object sender, EventArgs e)
        {
            txtcash.Clear();
        }

        private void gunaGradientButton21_Click(object sender, EventArgs e)
        {
            Guna.UI.WinForms.GunaGradientButton btn = (Guna.UI.WinForms.GunaGradientButton)sender;
            if (!string.IsNullOrEmpty(txtcash.Text))
            {
                double cash = Convert.ToDouble(txtcash.Text);
                double plusCash = Convert.ToDouble(btn.Tag);
                double value = cash + plusCash;
                txtcash.Text = value.ToString();
            }
            else
            {
                double cash = 0;
                double plusCash = Convert.ToDouble(btn.Tag);
                double value = cash + plusCash;
                txtcash.Text = value.ToString();
            }
        }

        private void gunaGradientButton23_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void gunaGradientButton24_Click(object sender, EventArgs e)
        {
            //if (_posform.lbldock.Text == "false")
                _posform.lbldock.Text = "true";
            //else
                //_posform.lbldock.Text = "false";

            this.Dispose();
        }

        private void gunaGradientButton25_Click(object sender, EventArgs e)
        {
            _posform.lbldock.Text = "false";
            this.Dispose();
        }
    }
}
