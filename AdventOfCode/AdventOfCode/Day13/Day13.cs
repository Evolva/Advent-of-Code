using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day13
{
    public enum FoldAxis
    {
        X,
        Y
    }

    public class FoldingInstruction
    {
        public FoldAxis Axis { get; }
        public int Offset { get; }

        public FoldingInstruction(FoldAxis axis, int offset)
        {
            Axis = axis;
            Offset = offset;
        }

        internal (int, int) ApplyOn((int x, int y) dot)
        {
            if (Axis == FoldAxis.Y)
            {
                var newY = dot.y <= Offset
                    ? dot.y
                    : Offset - (dot.y-Offset) ;
                return (dot.x, newY);
            }
            else
            {
                var newX = dot.x <= Offset
                    ? dot.x
                    : Offset - (dot.x - Offset);
                return (newX, dot.y);
            }
        }
    }

    public class Origami
    {
        private static readonly Regex DotCoordRegex = new Regex(@"(?<x>\d+),(?<y>\d+)");
        private static readonly Regex FoldRegex = new Regex(@"fold along (?<axis>[xy])=(?<offset>\d+)");

        public static Origami ReadFromInput(string[] input)
        {
            var currentLine = 0;
            var dots = new HashSet<(int, int)>();
            var folds = new List<FoldingInstruction>();

            while(DotCoordRegex.Match(input[currentLine]) is var match && match.Success)
            {
                var x = int.Parse(match.Groups["x"].Value);
                var y = int.Parse(match.Groups["y"].Value);

                dots.Add((x, y));
                currentLine++;
            }
            currentLine++;

            while (currentLine < input.Length && FoldRegex.Match(input[currentLine]) is var match && match.Success)
            {
                var axis = match.Groups["axis"].Value switch
                {
                    "x" => FoldAxis.X,
                    "y" => FoldAxis.Y,
                    _   => throw new ArgumentOutOfRangeException(),
                };
                var offset = int.Parse(match.Groups["offset"].Value);

                folds.Add(new FoldingInstruction(axis, offset));
                currentLine++;
            }

            return new Origami(dots, folds);
        }

        public HashSet<(int, int)> Dots;
        public IEnumerable<FoldingInstruction> FoldingInstructions;

        private Origami(HashSet<(int, int)> dots, IEnumerable<FoldingInstruction> foldingInstructions)
        {
            Dots = dots;
            FoldingInstructions = foldingInstructions;
        }

        public Origami Fold()
        {
            var instruction = FoldingInstructions.First();
            var newDots = new HashSet<(int, int)>();
            var remainingFolds = FoldingInstructions.Skip(1);

            foreach(var dot in Dots)
            {
                newDots.Add(instruction.ApplyOn(dot));
            }

            return new Origami(newDots, remainingFolds);
        }
    }

    public class Day13 : AdventCalendarSolver
    {
        protected override long Part1SampleResult => 17;
        protected override long SolvePart1(string[] input)
        {
            var foldedOnce = Origami.ReadFromInput(input).Fold();

            return foldedOnce.Dots.LongCount();
        }

        protected override long Part2SampleResult => -42;
        protected override long SolvePart2(string[] input)
        {
            var origami = Origami.ReadFromInput(input);

            while(origami.FoldingInstructions.Any())
            {
                origami = origami.Fold();
            }

            var maxY = origami.Dots.Select(d => d.Item2).Max();
            var maxX = origami.Dots.Select(d => d.Item1).Max();

            for (int y = 0; y <= maxY; y++)
            {
                for (int x = 0; x <= maxX; x++)
                {
                    Console.Write(origami.Dots.Contains((x, y)) ? "#" : ".");
                }
                Console.WriteLine();
            }



            return -42;
        }
    }
}