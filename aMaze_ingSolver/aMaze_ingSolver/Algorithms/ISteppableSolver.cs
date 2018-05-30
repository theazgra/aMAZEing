using aMaze_ingSolver.GraphUtils;

namespace aMaze_ingSolver.Algorithms
{
    delegate void AnimationProgress(Vertex currentVertex);

    interface ISteppableSolver : IMazeSolver
    {
        /// <summary>
        /// Delay between steps in milliseconds.
        /// </summary>
        int Delay { get; set; }

        /// <summary>
        /// Initialize solver with graph.
        /// </summary>
        /// <param name="graph">Graph to find path in.</param>
        void InitializeAnimation(Graph graph);

        /// <summary>
        /// Run solving in steps with set Delay.
        /// </summary>
        void RunAnimated();

        /// <summary>
        /// Pause the solver.
        /// </summary>
        void PauseAnimation();

        /// <summary>
        /// Perform just one solving step.
        /// </summary>
        /// <returns>Currently processed vertex.</returns>
        Vertex PerformStep();

        /// <summary>
        /// Event invoked on each animation step.
        /// </summary>
        event AnimationProgress OnAnimationProgress;

        /// <summary>
        /// Reset animation.
        /// </summary>
        void ResetAnimation();
    }
}
