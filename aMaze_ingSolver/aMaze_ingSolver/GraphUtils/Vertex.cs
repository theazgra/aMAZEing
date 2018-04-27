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
        public float EuclideanDistanceToEnd { get; set; } = float.PositiveInfinity;

        /// <summary>
        /// Previous vertex from which we get to this vertex.
        /// </summary>
        public Vertex Previous { get; set; }

        /// <summary>
        /// Combined distance of BestPathDistance and euclidean distance from end.
        /// </summary>
        public float TotalPathDistance
        {
            get
            {
                return EuclideanDistanceToEnd + BestPathDistance;
            }
        }

        /// <summary>
        /// Best (shortest) distance to this vertex.
        /// </summary>
        public float BestPathDistance { get; set; } = float.PositiveInfinity;

        private List<OrientedEdge> _neighbours;

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
            _neighbours = new List<OrientedEdge>();
        }

        public Vertex this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return GetNeighbour(Direction.Down);
                    case 1:
                        return GetNeighbour(Direction.Left);
                    case 2:
                        return GetNeighbour(Direction.Right);
                    case 3:
                        return GetNeighbour(Direction.Up);
                    default:
                        return null;
                }
            }
        }

        public OrientedEdge EdgeTo(Vertex destination)
        {
            foreach (OrientedEdge edge in _neighbours)
            {
                if (edge.Destination.Equals(destination))
                    return edge;
            }
            return null;
        }

        /// <summary>
        /// Get all neighbours.
        /// </summary>
        public IEnumerable<Vertex> Neighbours
        {
            get
            {
                List<Vertex> neighbours = new List<Vertex>();

                foreach (OrientedEdge edge in _neighbours)
                {
                    neighbours.Add(edge.Destination);
                }
                return neighbours;
            }
        }

        /// <summary>
        /// Get this vertex edges
        /// </summary>
        public IEnumerable<OrientedEdge> Edges
        {
            get
            {
                return _neighbours;
            }
        }

        /// <summary>
        /// Add neighbour in direction.
        /// </summary>
        /// <param name="direction">Direction in which neighbour is connected.</param>
        /// <param name="neighbour">Neighbour vertex.</param>
        public void AddNeighbour(Direction direction, Vertex neighbour)
        {
            _neighbours.Add(new OrientedEdge(this, neighbour, direction));
        }

        /// <summary>
        /// Get neighbour vertex in direction.
        /// </summary>
        /// <param name="direction">Direction of connection to vertex.</param>
        /// <returns>Connected neighbour or null.</returns>
        public Vertex GetNeighbour(Direction direction)
        {
            foreach (OrientedEdge edge in _neighbours)
            {
                if (edge.Direction == direction)
                    return edge.Destination;
            }

            return null;
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
            return _neighbours.Any(e => e.Direction == direction);
            //return _neighbours.ContainsKey(direction);
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
        /// <returns>Neighbouring vertices.</returns>
        public Queue<Vertex> GetOrderedNeighbours()
        {
            Queue<Vertex> vertices = new Queue<Vertex>();

            if (_neighbours.FirstOrDefault(e => e.Direction == Direction.Up) is OrientedEdge upEdge)
                vertices.Enqueue(upEdge.Destination);
            if (_neighbours.FirstOrDefault(e => e.Direction == Direction.Left) is OrientedEdge leftEdge)
                vertices.Enqueue(leftEdge.Destination);
            if (_neighbours.FirstOrDefault(e => e.Direction == Direction.Down) is OrientedEdge downEdge)
                vertices.Enqueue(downEdge.Destination);
            if (_neighbours.FirstOrDefault(e => e.Direction == Direction.Right) is OrientedEdge rightEdge)
                vertices.Enqueue(rightEdge.Destination);

            return vertices;
        }


        /// <summary>
        /// Edges ordered like up, left, down, right.
        /// </summary>
        /// <returns>Edges</returns>
        public Queue<OrientedEdge> GetOrderedEdges()
        {
            Queue<OrientedEdge> edges = new Queue<OrientedEdge>();

            if (_neighbours.FirstOrDefault(e => e.Direction == Direction.Up) is OrientedEdge upEdge)
                edges.Enqueue(upEdge);
            if (_neighbours.FirstOrDefault(e => e.Direction == Direction.Left) is OrientedEdge leftEdge)
                edges.Enqueue(leftEdge);
            if (_neighbours.FirstOrDefault(e => e.Direction == Direction.Down) is OrientedEdge downEdge)
                edges.Enqueue(downEdge);
            if (_neighbours.FirstOrDefault(e => e.Direction == Direction.Right) is OrientedEdge rightEdge)
                edges.Enqueue(rightEdge);

            return edges;
        }

        public override int GetHashCode()
        {
            var hashCode = -1732179082;
            hashCode = hashCode * -1521134295 + EqualityComparer<Point>.Default.GetHashCode(Location);
            hashCode = hashCode * -1521134295 + Visited.GetHashCode();
            hashCode = hashCode * -1521134295 + EuclideanDistanceToEnd.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Vertex>.Default.GetHashCode(Previous);
            return hashCode;
        }

        public void InitializeEdge(float initialValue)
        {
            foreach (OrientedEdge edge in _neighbours)
            {
                edge.Pheromones = initialValue;
                edge.Visited = false;
            }
        }

        /// <summary>
        /// Reset this vertex and connected edges.
        /// </summary>
        public void Reset()
        {

            this.Visited = false;
            this.BestPathDistance = float.PositiveInfinity;
            this.EuclideanDistanceToEnd = float.PositiveInfinity;
            this.Previous = null;
            foreach (OrientedEdge edge in _neighbours)
            {
                edge.Reset();
            }
        }
    }
}
