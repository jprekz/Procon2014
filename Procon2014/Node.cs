using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procon2014
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

        public Node(byte[,] c, byte si, byte sn, int h, Node f, Edge e, int s)
        {
            Cells = (byte[,])c.Clone();
            Selecting = si;
            SelectNum = sn;
            Heuristic = h;
            From = f;
            Swaped = e;
            Score = s;
        }
    }
}
