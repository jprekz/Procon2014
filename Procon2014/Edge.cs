using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procon2014
{
    enum Direction
    {
        U,D,R,L
    }

    struct Edge
    {
        public byte Selected;
        public Direction Swap;

        public Edge(int x,int y,Direction d)
        {
            Selected = (byte)(x * 16 + y);
            Swap = d;
        }
        public byte NextSelect
        {
            get
            {
                switch (Swap)
                {
                    case Direction.U:
                        return (byte)(Selected - 1);
                    case Direction.D:
                        return (byte)(Selected + 1);
                    case Direction.R:
                        return (byte)(Selected + 16);
                    case Direction.L:
                        return (byte)(Selected - 16);
                    default:
                        return Selected;    // arienkakkowara
                }
            }
        }
        public int x
        {
            get
            {
                return Selected / 16;
            }
        }

        public int y
        {
            get
            {
                return Selected % 16;
            }
        }

        public int nextx
        {
            get
            {
                switch (Swap)
                {
                    case Direction.R:
                        return x + 1;
                    case Direction.L:
                        return x - 1;
                    default:
                        return x;
                }
            }
        }

        public int nexty
        {
            get
            {
                switch (Swap)
                {
                    case Direction.U:
                        return y - 1;
                    case Direction.D:
                        return y + 1;
                    default:
                        return y;
                }
            }
        }

        public override string ToString()
        {
            return Selected.ToString("X2") + "," + Swap;
        } 
    }
}
