using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProgramingContestImageSort;

namespace ProconSortUI
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void fileSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            var ppme = new PpmEdit();
            var isort = new ImageSort();
            var ic = new ImageCreate();
            var pieces = new Pieces();
            ofd.ShowDialog();
            pictureBox1.Image = ic.ppmCut(isort.sort(ofd.FileName));
            pictureBox1.Width = PpmData.picWidth;
            pictureBox1.Height = PpmData.picHeight;
            this.Width = PpmData.picWidth + 16;
            this.Height = PpmData.picHeight + 64;
        }
    }
}
