using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aMaze_ingSolver.GraphUtils;

namespace aMaze_ingSolver.Algorithms
{
    class AStar : MazeSolver
    {
        public override string Name => "A*";

        public override event solved OnSolved;

        public override void SolveMaze(Graph graph)
        {
            _timer.Start();

            float distance = graph.Start.DistanceTo(graph.End);

            graph.CalculateDistancesFromVerticesToEnd();
            

            _timer.Stop();
            OnSolved?.Invoke();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
