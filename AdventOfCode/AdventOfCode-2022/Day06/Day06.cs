using DataStructures;

namespace AdventOfCode_2022.Day06
{

    public class Day06 : AdventCalendarProblem<long>
    {
        protected override long Part1SampleResult => 10;
        protected override long SolvePart1(string[] input)
        {
            const int nbChar = 4;

            var buffer = new CircularBuffer<char>(nbChar);

            var singleLine = input[0];

            for (var i = 0; i < singleLine.Length; i++)
            {
                buffer.Add(singleLine[i]);

                if (buffer.CurrentSize == nbChar && buffer.Distinct().Count() == nbChar)
                {
                    return i + 1;
                }
            }

            throw new InvalidOperationException();
        }

        protected override long Part2SampleResult => 29;
        protected override long SolvePart2(string[] input)
        {
            const int nbChar = 14;

            var buffer = new CircularBuffer<char>(nbChar);

            var singleLine = input[0];

            for (var i = 0; i < singleLine.Length; i++)
            {
                buffer.Add(singleLine[i]);

                if (buffer.CurrentSize == nbChar && buffer.Distinct().Count() == nbChar)
                {
                    return i + 1;
                }
            }

            throw new InvalidOperationException();
        }
    }
}
