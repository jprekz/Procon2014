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

        private Node ans;

        private Queue<Node> checkQueue = new Queue<Node>();

        public AStar(byte[,] c, int selectm, int selectc, int swapc)
            : base(c, selectm, selectc, swapc)
        {
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
            ans = focus;
            int test1, test2;

            foreach (Edge e in AllEdges)
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

                Node[] nextNodes = (focus.SelectNum == selectMax) ?
                    NextKeepLineNodes(focus) :
                    NextAllNodes(focus);

                test1 = 0;
                test2 = nextNodes.Count();

                foreach (Node m in nextNodes)
                {
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
