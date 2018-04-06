using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace aMaze_ingSolver
{
    struct ThreadParam
    {
        public int fromRow;
        public int toRow;
        public Bitmap bmp;

        public ThreadParam(int from, int to, Bitmap bmp)
        {
            fromRow = from;
            toRow = to;
            this.bmp = bmp;
        }
    }

    class BoolMatrix
    {

        public int Rows { get; private set; }
        public int Cols { get; private set; }

        public bool[,] Data { get; private set; }
        object _lock = new object();
        public BoolMatrix(Bitmap bmp)
        {
            Rows = bmp.Height;
            Cols = bmp.Width;

            Data = new bool[bmp.Height, bmp.Width];

            using (BitmapPlus bmpP = new BitmapPlus(bmp))
            {
                for (int row = 0; row < bmp.Height; ++row)
                {
                    for (int col = 0; col < bmp.Width; ++col)
                    {
                        Data[row, col] = bmpP.GetPixel(col, row).ToBool();
                    }
                }
            }
        }
    }
}
