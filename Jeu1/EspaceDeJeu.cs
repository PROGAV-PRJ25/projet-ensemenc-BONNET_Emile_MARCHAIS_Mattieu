
public class EspaceDeJeu
{
    public Terrain[,] CarteTerrains { get; private set; }
    public string[,] Grille { get; private set; }
    public int TailleX { get; private set; }
    public int TailleY { get; private set; }
    public int NombrePlanteMorte { get; set; }

    // Création de variables pour gérer la selection de l'inventaire
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

    // Création de variables pour gérer la selection des palntes à planter
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
    public int luminosité { get; set; } = 0;
    public bool ModeUrgence { get; set; } = false;
    public bool Retraite { get; set; } = false;
    public bool[,] EstLaboure { get; set; }
    public Plante Plantenull { get; } // Variable qui sert à géer le cas ou lorsqu'on cherche une plante qui n'existe pas 
    public List<Plante> Plantes = new List<Plante>();
    public List<string> PlantesDispo { get; set; } = new List<string> {"|Carotte|", "|Tomate|", "|Radis|", "|Salade|"};
    public List<string> ObjetBoutique { get; set; } = new List<string> { "Retraite", "Piment", "Melon", "Citrouille", "Fraise"};
    public List<string> Inventaire { get; set; } = new List<string> {"|Labourer| ", "|Planter| ", "|Récolter| ", "|Arroser| ", "|Frapper| ", "|Boutique| "};
    public Joueur Joueur { get; set; }
 
    

    public EspaceDeJeu(int tailleX, int tailleY, Joueur? joueur = null)
    {
        CarteTerrains = new Terrain[TailleX, TailleY];
        Grille = new string[TailleX, TailleY];
        TailleX = tailleX;
        TailleY = tailleY;
        NombrePlanteMorte = 0;
        EstLaboure = new bool[TailleX, TailleY];
        Plantenull = new Plante ("Plantenull", -1, -1, 0, 0, 0, this); 
        Joueur = joueur ?? new Joueur(0, 0);
        InitialiserGrille();
    }

    public void InitialiserGrille()
    /*Fonction servant à initialiser la grille de départ dans l'espace de jeu*/
    {
        Grille = new string[TailleX, TailleY];
        CarteTerrains = new Terrain[TailleX, TailleY];

        // Remplir avec un terrain par défaut
        for (int i = 0; i < TailleX; i++)
            for (int j = 0; j < TailleY; j++)
                CarteTerrains[i, j] = new Terrain("-");

        // Créer des patchs (3 à 5)
        string[] types = new string[] { "+", "*" };
        Random rnd = new Random();
        int nbPatchs = rnd.Next(3, 6);

        for (int p = 0; p < nbPatchs; p++)
        {
            string type = types[rnd.Next(types.Length)];
            int patchLargeur = rnd.Next(3, 6);
            int patchHauteur = rnd.Next(3, 6);
            int startX = rnd.Next(0, TailleX - patchLargeur);
            int startY = rnd.Next(0, TailleY - patchHauteur);

            for (int i = startX; i < startX + patchLargeur; i++)
            {
                for (int j = startY; j < startY + patchHauteur; j++)
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
    /*Fonction servant à redéfinir la grille à chaque étape*/
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
                if (EstLaboure[i, j] && Grille[i,j] != " B ")
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
    public void AfficherJeu()
    /*Fonction servant à afficher tout l'espace de jeu*/
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine();
        for (int i = 0; i < TailleX; i++)
        {
            Console.SetCursorPosition((Console.WindowWidth - TailleX*7)/2, Console.CursorTop);
            if(luminosité == 0 || luminosité >= 12)
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
                
                // Pour colorer le fond des terrains d'une couleur différente
                switch (CarteTerrains[i, j].Type)
                {
                    case "-": Console.BackgroundColor = ConsoleColor.DarkGreen; break;
                    case "+": Console.BackgroundColor = ConsoleColor.DarkCyan; break;
                    case "*": Console.BackgroundColor = ConsoleColor.DarkYellow; break;
                }
                
                // On colorie le Joueur en cyan
                if (Joueur.PositionX == j && Joueur.PositionY == i)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(Grille[i, j]);
                    Console.ForegroundColor = ConsoleColor.White;
                }

                // On colorie une plante en magenta si elle est malade, sinon si elle est prête à la récole elle est en blanc, et en noir sinon
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

                // Le reste est colorié en gris foncé
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(Grille[i, j]);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                
            }
            
            // On colorie les côté de la grille en noir s'il fait nuit et en gris foncé s'il fait jour
            if (luminosité == 0 || luminosité >= 12)
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
        Console.WriteLine("Pour bouger le Joueur : Z,Q,S,D \nPour selectionner une action dans l'invetaire : L,M (gauche, droite) // Pour choisir une plante : O,P (gauche, droite) // Pour effectuer une action : E ");
        Console.WriteLine();
        Console.WriteLine();
        AfficherInventaire(SelectInventaire);
        Console.WriteLine();
        Console.WriteLine();
        if (luminosité <= 4)
        {
            Console.Write("Luminosité: On est le matin, il fait sombre.");
        }
        else if (luminosité <= 8)
        {
            Console.Write("Luminosité: On est le midi, il fait jour.");
        }
        else if (luminosité <= 12)
        {
            Console.Write("Luminosité: On est le soir, il fait sombre.");
        }
        else if (luminosité <= 16)
        {
            Console.Write("Luminosité: On est la nuit, il fait nuit.");
        }
        Console.WriteLine("           Jours: " + Jours);
        Console.WriteLine();
        Console.WriteLine($"Actuellement {NombrePlanteMorte} sont mortent. Si 50 plantes meurent, vous avez perdu");
        Console.WriteLine();
        Console.WriteLine($"ARGENT : {Joueur.Argent} pièces");

        if (SelectionnerPlante(Joueur.PositionX, Joueur.PositionY) != Plantenull)
        {
            Plante plante = SelectionnerPlante(Joueur.PositionX, Joueur.PositionY);
            plante.AfficherPlanteStatistique();
        }
    }


    public void AfficherInventaire(int selection)
    /*Fonction pour afficher l'inventaire et surtout l'action que l'on séléctionne*/
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
    /*Fonction pour mettre à jours toutes les plantes de l'espace de jeu*/
    {


        foreach (var plante in Plantes)
        {
            if (Joueur.AmeliorationsAchetées.Contains("Irrigation automatique"))
            {
                plante.Hydratation = Math.Max(plante.Hydratation, 20);
            }
            plante.MetAJour();
        }

        int tempNombrePlanteMorte = Plantes.Count();

        // On supprome les plantes mortes de la liste
        Plantes = Plantes.Where(p => p.EsperanceDeVie > 0).ToList();
        NombrePlanteMorte += tempNombrePlanteMorte - Plantes.Count();

    }

    public Plante SelectionnerPlante(int x, int y)
    /*Fonction servant à séléctionner la plante se situant sur les coordonnée x, y*/
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