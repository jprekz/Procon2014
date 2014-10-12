using System;
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

        private List<Node>[] close;
        private PriorityQueue<Node>[] open;
        private Queue<Node> checkQueue = new Queue<Node>();

        public ParallelSearch(byte[,] c, int selectm, int selectc, int swapc)
        {
            game = new Game(c, selectm, selectc, swapc);

            close = new List<Node>[game.SelectMax];
            open = new PriorityQueue<Node>[game.SelectMax];

            solving = new Thread[game.SelectMax];
            for (int i = 0; i < game.SelectMax; i++)
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

        }

        private void OtherLineSolveThread(int solvingNumber)
        {

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
