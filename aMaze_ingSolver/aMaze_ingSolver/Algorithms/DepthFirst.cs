using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aMaze_ingSolver.GraphUtils;

namespace aMaze_ingSolver.Algorithms
{
    class DepthFirst : IMazeSolver
    {
        public event solved OnSolved;

        public bool Parallel { get; set; } = false;
        public bool Solved { get; set; } = false;
        public string Name => "Depth first";

        private Queue<Vertex> _resultPath;
        private Stopwatch _stopwatch;

        private Dictionary<Vertex, Vertex> _visited;
        //private List<Guid> _visited;


        public DepthFirst()
        {
            _resultPath = new Queue<Vertex>();
            _stopwatch = new Stopwatch();
            //_visited = new List<Guid>();
            _visited = new Dictionary<Vertex, Vertex>();
        }

        private bool Visited(Vertex vertex)
        {
            //return _visited.Contains(vertex.UniqueId);
            return _visited.ContainsKey(vertex);
        }

        private void AddVisited(Vertex vertex, Vertex previous)
        {
            //_visited.Add(vertex.UniqueId);
            _visited.Add(vertex, previous);
        }

        private Vertex GetPrevious(Vertex vertex)
        {
            if (!_visited.ContainsKey(vertex))
                return null;

            return _visited[vertex];
        }

        public Queue<Vertex> GetResultVertices()
        {
            //Return copy so our queue is left intact.
            return new Queue<Vertex>(_resultPath);
        }

        public TimeSpan GetSolveTime()
        {
            return _stopwatch.Elapsed;
        }

        public void SolveMaze(Graph graph)
        {

            List<Vertex> previous = new List<Vertex>();
            Queue<Vertex> queue = new Queue<Vertex>();
            _stopwatch.Start();

            queue.Enqueue(graph.Start);
            AddVisited(graph.Start, null);
            int loopIteration = 0;

            Vertex current = null;
            while (true)
            {
                ++loopIteration;
                current = queue.Dequeue();

                if (current.Equals(graph.End))
                {
                    break;
                }

                foreach (Vertex neighbour in current.GetOrderedNeighbours())
                {
                    if (!Visited(neighbour))
                    {
                        queue.Enqueue(neighbour);
                        AddVisited(neighbour, current);

                    }
                }
            }

            _resultPath.Clear();
            current = graph.End;

            while (current != null)
            {
                _resultPath.Enqueue(current);
                current = _visited[current];
            }

            _stopwatch.Stop();
            OnSolved?.Invoke();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
