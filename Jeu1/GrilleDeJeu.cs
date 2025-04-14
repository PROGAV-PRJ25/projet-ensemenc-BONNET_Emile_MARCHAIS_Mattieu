
public class GrilleDeJeu
{
    public string[,] Grille { get; private set; }
    public int TailleX { get; private set; }
    public int TailleY { get; private set; }
    public string[] Inventaire {get; set;}
    public int SelectInventaire {get; set;}

    public List<Plante> Plantes = new List<Plante>();

    public Joueur Joueur { get; set; } 
    

    public GrilleDeJeu(int tailleX, int tailleY, Joueur? joueur = null, string[]? inventaire = null)
    {
        TailleX = tailleX;
        TailleY = tailleY;
        Joueur = joueur ?? new Joueur(0, 0); // Default player if none provided
        Grille = new string[TailleX, TailleY];
        if (inventaire == null)
        {
            Inventaire = new string[] {"|_| ", "|_| ", "|_| ", "|_| ", "|_| ", "|_| ", "|_| ", "|_| "}; 
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

    public void DefineGrille(int x, int y)
    {
        // Reset everything to empty
        for (int i = 0; i < TailleX; i++)
            for (int j = 0; j < TailleY; j++)
                Grille[i, j] = " . ";

        // Place all plants
        foreach (var plante in Plantes)
        {
            Grille[plante.PlantePositionY, plante.PlantePositionX] = " " + plante.Affichage + " ";
        }

        // Place player last
        Grille[y, x] = Joueur.Affichage;
    }

    public void UpdatePlantes()
    {
        foreach (var plante in Plantes)
        {
            plante.MetAJour();
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
        AfficherInventaire(SelectInventaire);
    }

    public void AfficherInventaire(int selection)
    {
        for(int i = 0; i < Inventaire.Length; i++ )
        {
            if (i == selection)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(Inventaire[i]);
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.Write(Inventaire[i]);
            }
        }
    }
}