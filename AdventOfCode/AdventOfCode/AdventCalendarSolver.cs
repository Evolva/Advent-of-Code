using System;
using System.IO;

namespace AdventOfCode
{
    public abstract class AdventCalendarSolver
    {
        private readonly string[] InputFileContent;
        private readonly string[] SampleFileContent;

        protected AdventCalendarSolver()
        {
            InputFileContent = File.ReadAllLines($"{GetType().Name}/input.txt");
            SampleFileContent = File.ReadAllLines($"{GetType().Name}/sample.txt");
        }

        protected abstract int Part1SampleResult { get; }
        protected abstract int SolvePart1(string[] input);
        public void Part1()
        {
            var sampleResult = SolvePart1(SampleFileContent);
            if (sampleResult != Part1SampleResult)
            {
                throw new InvalidOperationException($"Bad Result for Part1 {sampleResult} was expecting {Part1SampleResult}");
            }

            var result = SolvePart1(InputFileContent);
            Console.WriteLine($"{GetType().Name} Part1 : {result}");
        }

        protected abstract int Part2SampleResult { get; }
        protected abstract int SolvePart2(string[] input);
        public void Part2()
        {
            var sampleResult = SolvePart2(SampleFileContent);
            if (sampleResult != Part2SampleResult)
            {
                throw new InvalidOperationException($"Bad Result for Part1 {sampleResult} was expecting {Part2SampleResult}");
            }

            var result = SolvePart2(InputFileContent);
            Console.WriteLine($"{GetType().Name} Part2 : {result}");
        }
    }
}
