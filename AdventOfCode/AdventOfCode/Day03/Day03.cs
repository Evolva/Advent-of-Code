using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Day03
{
    public class Day03 : AdventCalendarSolver
    {
        protected override int Part1SampleResult => 198;

        protected override int SolvePart1(string[] input)
        {
            var numberOfNumbers = input.Length;
            var numberLength = input[0].Length;

            var gammaRate = 0;
            var epsilonRate = 0;

            for (int i = 0; i < numberLength; i++)
            {
                var numberOfOne = input.Select(n => n[i]).Count(b => b == '1');
                var numberOfZero = numberOfNumbers - numberOfOne;

                if (numberOfOne > numberOfZero)
                {
                    gammaRate += (int)Math.Pow(2, numberLength-i-1);
                }
                else
                {
                    epsilonRate += (int)Math.Pow(2, numberLength-i-1);
                }
            }

            return gammaRate * epsilonRate;
        }


        protected override int Part2SampleResult => 230;

        protected override int SolvePart2(string[] input)
        {
            return GetOxygenRating(input) * GetC02Rating(input);
        }

        private int GetOxygenRating(string[] input)
        {
            var position = 0;
            string[] remainingNumbers = input;
            
            while(remainingNumbers.Length > 1)
            {
                var bitCriteria = GetOxygenBitCriteria(remainingNumbers, position);
                remainingNumbers = remainingNumbers.Where(n => n[position] == bitCriteria).ToArray();
                position++;
            }

            var oxygenRating = remainingNumbers.Single();
            var oxygenRatingLength = oxygenRating.Length;
            int oxygenRatingValue = 0;

            for(var i = 0; i < oxygenRatingLength; i++)
            {
                if (oxygenRating[i] == '1')
                {
                    oxygenRatingValue += (int)Math.Pow(2, oxygenRatingLength - i - 1);
                }
                
            }

            return oxygenRatingValue;
        }

        private static char GetOxygenBitCriteria(string[] input, int position)
        {
            var numberOfNumbers = input.Length;
            var numberOfOne = input.Select(n => n[position]).Count(b => b == '1');
            var numberOfZero = numberOfNumbers - numberOfOne;

            return numberOfOne >= numberOfZero ? '1' : '0';
        }

        private int GetC02Rating(string[] input)
        {
            var position = 0;
            string[] remainingNumbers = input;

            while (remainingNumbers.Length > 1)
            {
                var bitCriteria = GetC02BitCriteria(remainingNumbers, position);
                remainingNumbers = remainingNumbers.Where(n => n[position] == bitCriteria).ToArray();
                position++;
            }

            var co2Rating = remainingNumbers.Single();
            var co2RatingLength = co2Rating.Length;
            int co2RatingValue = 0;

            for (var i = 0; i < co2RatingLength; i++)
            {
                if (co2Rating[i] == '1')
                {
                    co2RatingValue += (int)Math.Pow(2, co2RatingLength - i - 1);
                }
            }

            return co2RatingValue;
        }

        private static char GetC02BitCriteria(string[] input, int position)
        {
            var numberOfNumbers = input.Length;
            var numberOfOne = input.Select(n => n[position]).Count(b => b == '1');
            var numberOfZero = numberOfNumbers - numberOfOne;

            return numberOfOne >= numberOfZero ? '0' : '1';
        }
    }
}
