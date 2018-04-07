using aMaze_ingSolver.GraphUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aMaze_ingSolver.Algorithms
{
    interface IMazeSolver
    {
        /// <summary>
        /// Determine in solver is paralellized.
        /// </summary>
        bool Parallel { get; set; }

        /// <summary>
        /// Solve maze by finding path.
        /// </summary>
        /// <param name="start">Entry point of maze.</param>
        void SaveMaze(Vertex start);

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
