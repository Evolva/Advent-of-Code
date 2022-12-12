namespace AdventOfCode_2022.Day08
{
    public class Day08 : AdventCalendarProblem<long>
    {
        private int[][] ParseTreeMap(string[] input)
        {
            var trees = new int[input.Length][];

            for (var i = 0; i < input.Length; i++)
            {
                var line = input[i];
                trees[i] = new int[line.Length];

                for (int j = 0; j < line.Length; j++)
                {
                    trees[i][j] = line[j] - '0';
                }
            }

            return trees;
        }

        protected override long Part1SampleResult => 21;
        protected override long SolvePart1(string[] input)
        {
            
            var trees = ParseTreeMap(input);

            long visibleTrees = 0;

            for (var i = 0; i < trees.Length; i++)
            {
                for (int j = 0; j < trees[i].Length; j++)
                {
                    if (i == 0 || j == 0 || i == trees.Length - 1 || j == trees[i].Length - 1)
                    {
                        //Edge
                        visibleTrees++;
                        continue;
                    }

                    var currentTreeHeight = trees[i][j];

                    var treeLine = new List<int>();
                    for (var k = i - 1; k >= 0; k--)
                    {
                        treeLine.Add(trees[k][j]);
                    }
                    if (treeLine.All(t => t < currentTreeHeight))
                    {
                        visibleTrees++;
                        continue;
                    }
                    
                    treeLine.Clear();
                    for (var k = i + 1; k < trees.Length; k++)
                    {
                        treeLine.Add(trees[k][j]);
                    }
                    if (treeLine.All(t => t < currentTreeHeight))
                    {
                        visibleTrees++;
                        continue;
                    }

                    treeLine.Clear();
                    for (var k = j - 1; k >= 0; k--)
                    {
                        treeLine.Add(trees[i][k]);
                    }
                    if (treeLine.All(t => t < currentTreeHeight))
                    {
                        visibleTrees++;
                        continue;
                    }

                    treeLine.Clear();
                    for (var k = j + 1; k < trees[i].Length; k++)
                    {
                        treeLine.Add(trees[i][k]);
                    }
                    if (treeLine.All(t => t < currentTreeHeight))
                    {
                        visibleTrees++;
                        continue;
                    }
                }
            }

            return visibleTrees;
        }

        protected override long Part2SampleResult => 8;
        protected override long SolvePart2(string[] input)
        {
            var trees = ParseTreeMap(input);

            long maxScenicScore = 0;

            for (var i = 1; i < trees.Length - 1; i++)
            {
                for (int j = 1; j < trees[i].Length - 1; j++)
                {

                    var currentTreeHeight = trees[i][j];

                    var treeLine = new List<int>();
                    for (var k = i - 1; k >= 0; k--)
                    {
                        if (trees[k][j] >= currentTreeHeight)
                        {
                            treeLine.Add(trees[k][j]);
                            break;
                        }
                        treeLine.Add(trees[k][j]);
                    }
                    var scoreUp = treeLine.Count();
                    

                    treeLine.Clear();
                    for (var k = i + 1; k < trees.Length; k++)
                    {
                        if (trees[k][j] >= currentTreeHeight)
                        {
                            treeLine.Add(trees[k][j]);
                            break;
                        }
                        treeLine.Add(trees[k][j]);
                    }
                    var scoreDown = treeLine.Count();

                    treeLine.Clear();
                    for (var k = j - 1; k >= 0; k--)
                    {
                        if (trees[i][k] >= currentTreeHeight)
                        {
                            treeLine.Add(trees[i][k]);
                            break;
                        }
                        treeLine.Add(trees[i][k]);
                    }
                    var scoreLeft = treeLine.Count();

                    treeLine.Clear();
                    for (var k = j + 1; k < trees[i].Length; k++)
                    {
                        if (trees[i][k] >= currentTreeHeight)
                        {
                            treeLine.Add(trees[i][k]);
                            break;
                        }
                        treeLine.Add(trees[i][k]);
                    }
                    var scoreRight = treeLine.Count();


                    maxScenicScore = Math.Max(maxScenicScore, scoreLeft * scoreRight * scoreUp * scoreDown);
                }
            }

            return maxScenicScore;
        }
    }
}
