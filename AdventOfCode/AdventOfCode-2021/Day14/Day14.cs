using AdventOfCode_2021.Day01;

namespace AdventOfCode_2021.Day14
{
    public class Day14 : AdventCalendarProblem<long>
    {
        protected override long Part1SampleResult => 1588;
        protected override long SolvePart1(string[] input)
        {
            return NoMoreBruteForceSolver(input, 10);
        }

        protected override long Part2SampleResult => 2_188_189_693_529;
        protected override long SolvePart2(string[] input)
        {
            return NoMoreBruteForceSolver(input, 40);
        }

        private long NoMoreBruteForceSolver(string[] input, int maxStep)
        {
            var polymerTemplate = input[0];
            var insertionRules = new Dictionary<(char, char), List<(char, char)>>();

            for (var line = 2; line < input.Length; line++)
            {
                var rule = input[line];
                var split = rule.Split("->", StringSplitOptions.RemoveEmptyEntries);
                var elements = split[0].Trim();

                var fstLetter = elements[0];
                var sndLetter = elements[1];

                var insert = split[1].Trim()[0];

                insertionRules[(fstLetter, sndLetter)] = new List<(char, char)>
                {
                    (fstLetter, insert),
                    (insert, sndLetter),
                };
            }

            var pairCounters = new Dictionary<(char, char), long>();
            foreach (var pair in polymerTemplate.Pairwise())
            {
                if (pairCounters.TryGetValue(pair, out var occurence))
                {
                    pairCounters[pair] = occurence + 1;
                }
                else
                {
                    pairCounters.Add(pair, 1);
                }
            }

            for (var step = 1; step <= maxStep; step++)
            {
                var newPairCounters = new Dictionary<(char, char), long>();

                foreach (var (inPair, occurenceOnPreviousStep) in pairCounters)
                {
                    var outPairs = insertionRules[inPair];

                    foreach (var outPair in outPairs)
                    {
                        if (newPairCounters.TryGetValue(outPair, out var cpt))
                        {
                            newPairCounters[outPair] = cpt + occurenceOnPreviousStep;
                        }
                        else
                        {
                            newPairCounters.Add(outPair, occurenceOnPreviousStep);
                        }
                    }
                }

                pairCounters = newPairCounters;
            }

            var lettersOccurence = new Dictionary<char, long>();
            var firstPair = true;

            foreach (var (letterPair, occurence) in pairCounters)
            {
                if (firstPair)
                {
                    var fstLetter = letterPair.Item1;

                    if (lettersOccurence.TryGetValue(fstLetter, out var fstLetterOccurence))
                    {
                        lettersOccurence[fstLetter] = occurence + fstLetterOccurence;
                    }
                    else
                    {
                        lettersOccurence.Add(fstLetter, occurence);
                    }

                    firstPair = false;
                }

                var sndLetter = letterPair.Item2;

                if (lettersOccurence.TryGetValue(sndLetter, out var sndLetterOccurence))
                {
                    lettersOccurence[sndLetter] = occurence + sndLetterOccurence;
                }
                else
                {
                    lettersOccurence.Add(sndLetter, occurence);
                }
            }

            var maxOccurence = lettersOccurence.Values.Max();
            var minOccurence = lettersOccurence.Values.Min();

            return maxOccurence - minOccurence;
        }
    }
}