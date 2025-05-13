public class Joueur
{
    public int[] TableauRecolte { get; set; } // 0 = Carotte; 1 = Tomate; 2 = Radis; 3 = Salade
    public int JoueurPositionX { get; set; }

    public int JoueurPositionY { get; set; }
    public int Argent { get; set; } = 100;

    public bool AFrappe { get; set; }
    public GrilleDeJeu Grille { get; set; }

    public string Affichage { get; set; } = " J ";
    public Joueur(int x, int y)
    {
        JoueurPositionX = x;
        JoueurPositionY = y;
        AFrappe = false;
        TableauRecolte = new int[] {0,0,0,0};
    }
    
    public void MoveJoueur(int tempsBoucle)
    {
        char? action = null;
        var timer = new System.Diagnostics.Stopwatch();
        timer.Start();

        while (timer.ElapsedMilliseconds < tempsBoucle)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                action = Char.ToLower(key.KeyChar);
                break;
            }
        }
        timer.Stop();

        // If no input, do nothing
        if (action == null)
            {
                return;
            }
            

        int tempX = JoueurPositionX;
        int tempY = JoueurPositionY;

        switch (action)
        {
            case 'z': tempY--; break;
            case 's': tempY++; break;
            case 'q': tempX--; break;
            case 'd': tempX++; break;

            case 'l': Grille.SelectInventaire --; break;
            case 'm': Grille.SelectInventaire ++; break;
            case 'o': Grille.SelectPlante --; break;
            case 'p': Grille.SelectPlante ++; break;
            case 'e': Action(Grille.SelectInventaire); break;
            default: return; // Invalid key, skip
        }


        // Stay within bounds
        if (tempX >= 0 && tempX < Grille.TailleY && tempY >= 0 && tempY < Grille.TailleX)
        {
            JoueurPositionX = tempX;
            JoueurPositionY = tempY;
        }
    }


    public void Labourer()
    {
        Grille.EstLaboure[JoueurPositionY, JoueurPositionX] = true;
    }
    public void PlacePlante(Plante plante)
    {
        if (Grille.EstLaboure[JoueurPositionY, JoueurPositionX])
        {
            Grille.Plantes.Add(plante);
            Grille.EstLaboure[JoueurPositionY, JoueurPositionX] = false;
        }
        
    }

    public void Recolter()
    {
        Plante plante = Grille.SelectionnerPlante(JoueurPositionX, JoueurPositionY) ;
        if(plante.Type == "Plantenull")
        {
            Console.WriteLine("Il n'y à pas de plante à cette position");
        }
        else if (plante.cycleStep != 10)
        {
            Console.WriteLine("La plante n'a pas atteint sa maturité");
        }
        else
        {
            plante.EsperanceDeVie = 0;
            if (plante.Type == "Carotte")
            {
                TableauRecolte[0]++;
            }
            else if (plante.Type == "Tomate")
            {
                TableauRecolte[1]++;
            }
            else if (plante.Type == "Radis")
            {
                TableauRecolte[2]++;
            }
            else if (plante.Type == "Salade")
            {
                TableauRecolte[3]++;
            }
        }
    }

    public void Action(int selection)
    {
        if(Grille.ModeUrgence)
        {
            switch (selection)
            {
                case 0: Console.WriteLine("Vous ne pouvez pas labourer, il y a un intru. Eliminez le d'abord"); break;
                case 1: Console.WriteLine("Vous ne pouvez rien planter, il y a un intru. Eliminez le d'abord"); break;
                case 2: Console.WriteLine("Vous ne pouvez pas récolter, il y a un intru. Eliminez le d'abord"); break;
                case 3: ;break;
                case 4: Frapper(); break;
            }
            
        }
        else
        {
            switch (selection)
            {
                case 0: Labourer(); break;
                case 1: PlacePlante(ChoixPlante()); break;
                case 2: Recolter(); break;
                case 3: Arroser();break;
                case 4: Console.WriteLine("Il n'y a personne à frapper ici"); break;
                case 5: AccederBoutique(); break;
            }        
        }
        switch (selection)
        {
            case 0: Labourer(); break;
            case 1: PlacePlante(ChoixPlante()); break;
            case 2: Recolter(); break;
        }    
    }
    public Plante ChoixPlante()
    {
        int x = JoueurPositionX;
        int y = JoueurPositionY;
        string type = Grille.PlantesDispo[Grille.SelectPlante].Trim('|');

        return type switch
        {
            "Carotte" => new Plante(type, x, y, 30, 40, 6, Grille),
            "Tomate" => new Plante(type, x, y, 40, 50, 8, Grille),
            "Radis" => new Plante(type, x, y, 50, 30, 5, Grille),
            "Salade" => new Plante(type, x, y, 15, 60, 4, Grille),
            "Piment" => new Plante(type, x, y, 25, 50, 7, Grille),
            "Melon" => new Plante(type, x, y, 60, 60, 10, Grille),
            "Citrouille" => new Plante(type, x, y, 70, 70, 10, Grille),
            "Fraise" => new Plante(type, x, y, 20, 30, 6, Grille),
            _ => new Plante("Carotte", x, y, 30, 40, 6, Grille)
        };
    }


    public void Frapper()
    {
        if(Grille.Grille[JoueurPositionY,JoueurPositionX] == " E ")
        {
            AFrappe = true;
        }
    }

    public void Arroser()
    {
        Plante plante = Grille.SelectionnerPlante(JoueurPositionX, JoueurPositionY) ;
        plante.Hydratation = Math.Min(plante.Hydratation + 30, 100);
    }

    public void AccederBoutique()
    {
        if (JoueurPositionX != 0 || JoueurPositionY != 0)
        {
            Console.WriteLine("Vous devez être sur la case boutique (0,0) pour accéder à la boutique !");
            return;
        }

        Console.Clear();
        Console.WriteLine("🛒 Bienvenue à la Boutique !");
        Console.WriteLine($"💰 Votre argent : {Argent} pièces\n");

        var nomPlantes = Grille.PlantesBoutique;
        var prixPlantes = new Dictionary<string, int>
        {
            {"Piment", 25}, {"Melon", 30}, {"Citrouille", 35}, {"Fraise", 20}
        };

        // Filtrer uniquement les plantes encore non débloquées
        var plantesAchetables = nomPlantes
            .Where(p => !Grille.PlantesDispo.Contains($"|{p}|"))
            .ToList();

        if (plantesAchetables.Count == 0)
        {
            Console.WriteLine("Toutes les plantes spéciales ont été débloquées !");
            Console.ReadKey(true);
            return;
        }

        Console.WriteLine("Plantes spéciales à débloquer :");
        for (int i = 0; i < plantesAchetables.Count; i++)
        {
            string p = plantesAchetables[i];
            Console.WriteLine($"{i + 1}. {p} - {prixPlantes[p]} pièces");
        }

        Console.WriteLine("\nAppuyez sur le numéro correspondant à la plante à acheter, ou une autre touche pour quitter.");
        var input = Console.ReadKey(true).KeyChar;

        if (char.IsDigit(input))
        {
            int choix = int.Parse(input.ToString()) - 1;

            if (choix >= 0 && choix < plantesAchetables.Count)
            {
                string plante = plantesAchetables[choix];
                int prix = prixPlantes[plante];

                if (Argent >= prix)
                {
                    Argent -= prix;
                    Grille.PlantesDispo = Grille.PlantesDispo.Append($"|{plante}|").ToArray();
                    Console.WriteLine($"✅ Vous avez débloqué la plante : {plante} !");
                }
                else
                {
                    Console.WriteLine("❌ Pas assez d'argent !");
                }
            }
        }

        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
        Console.ReadKey(true);
    }


    public override string ToString()
    {
        return Affichage;
    }

}

