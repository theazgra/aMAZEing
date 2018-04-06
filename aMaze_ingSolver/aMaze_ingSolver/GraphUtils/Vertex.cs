using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aMaze_ingSolver.GraphUtils
{
    class Vertex : IEquatable<Vertex>
    {
        public bool IsRoot { get; private set; } = false;

        public Point Location { get; }
        public int X => Location.X;
        public int Y => Location.Y;

        //public Vertex Parent { get; private set; }
        //public OrientedEdge EdgeToParent { get; private set; }
        //public List<Vertex> Neighbours { get; set; }
        private Dictionary<Direction, Vertex> _neighbours;

        public IEnumerable<Vertex> Neighbours
        {
            get
            {
                List<Vertex> neighbours = new List<Vertex>();

                if (_neighbours.ContainsKey(Direction.Left))
                    neighbours.Add(_neighbours[Direction.Left]);
                if (_neighbours.ContainsKey(Direction.Right))
                    neighbours.Add(_neighbours[Direction.Right]);
                if (_neighbours.ContainsKey(Direction.Up))
                    neighbours.Add(_neighbours[Direction.Up]);
                if (_neighbours.ContainsKey(Direction.Down))
                    neighbours.Add(_neighbours[Direction.Down]);

                return neighbours;
            }
        }


        public Vertex(int col, int row) : this(new Point(col, row))
        { }

        public Vertex(Point location)
        {
            

            Location = location;
            _neighbours = new Dictionary<Direction, Vertex>();


            //if (parent == null)
            //{
            //    IsRoot = true;
            //}
            //else
            //{
            //    EdgeToParent = new OrientedEdge(parent, this);
            //}
        }

        public void AddNeighbour(Direction direction, Vertex vertex)
        {
            _neighbours.Add(direction, vertex);
        }

        public Vertex GetNeighbour(Direction direction)
        {
            if (!_neighbours.ContainsKey(direction))
                return null;

            return _neighbours[direction];
        }



        public string Print()
        {
            throw new NotImplementedException();
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
            if(other == null)
            {
                return false;
            }
            return (this.X == other.X && this.Y == other.Y);
        }

        public bool HasNeighbour(Direction direction)
        {
            return this.GetNeighbour(direction) == null;
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
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return Location.ToString();
        }
    }
}
