
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using aMaze_ingSolver.GraphUtils;

namespace aMaze_ingSolver
{
    class Maze
    {
        private Bitmap _bmp;
        private BoolMatrix _mazeMatrix;
        public Graph Graph { get; private set; }
        public TimeSpan MatrixBuildTime { get; private set; }
        Stopwatch s = new Stopwatch();

        /// <summary>
        /// Matrix of the maze.
        /// </summary>
        public BoolMatrix Matrix
        {
            get
            {
                return _mazeMatrix;
            }
        }

        public Maze(Image img, int threadCount)
        {
            _bmp = new Bitmap(img);
            s.Start();
            _mazeMatrix = new BoolMatrix(_bmp, threadCount);
            s.Stop();
            MatrixBuildTime = s.Elapsed;
            Graph = new Graph(_mazeMatrix);
        }

        public void BuildTree()
        {
            Graph.BuildAsync();
        }
    }
}
