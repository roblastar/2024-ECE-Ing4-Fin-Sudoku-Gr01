using System.Collections.Generic;

namespace Sudoku.Coloration;

    public class Sommet
    {

        List<Sommet> adjacent;

        int id;               // Indice de la case du sudoku
        int col;          //couleur affecte à cette case
        int L, C;           //indice ligen colone
        int BlocSudok;             //indice du bloc ou se situe la case 



        //getter et setter pour recuperer et mettre a jour la couleur des sommets du graphe 
        public int getCol()
        {
            return col;
        }
        public void setCol(int color)
        {
            col = color;
        }



        public Sommet(int I, int color)         //constructeur des differents sommets qui compose notre graphe avec tous ses attributs
        {
            id = I;
            col = color;
            L = (int)(id/9);
            C = id%9;
            if (L < 3)
                BlocSudok = (int)(C / 3);
            else if (L < 6)
                BlocSudok = 3 + (int)(C / 3);
            else if (L < 9)
                BlocSudok = 6 + (int)(C / 3);
            adjacent = new List<Sommet>();
        }







      
        public bool testCol(int color)
        {
            foreach (Sommet s in adjacent)          
                if (s.col == color)             //test pour affecter les couleurs a chauqe sommets 
                    return false;               //couleur ne pas etre utilisé
            return true;        //couleur peut etre utilisé
        }



        //Détermination des adjacents
        public void Adjacent(List<Sommet> sommets)
        {
            adjacent = new List<Sommet>();
            foreach (Sommet s in sommets)
            {
                if (s != this)
                {
                    if (s.L == L || s.C == C || s.BlocSudok == BlocSudok)
                        adjacent.Add(s);
                }
            }
        }



        
    }
