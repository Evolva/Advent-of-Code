using System;
using System.Linq;

namespace AdventOfCode.Day02
{
    public enum Direction
    {
        Forward,
        Down,
        Up
    }

    public class SubmarineCommand
    {
        public Direction Direction { get; }
        public int Distance { get; }

        public SubmarineCommand(Direction direction, int distance)
        {
            Direction = direction;
            Distance = distance;
        }
    }

    public class SubmarinePositionPart1
    {
        public static SubmarinePositionPart1 InitialPosition = new SubmarinePositionPart1(0, 0);

        public int HorizontalPosition { get; }
        public int VerticalPosition { get; }

        private SubmarinePositionPart1(int hPos, int vPos)
        {
            HorizontalPosition = hPos;
            VerticalPosition = vPos;
        }

        public SubmarinePositionPart1 Apply(SubmarineCommand command)
        {
            return command.Direction switch
            {
                Direction.Forward => new SubmarinePositionPart1(HorizontalPosition + command.Distance, VerticalPosition),
                Direction.Down => new SubmarinePositionPart1(HorizontalPosition, VerticalPosition + command.Distance),
                Direction.Up => new SubmarinePositionPart1(HorizontalPosition, VerticalPosition - command.Distance),
                _ => throw new ArgumentOutOfRangeException(nameof(command.Direction), command.Direction, null),
            };
        }
    }

    public class SubmarinePositionPart2
    {
        public static SubmarinePositionPart2 InitialPosition = new SubmarinePositionPart2(0, 0, 0);

        public int HorizontalPosition { get; }
        public int VerticalPosition { get; }
        public int Aim { get; }

        private SubmarinePositionPart2(int hPos, int vPos, int aim)
        {
            HorizontalPosition = hPos;
            VerticalPosition = vPos;
            Aim = aim;
        }

        public SubmarinePositionPart2 Apply(SubmarineCommand command)
        {
            return command.Direction switch
            {
                Direction.Down => new SubmarinePositionPart2(HorizontalPosition, VerticalPosition, Aim + command.Distance),
                Direction.Up => new SubmarinePositionPart2(HorizontalPosition, VerticalPosition, Aim - command.Distance),

                Direction.Forward => new SubmarinePositionPart2(HorizontalPosition + command.Distance, VerticalPosition + command.Distance * Aim, Aim),
                
                _ => throw new ArgumentOutOfRangeException(nameof(command.Direction), command.Direction, null),
            };
        }
    }

    public class Day02 : AdventCalendarSolver
    {
        protected override long Part1SampleResult => 150;

        protected override long SolvePart1(string[] input)
        {
            var finalPosition = input
                .Select(s =>
                {
                    var token = s.Split();

                    var direction = token[0] switch
                    {
                        "forward" => Direction.Forward,
                        "down" => Direction.Down,
                        "up" => Direction.Up,
                        _ => throw new ArgumentOutOfRangeException("direction token", token[0], null),
                    };

                    var distance = int.Parse(token[1]);

                    return new SubmarineCommand(direction, distance);
                })
                .Aggregate(SubmarinePositionPart1.InitialPosition, (pos, command) => pos.Apply(command));

            return finalPosition.HorizontalPosition * finalPosition.VerticalPosition;
        }


        protected override long Part2SampleResult => 900;
        protected override long SolvePart2(string[] input)
        {
            var finalPosition = input
                .Select(s =>
                {
                    var token = s.Split();

                    var direction = token[0] switch
                    {
                        "forward" => Direction.Forward,
                        "down" => Direction.Down,
                        "up" => Direction.Up,
                        _ => throw new ArgumentOutOfRangeException("direction token", token[0], null),
                    };

                    var distance = int.Parse(token[1]);

                    return new SubmarineCommand(direction, distance);
                })
                .Aggregate(SubmarinePositionPart2.InitialPosition, (pos, command) => pos.Apply(command));

            return finalPosition.HorizontalPosition * finalPosition.VerticalPosition;
        }
    }
}