
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

        public Maze(Image img)
        {
            _bmp = new Bitmap(img);

            Stopwatch s = new Stopwatch();
            s.Start();
            _mazeMatrix = new BoolMatrix(_bmp);
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
