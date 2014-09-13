using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSolving
{
    class Node
    {
        public byte[,] Cells;
        public byte Selecting;
        public byte SelectNum;
        public int Heuristic;
        public Node From;
        public Edge Swaped;
        public int Score;

        public Node(byte[,] c, byte si, byte sn, Node f, Edge e, int s)
        {
            Cells = (byte[,])c.Clone();
            Selecting = si;
            SelectNum = sn;
            From = f;
            Swaped = e;
            Score = s;
        }
    }
}
