namespace AdventOfCode_2022.Day01
{
    public class Day01 : AdventCalendarProblem<long>
    {
        protected override long Part1SampleResult => 24000;
        protected override long SolvePart1(string[] input)
        {
            long max = 0;

            for(int i = 0; i < input.Length; i++)
            {
                long sum = 0;
                while (i < input.Length && input[i] != string.Empty)
                {
                    sum += long.Parse(input[i++]);
                }

                max = Math.Max(max, sum);
            }

            return max;
        }


        protected override long Part2SampleResult => 45000;
        protected override long SolvePart2(string[] input)
        {
            var top3CarryingElfs = new SortedList<long, long>();

            for (int i = 0; i < input.Length; i++)
            {
                long sum = 0;
                while (i < input.Length && input[i] != string.Empty)
                {
                    sum += long.Parse(input[i++]);
                }

                top3CarryingElfs.Add(sum, sum);
                if (top3CarryingElfs.Count() > 3)
                {
                    top3CarryingElfs.RemoveAt(0);
                }
            }

            return top3CarryingElfs.Sum(kv => kv.Value);
        }
    }
}
