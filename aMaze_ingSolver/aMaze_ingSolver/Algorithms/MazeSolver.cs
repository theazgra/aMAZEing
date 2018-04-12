using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using aMaze_ingSolver.GraphUtils;

namespace aMaze_ingSolver.Algorithms
{
    abstract class MazeSolver : IMazeSolver
    {
        public abstract event solved OnSolved;
        protected Queue<Vertex> _resultPath = new Queue<Vertex>();
        protected Stopwatch _timer = new Stopwatch();

        public bool Parallel { get; set; }
        public bool Solved { get; set; }

        public abstract string Name { get; }
        public abstract void SolveMaze(Graph graph);


        public Queue<Vertex> GetResultVertices()
        {
            if (_resultPath == null)
                return new Queue<Vertex>();

            return new Queue<Vertex>(_resultPath);
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
            if (_resultPath == null)
                return 0;   

            Queue<Vertex> resultCopy = new Queue<Vertex>(_resultPath);
            Vertex current = resultCopy.Dequeue();
            Vertex next = null;

            int length = 0;

            while (resultCopy.Count > 0)
            {
                next = resultCopy.Dequeue();
                length += DistanceBetweenVertices(current, next);
                current = next;
            }

            return length;
        }

        private int DistanceBetweenVertices(Vertex origin, Vertex destination)
        {
            switch (Utils.GetDirection(origin.Location, destination.Location))
            {
                case Direction.Up:
                    return (origin.Location.Y - destination.Location.Y);
                case Direction.Down:
                    return -(origin.Location.Y - destination.Location.Y);
                case Direction.Left:
                    return (origin.Location.X - destination.Location.X);
                case Direction.Right:
                    return -(origin.Location.X - destination.Location.X);
                default:
                    return 0;
            }
        }
    }
}
