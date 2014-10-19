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
        public byte[] Construct(int[][] edgeCompareValue,int leftvalue)
        {
            return pieceCreate(getEdges(edgeCompareValue),leftvalue);
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

        private byte[] pieceCreate(int[][] edges,int leftvalue)
        {
            bool[][] pair = new bool[PpmData.picDivision[0]*PpmData.picDivision[1]][];
            byte[] sortedPiece = new byte[PpmData.picDivision[0] * PpmData.picDivision[1] * 2 + 2];
            sortedPiece[0] = (byte)PpmData.picDivision[0];
            sortedPiece[1] = (byte)PpmData.picDivision[1];
            var firstpiece = -1;
            var nextpiece = -1;
            if (leftvalue != -1)
            {
                firstpiece = leftvalue;
                nextpiece = leftvalue;
                return pieceCheck(edges, leftvalue,sortedPiece);
            }
            var max = PpmData.picDivision[0] * PpmData.picDivision[1];
            var left = 0;
            for (int i = 0; i < PpmData.picDivision[0] * PpmData.picDivision[1];i++)
            {
                var pieces = pieceCheck(edges, i,sortedPiece);
                var count = 0;
                for(int j = 2;j < pieces.Length;j+=2)
                {
                    if (pieces[j] == 16 && pieces[j + 1] == 16)
                        count++;
                }
                if(count<max)
                {
                    left = i;
                    max = count;
                }
            }
            sortedPiece = pieceCheck(edges, left, sortedPiece);
            return sortedPiece;
        }

        private byte[] pieceCheck(int[][] edges,int nextpiece,byte[] sortedPiece)
        {
            var firstpiece = nextpiece;
            sortedPiece[2] = (byte)(nextpiece % PpmData.picDivision[0]);
            sortedPiece[3] = (byte)(nextpiece / PpmData.picDivision[0]);
            for (int y = 0; y < PpmData.picDivision[1]; y++)
            {
                for (int x = 0; x < PpmData.picDivision[0]; x++)
                {
                    var isassign = false;
                    var isfinish = false;
                    foreach (int[] edge in edges)
                    {
                        var isfound = false;
                        if ((x + y * PpmData.picDivision[0]) * 2 + 5 >= sortedPiece.Length)
                        {
                            isfinish = true;
                            break;
                        }

                        if (nextpiece % PpmData.picDivision[0] == edge[0] && nextpiece / PpmData.picDivision[0] == edge[1] && edge[2] == 3)
                        {
                            nextpiece = edge[4] * PpmData.picDivision[0] + edge[3];
                            isfound = true;
                        }
                        else if (nextpiece % PpmData.picDivision[0] == edge[3] && nextpiece / PpmData.picDivision[0] == edge[4] && edge[5] == 3)
                        {
                            nextpiece = edge[1] * PpmData.picDivision[0] + edge[0];
                            isfound = true;
                        }
                        else if (y > 0 && sortedPiece[(x + (y - 1) * PpmData.picDivision[0]) * 2 + 4] == edge[0] && sortedPiece[(x + (y - 1) * PpmData.picDivision[0]) * 2 + 5] == edge[1] && edge[2] == 2)
                        {
                            nextpiece = edge[4] * PpmData.picDivision[0] + edge[3];
                            isfound = true;
                        }
                        else if (y > 0 && sortedPiece[(x + (y - 1) * PpmData.picDivision[0]) * 2 + 4] == edge[3] && sortedPiece[(x + (y - 1) * PpmData.picDivision[0]) * 2 + 5] == edge[4] && edge[5] == 2)
                        {
                            nextpiece = edge[1] * PpmData.picDivision[0] + edge[0];
                            isfound = true;
                        }
                        if (isfound)
                        {
                            for (int i = 2; i < (x + y * PpmData.picDivision[0]) * 2 + 3; i += 2)
                            {
                                if (sortedPiece[i] == nextpiece % PpmData.picDivision[0] && sortedPiece[i + 1] == nextpiece / PpmData.picDivision[0])
                                {
                                    isfound = false;
                                }
                            }
                        }
                        if (isfound)
                        {
                            sortedPiece[(x + y * PpmData.picDivision[0]) * 2 + 4] = (byte)(nextpiece % PpmData.picDivision[0]);
                            sortedPiece[(x + y * PpmData.picDivision[0]) * 2 + 5] = (byte)(nextpiece / PpmData.picDivision[0]);
                            isassign = true;
                            break;
                        }
                    }

                    if(isfinish)
                    {
                        break;
                    }
                    if(!isassign)
                    {
                        sortedPiece[(x + y * PpmData.picDivision[0]) * 2 + 4] = 16;
                        sortedPiece[(x + y * PpmData.picDivision[0]) * 2 + 5] = 16;
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

 
