using static System.Formats.Asn1.AsnWriter;

namespace AdventOfCode_2022.Day09
{
    public class Day09 : AdventCalendarProblem<long>
    {
        private record Coord(int Horizontal, int Vertical);

        private enum Direction
        {
            Right,
            Up,
            Left,
            Down
        }

        private record Motion(Direction Direction, int Steps);

        private class RopeBridge
        {
            private readonly HashSet<Coord> _coordsVisitedByTail;
            private readonly Coord[] _rope;

            public RopeBridge(int numberOfKnot)
            {
                _coordsVisitedByTail = new HashSet<Coord>();

                _rope = Enumerable.Repeat(new Coord(0, 0), numberOfKnot).ToArray();
            }

            public long Simulate(string[] inputs)
            {
                foreach(var input in inputs)
                {
                    var motion = ParseMotion(input);

                    while (motion.Steps > 0)
                    {
                        _rope[0] = MoveHead(_rope[0], motion.Direction);

                        for(var i = 1; i < _rope.Length; i++)
                        {
                            _rope[i] = MakeNextKnotFollow(_rope[i-1], _rope[i]);
                        }
                        _coordsVisitedByTail.Add(_rope.Last());

                        motion = motion with { Steps = motion.Steps - 1 };
                    }
                }

                return _coordsVisitedByTail.LongCount();
            }

            private Motion ParseMotion(string m)
            {
                var s = m.Split(' ');

                var direction = s[0] switch
                {
                    "R" => Direction.Right,
                    "U" => Direction.Up,
                    "L" => Direction.Left,
                    "D" => Direction.Down,
                    _ => throw new ArgumentOutOfRangeException("direction", s[0], null)
                };

                var steps = int.Parse(s[1]);

                return new Motion(direction, steps);
            }

            private static Coord MoveHead(Coord headCoord, Direction direction)
            {
                return direction switch
                {
                    Direction.Right => headCoord with { Horizontal = headCoord.Horizontal + 1 },
                    Direction.Up    => headCoord with { Vertical   = headCoord.Vertical   + 1 },
                    Direction.Left  => headCoord with { Horizontal = headCoord.Horizontal - 1 },
                    Direction.Down  => headCoord with { Vertical   = headCoord.Vertical   - 1 },
                    _               => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
                };
            }

            private static Coord MakeNextKnotFollow(Coord headCoord, Coord tailCoord)
            {
                var diffHorizontal = headCoord.Horizontal - tailCoord.Horizontal;
                var diffVertical   = headCoord.Vertical   - tailCoord.Vertical;

                return (diffHorizontal, diffVertical) switch
                {
                    (var h, var v) when Math.Abs(h) <= 1 && Math.Abs(v) <= 1 => tailCoord,

                    ( 2,  0) => tailCoord with { Horizontal = tailCoord.Horizontal + 1 },
                    (-2,  0) => tailCoord with { Horizontal = tailCoord.Horizontal - 1 },
                    ( 0,  2) => tailCoord with { Vertical   = tailCoord.Vertical   + 1 },
                    ( 0, -2) => tailCoord with { Vertical   = tailCoord.Vertical   - 1 },

                    ( 2,  1) or ( 1,  2) or ( 2,  2) => tailCoord with { Horizontal = tailCoord.Horizontal + 1, Vertical = tailCoord.Vertical + 1 },
                    (-1,  2) or (-2,  1) or (-2,  2) => tailCoord with { Horizontal = tailCoord.Horizontal - 1, Vertical = tailCoord.Vertical + 1 },
                    (-2, -1) or (-1, -2) or (-2, -2) => tailCoord with { Horizontal = tailCoord.Horizontal - 1, Vertical = tailCoord.Vertical - 1 },
                    ( 1, -2) or ( 2, -1) or ( 2, -2) => tailCoord with { Horizontal = tailCoord.Horizontal + 1, Vertical = tailCoord.Vertical - 1 },

                    _ => throw new InvalidOperationException()
                };
            }
        }

        protected override long Part1SampleResult => 13;
        protected override long SolvePart1(string[] input)
        {
            return new RopeBridge(2).Simulate(input);
        }

        protected override long Part2SampleResult => 36;
        protected override long SolvePart2(string[] input)
        {
            return new RopeBridge(10).Simulate(input);
        }
    }
}
