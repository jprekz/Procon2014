using ProgramingContest3;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PuzzleSolving;
using ProconSortUI;

namespace Procon2014
{
    public partial class Form1 : Form
    {
        private IPuzzleSolving p;
        private System.Diagnostics.Stopwatch sw;
        public Form1()
        {
            InitializeComponent();
        }

        private void initializeManagement()
        {
            var gsp = new GetSortedPiece();
            byte[,] sorted = gsp.getsortedpiece();
            byte[,] cells = (byte[,])sorted.Clone();
            for (int y = 0; y != cells.GetLength(1); y++)
            {
                for (int x = 0; x != cells.GetLength(0); x++)
                {
                    byte buf = sorted[x, y];
                    cells[buf / 16, buf % 16] = (byte)(x * 16 + y);
                }
            }

            p = new AStar(cells, PpmData.picSetRepeat, PpmData.picSetRate, PpmData.picMoveRate);
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            buttonStart.Enabled = false;
            initializeManagement();
            sw = System.Diagnostics.Stopwatch.StartNew();
            p.FindBestAnswer += p_FindBestAnswer;
            p.FindBetterAnswer += p_FindBetterAnswer;
            p.Start();
        }

        void p_FindBestAnswer(object sender, EventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate()
            {
                this.textBox1.Text = p.GetAnswerString();
                this.Text = "Done " + sw.Elapsed;
                sw.Stop();
                p.Stop();
            });
        }

        void p_FindBetterAnswer(object sender, EventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate()
            {
                this.textBox1.Text = p.GetAnswerString();
                this.Text = "Not best " + sw.Elapsed;
            });
        }
    }
}
