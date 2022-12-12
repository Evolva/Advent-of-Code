using System.Diagnostics;

namespace AdventOfCode_2022.Day07
{

    public class Day07 : AdventCalendarProblem<long>
    {
        public interface IFileSytemEntry
        {
            string Name { get; }

            public long Size { get; }

            public IFileSytemEntry? Parent { get; }
        }

        [DebuggerDisplay("dir {Name}")]
        public class Directory : IFileSytemEntry
        {
            private readonly List<IFileSytemEntry> _content;

            public Directory(string name, IFileSytemEntry? parent)
            {
                _content = new List<IFileSytemEntry>();

                Name = name;
                Parent = parent;
            }

            public string Name { get; }


            public long Size => _content.Sum(x => x.Size);

            public IFileSytemEntry? Parent { get; }

            public void Add(IFileSytemEntry entry)
            {
                _content.Add(entry);
            }

            public Directory? GetSubDirectory(string name)
            {
                return _content.SingleOrDefault(c => c.Name == name) as Directory;
            }

            public IEnumerable<Directory> ListDirectories()
            {
                return _content.OfType<Directory>();
            }
        }

        [DebuggerDisplay("{Size} {Name}")]
        public class File : IFileSytemEntry
        {
            public File(string name, IFileSytemEntry parent, long size)
            {
                Name = name;
                Parent = parent;
                Size = size;
            }

            public string Name { get; }

            public long Size { get; }

            public IFileSytemEntry? Parent { get; }
        }


        private Directory ParseFileSystemAndReturnItsRoot(string[] input)
        {
            Directory? currentDirectory = null;

            var i = 0;

            while (i < input.Length)
            {
                var line = input[i];

                if (line.StartsWith("$ cd"))
                {
                    var folder = line.Split(" ").Last();

                    if (folder == "..")
                    {
                        currentDirectory = currentDirectory!.Parent as Directory;
                    }
                    else if (folder == "/")
                    {
                        currentDirectory = new Directory(folder, null);
                    }
                    else
                    {
                        currentDirectory = currentDirectory!.GetSubDirectory(folder);
                    }
                    i++;
                    continue;
                }
                else if (line == "$ ls")
                {
                    i++;
                    while (i < input.Length && !input[i].StartsWith("$ "))
                    {
                        var splittedLsContent = input[i].Split(" ");

                        if (splittedLsContent[0] == "dir")
                        {
                            var dirName = splittedLsContent.Last();
                            currentDirectory!.Add(new Directory(dirName, currentDirectory));
                        }
                        else if (long.TryParse(splittedLsContent[0], out var size))
                        {
                            var fileName = splittedLsContent.Last();
                            currentDirectory!.Add(new File(fileName, currentDirectory, size));
                        }
                        else
                        {
                            throw new InvalidOperationException();
                        }

                        i++;
                    }
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            while (currentDirectory?.Parent != null)
            {
                currentDirectory = currentDirectory.Parent as Directory;
            }

            return currentDirectory;
        }

        protected override long Part1SampleResult => 95437;
        protected override long SolvePart1(string[] input)
        {
            var directory = ParseFileSystemAndReturnItsRoot(input);

            long result = 0;

            var directoriesQueue = new Queue<Directory>();
            directoriesQueue.Enqueue(directory);

            while(directoriesQueue.Any())
            {
                var d = directoriesQueue.Dequeue();

                if (d.Size < 100000)
                {
                    result += d.Size;
                }

                foreach (var subDir in d.ListDirectories())
                {
                    directoriesQueue.Enqueue(subDir);
                }
            }

            return result;
        }

        protected override long Part2SampleResult => 24933642;
        protected override long SolvePart2(string[] input)
        {
            const long totalAvailableDiskSpace = 70_000_000;
            const long requiredDiskSpace = 30_000_000;

            var directory = ParseFileSystemAndReturnItsRoot(input);

            var unusedDiskSpace = totalAvailableDiskSpace - directory.Size;

            var diskSpaceToFree = requiredDiskSpace - unusedDiskSpace;

            var allDirectories = new List<Directory>();

            var directoriesQueue = new Queue<Directory>();
            directoriesQueue.Enqueue(directory);

            while (directoriesQueue.Any())
            {
                var d = directoriesQueue.Dequeue();

                allDirectories.Add(d);

                foreach (var subDir in d.ListDirectories())
                {
                    directoriesQueue.Enqueue(subDir);
                }
            }

            var smallestDirectoryBigEnough = allDirectories.OrderBy(d => d.Size).First(d => d.Size >= diskSpaceToFree);

            return smallestDirectoryBigEnough.Size;
        }
    }
}
