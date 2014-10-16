using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSolving
{
    public struct Answer
    {
        public string Str;
        public int Heauristic;
        public int Cost;
        public override string ToString()
        {
            string NL = Environment.NewLine;
            return "heauristic=" + Heauristic + NL + "cost=" + Cost + NL + NL + Str;
        }
    }
}
