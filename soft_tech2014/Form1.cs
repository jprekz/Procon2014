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

namespace soft_tech2014
{
    public partial class Form1 : Form
    {
        private Puzzle p;
        private System.Diagnostics.Stopwatch sw;
        public Form1()
        {
            InitializeComponent();
        }

        private void initialManagement() //Form1.Loadでしていた処理をppmファイルのパスをtextBox1.Textから指定するためにbutton1_Clickの中に移動した
        {
            ppmedit ppme = new ppmedit();
            byte[,] sorted = ppme.picsortToByte(this.textBox1.Text);
            byte[,] cells = (byte[,])sorted.Clone();
            for (int y = 0; y != cells.GetLength(1); y++)
            {
                for (int x = 0; x != cells.GetLength(0); x++)
                {
                    byte buf = sorted[x, y];
                    cells[buf / 16, buf % 16] = (byte)(x * 16 + y);
                }
            }

            p = new Puzzle(cells, ppme.ppmd.picsetrepeat, ppme.ppmd.picsetrate, ppme.ppmd.picmoverate);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            initialManagement();
            sw = System.Diagnostics.Stopwatch.StartNew();
            Thread t = new Thread(new ThreadStart(SolveThread));
            t.IsBackground = true;
            t.Start();
        }

        private void SolveThread()
        {
            for (int i = 0; !p.Done; i++)
            {
                p.SolvePuzzle(1000);
                string s = p.getString();
                this.BeginInvoke(
                    new UpdateFormDelegate(UpdateForm),
                    new object[] { i, s });
            }
            sw.Stop();
        }

        private delegate void UpdateFormDelegate(int val,string s);
        private void UpdateForm(int val, string s)
        {
            textBox1.Text = s;
            this.Text = "" + val;
            if (p.Done)
            {
                this.Text = "Done";
                this.button1.Enabled = true;
            }
            this.Text += " " + sw.Elapsed;
        }
    }
}
