public class Plante
{
    Random rnd = new Random();
    public string Type { get; set; }
    public int EsperanceDeVie { get; set; }
    public int PlantePositionX { get; set; }
    public int PlantePositionY { get; set; }
    public int BesoinEau { get; set; }
    public int Hydratation { get; set; }
    public Maladie? MaladieActuelle { get; set; }

    public int BesoinLuminosite { get; set; } 
    public double TauxCroissance { get; set; }
    public int cycleStep = 0;
    public string Affichage
    {
        get
        {
            if (Type == "Plantenull") return " . ";

            return Type switch
            {
                "Carotte" => cycleStep < 10 ? "c" : "C",
                "Tomate" => cycleStep < 10 ? "t" : "T",
                "Radis" => cycleStep < 10 ? "r" : "R",
                "Salade" => cycleStep < 10 ? "s" : "S",
                _ => "?"
            };
        }
    }

    public GrilleDeJeu Grille { get; set; }


    public Plante(string type, int x, int y, int esperanceDeVie, int besoinEau, int besoinLuminosite, GrilleDeJeu grille)
    {
        Type = type;

        PlantePositionX = x;
        PlantePositionY = y;
        EsperanceDeVie = esperanceDeVie;
        BesoinEau = besoinEau;
        Hydratation = 100; // Commence à 100%
        BesoinLuminosite = besoinLuminosite;
        Grille = grille;
    }

    public void MetAJour()
    {
        Hydratation -= 5;
        EsperanceDeVie--;

        // Vérifie si elle tombe malade (très faible chance)
        if (MaladieActuelle == null && rnd.NextDouble() < 0.01)
        {
            ContracterMaladie();
        }

        // Applique les effets si elle est déjà malade
        MaladieActuelle?.AppliquerEffet(this);

        // Tente de contaminer les voisines
        if (MaladieActuelle != null)
        {
            PropagerMaladie();
        }

        RecalculerTauxCroissance();


        if (((rnd.NextDouble()*100) <= TauxCroissance) && (cycleStep < 10 ))
        {
            cycleStep++;
        }

        if (TauxCroissance < 1)
        {
            EsperanceDeVie = 0; // La plante meurt d'insatisfaction
        }
        else if (TauxCroissance < 10)
        {
            if ((rnd.NextDouble()*100) <= 50)
                EsperanceDeVie = 0; // 1/2 chance de mourrir d'insatisfaction
        }
        else if (TauxCroissance < 20)
        {
            if ((rnd.NextDouble()*100) <= 30)
                EsperanceDeVie = 0; // 1/3 chance de mourrir d'insatisfaction
        }
    }

    private void RecalculerTauxCroissance()
    {
        double taux = 100;

        // Impact de la luminosité
        int lumiDiff = Math.Abs(Grille.luminosity - BesoinLuminosite);
        if (lumiDiff <= 1)
            taux += 10;
        else if (lumiDiff <= 3)
            taux -= 10;
        else
            taux -= 30;

        // Impact de l'eau
        int eauDiff = Hydratation - BesoinEau;
        if (eauDiff >= 20)
            taux += 10;
        else if (eauDiff < 0)
            taux -= 30;
        else
            taux -= 10;

        // Ajustement par l’âge
        if (EsperanceDeVie < 10)
            taux -= 20;
        string terrainSousPlante = Grille.CarteTerrains[PlantePositionY, PlantePositionX].Type;
        bool bonus = (Type == "Carotte" && terrainSousPlante == "*") ||
                    (Type == "Tomate" && terrainSousPlante == "+") ||
                    (Type == "Radis" && terrainSousPlante == "+") ||
                    (Type == "Salade" && terrainSousPlante == "-");

        if (bonus) taux += 10;

        TauxCroissance = Math.Clamp(taux, 0, 100);
    }

    private void ContracterMaladie()
    {
        // Maladie unique par type
        switch (Type)
        {
            case "Tomate":
                MaladieActuelle = new Maladie("MildiouTomate", 10, 15);
                break;
            case "Carotte":
                MaladieActuelle = new Maladie("RouilleCarotte", 20, 10);
                break;
            case "Radis":
                MaladieActuelle = new Maladie("PourritureRadis", 15, 20);
                break;
            case "Salade":
                MaladieActuelle = new Maladie("TacheSalade", 5, 15);
                break;
        }
    }

    private void PropagerMaladie()
    {
        var offsets = new (int dx, int dy)[] { (-1, 0), (1, 0), (0, -1), (0, 1) };
        foreach (var (dx, dy) in offsets)
        {
            int nx = PlantePositionX + dx;
            int ny = PlantePositionY + dy;

            if (nx >= 0 && nx < Grille.TailleY && ny >= 0 && ny < Grille.TailleX)
            {
                var voisine = Grille.SelectionnerPlante(nx, ny);
                if (voisine.Type == this.Type && voisine.MaladieActuelle == null)
                {
                    if (rnd.NextDouble() < 0.3) // propagation élevée
                    {
                        voisine.MaladieActuelle = new Maladie(this.MaladieActuelle.Type, this.MaladieActuelle.Duree, this.MaladieActuelle.Severite);
                    }
                }
            }
        }
    }


