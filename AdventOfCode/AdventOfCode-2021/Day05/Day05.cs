using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace AdventOfCode_2021.Day05
{
    public class Coordinate : IEquatable<Coordinate>
    {
        public int X { get; }
        public int Y { get; }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals([AllowNull] Coordinate other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Coordinate coord && coord != null)
            {
                return Equals(coord);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }

    public class Day05 : AdventCalendarProblem<long>
    {
        protected override long Part1SampleResult => 5;
        protected override long SolvePart1(string[] input)
        {
            var map = new Dictionary<Coordinate, int>();
            var regex = new Regex(@"(?<x1>\d+),(?<y1>\d+) -> (?<x2>\d+),(?<y2>\d+)");

            foreach (var line in input)
            {
                var match = regex.Match(line);
                var x1 = int.Parse(match.Groups["x1"].Value);
                var y1 = int.Parse(match.Groups["y1"].Value);
                var x2 = int.Parse(match.Groups["x2"].Value);
                var y2 = int.Parse(match.Groups["y2"].Value);

                if (x1 == x2)
                {
                    var x = x1;
                    var lower = Math.Min(y1, y2);
                    var upper = Math.Max(y1, y2);
                    for (var y = lower; y <= upper; y++)
                    {
                        var coord = new Coordinate(x, y);

                        if (map.TryGetValue(coord, out var numberOfCrossingLines))
                        {
                            map[coord] = numberOfCrossingLines + 1;
                        }
                        else
                        {
                            map[coord] = 1;
                        }
                    }
                }

                if (y1 == y2)
                {
                    var y = y1;
                    var lower = Math.Min(x1, x2);
                    var upper = Math.Max(x1, x2);
                    for (var x = lower; x <= upper; x++)
                    {
                        var coord = new Coordinate(x, y);

                        if (map.TryGetValue(coord, out var numberOfCrossingLines))
                        {
                            map[coord] = numberOfCrossingLines + 1;
                        }
                        else
                        {
                            map[coord] = 1;
                        }
                    }
                }
            }

            return map.Values.Where(v => v >= 2).Count();
        }

        protected override long Part2SampleResult => 12;
        protected override long SolvePart2(string[] input)
        {
            var map = new Dictionary<Coordinate, int>();
            var regex = new Regex(@"(?<x1>\d+),(?<y1>\d+) -> (?<x2>\d+),(?<y2>\d+)");

            foreach (var line in input)
            {
                var match = regex.Match(line);
                var x1 = int.Parse(match.Groups["x1"].Value);
                var y1 = int.Parse(match.Groups["y1"].Value);
                var x2 = int.Parse(match.Groups["x2"].Value);
                var y2 = int.Parse(match.Groups["y2"].Value);

                if (x1 == x2)
                {
                    var x = x1;
                    var lower = Math.Min(y1, y2);
                    var upper = Math.Max(y1, y2);
                    for (var y = lower; y <= upper; y++)
                    {
                        var coord = new Coordinate(x, y);

                        if (map.TryGetValue(coord, out var numberOfCrossingLines))
                        {
                            map[coord] = numberOfCrossingLines + 1;
                        }
                        else
                        {
                            map[coord] = 1;
                        }
                    }
                }

                else if (y1 == y2)
                {
                    var y = y1;
                    var lower = Math.Min(x1, x2);
                    var upper = Math.Max(x1, x2);
                    for (var x = lower; x <= upper; x++)
                    {
                        var coord = new Coordinate(x, y);

                        if (map.TryGetValue(coord, out var numberOfCrossingLines))
                        {
                            map[coord] = numberOfCrossingLines + 1;
                        }
                        else
                        {
                            map[coord] = 1;
                        }
                    }
                }

                else
                {
                    var lowerX = Math.Min(x1, x2);
                    var diffX = Math.Abs(x1 - x2);
                    var startingY = lowerX == x1 ? y1 : y2;
                    var endingY = lowerX == x1 ? y2 : y1;
                    var signY = endingY - startingY > 0 ? 1 : -1;

                    for (var i = 0; i <= diffX; i++)
                    {
                        var coord = new Coordinate(lowerX + i, startingY + i * signY);

                        if (map.TryGetValue(coord, out var numberOfCrossingLines))
                        {
                            map[coord] = numberOfCrossingLines + 1;
                        }
                        else
                        {
                            map[coord] = 1;
                        }
                    }
                }
            }

            return map.Values.Where(v => v >= 2).Count();
        }
    }
}
