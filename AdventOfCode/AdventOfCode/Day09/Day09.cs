using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AdventOfCode.Day05
{
    // rewrote another coordinate because I forgot I made one day05
    // I should still install VS2022 and .NET 5+ and use record...
    [DebuggerDisplay("I: {I}, J: {J}")]
    public class Coord : IEquatable<Coord>
    {
        public int I { get; }
        public int J { get; }

        public Coord(int i, int j)
        {
            I = i;
            J = j;
        }

        public static Coord operator +(Coord a, Coord b)
        {
            return new Coord(a.I + b.I, a.J + b.J);
        }

        public override int GetHashCode() => HashCode.Combine(I,J);

        public override bool Equals(object obj)
        {
            if (obj is Coord c)
            {
                return Equals(c);
            }

            return false;
        }

        public bool Equals([AllowNull] Coord other)
        {
            return I == other?.I && J == other?.J;
        }
    }

    public class Day09 : AdventCalendarSolver
    {
        private static Coord[] NeighborsOffsetToCheck = new[]
        {
            new Coord(-1,  0),
            new Coord( 1,  0),
            new Coord( 0, -1),
            new Coord( 0,  1),
        };

        protected override long Part1SampleResult => 15;
        protected override long SolvePart1(string[] input)
        {
            long sumLowPoint = 0;
            for(var i = 0; i < input.Length; ++i)
            {
                for (var j = 0; j < input[i].Length; ++j)
                {
                    var potentialLowPoint = input[i][j];
                    var potentailLowPointCoord = new Coord(i, j);
                    var minNeighbor = char.MaxValue;

                    foreach (var neighbor in NeighborsOffsetToCheck)
                    {
                        var coord = potentailLowPointCoord + neighbor;
                        if (TryGetChar(coord, input, out var c))
                        {
                            minNeighbor = minNeighbor < c.Value ? minNeighbor : c.Value;
                        }
                    }

                    if (minNeighbor > potentialLowPoint)
                    {
                        sumLowPoint += (potentialLowPoint - '0' ) + 1;
                    }
                }
            }

            return sumLowPoint;
        }

        protected override long Part2SampleResult => 1134;
        protected override long SolvePart2(string[] input)
        {
            List<Coord> LowPointCoord = new List<Coord>();

            for(var i = 0; i < input.Length; ++i)
            {
                for (var j = 0; j < input[i].Length; ++j)
                {
                    var potentialLowPoint = input[i][j];
                    var potentailLowPointCoord = new Coord(i, j);
                    var minNeighbor = char.MaxValue;

                    foreach (var neighborOffset in NeighborsOffsetToCheck)
                    {
                        var neighbor = potentailLowPointCoord + neighborOffset;
                        if (TryGetChar(neighbor, input, out var c))
                        {
                            minNeighbor = minNeighbor < c.Value ? minNeighbor : c.Value;
                        }
                    }

                    if (minNeighbor > potentialLowPoint)
                    {
                        LowPointCoord.Add(potentailLowPointCoord);
                    }
                }
            }

            return GetBassinSize(input, LowPointCoord).OrderByDescending(x => x).Take(3).Aggregate<long, long>(1, (a, b) => a * b);
        }

        private bool TryGetChar(Coord c, string[] input, out char? @char)
        {
            if (0 <= c.I && c.I < input.Length
                && 0 <= c.J && c.J < input[c.I].Length)
            {
                @char = input[c.I][c.J];
                return true;
            }

            @char = null;
            return false;
        }

        private IEnumerable<long> GetBassinSize(string[] input, List<Coord> lowPointCoords)
        {
            var bassinsSize = new List<long>();

            foreach(var lowPoint in lowPointCoords)
            {
                var alreadyVisitedCoords = new HashSet<Coord>();
                var pointToTests = new Queue<Coord>();
                pointToTests.Enqueue(lowPoint);

                while(pointToTests.Any())
                {
                    var currentPoint = pointToTests.Dequeue();
                    alreadyVisitedCoords.Add(currentPoint);
                    var currentPointChar = input[currentPoint.I][currentPoint.J];

                    foreach(var neighborOffset in NeighborsOffsetToCheck)
                    {
                        var neighbor = neighborOffset + currentPoint;
                        if (!alreadyVisitedCoords.Contains(neighbor) && TryGetChar(neighbor, input, out var neighborCharValue))
                        {
                            if (neighborCharValue.Value != '9' && neighborCharValue.Value > currentPointChar)
                            {
                                pointToTests.Enqueue(neighbor);
                            }
                        }
                    }
                }

                bassinsSize.Add(alreadyVisitedCoords.LongCount());
            }

            return bassinsSize;
        }
    }
}
