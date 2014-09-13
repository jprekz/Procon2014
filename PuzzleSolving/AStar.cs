using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSolving
{
    class AStar : IPuzzleSolving
    {
        private byte[,] startCells;
        private int selectMax,
            selectCost,
            swapCost,
            CellsX,
            CellsY;
        private Node start;

        public AStar(byte[,] c, int selectm, int selectc, int swapc)
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

        }


        public void Stop()
        {

        }

        public string GetAnswerString()
        {
            return "test";
        }

        public int GetAnswerCost()
        {
            return -1;
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
