using aMaze_ingSolver.Tree;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aMaze_ingSolver
{
    class Maze
    {
        private Bitmap _bmp;
        private Point _start;
        private Point _finish;

        private BoolMatrix _mazeMatrix;

        public Tree.Tree Tree { get; private set; }

        public Maze(Image img)
        {
            _bmp = new Bitmap(img);

            _mazeMatrix = new BoolMatrix(_bmp);

            FindStart();
            FindFinish();

            Tree = new Tree.Tree(new Vertex(_start, null), _mazeMatrix);
            Tree.BuildTree();
        }

        private void FindStart()
        {
            for (int x = 0; x < _bmp.Width; x++)
            {
                if (!_mazeMatrix.IsWall(x, 0))
                {
                    _start = new Point(x, 0);
                    break;
                }
            }
        }

        private void FindFinish()
        {
            for (int x = 0; x < _mazeMatrix.Cols; x++)
            {
                if (!_mazeMatrix.IsWall(x, _mazeMatrix.Rows - 1))
                {
                    _finish = new Point(x, _mazeMatrix.Rows - 1);
                    break;
                }
            }
        }










    }
}
