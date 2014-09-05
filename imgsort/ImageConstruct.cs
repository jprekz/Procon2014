using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramingContest1;

namespace ProgramingContestImageSort
{
    public class ImageConstruct
    {
        private int[][][] pieceData = null;
        private Dictionary<int,int[][]> pieceDictionary;
        private byte[] sortedPiece;
        public byte[] construct(int[][] edgeCompareValue)
        {
            int edgeNumber = ((PpmData.picPieceX - 1) * PpmData.picPieceY) + ((PpmData.picPieceY - 1) * PpmData.picPieceX);
            int edgeCompareValueCount;
            int pieceNumber = PpmData.picPieceX * PpmData.picPieceY;
            int arraySize = (((pieceNumber - 1) * 4) * pieceNumber) / 2;
            var comb = new Combination();

            for(edgeCompareValueCount = edgeNumber;edgeCompareValueCount < arraySize;edgeCompareValueCount++)
            {
                if(pieceJudge(edgeCompareValue,comb.combination(edgeCompareValueCount, edgeNumber)))
                {
                    break;
                }
            }
            if(pieceData == null)
            {
                throw new System.ArgumentException("有効な組み合わせが存在しません");
            }
            pieceMatching();
            return sortedPiece;

        }

        private bool pieceJudge(int[][] edgeCompareValue, int[][] edgeCombination)
        {
            int[][] edge = new int[edgeCombination[0].Length][];
            for (int edgeCombinationCount = 0; edgeCombinationCount < edgeCombination.Length; edgeCombinationCount++)
            {
                for (int i = 0; i < edgeCombination[0].Length; i++)
                {
                    edge[i] = edgeCompareValue[edgeCombination[edgeCombinationCount][i]];
                }
                if (!pieceNumGet(edge) || !edgeOverlapCheck(edge) || !edgeMatch(edge))
                {
                    continue;
                }
                return true;
            }
            return false;
        }

        private bool pieceNumGet(int[][] edge)
        {
            int[] piecesData = new int[edge.Length * 2];
            for(int i = 0;i < edge.Length;i++)
            {
                piecesData[i * 2] = edge[i][0] * 10 + edge[i][1];
                piecesData[i * 2 + 1] = edge[i][3] * 10 + edge[i][4];
            }
            return piecesData.Distinct().Count() == PpmData.picPieceX*PpmData.picPieceY;
        }

        private bool edgeOverlapCheck(int[][] edge)
        {
            int[] edgesData = new int[edge.Length * 2];
            for (int i = 0; i < edge.Length; i++)
            {
                edgesData[i * 2] = edge[i][0] * 100 + edge[i][1] * 10 + edge[i][2];
                edgesData[i * 2 + 1] = edge[i][3] * 100 + edge[i][4] * 10 + edge[i][5];
            }
            return edgesData.Count() == edgesData.Distinct().Count();
        }

