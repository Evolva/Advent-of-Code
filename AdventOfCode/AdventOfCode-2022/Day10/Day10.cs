using System.Text;

namespace AdventOfCode_2022.Day10
{
    public class Day10 : AdventCalendarProblem<long, string>
    {
        public abstract record CpuInstruction
        {
            public abstract int CycleToComplete { get; }
            public void Visit(VirtualMachine virtualMachine)
            {
                virtualMachine.Apply(this);
            }

            public abstract VirtualMachineState ChangeState(VirtualMachineState state);
        }

        public record Noop : CpuInstruction
        {
            public override int CycleToComplete => 1;

            public override VirtualMachineState ChangeState(VirtualMachineState state)
            {
                return state;
            }
        }

        public record AddX(int Increment) : CpuInstruction
        {
            public override int CycleToComplete => 2;

            public override VirtualMachineState ChangeState(VirtualMachineState state)
            {
                return state with { RegisterX = state.RegisterX + Increment };
            }
        }

        public record VirtualMachineState(int Cycle, int RegisterX) { }

        public class VirtualMachine
        {
            private VirtualMachineState state;

            public VirtualMachine(VirtualMachineState initialState)
            {
                state = initialState;
            }

            public IEnumerable<VirtualMachineState> Apply(CpuInstruction instruction)
            {
                for(var cycle = 1; cycle < instruction.CycleToComplete; cycle++)
                {
                    state = state with { Cycle = state.Cycle + 1 };
                    yield return state;
                }

                state = instruction.ChangeState(state) with { Cycle = state.Cycle + 1 };
                yield return state;
            }
        }

        public class Simulation
        {

            public IEnumerable<VirtualMachineState> Simulate(string[] input)
            {
                var initialState = new VirtualMachineState(1, 1);
                var vm = new VirtualMachine(initialState);

                yield return initialState;

                foreach (var instruction in input.Select(ParseInstruction))
                {
                    var states = vm.Apply(instruction);
                    foreach (var state in states)
                    {
                        yield return state;
                    }
                }
            }

            private CpuInstruction ParseInstruction(string str)
            {
                var param = str.Split(" ");

                return (param[0]) switch
                {
                    "noop" => new Noop(),
                    "addx" => new AddX(int.Parse(param[1])),
                    _      => throw new ArgumentOutOfRangeException("instructionName", param[0], null)
                };
            }
        }

        protected override long Part1SampleResult => 13140;
        protected override long SolvePart1(string[] input)
        {
            var states = new Simulation().Simulate(input);

            var keySignalStrenghts = states.Where(s => s.Cycle % 40 == 20);

            return keySignalStrenghts.Sum(s => s.RegisterX * s.Cycle);
        }

        protected override string Part2SampleResult =>
            "##..##..##..##..##..##..##..##..##..##.." + Environment.NewLine +
            "###...###...###...###...###...###...###." + Environment.NewLine +
            "####....####....####....####....####...." + Environment.NewLine +
            "#####.....#####.....#####.....#####....." + Environment.NewLine +
            "######......######......######......####" + Environment.NewLine +
            "#######.......#######.......#######....." + Environment.NewLine;

        protected override string SolvePart2(string[] input)
        {
            var states = new Simulation().Simulate(input);
            var registerXPerCycle = states.ToDictionary(keySelector: x => x.Cycle, elementSelector: x => x.RegisterX) ;

            var spriteOffset = new[] { -1, 0, +1 };

            var ouput = new StringBuilder();

            for (var row = 0; row < 6; row++)
            {
                for (var col = 0; col < 40; col++)
                {
                    var cycle = row * 40 + col + 1;
                    var spritePositions = spriteOffset.Select(so => registerXPerCycle[cycle] + so);

                    if (spritePositions.Contains(col))
                    {
                        ouput.Append('#');
                    }
                    else
                    {
                        ouput.Append('.');
                    }
                }

                ouput.Append(Environment.NewLine);
            }

            return ouput.ToString();
        }
    }
}
