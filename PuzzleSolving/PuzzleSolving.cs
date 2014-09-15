using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PuzzleSolving
{
    public abstract class PuzzleSolving : IPuzzleSolving
    {
        private Thread t;

        public PuzzleSolving()
        {
            t = new Thread(new ThreadStart(SolveThread));
            t.IsBackground = true;
        }

        protected abstract void SolveThread();

        public abstract string GetAnswerString();

        public abstract int GetAnswerCost();

        public void Start()
        {
            if (!t.IsAlive) t.Start();
        }

        public void Stop()
        {
            if (t.IsAlive) t.Interrupt();
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
