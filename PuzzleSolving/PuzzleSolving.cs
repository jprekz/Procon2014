using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSolving
{
    public abstract class PuzzleSolving : IPuzzleSolving
    {
        protected readonly byte[,] startCells;
        protected readonly int selectMax,
            selectCost,
            swapCost,
            cellsX,
            cellsY;
        protected readonly Edge[] AllEdges;

        public PuzzleSolving(byte[,] c, int selectm, int selectc, int swapc)
        {
            startCells = (byte[,])c.Clone();
            selectMax = selectm;
            selectCost = selectc;
            swapCost = swapc;
	        cellsX = startCells.GetLength(0);
            cellsY = startCells.GetLength(1);
            AllEdges = NewAllEdges();
        }

        public abstract void Start();

        public abstract void Stop();

        public abstract Answer GetAnswer();

        protected Node[] NewFirstNodes()
        {
            int p = 0;
            Node[] nodes = new Node[AllEdges.Length];
            Node firstNode = new Node(startCells, 0, 0, Heuristic(startCells), null, new Edge(), Heuristic(startCells));
            foreach (Edge e in AllEdges)
            {
                nodes[p++] = FirstSwap(firstNode, e);
            }
            return nodes;
        }

        protected Node[] NextNewLineNodes(Node n)
        {
            List<Node> nodes = new List<Node>();
            foreach (var e in AllEdges)
            {
                if (n.Selecting == e.Selected) continue;
                if (e.Reverse.Equals(n.Swaped)) continue;
                nodes.Add(Swap(n, e));
            }
            return nodes.ToArray();
        }

        protected Node[] NextKeepLineNodes(Node n)
        {
            int x = n.Selecting / 16,
                y = n.Selecting % 16,
                p = 0;
            Node[] nodes = new Node[(4 - ((x == 0 || x == cellsX - 1) ? 1 : 0) - ((y == 0 || y == cellsY - 1) ? 1 : 0)) - 1];
            if ((y != 0) && (n.Swaped.Swap != Direction.D))
            {
                nodes[p++] = Swap(n, new Edge(x, y, Direction.U));
            }
            if ((y != cellsY - 1) && (n.Swaped.Swap != Direction.U))
            {
                nodes[p++] = Swap(n, new Edge(x, y, Direction.D));
            }
            if ((x != 0) && (n.Swaped.Swap != Direction.R))
            {
                nodes[p++] = Swap(n, new Edge(x, y, Direction.L));
            }
            if ((x != cellsX - 1) && (n.Swaped.Swap != Direction.L))
            {
                nodes[p++] = Swap(n, new Edge(x, y, Direction.R));
            }
            return nodes;
        }

        protected Node[] NextAllNodes(Node n)
        {
            int p = 0;
            Node[] nodes = new Node[AllEdges.Length - 1];
            foreach (var e in AllEdges)
            {
                if (e.Reverse.Equals(n.Swaped)) continue;
                nodes[p++] = Swap(n, e);
            }
            return nodes;
        }


        protected int Heuristic(byte[,] cells)
        {
            int num,
                h = 0;
            for (int y = 0; y != cellsY; y++)
            {
                for (int x = 0; x != cellsX; x++)
                {
                    num = cells[x, y];
                    h += (Math.Abs(num / 16 - x) + Math.Abs(num % 16 - y));
                }
            }
            return h / 2 * swapCost;
        }

        private Node Swap(Node n, Edge e)
        {
            byte[,] nextCells = (byte[,])n.Cells.Clone();
            byte buf = nextCells[e.x, e.y];
            nextCells[e.x, e.y] = nextCells[e.nextx,e.nexty];
            nextCells[e.nextx, e.nexty] = buf;
            byte nextSelectNum = (n.Selecting == e.Selected) ? n.SelectNum : (byte)(n.SelectNum + 1);
            int nextHeuristic = Heuristic(nextCells);
            int nextScore = (n.Selecting == e.Selected) ?
                n.Score - n.Heuristic + nextHeuristic + swapCost :
                n.Score - n.Heuristic + nextHeuristic + swapCost + selectCost;
            return new Node(nextCells, e.NextSelect, nextSelectNum, nextHeuristic, n, e, nextScore);
        }

        private Node FirstSwap(Node n, Edge e)
        {
            byte[,] nextCells = (byte[,])n.Cells.Clone();
            byte buf = nextCells[e.x, e.y];
            nextCells[e.x, e.y] = nextCells[e.nextx,e.nexty];
            nextCells[e.nextx, e.nexty] = buf;
            int nextHeuristic = Heuristic(nextCells);
            int nextScore = nextHeuristic + swapCost + selectCost;
            return new Node(nextCells, e.NextSelect, 1, nextHeuristic, n, e, nextScore);
        }

        private Edge[] NewAllEdges()
        {
            Edge[] allEdge = new Edge[cellsX * cellsY * 4 - (cellsX + cellsY) * 2];
            int counter = 0;
            for (int y = 0; y != cellsY; y++)
            {
                for (int x = 0; x != cellsX; x++)
                {
                    if (y != 0)
                    {
                        allEdge[counter++] = new Edge(x, y, Direction.U);
                    }
                    if (y != cellsY - 1)
                    {
                        allEdge[counter++] = new Edge(x, y, Direction.D);
                    }
                    if (x != 0)
                    {
                        allEdge[counter++] = new Edge(x, y, Direction.L);
                    }
                    if (x != cellsX - 1)
                    {
                        allEdge[counter++] = new Edge(x, y, Direction.R);
                    }
                }
            }
            return allEdge;
        }


        protected string GetAnswerString(Node n)
        {
            // 経路を遡る
            Node back = n;
            List<Edge> route = new List<Edge>();
            while (back.From != null)
            {
                route.Add(back.Swaped);
                back = back.From;
            }
            route.Reverse();
            // わさわさ文字列操作
            string NL = Environment.NewLine;
            string answer = "";
            answer += n.SelectNum + NL;
            for (int i = 0; i != n.SelectNum; i++)
            {
                answer += route[0].Selected.ToString("X2") + NL;

                string buf = "" + route[0].Swap;
                while (route.Count != 1 && route[0].NextSelect == route[1].Selected)
                {
                    buf += route[1].Swap;
                    route.RemoveAt(0);
                }
                route.RemoveAt(0);

                answer += buf.Length + NL + buf + NL;
            }
            return answer;
        }

        protected int GetAnswerDiffs(Node n)
        {
            int diffs = 0;
            for (int y = 0; y < cellsY; y++)
            {
                for (int x = 0; x < cellsX; x++)
                {
                    if (n.Cells[x, y] != (byte)(x * 16 + y)) diffs++;
                }
            }
            return diffs;
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
