using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProgramingContest3
{
    public class ppmedit
    {
        public ppmdata ppmd = new ppmdata();

        public byte[,] picsortToByte(string ppmfile)
        {
            ppmdata ppmd = this.ppmtoint(ppmfile);
            int[] pieceseries = this.ppmsort(ppmd);
            byte[,] sortedpiece = new byte[ppmd.picdivision[0], ppmd.picdivision[1]];
            int pscount = 0;
            for (int y = 0; y < ppmd.picdivision[1]; y++)
            {
                for (int x = 0; x < ppmd.picdivision[0]; x++)
                {
                    sortedpiece[x, y] = (byte)((pieceseries[pscount] % ppmd.picdivision[0])*16 + (pieceseries[pscount] / ppmd.picdivision[0]));
                    pscount++;
                }
            }
            return sortedpiece;
        }


        public ppmdata ppmtoint(string ppmfile)
        {
            FileStream fs = new FileStream(ppmfile, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string[] str = new string[6];
            str[0] = sr.ReadLine();
            str[1] = sr.ReadLine();
            str[2] = sr.ReadLine();
            str[3] = sr.ReadLine();
            str[4] = sr.ReadLine();
            str[5] = sr.ReadLine();
            char[] space = new char[] { ' ' };
            int[] pixel = new int[3];
            int[, ,] ppmrgb = new int[int.Parse(str[4].Split(space)[0]), int.Parse(str[4].Split(space)[1]), 3];
            fs.Seek(0, SeekOrigin.Begin);
            int returncount = 0;
            while (returncount < 6)
            {
                if (fs.ReadByte() == 0x0A)
                {
                    returncount++;
                }
            }
            for (int Ycount = 0; Ycount < ppmrgb.GetLength(1); Ycount++)
            {
                for (int Xcount = 0; Xcount < ppmrgb.GetLength(0); Xcount++)
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
                        ppmrgb[Xcount, Ycount, rgb] = pixel[rgb];
                    }
                }
                if (pixel[0] == -1)
                {
                    break;
                }
            }
            fs.Close();
            ppmd.picdivision[0] = int.Parse((str[1].Trim(' ', '#').Split(space))[0]);
            ppmd.picdivision[1] = int.Parse((str[1].Trim(' ', '#').Split(space))[1]);
            ppmd.picsetrepeat = int.Parse((str[2].Trim(' ', '#')));
            ppmd.picsetrate = int.Parse((str[3].Trim(' ', '#').Split(space))[0]);
            ppmd.picmoverate = int.Parse((str[3].Trim(' ', '#').Split(space))[1]);
            ppmd.picwidth = int.Parse(str[4].Split(space)[0]);
            ppmd.picheight = int.Parse(str[4].Split(space)[1]);
            ppmd.picbitmap = ppmrgb;
            return ppmd;
        }

        private int[] ppmsort(ppmdata pd)
        {
            rgbdata rd = new rgbdata();
            bmpanalyze baz = new bmpanalyze();
            int[, ,] piecearound = rd.bmpsort(pd);
            int[] pieceseries = baz.analyze(pd, piecearound);
            return pieceseries;
        }
    }
    public class ppmdata
    {
        public int[] picdivision = new int[2];
        public int picsetrepeat;
        public int picsetrate;
        public int picmoverate;
        public int picwidth;
        public int picheight;
        public int[, ,] picbitmap;
    }
    public class edgedata
    {
        public int[, ,] tbedge;
        public int[, ,] lredge;
    }
    public class rgbdata
    {
        public int[, ,] bmpsort(ppmdata pd)
        {
            int[, ,] piecesaround = new int[pd.picdivision[0] * pd.picdivision[1], 4, 2];
            int[] percentset;
            int[,] maxtblr = new int[4, 2];
            for (int fpcount = 0; fpcount < pd.picdivision[0] * pd.picdivision[1]; fpcount++)
            {
                for (int spcount = 0; spcount < pd.picdivision[0] * pd.picdivision[1]; spcount++)
                {
                    if (fpcount == spcount)
                    {
                        continue;
                    }
                    percentset = rgbjudge(pd, fpcount, spcount);
                    if (maxtblr[percentset[0], 1] < percentset[1])
                    {
                        maxtblr[percentset[0], 0] = spcount;
                        maxtblr[percentset[0], 1] = percentset[1];
                    }
                }
                for (int tblrset = 0; tblrset < 4; tblrset++)
                {
                    piecesaround[fpcount, tblrset, 0] = maxtblr[tblrset, 0];
                    piecesaround[fpcount, tblrset, 1] = maxtblr[tblrset, 1];
                }
                for (int zero = 0; zero < 4; zero++)
                {
                    maxtblr[zero, 0] = 0;
                    maxtblr[zero, 1] = 0;
                }

            }
            return piecesaround;
        }
        private int[] rgbjudge(ppmdata pd, int firstpiece, int secondpiece)
        {
            int[] pieceedge;
            edgedata fped = new edgedata();
            edgedata sped = new edgedata();
            fped = rgbget(pd, firstpiece);
            sped = rgbget(pd, secondpiece);
            pieceedge = rgbcompare(fped, sped);
            return pieceedge;
        }
        private edgedata rgbget(ppmdata pd, int piecenumber)
        {
            int width = pd.picwidth / pd.picdivision[0];
            int height = pd.picheight / pd.picdivision[1];
            edgedata ed = new edgedata();
            int[, ,] tbedge = new int[2, 3, width];
            int[, ,] lredge = new int[2, 3, height];
            int pieceX = (piecenumber % pd.picdivision[0]) * width;
            int pieceY = (piecenumber / pd.picdivision[0]) * height;

            for (int tb = 0; tb < 2; tb++)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int rgb = 0; rgb < 3; rgb++)
                    {
                        tbedge[tb, rgb, x] = pd.picbitmap[pieceX + x, pieceY + (height * tb) - tb, rgb];
                    }
                }
            }
            for (int lr = 0; lr < 2; lr++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int rgb = 0; rgb < 3; rgb++)
                    {
                        lredge[lr, rgb, y] = pd.picbitmap[pieceX + (width * lr) - lr, pieceY + y, rgb];
                    }
                }
            }
            ed.tbedge = tbedge;
            ed.lredge = lredge;
            return ed;
        }
        private int[] rgbcompare(edgedata fped, edgedata sped)
        {
            int[,] pieceedge = new int[4, 2];
            int[] tblr = new int[4];
            for (int tbcount = 0; tbcount < 2; tbcount++)
            {
                for (int x = 0; x < fped.tbedge.GetLength(2); x++)
                {
                    for (int rgb = 0; rgb < 3; rgb++)
                    {
                        if (Math.Abs(fped.tbedge[tbcount, rgb, x] - sped.tbedge[Math.Abs(tbcount - 1), rgb, x]) < 2)
                        {
                            tblr[tbcount]++;
                        }
                    }
                }
            }
            for (int lrcount = 0; lrcount < 2; lrcount++)
            {
                for (int y = 0; y < fped.lredge.GetLength(2); y++)
                {
                    for (int rgb = 0; rgb < 3; rgb++)
                    {
                        if (Math.Abs(fped.lredge[lrcount, rgb, y] - sped.lredge[Math.Abs(lrcount - 1), rgb, y]) < 2)
                        {
                            tblr[lrcount + 2]++;
                        }
                    }
                }
            }
            for (int tbpercent = 0; tbpercent < 2; tbpercent++)
            {
                tblr[tbpercent] = (int)((tblr[tbpercent] / (float)fped.tbedge.GetLength(2)) * 100);
            }
            for (int lrpercent = 2; lrpercent < 4; lrpercent++)
            {
                tblr[lrpercent] = (int)((tblr[lrpercent] / (float)fped.lredge.GetLength(2)) * 100);
            }
            int[] max = { 0, tblr[0] };
            for (int maxloop = 1; maxloop < 4; maxloop++)
            {
                if (max[1] < tblr[maxloop])
                {
                    max[0] = maxloop;
                    max[1] = tblr[maxloop];
                }
            }
            return max;
        }
    }
    public class bmpanalyze
    {
        public int[] analyze(ppmdata pd, int[, ,] piecearound)
        {
            int[,] piecepattern = this.tlsort(pd, piecearound);
            int[,] piece = this.trsort(pd, piecearound, piecepattern);
            piece = this.blsort(pd, piecearound, piece);
            piece = this.brsort(pd, piecearound, piecepattern);
            int mode = 0, count = 0, maxcount = 0;
            int[] pieceseries = new int[piece.GetLength(1)];
            for (int piecenum = 0; piecenum < piece.GetLength(1); piecenum++)
            {
                for (int modeloop = 0; modeloop < 8; modeloop++)
                {
                    for (int analyzeloop = 0; analyzeloop < 8; analyzeloop++)
                    {
                        if (piece[modeloop, piecenum] == piece[analyzeloop, piecenum])
                        {
                            count++;
                        }
                    }
                    if (count > maxcount)
                    {
                        mode = piece[modeloop, piecenum];
                        maxcount = count;
                    }
                    count = 0;
                }
                pieceseries[piecenum] = mode;
                maxcount = 0;
            }
            /*for (int i = 0; i < piece.GetLength(1);i++ )
            {
                pieceseries[i] = piece[7, i];
            }*/
            return pieceseries;

        }
        private int[,] tlsort(ppmdata pd, int[, ,] piecearound)
        {
            int[,] piecepattern = new int[8, pd.picdivision[0] * pd.picdivision[1]];
            int firstpiece = 600;
            int fpnumber = 0;
            for (int fploop = 0; fploop < pd.picdivision[0] * pd.picdivision[1]; fploop++)
            {
                if (piecearound[fploop, 0, 1] + piecearound[fploop, 2, 1] < firstpiece)
                {
                    firstpiece = (piecearound[fploop, 0, 1] + piecearound[fploop, 2, 1]);
                    fpnumber = fploop;
                }
            }
            piecepattern[0, 0] = fpnumber;
            for (int pieceX = 1; pieceX < pd.picdivision[0]; pieceX++)
            {
                piecepattern[0, pieceX] = piecearound[piecepattern[0, pieceX - 1], 3, 0];
            }
            for (int pieceY = 1; pieceY < pd.picdivision[1]; pieceY++)
            {
                piecepattern[0, pieceY * pd.picdivision[0]] = piecearound[piecepattern[0, (pieceY - 1) * pd.picdivision[0]], 1, 0];
                for (int pieceX = 1; pieceX < pd.picdivision[0]; pieceX++)
                {
                    piecepattern[0, (pieceY * pd.picdivision[0]) + pieceX] = piecearound[piecepattern[0, (pieceY * pd.picdivision[0]) + pieceX - 1], 3, 0];
                }
            }

            piecepattern[1, 0] = fpnumber;
            for (int pieceY = 1; pieceY < pd.picdivision[1]; pieceY++)
            {
                piecepattern[1, pieceY * pd.picdivision[0]] = piecearound[piecepattern[1, (pieceY - 1) * pd.picdivision[0]], 1, 0];
            }
            for (int pieceX = 1; pieceX < pd.picdivision[0]; pieceX++)
            {
                piecepattern[1, pieceX] = piecearound[piecepattern[1, (pieceX - 1)], 3, 0];
                for (int pieceY = 1; pieceY < pd.picdivision[1]; pieceY++)
                {
                    piecepattern[1, (pieceY * pd.picdivision[0]) + pieceX] = piecearound[piecepattern[1, (pieceY - 1) * pd.picdivision[0] + pieceX], 1, 0];
                }
            }
            return piecepattern;
        }
        private int[,] trsort(ppmdata pd, int[, ,] piecearound, int[,] piecepattern)
        {
            int firstpiece = 600;
            int fpnumber = 0;
            for (int fploop = 0; fploop < pd.picdivision[0] * pd.picdivision[1]; fploop++)
            {
                if (piecearound[fploop, 0, 1] + piecearound[fploop, 3, 1] < firstpiece)
                {
                    firstpiece = (piecearound[fploop, 0, 1] + piecearound[fploop, 3, 1]);
                    fpnumber = fploop;
                }
            }
            piecepattern[2, pd.picdivision[0] - 1] = fpnumber;
            for (int pieceX = pd.picdivision[0] - 2; pieceX > -1; pieceX--)
            {
                piecepattern[2, pieceX] = piecearound[piecepattern[2, pieceX + 1], 2, 0];
            }
            for (int pieceY = 1; pieceY < pd.picdivision[1]; pieceY++)
            {
                piecepattern[2, (pieceY + 1) * pd.picdivision[0] - 1] = piecearound[piecepattern[0, pieceY * pd.picdivision[0] - 1], 1, 0];
                for (int pieceX = pd.picdivision[0] - 2; pieceX > -1; pieceX--)
                {
                    piecepattern[2, (pieceY * pd.picdivision[0]) + pieceX] = piecearound[piecepattern[2, (pieceY * pd.picdivision[0]) + pieceX + 1], 2, 0];
                }
            }

            piecepattern[3, pd.picdivision[0] - 1] = fpnumber;
            for (int pieceY = 1; pieceY < pd.picdivision[1]; pieceY++)
            {
                piecepattern[3, (pieceY + 1) * pd.picdivision[0] - 1] = piecearound[piecepattern[3, pieceY * pd.picdivision[0] - 1], 1, 0];
            }
            for (int pieceX = pd.picdivision[0] - 2; pieceX > -1; pieceX--)
            {
                piecepattern[3, pieceX] = piecearound[piecepattern[3, (pieceX + 1)], 2, 0];
                for (int pieceY = 1; pieceY < pd.picdivision[1]; pieceY++)
                {
                    piecepattern[3, (pieceY * pd.picdivision[0]) + pieceX] = piecearound[piecepattern[3, (pieceY - 1) * pd.picdivision[0] + pieceX], 1, 0];
                }
            }
            return piecepattern;
        }

        private int[,] blsort(ppmdata pd, int[, ,] piecearound, int[,] piecepattern)
        {
            int firstpiece = 600;
            int fpnumber = 0;
            for (int fploop = 0; fploop < pd.picdivision[0] * pd.picdivision[1]; fploop++)
            {
                if (piecearound[fploop, 1, 1] + piecearound[fploop, 2, 1] < firstpiece)
                {
                    firstpiece = (piecearound[fploop, 1, 1] + piecearound[fploop, 2, 1]);
                    fpnumber = fploop;
                }
            }
            piecepattern[4, (pd.picdivision[0] - 1) * pd.picdivision[1]] = fpnumber;
            for (int pieceX = (pd.picdivision[0] - 1) * pd.picdivision[1] + 1; pieceX < pd.picdivision[0] * pd.picdivision[1]; pieceX++)
            {
                piecepattern[4, pieceX] = piecearound[piecepattern[4, pieceX - 1], 3, 0];
            }
            for (int pieceY = pd.picdivision[1] - 2; pieceY > -1; pieceY--)
            {
                piecepattern[4, pieceY * pd.picdivision[0]] = piecearound[piecepattern[4, (pieceY + 1) * pd.picdivision[0]], 0, 0];
                for (int pieceX = 1; pieceX < pd.picdivision[0]; pieceX++)
                {
                    piecepattern[4, (pieceY * pd.picdivision[0]) + pieceX] = piecearound[piecepattern[4, (pieceY * pd.picdivision[0]) + pieceX - 1], 3, 0];
                }
            }

            piecepattern[5, (pd.picdivision[0] - 1) * pd.picdivision[1]] = fpnumber;
            for (int pieceY = pd.picdivision[1] - 2; pieceY > -1; pieceY--)
            {
                piecepattern[5, pieceY * pd.picdivision[0]] = piecearound[piecepattern[5, (pieceY + 1) * pd.picdivision[0]], 0, 0];
            }
            for (int pieceX = (pd.picdivision[0] - 1) * pd.picdivision[1] + 1; pieceX < pd.picdivision[0] * pd.picdivision[1]; pieceX++)
            {
                piecepattern[5, pieceX] = piecearound[piecepattern[5, (pieceX - 1)], 3, 0];
                for (int pieceY = pd.picdivision[1] - 2; pieceY > -1; pieceY--)
                {
                    piecepattern[5, pieceY * pd.picdivision[0] + pieceX % pd.picdivision[0]] = piecearound[piecepattern[5, (pieceY * pd.picdivision[0] + pieceX % pd.picdivision[0]) + pd.picdivision[0]], 0, 0];
                }
            }
            return piecepattern;
        }

        private int[,] brsort(ppmdata pd, int[, ,] piecearound, int[,] piecepattern)
        {
            int firstpiece = 600;
            int fpnumber = 0;
            for (int fploop = 0; fploop < pd.picdivision[0] * pd.picdivision[1]; fploop++)
            {
                if (piecearound[fploop, 1, 1] + piecearound[fploop, 3, 1] < firstpiece)
                {
                    firstpiece = (piecearound[fploop, 1, 1] + piecearound[fploop, 3, 1]);
                    fpnumber = fploop;
                }
            }
            piecepattern[6, pd.picdivision[0] * pd.picdivision[1] - 1] = fpnumber;
            for (int pieceX = pd.picdivision[0] * pd.picdivision[1] - 2; pieceX > pd.picdivision[0] * (pd.picdivision[1] - 1) - 1; pieceX--)
            {
                piecepattern[6, pieceX] = piecearound[piecepattern[6, pieceX + 1], 2, 0];
            }
            for (int pieceY = pd.picdivision[1] - 2; pieceY > -1; pieceY--)
            {
                piecepattern[6, pieceY * pd.picdivision[0] + (pd.picdivision[0] - 1)] = piecearound[piecepattern[6, (pieceY + 1) * pd.picdivision[0] + (pd.picdivision[0] - 1)], 0, 0];
                for (int pieceX = pd.picdivision[0] - 2; pieceX > -1; pieceX--)
                {
                    piecepattern[6, pieceY * pd.picdivision[0] + pieceX] = piecearound[piecepattern[6, pieceY * pd.picdivision[0] + pieceX + 1], 2, 0];
                }
            }
            piecepattern[7, pd.picdivision[0] * pd.picdivision[1] - 1] = fpnumber;
            for (int pieceY = pd.picdivision[1] - 2; pieceY > -1; pieceY--)
            {
                piecepattern[7, pieceY * pd.picdivision[0] + (pd.picdivision[0] - 1)] = piecearound[piecepattern[7, (pieceY + 1) * pd.picdivision[0] + (pd.picdivision[0] - 1)], 0, 0];
            }
            for (int pieceX = pd.picdivision[0] * pd.picdivision[1] - 2; pieceX > pd.picdivision[0] * (pd.picdivision[1] - 1) - 1; pieceX--)
            {
                piecepattern[7, pieceX] = piecearound[piecepattern[7, (pieceX + 1)], 2, 0];
                for (int pieceY = pd.picdivision[1] - 2; pieceY > -1; pieceY--)
                {
                    piecepattern[7, pieceY * pd.picdivision[0] + pieceX % pd.picdivision[0]] = piecearound[piecepattern[7, (pieceY * pd.picdivision[0] + pieceX % pd.picdivision[0]) + pd.picdivision[0]], 0, 0];
                }
            }

            return piecepattern;
        }
    }
}
