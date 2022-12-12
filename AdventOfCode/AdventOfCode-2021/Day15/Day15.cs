using Algorithms;
using System.Collections;
using System.Collections.Generic;

namespace AdventOfCode_2021.Day15
{
    public class Day15 : AdventCalendarProblem<long>
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
                isGoalReached: n => n == lowerRight,
                getNeighbors: n => GetNeighborsWithoutDiagonals(n, input),
                cost: (src, dst) => Part1_Cost(src, dst, input),
                costHeuristic: n => ManhattanDistance(n, lowerRight) * 0.1);

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

        protected override long Part2SampleResult => 315;
        protected override long SolvePart2(string[] croppedMap)
        {
            throw new NotImplementedException();
        }
    }
}
