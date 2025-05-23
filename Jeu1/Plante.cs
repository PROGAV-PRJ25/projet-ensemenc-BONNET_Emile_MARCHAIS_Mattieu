public class Plante
{
    Random rnd = new Random();
    public int PositionX { get; set; }
    public int PositionY { get; set; }
    public int BesoinEau { get; set; }
    public int Hydratation { get; set; }
    public int BesoinLuminosite { get; set; } 
    public int EsperanceDeVie { get; set; }
    public int Progression = 0;
    public double TauxCroissance { get; set; }
    public Maladie? MaladieActuelle { get; set; }
    public string Type { get; set; }
    public string Affichage
    {
        get
        {
            if (Type == "Plantenull") return " . ";

            return Type switch
            {
                "Carotte" => Progression < 100 ? "c" : "C",
                "Tomate" => Progression < 100 ? "t" : "T",
                "Radis" => Progression < 100 ? "r" : "R",
                "Salade" => Progression < 100 ? "s" : "S",
                "Piment" => Progression < 100 ? "p" : "P",
                "Melon" => Progression < 100 ? "m" : "M",
                "Citrouille" => Progression < 100 ? "h" : "H",
                "Fraise" => Progression < 100 ? "f" : "F",
                _ => "?"
            };
        }
    }

    public EspaceDeJeu Grille { get; set; }


    public Plante(string type, int x, int y, int esperanceDeVie, int besoinEau, int besoinLuminosite, EspaceDeJeu grille)
    {
        Type = type;

        PositionX = x;
        PositionY = y;
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
        if (MaladieActuelle == null && rnd.NextDouble() < 0.01) // 1% de chance
        {
            Maladie maladiePotentielle = new Maladie(Type, 0, 0);
            maladiePotentielle.ContracterMaladie(this);
        }
        // Applique les effets si elle est déjà malade
        MaladieActuelle?.AppliquerEffet(this);

        // Tente de contaminer les voisines
        if (MaladieActuelle != null)
        {
            MaladieActuelle.PropagerMaladie(this, rnd, Grille);
        }


        RecalculerTauxCroissance();


        if (((rnd.NextDouble()*100) <= TauxCroissance) && (Progression < 100 ))
        {
            Progression+=5;
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
        int lumiDiff = Math.Abs(Grille.luminosité - BesoinLuminosite);
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
        string terrainSousPlante = Grille.CarteTerrains[PositionY, PositionX].Type;
        bool bonus = (Type == "Carotte" && terrainSousPlante == "*") ||
                    (Type == "Tomate" && terrainSousPlante == "+") ||
                    (Type == "Radis" && terrainSousPlante == "+") ||
                    (Type == "Salade" && terrainSousPlante == "-") ||
                    (Type == "Piment" && terrainSousPlante == "-") || 
                    (Type == "Melon" && terrainSousPlante == "*") ||
                    (Type == "Citrouille" && terrainSousPlante == "*") ||
                    (Type == "Fraise" && terrainSousPlante == "+");
                    

        if (bonus) taux += 10;

        TauxCroissance = Math.Clamp(taux, 0, 100);
    }
    public void AfficherPlanteStatistique()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("                                --------------------------------Statistique de la plante--------------------------------");
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.White;

        //Stats de base
        string stat = $"                   Type: {Type}                                                 Espérance de vie: {EsperanceDeVie}";
        Console.WriteLine(stat);

        //Appréciation du terrain
        string appréciation = $"";
        string bonus = $"                   Votre {Type} aime bien ce terrain";
        string neutre = $"                   Votre {Type} ne semble pas affectée par ce terrain";
        switch (Type, Grille.CarteTerrains[PositionY, PositionX].Type)
        {
            case ("Carotte", "*"): appréciation = bonus; break;
            case ("Tomate", "+"): appréciation = bonus; break;
            case ("Radis", "+"): appréciation = bonus; break;
            case ("Salade", "-"): appréciation = bonus; break;
            case ("Piment", "-"): appréciation = bonus; break;
            case ("Melon", "*"): appréciation = bonus; break;
            case ("Citrouille", "*"): appréciation = bonus; break;
            case ("Fraise", "+"): appréciation = bonus; break; 
            default : appréciation = neutre; break;
        }
        Console.WriteLine(appréciation);
        Console.WriteLine();

        //Hydratation
        Console.Write("                   Hydratation :           ");
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
                Console.Write("██▒▒▒▒▒▒▒▒ 20%"); break;
            }
            case 3 : 
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("███▒▒▒▒▒▒▒ 30%"); break;
            }
            case 4 : 
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("████▒▒▒▒▒▒ 40%"); break;
            }
            case 5 : 
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("█████▒▒▒▒▒ 50%"); break;
            }
            case 6 : 
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("██████▒▒▒▒ 60%"); break;
            }
            case 7 : 
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("███████▒▒▒ 70%"); break;
            }
            case 8 : 
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("████████▒▒ 80%"); break;
            }
            case 9 : 
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("█████████▒ 90%"); break;
            }
            case 10 : 
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("██████████ 100%"); break;
            }
        }
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("                        ");

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
        Console.WriteLine();

        //Taux de croissance
        Console.Write("                   Jauge de satisfaction:  ");
        switch(TauxCroissance/10)
        {
            case 0: 
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
            default :
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("▒▒▒▒▒▒▒▒▒▒ 0%"); break;    
            }
        }
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine();

        // Progression de grandissement
        string plante = "";
        switch(Progression/10)
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
        if (Progression == 10)
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
