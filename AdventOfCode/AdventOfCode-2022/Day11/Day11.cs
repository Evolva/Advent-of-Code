using System.Linq.Expressions;
using System.Numerics;

namespace AdventOfCode_2022.Day11
{
    public class Day11 : AdventCalendarProblem
    {
        private static class KeepAwayGameParser
        {
            public static KeepAwayGame Parse(string[] input, bool withWorryRelief)
            {
                var strReader = new StringReader(string.Join(Environment.NewLine, input));

                var monkeys = new List<Monkey>();
                var lcm = 1;

                while (strReader.Peek() > 0)
                {
                    var (modulo, monkey) = ParseMonkey(strReader, withWorryRelief);

                    lcm = lcm * modulo;
                    monkeys.Add(monkey);

                    _ = strReader.ReadLine();
                }

                foreach(var monkey in monkeys)
                {
                    monkey.SetMonkeyLcm(lcm);
                }

                return new KeepAwayGame(monkeys);
            }

            private static (int, Monkey) ParseMonkey(StringReader strReader, bool withWorryRelief)
            {
                //Monkey 0:
                _ = strReader.ReadLine();

                //  Starting items: 79, 98
                var itemsLine = strReader.ReadLine();
                var items = itemsLine
                    .Split(":", StringSplitOptions.TrimEntries)
                    .Last()
                    .Split(",", StringSplitOptions.TrimEntries)
                    .Select(long.Parse);

                //  Operation: new = old * 19
                var operationLine = strReader.ReadLine();
                var inspectEffect = ParseInspectFunction(operationLine);

                //  Test: divisible by 19
                var testLine = strReader.ReadLine();
                if (!testLine.Contains("divisible by"))
                {
                    throw new ArgumentOutOfRangeException("testFunction", testLine, null);
                }
                var modulo = int.Parse(testLine.Split(" ").Last());

                //    If true: throw to monkey 2
                var trueActionLine = strReader.ReadLine();
                var targetIfTrue = int.Parse(trueActionLine.Split(" ").Last());
                //    If false: throw to monkey 0
                var falseActionLine = strReader.ReadLine();
                var targetIfFalse = int.Parse(falseActionLine.Split(" ").Last());

                var test = new MonkeyTest(nb => nb % modulo == 0, targetIfTrue, targetIfFalse);

                return (modulo, new Monkey(new Queue<long>(items), inspectEffect, test, withWorryRelief));
            }
            
            private static Func<long, long> ParseInspectFunction(string str)
            {
                const string old = "old";

                var tokens = str.Split("=", StringSplitOptions.TrimEntries).Last()
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries); ;

                var oldParam = Expression.Parameter(typeof(long), old);

                Expression leftOp = tokens[0] == old
                    ? oldParam
                    : Expression.Constant(long.Parse(tokens[0]));

                Expression rightOp = tokens[2] == old
                    ? oldParam
                    : Expression.Constant(long.Parse(tokens[2]));

                var body = tokens[1] switch
                {
                    "+" => Expression.Add(leftOp, rightOp),
                    "*" => Expression.Multiply(leftOp, rightOp),
                    _   => throw new ArgumentOutOfRangeException("operator", tokens[1], null),
                };

                return Expression.Lambda<Func<long, long>>(body, oldParam).Compile();
            }
        }
            

        private class KeepAwayGame
        {
            private readonly List<Monkey> _monkeys;

            public KeepAwayGame(List<Monkey> monkeys)
            {
                _monkeys = monkeys;
            }

            public void PlayRounds(int n)
            {
                for(var i = 0; i < n; i++)
                {
                    PlaySingleRound();
                }
            }

            private void PlaySingleRound()
            {
                foreach(var monkey in _monkeys)
                {
                    monkey.PlayTurn(_monkeys);
                }
            }

            public (Monkey, Monkey) GetTop2ActiveMonkeys()
            {
                var topMonkeys =  _monkeys.OrderByDescending(m => m.InspectionCount).Take(2).ToList();

                return (topMonkeys[0], topMonkeys[1]);
            }
        }


        private record MonkeyTest(Func<long, bool> Test, int MonkeyToThrowToIfTrue, int MonkeyToThrowToIfFalse);
        

        private class Monkey
        {
            private readonly Queue<long> _items;
            private readonly Func<long, long> _inspectEffect;
            private readonly MonkeyTest _monkeyTest;
            private readonly bool _withWorryRelief;

            public long InspectionCount { get; private set; }
            private long _lcm;

            public Monkey(Queue<long> initialItems, Func<long, long> inspectEffect, MonkeyTest monkeyTest, bool withWorryRelief)
            {
                _items = initialItems;
                _inspectEffect = inspectEffect;
                _monkeyTest = monkeyTest;
                _withWorryRelief = withWorryRelief;
                InspectionCount = 0;
            }

            public void SetMonkeyLcm(long lcm)
            {
                _lcm = lcm;
            }

            public void PlayTurn(IReadOnlyList<Monkey> monkeys)
            {
                while(_items.Any())
                {
                    var item = _items.Dequeue();

                    InspectionCount++;
                    var worryLevel = _withWorryRelief
                        ? (_inspectEffect(item) / 3) % _lcm
                        : _inspectEffect(item) % _lcm;

                    var targetMonkey = _monkeyTest.Test(worryLevel)
                        ? monkeys[_monkeyTest.MonkeyToThrowToIfTrue]
                        : monkeys[_monkeyTest.MonkeyToThrowToIfFalse];

                    targetMonkey.Receive(worryLevel);
                }
            }

            public void Receive(long newItem)
            {
                _items.Enqueue(newItem);
            }
        }

        protected override long Part1SampleResult => 10605;
        protected override long SolvePart1(string[] input)
        {
            var game = KeepAwayGameParser.Parse(input, withWorryRelief: true);
            game.PlayRounds(20);

            var (m1, m2) = game.GetTop2ActiveMonkeys();

            return m1.InspectionCount * m2.InspectionCount;
        }

        protected override long Part2SampleResult => 2_713_310_158;
        protected override long SolvePart2(string[] input)
        {
            var game = KeepAwayGameParser.Parse(input, withWorryRelief: false);
            game.PlayRounds(10_000);

            var (m1, m2) = game.GetTop2ActiveMonkeys();

            return m1.InspectionCount * m2.InspectionCount;
        }
    }
}
