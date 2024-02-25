using System;
using System.Collections.Generic;
using Sudoku.Shared;

namespace Sudoku.Norvig
{
    public class Norvig : ISudokuSolver
    {
        public SudokuGrid Solve(SudokuGrid s)
        {
            int[][] board = new int[9][];
            for (int i = 0; i < s.Cells.Length; i++)
            {
                board[i] = new int[9];
                for (int j = 0; j < s.Cells[i].Length; j++)
                {
                    board[i][j] = s.Cells[i][j];
                }
            }

            if (SolveSudoku(board))
            {
                // Update the original SudokuGrid with the solved values
                for (int i = 0; i < s.Cells.Length; i++)
                {
                    for (int j = 0; j < s.Cells[i].Length; j++)
                    {
                        s.Cells[i][j] = board[i][j];
                    }
                }
            }
            else
            {
                Console.WriteLine("No solution found for the Sudoku puzzle.");
            }

            return s;
        }

        private bool SolveSudoku(int[][] board)
        {
            var result = Search(ConvertToDictionary(board));
            if (result != null)
            {
                // Update the board with the solution
                foreach (var kvp in result)
                {
                    var row = kvp.Key[0] - 'A';
                    var col = kvp.Key[1] - '1';
                    board[row][col] = kvp.Value;
                }
                return true;
            }
            return false;
        }

        private Dictionary<string, int> Search(Dictionary<string, int> board)
        {
            if (board == null)
                return null;

            if (IsSolved(board))
                return board;

            var cell = GetNextEmptyCell(board);
            foreach (var digit in "123456789")
            {
                if (IsValidMove(board, cell, digit))
                {
                    board[cell] = int.Parse(digit.ToString());
                    var result = Search(new Dictionary<string, int>(board));
                    if (result != null)
                        return result;
                }
            }
            return null;
        }

        private bool IsSolved(Dictionary<string, int> board)
        {
            foreach (var kvp in board)
            {
                if (kvp.Value == 0)
                    return false;
            }
            return true;
        }

        private string GetNextEmptyCell(Dictionary<string, int> board)
        {
            foreach (var kvp in board)
            {
                if (kvp.Value == 0)
                    return kvp.Key;
            }
            return null;
        }

        private bool IsValidMove(Dictionary<string, int> board, string cell, char digit)
        {
            foreach (var unit in GetUnitsOfSquare(cell))
            {
                foreach (var square in unit)
                {
                    if (board.ContainsKey(square) && board[square] == digit - '0')
                        return false;
                }
            }
            return true;
        }

        private IEnumerable<IEnumerable<string>> GetUnitsOfSquare(string cell)
        {
            var row = cell[0];
            var col = cell[1];
            yield return GetRow(row);
            yield return GetCol(col);
            yield return GetSquare(row, col);
        }

        private IEnumerable<string> GetRow(char row)
        {
            for (char col = '1'; col <= '9'; col++)
                yield return row + col.ToString();
        }

        private IEnumerable<string> GetCol(char col)
        {
            for (char row = 'A'; row <= 'I'; row++)
                yield return row + col.ToString();
        }

        private IEnumerable<string> GetSquare(char row, char col)
        {
            var startRow = (char)(((row - 'A') / 3) * 3 + 'A');
            var startCol = (char)(((col - '1') / 3) * 3 + '1');
            for (char r = startRow; r < startRow + 3; r++)
        {
            for (char c = startCol; c < startCol + 3; c++)
        {
            yield return r.ToString() + c;
        }
    }
}


        private Dictionary<string, int> ConvertToDictionary(int[][] board)
        {
            var result = new Dictionary<string, int>();
            for (int i = 0; i < board.Length; i++)
            {
                for (int j = 0; j < board[i].Length; j++)
                {
                    result.Add(((char)('A' + i)).ToString() + (j + 1).ToString(), board[i][j]);
                }
            }
            return result;
        }
    }
}
