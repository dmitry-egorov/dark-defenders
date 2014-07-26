using System.Collections;
using System.Collections.Generic;

namespace Infrastructure.Util
{
    /// <summary>
    /// Decorator of <see cref="IEnumerator{T}"/> with additional capabilities.
    /// </summary>
    /// <typeparam name="T">Type of the element</typeparam>
    public class Iterator<T>: IEnumerator<T>
    {
        private readonly IEnumerator<T> _enumerator;

        /// <summary>
        /// Gets the current element in the collection.
        /// </summary>
        /// <returns>
        /// The current element in the collection.
        /// </returns>
        object IEnumerator.Current
        {
            get { return Current; }
        }

        /// <summary>
        /// Gets the element in the collection at the current position of the iterator.
        /// </summary>
        /// <returns>
        /// The element in the collection at the current position of the iterator.
        /// </returns>
        public T Current
        {
            get { return _enumerator.Current; }
        }

        /// <summary>
        /// Current element position in the collection.
        /// </summary>
        public long Position { get; private set; }

        /// <summary>
        /// Iterator passed the end of the collection.
        /// </summary>
        public bool IsFinished { get { return !IsNotFinished; } }

        /// <summary>
        /// Iterator didn't pass the end of the collection.
        /// </summary>
        public bool IsNotFinished { get; private set; }

        public Iterator(IEnumerator<T> enumerator)
        {
            Position = -1;
            _enumerator = enumerator;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _enumerator.Dispose();
        }

        /// <summary>
        /// Advances the iterator to the next element of the collection.
        /// </summary>
        /// <returns>
        /// True if the <see cref="Iterator{T}"/> was successfully advanced to the next element; false if the <see cref="Iterator{T}"/> has passed the end of the collection.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        public bool MoveNext()
        {
            var isNotFinished = _enumerator.MoveNext();

            IsNotFinished = isNotFinished;

            Position++;

            return isNotFinished;
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element in the collection.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception><filterpriority>2</filterpriority>
        public void Reset()
        {
            Position = -1;
            _enumerator.Reset();
        }
    }
}