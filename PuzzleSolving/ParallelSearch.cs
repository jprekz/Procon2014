using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PuzzleSolving
{
    public class ParallelSearch : PuzzleSolving
    {
        private Thread[] solving;
        private Thread checking;

        private Node ans;
        private PriorityQueue<Node>[] closeArray;
        private PriorityQueue<Node>[] openArray;
        private Queue<Node> checkQueue = new Queue<Node>();

        public ParallelSearch(byte[,] c, int selectm, int selectc, int swapc)
            : base(c, selectm, selectc, swapc)
        {
            closeArray = new PriorityQueue<Node>[selectMax];
            for (int i = 0; i < selectMax; i++)
            {
                closeArray[i] = new PriorityQueue<Node>(65536,
                    delegate(Node a, Node b) { return a.Heuristic - b.Heuristic; });
            }
            openArray = new PriorityQueue<Node>[selectMax];
            for (int i = 0; i < selectMax; i++)
            {
                openArray[i] = new PriorityQueue<Node>(65536,
                    delegate(Node a, Node b) { return a.Heuristic - b.Heuristic; });
            }

            solving = new Thread[selectMax];
            for (int i = 0; i < selectMax; i++)
            {
                solving[i] = new Thread(new ParameterizedThreadStart(SolveThread));
            }
            checking = new Thread(new ThreadStart(CheckThread));
        }

        public override void Start()
        {
            for (int i = 0; i < solving.Length; i++)
            {
                if (!solving[i].IsAlive) solving[i].Start(i);
            }
            if (!checking.IsAlive) checking.Start();
        }

        public override void Stop()
        {
            foreach (var t in solving)
            {
                if (t.IsAlive) t.Abort();
            }
            if (checking.IsAlive) checking.Abort();
        }

        private void SolveThread(object num)
        {
            int solvingNumber = (int)num;
            PriorityQueue<Node> open = openArray[solvingNumber];
            PriorityQueue<Node> close = closeArray[solvingNumber];
            Node focus;
            int passNodes, nodeNum;

            if (solvingNumber == 0)
            {
                Node[] firstNodes = NewFirstNodes();
                foreach (Node n in firstNodes)
                {
                    //if (n.Heuristic >= Heuristic(startCells)) continue;
                    open.Push(n);
                }
            }
            else
            {
                Thread.Sleep(100);
            }

            while (true)
            {
                if (solvingNumber != 0)
                {
                    if (closeArray[solvingNumber - 1].Count == 0)
                    {
                        Thread.Sleep(100);
                        continue;
                    }
                    Node[] n = NextNewLineNodes(closeArray[solvingNumber - 1][0]);
                    foreach (var m in n)
                    {
                        if (open.IndexOf(m) != -1) continue;
                        open.Push(m);
                    }
                }

                focus = open[0];

                close.Push(focus);
                open.RemoveAt(0);

                Node[] nextNodes = NextKeepLineNodes(focus);

                passNodes = 0;
                foreach (Node m in nextNodes)
                {
                    // 枝刈り
                    //if (m.Heuristic > focus.Heuristic) continue;
                    //if ((m.Heuristic == focus.Heuristic) && (m.SelectNum != focus.SelectNum)) continue;

                    passNodes++;
                    if ((nodeNum = close.ls.LastIndexOf(m)) != -1)
                    {
                        // 要らなくね
                        if (m.Score < close[nodeNum].Score)
                        {
                            open.Push(m);
                            close.RemoveAt(nodeNum);
                        }
                    }
                    else if ((nodeNum = open.IndexOf(m)) != -1)
                    {
                        if (m.Score < open[nodeNum].Score)
                        {
                            open.RemoveAt(nodeNum);
                            open.Push(m);
                        }
                    }
                    else
                    {
                        open.Push(m);
                        lock (((ICollection)checkQueue).SyncRoot) checkQueue.Enqueue(m);
                    }
                }
                Console.WriteLine("op:" + open.Count + " cl:" + close.Count + " pass:" + passNodes + "/" + nextNodes.Count() + " f:" + focus.Score);
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
                    if ((ans == null) ||
                        (ans.Heuristic > n.Heuristic) ||
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
