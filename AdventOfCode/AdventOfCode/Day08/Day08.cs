using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode.Day08
{
    public class Day08 : AdventCalendarSolver
    {
        protected override long Part1SampleResult => 26;
        protected override long SolvePart1(string[] input)
        {
            HashSet<int> outputValueLenght = new HashSet<int> { 2, 3, 4, 7 };

            var result = 0;

            foreach (var line in input)
            {
                var outputValues = line.Split('|')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                result += outputValues.Count(str => outputValueLenght.Contains(str.Length));
            }

            return result;
        }

        protected override long Part2SampleResult => 61_229;
        protected override long SolvePart2(string[] input)
        {
            var sevenSegmentSolver = new BadlyWiredSevenSegmentDisplaySolver();

            return input.Select(sevenSegmentSolver.Solve).Sum();
        }
    }

    public class BadlyWiredSevenSegmentDisplaySolver
    {
        private static readonly ImmutableHashSet<char> allPossibleSegment =
            ImmutableHashSet<char>.Empty.Union(new[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g' });

        private static readonly IReadOnlyDictionary<string, int> SegmentsToNumber = new Dictionary<string, int>
        {
            ["abcefg"] = 0,
            ["cf"]     = 1,
            ["acdeg"]  = 2,
            ["acdfg"]  = 3,
            ["bcdf"]   = 4,
            ["abdfg"]  = 5,
            ["abdefg"] = 6,
            ["acf"]    = 7,
            ["abcdefg"]= 8,
            ["abcdfg"] = 9,
        };

        public long Solve(string line)
        {
            var inputSignalToOutputSegment = new Dictionary<char, ISet<char>>();

            var splittedLine = line.Split('|');
            var inputSignalValues = splittedLine[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);

            string mapTo1 = null; //2 input signals
            string mapTo4 = null; //4 input signals
            string mapTo7 = null; //3 input signals
            string mapTo8 = null; //7 input signals
            List<string> canMapTo_0_6_9 = new List<string>(3); //6 input signals
            List<string> canMapTo_2_3_5 = new List<string>(3); //5 input signals

            foreach (var inputSignal in inputSignalValues)
            {
                switch (inputSignal.Length)
                {
                    case 2:
                        mapTo1 = inputSignal;
                        break;
                    case 4:
                        mapTo4 = inputSignal;
                        break;
                    case 3:
                        mapTo7 = inputSignal;
                        break;
                    case 7:
                        mapTo8 = inputSignal;
                        break;
                    case 6:
                        canMapTo_0_6_9.Add(inputSignal);
                        break;
                    case 5:
                        canMapTo_2_3_5.Add(inputSignal);
                        break;
                }
            }

            var inputThatMapToSegmentA = mapTo7.Except(mapTo1).Single();
            inputSignalToOutputSegment[inputThatMapToSegmentA] = new HashSet<char> { 'a' };

            var inputThatCanMapToEither_C_or_F = mapTo7.Intersect(mapTo1);
            foreach(var input in inputThatCanMapToEither_C_or_F)
            {
                inputSignalToOutputSegment[input] = new HashSet<char> { 'c', 'f' };
            }

            var inputThatCanMapToEither_B_or_D = mapTo4.Except(mapTo1);
            foreach (var input in inputThatCanMapToEither_B_or_D)
            {
                inputSignalToOutputSegment[input] = new HashSet<char> { 'b', 'd' };
            }

            var inputThatCanMapToEither_E_or_G = allPossibleSegment.Except(inputSignalToOutputSegment.Keys);
            foreach (var input in inputThatCanMapToEither_E_or_G)
            {
                inputSignalToOutputSegment[input] = new HashSet<char> { 'e', 'g' };
            }

            //input signal segment only present twice between 0, 6 and 9 can only mapped to either 'c', 'd', 'e'
            var segmentsOnlyPresentTwiceIn_0_6_9 = canMapTo_0_6_9.SelectMany(c => c).GroupBy(c => c).Where(g => g.Count() == 2).Select(g => g.Key);
            var C_D_E = new HashSet<char> { 'c', 'd', 'e' };
            foreach (var input in segmentsOnlyPresentTwiceIn_0_6_9)
            {
                var singlePossibleChoice = C_D_E.Intersect(inputSignalToOutputSegment[input]).Single();
                inputSignalToOutputSegment[input] = new HashSet<char> { singlePossibleChoice };

                foreach(var kvp in inputSignalToOutputSegment)
                {
                    if (kvp.Key != input && kvp.Value.Contains(singlePossibleChoice))
                    {
                        kvp.Value.Remove(singlePossibleChoice);
                    }
                }
            }

            //At this point all input signal should map to 1 output segment
            var inputSignalWithMoreThanOnePossibleOutputSegment = inputSignalToOutputSegment.Where(kvp => kvp.Value.Count > 1).ToList();
            if (inputSignalWithMoreThanOnePossibleOutputSegment.Any())
            {
                throw new InvalidOperationException("At least on input signal still map to more than one output segment");
            }

            var outputValues = splittedLine[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var result = 0;

            foreach(var outputValue in outputValues)
            {
                var segments =
                    new string(outputValue.Select(c => inputSignalToOutputSegment[c].Single()).OrderBy(c => c).ToArray());

                var number = SegmentsToNumber[segments];

                result = result * 10 + number;
            }


            return result;
        }
    }
}
