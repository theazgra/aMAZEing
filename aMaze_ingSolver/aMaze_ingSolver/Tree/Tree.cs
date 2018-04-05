using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aMaze_ingSolver.Tree
{
    class Tree
    {
        public Vertex Root { get; private set; }
        List<Vertex> visited;
        BoolMatrix _matrix;

        public Tree(Vertex root, Bitmap bmp)
        {
            
            visited = new List<Vertex>();
            Root = root;
            
        }

        public Tree(Vertex root, BoolMatrix maze)
        {
            
            visited = new List<Vertex>();
            Root = root;
            _matrix = maze;
        }

        public void BuildTree()
        {
            visited.AddIfNotIn(Root);

            FindNeighbourVertices(Root);

            //_bmp.Save("Vertex.png", System.Drawing.Imaging.ImageFormat.Png);
            

        }

        private void FindNeighbourVertices(Vertex parent)
        {
            //_bmp.SetPixel(parent.X, parent.Y, Color.Red);

            if (FindVertex(Direction.Left, parent) is Vertex leftVertex)
            {
                parent.Neighbours.Add(leftVertex);
                visited.Add(leftVertex);
            }
            if (FindVertex(Direction.Right, parent) is Vertex rightVertex)
            {
                parent.Neighbours.Add(rightVertex);
                visited.Add(rightVertex);
            }
            if (FindVertex(Direction.Up, parent) is Vertex upVertex)
            {
                parent.Neighbours.Add(upVertex);
                visited.Add(upVertex);
            }
            if (FindVertex(Direction.Down, parent) is Vertex downVertex)
            {
                parent.Neighbours.Add(downVertex);
                visited.Add(downVertex);
            }

            foreach (Vertex vertex in parent.Neighbours)
            {
                FindNeighbourVertices(vertex);
            }
        }

        private Vertex FindVertex(Direction direction, Vertex parent)
        {
            Point nextLocation = parent.Location.MoveInDirection(direction);//  MoveInDirection(parent.Location, direction);

            //while (!_bmp.IsWall(nextLocation))
            while(!_matrix.IsWall(nextLocation))
            {
                if (IsNewVertex(nextLocation, direction))
                {
                    return new Vertex(nextLocation, parent);
                }
                nextLocation = nextLocation.MoveInDirection(direction);// MoveInDirection(nextLocation, direction);
            }

            return null;
        }

        private bool IsNewVertex(Point location, Direction edgeDirection)
        {
            if (_matrix.IsWall(location))
                return false;

            if (visited.ContainsLocation(location))
                return false;

            Point down = location.Down();
            bool wallDown = _matrix.IsWall(down);
            Point up = location.Up();
            bool wallUp = _matrix.IsWall(up);
            Point left = location.Left();
            bool wallLeft = _matrix.IsWall(left);
            Point right = location.Right();
            bool wallRight = _matrix.IsWall(right);

            switch (edgeDirection)
            {
                case Direction.Up:
                    {
                        if (wallUp)
                            return true;

                        if (!wallLeft || !wallRight)
                            return true;

                        return false;
                    }
                case Direction.Down:
                    {
                        //Down is wall
                        if (wallDown)
                            return true;

                        if (!wallLeft || !wallRight)
                            return true;

                        return false;
                    }
                case Direction.Left:
                    {
                        if (wallLeft)
                            return true;

                        if (!wallUp || !wallDown)
                            return true;

                        return false;
                    }
                case Direction.Right:
                    {
                        if (wallRight)
                            return true;

                        if (!wallUp || !wallDown)
                            return true;

                        return false;
                    }
                default:
                    return false;
            }
        }

        private bool IsNewVertex(int x, int y, Direction edgeDirection)
        {
            return IsNewVertex(new Point(x, y), edgeDirection);
        }

        public IEnumerable<Vertex> GetVertices()
        {
            IEnumerable<Vertex> allVertices = Root.CollectVertices();
            foreach (Vertex vertex in allVertices)
            {
                yield return vertex;
            } 
        }

        public IEnumerable<OrientedEdge> GetEdges()
        {
            IEnumerable<OrientedEdge> allEdges = Root.CollectEdges();
            foreach (OrientedEdge edge in allEdges)
            {
                yield return edge;
            }
        }

    }
}
