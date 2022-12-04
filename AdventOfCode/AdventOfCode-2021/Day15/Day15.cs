using System.Collections;

namespace AdventOfCode_2021.Day15
{
    public class Day15 : AdventCalendarSolver
    {
        private static readonly List<(int, int)> NeighborsOffsetWithoutDiagonals
            = new List<(int, int)>
            {
                         (-1,  0),
                ( 0, -1),         ( 0,  1),
                         ( 1,  0)
            };

        protected override long Part1SampleResult => 40;
        protected override long SolvePart1(string[] input)
        {
            var lowerRight = (input.Length - 1, input[input.Length - 1].Length - 1);
            var costLowerRight = input[lowerRight.Item1][lowerRight.Item2] - '0';

            var path = A_Star.FindShortestPath(
                start: (0, 0),
                isGoal: n => n == lowerRight,
                getNeighbors: n => GetNeighborsWithoutDiagonals(n, input),
                cost: (src, dst) => Part1_Cost(src, dst, input),
                heuristic: n => ManhattanDistance(n, lowerRight) * 0.1);

            var pathAsList = path.Reverse().Select(x => new { node = x, cost = Part1_Cost(default, x, input) }).ToList();

            var result = pathAsList.Skip(1).Select(n => n.cost).Sum();
            return result;
        }

        private IEnumerable<(int, int)> GetNeighborsWithoutDiagonals((int, int) node, string[] input)
        {
            var (x, y) = node;

            foreach (var (dx, dy) in NeighborsOffsetWithoutDiagonals)
            {
                var potentialNeighbor = (x + dx, y + dy);

                if (IsInBound(potentialNeighbor, input))
                {
                    yield return potentialNeighbor;
                }
            }
        }

        private bool IsInBound((int, int) node, string[] input)
        {
            var (x, y) = node;
            return 0 <= x && x < input.Length
                && 0 <= y && y < input[x].Length;
        }

        private long Part1_Cost((int, int) _, (int, int) dst, string[] input)
        {
            return input[dst.Item1][dst.Item2] - '0';
        }

        private double ManhattanDistance((int, int) node, (int, int) goal)
        {
            var (nx, ny) = node;
            var (gx, gy) = goal;

            return Math.Abs(gx - nx) + Math.Abs(gy - ny);
        }

        private double EuclideanDistance((int, int) node, (int, int) goal)
        {
            var (nx, ny) = node;
            var (gx, gy) = goal;

            return Math.Sqrt(
                  Math.Pow(gx - nx, 2)
                + Math.Pow(gy - ny, 2));
        }

        protected override long Part2SampleResult => 315;
        protected override long SolvePart2(string[] croppedMap)
        {
            var input = StrechMap(croppedMap);
            return SolvePart1(input);
        }

        private string[] StrechMap(string[] input)
        {
            var resultMap = new string[input.Length * 5];
            //TODO
            return resultMap;
        }
    }

    public static class A_Star
    {
        public static IEnumerable<TNode> FindShortestPath<TNode>(
            TNode start,
            Predicate<TNode> isGoal,
            Func<TNode, IEnumerable<TNode>> getNeighbors,
            Func<TNode, TNode, double> cost,
            Func<TNode, double> heuristic)
        {
            //                                      node    parentNode, cost
            var visitedFromAndCost = new Dictionary<TNode, (TNode, double)>();
            var toVisit = new LazyManPriorityQueue<double, TNode>();

            toVisit.Enqueue(0, start);
            visitedFromAndCost.Add(start, (default, 0));

            var bestPath = new Queue<TNode>();

            while (toVisit.Any())
            {
                var (currentCost, current) = toVisit.Dequeue();

                if (isGoal(current))
                {
                    while (!current.Equals(start))
                    {
                        bestPath.Enqueue(current);
                        current = visitedFromAndCost[current].Item1;
                    }
                    bestPath.Enqueue(current);
                    break;
                }

                var neighboursWithCost = getNeighbors(current)
                    .Select(n => new { Neighbour = n, NewCost = visitedFromAndCost[current].Item2 + cost(current, n) })
                    .Where(x =>
                        !visitedFromAndCost.ContainsKey(x.Neighbour)
                        || x.NewCost < visitedFromAndCost[x.Neighbour].Item2)
                    .ToList();

                foreach (var x in neighboursWithCost)
                {
                    var priority = x.NewCost + heuristic(x.Neighbour);
                    toVisit.Enqueue(priority, x.Neighbour);
                    visitedFromAndCost[x.Neighbour] = (current, x.NewCost);
                }
            }
            return bestPath;
        }
    }


    //.NET 6 introduced PriorityQueue<TElement,TPriority>
    //but at this point where is the fun in upgrading ?
    public class LazyManPriorityQueue<TElement, TPriority>
        : IEnumerable<TElement>
    {
        private readonly SortedDictionary<TPriority, Queue<TElement>> _priorityQueue;
        public LazyManPriorityQueue()
        {
            _priorityQueue = new SortedDictionary<TPriority, Queue<TElement>>();
        }

        public void Enqueue(TElement element, TPriority priority)
        {
            if (!_priorityQueue.TryGetValue(priority, out var queue))
            {
                queue = new Queue<TElement>();
                _priorityQueue.Add(priority, queue);
            }
            queue.Enqueue(element);
        }

        public (TElement, TPriority) Dequeue()
        {
            var (priority, queue) = _priorityQueue.FirstOrDefault();

            var result = queue.Dequeue();
            if (queue.Count == 0)
            {
                _priorityQueue.Remove(priority);
            }

            return (result, priority);
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            return _priorityQueue.Values.SelectMany(x => x).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _priorityQueue.Values.SelectMany(x => x).GetEnumerator();
        }
    }
}