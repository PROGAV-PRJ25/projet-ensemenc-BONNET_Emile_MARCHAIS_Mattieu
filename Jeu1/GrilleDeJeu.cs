
public class GrilleDeJeu
{
    public string[,] Grille { get; private set; }
    public int TailleX { get; private set; }
    public int TailleY { get; private set; }
    public bool ModeUrgence {get; set;}
    public string[] Inventaire {get; set;}

    public string[] PlantesDispo {get; set;}

    private int _selectInventaire;
    public int SelectInventaire
    {
        get => _selectInventaire;
        set
        {
            if (value < 0)
            {
                _selectInventaire = Inventaire.Length - 1;
            }
            else if (value >= Inventaire.Length)
            {
                _selectInventaire = 0;
            }
            else
            {
                _selectInventaire = value;
            }
        }
    }
    private int _selectPlante;
    public int SelectPlante
    {
        get => _selectPlante;
        set
        {
            if (value < 0)
            {
                _selectPlante = PlantesDispo.Length - 1;
            }
            else if (value >= PlantesDispo.Length)
            {
                _selectPlante = 0;
            }
            else
            {
                _selectPlante = value;
            }
        }
    }

    public int Jours { get; set; } = 0;
    public int luminosity { get; set; } = 0;

    public bool[,] EstLaboure { get; set; }
    public Plante Plantenull { get; }


    public List<Plante> Plantes = new List<Plante>();

    public Joueur Joueur { get; set; } 
    public Rongeur Rongeur {get; set;}
    

    public GrilleDeJeu(int tailleX, int tailleY, Joueur? joueur = null, string[]? inventaire = null, string[]? plantesDispo = null)
    {
        TailleX = tailleX;
        TailleY = tailleY;
        ModeUrgence = false;
        EstLaboure = new bool[TailleX, TailleY];
        Joueur = joueur ?? new Joueur(0, 0); // Default player if none provided
        Grille = new string[TailleX, TailleY];
        Plantenull = new Plante ("Plantenull", -1, -1, 0, 40, 6, this);
        if (inventaire == null)
        {
            Inventaire = new string[] {"|Labourer| ", "|Planter| ", "|Récolter| ", "|Arroser| ", "|Frapper| ", "|_| ", "|_| ", "|_| "}; 
        }
        else
        {
            Inventaire = inventaire;
        }
        if (plantesDispo == null)
        {
            PlantesDispo = new string[] {"|Carotte|", "|Tomate|", "|Radis|", "|Salade|"};
        }
        else
        {
            PlantesDispo = plantesDispo;
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

    public Plante SelectionnerPlante(int x, int y)
    {
        foreach(Plante plante in Plantes)
        {
            if (plante.PlantePositionX == x && plante.PlantePositionY == y)
            {
                return plante;
            }
        }
        return Plantenull;
    }

    public void DefineGrille(int x, int y)
    {
        // Reset everything to empty
        for (int i = 0; i < TailleX; i++)
            for (int j = 0; j < TailleY; j++)
                Grille[i, j] = " . ";

        // PLace labourage
        for (int i = 0; i < TailleX; i++)
        {
            for (int j = 0; j < TailleY; j++)
            {
                if (EstLaboure[i, j])
                {
                    Grille[i, j] = " # ";
                }
            }
        }

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

        Plantes = Plantes.Where(p => (p.EsperanceDeVie > 0)).ToList();
    }


    public void AfficherGrille()
    {
        Console.Clear();
        for (int i = 0; i < TailleX; i++)
        {
            for (int j = 0; j < TailleY; j++)
            {
                if (SelectionnerPlante(j,i) == Plantenull)
                {
                    Console.Write(Grille[i, j]);
                }
                else if (SelectionnerPlante(j,i)?.cycleStep == 5)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(Grille[i, j]);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.Write(Grille[i, j]);
                }
                
            }
            Console.WriteLine();
        }
        AfficherInventaire(SelectInventaire);
        Console.WriteLine("\nJours: " + Jours);
        if (luminosity <= 4)
        {
            Console.WriteLine("Luminosité: On est le matin, il fait sombre.");
        }
        else if (luminosity <= 8)
        {
            Console.WriteLine("Luminosité: On est le midi, il fait jour.");
        }
        else if (luminosity <= 12)
        {
            Console.WriteLine("Luminosité: On est le soir, il fait sombre.");
        }
        else if (luminosity <= 16)
        {
            Console.WriteLine("Luminosité: On est la nuit, il fait nuit.");
        }

        foreach (var plante in Plantes)
        {
            if (Joueur.JoueurPositionX == plante.PlantePositionX && Joueur.JoueurPositionY == plante.PlantePositionY)
            {
                Console.WriteLine(plante);
            }

        }
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
        if (selection == 1)
        {
            for (int j = 0; j < PlantesDispo.Length; j++)
            {
                Console.Write("\n");
                if (j == SelectPlante)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(PlantesDispo[j]);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.Write(PlantesDispo[j]);
                }            
            }
        }
    }
}