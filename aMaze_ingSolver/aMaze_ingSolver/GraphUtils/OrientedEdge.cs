
using System;
using System.Collections.Generic;
using System.Drawing;

namespace aMaze_ingSolver.GraphUtils
{
    class OrientedEdge
    {
        public Vertex Origin { get; }
        public Vertex Destination { get; }
        public Direction Direction { get; }
        public int Length { get; }
        public bool Visited { get; set; } = false;

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
        
        public IEnumerable<Point> GetEdgePoints()
        {
            List<Point> points = new List<Point>();

            Point point = Origin.Location.MoveInDirection(Direction);

            while (point != Destination.Location)
            {
                points.Add(point);
                point = point.MoveInDirection(Direction);
            } 

            points.Add(point);
            points.Remove(Destination.Location);

            return points;
        }
    }
}
