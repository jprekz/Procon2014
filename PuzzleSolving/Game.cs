using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSolving
{
    public class Game
    {
        public readonly byte[,] StartCells;
        public readonly int SelectMax,
            SelectCost,
            SwapCost,
            CellsX,
            CellsY;

        public Game(byte[,] c, int selectm, int selectc, int swapc)
        {
            StartCells = (byte[,])c.Clone();
            SelectMax = selectm;
            SelectCost = selectc;
            SwapCost = swapc;
	        CellsX = StartCells.GetLength(0);
            CellsY = StartCells.GetLength(1);
        }
    }
}
