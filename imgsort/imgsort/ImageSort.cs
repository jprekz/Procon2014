using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramingContestImageSort
{
    public class ImageSort
    {
        public byte[] sort(string ppmPlace)
        {
            var pd = new PpmData();
            var ec = new EdgeCompare();
            var eComparer = new EdgeComparer();
            var ic = new ImageConstruct();

            pd.ppmRead(ppmPlace);
            int[][] edgeCompareValue = ec.compare();
            Array.Sort(edgeCompareValue, edgeCompare());
            return ic.construct(edgeCompareValue);
        }
        public static IComparer<int[]> edgeCompare()
        {
            return (IComparer<int[]>)new EdgeComparer();
        }

    }

    public class EdgeComparer : IComparer<int[]>
    {
        public int Compare(int[] x, int[] y)
        {
            int[] firstarray = x;
            int[] secondarray = y;
            return firstarray[6] - secondarray[6];
        }

        public int Compare(object x, object y)
        {
            if (x == null && y == null)
            {
                return 0;
            }
            if (x == null)
            {
                return -1;
            }
            if (y == null)
            {
                return 1;
            }

            if (!(x is int[]))
            {
                throw new ArgumentException("int[]型でなければなりません。", "x");
            }
            else if (!(y is int[]))
            {
                throw new ArgumentException("int[]型でなければなりません。", "y");
            }

            return this.Compare((int[])x, (int[])y);
        }
    }
}
