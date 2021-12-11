using AdventOfCode;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

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
                catch { }
                Console.ReadKey();
            }
        }
    }
}
