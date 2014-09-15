using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSolving
{
    class PriorityQueue<T> where T : IComparable<T>
    {
        private List<T> ls;
        private Compare cmp;

        public delegate int Compare(T a, T b);

        public PriorityQueue(int capacity)
        {
            ls = new List<T>(capacity);
            cmp = delegate(T a, T b) { return a.CompareTo(b); };
        }

        public T this[int i]
        {
            get
            {
                return ls[i];
            }
            set
            {
                this.RemoveAt(i);
                this.Push(value);
            }
        }

        public int IndexOf(T x)
        {
            return ls.IndexOf(x);
        }

        public void Push(T x)
        {
            int n = ls.Count;
            ls.Add(x);
            while (n != 0)
            {
                int i = (n - 1) / 2;
                // 親と値を入れ替え
                if (0 > cmp(ls[n], ls[i]))
                {
                    T tmp = ls[n]; ls[n] = ls[i]; ls[i] = tmp;
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
                if ((j != n - 1) && (0 < cmp(ls[j], ls[j + 1])))
                    j++;
                // 子と値を入れ替え
                if (0 < cmp(ls[i], ls[j]))
                {
                    T tmp = ls[j]; ls[j] = ls[i]; ls[i] = tmp;
                }
                i = j;
            }
        }
    }
}
