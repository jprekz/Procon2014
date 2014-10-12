using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PuzzleSolving
{
    public class ParallelSearch : IPuzzleSolving
    {
        private Thread[] solving;
        private Thread checking;

        private Game game;

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

        public void Start()
        {
            for (int i = 0; i < solving.Length; i++)
            {
                if (!solving[i].IsAlive) solving[i].Start(i);
            }
        }

        public void Stop()
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


        public string GetAnswerString()
        {
            throw new NotImplementedException();
        }

        public int GetAnswerCost()
        {
            throw new NotImplementedException();
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
