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
using ProconFileIO;

namespace ProconSortUI
{
    public partial class Main : Form
    {
        public ClientLibrary cl;
        public Main()
        {
            InitializeComponent();
        }

        public byte[] drawpiece;
        public byte[] selectpiece = new byte[2];
        Pieces piecesform;

        private void fileSelect_Click(object sender, EventArgs e)
        {
            cl = new ClientLibrary();
            var isort = new ImageSort();
            var ic = new ImageCreate();
            var pieces = new Pieces();
            piecesform = new Pieces();
            var ir = new ImageResize();
            var id = new InputDialog();
            id.ShowDialog();
            var sortedpiece = isort.sort(cl.GetProblemPath(id.result));
            string piecesline = "";
            piecesline += sortedpiece[0].ToString() + " " + sortedpiece[1].ToString();
            for (int i = 2; i < sortedpiece.Length; i+=2 )
            {
                piecesline +=  "," + sortedpiece[i].ToString() + " " + sortedpiece[i + 1].ToString();
            }
            pictureBox1.Image = ir.resize(ic.ppmCut(sortedpiece),500);
            pictureBox1.Width = pictureBox1.Image.Width;
            pictureBox1.Height = pictureBox1.Image.Height;
            piecesform.Show(this);
            byte[] origin = new byte[sortedpiece[0]*sortedpiece[1]*2+2];
            origin[0] = sortedpiece[0];
            origin[1] = sortedpiece[1];
            for(int y = 0; y < origin[1];y++)
            {
                for(int x = 0;x < origin[0];x++)
                {
                    origin[(y*origin[0]+x)*2+2] = (byte)x;
                    origin[(y*origin[0]+x)*2+3] = (byte)y;
                }
            }
            piecesform.ImageDraw(ir.resize(ic.ppmCut(origin),400));
            drawpiece = sortedpiece;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var ic = new ImageCreate();
            var ir = new ImageResize();
            var mouseposition = pictureBox1.PointToClient(Cursor.Position);
            var x = mouseposition.X / (pictureBox1.Image.Width / PpmData.picDivision[0]);
            var y = mouseposition.Y / (pictureBox1.Image.Height / PpmData.picDivision[1]);
            var pieceposition = ((mouseposition.X / (pictureBox1.Image.Width / PpmData.picDivision[0])) + (mouseposition.Y / (pictureBox1.Image.Height / PpmData.picDivision[1])) * PpmData.picDivision[0]) * 2 + 2;
            for(int i = 0;i < 2;i++)
            {
                drawpiece[pieceposition + i] = selectpiece[i];
            }
            pictureBox1.Image = ir.resize(ic.ppmCut(drawpiece), 500);
        }

        public void imgdraw(Image image)
        {
            var ir = new ImageResize();
            pictureBox1.Image = ir.resize(image, 500);
        }

        private void decide_Click(object sender, EventArgs e)
        {
            piecesform.Close();
            this.Close();
        }
    }
}