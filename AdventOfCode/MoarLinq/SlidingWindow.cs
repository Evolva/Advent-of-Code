namespace System.Collections.Generic;

public static partial class EnumerableExtensions
{
    public static IEnumerable<IEnumerable<T>> SlidingWindow<T>(this IEnumerable<T> source, int size)
    {
        var queue = new Queue<T>(size);

        foreach (var item in source)
        {
            if (queue.Count == size)
            {
                queue.Dequeue();
            }

            queue.Enqueue(item);

            if (queue.Count == size)
            {
                yield return queue.ToList();
            }
        }
    }
}