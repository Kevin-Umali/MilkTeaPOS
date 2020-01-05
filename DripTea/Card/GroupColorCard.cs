using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DripTea.Card
{
    public partial class GroupColorCard : UserControl
    {
        public GroupColorCard(Color c, string name)
        {
            InitializeComponent();
            gunaCirclePictureBox1.BaseColor = c;
            lblgroupname.Text = name;
        }

        private void GroupColorCard_Load(object sender, EventArgs e)
        {

        }
    }
}
