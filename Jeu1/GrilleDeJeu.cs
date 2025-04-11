using System.Reflection.Emit;

public class GrilleDeJeu
{
    public string[,] Grille { get; private set; }
    public int TailleX { get; private set; }
    public int TailleY { get; private set; }
    public string[] Inventaire {get; set;}
    public int SelectInventaire {get; private set;}
    

    public GrilleDeJeu(int tailleX, int tailleY, string[]? inventaire = null)
    {
        TailleX = tailleX;
        TailleY = tailleY;
        Grille = new string[TailleX, TailleY];
        if (inventaire == null)
        {
            Inventaire = new string[] {"houe", "vide", "vide", "vide", "vide", "vide", "vide", "vide"}; 
        }
        else
        {
            Inventaire = inventaire;
        }
         
        InitialiserGrille();
    }

    public void InitialiserGrille()
    {
        for (int i = 0; i < TailleX; i++)
        {
            for (int j = 0; j < TailleY; j++)
            {
                Grille[i, j] = " . ";
            }
        }
    }

    public void ModifierGrille(int x, int y, string valeur)
    {
        if (x >= 0 && x < TailleX && y >= 0 && y < TailleY)
        {
            Grille[x, y] = valeur;
        }
    }
    public void AfficherGrille()
    {
        Console.Clear();
        for (int i = 0; i < TailleX; i++)
        {
            for (int j = 0; j < TailleY; j++)
            {
                Console.Write(Grille[i, j]);
            }
            Console.WriteLine();
        }
    }

    public void AfficherInvetaire()
    {
        foreach (string elem in Inventaire)
        {
            Console.Write(elem);
            Console.WriteLine();
        }
    }

   /* public void SelectionnerInventaire()
    {
        bool again = true;
        while (again) //Tant que l'on ne rentre pas un mouvement valide
        {
            again = false;

            //On ajoute en variable temporaire les position X et Y
            int tempX = JoueurPositionX;
            
            switch (action)
            {
                case 'z':
                    tempY--;
                    break;
                case 'd':
                    tempX++;
                    break;
                case 's':
                    tempY++;
                    break;
                case 'q':
                    tempX--;
                    break;
                default: // Sécurité si le joueur appuie sur une touche non-valide. Cela recommence l'action.
                    again = true;
                    break;
            }
    }*/
}