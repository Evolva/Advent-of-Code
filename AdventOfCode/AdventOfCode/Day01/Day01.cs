using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day01
{
    public class Day01 : AdventCalendarSolver
    {
        public Day01() : base("Day01/input.txt") { }

        protected override int SolvePart1()
        {
            return FileContent
                .Select(int.Parse)
                .Pairwise()
                .Where(pair => pair.Item1 < pair.Item2)
                .Count();
        }

        protected override int SolvePart2()
        {
            return FileContent
                .Select(int.Parse)
                .SlidingWindow(3)
                .Pairwise()
                .Where(pair => pair.Item1.Sum() < pair.Item2.Sum())
                .Count();
        }
    }

    public static class EnumerableExtensions
    {
        public static IEnumerable<(T, T)> Pairwise<T>(this IEnumerable<T> source)
        {
            T previous = default;
            bool first = true;
            
            foreach(var item in source)
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
}
