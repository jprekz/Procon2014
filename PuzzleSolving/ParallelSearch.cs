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
        delegate int d(Node a, Node b);

        private Thread[] solving;
        private Thread checking;

        private Node ans;
        private PriorityQueue<Node>[] closeArray;
        private PriorityQueue<Node>[] openArray;
        private Queue<Node> checkQueue = new Queue<Node>();

        public ParallelSearch(byte[,] c, int selectm, int selectc, int swapc)
            : base(c, selectm, selectc, swapc)
        {

            PriorityQueue<Node>.Compare cmp = (Node a, Node b) =>
            {
                int diff = a.Heuristic - b.Heuristic;
                return diff;
                //return (diff != 0) ? diff : a.Score - b.Score;
            };
            closeArray = new PriorityQueue<Node>[selectMax];
            for (int i = 0; i < selectMax; i++)
            {
                closeArray[i] = new PriorityQueue<Node>(65536, cmp);
            }
            openArray = new PriorityQueue<Node>[selectMax];
            for (int i = 0; i < selectMax; i++)
            {
                openArray[i] = new PriorityQueue<Node>(65536, cmp);
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

        public override Answer GetAnswer()
        {
            return new Answer
            {
                Str = GetAnswerString(ans),
                Heauristic = ans.Heuristic,
                Cost = ans.Score - ans.Heuristic,
                Diffs = GetAnswerDiffs(ans)
            };
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
                    open.Push(n);
                }
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
                    passNodes++;
                    if ((nodeNum = close.ls.LastIndexOf(m)) != -1)
                    {
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
                        (GetAnswerDiffs(ans) > GetAnswerDiffs(n)) ||
                        ((GetAnswerDiffs(ans) == GetAnswerDiffs(n)) && (ans.Score > n.Score)))
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
