namespace AdventOfCode_2021.Day06
{
    public class Day06 : AdventCalendarSolver
    {
        protected override long Part1SampleResult => 5934;
        protected override long SolvePart1(string[] input)
        {
            return Simulate(input, 80);
        }

        protected override long Part2SampleResult => 26_984_457_539;
        protected override long SolvePart2(string[] input)
        {
            return Simulate(input, 256);
        }

        private long Simulate(string[] input, int numberOfDay)
        {
            var currentState = new long[9];

            foreach (var number in input[0].Split(',').Select(int.Parse))
            {
                currentState[number]++;
            }

            for (int day = 1; day <= numberOfDay; day++)
            {
                var nextState = new long[9];

                nextState[0] = currentState[1];
                nextState[1] = currentState[2];
                nextState[2] = currentState[3];
                nextState[3] = currentState[4];
                nextState[4] = currentState[5];
                nextState[5] = currentState[6];
                nextState[6] = currentState[7] + currentState[0];
                nextState[7] = currentState[8];
                nextState[8] = currentState[0];

                currentState = nextState;
            }

            return currentState.Aggregate<long, long>(seed: 0, func: (accu, n) => accu + n);
        }
    }
}
