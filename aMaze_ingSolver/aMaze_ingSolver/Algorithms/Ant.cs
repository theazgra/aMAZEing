using aMaze_ingSolver.GraphUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aMaze_ingSolver.Algorithms
{
    class Ant
    {
        public Vertex CurrentVertex { get; set; }
        public List<Vertex> VisitedVertices { get; set; }
        public int RouteCount { get; set; }
        public Graph Data { get; set; }
        public Dictionary<Vertex, int> TimeToReachVertex { get; set; }

        public  Ant(Vertex start)
        {
            TimeToReachVertex = new Dictionary<Vertex, int>();
            RouteCount = 0;
            CurrentVertex = start;
            VisitedVertices = new List<Vertex>()
            {
                start
            };
            TimeToReachVertex.Add(start, 0);
            Data = null;
        }
    }
}
