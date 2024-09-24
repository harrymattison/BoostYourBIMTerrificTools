using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoostYourBIMTerrificTools.PinComment
{
    public partial class PinCommentForm : Form
    {
        public PinCommentForm()
        {
            InitializeComponent();
        }

        public string getComment()
        {
            return txtPin.Text;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
