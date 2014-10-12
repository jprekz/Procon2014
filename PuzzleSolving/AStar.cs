using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PuzzleSolving
{
    public class AStar : PuzzleSolving
    {
        private Thread[] t = new Thread[2];

        private byte[,] startCells;
        private int selectMax,
            selectCost,
            swapCost,
            CellsX,
            CellsY;
        private Node ans;

        private Queue<Node> checkQueue = new Queue<Node>();

        public AStar(byte[,] c, int selectm, int selectc, int swapc)
        {
            startCells = (byte[,])c.Clone();
            selectMax = selectm;
            selectCost = selectc;
            swapCost = swapc;
	        CellsX = startCells.GetLength(0);
            CellsY = startCells.GetLength(1);

            t[0] = new Thread(new ThreadStart(SolveThread));
            t[1] = new Thread(new ThreadStart(CheckThread));
            foreach (Thread thread in t)
            {
                thread.IsBackground = true;
            }
        }

        public override void Start()
        {
            foreach (Thread thread in t)
            {
                if (!thread.IsAlive) thread.Start();
            }
        }

        public override void Stop()
        {
            foreach (Thread thread in t)
            {
                if (thread.IsAlive) thread.Abort();
            }
        }

        private void SolveThread()
        {
            PriorityQueue<Node> open = new PriorityQueue<Node>(65536);
            List<Node> close = new List<Node>(65536);
            Node focus = new Node(startCells, 0, 0, Heuristic(startCells), null, new Edge(), Heuristic(startCells));
            Edge[] allEdges = NewAllEdges();
            ans = focus;
            int test1, test2;

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
                    ans = focus;
                    OnFindBestAnswer(new EventArgs());
                    return;
                }
                close.Add(focus);
                open.RemoveAt(0);

                Edge[] edges = (focus.SelectNum == selectMax) ? NewLastLineEdges(focus.Selecting) : allEdges;

                test1 = 0;
                test2 = edges.Count();

                foreach (Edge e in edges)
                {
                    Node m = Swap(focus, e);

                    // 枝刈り
                    if (m.Heuristic > focus.Heuristic) continue;
                    if ((m.Heuristic == focus.Heuristic) && (m.SelectNum != focus.SelectNum)) continue;

                    int num;
                    if ((num = close.LastIndexOf(m)) != -1)
                    {
                        // 要らなくね
                        if (m.Score < close[num].Score)
                        {
                            open.Push(m);
                            close.RemoveAt(num);
                        }
                    }
                    else if ((num = open.IndexOf(m)) != -1)
                    {
                        if (m.Score < open[num].Score)
                        {
                            open.RemoveAt(num);
                            open.Push(m);
                        }
                    }
                    else
                    {
                        open.Push(m);
                        lock (((ICollection)checkQueue).SyncRoot) checkQueue.Enqueue(m);
                    }
                    test1++;
                }
                Console.WriteLine("op:" + open.Count + " cl:" + close.Count + " pass:" + test1 + "/" + test2 + " f:" + focus.Score);
            }
        }

        private void CheckThread()
        {
            Node n;
            while(true)
            {
                while (checkQueue.Count != 0)
                {
                    lock (((ICollection)checkQueue).SyncRoot) n = checkQueue.Dequeue();
                    if ((ans.Heuristic > n.Heuristic) ||
                        ((ans.Heuristic == n.Heuristic) && (ans.Score > n.Score)))
                    {
                        ans = n;
                        OnFindBetterAnswer(new EventArgs());
                    }
                }
                Thread.Sleep(50);
                Console.WriteLine("----------------------------------------" + checkQueue.Count);
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
            Edge[] allEdge = new Edge[CellsX * CellsY * 4 - (CellsX + CellsY) * 2];
            int counter = 0;
            for (int y = 0; y != CellsY; y++)
            {
                for (int x = 0; x != CellsX; x++)
                {
                    if (y != 0)
                    {
                        allEdge[counter++] = new Edge(x, y, Direction.U);
                    }
                    if (y != CellsY - 1)
                    {
                        allEdge[counter++] = new Edge(x, y, Direction.D);
                    }
                    if (x != 0)
                    {
                        allEdge[counter++] = new Edge(x, y, Direction.L);
                    }
                    if (x != CellsX - 1)
                    {
                        allEdge[counter++] = new Edge(x, y, Direction.R);
                    }
                }
            }
            return allEdge;
        }

        private Edge[] NewLastLineEdges(byte selecting)
        {
            int x = selecting / 16,
                y = selecting % 16;
            Edge[] edges = new Edge[(4 - ((x == 0 || x == CellsX - 1) ? 1 : 0) - ((y == 0 || y == CellsY - 1) ? 1 : 0))];

            int counter = 0;
            if (y != 0)
            {
                edges[counter++] = new Edge(x, y, Direction.U);
            }
            if (y != CellsY - 1)
            {
                edges[counter++] = new Edge(x, y, Direction.D);
            }
            if (x != 0)
            {
                edges[counter++] = new Edge(x, y, Direction.L);
            }
            if (x != CellsX - 1)
            {
                edges[counter++] = new Edge(x, y, Direction.R);
            }
            return edges;
        }

        public override string GetAnswerString()
        {
            Node n = ans;
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
            string answer = "---" + n.Heuristic/swapCost + " " + n.Score + NL;
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

        public override int GetAnswerCost()
        {
            return ans.Score;
        }
    }
}
