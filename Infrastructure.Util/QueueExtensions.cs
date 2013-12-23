using System.Collections.Generic;

namespace Infrastructure.Util
{
    public static class QueueExtensions
    {
        public static void Enqueue<T>(this Queue<T> queue, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                queue.Enqueue(item);
            }
        }

        public static IEnumerable<T> DequeueAll<T>(this Queue<T> queue)
        {
            while (queue.Count != 0)
            {
                yield return queue.Dequeue();
            }
        }
    }
}