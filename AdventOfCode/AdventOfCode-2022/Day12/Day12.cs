using Algorithms;
using System.Globalization;

namespace AdventOfCode_2022.Day12
{
    public class Day12 : AdventCalendarProblem<long>
    {
        private record Coord(int X, int Y);

        private static readonly List<(int dx, int dy)> NeighborsOffsetWithoutDiagonals
            = new List<(int, int)>
            {
                         (-1,  0),
                ( 0, -1),         ( 0,  1),
                         ( 1,  0)
            };

        private static Coord FindSpecificNode(string[] input, char node)
        {
            for (var i = 0; i < input.Length; i++)
            {
                for (var j = 0; j < input[i].Length; j++)
                {
                    var @char = input[i][j];
                    if (@char == node)
                    {
                        return new Coord(i, j);
                    }
                }
            }

            throw new InvalidOperationException();
        }

        private static Coord FindStart(string[] input) => FindSpecificNode(input, 'S');

        private static Coord FindEnd(string[] input) => FindSpecificNode(input, 'E');


        private static char GetHeight(Coord node, string[] input)
        {
            var (i, j) = node;
            return input[i][j] switch
            {
                'S' => 'a',
                'E' => 'z',
                var c => c
            };
        }

        private static IEnumerable<Coord> GetNeighbors(Coord currentNode, string[] input)
        {

            var currentHeight = GetHeight(currentNode, input);

            var neighborsInBound = NeighborsOffsetWithoutDiagonals
                .Select(n => new Coord(currentNode.X + n.dx, currentNode.Y + n.dy))
                .Where(n => IsInBound(n, input))
                .ToList();

            var validNeighbors = neighborsInBound
                .Where(n => GetHeight(n, input) - currentHeight <= 1).ToList();

            var validNeighborsHeight = validNeighbors.Select(n => GetHeight(n, input)).ToList();

            return validNeighbors;
        }

        private static bool IsInBound(Coord node, string[] input)
        {
            return 0 <= node.X && node.X < input.Length
                && 0 <= node.Y && node.Y < input[node.X].Length;
        }


        private double ManhattanDistance(Coord node, Coord goal)
        {
            var (nx, ny) = node;
            var (gx, gy) = goal;

            return Math.Abs(gx - nx) + Math.Abs(gy - ny);
        }


        protected override long Part1SampleResult => 31;
        protected override long SolvePart1(string[] input)
        {
            var start = FindStart(input);
            var end = FindEnd(input);

            var path = A_Star.FindShortestPath(
                start: start,
                isGoalReached: node => node == end,
                getNeighbors: node => GetNeighbors(node, input),
                cost: (src, dst) => 1d,
                costHeuristic: node => ManhattanDistance(node, end) * 0.1);

            //PrettyPrintPath(path, input);

            return path.LongCount() - 1;
        }

        private static void PrettyPrintPath(IEnumerable<Coord> path, string[] input)
        {
            var directions = path
                .Pairwise()
                .Select(x =>
                {
                    var (src, dst) = x;
                    var dX = dst.X - src.X;
                    var dY = dst.Y - src.Y;
            
                    var direction = (dX, dY) switch
                    {
                        (-1,  0) => '^',
                        ( 0, -1) => '<',
                        ( 0,  1) => '>',
                        ( 1,  0) => 'v'
                    };
            
                    return (src, direction);
                }).ToDictionary(x => x.src, x => x.direction);

            var height = input.Length;
            var width = input[0].Length;

            var end = FindEnd(input);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var coord = new Coord(i, j);

                    if (coord == end)
                    {
                        Console.Write('E');
                    }
                    else
                    {
                        if (directions.TryGetValue(coord, out var dir))
                        {
                            Console.Write(dir);
                        }
                        else
                        {
                            Console.Write('.');
                        }
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine();
        }

        protected override long Part2SampleResult => 29;
        protected override long SolvePart2(string[] input)
        {
            var possibleStarts = FindAllPossibleStart(input);

            var end = FindEnd(input);

            var minPath = long.MaxValue;

            foreach (var possibleStart in possibleStarts)
            {
                var path = A_Star.FindShortestPath(
                    start: possibleStart,
                    isGoalReached: node => node == end,
                    getNeighbors: node => GetNeighbors(node, input),
                    cost: (src, dst) => 1d,
                    costHeuristic: node => ManhattanDistance(node, end) * 0.1);

                if (path == null) { continue; }

                minPath = Math.Min(minPath, path.LongCount() - 1);
            }

            return minPath;
        }

        private static IEnumerable<Coord> FindAllPossibleStart(string[] input)
        {
            for (int x = 0; x < input.Length; x++)
            {
                for (int y = 0; y < input[x].Length; y++)
                {
                    var c = input[x][y];

                    if (c == 'a' || c == 'S')
                    {
                        yield return new Coord(x, y);
                    }
                }
            }
        }
    }
}
