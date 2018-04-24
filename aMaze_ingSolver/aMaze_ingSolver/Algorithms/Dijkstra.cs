using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aMaze_ingSolver.GraphUtils;

namespace aMaze_ingSolver.Algorithms
{
    class Dijkstra : MazeSolver
    {
        public override string Name => "Dijkstra";
        public override event Solved OnSolved;
        public override event SolveProgress OnSolveProgress;

        /// <summary>
        /// Path to vertex, tuple present previous vertex and best length
        /// </summary>
        private Dictionary<Vertex, Tuple<Vertex, float>> _pathToVertex;

        private SortedDictionary<Vertex, Tuple<Vertex, float>> _sorted;
        SortedList<float, Vertex> _sl;

        

        private bool _invokeEvents = false;

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

        private Vertex GetTopUnvisitedVertex()
        {
            Vertex key = _pathToVertex.Keys.Where(v => !v.Visited).OrderBy(v => _pathToVertex[v].Item2).First();
            return key;
        }


        public override void SolveMaze(Graph graph)
        {
            _invokeEvents = MazeForm.InvokeDelegates();
            long visited = 0;
            int vertCount = graph.Vertices.Count;
            int invokeStep = (int)Math.Floor((float)(vertCount / 2000));
            graph.ResetVisited();
            _resultPath.Clear();

            _sl = new SortedList<float, Vertex>();
            

            _timer.Start();
            
            _pathToVertex = new Dictionary<Vertex, Tuple<Vertex, float>>();
            _pathToVertex.Add(graph.Start, new Tuple<Vertex, float>(null, 0));
            Vertex current = null;

            #region FixedImplementation
            //Start by adding start neighbours
            foreach (Vertex neighbours in graph.Start.Neighbours)
            {
                _pathToVertex.Add(neighbours, new Tuple<Vertex, float>(graph.Start, float.PositiveInfinity));
            }

            int iteration = 0;
            //loop while end is not visited.
            while (!graph.End.Visited)
            {
                ++iteration;
                current = GetTopUnvisitedVertex();
                foreach (Vertex neighbour in current.Neighbours)
                {
                    if (neighbour.Visited)
                        continue;

                    //distance to connected vertex using best distance to current vertex.
                    float lenToVertex = GetBestDistance(current) + current.PathDistanceTo(neighbour);
                    if (lenToVertex < GetBestDistance(neighbour))
                    {
                        AddNewBestDistance(current, neighbour, lenToVertex);
                    }
                }
                current.Visited = true;
                ++visited;
                if (_invokeEvents && iteration >= invokeStep)
                {
                    iteration = 0;
                    float perc = ((float)visited / (float)vertCount) * 100.0f;
                    OnSolveProgress?.Invoke(perc);
                }
            }


            #endregion

#if false
            

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
                    float lenToVertex = GetBestDistance(currentVertex) + currentVertex.PathDistanceTo(neighbour);
                    if (lenToVertex < GetBestDistance(neighbour))
                    {
                        AddNewBestDistance(currentVertex, neighbour, lenToVertex);
                    }
                    queue.Enqueue(neighbour);
                }

                currentVertex.Visited = true;
                ++visited;
            }
#endif

            
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
