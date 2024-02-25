using Google.OrTools.Sat;

namespace Library.ConstraintSolver;

public class ConstraintSolver
{
    private readonly CpSolver _solver = new();
    
    public int[,] Solve(int[,] puzzle)
    {
        (CpModel model, IntVar[,]? grid) = CreateModel(puzzle);
        CpSolverStatus status = _solver.Solve(model);

        return status is CpSolverStatus.Feasible or CpSolverStatus.Optimal
            ? ExtractSolution(_solver, grid)
            : throw new Exception("No Solution");
    }

    private static (CpModel, IntVar[,]) CreateModel(int[,] puzzle)
    {
        CpModel model = new();
        IntVar[,] grid = new IntVar[9, 9];

        CreateVariables(model, grid, puzzle);
        AddConstraints(model, grid);

        return (model, grid);
    }

    private static void CreateVariables(CpModel model, IntVar[,] grid, int[,] puzzle)
    {
        for (int i = 0; i < 9; ++i)
        {
            for (int j = 0; j < 9; ++j)
            {
                grid[i, j] = model.NewIntVar(1, 9, $"grid[{i},{j}]");
                if (puzzle[i, j] != 0)
                {
                    model.Add(grid[i, j] == puzzle[i, j]);
                }
            }
        }
    }

    private static void AddConstraints(CpModel model, IntVar[,] grid)
    {
        for (int i = 0; i < 9; ++i)
        {
            AddRowConstraint(model, grid, i);
            AddColumnConstraint(model, grid, i);
        }

        for (int i = 0; i < 9; i += 3)
        {
            for (int j = 0; j < 9; j += 3)
            {
                AddCellConstraint(model, grid, i, j);
            }
        }
    }

    private static void AddRowConstraint(CpModel model, IntVar[,] grid, int row)
    {
        IntVar[] intVars = Enumerable
            .Range(0, 9)
            .Select(col => grid[row, col])
            .ToArray();

        model.AddAllDifferent(intVars);
    }

    private static void AddColumnConstraint(CpModel model, IntVar[,] grid, int col)
    {
        IntVar[] intVars = Enumerable
            .Range(0, 9)
            .Select(row => grid[row, col])
            .ToArray();

        model.AddAllDifferent(intVars);
    }

    private static void AddCellConstraint(CpModel model, IntVar[,] grid, int startRow, int startCol)
    {
        IntVar[] cellVariables = Enumerable
            .Range(0, 3)
            .SelectMany(i => Enumerable.Range(0, 3).Select(j => grid[startRow + i, startCol + j]))
            .ToArray();

        model.AddAllDifferent(cellVariables);
    }

    private static int[,] ExtractSolution(CpSolver solver, IntVar[,] grid)
    {
        int[,] solution = new int[9, 9];

        for (int i = 0; i < 9; ++i)
        {
            for (int j = 0; j < 9; ++j)
            {
                solution[i, j] = (int)solver.Value(grid[i, j]);
            }
        }

        return solution;
    }
}