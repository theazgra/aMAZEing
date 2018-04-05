using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aMaze_ingSolver.Tree
{
    class Node : IEquatable<Node>
    {
        public bool IsRoot { get; private set; } = false;
        private Point _location;


        public Node Parent { get; private set; }
        public List<Node> Neighbours { get; set; }

        public bool AllNeighboursVisited { get; set; } = false;

        public Node(int x, int y, Node parent)
        {
            Parent = parent;

            _location = new Point(x, y);
            Neighbours = new List<Node>();


            if (parent == null)
                IsRoot = true;
        }

        public Node(Point location, Node parent)
        {
            Parent = parent;

            _location = location;
            Neighbours = new List<Node>();


            if (parent == null)
                IsRoot = true;
        }

        public Point Location
        {
            get
            {
                return _location;
            }
        }

        public int X => _location.X;


        public int Y => _location.Y;

        public string Print()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("Node at [X:{0};Y:{1}]", X, Y));
            foreach (Node node in Neighbours)
            {
                sb.AppendLine("\t" + node.Print());
            }

            return sb.ToString();
        }


        public bool IsMyLocation(int x, int y)
        {
            return (X == x && Y == y);
        }

        public bool IsMyLocation(Point location)
        {
            return IsMyLocation(location.X, location.Y);
        }

        public bool Equals(Node other)
        {
            return (this.X == other.X && this.Y == other.Y);
        }
    }
}
