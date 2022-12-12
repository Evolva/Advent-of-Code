namespace AdventOfCode_Helper;

public abstract class AdventCalendarProblem<Part1Type, Part2Type> : IAdventCalendarProblem
    where Part1Type : IEquatable<Part1Type>
{
    private readonly string[] InputFileContent;
    private readonly string[] SampleFileContent;
    private readonly string[]? Sample2FileContent;

    protected AdventCalendarProblem()
    {
        InputFileContent = File.ReadAllLines($"{GetType().Name}/input.txt");
        SampleFileContent = File.ReadAllLines($"{GetType().Name}/sample.txt");

        var sample2FileName = $"{GetType().Name}/sample2.txt";
        if (File.Exists(sample2FileName))
        {
            Sample2FileContent = File.ReadAllLines(sample2FileName);
        }
    }

    protected abstract Part1Type Part1SampleResult { get; }
    protected abstract Part1Type SolvePart1(string[] input);
    private void Part1()
    {
        var sampleResult = SolvePart1(SampleFileContent);
        if (!sampleResult.Equals(Part1SampleResult))
        {
            throw new InvalidOperationException($"Bad Result for Part1 {sampleResult} was expecting {Part1SampleResult}");
        }

        var result = SolvePart1(InputFileContent);
        Console.WriteLine($"{GetType().Name} Part1 :");
        Console.WriteLine(result);
    }

    protected abstract Part2Type Part2SampleResult { get; }
    protected abstract Part2Type SolvePart2(string[] input);
    private void Part2()
    {
        var sampleResult = SolvePart2(Sample2FileContent ?? SampleFileContent);
        if (!sampleResult.Equals(Part2SampleResult))
        {
            throw new InvalidOperationException($"Bad Result for Part2 {sampleResult} was expecting {Part2SampleResult}");
        }

        var result = SolvePart2(InputFileContent);
        Console.WriteLine($"{GetType().Name} Part2 :");
        Console.WriteLine(result);
    }

    public void Solve()
    {
        try
        {
            Part1();
            Part2();
        }
        catch (NotImplementedException)
        {
            //NOOP
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}

public abstract class AdventCalendarProblem<TType> : AdventCalendarProblem<TType, TType>
    where TType : IEquatable<TType>
{ }

public abstract class AdventCalendarProblem : AdventCalendarProblem<long, long>
{ }
