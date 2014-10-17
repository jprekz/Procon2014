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
using ProconFileIO;

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
            ProconSortUI.Main.cl.ReturnRespons += ReturnSubmitAnswer;
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
            this.submit1.Enabled = true;
            this.submit2.Enabled = true;
            this.stop1.Enabled = true;
            this.stop2.Enabled = true;
        }

        private void submit1_Click(object sender, EventArgs e)
        {
            this.submit1.Enabled = false;
            this.submit2.Enabled = false;
            ProconSortUI.Main.cl.SubmitAnswer(p1.GetAnswer().Str);
        }

        private void submit2_Click(object sender, EventArgs e)
        {
            this.submit1.Enabled = false;
            this.submit2.Enabled = false;
            ProconSortUI.Main.cl.SubmitAnswer(p2.GetAnswer().Str);
        }
        private void stop1_Click(object sender, EventArgs e)
        {
            p1.Stop();
        }

        private void stop2_Click(object sender, EventArgs e)
        {
            p2.Stop();
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
        
        void ReturnSubmitAnswer(object sender, EventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate()
            {
                this.serverReturn.Text = ProconSortUI.Main.cl.Respons;
                this.submit1.Enabled = true;
                this.submit2.Enabled = true;
            });
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F1) this.submit1_Click(this, new EventArgs());
            else if (e.KeyData == Keys.F2) this.stop1_Click(this, new EventArgs());
            else if (e.KeyData == Keys.F3) this.submit2_Click(this, new EventArgs());
            else if (e.KeyData == Keys.F4) this.stop2_Click(this, new EventArgs());
        }
    }
}
