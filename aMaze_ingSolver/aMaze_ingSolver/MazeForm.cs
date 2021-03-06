﻿using aMaze_ingSolver.Algorithms;
using aMaze_ingSolver.GraphUtils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aMaze_ingSolver
{
    public partial class MazeForm : Form
    {
        delegate void SetEmptyInfo();
        delegate void SetInfo(string msg);
        delegate void SetPercentInfo(float percent);

        private string _mazeFile = "../maze/tiny.png";
        private Image _image;
        private Bitmap _drawBmp;
        private Bitmap _animationBmp;
        private Maze _maze;
        private static bool _invoke = false;
        private List<IMazeSolver> _solvers;
        private IMazeSolver _selectedSolver;
        private ISteppableSolver _steppableSolver;

        private float _scale = 1.0f;
        private const float _scaleMin = 0.01f;
        private const float _scaleMax = 20.0f;
        private const float _smallStepFrom = 1.0f;
        private int _threadCount = 4;

        private bool _runningAnimation = false;

        public MazeForm()
        {
            InitializeComponent();
            _solvers = new List<IMazeSolver>
            {
                new LeftTurn(),
                new BreadthFirst(),
                new DepthFirst(),
                new Dijkstra(),
                new AStar(),
                //new ShortestPathACO()
            };
            tbThreadCount.Text = "4";
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
            _scale = 1.0f;
            lbMatrixInfo.Text = "Loading image...";
            _image = Image.FromFile(_mazeFile);
            lbMatrixInfo.Text = "Creating bitmap...";
            _drawBmp = new Bitmap(_image);
            lbMatrixInfo.Text = "Converting to matrix...";

            _maze = new Maze(_image, _threadCount);

            lbMatrixInfo.Text = string.Format("Matrix build time: {0} ms.", _maze.MatrixBuildTime.TotalMilliseconds.ToString("### ### ###"));

            _maze.Graph.OnBuildProgress += Tree_OnBuildProgress;
            _maze.Graph.OnBuildCompleted += Graph_OnBuildCompleted;

            _maze.BuildTree();

            UpdateScale();
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
                lbInfo.Text = string.Format("Graph completed after: {0} ms", time.TotalMilliseconds.ToString("### ### ###"));
                lbPathSize.Text = string.Format("{0} vertices", _maze.Graph.Vertices.Count);
            }
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


        private void InvalidateMaze()
        {
            ClearImage();
            using (BitmapPlus bmpPlus = new BitmapPlus(_drawBmp, System.Drawing.Imaging.ImageLockMode.WriteOnly))
            {
                if (chbShowResult.Checked && _selectedSolver != null)
                {
                    DrawResultPath(bmpPlus);
                }
                else
                {

                    if (chbShowVertices.Checked)
                    {
                        foreach (Vertex vertex in _maze.Graph.Vertices)
                        {
                            bmpPlus.SetPixel(vertex.X, vertex.Y, Color.Red);
                        }
                    }

                    if (chbShowStartEnd.Checked)
                    {
                        bmpPlus.SetPixel(_maze.Graph.Start.X, _maze.Graph.Start.Y, Color.Blue);
                        bmpPlus.SetPixel(_maze.Graph.End.X, _maze.Graph.End.Y, Color.Blue);
                    }
                }
            }

            if (_scale >= 1.0f)
                imgBox.Image = _drawBmp.ResizeImage((int)((float)_drawBmp.Width * _scale), (int)((float)_drawBmp.Height * _scale));
            else
                imgBox.Image = _drawBmp.ResizeImage(
                    (int)((float)_drawBmp.Width * _scale),
                    (int)((float)_drawBmp.Height * _scale),
                    System.Drawing.Drawing2D.InterpolationMode.Bicubic);
        }



        private void DrawResultPath(BitmapPlus bmpPlus)
        {
            Color color = _selectedSolver.MazeColor;
            Queue<Vertex> resultPath = new Queue<Vertex>(_selectedSolver.GetResultVertices());

            if (resultPath.Count <= 0)
            {
                MessageBox.Show("Empty queue");
                return;
            }

            Vertex current = null;
            if (false) //chbParallel.Checked || true
            {
                while (resultPath.Count != 0)
                {
                    current = resultPath.Dequeue();
                    bmpPlus.SetPixel(current.X, current.Y, color);
                }
            }
            else
            {

                List<Vertex> pathVertices = new List<Vertex>(_selectedSolver.GetResultVertices());
                //pre-sort
                pathVertices = pathVertices
                    .OrderBy(v => v.Location.Y)
                    .GroupBy(v => v.Location.Y)
                    .SelectMany(g => g.OrderBy(v => v.X))
                    .ToList();

                _maze.Graph.Reset();
                bool currentSmaller = true;
                
                foreach (Vertex vertex in pathVertices)
                {
                    //check on same row
                    IEnumerable<Vertex> onRow =
                        pathVertices.Where(v => (v.Y == vertex.Y) && !v.Visited && v.Location != vertex.Location);
                    foreach (Vertex vertexOnRow in onRow)
                    {
                        currentSmaller = (vertex.X < vertexOnRow.X);
                        int from = (currentSmaller) ? vertex.X : vertexOnRow.X;
                        int to = (currentSmaller) ? vertexOnRow.X : vertex.X;
                        if (_maze.Matrix.FreeXPath(vertex.Y, from, to))
                        {
                            OrientedEdge virtualEdge = (currentSmaller)
                                ? vertex.CreateVirtualEdgeTo(vertexOnRow) : vertexOnRow.CreateVirtualEdgeTo(vertex);
                            foreach (Point p in virtualEdge.GetEdgePoints(true))
                            {
                                bmpPlus.SetPixel(p, color);
                            }
                        }
                    }

                    vertex.Visited = true;
                }

                _maze.Graph.Reset();
                foreach (Vertex vertex in pathVertices)
                {
                    //connect vertical
                    IEnumerable<Vertex> onCol =
                        pathVertices.Where(v => (v.X == vertex.X) && !v.Visited && v.Location != vertex.Location);

                    foreach (Vertex vertexOnCol in onCol)
                    {
                        currentSmaller = vertex.Y < vertexOnCol.Y;
                        int from = (currentSmaller) ? vertex.Y : vertexOnCol.Y;
                        int to = (currentSmaller) ? vertexOnCol.Y : vertex.Y;
                        if (_maze.Matrix.FreeYPath(vertex.X, from, to))
                        {
                            OrientedEdge virtualEdge = (currentSmaller)
                                  ? vertex.CreateVirtualEdgeTo(vertexOnCol) : vertexOnCol.CreateVirtualEdgeTo(vertex);

                            foreach (Point p in virtualEdge.GetEdgePoints(true))
                            {
                                bmpPlus.SetPixel(p, color);
                            }
                        }
                    }

                    vertex.Visited = true;
                }
            }
        }

        private void ClearImage()
        {
            _drawBmp = new Bitmap(_image);
        }

        private void ChbShowVertices_CheckedChanged(object sender, EventArgs e)
        {
            InvalidateMaze();
        }

        private void ScaleFactor_ValueChanged(object sender, EventArgs e)
        {
            InvalidateMaze();
        }

        private void scaleUp_Click(object sender, EventArgs e)
        {
            float step = _scale <= _smallStepFrom ? 0.1f : 1.0f;

            if (_scale < _scaleMax)
                _scale += step;

            UpdateScale();
        }

        private void UpdateScale()
        {
            lbScale.Text = _scale.ToString("0.00");
            InvalidateMaze();
        }

        private void scaleDown_Click(object sender, EventArgs e)
        {
            float step = _scale <= _smallStepFrom ? 0.1f : 1.0f;

            if (_scale > _scaleMin)
                _scale -= step;

            UpdateScale();

        }

        private void ResetClick(object sender, EventArgs e)
        {
            chbShowResult.Checked = false;
            chbShowResult.Enabled = false;

            if (_selectedSolver != null)
                _selectedSolver.Reset();
            if (_steppableSolver != null)
            {
                _steppableSolver.ResetAnimation();
                if (_animationBmp != null)
                {
                    _animationBmp.Dispose();
                    _animationBmp = new Bitmap(_image);

                    if (_scale >= 1.0f)
                        imgBox.Image = _animationBmp.ResizeImage((int)((float)_drawBmp.Width * _scale), (int)((float)_drawBmp.Height * _scale));
                    else
                        imgBox.Image = _animationBmp.ResizeImage(
                            (int)((float)_drawBmp.Width * _scale),
                            (int)((float)_drawBmp.Height * _scale),
                            System.Drawing.Drawing2D.InterpolationMode.Bicubic);
                }
            }

            btnSolve.Text = "Solve";
            InvalidateMaze();
        }

        private void chbInvoke_CheckedChanged(object sender, EventArgs e)
        {
            _invoke = (sender as CheckBox).Checked;
        }

        private void SolveUsingSolver(object sender, EventArgs e)
        {
            if (_selectedSolver != null && !_selectedSolver.Solved)
            {
                _selectedSolver.Parallel = chbParallel.Checked;
                if (_selectedSolver.Parallel)
                {
                    _selectedSolver.ThreadCount = _threadCount;
                }
                else
                {
                    _selectedSolver.ThreadCount = 1;
                }
                _selectedSolver.OnSolved += MazeSolved;
                _selectedSolver.OnSolveProgress += SolveProgression;

                _maze.Graph.Reset();
                Task.Factory.StartNew(() => _selectedSolver.SolveMaze(_maze.Graph));
            }
        }

        private void SolveProgression(float percentDone)
        {
            if (pbSolve.InvokeRequired)
            {
                SetPercentInfo setInfo = new SetPercentInfo(SolveProgression);
                this.Invoke(setInfo, percentDone);
            }
            else
            {
                pbSolve.Value = (int)percentDone;
            }
        }

        private void MazeSolved()
        {
            if (_selectedSolver != null)
            {


                if (lbSolveTime.InvokeRequired)
                {
                    SetEmptyInfo setInfo = new SetEmptyInfo(MazeSolved);
                    this.Invoke(setInfo);
                }
                else
                {
                    chbShowResult.Enabled = true;
                    btnSolve.Text = "Solved";
                    _selectedSolver.Solved = true;

                    pbSolve.Value = pbSolve.Maximum;

                    ShowSolveTime(_selectedSolver.GetSolveTime());
                    //ShowPathLength(_selectedSolver.GetPathLength());
                    LogSolveTime(_selectedSolver);
                }
            }
        }

        private void ShowSolveTime(TimeSpan ts)
        {
            lbSolveTime.Text = string.Format("Path found after: {0} ms",
                                    ts.TotalMilliseconds.ToString("### ### ###.000"));
        }

        private void LogSolveTime(IMazeSolver selectedSolver)
        {
            using (StreamWriter writer = new StreamWriter("SolverLog.txt", true, System.Text.Encoding.UTF8))
            {
                writer.WriteLine("{0};{1};{2}[ms];{3}_threads",
                    new FileInfo(_mazeFile).Name, selectedSolver.Name, selectedSolver.GetSolveTime().TotalMilliseconds,
                    chbParallel.Checked ? _threadCount : 1);
            }
        }

        private void ShowPathLength(int length)
        {
            lbPathSize.Text = string.Format("Path length: {0}", length.ToString("### ### ###"));
        }

        private void ShowResult(object sender, EventArgs e)
        {
            InvalidateMaze();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (_selectedSolver != null && _selectedSolver.Solved)
            {
                bool pathOnly = false;
                using (ExportTypeForm exportType = new ExportTypeForm())
                {
                    if (exportType.ShowDialog() == DialogResult.OK)
                    {
                        pathOnly = exportType.rbPathOnly.Checked;
                    }
                    else
                    {
                        return;
                    }
                }
                if (!pathOnly)
                {
                    if (!chbShowResult.Checked)
                    {
                        using (BitmapPlus bmpp = new BitmapPlus(_drawBmp, System.Drawing.Imaging.ImageLockMode.WriteOnly))
                        {
                            DrawResultPath(bmpp);
                        }
                    }

                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        AddExtension = true,
                        DefaultExt = ".png",
                        FileName = "maze.png",
                        Title = "Save solved maze as image."
                    };

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        _drawBmp.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
                else
                {
                    using (Bitmap bitmap = new Bitmap(_drawBmp.Width, _drawBmp.Height))
                    {
                        using (BitmapPlus bmp = new BitmapPlus(bitmap, System.Drawing.Imaging.ImageLockMode.ReadWrite))
                        {
                            DrawResultPath(bmp);
                        }

                        SaveFileDialog saveFileDialog = new SaveFileDialog
                        {
                            AddExtension = true,
                            DefaultExt = ".png",
                            FileName = "path.png",
                            Title = "Save solved maze as image."
                        };

                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            bitmap.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                        }
                    }
                }


            }
            else
            {
                MessageBox.Show("Not solved.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnSaveGraph(object sender, EventArgs e)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                AddExtension = true,
                DefaultExt = ".png",
                FileName = "graph.png",
                Title = "Save graph as image."
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                _maze.Graph.SaveAsImage(_image.Size, saveFileDialog.FileName);
            }
        }

        private void solverSelection_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue != CheckState.Checked)
                return;

            (sender as CheckedListBox).ItemCheck -= solverSelection_ItemCheck;

            for (int i = 0; i < (sender as CheckedListBox).Items.Count; i++)
            {
                (sender as CheckedListBox).SetItemChecked(i, i == e.Index);
            }

            if ((sender as CheckedListBox).SelectedItem is IMazeSolver solver)
            {
                SelectSolver(solver);
            }
            else
            {
                _selectedSolver = null;
                //throw new ArgumentException("Wrong solvert in checked list box.");
            }
            (sender as CheckedListBox).ItemCheck += solverSelection_ItemCheck;
        }

        private void SelectSolver(IMazeSolver solver)
        {
            _selectedSolver = solver;

            if (solver is ISteppableSolver steppableSolver)
            {
                miPlay.Enabled = true;
                miPause.Enabled = true;
                miStep.Enabled = true;
                _steppableSolver = steppableSolver;

                InitializeAnimation();
                _steppableSolver.InitializeAnimation(_maze.Graph);
            }
            else
            {
                miPlay.Enabled = false;
                miPause.Enabled = false;
                miStep.Enabled = false;
                _steppableSolver = null;
            }

            if (!solver.SupportParallel)
                chbParallel.Checked = false;
            chbParallel.Enabled = solver.SupportParallel;
            tbThreadCount.Enabled = solver.SupportThreadCount;

            if (!solver.Solved)
                chbShowResult.Checked = false;

            InvalidateMaze();
            ShowPathLength(_selectedSolver.GetPathLength());
            ShowSolveTime(_selectedSolver.GetSolveTime());

            btnSolve.Text = _selectedSolver.Solved ? "Solved" : "Solve";

        }

        private void chbParallel_Click(object sender, EventArgs e)
        {
            bool check = (sender as CheckBox).Checked;
            gbThreadCount.Enabled = check;
            if (!check)
            {
                tbThreadCount.Text = "1";
                _threadCount = 1;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(tbThreadCount.Text))
                    tbThreadCount.Text = "1";
            }
        }

        private void tbThreadCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void UpdateThreadCount(object sender, EventArgs e)
        {
            string txt = (sender as TextBox).Text;
            if (string.IsNullOrWhiteSpace(txt))
            {
                _threadCount = 1;
                return;
            }

            if (int.TryParse(txt, out int tc))
            {
                _threadCount = tc;
            }
            else
            {
                throw new Exception("Wrong thread count");
            }
        }

        private void SelectionChanged(object sender, EventArgs e)
        {
            if (solverSelection.CheckedItems.Count == 0)
            {
                chbShowResult.Checked = false;
                InvalidateMaze();
            }
        }

        private void miPlay_Click(object sender, EventArgs e)
        {
            _steppableSolver.Delay = 500;
            _steppableSolver.OnAnimationProgress += RenderAnimation;
            _steppableSolver.RunAnimated();
        }

        private void InitializeAnimation()
        {
            _animationBmp = new Bitmap(_image);
        }

        private void RenderAnimation(Vertex currentAnimationVertex)
        {
            _animationBmp.SetPixel(currentAnimationVertex.X, currentAnimationVertex.Y, Color.Red);

            if (_scale >= 1.0f)
                imgBox.Image = _animationBmp.ResizeImage((int)((float)_drawBmp.Width * _scale), (int)((float)_drawBmp.Height * _scale));
            else
                imgBox.Image = _animationBmp.ResizeImage(
                    (int)((float)_drawBmp.Width * _scale),
                    (int)((float)_drawBmp.Height * _scale),
                    System.Drawing.Drawing2D.InterpolationMode.Bicubic);
        }

        private void miPause_Click(object sender, EventArgs e)
        {
            _runningAnimation = false;
            _steppableSolver.PauseAnimation();
        }

        private void miStep_Click(object sender, EventArgs e)
        {
            Vertex vertex = _steppableSolver.PerformStep();
            RenderAnimation(vertex);
        }
    }
}
