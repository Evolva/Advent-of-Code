using System.Diagnostics;
using System.Numerics;

namespace Algorithms
{
    public static class A_Star
    {
        public record Origin<TNode, TCost>(TNode? FromNode, TCost WithCost)
            where TNode : notnull
            where TCost : INumberBase<TCost>, IComparisonOperators<TCost, TCost, bool>;

        public static IEnumerable<TNode>? FindShortestPath<TNode, TCost>(
            TNode start,
            Predicate<TNode> isGoalReached,
            Func<TNode, IEnumerable<TNode>> getNeighbors,
            Func<TNode, TNode, TCost> cost,
            Func<TNode, TCost> costHeuristic)

            where TNode : notnull
            where TCost : INumberBase<TCost>, IComparisonOperators<TCost, TCost, bool>
        {
            var visitedFromAndCost = new Dictionary<TNode, Origin<TNode, TCost>>();
            var toVisit = new PriorityQueue<TNode, TCost>();

            toVisit.Enqueue(start, TCost.Zero);
            visitedFromAndCost.Add(start, new Origin<TNode, TCost>(default, TCost.Zero));

            while (toVisit.TryDequeue(out var current, out var currentCost))
            {
                if (isGoalReached(current))
                {
                    var bestPath = new Stack<TNode>();

                    while (!current.Equals(start))
                    {
                        bestPath.Push(current);
                        current = visitedFromAndCost[current].FromNode;
                    }
                    bestPath.Push(current);
                    return bestPath;
                }

                var neighboursWithCost = getNeighbors(current)
                    .Select(n => new { Neighbour = n, NewCost = visitedFromAndCost[current].WithCost + cost(current, n) })
                    .Where(x =>
                           (!visitedFromAndCost.ContainsKey(x.Neighbour))
                        || x.NewCost < visitedFromAndCost[x.Neighbour].WithCost)
                    .ToList();

                foreach (var x in neighboursWithCost)
                {
                    var priority = x.NewCost + costHeuristic(x.Neighbour);
                    toVisit.Enqueue(x.Neighbour, priority);
                    visitedFromAndCost[x.Neighbour] = new Origin<TNode, TCost>(current, x.NewCost);
                }
            }

            return null;
        }

        public static (IEnumerable<TNode>? path, Dictionary<TNode, Origin<TNode, TCost>>? visitedFromAndCost) FindShortestPathWithDebug<TNode, TCost>(
            TNode start,
            Predicate<TNode> isGoalReached,
            Func<TNode, IEnumerable<TNode>> getNeighbors,
            Func<TNode, TNode, TCost> cost,
            Func<TNode, TCost> costHeuristic)

            where TNode : notnull
            where TCost : INumberBase<TCost>, IComparisonOperators<TCost, TCost, bool>
        {
            var visitedFromAndCost = new Dictionary<TNode, Origin<TNode, TCost>>();
            var toVisit = new PriorityQueue<TNode, TCost>();

            toVisit.Enqueue(start, TCost.Zero);
            visitedFromAndCost.Add(start, new Origin<TNode, TCost>(default, TCost.Zero));

            while (toVisit.TryDequeue(out var current, out var currentCost))
            {
                if (isGoalReached(current))
                {
                    var bestPath = new Stack<TNode>();

                    while (!current.Equals(start))
                    {
                        bestPath.Push(current);
                        current = visitedFromAndCost[current].FromNode;
                    }
                    bestPath.Push(current);
                    return (bestPath, null);
                }

                var neighboursWithCost = getNeighbors(current)
                    .Select(n => new { Neighbour = n, NewCost = visitedFromAndCost[current].WithCost + cost(current, n) })
                    .Where(x =>
                           (!visitedFromAndCost.ContainsKey(x.Neighbour))
                        || x.NewCost < visitedFromAndCost[x.Neighbour].WithCost)
                    .ToList();

                foreach (var x in neighboursWithCost)
                {
                    var priority = x.NewCost + costHeuristic(x.Neighbour);
                    toVisit.Enqueue(x.Neighbour, priority);
                    visitedFromAndCost[x.Neighbour] = new Origin<TNode, TCost>(current, x.NewCost);
                }
            }

            return (null, visitedFromAndCost);
        }
    }
}