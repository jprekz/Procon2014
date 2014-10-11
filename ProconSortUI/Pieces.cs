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
    public partial class Pieces : Form
    {
        public Pieces()
        {
            InitializeComponent();
        }

        public void ImageDraw(Image image)
        {
            this.pictureBox1.Image = image;
            pictureBox1.Width = image.Width;
            pictureBox1.Height = image.Height;
            this.Width = image.Width + 36;
            this.Height = image.Height + 64;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            byte[] selectpiece = new byte[2];
            var mouseposition = pictureBox1.PointToClient(Cursor.Position);
            selectpiece[0] = (byte)(mouseposition.X / (pictureBox1.Image.Width / PpmData.picDivision[0]));
            selectpiece[1] = (byte)(mouseposition.Y / (pictureBox1.Image.Height / PpmData.picDivision[1]));
            ((Main)this.Owner).selectpiece = selectpiece;
        }

    }
}
