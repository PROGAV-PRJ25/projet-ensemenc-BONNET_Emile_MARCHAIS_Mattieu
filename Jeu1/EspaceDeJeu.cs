
public class EspaceDeJeu
{
    public Terrain[,] CarteTerrains { get; private set; }
    public string[,] Grille { get; private set; }
    public int TailleX { get; private set; }
    public int TailleY { get; private set; }
    public int NombrePlanteMorte { get; set; }
    private int _selectInventaire;
    public int SelectInventaire
    {
        get => _selectInventaire;
        set
        {
            if (value < 0)
            {
                _selectInventaire = Inventaire.Count() - 1;
            }
            else if (value >= Inventaire.Count())
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
                _selectPlante = PlantesDispo.Count() - 1;
            }
            else if (value >= PlantesDispo.Count())
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
    public bool ModeUrgence { get; set; } = false;
    public bool Retraite { get; set; } = false;
    public bool[,] EstLaboure { get; set; }
    public Plante Plantenull { get; }
    public List<Plante> Plantes = new List<Plante>();
    public List<string> PlantesDispo { get; set; } = new List<string> {"|Carotte|", "|Tomate|", "|Radis|", "|Salade|"};
    public List<string> ObjetBoutique { get; set; } = new List<string> { "Piment", "Melon", "Citrouille", "Fraise", "Retraite" };
    public List<string> Inventaire { get; set; } = new List<string> {"|Labourer| ", "|Planter| ", "|Récolter| ", "|Arroser| ", "|Frapper| ", "|Boutique| ", "|_| ", "|_| "};
    public Joueur Joueur { get; set; }
 
    

    public EspaceDeJeu(int tailleX, int tailleY, Joueur? joueur = null)
    {
        CarteTerrains = new Terrain[TailleX, TailleY];
        Grille = new string[TailleX, TailleY];
        TailleX = tailleX;
        TailleY = tailleY;
        NombrePlanteMorte = 0;
        EstLaboure = new bool[TailleX, TailleY];
        Plantenull = new Plante ("Plantenull", -1, -1, 0, 40, 6, this);
        Joueur = joueur ?? new Joueur(0, 0);
        InitialiserGrille();
    }

    public void InitialiserGrille()
    {
        Grille = new string[TailleX, TailleY];
        CarteTerrains = new Terrain[TailleX, TailleY];

        // Remplir avec un terrain par défaut
        for (int i = 0; i < TailleX; i++)
            for (int j = 0; j < TailleY; j++)
                CarteTerrains[i, j] = new Terrain("-");

        // Créer des patchs (3 à 4)
        string[] types = new string[] { "+", "*" };
        Random rnd = new Random();
        int nbPatchs = rnd.Next(3, 5);

        for (int p = 0; p < nbPatchs; p++)
        {
            string type = types[rnd.Next(types.Length)];
            int patchWidth = rnd.Next(3, 6);
            int patchHeight = rnd.Next(3, 6);
            int startX = rnd.Next(0, TailleX - patchWidth);
            int startY = rnd.Next(0, TailleY - patchHeight);

            for (int i = startX; i < startX + patchWidth; i++)
            {
                for (int j = startY; j < startY + patchHeight; j++)
                {
                    CarteTerrains[i, j] = new Terrain(type);
                }
            }
        }

        for (int i = 0; i < TailleX; i++)
        {
            for (int j = 0; j < TailleY; j++)
            {
                Grille[i, j] = "   ";
            }
        }
    }

    public void DefinirGrille(int x, int y)
    {
        // Vider la grille pour mettre à jours
        for (int i = 0; i < TailleX; i++)
            for (int j = 0; j < TailleY; j++)
                Grille[i, j] = "   ";

        Grille[0, 0] = " B ";

        // Placer les labourages
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

        // Placer toutes les plantes
        foreach (var plante in Plantes)
        {
            Grille[plante.PositionY, plante.PositionX] = " " + plante.Affichage + " ";
        }

        // Placer le joueur en dernier
        Grille[y, x] = Joueur.Affichage;
    }
    public void AfficherGrille()
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine();
        for (int i = 0; i < TailleX; i++)
        {
            Console.SetCursorPosition((Console.WindowWidth - TailleX*9)/2, Console.CursorTop);
            if(luminosity == 0 || luminosity >= 12)
            {
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else
            {   
                Console.BackgroundColor = ConsoleColor.DarkGray;
            }
            Console.Write("                              ");
            for (int j = 0; j < TailleY; j++)
            {
                switch(CarteTerrains[i, j].Type)
                {
                    case "-": Console.BackgroundColor = ConsoleColor.DarkGreen; break;
                    case "+": Console.BackgroundColor = ConsoleColor.DarkCyan; break;
                    case "*": Console.BackgroundColor = ConsoleColor.DarkYellow; break;
                }
                if (Joueur.PositionX == j && Joueur.PositionY == i)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(Grille[i, j]);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (SelectionnerPlante(j, i) != Plantenull)
                {
                    Plante plante = SelectionnerPlante(j, i);
                    if (plante.MaladieActuelle != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                    }
                    else if (plante.Progression == 10)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Black; 
                    }
                    Console.Write(Grille[i, j]);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(Grille[i, j]);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                
            }
            if(luminosity == 0 || luminosity >= 12)
            {
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else
            {   
                Console.BackgroundColor = ConsoleColor.DarkGray;
            }
            Console.Write("                              ");
            Console.WriteLine();
        }
        Console.BackgroundColor = ConsoleColor.Black;
        Console.WriteLine();
        Console.WriteLine();
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
        Console.WriteLine();
        Console.WriteLine($"Actuellement {NombrePlanteMorte} sont mortent. Si 50 plantes meurent, vous avez perdu");
        Console.WriteLine($" Argent : {Joueur.Argent} pièces");

        if (SelectionnerPlante(Joueur.PositionX, Joueur.PositionY) != Plantenull)
        {
            Plante plante = SelectionnerPlante(Joueur.PositionX, Joueur.PositionY);
            plante.AfficherPlanteStatistique();
        }
    }


    public void AfficherInventaire(int selection)
    {
        for(int i = 0; i < Inventaire.Count; i++ )
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
        Console.WriteLine("");
        if (selection == 1)
        {
            for (int j = 0; j < PlantesDispo.Count(); j++)
            {
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
        public void UpdatePlantes()
    {
        foreach (var plante in Plantes)
        {
            plante.MetAJour();
        }

        int tempNombrePlanteMorte = Plantes.Count();
        Plantes = Plantes.Where(p => p.EsperanceDeVie > 0).ToList();
        NombrePlanteMorte += tempNombrePlanteMorte - Plantes.Count();

    }

    public Plante SelectionnerPlante(int x, int y)
    {
        foreach(Plante plante in Plantes)
        {
            if (plante.PositionX == x && plante.PositionY == y)
            {
                return plante;
            }
        }
        return Plantenull;
    }
}