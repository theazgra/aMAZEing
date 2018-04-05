
using System;
using System.Collections.Generic;
using System.Drawing;

namespace aMaze_ingSolver.Tree
{
    class OrientedEdge
    {
        public Vertex Origin { get; set; }
        public Vertex Destination { get; set; }
        public Direction Direction { get; set; }
        public int Size { get; private set; }

        public OrientedEdge(Vertex origin, Vertex destination)
        {
            Origin = origin;
            Destination = destination;
            Direction = Utils.GetDirection(origin.Location, destination.Location);

            if (Direction == Direction.Up || Direction == Direction.Down)
            {
                Size = Math.Abs(Destination.Y - Origin.Y);
            }
            else
            {
                Size = Math.Abs(Destination.X - Origin.X);
            }
        }

        public IEnumerable<Point> GetEdgePoints()
        {
            List<Point> points = new List<Point>();

            Point point = Origin.Location;
            do
            {
                points.Add(point);
                point = point.MoveInDirection(Direction);

            } while (point != Destination.Location);

            points.Add(point);

            return points;
        }
    }
}
