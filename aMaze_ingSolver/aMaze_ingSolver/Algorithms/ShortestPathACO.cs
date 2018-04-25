using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aMaze_ingSolver.GraphUtils;

namespace aMaze_ingSolver.Algorithms
{
    /// <summary>
    /// Based on SHORTEST PATH PROBLEM SOLVING BASED ON ANT COLONY OPTIMIZATION METAHEURISTIC
    /// (MARIUSZ GŁ ˛ABOWSKI, BARTOSZ MUSZNICKI, PRZEMYSŁAW NOWAK, PIOTR ZWIERZYKOWSKI)
    /// https://www.degruyter.com/downloadpdf/j/ipc.2012.17.issue-1-2/v10248-012-0011-5/v10248-012-0011-5.pdf
    /// </summary>
    class ShortestPathACO : MazeSolver
    {
        public override string Name => "ShortestPathACO";
        public override event Solved OnSolved;

        /// <summary>
        /// Number of ants.
        /// </summary>
        int antCount = 2;
        /// <summary>
        /// Influence of pheromones on the choice of next vertex.
        /// </summary>
        float alpha = 1.0f;
        /// <summary>
        /// Influence of remaining data on the choice of the next vertex.
        /// </summary>
        float beta = 1.0f;

        /// <summary>
        /// speed at which evaporation of the pheromone trail occurs; takes on values from interval[0, 1].
        /// </summary>
        float gamma = 1.0f;

        /// <summary>
        /// Initial level of pheromones on the edges.
        /// </summary>
        float tau_0 = 1.0f;

        /// <summary>
        /// The minimum acceptable level of pheromones on edges.
        /// </summary>
        float tau_min;
        /// <summary>
        /// The maximum acceptable level of pheromones on edges.
        /// </summary>
        float tau_max;

        float convergence = 0;
        float last_length = float.PositiveInfinity;

        List<Ant> _ants;
        //S initial vertex, T end vertex (For us Start and End of Graph.)

        public override void SolveMaze(Graph graph)
        {
            _ants = new List<Ant>();
            graph.Reset();
            _timer.Start();

            Initialization(graph);

            _timer.Stop();
            OnSolved?.Invoke();
        }

        private void Initialization(Graph graph)
        {

            int C = 0;
            //a_{ij} = cost, length
            foreach (Vertex vertex in graph.Vertices)
            {
                vertex.InitializeEdge(tau_0);
                foreach (OrientedEdge edge in vertex.Edges)
                {
                    if (edge.Length > C)
                        C = edge.Length;
                }
            }

            int length = C * (graph.Vertices.Count - 1);
            int time = 0;

            for (int i = 0; i < antCount; i++)
            {
                _ants.Add(new Ant(graph.Start));
            }
        }

        //private float CalculateCoefficient()

        ////current vertex i is in ant.
        //private Vertex SelectNextVertex(Ant ant)
        //{

        //}

        public override string ToString()
        {
            return Name;
        }
    }
}
