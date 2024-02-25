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

#### Ajout Solver 2 : 

# Sudoku solver : 
Implémentation d'un solveur de Sudoku en utilisant la programmation par contraintes avec la bibliothèque Google OR-Tools.

## Approche : 

On note donc que l’utilisation de techniques humaines d’inférence permet de résoudre la plupart des
Sudoku mais bloque sur les plus difficiles qui nécessitent en plus une part additionnelle
d’exploration.

Nous avons donc ajouter un nuggets OR-tools, afin de pouvoir compléter notre solveur précedent afin d'avoir un solver par contrainte.
Ce dernier ne se met en marche seulement si les methodes humaines ne suffisent pas.

-Création du modèle :

Le modèle (CpModel) est créé.
Des variables de décision sont ajoutées pour chaque cellule de la grille, avec des contraintes pour fixer les valeurs initiales si elles sont présentes.

-Ajout des contraintes :
Des contraintes sont ajoutées pour garantir que chaque ligne, chaque colonne et chaque région (3x3) de la grille contient des valeurs distinctes.

-Résolution du modèle :

Le modèle est résolu en utilisant un solveur (CpSolver).
Extraction de la solution :

Si une solution est trouvée, les valeurs des cellules sont extraites du solveur et renvoyées.

## Test de Performance : 

- Grille Facile : Temps d'exécution moyen de 2 ms.
- Grille Moyenne : Temps d'exécution moyen de 275 ms.
- Grille Difficile : Temps d'exécution moyen de 246 ms.
