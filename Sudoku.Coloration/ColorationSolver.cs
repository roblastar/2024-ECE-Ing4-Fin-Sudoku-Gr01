using Sudoku.Shared;

namespace Sudoku.Coloration;

    public class ColorationSolver : ISudokuSolver
    {
        SudokuGrid ISudokuSolver.Solve(SudokuGrid s)
        {
                //On passe notre sudoku sous la forme d'un graphe pour réaliser la colorisation de ce dernier en fonction des différents sommets
                Graphe Colographe = new Graphe(s);

                Console.WriteLine("Sudoku à resoudre \n");
                Colographe.AffichageGrid();

                //Colorisation par graphe pour resoiudre le sudoku
                Colographe.COLORISATION();

                Console.WriteLine("Sudoku resolu \n");
                Colographe.AffichageGrid();

                //return de la solution trouver pour valider la resolution par colorisation de graphe
                return Colographe.getGrid();
        }
    }
