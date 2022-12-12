using System;
using System.Text.RegularExpressions;

namespace AdventOfCode_2022.Day05
{


    public class Day05 : AdventCalendarProblem<string>
    {
        protected override string Part1SampleResult => "CMZ";
        protected override string SolvePart1(string[] input)
        {
            var ship = ParseShip(input, out var startOfInstruction);

            MoveCrateOneByOne(ship, input, startOfInstruction);

            return new string(ship.Select(s => s.Peek()).ToArray());
        }

        private List<Stack<char>> ParseShip(string[] input, out int startOfInstructions)
        {
            var i = 0;
            while (input[i].Trim().StartsWith("["))
            {
                i++;
            }
            startOfInstructions = i + 2;

            var nbStacks = int.Parse(input[i].Split(" ", StringSplitOptions.RemoveEmptyEntries).Last());

            var ship = Enumerable.Repeat(0,nbStacks).Select(_ => new Stack<char>()).ToList();
            
            for(var j = i - 1; j >= 0; j--)
            {
                var lineOfCrate = input[j];

                for(var stack = 0; stack < nbStacks; stack++)
                {
                    var crateContent = lineOfCrate[stack * 4 + 1];
                    if (crateContent != ' ')
                    {
                        ship[stack].Push(crateContent);
                    }
                }
            }


            return ship;
        }

        private readonly Regex InstructionMatchingRegex = new Regex(@"move (?<nbAction>\d+) from (?<source>\d+) to (?<destination>\d+)");

        private void MoveCrateOneByOne(List<Stack<char>> ship, string[] input, int startOfInstruction)
        {
            for (var i = startOfInstruction; i < input.Length; i++)
            {
                var instruction = input[i];

                var m = InstructionMatchingRegex.Match(instruction);
                if (!m.Success) { throw new InvalidOperationException(); }

                var nbAction = int.Parse(m.Groups["nbAction"].Value);
                var sourceStack = int.Parse(m.Groups["source"].Value) - 1;
                var destinationStack = int.Parse(m.Groups["destination"].Value) - 1;

                for (var j = 0; j < nbAction; j++)
                {
                    var crateContent = ship[sourceStack].Pop();
                    ship[destinationStack].Push(crateContent);
                }
            }
        }


        protected override string Part2SampleResult => "MCD";
        protected override string SolvePart2(string[] input)
        {
            var ship = ParseShip(input, out var startOfInstruction);

            MoveStackOfCrates(ship, input, startOfInstruction);

            return new string(ship.Select(s => s.Peek()).ToArray());
        }

        private void MoveStackOfCrates(List<Stack<char>> ship, string[] input, int startOfInstruction)
        {
            for (var i = startOfInstruction; i < input.Length; i++)
            {
                var instruction = input[i];

                var m = InstructionMatchingRegex.Match(instruction);
                if (!m.Success) { throw new InvalidOperationException(); }

                var nbCrateToMove = int.Parse(m.Groups["nbAction"].Value);
                var sourceStack = int.Parse(m.Groups["source"].Value) - 1;
                var destinationStack = int.Parse(m.Groups["destination"].Value) - 1;

                var stackOfCrates = new Stack<char>();
                for (var j = 0; j < nbCrateToMove; j++)
                {
                    stackOfCrates.Push(ship[sourceStack].Pop());
                }

                while(stackOfCrates.Any())
                {
                    ship[destinationStack].Push(stackOfCrates.Pop());
                }
            }
        }
    }
}
