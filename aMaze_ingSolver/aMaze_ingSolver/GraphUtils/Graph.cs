using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace aMaze_ingSolver.GraphUtils
{
    class Graph
    {
        public delegate void BuildProgress(string msg);
        public event BuildProgress OnBuildProgress;

        public delegate void BuildCompleted(TimeSpan time);
        public event BuildCompleted OnBuildCompleted;

        public List<Vertex> Vertices { get; private set; }

        public Vertex Start { get; private set; }
        public Vertex End { get; private set; }
        BoolMatrix _matrix;

        bool invoke = false;

        public Graph(BoolMatrix maze)
        {
            Vertices = new List<Vertex>();
            _matrix = maze;
        }



        public async void BuildAsync()
        {
            invoke = MazeForm.InvokeDelegates();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            await Task.Run(() => VerticalBuildScan());

            stopwatch.Stop();

            OnBuildCompleted?.Invoke(stopwatch.Elapsed);
        }

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

        //private void FindNeighbourVertices(Vertex parent)
        //{

        //    //OnBuildProgress?.Invoke(string.Format("Call: {0}", ++callCount));
        //    //_bmp.SetPixel(parent.X, parent.Y, Color.Red);

        //    if (FindVertex(Direction.Left, parent) is Vertex leftVertex)
        //    {
        //        parent.AddNeighbour(Direction.Left, leftVertex);
        //        visited.Add(leftVertex);

        //    }
        //    if (FindVertex(Direction.Right, parent) is Vertex rightVertex)
        //    {
        //        parent.AddNeighbour(Direction.Right, rightVertex);
        //        visited.Add(rightVertex);
        //    }
        //    if (FindVertex(Direction.Up, parent) is Vertex upVertex)
        //    {
        //        parent.AddNeighbour(Direction.Up, upVertex);
        //        visited.Add(upVertex);
        //    }
        //    if (FindVertex(Direction.Down, parent) is Vertex downVertex)
        //    {
        //        parent.AddNeighbour(Direction.Down, downVertex);
        //        visited.Add(downVertex);
        //    }


        //    if (parent.GetNeighbour(Direction.Left) is Vertex l)
        //        FindNeighbourVertices(l);
        //    if (parent.GetNeighbour(Direction.Right) is Vertex r)
        //        FindNeighbourVertices(r);
        //    if (parent.GetNeighbour(Direction.Up) is Vertex u)
        //        FindNeighbourVertices(u);
        //    if (parent.GetNeighbour(Direction.Down) is Vertex d)
        //        FindNeighbourVertices(d);

        //}

        //private Vertex FindVertex(Direction direction, Vertex parent)
        //{
        //    Point nextLocation = parent.Location.MoveInDirection(direction);//  MoveInDirection(parent.Location, direction);

        //    //while (!_bmp.IsWall(nextLocation))
        //    while (!_matrix.IsWall(nextLocation))
        //    {
        //        if (IsNewVertex(nextLocation, direction))
        //        {
        //            return new Vertex(nextLocation);
        //        }
        //        nextLocation = nextLocation.MoveInDirection(direction);// MoveInDirection(nextLocation, direction);
        //    }

        //    return null;
        //}

        //private bool IsNewVertex(Point location, Direction edgeDirection)
        //{
        //    if (_matrix.IsWall(location))
        //        return false;


        //    if (visited.ContainsLocation(location))
        //        return false;

        //    Point down = location.Down();
        //    bool wallDown = _matrix.IsWall(down);
        //    Point up = location.Up();
        //    bool wallUp = _matrix.IsWall(up);
        //    Point left = location.Left();
        //    bool wallLeft = _matrix.IsWall(left);
        //    Point right = location.Right();
        //    bool wallRight = _matrix.IsWall(right);

        //    switch (edgeDirection)
        //    {
        //        case Direction.Up:
        //            {
        //                if (wallUp)
        //                    return true;

        //                if (!wallLeft || !wallRight)
        //                    return true;

        //                return false;
        //            }
        //        case Direction.Down:
        //            {
        //                //Down is wall
        //                if (wallDown)
        //                    return true;

        //                if (!wallLeft || !wallRight)
        //                    return true;

        //                return false;
        //            }
        //        case Direction.Left:
        //            {
        //                if (wallLeft)
        //                    return true;

        //                if (!wallUp || !wallDown)
        //                    return true;

        //                return false;
        //            }
        //        case Direction.Right:
        //            {
        //                if (wallRight)
        //                    return true;

        //                if (!wallUp || !wallDown)
        //                    return true;

        //                return false;
        //            }
        //        default:
        //            return false;
        //    }
        //}

        //private bool IsNewVertex(int x, int y, Direction edgeDirection)
        //{
        //    return IsNewVertex(new Point(x, y), edgeDirection);
        //}

    }
}
