using aMaze_ingSolver.Algorithms;
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
        delegate void SetEmptyInfo();
        delegate void SetInfo(string msg);

        private string _mazeFile = "../maze/tiny.png";
        private Image _image;
        private Bitmap _drawBmp;
        private Maze _maze;
        private static bool _invoke = false;
        private List<IMazeSolver> _solvers;
        private IMazeSolver _selectedSolver;

        public MazeForm()
        {
            InitializeComponent();
            _solvers = new List<IMazeSolver> { new LeftTurn() };
        }

        private void MazeForm_Load(object sender, EventArgs e)
        {
            foreach (IMazeSolver solver in _solvers)
            {
                solverSelection.Items.Add(solver);
            }

            LoadImage();
        }

        public static bool InvokeDelegates()
        {
            return _invoke;
        }

        private void SetAllUnsolved()
        {
            foreach (IMazeSolver solver in _solvers)
            {
                solver.Solved = false;
            }

            btnSolve.Text = "Solve";
        }

        private void LoadImage()
        {
            SetAllUnsolved();

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
                chbShowResult.Checked = false;

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
            if (chbShowResult.Checked && _selectedSolver != null)
            {
                //foreach (Vertex vertex in _selectedSolver.GetResultVertices())
                //{
                //    _drawBmp.SetPixel(vertex.X, vertex.Y, Color.Purple);
                //}
                Queue<Vertex> _resultPath = new Queue<Vertex>(_selectedSolver.GetResultVertices());

                Vertex previous = _resultPath.Dequeue();
                if (_resultPath != null)
                {
                    while (_resultPath.Count != 0)
                    {
                        Vertex current = _resultPath.Dequeue();

                        Direction direction = Utils.GetDirection(previous.Location, current.Location);
                        while (!previous.Equals(current))
                        {

                            _drawBmp.SetPixel(previous.X, previous.Y, Color.Blue);
                            previous = new Vertex(previous.Location.MoveInDirection(direction));
                        }
                        _drawBmp.SetPixel(current.X, current.Y, Color.Blue);

                        previous = current;
                    }
                }
            }
            else
            {
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

        private void chbInvoke_CheckedChanged(object sender, EventArgs e)
        {
            _invoke = (sender as CheckBox).Checked;
        }

        private void SolverSelected(object sender, EventArgs e)
        {
            if ((sender as CheckedListBox).SelectedItem is IMazeSolver solver)
            {
                _selectedSolver = solver;
                btnSolve.Text = "Solve";
            }
            else
            {
                _selectedSolver = null;
                //throw new ArgumentException("Wrong solvert in checked list box.");
            }
        }

        private void SolveUsingSolver(object sender, EventArgs e)
        {
            if (_selectedSolver != null && !_selectedSolver.Solved)
            {
                _selectedSolver.OnSolved += MazeSolved;
                _selectedSolver.SolveMaze(_maze.Graph);
            }
        }

        private void MazeSolved()
        {
            if (_selectedSolver != null)
            {
                btnSolve.Text = "Solved";
                _selectedSolver.Solved = true;

                if (lbSolveTime.InvokeRequired)
                {
                    SetEmptyInfo setInfo = new SetEmptyInfo(MazeSolved);
                    this.Invoke(setInfo);
                }
                else
                {
                    //lbSolveTime.Text = string.Format("Completed after: {0} ms", time.ToString("mm':'ss':'fff"));
                    lbSolveTime.Text = "Solve time: " + _selectedSolver.GetSolveTime().ToString("mm':'ss':'fff");
                }
            }
        }

        private void ShowResult(object sender, EventArgs e)
        {
            DrawImage();
        }
    }
}
