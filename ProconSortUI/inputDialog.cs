using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProconSortUI
{
    public partial class InputDialog : Form
    {
        public InputDialog()
        {
            InitializeComponent();
        }

        public int result;

        private void submit_Click(object sender, EventArgs e)
        {
            result = int.Parse(problemnum.Text);
            this.Close();
            return;
        }
    }
}
