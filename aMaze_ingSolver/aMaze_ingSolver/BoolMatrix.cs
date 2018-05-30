using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace aMaze_ingSolver
{
    /// <summary>
    /// Parameter of the thread work to load image into bool matrix.
    /// </summary>
    struct Param
    {
        public Bitmap bmp;
        public int rowFrom;
        public int rowTo;

        public Param(Bitmap bmp, int rowFrom, int rowTo)
        {
            this.bmp = bmp;
            this.rowFrom = rowFrom;
            this.rowTo = rowTo;
        }
    }

    class BoolMatrix
    {
        private TimeSpan _loadTime;
        object _lock = new object();

        /// <summary>
        /// Number of rows.
        /// </summary>
        public int Rows { get; private set; }
        /// <summary>
        /// Number of columns.
        /// </summary>
        public int Cols { get; private set; }

        /// <summary>
        /// Bool matrix data.
        /// </summary>
        public bool[,] Data { get; private set; }

        public BoolMatrix(Bitmap bmp, int threadCount)
        {
            Rows = bmp.Height;
            Cols = bmp.Width;

            Data = new bool[bmp.Height, bmp.Width];

            System.Diagnostics.Stopwatch loadTime = new System.Diagnostics.Stopwatch();
            loadTime.Start();
            if (threadCount > 1)
            {
                LoadParallel(bmp, threadCount);
            }
            else
            {
                using (BitmapPlus bmpP = new BitmapPlus(bmp, System.Drawing.Imaging.ImageLockMode.ReadOnly))
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

            loadTime.Stop();
            _loadTime = loadTime.Elapsed;
        }

        /// <summary>
        /// Get time needed to load this image into bitmap.
        /// </summary>
        /// <returns>Timespan of load.</returns>
        public TimeSpan GetLoadTime()
        {
            return _loadTime;
        }
        
        /// <summary>
        /// Load image into bitmap in parallel.
        /// </summary>
        /// <param name="bmp">Bitmap from which to load.</param>
        /// <param name="threadCount">Thread count.</param>
        private void LoadParallel(Bitmap bmp, int threadCount)
        {
            Task[] tasks = new Task[threadCount];
            int threadRowSize = bmp.Height / threadCount;


            for (int i = 0; i < threadCount; i++)
            {
                int from = i * threadRowSize;
                int to = (i == threadCount - 1) ? bmp.Height : (i * threadRowSize) + threadRowSize;

                Param param = new Param(new Bitmap(bmp), from, to);
                tasks[i] = Task.Factory.StartNew(() => Work(param));
            }
            Task.WaitAll(tasks);

        }

        /// <summary>
        /// Thread work.
        /// </summary>
        /// <param name="param">Thread parameter.</param>
        private void Work(Param param)
        {
            using (BitmapPlus bmpP = new BitmapPlus(param.bmp, System.Drawing.Imaging.ImageLockMode.ReadOnly))
            {
                for (int row = param.rowFrom; row < param.rowTo; ++row)
                {
                    for (int col = 0; col < param.bmp.Width; ++col)
                    {
                        Data[row, col] = bmpP.GetPixel(col, row).ToBool();
                    }
                }
            }
        }
    }
}
