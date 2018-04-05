using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aMaze_ingSolver.Tree
{
    class Vertex : IEquatable<Vertex>
    {
        public bool IsRoot { get; private set; } = false;

        public Point Location { get; }
        public int X => Location.X;
        public int Y => Location.Y;

        public Vertex Parent { get; private set; }
        public List<Vertex> Neighbours { get; set; }
        public OrientedEdge EdgeToParent { get; private set; }


        public Vertex(int x, int y, Vertex parent) : this(new Point(x, y), parent)
        { }

        public Vertex(Point location, Vertex parent)
        {
            Parent = parent;

            Location = location;
            Neighbours = new List<Vertex>();


            if (parent == null)
            {
                IsRoot = true;
            }
            else
            {
                EdgeToParent = new OrientedEdge(parent, this);
            }
        }



        public string Print()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("Node at [X:{0};Y:{1}]", X, Y));
            foreach (Vertex vertex in Neighbours)
            {
                sb.AppendLine("\t" + vertex.Print());
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

        public bool Equals(Vertex other)
        {
            return (this.X == other.X && this.Y == other.Y);
        }

        public IEnumerable<Vertex> CollectVertices()
        {
            List<Vertex> allVertices = new List<Vertex>();
            foreach (Vertex vertex in Neighbours)
            {
                allVertices.AddRange(vertex.CollectVertices());
            }
            allVertices.Add(this);
            return allVertices;
        }

        public IEnumerable<OrientedEdge> CollectEdges()
        {
            List<OrientedEdge> allVertices = new List<OrientedEdge>();
            foreach (Vertex vertex in Neighbours)
            {
                allVertices.AddRange(vertex.CollectEdges());
            }
            allVertices.Add(this.EdgeToParent);
            return allVertices;
        }
    }
}
