using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProconSortUI
{
    public class PpmData
    {
        public static int[] picDivision = new int[2];
        public static int picSetRepeat { get; private set; }
        public static int picSetRate { get; private set; }
        public static int picMoveRate { get; private set; }
        public static int picWidth { get; private set; }
        public static int picHeight { get; private set; }
        public static int[, ,] picBitmap { get; private set; }

        public int this[int i]
        {
            get { return PpmData.picDivision[i]; }
        }

        public int this[int i,int j,int k]
        {
            get { return PpmData.picBitmap[i,j,k]; }
        }

        public void ppmRead(string ppmFile)
        {
            FileStream fs = new FileStream(ppmFile, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string[] str = new string[6];
            for (int i = 0; i < 6;i++ ) //6行のテキスト行の読み取り
            {
                str[i] = sr.ReadLine();
            }
            char[] space = new char[] { ' ' };
            int[] pixel = new int[3];
            int[, ,] ppmRgb = new int[int.Parse(str[4].Split(space)[0]), int.Parse(str[4].Split(space)[1]), 3];
            fs.Seek(0, SeekOrigin.Begin);
            int returncount = 0;
            while (returncount < 6)//6行のテキスト行の読み飛ばし
            {
                if (fs.ReadByte() == 0x0A)
                {
                    returncount++;
                }
            }
            for (int Ycount = 0; Ycount < ppmRgb.GetLength(1); Ycount++)
            {
                for (int Xcount = 0; Xcount < ppmRgb.GetLength(0); Xcount++)
                {
                    pixel[0] = fs.ReadByte();
                    pixel[1] = fs.ReadByte();
                    pixel[2] = fs.ReadByte();
                    if (pixel[2] == -1)
                    {
                        break;
                    }
                    for (int rgb = 0; rgb < 3; rgb++)
                    {
                        ppmRgb[Xcount, Ycount, rgb] = pixel[rgb];
                    }
                }
                if (pixel[0] == -1)
                {
                    break;
                }
            }
            fs.Close();

            PpmData.picDivision[0] = int.Parse((str[1].Trim(' ', '#').Split(space))[0]);
            PpmData.picDivision[1] = int.Parse((str[1].Trim(' ', '#').Split(space))[1]);
            PpmData.picSetRepeat = int.Parse((str[2].Trim(' ', '#')));
            PpmData.picSetRate = int.Parse((str[3].Trim(' ', '#').Split(space))[0]);
            PpmData.picMoveRate = int.Parse((str[3].Trim(' ', '#').Split(space))[1]);
            PpmData.picWidth = int.Parse(str[4].Split(space)[0]);
            PpmData.picHeight = int.Parse(str[4].Split(space)[1]);
            PpmData.picBitmap = ppmRgb;
        }
    }
}
