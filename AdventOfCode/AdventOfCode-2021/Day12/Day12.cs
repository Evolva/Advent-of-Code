namespace AdventOfCode_2021.Day12
{
    public class Day12 : AdventCalendarProblem<long>
    {
        public class CaveMap
        {
            public static CaveMap ReadFromFile(string[] input)
            {
                var nodeLinks = new Dictionary<string, HashSet<string>>();

                foreach (var line in input)
                {
                    var split = line.Split('-');
                    var from = split[0];
                    var to = split[1];

                    if (nodeLinks.TryGetValue(from, out var fromLinks))
                    {
                        fromLinks.Add(to);
                    }
                    else
                    {
                        nodeLinks[from] = new HashSet<string> { to };
                    }

                    if (nodeLinks.TryGetValue(to, out var toLinks))
                    {
                        toLinks.Add(from);
                    }
                    else
                    {
                        nodeLinks[to] = new HashSet<string> { from };
                    }
                }

                return new CaveMap(nodeLinks);
            }

            private readonly Dictionary<string, HashSet<string>> nodeLinks;

            private CaveMap(Dictionary<string, HashSet<string>> nodeLinks)
            {
                this.nodeLinks = nodeLinks;
            }

            public long CountPathsWithoutVisitingSmallCaveTwice()
            {
                var possiblePath = new HashSet<string>();
                var pathQueue = new Queue<string>();
                pathQueue.Enqueue("start");

                while (pathQueue.Any())
                {
                    var currentPath = pathQueue.Dequeue();
                    var visitedNodes = currentPath.Split(',');
                    var lastNode = visitedNodes.Last();

                    var possibleNextNodes = nodeLinks[lastNode];
                    foreach (var possibleNextNode in possibleNextNodes)
                    {
                        if (possibleNextNode == "end")
                        {
                            //reached the end 
                            possiblePath.Add($"{currentPath},end");
                            continue;
                        }

                        if (IsSmallCave(possibleNextNode) && visitedNodes.Contains(possibleNextNode))
                        {
                            //small cave already visited
                            continue;
                        }

                        pathQueue.Enqueue($"{currentPath},{possibleNextNode}");
                    }
                }

                return possiblePath.Count;
            }

            public long CountPathVisitingASingleSmallCaveTwice()
            {
                var possiblePath = new HashSet<string>();
                var pathQueue = new Queue<string>();
                pathQueue.Enqueue("start");

                while (pathQueue.Any())
                {
                    var currentPath = pathQueue.Dequeue();
                    var visitedNodes = currentPath.Split(',');
                    var lastNode = visitedNodes.Last();

                    var possibleNextNodes = nodeLinks[lastNode];
                    foreach (var possibleNextNode in possibleNextNodes)
                    {
                        if (possibleNextNode == "start")
                        {
                            //can't return to start
                            continue;
                        }

                        if (possibleNextNode == "end")
                        {
                            //reached the end 
                            possiblePath.Add($"{currentPath},end");
                            continue;
                        }

                        if (IsSmallCave(possibleNextNode) && !CanVisitSmallCave(visitedNodes, possibleNextNode))
                        {
                            continue;
                        }

                        pathQueue.Enqueue($"{currentPath},{possibleNextNode}");
                    }
                }

                return possiblePath.Count;
            }

            private bool IsSmallCave(string s) => s.All(c => 'a' <= c && c <= 'z');

            private bool CanVisitSmallCave(string[] visitedNodes, string smallCave)
            {
                var visitedSmallCave = new HashSet<string>();
                var aNodeHasBeenVisitedTwice = false;

                foreach (var node in visitedNodes.Where(n => IsSmallCave(n)))
                {
                    if (!visitedSmallCave.Add(node))
                    {
                        aNodeHasBeenVisitedTwice = true;
                    }
                }

                if (aNodeHasBeenVisitedTwice)
                {
                    return !visitedSmallCave.Contains(smallCave);
                }
                return true;
            }
        }


        protected override long Part1SampleResult => 10;
        protected override long SolvePart1(string[] input)
        {
            return CaveMap.ReadFromFile(input).CountPathsWithoutVisitingSmallCaveTwice();
        }

        protected override long Part2SampleResult => 36;
        protected override long SolvePart2(string[] input)
        {
            var caveMap = CaveMap.ReadFromFile(input);

            return caveMap.CountPathVisitingASingleSmallCaveTwice();
        }
    }
}