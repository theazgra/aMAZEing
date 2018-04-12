using System.Collections.Generic;
using System.Diagnostics;
using aMaze_ingSolver.GraphUtils;

namespace aMaze_ingSolver.Algorithms
{
    class DepthFirst : MazeSolver
    {
        
        private Dictionary<Vertex, Vertex> _visited;

        public override string Name => "Depth first";
        public override event solved OnSolved;

        public DepthFirst()
        {
            _visited = new Dictionary<Vertex, Vertex>();
        }

        private bool Visited(Vertex vertex)
        {
            return _visited.ContainsKey(vertex);
        }

        private void AddVisited(Vertex vertex, Vertex previous)
        {
            _visited.Add(vertex, previous);
        }

        private Vertex GetPrevious(Vertex vertex)
        {
            if (!_visited.ContainsKey(vertex))
                return null;

            return _visited[vertex];
        }

        public override void SolveMaze(Graph graph)
        {

            List<Vertex> previous = new List<Vertex>();
            Queue<Vertex> queue = new Queue<Vertex>();
            _timer.Start();

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

            _timer.Stop();
            OnSolved?.Invoke();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
