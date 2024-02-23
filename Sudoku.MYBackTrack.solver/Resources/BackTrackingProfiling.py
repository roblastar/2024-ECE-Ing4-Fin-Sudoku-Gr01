def find_next_cell_to_fill(grid):
    # Initialisation des variables
    min_possibilities = 10  # Initialisation à un nombre supérieur au maximum de possibilités
    next_cell = None  # Initialisation de la prochaine cellule à remplir à None
    
    # Parcours de la grille
    for i in range(9):
        for j in range(9):
            # Vérification si la cellule est vide
            if grid[i][j] == 0:
                # Récupération des possibilités pour cette cellule
                possibilities = get_possible_values(grid, i, j)
                
                # Mise à jour de la prochaine cellule si le nombre de possibilités est plus petit
                if len(possibilities) < min_possibilities:
                    min_possibilities = len(possibilities)
                    next_cell = (i, j)
    
    return next_cell  # Renvoie la prochaine cellule à remplir


def get_possible_values(grid, row, col):
    # Récupération des valeurs présentes dans la même ligne
    row_values = set(grid[row])
    
    # Récupération des valeurs présentes dans la même colonne
    col_values = set(grid[i][col] for i in range(9))
    
    # Récupération des valeurs présentes dans la même sous-grille
    box_values = set(grid[i][j] for i in range(row//3*3, row//3*3+3) for j in range(col//3*3, col//3*3+3))
    
    # Retourne l'ensemble des valeurs possibles pour cette cellule
    return {1, 2, 3, 4, 5, 6, 7, 8, 9} - row_values - col_values - box_values


def solve_sudoku(grid):
    # Trouver la prochaine cellule à remplir
    next_cell = find_next_cell_to_fill(grid)
    
    # Vérifier si la grille est déjà remplie
    if not next_cell:
        return True  # La grille est remplie, retourne True (sudoku résolu)
    
    # Récupérer les possibilités pour la prochaine cellule
    row, col = next_cell
    possibilities = get_possible_values(grid, row, col)
    
    # Essayer chaque possibilité pour la prochaine cellule
    for num in possibilities:
        grid[row][col] = num  # Place le nombre dans la cellule
        
        # Appel récursif pour remplir le reste de la grille
        if solve_sudoku(grid):
            return True  # Si une solution est trouvée, retourne True
        
        # Si aucune solution n'est trouvée, réinitialise la cellule et essaie la prochaine possibilité
        grid[row][col] = 0
    
    return False  # Retourne False si aucune solution n'est trouvée pour cette branche de l'arbre de recherche
