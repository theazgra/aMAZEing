using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using aMaze_ingSolver.GraphUtils;

namespace aMaze_ingSolver.Algorithms
{
    struct BlockingQueueJobInfo
    {
        /// <summary>
        /// (Current, next)
        /// </summary>
        public ConcurrentQueue<VertexPair> blockingCollection;
        public CancellationToken cancelToken;
        public BlockingQueueJobInfo(ConcurrentQueue<VertexPair> blockingCollection, CancellationToken cancellationToken)
        {
            this.blockingCollection = blockingCollection;
            this.cancelToken = cancellationToken;
        }
    }

    struct BlockingStackJobInfo
    {
        /// <summary>
        /// (Current, next)
        /// </summary>
        public ConcurrentStack<VertexPair> blockingCollection;
        public CancellationToken cancelToken;
        public BlockingStackJobInfo(ConcurrentStack<VertexPair> blockingCollection, CancellationToken cancellationToken)
        {
            this.blockingCollection = blockingCollection;
            this.cancelToken = cancellationToken;
        }
    }

    struct VertexParam
    {
        public Vertex currentVertex;
        public Vertex nextVertex;
        //public CancellationToken cancelToken;
        public int threadToken;

        public VertexParam(Vertex current, Vertex next, int token)
        {
            currentVertex = current;
            nextVertex = next;
            threadToken = token;
        }
    }

    abstract class MazeSolver : IMazeSolver
    {
        public abstract event Solved OnSolved;
        public virtual event SolveProgress OnSolveProgress;

        protected Queue<Vertex> _resultPath = new Queue<Vertex>();
        protected Stopwatch _timer = new Stopwatch();

        public bool Parallel { get; set; }
        public bool Solved { get; set; }
        public int ThreadCount { get; set; } = 1;

        public abstract string Name { get; }
        public abstract bool SupportParallel { get; }
        private Color _mazeColor = Color.Empty;
        public Color MazeColor
        {
            get
            {
                if (_mazeColor == Color.Empty)
                    _mazeColor = Color.Blue;
                    //_mazeColor = RandomColor();

                return _mazeColor;
            }
        }


        public abstract void SolveMaze(Graph graph);


        public Queue<Vertex> GetResultVertices()
        {
            if (_resultPath == null)
                return new Queue<Vertex>();

            return new Queue<Vertex>(_resultPath);
        }

        public Queue<Vertex> GetOrderedResultVertices()
        {
            if (_resultPath == null)
                return new Queue<Vertex>();

            IEnumerable<Vertex> path = _resultPath;

            //path = path.OrderByDescending(v => v.Location, new FinalPathComparer());
            //path = path.OrderBy(v => v.Location.Y);
            path = path
                .OrderBy(v => v.Location.Y)
                .GroupBy(v => v.Location.Y)
                .SelectMany(g => g.OrderBy(v => v.X));

            return new Queue<Vertex>(path);
        }



        public TimeSpan GetSolveTime()
        {
            if (_timer == null)
                return new TimeSpan();

            return _timer.Elapsed;
        }

        public int GetResultVertexCount()
        {
            if (_resultPath == null)
                return 0;

            return _resultPath.Count;
        }

        public int GetPathLength()
        {
            if (!Solved || _resultPath == null || _resultPath.Count <= 0)
                return 0;

            Queue<Vertex> resultCopy = new Queue<Vertex>(_resultPath);
            Vertex current = resultCopy.Dequeue();
            Vertex next = null;

            int length = 0;

            while (resultCopy.Count > 0)
            {
                next = resultCopy.Dequeue();
                length += current.PathDistanceTo(next);
                current = next;
            }

            return length;
        }

        public long GetTicks()
        {
            if (_timer == null)
                return 0;

            return _timer.ElapsedTicks;
        }

        public void Reset()
        {
            Solved = false;
            _timer.Reset();
            _resultPath.Clear();
        }

        private Color RandomColor()
        {
            Random random = new Random(new Random().Next());
            int r = random.Next(50, 256);
            int g = random.Next(50, 256);
            int b = random.Next(50, 256);
            return Color.FromArgb(r, g, b);
        }
    }
}
