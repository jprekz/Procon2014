using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PuzzleSolving
{
    class AStar : IPuzzleSolving
    {
        private Thread t;
        private byte[,] startCells;
        private int selectMax,
            selectCost,
            swapCost,
            CellsX,
            CellsY;

        public AStar(byte[,] c, int selectm, int selectc, int swapc)
        {
            startCells = (byte[,])c.Clone();
            selectMax = selectm;
            selectCost = selectc;
            swapCost = swapc;
	        CellsX = startCells.GetLength(0);
            CellsY = startCells.GetLength(1);
            t = new Thread(new ThreadStart(SolveThread));
            t.IsBackground = true;
        }

        public void Start()
        {
            if (!t.IsAlive) t.Start();
        }

        public void Stop()
        {
            if (t.IsAlive) t.Interrupt();
        }

        private void SolveThread()
        {
            PriorityQueue<Node> open = new PriorityQueue<Node>(65536);
            List<Node> close = new List<Node>(65536);
            Node focus = new Node(startCells, 0, 0, Heuristic(startCells), null, new Edge(), Heuristic(startCells));
            foreach (Edge e in allEdges)
            {
                Node n = FirstSwap(focus, e);
                if (n.Heuristic >= focus.Heuristic) continue;
                open.Push(n);
            }
            while (true)
            {
                focus = open[0];
                if (focus.Heuristic == 0)
                {
                }
                //close.Add(focus);
                close.Insert(0, focus); //more fast
                open.RemoveAt(0);

                Edge[] edges = (focus.SelectNum == selectMax) ? NewLastLineEdges(focus.Selecting) : allEdges;
                foreach (Edge e in edges)
                {
                    Node m = Swap(focus, e);

                    // 枝刈り
                    if (m.Heuristic > focus.Heuristic) continue;
                    if ((m.Heuristic == focus.Heuristic) && (m.SelectNum != focus.SelectNum)) continue;
                    if ((NodeMatching(close, m) == -1) && (NodeMatching(open, m) == -1)) open.Add(m);
                }
            }
        }


        private int Heuristic(byte[,] cells)
        {
            int num,
                h = 0;
            for (int y = 0; y != CellsY; y++)
            {
                for (int x = 0; x != CellsX; x++)
                {
                    num = cells[x, y];
                    h += (Math.Abs(num / 16 - x) + Math.Abs(num % 16 - y));
                }
            }
            return h / 2 * swapCost;
        }



        public string GetAnswerString()
        {
            return "test";
        }

        public int GetAnswerCost()
        {
            return -1;
        }

        public event EventHandler FindBestAnswer;
        protected virtual void OnFindBestAnswer(EventArgs e)
        {
            if (FindBestAnswer != null) FindBestAnswer(this, e);
        }

        public event EventHandler FindBetterAnswer;
        protected virtual void OnFindBetterAnswer(EventArgs e)
        {
            if (FindBetterAnswer != null) FindBetterAnswer(this, e);
        }

        public event EventHandler SolvingError;
        protected virtual void OnSolvingError(EventArgs e)
        {
            if (SolvingError != null) SolvingError(this, e);
        }
    }
}
