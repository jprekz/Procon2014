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

        public override Answer GetAnswer()
        {
            return new Answer
            {
                Str = GetAnswerString(ans),
                Heauristic = ans.Heuristic,
                Cost = ans.Score - ans.Heuristic
            };
        }

        private void SolveThread()
        {
            PriorityQueue<Node> open = new PriorityQueue<Node>(65536);
            List<Node> close = new List<Node>(65536);
            Node focus;
            int passNodes, num;

            Node[] firstNodes = NewFirstNodes();
            foreach (Node n in firstNodes)
            {
                if (n.Heuristic >= Heuristic(startCells)) continue;
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

                passNodes = 0;
                foreach (Node m in nextNodes)
                {
                    // 枝刈り
                    if (m.Heuristic > focus.Heuristic) continue;
                    if ((m.Heuristic == focus.Heuristic) && (m.SelectNum != focus.SelectNum)) continue;

                    passNodes++;
                    if ((num = close.LastIndexOf(m)) != -1)
                    {
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

    }
}
