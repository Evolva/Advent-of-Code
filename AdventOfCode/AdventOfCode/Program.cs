using System;
using System.Linq;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var type = typeof(AdventCalendarSolver).Assembly.GetTypes().SingleOrDefault(t => t.Name == args[0]);
            var instance = Activator.CreateInstance(type);
            if (instance is AdventCalendarSolver solver)
            {
                try
                {
                    solver.Part1();
                    solver.Part2();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                Console.ReadKey();
            }
        }
    }
}
