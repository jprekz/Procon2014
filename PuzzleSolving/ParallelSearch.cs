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

        private List<Node>[] closeArray;
        private PriorityQueue<Node>[] openArray;
        private Queue<Node> checkQueue = new Queue<Node>();

        public ParallelSearch(byte[,] c, int selectm, int selectc, int swapc)
            : base(c, selectm, selectc, swapc)
        {
            closeArray = new List<Node>[selectMax];
            openArray = new PriorityQueue<Node>[selectMax];

            solving = new Thread[selectMax];
            for (int i = 0; i < selectMax; i++)
            {
                solving[i] = new Thread(new ParameterizedThreadStart(SolveThread));
            }
        }

        public override void Start()
        {
            for (int i = 0; i < solving.Length; i++)
            {
                if (!solving[i].IsAlive) solving[i].Start(i);
            }
        }

        public override void Stop()
        {
            foreach (var t in solving)
            {
                if (t.IsAlive) t.Abort();
            }
        }

        private void SolveThread(object num)
        {
            if ((int)num == 0)
            {
                FirstLineSolveThread();
            }
            else
            {
                OtherLineSolveThread((int)num);
            }
        }

        private void FirstLineSolveThread()
        {
            PriorityQueue<Node> open = openArray[0];
            List<Node> close = closeArray[0];
            Node focus;
            int passNodes, num;

            Node[] firstNodes = NewFirstNodes();
            foreach (Node n in firstNodes)
            {
                //if (n.Heuristic >= Heuristic(startCells)) continue;
                open.Push(n);
            }

            while (true)
            {
                focus = open[0];

                close.Add(focus);
                open.RemoveAt(0);

                Node[] nextNodes = NextKeepLineNodes(focus);

                passNodes = 0;
                foreach (Node m in nextNodes)
                {
                    // 枝刈り
                    //if (m.Heuristic > focus.Heuristic) continue;
                    //if ((m.Heuristic == focus.Heuristic) && (m.SelectNum != focus.SelectNum)) continue;

                    passNodes++;
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
                }
                Console.WriteLine("op:" + open.Count + " cl:" + close.Count + " pass:" + passNodes + "/" + nextNodes.Count() + " f:" + focus.Score);
            }
        }

        private void OtherLineSolveThread(int solvingNumber)
        {
            PriorityQueue<Node> open = openArray[solvingNumber];
            List<Node> close = closeArray[solvingNumber];
            Node focus;
            int passNodes, num;

            while (true)
            {



                focus = open[0];

                close.Add(focus);
                open.RemoveAt(0);

                Node[] nextNodes = NextKeepLineNodes(focus);

                passNodes = 0;
                foreach (Node m in nextNodes)
                {
                    // 枝刈り
                    //if (m.Heuristic > focus.Heuristic) continue;
                    //if ((m.Heuristic == focus.Heuristic) && (m.SelectNum != focus.SelectNum)) continue;

                    passNodes++;
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
                }
                Console.WriteLine("op:" + open.Count + " cl:" + close.Count + " pass:" + passNodes + "/" + nextNodes.Count() + " f:" + focus.Score);
            }
        }


        public override string GetAnswerString()
        {
            throw new NotImplementedException();
        }

        public override int GetAnswerCost()
        {
            throw new NotImplementedException();
        }
    }
}
