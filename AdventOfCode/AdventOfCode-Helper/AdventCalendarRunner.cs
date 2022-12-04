using System.Reflection;

namespace AdventOfCode_Helper;

public static class AdventCalendarRunner
{
    public static void Run(string[] args)
    {
        var type = Assembly.GetEntryAssembly()!.GetTypes().SingleOrDefault(t => t.Name == args[0]);
        var instance = Activator.CreateInstance(type);
        if (instance is AdventCalendarSolver solver)
        {
            try
            {
                solver.Part1();
                solver.Part2();
            }
            catch (NotImplementedException)
            {
                //NOOP
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadKey();
        }
    }
}
