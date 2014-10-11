using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProconSortUI;

namespace ProgramingContestImageSort
{
    public class EdgeCompare
    {
        public int[][] compare()
        {
            int pieceNumber = PpmData.picDivision[0] * PpmData.picDivision[1];
            int arraySize = (((pieceNumber - 1) * 4) * pieceNumber)/2;
            int arrayCount = 0;
            int[][] edgeCompareValue = new int[arraySize][];
            for (int i = 0; i < arraySize; i++)
            {
                edgeCompareValue[i] = new int[7];
            }
            int[] directionValue;

            for (int firstPieceY = 0; firstPieceY < PpmData.picDivision[1]; firstPieceY++)
            {
                for (int firstPieceX = 0; firstPieceX < PpmData.picDivision[0]; firstPieceX++)
                {
                    for (int secondPieceY = 0; secondPieceY < PpmData.picDivision[1]; secondPieceY++)
                    {
                        for (int secondPieceX = 0; secondPieceX < PpmData.picDivision[0]; secondPieceX++)
                        {
                            if (firstPieceX + (firstPieceY * PpmData.picDivision[0]) >= secondPieceX + (secondPieceY * PpmData.picDivision[0]))
                            {
                                continue;
                            }

                            for (int color = 0; color < 3; color++)
                            {
                                directionValue = edgeValueCalc(firstPieceX, firstPieceY, secondPieceX, secondPieceY, color);
                                for(int direction = 0;direction < 4;direction++)
                                {
                                    if(color == 0)
                                    {
                                        edgeCompareValue[arrayCount + direction][0] = firstPieceX;
                                        edgeCompareValue[arrayCount + direction][1] = firstPieceY;
                                        edgeCompareValue[arrayCount + direction][2] = direction;
                                        edgeCompareValue[arrayCount + direction][3] = secondPieceX;
                                        edgeCompareValue[arrayCount + direction][4] = secondPieceY;
                                        edgeCompareValue[arrayCount + direction][5] = direction < 2 ? direction + 2 : direction - 2;
                                    }
                                    edgeCompareValue[arrayCount+direction][6] += directionValue[direction];
                                }
                            }
                            arrayCount += 4;
                        }
                    }
                }
            }
            return edgeCompareValue;

        }

        private int[] edgeValueCalc(int firstPieceX, int firstPieceY, int secondPieceX, int secondPieceY, int color)
        {
            int firstPieceInitialX = (PpmData.picWidth / PpmData.picDivision[0]) * firstPieceX;
            int firstPieceInitialY = (PpmData.picHeight / PpmData.picDivision[1]) * firstPieceY;
            int secondPieceInitialX = (PpmData.picWidth / PpmData.picDivision[0]) * secondPieceX;
            int secondPieceInitialY = (PpmData.picHeight / PpmData.picDivision[1]) * secondPieceY;
            int[] compareValue = new int[4];
            for (int firstPieceEdgeDirection = 0; firstPieceEdgeDirection < 4; firstPieceEdgeDirection++) //上左下右で処理
            {
                int length = (firstPieceEdgeDirection % 2 == 0) ? (PpmData.picWidth / PpmData.picDivision[0]) : (PpmData.picHeight / PpmData.picDivision[1]);

                for (int comparePixel = 0;comparePixel < length;comparePixel++)
                {
                    int firstPiecePixelX = 0;
                    int firstPiecePixelY = 0;
                    int secondPiecePixelX = 0;
                    int secondPiecePixelY = 0;
                    switch(firstPieceEdgeDirection)
                    {
                        case 0:
                            firstPiecePixelX = comparePixel;
                            secondPiecePixelX = comparePixel;
                            secondPiecePixelY = PpmData.picHeight / PpmData.picDivision[1] - 1;
                            break;
                        case 1:
                            secondPiecePixelX = PpmData.picWidth / PpmData.picDivision[0] - 1;
                            firstPiecePixelY = comparePixel;
                            secondPiecePixelY = comparePixel;
                            break;
                        case 2:
                            firstPiecePixelX = comparePixel;
                            secondPiecePixelX = comparePixel;
                            firstPiecePixelY = PpmData.picHeight / PpmData.picDivision[1] - 1;
                            break;
                        case 3:
                            firstPiecePixelX = PpmData.picWidth / PpmData.picDivision[0] - 1;
                            firstPiecePixelY = comparePixel;
                            secondPiecePixelY = comparePixel;
                            break;
                    }

                   compareValue[firstPieceEdgeDirection] += Math.Abs(PpmData.picBitmap[firstPieceInitialX + firstPiecePixelX,firstPieceInitialY + firstPiecePixelY, color] - PpmData.picBitmap[secondPieceInitialX + secondPiecePixelX,secondPieceInitialY + secondPiecePixelY, color]);

                }

            }
            return compareValue;
        }
    }
}
