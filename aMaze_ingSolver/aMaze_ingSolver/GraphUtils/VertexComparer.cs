using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aMaze_ingSolver.GraphUtils
{
    class VertexDistanceComparer : IComparer<Vertex>
    {
        public int Compare(Vertex x, Vertex y)
        {
            if (x.Visited && y.Visited)
                return 0;
            else
            {
                //< 0  = x < y
                // 0   = x == 0
                //> 0  = x > y
                //This comparsion is meent for unvisited vertices.
                if (x.BestPathDistance == float.PositiveInfinity || y.BestPathDistance != float.PositiveInfinity)
                    return 1;
                if (y.BestPathDistance == float.PositiveInfinity || x.BestPathDistance != float.PositiveInfinity)
                    return -1;

                return ((int)x.BestPathDistance - (int)y.BestPathDistance);
            }
        }
    }

    class FinalPathComparer : IComparer<Point>
    {
        public int Compare(Point x, Point y)
        {
            if (x.Equals(y))
                return 0;

            //First Y
            if (x.Y < y.Y)
                return -1;
            else if (x.Y > y.Y)
                return 1;
            else
            {
                if (x.X < y.X)
                    return -1;
                else
                    return 1;
            }
        }
    }

    class VertexPositionComparer : IEqualityComparer<Vertex>
    {
        public bool Equals(Vertex x, Vertex y)
        {
            return (x.X == y.X && x.Y == y.Y);
        }

        public int GetHashCode(Vertex obj)
        {
            var hashCode = -1732179082;
            hashCode = hashCode * -1521134295 + EqualityComparer<System.Drawing.Point>.Default.GetHashCode(obj.Location);
            hashCode = hashCode * -1521134295 + obj.Visited.GetHashCode();
            hashCode = hashCode * -1521134295 + obj.EuclideanDistanceToEnd.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Vertex>.Default.GetHashCode(obj.Previous);
            return hashCode;
        }
    }
}
