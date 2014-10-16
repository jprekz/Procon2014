using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace ProconSortUI
{
    /// <summary>
    /// Bitmap処理を高速化するためのクラス
    /// </summary>
    class BitmapPlus
    {
        /// <summary>
        /// オリジナルのBitmapオブジェクト
        /// </summary>
        private Bitmap _bmp = null;

        /// <summary>
        /// Bitmapに直接アクセスするためのオブジェクト
        /// </summary>
        private BitmapData _img = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="original"></param>
        public BitmapPlus(Bitmap original)
        {
            // オリジナルのBitmapオブジェクトを保存
            _bmp = original;
        }

        /// <summary>
        /// Bitmap処理の高速化開始
        /// </summary>
        public void BeginAccess()
        {
            // Bitmapに直接アクセスするためのオブジェクト取得(LockBits)
            _img = _bmp.LockBits(new Rectangle(0, 0, _bmp.Width, _bmp.Height),
                System.Drawing.Imaging.ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        }

        /// <summary>
        /// Bitmap処理の高速化終了
        /// </summary>
        public void EndAccess()
        {
            if (_img != null)
            {
                // Bitmapに直接アクセスするためのオブジェクト開放(UnlockBits)
                _bmp.UnlockBits(_img);
                _img = null;
            }
        }

        /// <summary>
        /// BitmapのGetPixel同等
        /// </summary>
        /// <param name="x">Ｘ座標</param>
        /// <param name="y">Ｙ座標</param>
        /// <returns>Colorオブジェクト</returns>
        public Color GetPixel(int x, int y)
        {
            if (_img == null)
            {
                // Bitmap処理の高速化を開始していない場合はBitmap標準のGetPixel
                return _bmp.GetPixel(x, y);
            }

            // Bitmap処理の高速化を開始している場合はBitmapメモリへの直接アクセス
            IntPtr adr = _img.Scan0;
            int pos = x * 3 + _img.Stride * y;
            byte b = System.Runtime.InteropServices.Marshal.ReadByte(adr, pos + 0);
            byte g = System.Runtime.InteropServices.Marshal.ReadByte(adr, pos + 1);
            byte r = System.Runtime.InteropServices.Marshal.ReadByte(adr, pos + 2);
            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// BitmapのSetPixel同等
        /// </summary>
        /// <param name="x">Ｘ座標</param>
        /// <param name="y">Ｙ座標</param>
        /// <param name="col">Colorオブジェクト</param>
        public void SetPixel(int x, int y, Color col)
        {
            if (_img == null)
            {
                // Bitmap処理の高速化を開始していない場合はBitmap標準のSetPixel
                _bmp.SetPixel(x, y, col);
                return;
            }

            // Bitmap処理の高速化を開始している場合はBitmapメモリへの直接アクセス
            IntPtr adr = _img.Scan0;
            int pos = x * 3 + _img.Stride * y;
            System.Runtime.InteropServices.Marshal.WriteByte(adr, pos + 0, col.B);
            System.Runtime.InteropServices.Marshal.WriteByte(adr, pos + 1, col.G);
            System.Runtime.InteropServices.Marshal.WriteByte(adr, pos + 2, col.R);
        }
    }
}
