namespace AdventOfCode_2022.Day03
{
    public class Day03 : AdventCalendarSolver
    {
        protected override long Part1SampleResult => 157;
        protected override long SolvePart1(string[] input)
        {
            long sum = 0;

            foreach(var rucksack in input)
            {
                var cut = rucksack.Length / 2;
                var firstCompartment = rucksack.Substring(0, cut);
                var secondCompartment = rucksack.Substring(cut);

                var item = firstCompartment.Distinct()
                    .Join(secondCompartment.Distinct(),
                        outerKeySelector: c => c,
                        innerKeySelector: c => c,
                        resultSelector: (l, r) => r)
                    .Single();

                if ('A' <= item && item <= 'Z')
                {
                    var point = item - 'A' + 27;
                    sum += point;
                }
                else
                {
                    var point = item - 'a' + 1;
                    sum += point;
                }
            }

            return sum;
        }

        protected override long Part2SampleResult => 70;
        protected override long SolvePart2(string[] input)
        {
            long sum = 0;

            for(var i = 0; i < input.Length; i += 3)
            {
                var elf1 = input[i  ].Distinct();
                var elf2 = input[i+1].Distinct();
                var elf3 = input[i+2].Distinct();

                var item = elf1.Concat(elf2).Concat(elf3)
                    .GroupBy(keySelector: c => c)
                    .Select(kvp => (kvp.Key, kvp.Count()))
                    .Single(x => x.Item2 == 3).Key;

                if ('A' <= item && item <= 'Z')
                {
                    var point = item - 'A' + 27;
                    sum += point;
                }
                else
                {
                    var point = item - 'a' + 1;
                    sum += point;
                }
            }

            return sum;
        }
    }
}
