
using System;
using System.Collections.Generic;
using System.Drawing;

namespace aMaze_ingSolver.GraphUtils
{
    class OrientedEdge
    {
        /// <summary>
        /// Origin of edge.
        /// </summary>
        public Vertex Origin { get; }

        /// <summary>
        /// End of edge.
        /// </summary>
        public Vertex Destination { get; }

        /// <summary>
        /// Direction of edge.
        /// </summary>
        public Direction Direction { get; }

        /// <summary>
        /// Length of the edge.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Is edge visited?
        /// </summary>
        public bool Visited { get; set; } = false;

        /// <summary>
        /// Pheromones on the edge, for ACO.
        /// </summary>
        public float Pheromones { get; set; } = 0.0f;


        public OrientedEdge(Vertex origin, Vertex destination, Direction direction)
        {
            this.Origin = origin;
            this.Destination = destination;
            this.Length = origin.PathDistanceTo(destination);
            this.Direction = direction;
        }

        /// <summary>
        /// Set visited to false.
        /// </summary>
        public  void Reset()
        {
            this.Visited = false;
        }
        
        /// <summary>
        /// Get points creating edge.
        /// </summary>
        /// <param name="includeVertices">Include edges?</param>
        /// <returns>Points.</returns>
        public IEnumerable<Point> GetEdgePoints(bool includeVertices = false)
        {
            List<Point> points = new List<Point>();

            Point point = Origin.Location.MoveInDirection(Direction);
            if (Direction == Direction.NoDirection)
                return points;

            while (point != Destination.Location)
            {
                points.Add(point);
                point = point.MoveInDirection(Direction);
            } 
            points.Add(point);

            if (!includeVertices)
                points.Remove(Destination.Location);
            else
                points.Add(Origin.Location);

            return points;
        }
    }
}
