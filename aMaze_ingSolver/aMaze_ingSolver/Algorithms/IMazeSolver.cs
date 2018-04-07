using System;
using aMaze_ingSolver.GraphUtils;
using System.Collections.Generic;

namespace aMaze_ingSolver.Algorithms
{
    delegate void solved();
    interface IMazeSolver
    {
        /// <summary>
        /// Event invoked when maze is solved.
        /// </summary>
        event solved OnSolved;

        /// <summary>
        /// Determine in solver is paralellized.
        /// </summary>
        bool Parallel { get; set; }

        /// <summary>
        /// If current graph was solved.
        /// </summary>
        bool Solved { get; set; }

        /// <summary>
        /// Name of solver.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Solve maze by finding path.
        /// </summary>
        void SolveMaze(Graph graph);

        /// <summary>
        /// Get vertices creating path from start to finish.
        /// </summary>
        /// <returns>Vertices creating path.</returns>
        Queue<Vertex> GetResultVertices();

        /// <summary>
        /// Get time needed for path finding.
        /// </summary>
        /// <returns>Time span.</returns>
        TimeSpan GetSolveTime();
    }
}
