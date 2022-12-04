namespace AdventOfCode_2021.Day10
{


    public class Day10 : AdventCalendarSolver
    {
        private static char[] OpeningTokens = new[] { '(', '[', '{', '<' };

        private static readonly IDictionary<char, char> OpeningTokensByClosingToken
            = new Dictionary<char, char>
            {
                [')'] = '(',
                [']'] = '[',
                ['}'] = '{',
                ['>'] = '<',
            };

        private static readonly IDictionary<char, long> InvalidScorePerToken
            = new Dictionary<char, long>
            {
                [')'] = 3,
                [']'] = 57,
                ['}'] = 1197,
                ['>'] = 25137,
            };

        private static readonly IDictionary<char, long> AutoCompleteScorePerToken
            = new Dictionary<char, long>
            {
                ['('] = 1,
                ['['] = 2,
                ['{'] = 3,
                ['<'] = 4,
            };


        protected override long Part1SampleResult => 26397;
        protected override long SolvePart1(string[] input)
        {
            long sum = 0;
            var tokenStack = new Stack<char>();

            foreach (var line in input)
            {
                tokenStack.Clear();
                foreach (var token in line)
                {
                    if (OpeningTokens.Contains(token))
                    {
                        tokenStack.Push(token);
                    }
                    else
                    {
                        if (tokenStack.TryPeek(out var matchingOpeningToken) && OpeningTokensByClosingToken[token] == matchingOpeningToken)
                        {
                            tokenStack.Pop();
                        }
                        else
                        {
                            sum += InvalidScorePerToken[token];
                            break;
                        }
                    }
                }
            }

            return sum;
        }

        protected override long Part2SampleResult => 288_957;
        protected override long SolvePart2(string[] input)
        {
            var lineScores = new List<long>();
            var tokenStack = new Stack<char>();

            foreach (var line in input)
            {
                tokenStack.Clear();
                var isValid = true;
                foreach (var token in line)
                {
                    if (OpeningTokens.Contains(token))
                    {
                        tokenStack.Push(token);
                    }
                    else
                    {
                        if (tokenStack.TryPeek(out var matchingOpeningToken) && OpeningTokensByClosingToken[token] == matchingOpeningToken)
                        {
                            tokenStack.Pop();
                        }
                        else
                        {
                            isValid = false;
                            break;
                        }
                    }
                }

                if (isValid && tokenStack.Any())
                {
                    long lineScore = 0;
                    while (tokenStack.Any())
                    {
                        var token = tokenStack.Pop();
                        lineScore = lineScore * 5 + AutoCompleteScorePerToken[token];
                    }

                    lineScores.Add(lineScore);
                }
            }

            return lineScores.OrderBy(x => x).ElementAt(lineScores.Count / 2);
        }
    }
}
