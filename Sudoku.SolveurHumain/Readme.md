# Sudoku Solver

Ce projet contient un solver de Sudoku utilisant une approche Humaine.

## Approche

-Notre solver utilise une une classe Kermalis.SudokuSolver.Solver qui est partielle
-	Dans un premier temps nous avons procéder à l’extraction du code permettant la résolution du sudoku dans un dossier librairie
-	Pour pouvoir s’en servir dans notre classe SolveurHumain.cs nous avons dû trouver un moyen d’interfacer avec la librairie et ça classe représentant la board. Pour nous il s’agit de la classe Sudokugrid, pour eux la classe puzzle.
-	Nous avons pour cela tout d’abord créer une méthode permettant de passer d’un SudokuGrid à un Puzzle puis utilisé le solver sur le puzzle
-	Enfin nous avons fait une méthode pour faire l’inverse et renvoyer l’objet attendu


## Tests de Performance

Nous avons utilisé le projet Sudoku.Benchmark pour mesurer les performances de notre solver sur différentes grilles de Sudoku. Les résultats des tests sont présentés ci-dessous :

- Grille Facile : Temps d'exécution moyen de 2 ms.
- Grille Moyenne : Temps d'exécution moyen de 223 ms.
- Grille Difficile : Temps d'exécution moyen de 670 ms.

## Remarque : 

Si pour le niveaux de difficulté Facile la résolution ne semble avoir aucun problème, certaine grille du niveaux difficile et moyen contiennent néanmoins des erreurs. 
La limite de résolution de ce Solveur se trouve donc en fonction des niveaux