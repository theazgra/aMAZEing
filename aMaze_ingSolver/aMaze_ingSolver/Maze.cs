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
        private Node _root;
        List<Node> visited = new List<Node>();
        Color globalColor;

        int c = 0;

        public Maze(Image img)
        {
            _bmp = new Bitmap(img);
            globalColor = Color.FromArgb(150, 200, 180);


            FindStart();
            FindFinish();
            BuildTree();

        }

        //Maze movement
        private Point Up(Point origin) => new Point(origin.X, origin.Y - 1);
        private Point Down(Point origin) => new Point(origin.X, origin.Y + 1);
        private Point Left(Point origin) => new Point(origin.X - 1, origin.Y);
        private Point Right(Point origin) => new Point(origin.X + 1, origin.Y);

        private Color GetColor()
        {
            c = (c + 1) % 3;
            switch (c)
            {
                case 0:
                    globalColor = Color.FromArgb(globalColor.R - 10, globalColor.G, globalColor.B);
                    break;
                case 1:
                    globalColor = Color.FromArgb(globalColor.R, globalColor.G - 10, globalColor.B);
                    break;
                case 2:
                    globalColor = Color.FromArgb(globalColor.R, globalColor.G, globalColor.B - 10);
                    break;
                default:
                    break;
            }

            return globalColor;
        }

        private PictureBox pb;
        public void SetCanvas(PictureBox imgBox)
        {
            pb = imgBox;
            DrawCanvas();

            //using (StreamWriter writer = new StreamWriter("Tree.txt", false, Encoding.UTF8))
            //{
            //    writer.WriteLine(_root.Print());
            //}
        }

        private void DrawCanvas()
        {
            if (pb == null)
                return;

            //foreach (Node node in visited)
            //{

            //}

            pb.Image = _bmp;
        }

        public void BuildTree()
        {


            _root = new Node(_start.X, _start.Y, null);
            visited.Add(_root);

            FindNeightbour(_root);

            _bmp.Save("vertex.png", System.Drawing.Imaging.ImageFormat.Png);



        }

        private void FindNeightbour(Node parent)
        {
            _bmp.SetPixel(parent.X, parent.Y, Color.Red);
            DrawCanvas();

            //Left
            //Point left = Left(parent.Location)
            //if (!IsWall(left) && !visited.ContainsLocation(left))
            //{
            //    Node leftNeightbour = new Node(left, parent);
            //    parent.Neighbours.Add(leftNeightbour);
            //    visited.Add(leftNeightbour);
            //}

            if (FindVertex(Direction.Left, parent) is Node leftVertex)
            {
                parent.Neighbours.Add(leftVertex);
                visited.Add(leftVertex);
            }
            if (FindVertex(Direction.Right, parent) is Node rightVertex)
            {
                parent.Neighbours.Add(rightVertex);
                visited.Add(rightVertex);
            }
            if (FindVertex(Direction.Up, parent) is Node upVertex)
            {
                parent.Neighbours.Add(upVertex);
                visited.Add(upVertex);
            }
            if (FindVertex(Direction.Down, parent) is Node downVertex)
            {
                parent.Neighbours.Add(downVertex);
                visited.Add(downVertex);
            }

            /*
            //Right
            Point right = Right(parent.Location);
            //if (!IsWall(right) && !visited.ContainsLocation(right))
            if (IsNewVertex(right, parent))
            {
                Node rightNeightbour = new Node(right, parent);
                parent.Neighbours.Add(rightNeightbour);
                visited.Add(rightNeightbour);

            }
            //Down
            Point down = Down(parent.Location);
            //if (!IsWall(down) && !visited.ContainsLocation(down))
            if (IsNewVertex(down, parent))
            {
                Node downNeightbour = new Node(down, parent);
                parent.Neighbours.Add(downNeightbour);
                visited.Add(downNeightbour);

            }
            //Up
            Point up = Up(parent.Location);
            //if (!IsWall(up) && !visited.ContainsLocation(up))
            if (IsNewVertex(up, parent))
            {
                Node upNeightbour = new Node(up, parent);
                parent.Neighbours.Add(upNeightbour);
                visited.Add(upNeightbour);

            }
            */
            foreach (Node node in parent.Neighbours)
            {
                FindNeightbour(node);
            }
        }



        public Image GetImage()
        {
            _bmp.SetPixel(_start.X, _start.Y, Color.Red);
            _bmp.SetPixel(_finish.X, _finish.Y, Color.Blue);

            return _bmp;
        }

        private void FindStart()
        {
            for (int x = 0; x < _bmp.Width; x++)
            {
                if (!IsWall(x, 0))
                {
                    _start = new Point(x, 0);
                    break;
                }
            }
        }

        private void FindFinish()
        {
            for (int x = 0; x < _bmp.Width; x++)
            {
                if (!IsWall(x, _bmp.Height - 1))
                {
                    _finish = new Point(x, _bmp.Height - 1);
                    break;
                }
            }
        }

        private Node FindVertex(Direction direction, Node parent)
        {
            Point nextLocation = MoveInDirection(parent.Location, direction);

            while (!IsWall(nextLocation))
            {
                if (IsNewVertex(nextLocation, direction))
                {
                    return new Node(nextLocation, parent);
                }
                nextLocation = MoveInDirection(nextLocation, direction);
            }

            return null;
        }

        private bool IsNewVertex(Point location, Direction edgeDirection)
        {
            if (IsWall(location))
                return false;

            if (visited.ContainsLocation(location))
                return false;

            Point down = Down(location);
            bool wallDown = IsWall(down);
            Point up = Up(location);
            bool wallUp = IsWall(up);
            Point left = Left(location);
            bool wallLeft = IsWall(left);
            Point right = Right(location);
            bool wallRight = IsWall(right);

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

        private Direction GetDirection(Point origin, Point destination)
        {
            int deltaX = origin.X - destination.X;
            int deltaY = origin.Y - destination.Y;

            if (deltaX == 0)
            {
                return (deltaY > 0) ? Direction.Up : Direction.Down;
            }
            else if (deltaY == 0)
            {
                return (deltaX > 0) ? Direction.Left : Direction.Right;
            }

            return Direction.NoDirection;
        }

        private Point MoveInDirection(Point origin, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Up(origin);
                case Direction.Down:
                    return Down(origin);
                case Direction.Left:
                    return Left(origin);
                case Direction.Right:
                    return Right(origin);
                default:
                    return origin;
            }
        }

        public bool IsWall(Point p)
        {
            return IsWall(p.X, p.Y);
        }

        public bool IsWall(int x, int y)
        {
            if (x < 0 || x >= _bmp.Width || y < 0 || y >= _bmp.Height)
                return true;

            Color c = _bmp.GetPixel(x, y);

            bool wall = (c.R == 0) && (c.G == 0) && (c.B == 0);
            return wall;
        }

        public string ToString(Color c)
        {
            return string.Format("[R:{0};G:{1};B:{2}]", c.R, c.G, c.B);
        }

        public void Print()
        {
            for (int i = 0; i < _bmp.Height; i++)
            {
                //Console.WriteLine("row start");
                for (int j = 0; j < _bmp.Width; j++)
                {
                    Console.Write(ToString(_bmp.GetPixel(j, i)));
                }
                Console.WriteLine();

            }
        }
    }
}
