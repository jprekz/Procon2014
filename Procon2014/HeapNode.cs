using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace soft_tech2014
{
    class HeapNode
    {
        public List<Node> ls;
        public HeapNode(int capacity)
        {
            ls = new List<Node>(capacity);
        }

        public void Push(Node nd)
        {
            int n = ls.Count;
            ls.Add(nd);
            while (n != 0)
            {
                int i = (n - 1) / 2;
                // 親と値を入れ替え
                if (0 > Cmp(ls[n], ls[i]))
                {
                    Node tmp = ls[n]; ls[n] = ls[i]; ls[i] = tmp;
                    n = i;
                }
                else
                {
                    break;
                }
            }
        }

        public void RemoveAt(int at)
        {
            int n = ls.Count - 1;
            ls[at] = ls[n];
            ls.RemoveAt(n);

            for (int i = at, j; (j = 2 * i + 1) < n; )
            {
                if ((j != n - 1) && (0 < Cmp(ls[j], ls[j + 1])))
                    j++;
                // 子と値を入れ替え
                if (0 < Cmp(ls[i], ls[j]))
                {
                    Node tmp = ls[j]; ls[j] = ls[i]; ls[i] = tmp;
                }
                i = j;
            }
        }

        private int Cmp(Node a, Node b)
        {
            //if (a.Score != b.Score) return (a.Score - b.Score);
            //else return (a.Heuristic - b.Heuristic);
            //else return (b.SelectNum - a.SelectNum);
            //else return (a.SelectNum - b.SelectNum);
            //return ((a.Score - b.Score) + (a.Heuristic - b.Heuristic) / 1);
            return (a.Score - b.Score);
        }
    }
}
