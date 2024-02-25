using Sudoku.Shared;
using org.chocosolver.solver;
using org.chocosolver.solver.variables;
using org.chocosolver.solver.search;
using static org.chocosolver.util.tools.ArrayUtils;
using Sudoku.ChocoSolver;
using java.lang;
using java.util;
using org.chocosolver.solver.search.strategy;
using org.chocosolver.util.tools;

namespace Sudoku.ChocoSolver;

public class ChocoSolver : ISudokuSolver {

    private Model model; // Model choco solver pour la CSP
    private int fullSize; // Largeur totale du sudoku
    private int[] valueSudoku = { 1, 2, 3, 4, 5, 6, 7, 8, 9 }; // Valeur possible d'une case dans un sudoku
    private IntVar[][] sdkGrid; // Grille chocosolver du sudoku
    private int[][] sdkToSolve; // Tableau 2D C# pour stocker le sudoku à résoudre

    // Fonction permettant de résoudre le sudoku
    public SudokuGrid Solve(SudokuGrid s)
    {
        // On récupère le sudoku à résoudre à l'aide de Sudoku.Shared
        sdkToSolve = s.CloneSudoku().Cells;

        // Model chocosolver pour la résolution par CSP
        model = new Model();

        // Variables de taille des sections et de la grille complète du sudoku
        int sectionSize = 3;
        fullSize = sectionSize * 3;

        // On initialise le model de 9x9 avec les valeurs possibles de 1 à 9
        sdkGrid = model.intVarMatrix(fullSize, fullSize, 1, valueSudoku.Length);

        // On rempli la grille ChocoSolver avec le sudoku à résoudre
        for (int i = 0; i < fullSize; i++)
        {
            for (int j = 0; j < fullSize; j++)
            {
                int a = sdkToSolve[i][j];
                if (a != 0)
                {
                    model.arithm(sdkGrid[i][j], "=", a).post();
                }
            }
        }

        // Contrainte sur les lignes et colonnes de la grille totale
        for (int i = 0; i < fullSize; i++)
        {
            IntVar[] row = new IntVar[fullSize];
            IntVar[] column = new IntVar[fullSize];
            for (int j = 0; j < fullSize; j++)
            {
                row[j] = sdkGrid[i][j];
                column[j] = sdkGrid[j][i];
            }
            model.allDifferent(row).post();
            model.allDifferent(column).post();
        }

        // Contrainte sur les sections 3x3 de la grille de sudoku
        for (int i = 0; i < sectionSize; i++)
        {
            for (int j = 0; j < sectionSize; j++)
            {
                IntVar[] section = new IntVar[fullSize];
                int idx = 0;
                for (int xoff = 0; xoff < sectionSize; xoff++)
                {
                    for (int yoff = 0; yoff < sectionSize; yoff++)
                    {
                        section[idx++] = sdkGrid[i * sectionSize + xoff][j * sectionSize + yoff];
                    }
                }
                model.allDifferent(section).post();
            }
        }

        // On résout le sudoku 
        solveByDefault();

        s.Cells = sdkToSolve;

        return s;
    }


    // Fonction permettant de construire la solution du sudoku à l'aide de ChocoSolver
    private System.Boolean buildSol() {

        System.Boolean isSolved = model.getSolver().solve();
        if (!isSolved)
        {
            return false;
        }

        // On créer la solution 
        List<List<int>> sol = new List<List<int>>();

        for (int i = 0; i < fullSize; i++)
        {
            List<int> row = new List<int>();
            for (int j = 0; j < fullSize; j++)
            {
                row.Add(sdkGrid[i][j].getValue() - 1);
            }
            sol.Add(row);
        }

        // On vérifie si toutes les solutions ont été trouver
        System.Boolean moreSolutions = model.getSolver().solve();
        if (moreSolutions)
        {
            return false;
        }

        for (int i = 0; i < fullSize; i++)
        {
            for (int j = 0; j < fullSize; j++)
            {
                this.sdkToSolve[i][j] = valueSudoku[sol[i][j]];
            }
        }
        return true;
    }

    private System.Boolean solveByDefault()
    {
        return buildSol();
    }
}