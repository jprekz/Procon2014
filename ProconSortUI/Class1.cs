using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace ProconSortUI
{
    public class PpmEdit
    {
        PpmData ppmd = new PpmData();
        public byte[] picSort(string ppmfile)
        {
            ppmd.ppmRead(ppmfile);
            int[] pieceSeries = this.ppmSort(ppmd);
            byte[] sortedPiece = new byte[PpmData.picDivision[0] * PpmData.picDivision[1] * 2 + 2];
            int psCount = 0;
            sortedPiece[0] = (byte)PpmData.picDivision[0];
            sortedPiece[1] = (byte)PpmData.picDivision[1];
            for (int y = 0; y < PpmData.picDivision[1]; y++)
            {
                for (int x = 0; x < PpmData.picDivision[0]; x++)
                {
                    sortedPiece[(y * PpmData.picDivision[0] + x) * 2 + 2] = (byte)(pieceSeries[psCount] % PpmData.picDivision[0]);
                    sortedPiece[(y * PpmData.picDivision[0] + x) * 2 + 3] = (byte)(pieceSeries[psCount] / PpmData.picDivision[0]);
                    psCount++;
                }
            }
            return sortedPiece;

        }

        public int[] ppmSort(PpmData pd)
        {
            RgbData rd = new RgbData();
            BmpAnalyze baz = new BmpAnalyze();
            int[, ,] pieceAround = rd.bmpSort();
            int[][] pieceSeries = baz.analyze(pieceAround);
            return pieceSeries[8];
        }
    }

    public class EdgeData
    {
        public int[, ,] tbEdge;
        public int[, ,] lrEdge;
    }
    public class RgbData
    {
        public int[, ,] bmpSort()
        {
            int[, ,] piecesAround = new int[PpmData.picDivision[0] * PpmData.picDivision[1], 4, 2];
            int[] percentSet;
            int[,] maxTblr = new int[4, 2];
            for (int fpCount = 0; fpCount < PpmData.picDivision[0] * PpmData.picDivision[1]; fpCount++)
            {
                for (int spCount = 0; spCount < PpmData.picDivision[0] * PpmData.picDivision[1]; spCount++)
                {
                    if (fpCount == spCount)
                    {
                        continue;
                    }
                    percentSet = rgbJudge(fpCount, spCount);
                    if (maxTblr[percentSet[0], 1] < percentSet[1])
                    {
                        maxTblr[percentSet[0], 0] = spCount;
                        maxTblr[percentSet[0], 1] = percentSet[1];
                    }
                }
                for (int tblrSet = 0; tblrSet < 4; tblrSet++)
                {
                    piecesAround[fpCount, tblrSet, 0] = maxTblr[tblrSet, 0];
                    piecesAround[fpCount, tblrSet, 1] = maxTblr[tblrSet, 1];
                }
                for (int zero = 0; zero < 4; zero++)
                {
                    maxTblr[zero, 0] = 0;
                    maxTblr[zero, 1] = 0;
                }

            }
            return piecesAround;
        }
        private int[] rgbJudge(int firstPiece, int secondPiece)
        {
            int[] pieceEdge;
            EdgeData fped = new EdgeData();
            EdgeData sped = new EdgeData();
            fped = rgbGet(firstPiece);
            sped = rgbGet(secondPiece);
            pieceEdge = rgbCompare(fped, sped);
            return pieceEdge;
        }
        private EdgeData rgbGet(int pieceNumber)
        {
            int width = PpmData.picWidth / PpmData.picDivision[0];
            int height = PpmData.picHeight / PpmData.picDivision[1];
            EdgeData ed = new EdgeData();
            int[, ,] tbEdge = new int[2, 3, width];
            int[, ,] lrEdge = new int[2, 3, height];
            int pieceX = (pieceNumber % PpmData.picDivision[0]) * width;
            int pieceY = (pieceNumber / PpmData.picDivision[0]) * height;

            for (int tb = 0; tb < 2; tb++)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int rgb = 0; rgb < 3; rgb++)
                    {
                        tbEdge[tb, rgb, x] = PpmData.picBitmap[pieceX + x, pieceY + (height * tb) - tb, rgb];
                    }
                }
            }
            for (int lr = 0; lr < 2; lr++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int rgb = 0; rgb < 3; rgb++)
                    {
                        lrEdge[lr, rgb, y] = PpmData.picBitmap[pieceX + (width * lr) - lr, pieceY + y, rgb];
                    }
                }
            }
            ed.tbEdge = tbEdge;
            ed.lrEdge = lrEdge;
            return ed;
        }
        private int[] rgbCompare(EdgeData fped, EdgeData sped)
        {
            int[,] pieceEdge = new int[4, 2];
            int[] tblr = new int[4];
            for (int tbCount = 0; tbCount < 2; tbCount++)
            {
                for (int x = 0; x < fped.tbEdge.GetLength(2); x++)
                {
                    for (int rgb = 0; rgb < 3; rgb++)
                    {
                        if (Math.Abs(fped.tbEdge[tbCount, rgb, x] - sped.tbEdge[Math.Abs(tbCount - 1), rgb, x]) < 2)
                        {
                            tblr[tbCount]++;
                        }
                    }
                }
            }
            for (int lrCount = 0; lrCount < 2; lrCount++)
            {
                for (int y = 0; y < fped.lrEdge.GetLength(2); y++)
                {
                    for (int rgb = 0; rgb < 3; rgb++)
                    {
                        if (Math.Abs(fped.lrEdge[lrCount, rgb, y] - sped.lrEdge[Math.Abs(lrCount - 1), rgb, y]) < 2)
                        {
                            tblr[lrCount + 2]++;
                        }
                    }
                }
            }
            for (int tbPercent = 0; tbPercent < 2; tbPercent++)
            {
                tblr[tbPercent] = (int)((tblr[tbPercent] / (float)fped.tbEdge.GetLength(2)) * 100);
            }
            for (int lrPercent = 2; lrPercent < 4; lrPercent++)
            {
                tblr[lrPercent] = (int)((tblr[lrPercent] / (float)fped.lrEdge.GetLength(2)) * 100);
            }
            int[] max = { 0, tblr[0] };
            for (int maxLoop = 1; maxLoop < 4; maxLoop++)
            {
                if (max[1] < tblr[maxLoop])
                {
                    max[0] = maxLoop;
                    max[1] = tblr[maxLoop];
                }
            }
            return max;
        }
    }
    public class BmpAnalyze
    {
        public int[][] analyze(int[, ,] pieceAround)
        {
            int[][] piecePattern = this.tlSort(pieceAround);
            piecePattern = this.trSort(pieceAround, piecePattern);
            piecePattern = this.blSort(pieceAround, piecePattern);
            piecePattern = this.brSort(pieceAround, piecePattern);
            int mode = 0, count = 0, maxCount = 0;
            for (int pieceNum = 0; pieceNum < piecePattern[0].GetLength(0); pieceNum++)
            {
                for (int modeLoop = 0; modeLoop < 8; modeLoop++)
                {
                    for (int analyzeLoop = 0; analyzeLoop < 8; analyzeLoop++)
                    {
                        if (piecePattern[modeLoop][pieceNum] == piecePattern[analyzeLoop][pieceNum])
                        {
                            if ((pieceNum <= piecePattern[0].GetLength(0) / 2 && pieceNum % PpmData.picDivision[0] <= PpmData.picDivision[1] / 2 && analyzeLoop < 2) || (pieceNum <= piecePattern[0].GetLength(0) / 2 && pieceNum % PpmData.picDivision[0] > PpmData.picDivision[1] / 2 && analyzeLoop <= 3 && analyzeLoop > 1) || (pieceNum > piecePattern[0].GetLength(0) / 2 && pieceNum % PpmData.picDivision[0] <= PpmData.picDivision[1] / 2 && analyzeLoop <= 5 && analyzeLoop > 3) || (pieceNum > piecePattern[0].GetLength(0) / 2 && pieceNum % PpmData.picDivision[0] > PpmData.picDivision[1] / 2 && analyzeLoop <= 7 && analyzeLoop > 5))
                            {
                                count++;
                            }
                            count++;
                        }
                    }
                    if (count > maxCount)
                    {
                        mode = piecePattern[modeLoop][pieceNum];
                        maxCount = count;
                    }
                    count = 0;
                }
                piecePattern[8][pieceNum] = mode;
                maxCount = 0;
            }
            return piecePattern;

        }
        private int[][] tlSort(int[, ,] pieceAround)
        {
            int[][] piecePattern = new int[9][];
            for (int x = 0; x < piecePattern.Length; x++)
            {
                piecePattern[x] = new int[PpmData.picDivision[0] * PpmData.picDivision[1]];
            }
            int firstPiece = 600;
            int fpNumber = 0;
            for (int fpLoop = 0; fpLoop < PpmData.picDivision[0] * PpmData.picDivision[1]; fpLoop++)
            {
                if (pieceAround[fpLoop, 0, 1] + pieceAround[fpLoop, 2, 1] < firstPiece)
                {
                    firstPiece = (pieceAround[fpLoop, 0, 1] + pieceAround[fpLoop, 2, 1]);
                    fpNumber = fpLoop;
                }
            }
            piecePattern[0][0] = fpNumber;
            for (int pieceX = 1; pieceX < PpmData.picDivision[0]; pieceX++)
            {
                piecePattern[0][pieceX] = pieceAround[piecePattern[0][pieceX - 1], 3, 0];
            }
            for (int pieceY = 1; pieceY < PpmData.picDivision[1]; pieceY++)
            {
                piecePattern[0][pieceY * PpmData.picDivision[0]] = pieceAround[piecePattern[0][(pieceY - 1) * PpmData.picDivision[0]], 1, 0];
                for (int pieceX = 1; pieceX < PpmData.picDivision[0]; pieceX++)
                {
                    piecePattern[0][(pieceY * PpmData.picDivision[0]) + pieceX] = pieceAround[piecePattern[0][(pieceY * PpmData.picDivision[0]) + pieceX - 1], 3, 0];
                }
            }

            piecePattern[1][0] = fpNumber;
            for (int pieceY = 1; pieceY < PpmData.picDivision[1]; pieceY++)
            {
                piecePattern[1][pieceY * PpmData.picDivision[0]] = pieceAround[piecePattern[1][(pieceY - 1) * PpmData.picDivision[0]], 1, 0];
            }
            for (int pieceX = 1; pieceX < PpmData.picDivision[0]; pieceX++)
            {
                piecePattern[1][pieceX] = pieceAround[piecePattern[1][(pieceX - 1)], 3, 0];
                for (int pieceY = 1; pieceY < PpmData.picDivision[1]; pieceY++)
                {
                    piecePattern[1][(pieceY * PpmData.picDivision[0]) + pieceX] = pieceAround[piecePattern[1][(pieceY - 1) * PpmData.picDivision[0] + pieceX], 1, 0];
                }
            }
            return piecePattern;
        }
        private int[][] trSort(int[, ,] pieceAround, int[][] piecePattern)
        {
            int firstPiece = 600;
            int fpNumber = 0;
            for (int fpLoop = 0; fpLoop < PpmData.picDivision[0] * PpmData.picDivision[1]; fpLoop++)
            {
                if (pieceAround[fpLoop, 0, 1] + pieceAround[fpLoop, 3, 1] < firstPiece)
                {
                    firstPiece = (pieceAround[fpLoop, 0, 1] + pieceAround[fpLoop, 3, 1]);
                    fpNumber = fpLoop;
                }
            }
            piecePattern[2][PpmData.picDivision[0] - 1] = fpNumber;
            for (int pieceX = PpmData.picDivision[0] - 2; pieceX > -1; pieceX--)
            {
                piecePattern[2][pieceX] = pieceAround[piecePattern[2][pieceX + 1], 2, 0];
            }
            for (int pieceY = 1; pieceY < PpmData.picDivision[1]; pieceY++)
            {
                piecePattern[2][(pieceY + 1) * PpmData.picDivision[0] - 1] = pieceAround[piecePattern[0][pieceY * PpmData.picDivision[0] - 1], 1, 0];
                for (int pieceX = PpmData.picDivision[0] - 2; pieceX > -1; pieceX--)
                {
                    piecePattern[2][(pieceY * PpmData.picDivision[0]) + pieceX] = pieceAround[piecePattern[2][(pieceY * PpmData.picDivision[0]) + pieceX + 1], 2, 0];
                }
            }

            piecePattern[3][PpmData.picDivision[0] - 1] = fpNumber;
            for (int pieceY = 1; pieceY < PpmData.picDivision[1]; pieceY++)
            {
                piecePattern[3][(pieceY + 1) * PpmData.picDivision[0] - 1] = pieceAround[piecePattern[3][pieceY * PpmData.picDivision[0] - 1], 1, 0];
            }
            for (int pieceX = PpmData.picDivision[0] - 2; pieceX > -1; pieceX--)
            {
                piecePattern[3][pieceX] = pieceAround[piecePattern[3][(pieceX + 1)], 2, 0];
                for (int pieceY = 1; pieceY < PpmData.picDivision[1]; pieceY++)
                {
                    piecePattern[3][(pieceY * PpmData.picDivision[0]) + pieceX] = pieceAround[piecePattern[3][(pieceY - 1) * PpmData.picDivision[0] + pieceX], 1, 0];
                }
            }
            return piecePattern;
        }

        private int[][] blSort(int[, ,] pieceAround, int[][] piecePattern)
        {
            int firstPiece = 600;
            int fpNumber = 0;
            for (int fpLoop = 0; fpLoop < PpmData.picDivision[0] * PpmData.picDivision[1]; fpLoop++)
            {
                if (pieceAround[fpLoop, 1, 1] + pieceAround[fpLoop, 2, 1] < firstPiece)
                {
                    firstPiece = (pieceAround[fpLoop, 1, 1] + pieceAround[fpLoop, 2, 1]);
                    fpNumber = fpLoop;
                }
            }
            piecePattern[4][(PpmData.picDivision[0] - 1) * PpmData.picDivision[1]] = fpNumber;
            for (int pieceX = (PpmData.picDivision[0] - 1) * PpmData.picDivision[1] + 1; pieceX < PpmData.picDivision[0] * PpmData.picDivision[1]; pieceX++)
            {
                piecePattern[4][pieceX] = pieceAround[piecePattern[4][pieceX - 1], 3, 0];
            }
            for (int pieceY = PpmData.picDivision[1] - 2; pieceY > -1; pieceY--)
            {
                piecePattern[4][pieceY * PpmData.picDivision[0]] = pieceAround[piecePattern[4][(pieceY + 1) * PpmData.picDivision[0]], 0, 0];
                for (int pieceX = 1; pieceX < PpmData.picDivision[0]; pieceX++)
                {
                    piecePattern[4][(pieceY * PpmData.picDivision[0]) + pieceX] = pieceAround[piecePattern[4][(pieceY * PpmData.picDivision[0]) + pieceX - 1], 3, 0];
                }
            }

            piecePattern[5][(PpmData.picDivision[0] - 1) * PpmData.picDivision[1]] = fpNumber;
            for (int pieceY = PpmData.picDivision[1] - 2; pieceY > -1; pieceY--)
            {
                piecePattern[5][pieceY * PpmData.picDivision[0]] = pieceAround[piecePattern[5][(pieceY + 1) * PpmData.picDivision[0]], 0, 0];
            }
            for (int pieceX = (PpmData.picDivision[0] - 1) * PpmData.picDivision[1] + 1; pieceX < PpmData.picDivision[0] * PpmData.picDivision[1]; pieceX++)
            {
                piecePattern[5][pieceX] = pieceAround[piecePattern[5][(pieceX - 1)], 3, 0];
                for (int pieceY = PpmData.picDivision[1] - 2; pieceY > -1; pieceY--)
                {
                    piecePattern[5][pieceY * PpmData.picDivision[0] + pieceX % PpmData.picDivision[0]] = pieceAround[piecePattern[5][(pieceY * PpmData.picDivision[0] + pieceX % PpmData.picDivision[0]) + PpmData.picDivision[0]], 0, 0];
                }
            }
            return piecePattern;
        }

        private int[][] brSort(int[, ,] pieceAround, int[][] piecePattern)
        {
            int firstPiece = 600;
            int fpNumber = 0;
            for (int fpLoop = 0; fpLoop < PpmData.picDivision[0] * PpmData.picDivision[1]; fpLoop++)
            {
                if (pieceAround[fpLoop, 1, 1] + pieceAround[fpLoop, 3, 1] < firstPiece)
                {
                    firstPiece = (pieceAround[fpLoop, 1, 1] + pieceAround[fpLoop, 3, 1]);
                    fpNumber = fpLoop;
                }
            }
            piecePattern[6][PpmData.picDivision[0] * PpmData.picDivision[1] - 1] = fpNumber;
            for (int pieceX = PpmData.picDivision[0] * PpmData.picDivision[1] - 2; pieceX > PpmData.picDivision[0] * (PpmData.picDivision[1] - 1) - 1; pieceX--)
            {
                piecePattern[6][pieceX] = pieceAround[piecePattern[6][pieceX + 1], 2, 0];
            }
            for (int pieceY = PpmData.picDivision[1] - 2; pieceY > -1; pieceY--)
            {
                piecePattern[6][pieceY * PpmData.picDivision[0] + (PpmData.picDivision[0] - 1)] = pieceAround[piecePattern[6][(pieceY + 1) * PpmData.picDivision[0] + (PpmData.picDivision[0] - 1)], 0, 0];
                for (int pieceX = PpmData.picDivision[0] - 2; pieceX > -1; pieceX--)
                {
                    piecePattern[6][pieceY * PpmData.picDivision[0] + pieceX] = pieceAround[piecePattern[6][pieceY * PpmData.picDivision[0] + pieceX + 1], 2, 0];
                }
            }

            piecePattern[7][PpmData.picDivision[0] * PpmData.picDivision[1] - 1] = fpNumber;
            for (int pieceY = PpmData.picDivision[1] - 2; pieceY > -1; pieceY--)
            {
                piecePattern[7][pieceY * PpmData.picDivision[0] + (PpmData.picDivision[0] - 1)] = pieceAround[piecePattern[7][(pieceY + 1) * PpmData.picDivision[0] + (PpmData.picDivision[0] - 1)], 0, 0];
            }
            for (int pieceX = PpmData.picDivision[0] * PpmData.picDivision[1] - 2; pieceX > PpmData.picDivision[0] * (PpmData.picDivision[1] - 1) - 1; pieceX--)
            {
                piecePattern[7][pieceX] = pieceAround[piecePattern[7][(pieceX + 1)], 2, 0];
                for (int pieceY = PpmData.picDivision[1] - 2; pieceY > -1; pieceY--)
                {
                    piecePattern[7][pieceY * PpmData.picDivision[0] + pieceX % PpmData.picDivision[0]] = pieceAround[piecePattern[7][(pieceY * PpmData.picDivision[0] + pieceX % PpmData.picDivision[0]) + PpmData.picDivision[0]], 0, 0];
                }
            }
            return piecePattern;
        }
    }
}
