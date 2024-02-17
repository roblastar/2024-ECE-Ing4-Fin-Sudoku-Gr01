using Sudoku.Shared;
using Sudoku.SolveurHumain;
using Kermalis.SudokuSolver;

namespace Sudoku.SolveurHumain
{
    public class SolveurHumain : ISudokuSolver
    {
        public SudokuGrid Solve(SudokuGrid s)
        {
            // Convertir SudokuGrid en Puzzle si nécessaire
            Puzzle puzzle = ConvertToPuzzle(s);
            
            Solver solver = new Solver(puzzle);
            
            // Appeler la méthode de résolution
            bool success = solver.TrySolve();
            
            if (success)
            {
                // Si le puzzle est résolu, convertir Puzzle en SudokuGrid
                return ConvertToSudokuGrid(solver.Puzzle);
            }
            else
            {
                // Gérer l'échec de la résolution
                // Peut-être retourner le SudokuGrid original ou signaler l'échec d'une autre manière
                return s;
            }
        }

	private Puzzle ConvertToPuzzle(SudokuGrid s)
	{
		int[][] board = new int[9][];
		for (int i = 0; i < 9; i++)
		{
			board[i] = new int[9];
			for (int j = 0; j < 9; j++)
			{
				// Supposons que SudokuGrid a une méthode ou un indexeur pour accéder aux valeurs
				// et que les cases vides sont représentées par 0 dans SudokuGrid
				// Vous devrez ajuster cette logique en fonction de la structure réelle de SudokuGrid
				board[i][j] = s[i, j]; // À ajuster selon l'implémentation de SudokuGrid
			}
		}
		
		// Créez l'objet Puzzle avec la matrice créée et isCustom défini comme vous le souhaitez
		bool isCustom = true; // ou false, selon votre logique d'application
		return new Puzzle(board, isCustom);
	}


        // Vous devez également implémenter cette conversion
        private SudokuGrid ConvertToSudokuGrid(Puzzle puzzle)
        {
            // Conversion logique ici
            return new SudokuGrid();
        }
    }
}
