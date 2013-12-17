using System.Collections.Generic;

namespace Infrastructure.Util
{
    public static class QueueExtensions
    {
        public static void Enqueue<T>(this Queue<T> queue, IEnumerable<T> items)
        {
            items.ForEach(queue.Enqueue);
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