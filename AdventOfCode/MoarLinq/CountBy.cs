namespace System.Collections.Generic;

public static partial class EnumerableExtensions
{
    public static Dictionary<V, long> CountBy<T, V>(this IEnumerable<T> source, Func<T, V> selector)
        where V : notnull
    {
        var result = new Dictionary<V, long>();

        foreach (var elt in source)
        {
            var key = selector(elt);

            if (result.TryGetValue(key, out var count))
            {
                result[key] = count + 1;
            }
            else
            {
                result[key] = 1;
            }
        }

        return result;
    }
}

