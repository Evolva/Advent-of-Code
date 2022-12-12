namespace AdventOfCode_2021.Day01;

public class Day01 : AdventCalendarProblem
{
    protected override long Part1SampleResult => 7;
    protected override long SolvePart1(string[] input)
    {
        return input
            .Select(int.Parse)
            .Pairwise()
            .Where(pair => pair.Item1 < pair.Item2)
            .Count();
    }


    protected override long Part2SampleResult => 5;
    protected override long SolvePart2(string[] input)
    {
        return input
            .Select(int.Parse)
            .SlidingWindow(3)
            .Pairwise()
            .Where(pair => pair.Item1.Sum() < pair.Item2.Sum())
            .Count();
    }
}
