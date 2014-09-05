using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramingContest1;

namespace ProgramingContestImageSort
{
    class EdgeCompare
    {
        public int[][] compare()
        {
            int pieceNumber = PpmData.picPieceX * PpmData.picPieceY;
            int arraySize = (((pieceNumber - 1) * 4) * pieceNumber)/2;
            int arrayCount = 0;
            int[][] edgeCompareValue = new int[arraySize][];
            for (int i = 0; i < arraySize; i++)
            {
                edgeCompareValue[i] = new int[7];
            }
            int[] directionValue;

            for (int firstPieceY = 0; firstPieceY < PpmData.picPieceY; firstPieceY++)
            {
                for (int firstPieceX = 0; firstPieceX < PpmData.picPieceX; firstPieceX++)
                {
                    for (int secondPieceY = 0; secondPieceY < PpmData.picPieceY; secondPieceY++)
                    {
                        for (int secondPieceX = 0; secondPieceX < PpmData.picPieceX; secondPieceX++)
                        {
                            if (firstPieceX + (firstPieceY * PpmData.picPieceX) >= secondPieceX + (secondPieceY * PpmData.picPieceX))
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
            int firstPieceInitialX = (PpmData.picWidth / PpmData.picPieceX) * firstPieceX;
            int firstPieceInitialY = (PpmData.picHeight / PpmData.picPieceY) * firstPieceY;
            int secondPieceInitialX = (PpmData.picWidth / PpmData.picPieceX) * secondPieceX;
            int secondPieceInitialY = (PpmData.picHeight / PpmData.picPieceY) * secondPieceY;
            int[] compareValue = new int[4];
            for (int firstPieceEdgeDirection = 0; firstPieceEdgeDirection < 4; firstPieceEdgeDirection++) //上左下右で処理
            {
                int length = (firstPieceEdgeDirection % 2 == 0) ? (PpmData.picWidth / PpmData.picPieceX) : (PpmData.picHeight / PpmData.picPieceY);

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
                            secondPiecePixelY = PpmData.picHeight / PpmData.picPieceY - 1;
                            break;
                        case 1:
                            secondPiecePixelX = PpmData.picWidth / PpmData.picPieceX - 1;
                            firstPiecePixelY = comparePixel;
                            secondPiecePixelY = comparePixel;
                            break;
                        case 2:
                            firstPiecePixelX = comparePixel;
                            secondPiecePixelX = comparePixel;
                            firstPiecePixelY = PpmData.picHeight / PpmData.picPieceY - 1;
                            break;
                        case 3:
                            firstPiecePixelX = PpmData.picWidth / PpmData.picPieceX - 1;
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
