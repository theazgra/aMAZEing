﻿using System;
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

        private List<Vertex> _unvisited;
        private bool _invokeEvents = false;

        private float GetBestDistance(Vertex v)
        {
            return v.BestPathDistance;
        }

        private void AddNewBestDistance(Vertex previous, Vertex current, float distance)
        {
            current.Previous = previous;
            current.BestPathDistance = distance;

            if (!_unvisited.Contains(current))
            {
                _unvisited.Add(current);
            }

            _unvisited = _unvisited.OrderBy(v => v.BestPathDistance).ToList();
        }

        private Vertex GetTopUnvisitedVertex()
        {
            return _unvisited.First();
        }


        public override void SolveMaze(Graph graph)
        {
            _invokeEvents = MazeForm.InvokeDelegates();
            long loopIteration = 0;
            int vertCount = graph.Vertices.Count;
            int invokeStep = (int)Math.Ceiling((float)(vertCount / 2000));
            graph.ResetVisited();
            _resultPath.Clear();

            _timer.Start();

            _unvisited = new List<Vertex>();
            AddNewBestDistance(null, graph.Start, 0);
            Vertex current = null;

            //Start by adding start neighbours
            foreach (Vertex neighbours in graph.Start.Neighbours)
            {
                AddNewBestDistance(graph.Start, neighbours, float.PositiveInfinity);
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
                _unvisited.Remove(current);

                ++loopIteration;
                if (_invokeEvents && iteration >= invokeStep)
                {
                    iteration = 0;
                    float perc = ((float)loopIteration / (float)vertCount) * 100.0f;
                    OnSolveProgress?.Invoke(perc);
                }
            }

            current = graph.End;
            _resultPath.Enqueue(current);
            while (current != graph.Start)
            {
                current = current.Previous;
                //current = _pathToVertex[current].Item1;
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
