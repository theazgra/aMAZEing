using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aMaze_ingSolver
{
    class BoolMatrix
    {
        public int Rows { get; private set; }
        public int Cols { get; private set; }

        public bool[,] Data { get; private set; }

        public BoolMatrix(Bitmap bmp)
        {
            Rows = bmp.Height;
            Cols = bmp.Width;

            Data = new bool[bmp.Height, bmp.Width];
            for (int row = 0; row < bmp.Height; ++row)
            {
                for (int col = 0; col < bmp.Width; ++col)
                {
                    Data[row, col] = bmp.GetPixel(col, row).ToBool();
                }
            }
        }
    }
}
