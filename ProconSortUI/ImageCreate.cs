using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace ProconSortUI
{
    class ImageCreate
    {
        public Bitmap ppmCut(byte[] sortedpiece,bool[] colorpiece = null)
        {
            int width = PpmData.picWidth / PpmData.picDivision[0];
            int height = PpmData.picHeight / PpmData.picDivision[1];
            int x, y;
            int sortedX = 0, sortedY = 0, xStart = 0, yStart = 0;
            int[, ,] sortedBmp = new int[PpmData.picWidth, PpmData.picHeight, 3];
            for (int i = 2; i < (sortedpiece[0] * sortedpiece[1])*2+2;i += 2)
            {
                x = sortedpiece[i];
                y = sortedpiece[i+1];

                if ((x < 16&&(colorpiece==null||!colorpiece[(i-2)/2])))
                {
                    for (int originY = y * height; originY < y * height + height; originY++)
                    {
                        for (int originX = x * width; originX < x * width + width; originX++)
                        {
                            for (int rgb = 0; rgb < 3; rgb++)
                            {
                                sortedBmp[sortedX + (xStart * width), sortedY + (yStart * height), rgb] = PpmData.picBitmap[originX, originY, rgb];
                            }
                            sortedX++;
                        }
                        sortedY++;
                        sortedX = 0;
                    }
                    xStart++;
                    if (xStart > PpmData.picDivision[0] - 1)
                    {
                        xStart = 0;
                        yStart++;
                    }
                }
                else if(colorpiece != null&&colorpiece[(i-2)/2])
                {
                    for (int originY = y * height; originY < y * height + height; originY++)
                    {
                        for (int originX = x * width; originX < x * width + width; originX++)
                        {
                            
                            sortedBmp[sortedX + (xStart * width), sortedY + (yStart * height), 0] = (originX - x * width < 20 && originY - y * height < 20) ? 255 : PpmData.picBitmap[originX, originY, 0];
                            sortedBmp[sortedX + (xStart * width), sortedY + (yStart * height), 1] = (originX - x * width < 20 && originY - y * height < 20) ? 0 : PpmData.picBitmap[originX, originY, 1];
                            sortedBmp[sortedX + (xStart * width), sortedY + (yStart * height), 2] = (originX - x * width < 20 && originY - y * height < 20) ? 0 : PpmData.picBitmap[originX, originY, 2];
                            sortedX++;
                        }
                        sortedY++;
                        sortedX = 0;
                    }
                    xStart++;
                    if (xStart > PpmData.picDivision[0] - 1)
                    {
                        xStart = 0;
                        yStart++;
                    }
                }
                else
                {
                    for (int originY = 0; originY < height; originY++)
                    {
                        for (int originX = 0; originX < width; originX++)
                        {
                            for (int rgb = 0; rgb < 3; rgb++)
                            {
                                sortedBmp[sortedX + (xStart * width), sortedY + (yStart * height), rgb] = 0;
                            }
                            sortedX++;
                        }
                        sortedY++;
                        sortedX = 0;
                    }
                    xStart++;
                    if (xStart > PpmData.picDivision[0] - 1)
                    {
                        xStart = 0;
                        yStart++;
                    }
                }
                sortedX = 0;
                sortedY = 0;
            }
            Bitmap bmp = new Bitmap(PpmData.picWidth, PpmData.picHeight);
            var bmpplus = new BitmapPlus(bmp);
            bmpplus.BeginAccess();
            for (int yCount = 0; yCount < PpmData.picHeight; yCount++)
            {
                for (int xCount = 0; xCount < PpmData.picWidth; xCount++)
                {
                    Color rgb = Color.FromArgb(sortedBmp[xCount, yCount, 0], sortedBmp[xCount, yCount, 1], sortedBmp[xCount, yCount, 2]);
                    bmpplus.SetPixel(xCount, yCount, rgb);
                }
            }
            bmpplus.EndAccess();

            return bmp;
        }
    }
}
