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
        public static ClientLibrary cl;

        public Main()
        {
            InitializeComponent();
        }

        public byte[] drawpiece;
        public byte[] selectpiece = new byte[2];
        Pieces piecesform;

        private void Main_Load(object sender, EventArgs e)
        {
            decide.Enabled = false;
        }

        public void decideEnabled()
        {
            decide.Enabled = true;
        }

        private void fileSelect_Click(object sender, EventArgs e)
        {
            cl = new ClientLibrary();
            var isort = new ImageSort();
            var ic = new ImageCreate();
            var pieces = new Pieces();
            piecesform = new Pieces();
            var ir = new ImageResize();
            var id = new InputDialog();
            string result;
            do
            {
                id.ShowDialog();
                result = cl.GetProblemPath(id.result);
                id.labelwrite(result);
            } while (result != "C:\\Users\\" + Environment.UserName + "\\Desktop\\" + string.Format("prob{0:D2}.ppm", id.result));
            var sortedpiece = isort.sort(result);
            string piecesline = "";
            piecesline += sortedpiece[0].ToString() + " " + sortedpiece[1].ToString();
            for (int i = 2; i < sortedpiece.Length; i+=2 )
            {
                piecesline +=  "," + sortedpiece[i].ToString() + " " + sortedpiece[i + 1].ToString();
            }
            pictureBox1.Image = ir.resize(ic.ppmCut(sortedpiece),500);
            this.enableddecide(sortedpiece);
            pictureBox1.Width = pictureBox1.Image.Width;
            pictureBox1.Height = pictureBox1.Image.Height;
            piecesform.Show(this);
            pieceformDraw(sortedpiece);
            drawpiece = sortedpiece;
        }

        public void pieceformDraw(byte[] sortedpiece)
        {
            var ic = new ImageCreate();
            var ir = new ImageResize();
            byte[] origin = new byte[sortedpiece[0] * sortedpiece[1] * 2 + 2];
            origin[0] = sortedpiece[0];
            origin[1] = sortedpiece[1];
            for (int y = 0; y < origin[1]; y++)
            {
                for (int x = 0; x < origin[0]; x++)
                {
                    origin[(y * origin[0] + x) * 2 + 2] = (byte)x;
                    origin[(y * origin[0] + x) * 2 + 3] = (byte)y;
                }
            }
            bool[] colorpiece = new bool[PpmData.picDivision[0] * PpmData.picDivision[1]];
            for (int i = 0; i < colorpiece.Length; i++)
            {
                colorpiece[i] = true;
            }
            for (int i = 2; i < origin.Length; i += 2)
            {
                for (int j = 2; j < sortedpiece.Length; j += 2)
                {
                    if (origin[i] == sortedpiece[j] && origin[i + 1] == sortedpiece[j + 1])
                    {
                        colorpiece[(i - 2) / 2] = false;
                    }
                }
            }
            piecesform.ImageDraw(ir.resize(ic.ppmCut(origin, colorpiece), 400));
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var ic = new ImageCreate();
            var ir = new ImageResize();
            var mouseposition = pictureBox1.PointToClient(Cursor.Position);
            var x = mouseposition.X / (pictureBox1.Image.Width / PpmData.picDivision[0]);
            var y = mouseposition.Y / (pictureBox1.Image.Height / PpmData.picDivision[1]);
            var pieceposition = ((int)(mouseposition.X / (pictureBox1.Image.Width / (double)PpmData.picDivision[0])) + (int)(mouseposition.Y / (pictureBox1.Image.Height / (double)PpmData.picDivision[1])) * PpmData.picDivision[0]) * 2 + 2;
            for(int i = 0;i < 2;i++)
            {
                drawpiece[pieceposition + i] = selectpiece[i];
            }
            pictureBox1.Image = ir.resize(ic.ppmCut(drawpiece), 500);
            enableddecide(drawpiece);
            pieceformDraw(drawpiece);
        }

        public void imageDraw(byte[] sortedpiece)
        {
            var ir = new ImageResize();
            var ic = new ImageCreate();
            pictureBox1.Image = ir.resize(ic.ppmCut(sortedpiece), 500);
            enableddecide(sortedpiece);
        }

        public void enableddecide(byte[] sortedpiece)
        {
            var isfound = true;
            var isnotassign = true;
            for (int i = 2; i < sortedpiece.Length; i+=2)
            {
                if (sortedpiece[i] == 16)
                {
                    isfound = false;
                }
                for (int j = 2; j < sortedpiece.Length; j+=2)
                {
                    if (sortedpiece[i] == sortedpiece[j] && sortedpiece[i+1] == sortedpiece[j+1] && i != j)
                    {
                        isnotassign = false;
                    }
                }
            }
            if (isfound && isnotassign)
            {
                this.decide.Enabled = true;
            }
        }

        private void decide_Click(object sender, EventArgs e)
        {
            piecesform.Close();
            this.Close();
        }

        public void pieceLotate(Keys key)
        {
            var pieces = new byte[drawpiece.Length];
            switch(key)
            {
                case Keys.J:
                    pieces[0] = drawpiece[0];
                    pieces[1] = drawpiece[1];
                    for(int i = 2;i < drawpiece.Length;i+=2)
                    {
                        if(i < drawpiece[0]*2+2)
                        {
                            pieces[i] = drawpiece[(drawpiece[0] * (drawpiece[1] - 1))*2 + i];
                            pieces[i+1] = drawpiece[(drawpiece[0] * (drawpiece[1] - 1)) * 2 + i+1];
                        }
                        else
                        {
                            pieces[i] = drawpiece[i - drawpiece[0]*2];
                            pieces[i+1] = drawpiece[i+1 - drawpiece[0]*2];
                        }
                    }
                    drawpiece = pieces;
                    this.imageDraw(drawpiece);
                    pieceformDraw(drawpiece);
                    break;
                case Keys.K:
                    pieces[0] = drawpiece[0];
                    pieces[1] = drawpiece[1];
                    for(int i = 2;i < drawpiece.Length;i+=2)
                    {
                        if (i < (drawpiece[0] * (drawpiece[1] - 1)) * 2 + 2)
                        {
                            pieces[i] = drawpiece[i + drawpiece[0] * 2];
                            pieces[i + 1] = drawpiece[i + 1 + drawpiece[0] * 2];
                        }
                        else
                        {
                            pieces[i] = drawpiece[i - drawpiece[0]*2*(drawpiece[1]-1)];
                            pieces[i+1] = drawpiece[i+1 - drawpiece[0]*2*(drawpiece[1]-1)];
                        }
                    }
                    drawpiece = pieces;
                    this.imageDraw(drawpiece);
                    pieceformDraw(drawpiece);
                    break;
                case Keys.L:
                    pieces[0] = drawpiece[0];
                    pieces[1] = drawpiece[1];
                    for(int i = 2;i < drawpiece.Length;i+=2)
                    {
                        if ((i - 2) % (drawpiece[0] * 2) > 0)
                        {
                            pieces[i] = drawpiece[i - 2];
                            pieces[i + 1] = drawpiece[i - 1];
                        }
                        else
                        {
                            pieces[i] = drawpiece[i + (drawpiece[0] - 1) * 2];
                            pieces[i + 1] = drawpiece[i + 1 + (drawpiece[0] - 1) * 2];
                        }
                    }
                    drawpiece = pieces;
                    this.imageDraw(drawpiece);
                    pieceformDraw(drawpiece);
                    break;
                case Keys.H:
                    pieces[0] = drawpiece[0];
                    pieces[1] = drawpiece[1];
                    for (int i = 2; i < drawpiece.Length; i += 2)
                    {
                        if ((i - 2) / 2 % drawpiece[0] < drawpiece[0] - 1)
                        {
                            pieces[i] = drawpiece[i + 2];
                            pieces[i + 1] = drawpiece[i + 3];
                        }
                        else
                        {
                            pieces[i] = drawpiece[i - (drawpiece[0]-1)*2];
                            pieces[i + 1] = drawpiece[i + 1 - (drawpiece[0] - 1) * 2];
                        }
                    }
                    drawpiece = pieces;
                    this.imageDraw(drawpiece);
                    pieceformDraw(drawpiece);
                    break;
                    
            }
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            pieceLotate(e.KeyData);
        }

    }
}