        private bool edgeMatch(int[][] edge)
        {
            int[][] edgeType = new int[edge.Length * 2][];
            for(int i = 0;i < edge.Length * 2;i++)
            {
                edgeType[i] = new int[3];
            }
            int[][][] edgeGroup = new int[PpmData.picPieceX * PpmData.picPieceY][][];
            for (int i = 0; i < PpmData.picPieceX * PpmData.picPieceY;i++ )
            {
                edgeGroup[i] = new int[5][];
                edgeGroup[i][0] = new int[1];
                for(int j = 1;j < 5;j++)
                {
                    edgeGroup[i][j] = new int[2];
                }
            }

            for (int i = 0; i < edge.Length; i++)
            {
                edgeType[i * 2][0] = edge[i][0] * 10 + edge[i][1];
                edgeType[i * 2][1] = edge[i][2];
                edgeType[i * 2][2] = edge[i][3] * 10 + edge[i][4];
                edgeType[i * 2 + 1][0] = edge[i][3] * 10 + edge[i][4];
                edgeType[i * 2 + 1][1] = edge[i][5];
                edgeType[i * 2 + 1][2] = edge[i][0] * 10 + edge[i][1];
            }
            for (int i = 0; i < edgeType.Length;i++ )
            {
                edgeType[i][0] = edgeType[i][0] + 1;
            }
            for (int i = 0; i < edgeType.Length; i++)
            {
                for (int j = 0; j < edgeGroup.Length; j++)
                {
                    if (edgeType[i][0] == edgeGroup[j][0][0])
                    {
                        edgeGroup[j][edgeType[i][1] + 1][0] = 1;
                        edgeGroup[j][edgeType[i][1] + 1][1] = edgeType[i][2];

                        break;
                    }
                    if (j + 1 == edgeGroup.Length)
                    {
                        for (int k = 0; k < edgeGroup.Length; k++)
                        {
                            if (edgeGroup[k][0][0] == 0)
                            {
                                edgeGroup[k][0][0] = edgeType[i][0];
                                edgeGroup[k][edgeType[i][1] + 1][0] = 1;
                                edgeGroup[k][edgeType[i][1] + 1][1] = edgeType[i][2];
                                break;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < edgeType.Length; i++)
            {
                edgeType[i][0] = edgeType[i][0] - 1;
            }
            for(int i = 0; i < edgeGroup.Length;i++)
            {
                edgeGroup[i][0][0] = edgeGroup[i][0][0] - 1;
            }
            int twoEdgeCount = 0;
            int threeEdgeCount = 0;
            int fourEdgeCount = 0;
            for(int i = 0;i < edgeGroup.Length;i++)
            {
                switch(edgeGroup[i][1][0] + edgeGroup[i][2][0] + edgeGroup[i][3][0] + edgeGroup[i][4][0])
                {
                    case 2:
                        twoEdgeCount++;
                        break;
                    case 3:
                        threeEdgeCount++;
                        break;
                    case 4:
                        fourEdgeCount++;
                        break;
                    default:
                        break;
                }
            }
            if (twoEdgeCount != 4 || threeEdgeCount != (PpmData.picPieceX - 2) * 2 + (PpmData.picPieceY - 2) * 2 || fourEdgeCount != (PpmData.picPieceX * PpmData.picPieceY - twoEdgeCount - threeEdgeCount))
            {
                return false;
            }
            pieceData = edgeGroup;
            return true;
        }

        private bool pieceMatching()
        {
            int cornerPiece; //左上のみに

            for (int i = 0; i < pieceData.Length; i++)
            {
                if (pieceData[i][1][0] + pieceData[i][2][0] + pieceData[i][3][0] + pieceData[i][4][0] == 2)
                {
                    if (pieceData[i][3][0] + pieceData[i][4][0] == 2)
                    {
                        cornerPiece = pieceData[i][0][0];
                        dictionaryCreate();
                        return pieceArrange(cornerPiece);
                    }
                    else if (!(pieceData[i][1][0] + pieceData[i][2][0] == 2 || pieceData[i][2][0] + pieceData[i][3][0] == 2 || pieceData[i][4][0] + pieceData[i][1][0] == 2))
                    {
                        return false;
                    }
                }
            }
            return false;
        }
        private void dictionaryCreate()
        {
            pieceDictionary = new Dictionary<int, int[][]>();
            for(int i = 0;i < pieceData.Length;i++)
            {
                int[][] piece = new int[4][];
                for (int j = 0; j < 4; j++)
                {
                    piece[j] = new int[2];
                }
                for(int j = 0;j < 4;j++)
                {
                    piece[j] = pieceData[i][j + 1];
                }
                pieceDictionary[pieceData[i][0][0]] = piece;
                piece = null;
            }
        }

        private bool pieceArrange(int cornerPiece)
        {
            int nextPiece = cornerPiece;
            int underPiece = cornerPiece;
            this.sortedPiece = new byte[PpmData.picPieceX*PpmData.picPieceY*2+2];
            this.sortedPiece[0] = (byte)PpmData.picPieceX;
            this.sortedPiece[1] = (byte)PpmData.picPieceY;
            int sortedPieceIndexCount = 2;
            for(int pieceY = 0;pieceY < PpmData.picPieceY;pieceY++)
            {
                for(int pieceX = 0;pieceX < PpmData.picPieceX;pieceX++)
                {
                    if((pieceDictionary[nextPiece][1][0] != 0 && pieceDictionary[(pieceDictionary[nextPiece][1][1])][3][1] != nextPiece)||(pieceDictionary[nextPiece][0][0] != 0&&pieceDictionary[(pieceDictionary[nextPiece][0][1])][2][1] != nextPiece))
                    {
                        return false;
                    }
                    sortedPiece[sortedPieceIndexCount] = (byte)(nextPiece / 10);
                    sortedPiece[sortedPieceIndexCount + 1] = (byte)(nextPiece - (nextPiece / 10 * 10));
                    sortedPieceIndexCount += 2;
                    if (pieceDictionary[nextPiece][3][0] != 0)
                    {
                        nextPiece = pieceDictionary[nextPiece][3][1];
                    }
                }
                nextPiece = pieceDictionary[underPiece][2][1];
                underPiece = nextPiece;
            }
            return true;
        }
    }
}
