using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procon2014
{
    class Puzzle
    {
        private byte[,] startCells;
        private int selectMax,
            selectCost,
            swapCost,
            CellsX,
            CellsY;

        private HeapNode open;
        private List<Node> close;
        private Edge[] allEdges;
        private Node start;

        private bool firstSolve = true;
        private Node focus;

        public bool Done = false;

        public Puzzle(byte[,] c,int selectm,int selectc,int swapc)
        {
            startCells = (byte[,])c.Clone();
            selectMax = selectm;
            selectCost = selectc;
            swapCost = swapc;
	        CellsX = startCells.GetLength(0);
            CellsY = startCells.GetLength(1);
            open = new HeapNode(65536);
            close = new List<Node>(65536);
            allEdges = NewAllEdges();
            start = new Node(startCells, 0, 0, Heuristic(startCells), null, new Edge(), Heuristic(startCells));
        }

        public bool SolvePuzzle(int loopLimit)
        {
            if (firstSolve)
            {
                focus = start;
                foreach (Edge e in allEdges)
                {
                    Node n = FirstSwap(start, e);
                    if (n.Heuristic >= start.Heuristic) continue;
                    open.Push(n);
                }
                firstSolve = false;
            }
            int test1,test2;
            int loopCounter = 0;
            int num;
            while (loopCounter++ < loopLimit)
            {
                focus = open.ls[0];
                if (focus.Heuristic == 0)
                {
                    return Done = true;
                }
                //close.Add(focus);
                close.Insert(0, focus); //more fast
                open.RemoveAt(0);
                test1 = 0;test2=0;

                Edge[] edges = (focus.SelectNum == selectMax) ? NewLastLineEdges(focus.Selecting) : allEdges;
                test2 = edges.Count();
                foreach (Edge e in edges)
                {
                    Node m = Swap(focus, e);

                    // 枝刈り
                    if (m.Heuristic > focus.Heuristic) continue;
                    if ((m.Heuristic == focus.Heuristic) && (m.SelectNum != focus.SelectNum)) continue;
                    if ((NodeMatching(close, m) == -1) && (NodeMatching(open.ls, m) == -1)) open.Push(m);
                    //if ((num = NodeMatching(close, m)) != -1)
                    //{
                    //    // 要らなくね
                    //    if (m.Score < close[num].Score)
                    //    {
                    //        open.Push(m);
                    //        close.RemoveAt(num);
                    //    }
                    //}
                    //else if ((num = NodeMatching(open.ls, m)) != -1)
                    //{
                    //    // こっちも全然引っかからない
                    //    if (m.Score < open.ls[num].Score)
                    //    {
                    //        open.RemoveAt(num);
                    //        open.Push(m);
                    //    }
                    //}
                    //else
                    //{
                    //    open.Push(m);
                    //}
                    test1++;
                }
                Console.WriteLine("op:" + open.ls.Count + " cl:" + close.Count + " pass:" + test1 + "/" + test2 + " f:" + focus.Score);
            }
            return Done;
        }

        public string getString()
        {
            if (Done)
            {
                return RouteToString(focus);
            }
            else
            {
                Node target = open.ls[0];
                foreach (Node n in open.ls)
                {
                    if (target.Heuristic > n.Heuristic)
                    {
                        target = n;
                    }
                    if ((target.Heuristic == n.Heuristic) && (target.Score > n.Score))
                    {
                        target = n;
                    }
                }
                foreach (Node n in close)
                {
                    if (target.Heuristic > n.Heuristic)
                    {
                        target = n;
                    }
                    if ((target.Heuristic == n.Heuristic) && (target.Score > n.Score))
                    {
                        target = n;
                    }
                }
                return RouteToString(target);
            }
        }

        private string RouteToString(Node n)
        {
            // 経路を遡る
            List<Edge> route = new List<Edge>();
            Node back = n;
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

        private int NodeMatching(List<Node> ls, Node n)
        {
            return ls.FindIndex(
                delegate(Node m)
                {
                    return ConditionEqual(n, m);
                }
                );
        }

        private bool ConditionEqual(Node a, Node b)
        {
            if (a.Selecting != b.Selecting) return false;
            if (a.SelectNum != b.SelectNum) return false;
            for (int y = 0; y != CellsY; y++)
            {
                for (int x = 0; x != CellsX; x++)
                {
                    if (a.Cells[x, y] != b.Cells[x, y]) return false;
                }
            }
            return true;
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
    }
}
