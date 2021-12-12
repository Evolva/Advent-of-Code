using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day04
{
    public class BingoBoard
    {
        private readonly int?[][] Board;

        public BingoBoard(int?[][] board)
        {
            Board = board;
        }

        public bool TryToBingo(int number)
        {
            bool found = false;
            int i, j = 0;
            for (i = 0; i < Board.Length; i++)
            {
                for(j = 0; j < Board[i].Length; j++)
                {
                    if(Board[i][j].HasValue && Board[i][j].Value == number)
                    {
                        Board[i][j] = null;
                        found = true;
                        break;
                    }
                }
                if (found)
                {
                    break;
                }
            }

            return found && (Board[i].All(x => !x.HasValue) || Board.All(x => !x[j].HasValue));
        }

        public int GetBoardScore()
        {
            return Board.SelectMany(x => x).Select(x => x.HasValue ? x.Value : 0).Sum();
        }
    }

    public class BingoGame
    {
        private readonly IList<int> NumberSequence;
        private readonly IList<BingoBoard> Boards;

        public BingoGame(string[] input)
        {
            var state = ParseGameState(input);

            NumberSequence = state.numberSequence;
            Boards = state.boards;
        }

        private (IList<int> numberSequence, IList<BingoBoard> boards)  ParseGameState(string[] input)
        {
            var sequence = input[0].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

            var boards = new List<BingoBoard>();
            for (var i = 2; i < input.Length; i += 6)
            {
                var board = new int?[5][];
                for(var line = 0; line < 5; line++)
                {
                    var lineN = input[i+ line].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => (int?)int.Parse(x)).ToArray();
                    board[line] = lineN;
                }
                boards.Add(new BingoBoard(board));
            }

            return (sequence, boards);
        }
    
        public int Play()
        {
            foreach(var number in NumberSequence)
            {
                foreach(var board in Boards)
                {
                    if (board.TryToBingo(number))
                    {
                        return board.GetBoardScore() * number;
                    }
                }
            }

            throw new InvalidOperationException("Not a single board bingo'ed before the end of the number sequence");
        }

        public int PlayToLose()
        {
            var boardsThatDidntWinYet = Boards;

            foreach (var number in NumberSequence)
            {
                foreach (var board in boardsThatDidntWinYet.ToList())
                {
                    if (board.TryToBingo(number))
                    {
                        boardsThatDidntWinYet.Remove(board);
                        if (boardsThatDidntWinYet.Count() == 0)
                        {
                            return board.GetBoardScore() * number;
                        }
                    }
                }
            }

            throw new InvalidOperationException("At least a board never bingo'ed before the end of the number sequence");
        }
    }

    public class Day04 : AdventCalendarSolver
    {
        protected override int Part1SampleResult => 4512;
        protected override int SolvePart1(string[] input)
        {
            return new BingoGame(input).Play();
        }

        protected override int Part2SampleResult => 1924;
        protected override int SolvePart2(string[] input)
        {
            return new BingoGame(input).PlayToLose();
        }
    }
}
