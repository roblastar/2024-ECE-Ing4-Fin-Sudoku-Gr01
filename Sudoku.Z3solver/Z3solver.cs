/*using Sudoku.Shared;

namespace Sudoku.Z3solver;

public class Z3solver : ISudokuSolver
{
    public SudokuGrid Solve(SudokuGrid s)
    {
        return s;
    }
}*/

using Microsoft.Z3;
using Sudoku.Shared;

namespace Sudoku.Z3solver
{
    public class Z3solver : ISudokuSolver
    {
        public SudokuGrid Solve(SudokuGrid s)
        {
            return SolveWithZ3(s);
        }

        private SudokuGrid SolveWithZ3(SudokuGrid s)
        {
            Context ctx = new Context();
            // 9x9 matrix of integer variables
            IntExpr[][] X = new IntExpr[9][];
            for (uint i = 0; i < 9; i++)
            {
                X[i] = new IntExpr[9];
                for (uint j = 0; j < 9; j++)
                    X[i][j] = (IntExpr)ctx.MkConst(ctx.MkSymbol("x_" + (i + 1) + "_" + (j + 1)), ctx.IntSort);
            }

            // each cell contains a value in {1, ..., 9}
            BoolExpr[][] cells_c = new BoolExpr[9][];
            for (uint i = 0; i < 9; i++)
            {
                cells_c[i] = new BoolExpr[9];
                for (uint j = 0; j < 9; j++)
                    cells_c[i][j] = ctx.MkAnd(ctx.MkLe(ctx.MkInt(1), X[i][j]),
                                              ctx.MkLe(X[i][j], ctx.MkInt(9)));
            }

            // each row contains a digit at most once
            BoolExpr[] rows_c = new BoolExpr[9];
            for (uint i = 0; i < 9; i++)
                rows_c[i] = ctx.MkDistinct(X[i]);

            // each column contains a digit at most once
            BoolExpr[] cols_c = new BoolExpr[9];
            for (uint j = 0; j < 9; j++)
            {
                IntExpr[] column = new IntExpr[9];
                for (uint i = 0; i < 9; i++)
                    column[i] = X[i][j];

                cols_c[j] = ctx.MkDistinct(column);
            }

            // each 3x3 square contains a digit at most once
            BoolExpr[][] sq_c = new BoolExpr[3][];
            for (uint i0 = 0; i0 < 3; i0++)
            {
                sq_c[i0] = new BoolExpr[3];
                for (uint j0 = 0; j0 < 3; j0++)
                {
                    IntExpr[] square = new IntExpr[9];
                    for (uint i = 0; i < 3; i++)
                        for (uint j = 0; j < 3; j++)
                            square[3 * i + j] = X[3 * i0 + i][3 * j0 + j];
                    sq_c[i0][j0] = ctx.MkDistinct(square);
                }
            }

            BoolExpr sudoku_c = ctx.MkTrue();
            foreach (BoolExpr[] t in cells_c)
                sudoku_c = ctx.MkAnd(ctx.MkAnd(t), sudoku_c);
            sudoku_c = ctx.MkAnd(ctx.MkAnd(rows_c), sudoku_c);
            sudoku_c = ctx.MkAnd(ctx.MkAnd(cols_c), sudoku_c);
            foreach (BoolExpr[] t in sq_c)
                sudoku_c = ctx.MkAnd(ctx.MkAnd(t), sudoku_c);

            // sudoku instance, we use '0' for empty cells
            int[,] instance = {{0,0,0,0,9,4,0,3,0},
                               {0,0,0,5,1,0,0,0,7},
                               {0,8,9,0,0,0,0,4,0},
                               {0,0,0,0,0,0,2,0,8},
                               {0,6,0,2,0,1,0,5,0},
                               {1,0,2,0,0,0,0,0,0},
                               {0,7,0,0,0,0,5,2,0},
                               {9,0,0,0,6,5,0,0,0},
                               {0,4,0,9,7,0,0,0,0}};

            BoolExpr instance_c = ctx.MkTrue();
            for (uint i = 0; i < 9; i++)
                for (uint j = 0; j < 9; j++)
                    instance_c = ctx.MkAnd(instance_c,
                        (BoolExpr)
                        ctx.MkITE(ctx.MkEq(ctx.MkInt(instance[i, j]), ctx.MkInt(0)),
                                    ctx.MkTrue(),
                                    ctx.MkEq(X[i][j], ctx.MkInt(instance[i, j]))));

            Solver z3Solver = ctx.MkSolver();
            z3Solver.Assert(sudoku_c);
            z3Solver.Assert(instance_c);

            if (z3Solver.Check() == Status.SATISFIABLE)
            {
                Model m = z3Solver.Model;
                int[,] solution = new int[9, 9];

                for (uint i = 0; i < 9; i++)
                {
                    for (uint j = 0; j < 9; j++)
                    {
                        Expr cellValue = m.Evaluate(X[i][j]);

                        if (cellValue is IntNum)
                            solution[i, j] = (int)(cellValue as IntNum).Int;
                        else
                        {
                            // Handle the case where the value is not an integer
                            Console.WriteLine("Unexpected non-integer value in the solution.");
                            ctx.Dispose(); // Libérer les ressources de Z3
                            throw new Exception("Unexpected non-integer value in the solution");
                        }
                    }
                }

                ctx.Dispose(); // Libérer les ressources de Z3

                // Mettez à jour la grille SudokuGrid avec la solution
                for (uint i = 0; i < 9; i++)
                    for (uint j = 0; j < 9; j++)
                        s[i, j] = solution[i, j];

                return s;
            }
            else
            {
                // Ajustez le comportement lorsque la grille n'est pas résolue.
                Console.WriteLine("Failed to solve sudoku");
                ctx.Dispose(); // Libérer les ressources de Z3
                throw new Exception("Sudoku not solvable");
                ///test
            }
        }
    }
}