using System;
using System.IO;

namespace AdventOfCode
{
    public abstract class AdventCalendarSolver
    {
        protected readonly string[] FileContent;

        protected AdventCalendarSolver(string inputFilePath)
        {
            FileContent = File.ReadAllLines(inputFilePath);
        }

        protected abstract int SolvePart1();
        public void Part1()
        {
            var result = SolvePart1();
            Console.WriteLine($"{GetType().Name} {nameof(Part1)} : {result}");
        }

        protected abstract int SolvePart2();
        public void Part2()
        {
            var result = SolvePart2();
            Console.WriteLine($"{GetType().Name} {nameof(Part2)} : {result}");
        }
    }
}
