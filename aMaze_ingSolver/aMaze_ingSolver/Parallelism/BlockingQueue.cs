using System;
using System.Collections.Generic;
using System.Threading;

namespace aMaze_ingSolver.Parallelism
{
    class BlockingQueue<T> 
    {
        private readonly Queue<T> _queue = new Queue<T>();

        /// <summary>
        /// Number of items in queue.
        /// </summary>
        public int Count
        {
            get
            {
                lock (_queue)
                {
                    return _queue.Count;
                }
            }
        }

        /// <summary>
        /// Enqueue item into queue, wakes any waiting threads.
        /// </summary>
        /// <param name="item">Item to enqueue.</param>
        public void Enqueue(T item)
        {
            lock (_queue)
            {
                _queue.Enqueue(item);

                if (_queue.Count >= 1)
                {
                    Monitor.PulseAll(_queue);
                }
            }
        }

        /// <summary>
        /// Dequeue item from queue. If queue is empty, thread is blocked, until item is ready or cancellation is requested.
        /// </summary>
        /// <param name="cancellationToken">Token on which to interrupt waiting.</param>
        /// <returns>First item.</returns>
        public T Dequeue(CancellationToken cancellationToken)
        {
            return InternalDequeue(cancellationToken);
        }

        /// <summary>
        /// Dequeue item from queue. If queue is empty, thread is blocked.
        /// </summary>
        /// <returns>First item.</returns>
        public T Dequeue()
        {
            return InternalDequeue(CancellationToken.None);
        }

        /// <summary>
        /// Internal dequeue, work with cancellation token.
        /// </summary>
        /// <param name="cancellationToken">Token on which to cancel.</param>
        /// <returns>Dequeued item.</returns>
        private T InternalDequeue(CancellationToken cancellationToken)
        {
            if (cancellationToken != CancellationToken.None)
            {
                if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException(cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();
            }

            lock (_queue)
            {
                while (_queue.Count == 0)
                {
                    Monitor.Wait(_queue);
                }
                return _queue.Dequeue();
            }
        }
    }
}
