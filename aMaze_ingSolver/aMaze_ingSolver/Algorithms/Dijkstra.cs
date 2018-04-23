using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aMaze_ingSolver.GraphUtils;

namespace aMaze_ingSolver.Algorithms
{
    class Dijkstra : MazeSolver
    {
        public override string Name => "Dijkstra";
        public override event solved OnSolved;

        /// <summary>
        /// Path to vertex, tuple present previous vertex and best length
        /// </summary>
        private Dictionary<Vertex, Tuple<Vertex, float>> _pathToVertex;

        private float GetBestDistance(Vertex v)
        {
            if (!_pathToVertex.ContainsKey(v))
                return float.PositiveInfinity;

            return _pathToVertex[v].Item2;
        }

        private void AddNewBestDistance(Vertex previous, Vertex destination, float distance)
        {
            if (_pathToVertex.ContainsKey(destination))
                _pathToVertex[destination] = new Tuple<Vertex, float>(previous, distance);
            else
                _pathToVertex.Add(destination, new Tuple<Vertex, float>(previous, distance));
        }



        public override void SolveMaze(Graph graph)
        {
            graph.ResetVisited();
            _resultPath.Clear();
            _timer.Start();

            long visited = 0;
            _pathToVertex = new Dictionary<Vertex, Tuple<Vertex, float>>();
            _pathToVertex.Add(graph.Start, new Tuple<Vertex, float>(null, 0));

            Queue<Vertex> queue = new Queue<Vertex>();
            queue.Enqueue(graph.Start);

            Vertex currentVertex = null;
            while (visited != graph.Vertices.Count)
            {
                if (queue.Count <= 0)
                    break;
                currentVertex = queue.Dequeue();
                if (currentVertex.Visited)
                    continue;

                //Connected vertices, ordered by distance length { .OrderBy(n => currentVertex.DistanceTo(n)) } ??
                foreach (Vertex neighbour in currentVertex.Neighbours)
                {
                    //distance to connected vertex using best distance to current vertex.
                    float lenToVertex = GetBestDistance(currentVertex) + currentVertex.DistanceTo(neighbour);
                    if (lenToVertex < GetBestDistance(neighbour))
                    {
                        AddNewBestDistance(currentVertex, neighbour, lenToVertex);
                    }
                    queue.Enqueue(neighbour);
                }

                currentVertex.Visited = true;
                ++visited;
            }

            Vertex current = null;
            current = graph.End;
            _resultPath.Enqueue(current);
            while (current != graph.Start)
            {
                current = _pathToVertex[current].Item1;
                _resultPath.Enqueue(current);
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
