using Kermalis.SudokuSolver;
using Library.ConstraintSolver;
using Sudoku.Shared;

namespace Sudoku.SolveurHumain;

public class SolveurHumain : ISudokuSolver
{
    public static Puzzle MapSudokuGridToPuzzle(SudokuGrid s)
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

        return new Puzzle(board, false);
    }

    public static int[,] GetBoardFromSudokuGrid(SudokuGrid s)
    {
        int[,] board = new int[9,9];
        
        for (int i = 0; i < s.Cells.Length; i++)
        {
            for (int j = 0; j < s.Cells[i].Length; j++)
            {
                board[i,j] = s.Cells[i][j];
            }
        }
        return board;
    }

    public static SudokuGrid GetSudokuGridFromBoard(int[,] board, SudokuGrid s)
    {
        for (int i = 0; i < s.Cells.Length; i++)
        {
            for (int j = 0; j < s.Cells[i].Length; j++)
            {
                s.Cells[i][j] = board[i, j];
            }
        }

        return s;
    }

    public static SudokuGrid MapPuzzleToSudokuGrid(Puzzle p, SudokuGrid s)
    {
        for (int i = 0; i < s.Cells.Length; i++)
        {
            for (int j = 0; j < s.Cells[i].Length; j++)
            {
                s.Cells[i][j] = p[i, j].Value;
            }
        }

        return s;
    }
    

    public SudokuGrid Solve(SudokuGrid s)
    {
        Puzzle puzzle = MapSudokuGridToPuzzle(s);
        Solver solver = new Solver(puzzle);
        bool isSolved = solver.TrySolve();
        
        if (isSolved)
        {
            return MapPuzzleToSudokuGrid(puzzle, s);
        }
        
        
        ConstraintSolver cs = new ConstraintSolver();
        int[,] board = GetBoardFromSudokuGrid(s);
        board = cs.Solve(board);
        return GetSudokuGridFromBoard(board, s);
    }
}