using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSolving
{
    class Node : IComparable<Node>
    {
        public readonly byte[,] Cells;
        public readonly byte Selecting;
        public readonly byte SelectNum;
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

        public static bool operator ==(Node a, Node b)
        {
            if (object.ReferenceEquals(a, b)) return true;
            if (((object)a == null) || ((object)b == null)) return false;
            if (a.Selecting != b.Selecting) return false;
            if (a.SelectNum != b.SelectNum) return false;
            for (int y = 0; y != a.Cells.GetLength(1); y++)
            {
                for (int x = 0; x != a.Cells.GetLength(0); x++)
                {
                    if (a.Cells[x, y] != b.Cells[x, y]) return false;
                }
            }
            return true;
        }

        public int CompareTo(Node obj)
        {
            return (this.Score - (obj).Score);
        }
    }
}
