namespace System.Collections.Generic;

public static partial class EnumerableExtensions
{
    public static IEnumerable<(T, T)> Pairwise<T>(this IEnumerable<T> source)
    {
        T previous = default;
        bool first = true;

        foreach (var item in source)
        {
            if (first)
            {
                first = false;
                previous = item;
                continue;
            }

            yield return (previous, item);
            previous = item;
        }
    }
}

