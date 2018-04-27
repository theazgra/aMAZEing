using aMaze_ingSolver.GraphUtils;
using aMaze_ingSolver.Parallelism;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace aMaze_ingSolver.Algorithms
{
    class DepthFirst : MazeSolver
    {
        public override bool SupportParallel => false;
        private Dictionary<Vertex, Vertex> _visited;
        private SimpleSemaphore _semaphore;
        private object _lock = new object();
        private object _taskLock = new object();

        private Vertex _end;
        private List<Task> _tasks;


        public override string Name => "Depth first";
        public override event Solved OnSolved;

        public DepthFirst()
        {
            _visited = new Dictionary<Vertex, Vertex>();
            _tasks = new List<Task>();
        }

        private bool Visited(Vertex vertex)
        {
            lock (_lock)
            {
                return _visited.ContainsKey(vertex);
            }
        }

        private void AddVisited(Vertex vertex, Vertex previous)
        {
            lock (_lock)
            {
                if (!_visited.ContainsKey(vertex))
                    _visited.Add(vertex, previous);
            }
        }

        public override void SolveMaze(Graph graph)
        {
            if (!Parallel)
            {
                NonParallelSolution(graph);
            }
            else
            {
                _visited.Clear();

                _end = graph.End;
                ParallelSolution(graph);
            }
        }

        private void ParallelSolution(Graph graph)
        {

            _semaphore = new SimpleSemaphore(ThreadCount);
            int token = _semaphore.GetToken();

            VertexParam vp = new VertexParam(null, graph.Start, token);
            _timer.Reset();
            _timer.Start();
            StartThread(vp);

            bool wait = true;
            while (wait)
            {
                List<Task> toRemove = new List<Task>();
                IEnumerable<Task> check;
                lock (_taskLock)
                {
                    check = _tasks.ToArray();
                }
                foreach (Task task in check)
                {
                    if (task.IsCompleted)
                    {
                        toRemove.Add(task);
                    }
                }

                lock (_taskLock)
                {
                    _tasks.RemoveAll(t => toRemove.Contains(t));
                    wait = _tasks.Count > 0;
                }
            }

            _resultPath.Clear();
            Vertex current = graph.End;

            while (current != null)
            {
                _resultPath.Enqueue(current);
                current = _visited[current];
            }

            _timer.Stop();
            OnSolved?.Invoke();
            System.Console.WriteLine("Finished");
        }

        private void StartThread(VertexParam param)
        {
            //Thread worker = new Thread(ThreadWork);
            //worker.Start(param);
            lock (_taskLock)
            {
                _tasks.Add(Task.Factory.StartNew(() => ThreadWork(param)));
            }
        }

        private void ThreadWork(object param)
        {
            if (param is VertexParam vertexParam)
            {
                Stack<Vertex> queue = new Stack<Vertex>();
                queue.Push(vertexParam.nextVertex);
                AddVisited(vertexParam.nextVertex, vertexParam.currentVertex);

                Vertex current = null;
                while (true)
                {
                    if (queue.Count <= 0)
                        break;

                    current = queue.Pop();

                    if (current.Equals(_end))
                        break;

                    foreach (Vertex neighbour in current.GetOrderedNeighbours())
                    {
                        if (!Visited(neighbour))
                        {
                            AddVisited(neighbour, current);

                            if (_semaphore.HasFreeToken())
                            {
                                int token = _semaphore.GetToken();
                                VertexParam vp = new VertexParam(current, neighbour, token);
                                StartThread(vp);
                            }
                            else
                            {
                                queue.Push(neighbour);
                            }
                        }
                    }
                }


                _semaphore.ReturnToken(vertexParam.threadToken);

            }
            else
            {
                throw new System.Exception("Thread is expectiong Vertex object as a parameter.");
            }
        }

        private void NonParallelSolution(Graph graph)
        {
            Stack<Vertex> queue = new Stack<Vertex>();
            _timer.Start();
            _visited.Clear();

            queue.Push(graph.Start);
            AddVisited(graph.Start, null);
            int loopIteration = 0;

            Vertex current = null;
            while (true)
            {
                ++loopIteration;
                current = queue.Pop();

                if (current.Equals(graph.End))
                {
                    break;
                }

                foreach (Vertex neighbour in current.GetOrderedNeighbours())
                {
                    if (!Visited(neighbour))
                    {
                        queue.Push(neighbour);
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

