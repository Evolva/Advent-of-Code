using System;
using System.Collections.Generic;

namespace AdventOfCode.Day05
{


    public class Day11 : AdventCalendarSolver
    {
        private static IReadOnlyList<(int di, int dj)> FlashRange = new List<(int, int)>
        {
            (-1, -1),(-1, 0),(-1, 1),
            ( 0, -1),        ( 0, 1),
            ( 1, -1),( 1, 0),( 1, 1),
        };

        protected override long Part1SampleResult => 1656;
        protected override long SolvePart1(string[] input)
        {
            var initialState = new int[10, 10];

            for(int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    var c = input[i][j];
                    initialState[i,j] = c - '0';
                }
            }

            long flashCounter = 0;

            for (var step = 1; step <= 100; step++)
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        initialState[i, j]++;
                        if (initialState[i, j] == 10)
                        {
                            IncreaseAround(initialState, i, j);
                        }
                    }
                }

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (initialState[i, j] >= 10)
                        {
                            initialState[i, j] = 0;
                            flashCounter++;
                        }
                    }
                }
            }

            return flashCounter;
        }

        private static void IncreaseAround(int[,] state, int i, int j)
        {
            foreach(var range in FlashRange)
            {
                if (IsInBound(i + range.di, j + range.dj))
                {
                    state[i + range.di, j + range.dj]++;
                    if (state[i + range.di, j + range.dj] == 10)
                    {
                        IncreaseAround(state, i + range.di, j + range.dj);
                    }
                }
            }
        }

        private static bool IsInBound(int i, int j) => 0 <= i && i < 10 && 0 <= j && j < 10;

        protected override long Part2SampleResult => 195;
        protected override long SolvePart2(string[] input)
        {
            var initialState = new int[10, 10];

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    var c = input[i][j];
                    initialState[i, j] = c - '0';
                }
            }

            long stepCounter = 1;
            while (true)
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        initialState[i, j]++;
                        if (initialState[i, j] == 10)
                        {
                            IncreaseAround(initialState, i, j);
                        }
                    }
                }

                var allFlashed = true;
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (initialState[i, j] >= 10)
                        {
                            initialState[i, j] = 0;
                            continue;
                        }
                        allFlashed = false;
                    }
                }

                if (allFlashed)
                {
                    break;
                }
                stepCounter++;
            }

            return stepCounter;
        }
    }
}
