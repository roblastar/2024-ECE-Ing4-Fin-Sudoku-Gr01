using System;
using System.Collections.Generic;
using Sudoku.Shared;

namespace Sudoku.Coloration
{
    public class ColorationSolver : ISudokuSolver
    {
        public SudokuGrid Solve(SudokuGrid grid)
        {
            int size = grid.Size;

            // Construction du graphe
            Graph graph = BuildGraph(grid);

            // Coloration du graphe
            Dictionary<int, int> colors = ColorGraph(graph);

            // Remplacement des valeurs dans la grille avec les couleurs attribuées
            SudokuGrid solution = new(size);
            foreach (var cell in colors)
            {
                int row = cell.Key / size;
                int col = cell.Key % size;
                solution.SetCellValue(row, col, cell.Value + 1); // Ajout de 1 car les couleurs commencent souvent à 1
            }

            return solution;
        }

        // Construit un graphe à partir de la grille de Sudoku
        private Graph BuildGraph(SudokuGrid grid)
        {
            int size = grid.Size;
            Graph graph = new Graph(size * size);

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    int cellIndex = row * size + col;
                    if (grid.GetCellValue(row, col) == 0)
                    {
                        for (int i = 0; i < size; i++)
                        {
                            // Ajoute des arêtes vers les cellules de la même ligne et colonne
                            if (i != col)
                                graph.AddEdge(cellIndex, row * size + i);
                            if (i != row)
                                graph.AddEdge(cellIndex, i * size + col);
                        }
                    }
                }
            }

            return graph;
        }

        // Coloration du graphe en utilisant l'algorithme de coloration de graphe de Welsh-Powell
        private Dictionary<int, int> ColorGraph(Graph graph)
        {
            Dictionary<int, int> colors = new Dictionary<int, int>();

            // Trie les sommets par degré décroissant
            List<int> sortedVertices = SortVerticesByDegree(graph);

            // Initialise les couleurs
            HashSet<int> usedColors = new HashSet<int>();

            // Attribue les couleurs aux sommets
            foreach (int vertex in sortedVertices)
            {
                HashSet<int> adjacentColors = GetAdjacentColors(graph, vertex, colors);
                int availableColor = 1;
                while (adjacentColors.Contains(availableColor))
                {
                    availableColor++;
                }

                colors[vertex] = availableColor;
                usedColors.Add(availableColor);
            }

            return colors;
        }

        private List<int> SortVerticesByDegree(Graph graph)
        {
            List<int> sortedVertices = new List<int>(graph.V);
            for (int vertex = 0; vertex < graph.V; vertex++)
            {
                sortedVertices.Add(vertex);
            }

            sortedVertices.Sort((v1, v2) => graph.adj[v2].Count.CompareTo(graph.adj[v1].Count));

            return sortedVertices;
        }

        private HashSet<int> GetAdjacentColors(Graph graph, int vertex, Dictionary<int, int> colors)
        {
            HashSet<int> adjacentColors = new HashSet<int>();
            foreach (int neighbor in graph.adj[vertex])
            {
                if (colors.ContainsKey(neighbor))
                {
                    adjacentColors.Add(colors[neighbor]);
                }
            }

            return adjacentColors;
        }

        internal class Graph
        {
            public int V;
            public List<List<int>> adj;

            public Graph(int v)
            {
                V = v;
                adj = new List<List<int>>();
                for (int i = 0; i < v; ++i)
                {
                    adj.Add(new List<int>());
                }
            }

            public void AddEdge(int v, int w)
            {
                adj[v].Add(w);
                adj[w].Add(v);
            }
        }
    }
}
