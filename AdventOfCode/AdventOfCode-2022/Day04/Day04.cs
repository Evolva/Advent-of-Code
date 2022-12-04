namespace AdventOfCode_2022.Day04
{
    public class Day04 : AdventCalendarSolver
    {
        protected override long Part1SampleResult => 2;
        protected override long SolvePart1(string[] input)
        {
            long count = 0;

            foreach(var line in input)
            {
                var sections = line.Split(',').SelectMany(x => x.Split("-")).Select(int.Parse).ToList();

                var l1 = sections[0];
                var h1 = sections[1];
                
                var l2 = sections[2];
                var h2 = sections[3];

                if (   (l1 <= l2 && h2 <= h1)
                    || (l2 <= l1 && h1 <= h2))
                {
                    count++;
                }
            }

            return count;
        }

        protected override long Part2SampleResult => 4;
        protected override long SolvePart2(string[] input)
        {
            long count = 0;

            foreach (var line in input)
            {
                var sections = line.Split(',').SelectMany(x => x.Split("-")).Select(int.Parse).ToList();

                var l1 = sections[0];
                var h1 = sections[1];

                var l2 = sections[2];
                var h2 = sections[3];

                if (   (l1 <= l2 && l2 <= h1)
                    || (l1 <= h2 && h2 <= h1)
                    || (l2 <= l1 && l1 <= h2)
                    || (l2 <= h1 && h1 <= h2))
                {
                    count++;
                }
            }

            return count;
        }
    }
}
