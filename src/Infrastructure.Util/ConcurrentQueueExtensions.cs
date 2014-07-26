using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Infrastructure.Util
{
    public static class ConcurrentQueueExtensions
    {
        public static IEnumerable<T> DequeueAllCurrent<T>(this ConcurrentQueue<T> concurrentQueue)
        {
            while (true)
            {
                T item;
                var success = concurrentQueue.TryDequeue(out item);

                if (!success)
                {
                    yield break;
                }

                yield return item;
            }
        }
    }
}