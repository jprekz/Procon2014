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

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Node a = obj as Node;
            if ((object)a == null) return false;
            if (a.Selecting != Selecting) return false;
            if (a.SelectNum != SelectNum) return false;
            for (int y = 0; y != Cells.GetLength(1); y++)
            {
                for (int x = 0; x != Cells.GetLength(0); x++)
                {
                    if (a.Cells[x, y] != Cells[x, y]) return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public int CompareTo(Node obj)
        {
            return (this.Score - (obj).Score);
        }
    }
}
