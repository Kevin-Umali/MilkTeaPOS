using DripTea.Card;
using DripTea.ViewUserControl;
using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DripTea.ViewForm.POSForm
{
    public partial class POSForm : Form
    {
        protected string conn = "Data source=driptea.db;Version=3;New=False;Compress=True";
        private string name = string.Empty;

        decimal total = 0;
        decimal discounted = 0;
        decimal discountedTotal = 0;
        public POSForm(string _name)
        {
            InitializeComponent();
            Guna.UI.Lib.GraphicsHelper.ShadowForm(this);
            name = _name;
            DisposableAndOpenControl(new MilkTeaUserControl());
            circlename.Text = GenerateNameCircle(_name);
            lblname.Text = _name;
        }

        private string GenerateNameCircle(string yourname)
        {
            string _name = string.Empty;
            int x = 0;
            string[] getinitials = yourname.Split(' ');
            foreach(string str in getinitials)
            {
                x++;
            }

            if(x == 1)
            {
                _name = getinitials[0].ToUpper().ToString();
            }
            else if(x >= 2)
            {
                _name = getinitials[0].Substring(0,1).ToUpper().ToString() 
                    + getinitials[1].Substring(0, 1).ToUpper().ToString();
            }
            return _name;
        }
        private void DisposableAndOpenControl(UserControl uc)
        {
            foreach (Control c in mainpanel.Controls)
            {
                mainpanel.Controls.Clear();
                c.Dispose();
            }
            uc.Dock = DockStyle.Fill;
            mainpanel.Controls.Add(uc);
        }

        public void POSForm_Load(object sender, EventArgs e)
        {
            Bunifu.Snackbar.Show(this, "Welcome " + name, 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Success);
            LoadData();
            lblorderno.Text = string.Format("{0}", 
                Properties.Settings.Default.OrderNo.ToString());

            //decimal vat;
            //decimal sVat;
            //decimal sum;

            //vat = 1.12m; // Percent

            //sVat = (decimal)(1000 / 1.12m);
            //vat = 1000 - sVat;
            //sum = sVat + vat;

            //MessageBox.Show(sVat.ToString());
            //MessageBox.Show(vat.ToString());
            //MessageBox.Show(total.ToString());

        }

        private void CalculateAll()
        {
            if (total != null || total >= 0)
            {
                decimal fixVatPercentage = 1.12m;
                decimal vat;
                decimal vatable;

                vatable = Math.Round(total / fixVatPercentage, 2, MidpointRounding.AwayFromZero);
                lblvatable.Text = "₱" + vatable.ToString();
                vat = Math.Round(total - vatable, 2, MidpointRounding.AwayFromZero);
                lblvat.Text = "₱" + vat.ToString();


                lbltotal.Text = String.Format("₱{0:0.##}", total.ToString());
            }
        }
        public void LoadData()
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "Select * From Cart Order By ProductName";
                    DataTable dt = new DataTable();
                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        using (SQLiteDataReader dr = sqlcmd.ExecuteReader())
                        {
                            try
                            {
                                dt.Clear();
                                dt.Dispose();

                                dt.Load(dr);
                                //bunifuCustomDataGrid1.DataSource = dt;
                            }
                            finally
                            {
                                if (sqlcon.State == ConnectionState.Open)
                                {
                                    sqlcon.Close();
                                    flowLayoutPanel1.Controls.Clear();
                                    foreach (Control c in flowLayoutPanel1.Controls)
                                    {
                                        flowLayoutPanel1.Controls.Clear();
                                        c.Dispose();
                                    }
                                    total = 0;
                                    LoadCartCard(dt);
                                    CalculateAll();
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

        private void LoadCartCard(DataTable dt)
        {
            if(dt.Rows.Count >= 1)
            {
                foreach(DataRow dr in dt.Rows)
                {
                    addCartCard(dr["ID"].ToString(), dr["ProductName"].ToString(),
                        dr["MiscItem"].ToString(), dr["Size"].ToString(), dr["Quantity"].ToString(),
                        dr["TotalPrice"].ToString(), dr["Price"].ToString());

                    total += Convert.ToDecimal(dr["TotalPrice"].ToString());
                }
            }
            lblcart1.Text = string.Format("({0})", dt.Rows.Count.ToString());
        }

        private async void addCartCard(string id, string productname, string miscitem,
            string size, string qty, string price, string productprice)
        {
            
            CartOrderCard coc = new CartOrderCard(id, productname, miscitem, size, qty, price, productprice);
            await Task.Run(() =>
            {
                if (this.flowLayoutPanel1.InvokeRequired)
                {
                    this.flowLayoutPanel1.Invoke(new MethodInvoker(
                                    delegate
                                    {
                                        flowLayoutPanel1.Controls.Add(coc);
                                    }
                                    )
                        );
                }
            }); 
                
        }

        private void gunaAdvenceButton1_Click(object sender, EventArgs e)
        {
            DisposableAndOpenControl(new MilkTeaUserControl());
        }

        private void gunaControlBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm f1 = new LoginForm();
            f1.Closed += (s, args) => this.Dispose();
            f1.Show();
        }

        private void POSForm_Resize(object sender, EventArgs e)
        {
            //if(this.WindowState == FormWindowState.Maximized)
            //{
            //    //int previouswidth = 1200;
            //    //int newwidth = this.Size.Width;
            //    //int plusX = Math.Abs(previouswidth - newwidth);
            //    //this.panel6.Width = panel6.Width + plusX;
            //}
            //else
            //{
            //    //panel6.Size = new System.Drawing.Size(380, 700);
            //}
        }

        private void txtDiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            if (total != null || total >= 0)
            {
                if (!string.IsNullOrEmpty(txtDiscount.Text))
                {
                    if (Convert.ToInt32(txtDiscount.Text) >= 101)
                    {
                        txtDiscount.Text = "100";

                        decimal discount = 100;
                        discounted = (total * discount / 100);
                        lbldiscountprice.Text = "₱" + discounted.ToString();

                        discountedTotal = Math.Round(total - discounted, 2, MidpointRounding.AwayFromZero);
                        lbltotal.Text = String.Format("₱{0:0.##}", discountedTotal.ToString());
                    }
                    else
                    {
                        decimal discount = Convert.ToDecimal(this.txtDiscount.Text);
                        discounted = (total * discount / 100);
                        lbldiscountprice.Text = "₱" + discounted.ToString();

                        discountedTotal = Math.Round(total - discounted, 2, MidpointRounding.AwayFromZero);
                        lbltotal.Text = String.Format("₱{0:0.##}", discountedTotal.ToString());
                    }
                }
                else
                {
                    discountedTotal = 0;
                    lbldiscountprice.Text = "₱" + discountedTotal.ToString();
                    lbltotal.Text = String.Format("₱{0:0.##}", total.ToString());
                }
            }
        }

        private void gunaGradientButton1_Click(object sender, EventArgs e)
        {
            txtdock_TextChanged(sender, e);
        }

        private void txtdock_TextChanged(object sender, EventArgs e)
        {
            bool checkDock = Convert.ToBoolean(lbldock.Text);
            if(checkDock)
            {
                gunaGradientButton1.Text = "[F1] ENTER CASH (DOCK)";
                if (Application.OpenForms["EnterCashForm"] == null)
                {
                    var ecf = new EnterCashForm(this, true);
                    ecf.TopLevel = false;
                    ecf.Dock = DockStyle.Bottom;
                    ecf.Show();
                    this.Controls.Add(ecf);


                    panel6.SendToBack();
                }
                else
                {
                    Application.OpenForms["EnterCashForm"].Activate();
                }
            }
            else
            {
                gunaGradientButton1.Text = "[F1] ENTER CASH (OPEN)";
                if (Application.OpenForms["EnterCashForm"] == null)
                {
                    var ecf = new EnterCashForm(this, false);
                    ecf.Show();
                }
                else
                {
                    Application.OpenForms["EnterCashForm"].Activate();
                }
            }
        }

        private void lblcash_TextChanged(object sender, EventArgs e)
        {
            if (total != null || total >= 0)
            {
                if(!string.IsNullOrEmpty(lblcash.Text))
                {
                    var TotalPrice = Convert.ToDecimal(lbltotal.Text.Substring(1));
                    var Cash = Convert.ToDecimal(lblcash.Text);

                    decimal change = Math.Round(Cash - TotalPrice, 2, MidpointRounding.AwayFromZero);
                    lblchange.Text = "₱" + change.ToString();
                }
                else
                {
                    lblchange.Text = "₱0";
                }
            }
        }

        private void lbltotal_TextChanged(object sender, EventArgs e)
        {
            if (total != null || total >= 0)
            {
                if (!string.IsNullOrEmpty(lblcash.Text))
                {
                    var TotalPrice = Convert.ToDecimal(lbltotal.Text.Substring(1));
                    var Cash = Convert.ToDecimal(lblcash.Text);

                    decimal change = Math.Round(Cash - TotalPrice, 2, MidpointRounding.AwayFromZero);
                    lblchange.Text = "₱" + change.ToString();
                }
                else
                {
                    lblchange.Text = "₱0";
                }
            }
        }

        private void gunaGradientButton2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblchange.Text) && !string.IsNullOrEmpty(lblcash.Text)
                && !string.IsNullOrEmpty(lbltotal.Text))
            {
                decimal changed = Convert.ToDecimal(lblchange.Text.Substring(1));
                decimal cash = Convert.ToDecimal(lblcash.Text);
                decimal total = Convert.ToDecimal(lbltotal.Text.Substring(1));
                if(total != 0 && total >= 0)
                {
                    if (cash <= 0 && cash == 0)
                    {
                        Bunifu.Snackbar.Show(this, "Please enter cash to order.", 
                            3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Information);
                    }
                    else if (cash <= total)
                    {
                        Bunifu.Snackbar.Show(this, "Not enough cash to proceed", 
                            3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Information);
                    }
                    else
                    {
                        if (changed >= 0)
                        {
                            AddToOrder();
                        }
                    }
                }
            }
        }

        private void AddToOrder()
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "INSERT INTO Orders (OrderNo, ProductID, ProductName, Size, Quantity, Price, Status) " +
                        "SELECT @OrderNo, ID, ProductName, Size, Quantity, TotalPrice, 'Pending' FROM Cart";
                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        try
                        {
                            sqlcmd.Parameters.AddWithValue("@OrderNo", lblorderno.Text);
                            sqlcmd.ExecuteNonQuery();
                        }
                        finally
                        {
                            if (sqlcon.State == ConnectionState.Open)
                            {
                                sqlcon.Close();
                                Bunifu.Snackbar.Show(this, "Success", 3000, Snackbar.Views.SnackbarDesigner.MessageTypes.Success);
                                DialogResult result = MessageBox.Show("Do you want to print receipt",
                                "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (result == DialogResult.Yes)
                                {
                                    if (Application.OpenForms["ReceiptPrintForm"] == null)
                                    {
                                        ReceiptPrintForm f1 =
                                            new ReceiptPrintForm(lblorderno.Text, lblvatable.Text,
                                            lblvat.Text, txtDiscount.Text, lbldiscountprice.Text,
                                            lbltotal.Text, lblcash.Text, lblchange.Text,
                                            name);

                                        new PopupEffect.transparentBg(this.FindForm(), f1);
                                    }
                                }
                                else if (result == DialogResult.No)
                                {
                                    //Do Nothing
                                }
                                TruncateUpdateAndVacuum();
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

        private void TruncateUpdateAndVacuum()
        {
            try
            {
                using (SQLiteConnection sqlcon = new SQLiteConnection(conn))
                {
                    sqlcon.Open();
                    string query = "DELETE FROM Cart; " +
                        "update sqlite_sequence set seq = 0 where name = 'Cart'; " +
                        "VACUUM; ";

                    using (SQLiteCommand sqlcmd = new SQLiteCommand(query, sqlcon))
                    {
                        try
                        {
                            sqlcmd.ExecuteNonQuery();
                        }
                        finally
                        {
                            if (sqlcon.State == ConnectionState.Open)
                            {
                                sqlcon.Close();
                                LoadData();

                                Properties.Settings.Default.OrderNo += 1;
                                Properties.Settings.Default.Save();
                                lblorderno.Text = string.Format("{0}",
                                    Properties.Settings.Default.OrderNo.ToString());


                                lblcash.Text = "0";

                                if (Application.OpenForms["OrderStatusForm"] != null)
                                {
                                    OrderStatusForm f2 = (OrderStatusForm)Application.OpenForms["OrderStatusForm"];
                                    f2.OrderStatusForm_Load(f2, EventArgs.Empty);
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

        private void gunaAdvenceButton2_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["OrderStatusForm"] == null)
            {
                //mainpanel.Controls.Clear();
                //foreach (Control ctrl in mainpanel.Controls)
                //    ctrl.Dispose();

                var osf = new OrderStatusForm();
                //osf.Dock = DockStyle.Bottom;
                osf.Show();
            }

        }

        private void POSForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                // do stuff
                gunaGradientButton1_Click(sender, e);
            }

            if (e.KeyCode == Keys.F2)
            {
                // do stuff
                gunaGradientButton2_Click(sender, e);
            }
        }

        private void gunaAdvenceButton3_Click(object sender, EventArgs e)
        {
            DisposableAndOpenControl(new TransactionHistoryUserControl());
        }

        private void gunaAdvenceButton4_Click(object sender, EventArgs e)
        {

        }

        //private bool CheckOpenForm()
        //{
        //    bool FormOpen = false;

        //    FormCollection fc = Application.OpenForms;
        //    foreach (Form frm in fc)
        //    {
        //        //iterate through
        //        if (frm.Name == "EnterCashForm")
        //        {
        //            FormOpen = true;
        //        }
        //    }
        //    return FormOpen;
        //}
    }
}
