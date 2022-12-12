namespace AdventOfCode_2021.Day07
{
    public class Day07 : AdventCalendarProblem<long>
    {
        protected override long Part1SampleResult => 37;
        protected override long SolvePart1(string[] input)
        {
            var numbers = input[0].Split(',').Select(long.Parse).ToList();

            var lazyMedian = numbers.OrderBy(x => x).ElementAt(numbers.Count / 2);

            return numbers.Select(x => Math.Abs(x - lazyMedian)).Sum();
        }

        protected override long Part2SampleResult => 168;
        protected override long SolvePart2(string[] input)
        {
            var numbers = input[0].Split(',').Select(long.Parse).ToList();

            var average = (long)Math.Round(numbers.Average(), digits: 0);

            var minCost = long.MaxValue;
            //I expect the mincost to be close to the average
            for (int offset = -1; offset <= 1; offset++)
            {
                var currentCost = numbers.Select(x => CostFunction(x, average + offset)).Sum();
                minCost = Math.Min(currentCost, minCost);
            }

            return minCost;
        }

        private static IList<long> CostValues = new List<long> { 0, 1 };

        private long CostFunction(long number, long target)
        {
            var distance = Math.Abs(number - target);

            var distanceAsInt = Convert.ToInt32(distance);

            if (distanceAsInt >= CostValues.Count)
            {
                for (var i = CostValues.Count; i <= distanceAsInt; i++)
                {
                    CostValues.Add(CostValues[i - 1] + i);
                }
            }

            return CostValues[distanceAsInt];
        }
    }
}
