using System.Collections.Generic;
using System.Diagnostics;
using aMaze_ingSolver.GraphUtils;
using aMaze_ingSolver.Parallelism;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace aMaze_ingSolver.Algorithms
{
    class BreadthFirst : MazeSolver
    {
        public override bool SupportParallel => true;
        private Dictionary<Vertex, Vertex> _visited;
        private SimpleSemaphore _semaphore;
        private object _lock = new object();
        private object _taskLock = new object();

        private Vertex _end;
        private List<Task> _tasks;


        public override string Name => "Breadth first";
        public override event Solved OnSolved;

        public BreadthFirst()
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
                if (previous == null)
                {
                    _visited.Add(vertex, previous);
                    return;
                }
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

            
            while (true)
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
                    if (_tasks.Count <= 0)
                        break;
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
            lock (_taskLock)
            {
                _tasks.Add(Task.Factory.StartNew(() => ThreadWork(param)));
            }
        }

        private void ThreadWork(object param)
        {
            if (param is VertexParam vertexParam)
            {
                Queue<Vertex> queue = new Queue<Vertex>();
                queue.Enqueue(vertexParam.nextVertex);
                AddVisited(vertexParam.nextVertex, vertexParam.currentVertex);

                Vertex current = null;
                while (true)
                {
                    if (queue.Count <= 0)
                        break;

                    current = queue.Dequeue();

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
                                queue.Enqueue(neighbour);
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
            Queue<Vertex> queue = new Queue<Vertex>();
            _timer.Start();
            _visited.Clear();

            queue.Enqueue(graph.Start);
            AddVisited(graph.Start, null);

            Vertex current = null;
            while (true)
            {
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
