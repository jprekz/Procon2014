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
        private IPuzzleSolving p1;
        private IPuzzleSolving p2;
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

            p1 = new AStar(cells, PpmData.picSetRepeat, PpmData.picSetRate, PpmData.picMoveRate);
            p2 = new ParallelSearch(cells, PpmData.picSetRepeat, PpmData.picSetRate, PpmData.picMoveRate);
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            buttonStart.Enabled = false;
            initializeManagement();
            sw = System.Diagnostics.Stopwatch.StartNew();
            p1.FindBestAnswer += p1_FindBestAnswer;
            p2.FindBestAnswer += p2_FindBestAnswer;
            p1.FindBetterAnswer += p1_FindBetterAnswer;
            p2.FindBetterAnswer += p2_FindBetterAnswer;
            p1.Start();
            p2.Start();
        }

        void p1_FindBestAnswer(object sender, EventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate()
            {
                this.textBox1.Text = "Done " + sw.Elapsed;
                this.textBox1.Text += Environment.NewLine;
                this.textBox1.Text += p1.GetAnswer();
                p1.Stop();
            });
        }

        void p2_FindBestAnswer(object sender, EventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate()
            {
                this.textBox2.Text = "Done " + sw.Elapsed;
                this.textBox2.Text += Environment.NewLine;
                this.textBox2.Text += p2.GetAnswer();
                p2.Stop();
            });
        }

        void p1_FindBetterAnswer(object sender, EventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate()
            {
                this.textBox1.Text = "Not best " + sw.Elapsed;
                this.textBox1.Text += Environment.NewLine;
                this.textBox1.Text += p1.GetAnswer();
            });
        }

        void p2_FindBetterAnswer(object sender, EventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate()
            {
                this.textBox2.Text = "Not best " + sw.Elapsed;
                this.textBox2.Text += Environment.NewLine;
                this.textBox2.Text += p2.GetAnswer();
            });
        }
    }
}
