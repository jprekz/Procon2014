using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ProconSortUI
{
    public class ImageResize
    {
        public Image resize(Image input, int size)
        {
            var width = 0;
            var height = 0;
            if (input.Width > input.Height)
            {
                width = size;
                height = (int)(input.Height * ((double)size / input.Width));
            }
            else if (input.Width < input.Height)
            {
                height = size;
                width = (int)(input.Width * ((double)size / input.Height));
            }
            else
            {
                height = size;
                width = size;
            }
            var canvas = new Bitmap(width, height);
            var graphics = Graphics.FromImage(canvas);
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            graphics.DrawImage(input, 0, 0, width, height);
            return canvas;
        }
    }
}
