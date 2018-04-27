using System;
using aMaze_ingSolver.GraphUtils;
using System.Collections.Generic;
using System.Drawing;

namespace aMaze_ingSolver.Algorithms
{
    delegate void Solved();
    delegate void SolveProgress(float percentDone);

    interface IMazeSolver
    {
        /// <summary>
        /// Event invoked when maze is solved.
        /// </summary>
        event Solved OnSolved;

        /// <summary>
        /// Event informing about solving process.
        /// </summary>
        event SolveProgress OnSolveProgress;

        /// <summary>
        /// Determine if solver is paralellized.
        /// </summary>
        bool Parallel { get; set; }

        /// <summary>
        /// If algorithm support parallel implementation.
        /// </summary>
        bool SupportParallel { get; }

        /// <summary>
        /// Number of threads avaible for solver if Parallel is true.
        /// </summary>
        int ThreadCount { get; set; }

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
        /// Get vertices creating path from start to finish in good order.
        /// </summary>
        /// <returns>Ordered vertices.</returns>
        Queue<Vertex> GetOrderedResultVertices();

        /// <summary>
        /// Get the lenght of the result path.
        /// </summary>
        /// <returns>Length.</returns>
        int GetPathLength();
        
        /// <summary>
        /// Get number of vertices in result path.
        /// </summary>
        /// <returns>Number of vertices in result path.</returns>
        int GetResultVertexCount();

        /// <summary>
        /// Get time needed for path finding.
        /// </summary>
        /// <returns>Time span.</returns>
        TimeSpan GetSolveTime();

        /// <summary>
        /// Get stopwatch ticks.
        /// </summary>
        /// <returns>Ticks of stopwatch</returns>
        long GetTicks();

        /// <summary>
        /// Reset solver.
        /// </summary>
        void Reset();

        /// <summary>
        /// Get random maze color.
        /// </summary>
        Color MazeColor { get; }
    }
}
