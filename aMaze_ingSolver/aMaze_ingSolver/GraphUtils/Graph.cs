using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace aMaze_ingSolver.GraphUtils
{
    [Serializable]
    class Graph
    {
        #region Events
        public delegate void BuildProgress(string msg);
        public delegate void BuildCompleted(TimeSpan time);
        public event BuildProgress OnBuildProgress;
        public event BuildCompleted OnBuildCompleted;


        #endregion

        private BoolMatrix _matrix;
        private bool invoke = false;

        public List<Vertex> Vertices { get; private set; }
        /// <summary>
        /// Entry point of maze.
        /// </summary>
        public Vertex Start { get; private set; }
        /// <summary>
        /// Exit point of maze.
        /// </summary>
        public Vertex End { get; private set; }



        public Graph(BoolMatrix maze)
        {
            Vertices = new List<Vertex>();
            _matrix = maze;
        }

        /// <summary>
        /// Build the graph from maze asynchronously.
        /// </summary>
        public async void BuildAsync()
        {
            invoke = MazeForm.InvokeDelegates();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            await Task.Run(() => VerticalBuildScan());

            stopwatch.Stop();

            OnBuildCompleted?.Invoke(stopwatch.Elapsed);
        }

        /// <summary>
        /// Convert bool matrix to Graph.
        /// </summary>
        private void VerticalBuildScan()
        {
            long vertexCount = 0;

            Vertex[] upperRowVertices = new Vertex[_matrix.Cols];

            //Find start
            for (int col = 1; col < _matrix.Cols - 1; ++col)
            {
                if (_matrix.IsPath(0, col))
                {
                    Start = new Vertex(col, 0);
                    //upperRowVertices.Add(Start);
                    upperRowVertices[col] = Start;
                    Vertices.Add(Start);
                    break;
                }
            }



            //Scan maze
            for (int row = 1; row < _matrix.Rows - 1; ++row)
            {
                float percent = ((float)row / (float)(_matrix.Rows - 1)) * 100.0f;

                if (invoke)
                {
                    OnBuildProgress?.Invoke(string.Format("Processing... {0}% completed.", percent));
                }


                Vertex leftVertex = null;

                bool prevIsPath = false;
                bool currentIsPath = false;
                bool nextIsPath = _matrix.IsPath(row, 1);

                for (int col = 1; col < _matrix.Cols - 1; ++col)
                {

                    prevIsPath = currentIsPath;
                    currentIsPath = nextIsPath;
                    nextIsPath = _matrix.IsPath(row, col + 1);

                    Vertex foundVertex = null;
                    if (!currentIsPath)
                        continue;

                    if (prevIsPath)
                    {
                        if (nextIsPath)
                        {
                            // --**--
                            if (_matrix.IsPath(row - 1, col) || _matrix.IsPath(row + 1, col))
                            {
                                foundVertex = new Vertex(col, row);
                                Vertices.Add(foundVertex);
                                ++vertexCount;
                                if (leftVertex != null)
                                {
                                    foundVertex.AddNeighbour(Direction.Left, leftVertex);
                                    leftVertex.AddNeighbour(Direction.Right, foundVertex);
                                }
                                leftVertex = foundVertex;
                            }
                        }
                        else
                        {
                            // --**||
                            foundVertex = new Vertex(col, row);
                            Vertices.Add(foundVertex);
                            ++vertexCount;
                            if (leftVertex != null)
                            {
                                foundVertex.AddNeighbour(Direction.Left, leftVertex);
                                leftVertex.AddNeighbour(Direction.Right, foundVertex);
                            }
                            leftVertex = null;
                        }
                    }
                    else //Previous is wall
                    {
                        // ||**
                        if (nextIsPath)
                        {
                            // ||**--
                            foundVertex = new Vertex(col, row);
                            Vertices.Add(foundVertex);
                            ++vertexCount;
                            leftVertex = foundVertex;
                        }
                        else
                        {
                            // ||**||

                            if (_matrix.IsWall(col, row - 1) || _matrix.IsWall(col, row + 1))
                            {
                                foundVertex = new Vertex(col, row);
                                Vertices.Add(foundVertex);
                                ++vertexCount;
                            }
                        }
                    }

                    if (foundVertex != null)
                    {
                        if (_matrix.IsPath(row - 1, col))
                        {
                            Vertex aboveVertex = upperRowVertices[col];
                            aboveVertex.AddNeighbour(Direction.Down, foundVertex);
                            foundVertex.AddNeighbour(Direction.Up, aboveVertex);
                        }

                        if (_matrix.IsPath(row + 1, col))
                        {
                            upperRowVertices[col] = foundVertex;
                        }
                        else
                        {
                            upperRowVertices[col] = null;
                        }
                    }
                }
            }

            for (int col = 1; col < _matrix.Cols - 1; col++)
            {
                if (_matrix.IsPath(_matrix.Rows - 1, col))
                {
                    End = new Vertex(col, _matrix.Rows - 1);
                    Vertex upperVertex = upperRowVertices[col];
                    upperVertex.AddNeighbour(Direction.Down, End);
                    End.AddNeighbour(Direction.Up, upperVertex);
                    Vertices.Add(End);
                }
            }
        }

        /// <summary>
        /// Set each vertex unvisited, distances to infinite and previous vertex to null.
        /// </summary>
        public void Reset()
        {
            Parallel.ForEach(Vertices, (vertex) =>
            {
                vertex.Reset();
            });

            //foreach (Vertex vertex in Vertices)
            //{

            //}
        }

        /// <summary>
        /// Calculate distance from vertex to end of the maze for all vertices.
        /// </summary>
        internal void CalculateDistancesFromVerticesToEnd(int threadCount)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            if (threadCount <= 1)
            {
                foreach (Vertex v in this.Vertices)
                {
                    v.EuclideanDistanceToEnd = v.EuclideanDistanceTo(this.End);
                }
            }
            else
            {
                Parallel.ForEach(this.Vertices, (vertex) =>
                {
                    vertex.EuclideanDistanceToEnd = vertex.EuclideanDistanceTo(this.End);
                });
            }
            stopwatch.Stop();
        }

        public void SaveAsImage(Size originalSize, string fileName)
        {
            float scale = 1.0f;
            Color red = Color.Red;
            Color blue = Color.LightBlue;

            using (Bitmap bitmap = new Bitmap((int)(originalSize.Width * scale), (int)(originalSize.Height * scale)))
            {
                using (BitmapPlus bmp = new BitmapPlus(bitmap, System.Drawing.Imaging.ImageLockMode.ReadWrite))
                {
                    foreach (Vertex vertex in this.Vertices)
                    {
                        bmp.SetPixel(vertex.Location, red);
                        foreach (OrientedEdge edge in vertex.Edges)
                        {
                            foreach (Point p in edge.GetEdgePoints())
                            {
                                bmp.SetPixel(p, blue);
                            }
                        }
                    }
                }
                bitmap.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        /// <summary>
        /// Clone this graph into another object.
        /// </summary>
        /// <returns>Same graph.</returns>
        public Graph Clone()
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memStream, this);
                memStream.Position = 0;
                Graph clone = (Graph)binaryFormatter.Deserialize(memStream);
                return clone;
            }
        }
    }
}
