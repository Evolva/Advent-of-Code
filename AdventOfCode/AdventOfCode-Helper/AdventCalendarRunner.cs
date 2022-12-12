using System.Reflection;

namespace AdventOfCode_Helper;

public static class AdventCalendarRunner
{
    public static void Run(string[] args)
    {
        var type = Assembly.GetEntryAssembly()!.GetTypes().SingleOrDefault(t => t.Name == args[0]);

        if (type == null)
        {
            Console.WriteLine($"Unable to find type '{args[0]}' in ");
            Console.ReadKey();
            return;
        }

        var instance = Activator.CreateInstance(type);
        if (instance is IAdventCalendarProblem solver)
        {
            solver.Solve();
            Console.ReadKey();
        }
    }
}
