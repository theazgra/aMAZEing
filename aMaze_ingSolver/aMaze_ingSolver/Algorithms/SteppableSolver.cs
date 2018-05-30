using System.Timers;
using aMaze_ingSolver.GraphUtils;

namespace aMaze_ingSolver.Algorithms
{
    abstract class SteppableSolver : MazeSolver, ISteppableSolver
    {
        private Timer _animationTimer;
        private bool _animationFinished;

        protected Graph _animationGraph;

        public int Delay { get; set; }
        public event AnimationProgress OnAnimationProgress;

        public bool AnimationFinished
        {
            get
            {
                return _animationFinished;
            }
            protected set
            {
                _animationFinished = value;
                if (value)
                {
                    _animationTimer.Stop();
                }
            }
        }

        public abstract void ResetAnimation();

        public void InitializeAnimation(Graph graph)
        {
            _animationGraph = graph;
        }

        public void PauseAnimation()
        {
            _animationTimer.Stop();
        }

        public abstract Vertex PerformStep();
        

        public void RunAnimated()
        {
            if (_animationGraph == null)
                throw new System.ArgumentNullException(nameof(_animationGraph));

            _animationTimer = new Timer(Delay)
            {
                AutoReset = true
            };
            _animationTimer.Elapsed += InvokeStep;
            _animationTimer.Start();
        }

        private void InvokeStep(object sender, ElapsedEventArgs e)
        {
            Vertex vertex = PerformStep();
            OnAnimationProgress?.Invoke(vertex);
        }
    }
}
