using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProconSortUI;

namespace ProgramingContestImageSort
{
    public class ImageConstruct
    {
        private int[][][] pieceData = null;
        private Dictionary<int,int[][]> pieceDictionary;
        private byte[] sortedPiece;
        public byte[] Construct(int[][] edgeCompareValue)
        {
            return pieceCreate(getEdges(edgeCompareValue));
        }

        private int[][] getEdges(int[][] edges)
        {
            var edgeCount = 0;
            var allEdgeCount = 0;
            var edgeOverLap = false;
            var edgeList = new List<int[]>();
            while(edgeCount < (PpmData.picDivision[0] - 1) * PpmData.picDivision[1] + PpmData.picDivision[0] * (PpmData.picDivision[1] - 1))
            {
                foreach(int[] edge in edgeList)
                {
                    if (edge[0] == edges[allEdgeCount][0] && edge[1] == edges[allEdgeCount][1] && edge[2] == edges[allEdgeCount][2])
                    {
                        edgeOverLap = true; 
                    }
                    else if (edge[3] == edges[allEdgeCount][0] && edge[4] == edges[allEdgeCount][1] && edge[5] == edges[allEdgeCount][2])
                    {
                        edgeOverLap = true; 
                    }
                    else if (edge[0] == edges[allEdgeCount][3] && edge[1] == edges[allEdgeCount][4] && edge[2] == edges[allEdgeCount][5])
                    {
                        edgeOverLap = true; 
                    }
                    else if (edge[3] == edges[allEdgeCount][3] && edge[4] == edges[allEdgeCount][4] && edge[5] == edges[allEdgeCount][5])
                    {
                        edgeOverLap = true; 
                    }
                }
                if(edgeOverLap)
                {
                    allEdgeCount++;
                    edgeOverLap = false;
                    continue;
                }
                edgeList.Add(edges[allEdgeCount]);
                allEdgeCount++;
                edgeCount++;
            }
            return edgeList.ToArray();
        }

        private byte[] pieceCreate(int[][] edges)
        {
            bool[][] pair = new bool[PpmData.picDivision[0]*PpmData.picDivision[1]][];
            byte[] sortedPiece = new byte[PpmData.picDivision[0] * PpmData.picDivision[1] * 2 + 2];
            sortedPiece[0] = (byte)PpmData.picDivision[0];
            sortedPiece[1] = (byte)PpmData.picDivision[1];
            var firstpiece = -1;
            var nextpiece = -1;
            for (int i = 0; i < PpmData.picDivision[0] * PpmData.picDivision[1];i++ )
            {
                pair[i] = new bool[4];
                pair[i][0] = false;
                pair[i][1] = false;
                pair[i][2] = false;
                pair[i][3] = false;
            }
            foreach (int[] edge in edges)
            {
                pair[edge[0] + edge[1] * PpmData.picDivision[0]][edge[2]] = true;
                pair[edge[3] + edge[4] * PpmData.picDivision[0]][edge[5]] = true;
            }
            for(int i = 0; i < pair.GetLength(0);i++)
            {
                if (!pair[i][0] && !pair[i][1])
                {
                    firstpiece = i;
                    nextpiece = i;
                    sortedPiece[2] = (byte)(nextpiece % PpmData.picDivision[0]);
                    sortedPiece[3] = (byte)(nextpiece / PpmData.picDivision[0]);
                    break;
                }
            }
            for(int y = 0;y < PpmData.picDivision[1];y++)
            {
                for(int x = 0;x < PpmData.picDivision[0];x++)
                {
                    foreach (int[] edge in edges)
                    {
                        if ((x + y * PpmData.picDivision[0]) * 2 + 5 >= sortedPiece.Length)
                        {
                            break;
                        }
                        if (nextpiece % PpmData.picDivision[0] == edge[0] && nextpiece / PpmData.picDivision[0] == edge[1] && edge[2] == 3)
                        {
                            nextpiece = edge[4] * PpmData.picDivision[0] + edge[3];
                            sortedPiece[(x + y * PpmData.picDivision[0]) * 2 + 4] = (byte)(nextpiece % PpmData.picDivision[0]);
                            sortedPiece[(x + y * PpmData.picDivision[0]) * 2 + 5] = (byte)(nextpiece / PpmData.picDivision[0]);
                            break;
                        }
                        else if (nextpiece % PpmData.picDivision[0] == edge[3] && nextpiece / PpmData.picDivision[0] == edge[4] && edge[5] == 3)
                        {
                            nextpiece = edge[1] * PpmData.picDivision[0] + edge[0];
                            sortedPiece[(x + y * PpmData.picDivision[0]) * 2 + 4] = (byte)(nextpiece % PpmData.picDivision[0]);
                            sortedPiece[(x + y * PpmData.picDivision[0]) * 2 + 5] = (byte)(nextpiece / PpmData.picDivision[0]);
                            break;
                        }
                    }
                }
                if (y < PpmData.picDivision[1] - 1)
                {
                    foreach (int[] edge in edges)
                    {
                        if (firstpiece % PpmData.picDivision[0] == edge[0] && firstpiece / PpmData.picDivision[0] == edge[1] && edge[2] == 2)
                        {
                            firstpiece = edge[4] * PpmData.picDivision[0] + edge[3];
                            nextpiece = firstpiece;
                            sortedPiece[(y * PpmData.picDivision[0] + PpmData.picDivision[0]) * 2 + 2] = (byte)(firstpiece % PpmData.picDivision[0]);
                            sortedPiece[(y * PpmData.picDivision[0] + PpmData.picDivision[0]) * 2 + 3] = (byte)(firstpiece / PpmData.picDivision[0]);
                            break;
                        }
                        else if (firstpiece % PpmData.picDivision[0] == edge[3] && firstpiece / PpmData.picDivision[0] == edge[4] && edge[5] == 2)
                        {
                            firstpiece = edge[1] * PpmData.picDivision[0] + edge[0];
                            nextpiece = firstpiece;
                            sortedPiece[(y * PpmData.picDivision[0] + PpmData.picDivision[0]) * 2 + 2] = (byte)(firstpiece % PpmData.picDivision[0]);
                            sortedPiece[(y * PpmData.picDivision[0] + PpmData.picDivision[0]) * 2 + 3] = (byte)(firstpiece / PpmData.picDivision[0]);
                            break;
                        }
                    }
                }
            }
            return sortedPiece;
        }
    }
}
 
