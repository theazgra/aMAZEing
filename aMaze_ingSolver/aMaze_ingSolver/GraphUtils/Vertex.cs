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
        /// <summary>
        /// Location of vertex.
        /// </summary>
        public Point Location { get; }

        /// <summary>
        /// X component of location.
        /// </summary>
        public int X => Location.X;
        /// <summary>
        /// Y component of location.
        /// </summary>
        public int Y => Location.Y; 

        /// <summary>
        /// Vertex was visited by algorithm.
        /// </summary>
        public bool Visited { get; set; }

        /// <summary>
        /// Distance from this vertex to End of the maze. Used in A*.
        /// </summary>
        public float DistanceFromEnd { get; set; }

        /// <summary>
        /// Previous vertex from which we get to this vertex.
        /// </summary>
        public Vertex Previous { get; set; }

        /// <summary>
        /// Best (shortest) distance to this vertex.
        /// </summary>
        public float BestPathDistance { get; set; } = float.PositiveInfinity;

        private Dictionary<Direction, Vertex> _neighbours;

        /// <summary>
        /// Creates vertex at given location.
        /// </summary>
        /// <param name="col">Column in image or X component of location.</param>
        /// <param name="row">Row in image or Y component of location.</param>
        public Vertex(int col, int row) : this(new Point(col, row))
        { }

        public Vertex(Point location)
        {
            Location = location;
            _neighbours = new Dictionary<Direction, Vertex>();
        }

        /// <summary>
        /// Get all neighbours.
        /// </summary>
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

        /// <summary>
        /// Add neighbour in direction.
        /// </summary>
        /// <param name="direction">Direction in which neighbour is connected.</param>
        /// <param name="vertex">Neighbour vertex.</param>
        public void AddNeighbour(Direction direction, Vertex vertex)
        {
            _neighbours.Add(direction, vertex);
        }

        /// <summary>
        /// Get neighbour vertex in direction.
        /// </summary>
        /// <param name="direction">Direction of connection to vertex.</param>
        /// <returns>Connected neighbour or null.</returns>
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

        public bool Equals(Vertex other)
        {
            if (other == null)
            {
                return false;
            }
            return (this.X == other.X && this.Y == other.Y);
        }

        public override bool Equals(object obj)
        {
            if (obj is Vertex v)
                return this.Equals(v);

            return base.Equals(obj);
        }

        /// <summary>
        /// Check if neighbour is connected in direction.
        /// </summary>
        /// <param name="direction">Direction of connection to neighbour.</param>
        /// <returns>True if neighbour is connected.</returns>
        public bool HasNeighbour(Direction direction)
        {
            //return this.GetNeighbour(direction) == null;
            return _neighbours.ContainsKey(direction);
        }

        /// <summary>
        /// Collect connected neighbours and self.
        /// </summary>
        /// <returns>Connected neighbours and self.</returns>
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

        public override string ToString()
        {
            return Location.ToString();
        }

        /// <summary>
        /// Neighbours ordered like up, left, down, right.
        /// </summary>
        /// <returns></returns>
        public Queue<Vertex> GetOrderedNeighbours()
        {
            Queue<Vertex> vertices = new Queue<Vertex>();

            if (_neighbours.ContainsKey(Direction.Up))
                vertices.Enqueue(_neighbours[Direction.Up]);

            if (_neighbours.ContainsKey(Direction.Left))
                vertices.Enqueue(_neighbours[Direction.Left]);

            if (_neighbours.ContainsKey(Direction.Down))
                vertices.Enqueue(_neighbours[Direction.Down]);

            if (_neighbours.ContainsKey(Direction.Right))
                vertices.Enqueue(_neighbours[Direction.Right]);


            return vertices;
        }

        public override int GetHashCode()
        {
            var hashCode = -1732179082;
            hashCode = hashCode * -1521134295 + EqualityComparer<Point>.Default.GetHashCode(Location);
            hashCode = hashCode * -1521134295 + Visited.GetHashCode();
            hashCode = hashCode * -1521134295 + DistanceFromEnd.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Vertex>.Default.GetHashCode(Previous);
            return hashCode;
        }
    }
}
