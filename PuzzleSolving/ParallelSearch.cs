using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PuzzleSolving
{
    class ParallelSearch : IPuzzleSolving
    {
        private Thread[] t = new Thread[1];

        private byte[,] startCells;
        private int selectMax,
            selectCost,
            swapCost,
            CellsX,
            CellsY;

        public ParallelSearch(byte[,] c, int selectm, int selectc, int swapc)
        {
            startCells = (byte[,])c.Clone();
            selectMax = selectm;
            selectCost = selectc;
            swapCost = swapc;
	        CellsX = startCells.GetLength(0);
            CellsY = startCells.GetLength(1);
        }
        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
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
