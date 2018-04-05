using aMaze_ingSolver.Tree;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aMaze_ingSolver
{
    public partial class MazeForm : Form
    {
        private string _mazeFile = "../maze/tiny.png";
        private Image _image;
        private Bitmap _drawBmp;
        private Maze _maze;
        private bool _dirty = false;
        public MazeForm()
        {
            InitializeComponent();

            LoadImage();
            
        }

        private void LoadImage()
        {
            scaleFactor.Value = 1;
            _image = Image.FromFile(_mazeFile);
            _drawBmp = new Bitmap(_image);
            _maze = new Maze(_image);

            _maze.OnTreeBuildFinished += _maze_OnTreeBuildFinished;
            _maze.BuildTree();
            DrawImage();
        }

        private void _maze_OnTreeBuildFinished(TimeSpan timeSpan)
        {
            infoText.Text = string.Format("Tree built time: {0} ms", timeSpan.Milliseconds);
        }

        private void miLoadMaze_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "Images|*.png;*.bmp;*.jpg;*.jpeg"
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(ofd.FileName))
                {
                    _mazeFile = ofd.FileName;
                    LoadImage();
                }
            }
        }


        private void DrawImage()
        {
            ClearImage();
            if (chbShowVertices.Checked)
            {
                _dirty = true;
                foreach (Vertex vertex in _maze.Tree.GetVertices())
                {
                    _drawBmp.SetPixel(vertex.X, vertex.Y, Color.Red);
                }
            }

            imgBox.Image = _drawBmp.ResizeImage(_drawBmp.Width * scaleFactor.Value, _drawBmp.Height * scaleFactor.Value);
        }

        private void ClearImage()
        {
            _drawBmp = new Bitmap(_image);
            //imgBox.Image = _image;
            _dirty = false;
        }

        private void ChbShowVertices_CheckedChanged(object sender, EventArgs e)
        {
            DrawImage();
        }

        private void ScaleFactor_ValueChanged(object sender, EventArgs e)
        {
            DrawImage();
        }
    }
}