    public void AfficherPlanteStatistique()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("--------------------------------Statistique de la plante--------------------------------");
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.White;

        //Stats de base
        string stat = $"Type: {Type}, Espérance de vie: {EsperanceDeVie}";
        Console.WriteLine(stat);
        Console.WriteLine();

        //Hydratation
        Console.Write("Hydratation:  ");
        switch(Hydratation/10)
        {
            case 0 : 
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("▒▒▒▒▒▒▒▒▒▒ 0%"); break;
            }
            case 1 : 
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("█▒▒▒▒▒▒▒▒▒ 10%"); break;
            }
            case 2 : 
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("██▒▒▒▒▒▒▒▒ 20%"); break;
            }
            case 3 : 
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("███▒▒▒▒▒▒▒ 30%"); break;
            }
            case 4 : 
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("████▒▒▒▒▒▒ 40%"); break;
            }
            case 5 : 
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("█████▒▒▒▒▒ 50%"); break;
            }
            case 6 : 
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("██████▒▒▒▒ 60%"); break;
            }
            case 7 : 
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("███████▒▒▒ 70%"); break;
            }
            case 8 : 
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("████████▒▒ 80%"); break;
            }
            case 9 : 
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("█████████▒ 90%"); break;
            }
            case 10 : 
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("██████████ 100%"); break;
            }
        }
        Console.ForegroundColor = ConsoleColor.White;

        //Maladie
        string maladie;
        if (MaladieActuelle == null)
        {
            maladie = "La plante est en parfaite santé";
        }
        else
        {
            maladie = $"La plante possède {MaladieActuelle.Type} il reste {MaladieActuelle.Duree} avant qu'elle guerisse.";
        }
        Console.WriteLine(maladie);

        //Taux de croissance
        Console.Write("Jauge de satisfaction:  ");
        switch(TauxCroissance/10)
        {
            case 0 : 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("▒▒▒▒▒▒▒▒▒▒ 0%"); break;
            }
            case 1 : 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("█▒▒▒▒▒▒▒▒▒ 10%"); break;
            }
            case 2 : 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("██▒▒▒▒▒▒▒▒ 20%"); break;
            }
            case 3 : 
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("███▒▒▒▒▒▒▒ 30%"); break;
            }
            case 4 : 
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("████▒▒▒▒▒▒ 40%"); break;
            }
            case 5 : 
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("█████▒▒▒▒▒ 50%"); break;
            }
            case 6 : 
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("██████▒▒▒▒ 60%"); break;
            }
            case 7 : 
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("███████▒▒▒ 70%"); break;
            }
            case 8 : 
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("████████▒▒ 80%"); break;
            }
            case 9 : 
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("█████████▒ 90%"); break;
            }
            case 10 : 
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("██████████ 100%"); break;
            }
        }
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine();

        // Progression de grandissement
        string plante = "";
        switch(cycleStep)
        {
            case 1: 
            {
                plante =  "                          \n";
                plante += "                          \n";
                plante += "                          \n";
                plante += "                    ( * )   ";
                break;
            }
            case 2: 
            {
                plante =  "                          \n";
                plante += "                          \n";
                plante += "                      |   \n";
                plante += "                    ( * )   ";
                break;
            }
            case 3: 
            {
                plante =  "                          \n";
                plante += "                          \n";
                plante += "                      |   \n";
                plante += "                    ( * )   ";
                break;
            }
            case 4: 
            {
                plante =  "                          \n";
                plante += "                      |   \n";
                plante += "                      |   \n";
                plante += "                    ( * )   ";
                break;
            }
            case 5: 
            {
                plante =  "                          \n";
                plante += "                      |   \n";
                plante += "                      |   \n";
                plante += "                    ( * )   ";
                break;
            }
            case 6: 
            {
                plante =  "                          \n";
                plante += "                      |   \n";
                plante += "                      |/   \n";
                plante += "                    ( * )   ";
                break;
            }
            case 7: 
            {
                plante =  "                      o    \n";
                plante += "                      |   \n";
                plante += "                      |/  \n";
                plante += "                    ( * )   ";
                break;
            }
            case 8: 
            {
                plante =  "                      o   \n";
                plante += "                      | / \n";
                plante += "                      |/  \n";
                plante += "                    ( * )   ";
                break;
            }
            case 9: 
            {
                plante =  "                      o   \n";
                plante += "                      | / \n";
                plante += "                      |/  \n";
                plante += "                    ( * )   ";
                break;
            }
            case 10: 
            {
                plante =  "                    ( o ) \n";
                plante += "                      | / \n";
                plante += "                      |/  \n";
                plante += "                    ( * )   ";
                break;
            }
        }
        if (cycleStep == 10)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(plante);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Votre plante est arrivée à maturité, Bravo !!! Vous pouvez la récupérer");
        }
        else 
        {
            Console.WriteLine(plante);
        }
    }
    public override string ToString()
    {
        return $"Type: {Type}, Espérance de vie: {EsperanceDeVie}, Hydratation actuelle {Hydratation}";
    }
}
