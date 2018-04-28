using System;
using System.Collections.Generic;
using System.Threading;

namespace aMaze_ingSolver.Parallelism
{
    class BlockingStack<T>
    {
        private readonly Stack<T> _stack = new Stack<T>();

        /// <summary>
        /// Number of elements in stack.
        /// </summary>
        public int Count
        {
            get
            {
                lock (_stack)
                {
                    return _stack.Count;
                }
            }
        }

        /// <summary>
        /// Push item into stack, wakes any waiting threads.
        /// </summary>
        /// <param name="item">Item to enqueue.</param>
        public void Push(T item)
        {
            lock (_stack)
            {
                _stack.Push(item);

                if (_stack.Count >= 1)
                {
                    Monitor.PulseAll(_stack);
                }
            }
        }

        /// <summary>
        /// Pop item from stack. If stack is empty, thread is blocked, until item is ready or cancellation is requested.
        /// </summary>
        /// <param name="cancellationToken">Token on which to interrupt waiting.</param>
        /// <returns>First item.</returns>
        public T Pop(CancellationToken cancellationToken)
        {
            return InternalPop(cancellationToken);
        }

        /// <summary>
        /// Pop item from stack. If stack is empty, thread is blocked.
        /// </summary>
        /// <returns>First item.</returns>
        public T Pop()
        {
            return InternalPop(CancellationToken.None);
        }

        private T InternalPop(CancellationToken cancellationToken)
        {
            if (cancellationToken != CancellationToken.None)
            {
                if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException(cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();
            }

            lock (_stack)
            {
                while (_stack.Count == 0)
                {
                    Monitor.Wait(_stack);
                }
                return _stack.Pop();
            }
        }
    }
}
