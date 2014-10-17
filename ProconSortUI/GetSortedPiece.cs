using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ProconFileIO;

namespace ProconSortUI
{
    public class GetSortedPiece
    {
        public ClientLibrary cl;
        public byte[,] getsortedpiece()
        {
            var mainform = new Main();
            cl = mainform.cl;
            mainform.ShowDialog();
            return picsortToByte(mainform.drawpiece);  
        }
        private byte[,] picsortToByte(byte[] pieceseries)
        {
            byte[,] sortedpiece = new byte[pieceseries[0], pieceseries[1]];
            int pscount = 0;
            for (int y = 0; y < pieceseries[1]; y++)
            {
                for (int x = 0; x < pieceseries[0]; x++)
                {
                    sortedpiece[x, y] = (byte)(pieceseries[pscount * 2 + 2] * 16 + pieceseries[pscount * 2 + 3]);
                    pscount++;
                }
            }
            return sortedpiece;
        }
    }
}
