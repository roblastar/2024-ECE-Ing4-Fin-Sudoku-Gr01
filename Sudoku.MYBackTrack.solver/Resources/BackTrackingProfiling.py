from timeit import default_timer

#instance = ((0,0,0,0,9,4,0,3,0),
#           (0,0,0,5,1,0,0,0,7),
#           (0,8,9,0,0,0,0,4,0),
#           (0,0,0,0,0,0,2,0,8),
#           (0,6,0,2,0,1,0,5,0),
#           (1,0,2,0,0,0,0,0,0),
#           (0,7,0,0,0,0,5,2,0),
#           (9,0,0,0,6,5,0,0,0),
#           (0,4,0,9,7,0,0,0,0))

def solveSudoku(grid):
    """
    Résout le Sudoku en utilisant une approche de backtracking optimisée.
    """
    empty_cell = find_empty_location(grid)
    if not empty_cell:
        return True  # Toutes les cellules sont remplies, le Sudoku est résolu
    row, col = empty_cell
    
    # Essayer les chiffres de 1 à 9 dans la cellule vide
    for num in range(1, 10):
        if is_valid_move(grid, row, col, num):
            grid[row][col] = num  # Essayer le numéro
            if solveSudoku(grid):
                return True  # Si le Sudoku est résolu, terminer
            grid[row][col] = 0  # Annuler la tentative actuelle
    return False  # Aucune solution trouvée pour cette configuration

def find_empty_location(grid):
    """
    Trouve la prochaine cellule vide dans la grille et renvoie ses coordonnées.
    """
    for row in range(9):
        for col in range(9):
            if grid[row][col] == 0:
                return row, col
    return None

def is_valid_move(grid, row, col, num):
    """
    Vérifie si placer un numéro dans une certaine position est valide.
    """
    # Vérification de la ligne et de la colonne
    if num in grid[row] or num in [grid[i][col] for i in range(9)]:
        return False
    
    # Vérification de la sous-grille 3x3
    start_row, start_col = 3 * (row // 3), 3 * (col // 3)
    for i in range(3):
        for j in range(3):
            if grid[i + start_row][j + start_col] == num:
                return False
    
    return True


#start = default_timer()
if(solveSudoku(instance)):
	#print_grid(instance)
	r=instance
else:
	print ("Aucune solution trouvée")

#execution = default_timer() - start
#print("Le temps de résolution est de : ", execution, " seconds as a floating point value")