using AdventOfCode_2021.Day01;

namespace AdventOfCode_2021.Day14
{
    public class Day14 : AdventCalendarSolver
    {
        protected override long Part1SampleResult => 1588;
        protected override long SolvePart1(string[] input)
        {
            return NoMoreBruteForceSolver(input, 10);

            //var polymerTemplate = input[0];
            //var insertionRules = new Dictionary<string, string>();
            //
            //for (var line = 2; line < input.Length; line++)
            //{
            //    var rule = input[line];
            //    var split = rule.Split("->", StringSplitOptions.RemoveEmptyEntries);
            //
            //    var elements = split[0].Trim();
            //    var insert = split[1].Trim();
            //    var produce = insert + elements[1];
            //
            //    insertionRules[elements] = produce;
            //}
            //
            //for (var step = 1; step <= 10; step++)
            //{
            //    var polymerBuilder = new StringBuilder();
            //
            //    var first = true;
            //    foreach (var pair in polymerTemplate.Pairwise())
            //    {
            //        if (first)
            //        {
            //            first = false;
            //            polymerBuilder.Append(pair.Item1);
            //        }
            //
            //        string elements = string.Empty + pair.Item1 + pair.Item2;
            //
            //        polymerBuilder.Append(insertionRules[elements]);
            //    }
            //    polymerTemplate = polymerBuilder.ToString();
            //}
            //
            //var occurence = polymerTemplate.CountBy(x => x);
            //var maxOccurence = occurence.Values.Max();
            //var minOccurence = occurence.Values.Min();
            //
            //return maxOccurence - minOccurence;
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

public static class EnumerableExtensions
{
    public static Dictionary<V, long> CountBy<T, V>(this IEnumerable<T> source, Func<T, V> selector)
    {
        var result = new Dictionary<V, long>();

        foreach (var elt in source)
        {
            var key = selector(elt);

            if (result.TryGetValue(key, out var count))
            {
                result[key] = count + 1;
            }
            else
            {
                result[key] = 1;
            }
        }

        return result;
    }
}