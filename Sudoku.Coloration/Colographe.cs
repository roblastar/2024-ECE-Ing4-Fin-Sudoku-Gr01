using Sudoku.Shared;

namespace Sudoku.Coloration;
    public class Graphe
    {
        readonly SudokuGrid G;         //def de notre grille de sudoku qui passera sous forme de graphe
        List<Sommet> S;     //def des sommets de noitre graphe
        public const int ordre = 81;
        



        #pragma warning disable CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.
        public Graphe(SudokuGrid grille)          //warning inconnue sur graphe ??
        #pragma warning restore CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.
        {
            G = grille.CloneSudoku();
            CreaGraphe();
        }
        public SudokuGrid getGrid()
        {
            return G;
        }
        



        public void AffichageGrid()
        {
            Console.WriteLine("-----------------------------------");
            int i = 0;

            foreach (Sommet s in S)
            {
                if (i % 3 == 0)
                    Console.Write("| ");

                Console.Write("{0,2:#0} ", s.getCol()," ");
                i++;

                if (i % 9 == 0)
                    Console.WriteLine(" |");

                if (i % 27 == 0)
                    Console.WriteLine("-----------------------------------");
            }

            Console.WriteLine();
        }







        // Ajout de l'ensemble des sommets dans le graphe
        void CreaGraphe()
        {
            S = new List<Sommet>(); //crea d'une nouvelle liste de sommets pour réaliser la colorisation 

            int l, c;           //decla des lignes et colonne

            for (int i=0; i<ordre; i++)
            {
                l = (int)(i/9);
                c = i%9;
                S.Add(new Sommet(i, G.Cells[l][c]));
            }
            foreach (Sommet s in S)
            {
                s.Adjacent(S);          
            }
        }
        





        public void COLORISATION()
        {
            // Test : Si première case déjà colorier on part de cette couleu
            if (S.First().getCol() != 0)
                ColGraphe(0, 0, S.First().getCol());
            else
            {
                //sinon test des déffirentes couleur pour le premier sommets 
                for (int col=1; col<=9; col++)
                {
                    if (ColGraphe(0, 0, col))       //si premiere case attributaion de ca couleur trouver
                    {
                        break;     
                    }
                }
            }

            //nouveau sommets trouver on update notre graphe
            for (int i=0; i<S.Count; i++)
                G.Cells[(int)(i/9)][i%9] = S.ElementAt(i).getCol();
        }

       
        //boucle pour continuer tant que le graphe n 'est pas complet
        //true incomplet / flase complte 
        bool ColGraphe(int nbS, int nbCol, int col)
        {
            //Parcourt des sommets
            Sommet s = S.ElementAt(nbS);
            int buffer = 0;     

            //test si la case est deja colorié
            if (s.getCol() != 0)
                buffer = s.getCol(); //on garde la couleur trouver 
            else
            {
                //teste  de la couleur avec ses adjacents
                if (s.testCol(col))
                {
                    buffer = 0;
                    s.setCol(col);  //set de la nouvelle couleur du sommet en fonction des ses sommets adjacents
                }
            }

            //test si la case est deja colorié
            if (s.getCol() != 0)
            {
                nbCol++;
                nbS++;

                //condition d arrete si le sudoku est fini
                if (nbS == ordre)
                    return true;
               

                nbCol = nbCol % 9;      //remise à 0 du nombre de couleur si on attaint la 9eme 


                //test si la case suivante est deja colorié sinon on relabnce l'algo de colorisation de graphe 
                if (S.ElementAt(nbS).getCol() != 0)
                {
                    if (ColGraphe(nbS, nbCol, S.ElementAt(nbS).getCol()))
                        return true;        //graphe incomplet 
                }
                else
                {

                    for (int color=1; color<=9; color++)
                    {
                        if (ColGraphe(nbS, nbCol, color))
                            return true;         //graphe incomplet 
                    }
                }

                s.setCol(buffer);           //resolution n'a pas marche on  recupe la couleur stocker au debut pour notre sommets
            }
            
            return (nbS == ordre);
        }
    }


