namespace aMaze_ingSolver.Algorithms
{
    interface ISteppable : IMazeSolver
    {
        /// <summary>
        /// Delay between steps in milliseconds.
        /// </summary>
        int Delay { get; set; }

        /// <summary>
        /// Run solving in steps with set Delay.
        /// </summary>
        void RunAnimated();

        /// <summary>
        /// Perform just one solving step.
        /// </summary>
        void PerformStep();
    }
}
