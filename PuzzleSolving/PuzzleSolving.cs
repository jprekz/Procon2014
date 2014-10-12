using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSolving
{
    public abstract class PuzzleSolving : IPuzzleSolving
    {
        protected readonly byte[,] startCells;
        protected readonly int selectMax,
            selectCost,
            swapCost,
            cellsX,
            cellsY;

        public PuzzleSolving(byte[,] c, int selectm, int selectc, int swapc)
        {
            startCells = (byte[,])c.Clone();
            selectMax = selectm;
            selectCost = selectc;
            swapCost = swapc;
	        cellsX = startCells.GetLength(0);
            cellsY = startCells.GetLength(1);
        }

        public abstract void Start();

        public abstract void Stop();

        public abstract string GetAnswerString();

        public abstract int GetAnswerCost();

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
