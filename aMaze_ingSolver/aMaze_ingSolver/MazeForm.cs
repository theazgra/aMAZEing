using aMaze_ingSolver.GraphUtils;
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
        delegate void SetInfo(string msg);

        private string _mazeFile = "../maze/tiny.png";
        private Image _image;
        private Bitmap _drawBmp;
        private Maze _maze;
        private static bool _invoke = false;

        public MazeForm()
        {
            InitializeComponent();
        }

        public static bool InvokeDelegates()
        {
            return _invoke;
        }

        private void LoadImage()
        {
            lbMatrixInfo.Text = string.Empty;
            scaleFactor.Value = 1;
            lbMatrixInfo.Text = "Loading image...";
            _image = Image.FromFile(_mazeFile);
            lbMatrixInfo.Text = "Creating bitmap...";
            _drawBmp = new Bitmap(_image);
            lbMatrixInfo.Text = "Converting to matrix...";

            _maze = new Maze(_image);

            lbMatrixInfo.Text = string.Format("Matrix build time: {0} ms.", _maze.MatrixBuildTime.ToString("mm':'ss':'fff"));
            
            _maze.Graph.OnBuildProgress += Tree_OnBuildProgress;
            _maze.Graph.OnBuildCompleted += Graph_OnBuildCompleted;

            _maze.BuildTree();
            DrawImage();
        }

        private void Graph_OnBuildCompleted(TimeSpan time)
        {
            if (lbInfo.InvokeRequired)
            {
                SetInfo setInfo = new SetInfo(Tree_OnBuildProgress);
                this.Invoke(setInfo, new object[] { string.Format("Completed after: {0} ms", time.ToString("mm':'ss':'fff")) });
            }
            else
            {
                lbInfo.Text = string.Format("Completed after: {0} ms", time.ToString("mm':'ss':'fff"));

                Task.Run(() => PaintAndSave());

            }
        }

        private void PaintAndSave()
        {
            Bitmap bmp = new Bitmap(_image);
            foreach (Vertex vertex in _maze.Graph.Vertices)
            {
                bmp.SetPixel(vertex.X, vertex.Y, Color.Red);
            }

            bmp.SetPixel(_maze.Graph.Start.X, _maze.Graph.Start.Y, Color.Blue);
            bmp.SetPixel(_maze.Graph.End.X, _maze.Graph.End.Y, Color.Blue);

            bmp.Save("Result.png", System.Drawing.Imaging.ImageFormat.Png);
        }

        private void Tree_OnBuildProgress(string msg)
        {
            if (lbInfo.InvokeRequired)
            {
                SetInfo setInfo = new SetInfo(Tree_OnBuildProgress);
                this.Invoke(setInfo, new object[] { msg });
            }
            else
            {
                lbInfo.Text = msg;

            }
        }

        private void miLoadMaze_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "Images|*.png;*.bmp;*.jpg;*.jpeg"
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                chbShowStartEnd.Checked = false;
                chbShowVertices.Checked = false;
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
                foreach (Vertex vertex in _maze.Graph.Vertices)
                {
                    _drawBmp.SetPixel(vertex.X, vertex.Y, Color.Red);
                }
            }

            if (chbShowStartEnd.Checked)
            {
                _drawBmp.SetPixel(_maze.Graph.Start.X, _maze.Graph.Start.Y, Color.Blue);
                _drawBmp.SetPixel(_maze.Graph.End.X, _maze.Graph.End.Y, Color.Blue);
            }

            imgBox.Image = _drawBmp.ResizeImage(_drawBmp.Width * scaleFactor.Value, _drawBmp.Height * scaleFactor.Value);
        }

        private void ClearImage()
        {
            _drawBmp = new Bitmap(_image);
        }

        private void ChbShowVertices_CheckedChanged(object sender, EventArgs e)
        {
            DrawImage();
        }

        private void ScaleFactor_ValueChanged(object sender, EventArgs e)
        {
            DrawImage();
        }

        private void scaleUp_Click(object sender, EventArgs e)
        {
            if (scaleFactor.Value < scaleFactor.Maximum)
                scaleFactor.Value += 1;
        }

        private void scaleDown_Click(object sender, EventArgs e)
        {
            if (scaleFactor.Value > scaleFactor.Minimum)
                scaleFactor.Value -= 1;
        }

        private void MazeForm_Load(object sender, EventArgs e)
        {
            LoadImage();
        }

        private void chbInvoke_CheckedChanged(object sender, EventArgs e)
        {
            _invoke = (sender as CheckBox).Checked;
        }
    }
}
