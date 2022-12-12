namespace AdventOfCode_2022.Day02
{
    public class Day02 : AdventCalendarProblem<long>
    {
        private enum HandShape
        {
            Rock     = 1,
            Papper   = 2,
            Scissors = 3
        }

        private static HandShape ParseHandShape(char c)
        {
            return c switch
            {
                'A' => HandShape.Rock,
                'X' => HandShape.Rock,

                'B' => HandShape.Papper,
                'Y' => HandShape.Papper,

                'C' => HandShape.Scissors,
                'Z' => HandShape.Scissors,
            };
        }

        private static long ScopePerMatch(HandShape opponent, HandShape you)
        {
            return (opponent, you) switch
            {
                (var o, var y) when o == y => 3,

                (HandShape.Rock, HandShape.Papper) => 6,
                (HandShape.Rock, HandShape.Scissors) => 0,

                (HandShape.Papper, HandShape.Rock) => 0,
                (HandShape.Papper, HandShape.Scissors) => 6,

                (HandShape.Scissors, HandShape.Papper) => 0,
                (HandShape.Scissors, HandShape.Rock) => 6,
            };
        }

        protected override long Part1SampleResult => 15;
        protected override long SolvePart1(string[] input)
        {
            long totalScore = 0;
            foreach(var line in input)
            {
                var opponent = ParseHandShape(line[0]);
                var you = ParseHandShape(line[2]);

                var handShapeScore = (long)you;
                var dualScore = ScopePerMatch(opponent, you);

                totalScore += handShapeScore + dualScore;
            }

            return totalScore;
        }

        private enum DesiredOutcome
        {
            Lose,
            Draw,
            Win
        }

        private static DesiredOutcome ParseDesiredOutcome(char c)
        {
            return c switch
            {
                'X' => DesiredOutcome.Lose,
                'Y' => DesiredOutcome.Draw,
                'Z' => DesiredOutcome.Win,
            };
        }


        private HandShape PickYourHandShape(DesiredOutcome outcome, HandShape opponent)
        {
            return (outcome, opponent) switch
            {
                (DesiredOutcome.Draw, var o) => o,

                (DesiredOutcome.Win, HandShape.Rock) => HandShape.Papper,
                (DesiredOutcome.Win, HandShape.Papper) => HandShape.Scissors,
                (DesiredOutcome.Win, HandShape.Scissors) => HandShape.Rock,

                (DesiredOutcome.Lose, HandShape.Rock) => HandShape.Scissors,
                (DesiredOutcome.Lose, HandShape.Papper) => HandShape.Rock,
                (DesiredOutcome.Lose, HandShape.Scissors) => HandShape.Papper,
            };
        }

        protected override long Part2SampleResult => 12;
        protected override long SolvePart2(string[] input)
        {
            long totalScore = 0;
            foreach (var line in input)
            {
                var opponent = ParseHandShape(line[0]);
                var outcome = ParseDesiredOutcome(line[2]);

                var you = PickYourHandShape(outcome, opponent);

                var handShapeScore = (long)you;
                var dualScore = ScopePerMatch(opponent, you);

                totalScore += handShapeScore + dualScore;
            }

            return totalScore;
        }
    }
}
