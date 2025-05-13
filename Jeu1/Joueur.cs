public class Joueur
{
    public int[] TableauRecolte { get; set; } // 0 = Carotte; 1 = Tomate; 2 = Radis; 3 = Salade 
    public int PositionX { get; set; }

    public int PositionY { get; set; }
    public int Argent { get; set; } = 100;

    public bool AFrappe { get; set; }
    public EspaceDeJeu Grille { get; set; }

    public string Affichage { get; set; } = " J ";


    public Joueur(int x, int y)
    {
        PositionX = x;
        PositionY = y;
        AFrappe = false;
        TableauRecolte = new int[] {0,0,0,0,0,0,0,0};
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

        // Si aucune instruction rentrée, on ne fait rien 
        if (action == null)
            {
                return;
            }
            
        int tempX = PositionX; //VAriables temporaires
        int tempY = PositionY;

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
            default: return; 
        }

        if (tempX >= 0 && tempX < Grille.TailleY && tempY >= 0 && tempY < Grille.TailleX)
        {
            PositionX = tempX;
            PositionY = tempY;
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
    }

    public void Labourer()
    {
        Grille.EstLaboure[PositionY, PositionX] = true;
    }
    public void PlacePlante(Plante plante)
    {
        if (Grille.EstLaboure[PositionY, PositionX])
        {
            Grille.Plantes.Add(plante);
            Grille.EstLaboure[PositionY, PositionX] = false;
        }
        
    }

    public void Recolter()
    {
        Plante plante = Grille.SelectionnerPlante(PositionX, PositionY) ;
        if(plante.Type == "Plantenull")
        {
            Console.WriteLine("Il n'y à pas de plante à cette position");
        }
        else if (plante.Progression != 10)
        {
            Console.WriteLine("La plante n'a pas atteint sa maturité");
        }
        else
        {
            Grille.Plantes.Remove(plante);
            if (plante.Type == "Carotte")
            {
                TableauRecolte[0]++;
                Argent += 10;
            }
            else if (plante.Type == "Tomate")
            {
                TableauRecolte[1]++;
                Argent += 10;
            }
            else if (plante.Type == "Radis")
            {
                TableauRecolte[2]++;
                Argent += 10;
            }
            else if (plante.Type == "Salade")
            {
                TableauRecolte[3]++;
                Argent += 10;
            }
            else if (plante.Type == "Piment")
            {
                TableauRecolte[4]++;
                Argent += 15;
            }
            else if (plante.Type == "Melon")
            {
                TableauRecolte[5]++;
                Argent += 20;
            }
            else if (plante.Type == "Citrouille")
            {
                TableauRecolte[6]++;
                Argent += 25;
            }
            else if (plante.Type == "Fraise")
            {
                TableauRecolte[7]++;
                Argent += 30;
            }
        }
    }
    public Plante ChoixPlante()
    {
        int x = PositionX;
        int y = PositionY;
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

    public void Arroser()
    {
        Plante plante = Grille.SelectionnerPlante(PositionX, PositionY) ;
        plante.Hydratation = Math.Min(plante.Hydratation + 30, 100);
    }

    public void Frapper()
    {
        if(Grille.Grille[PositionY,PositionX] == " E ")
        {
            AFrappe = true;
        }
    }

    public void AccederBoutique()
    {
        if (PositionX != 0 || PositionY != 0)
        {
            Console.WriteLine("Vous devez être sur la case boutique (0,0) pour accéder à la boutique !");
            Console.ReadKey(true);
            return;
        }

        var prixPlantes = new Dictionary<string, int>
        {
            {"Piment", 25}, {"Melon", 30}, {"Citrouille", 35}, {"Fraise", 20}
        };

        // Liste des plantes non encore achetées
        var plantesAchetables = Grille.PlantesBoutique
            .Where(p => !Grille.PlantesDispo.Contains($"|{p}|"))
            .ToList();

        if (plantesAchetables.Count == 0)
        {
            Console.Clear();
            Console.WriteLine("🛒 Toutes les plantes spéciales ont déjà été débloquées !");
            Console.ReadKey(true);
            return;
        }

        int selection = 0;
        bool enCours = true;

        while (enCours)
        {
            Console.Clear();
            Console.WriteLine("🛒 Boutique - Plantes spéciales à débloquer :");
            Console.WriteLine($"💰 Argent : {Argent} pièces\n");

            for (int i = 0; i < plantesAchetables.Count; i++)
            {
                string plante = plantesAchetables[i];
                Console.WriteLine("\n");
                if (i == selection)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"|{plante}| ");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.Write($"|{plante}| ");
                }
            }

            Console.WriteLine("\n\nUtilise 'z'/'s' pour naviguer, 'e' pour acheter, 'x' pour quitter.");

            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.KeyChar)
            {
                case 'z':
                    selection = (selection - 1 + plantesAchetables.Count) % plantesAchetables.Count;
                    break;
                case 's':
                    selection = (selection + 1) % plantesAchetables.Count;
                    break;
                case 'e':
                    string choix = plantesAchetables[selection];
                    int prix = prixPlantes[choix];
                    if (Argent >= prix)
                    {
                        Argent -= prix;
                        Grille.PlantesDispo.Add($"|{choix}|");
                        Console.WriteLine($"\n✅ {choix} débloquée pour la plantation !");
                        plantesAchetables.RemoveAt(selection);
                        if (plantesAchetables.Count == 0)
                        {
                            Console.WriteLine("\nToutes les plantes ont été achetées !");
                            enCours = false;
                        }
                        else
                        {
                            selection %= plantesAchetables.Count;
                        }
                        Console.ReadKey(true);
                    }
                    else
                    {
                        Console.WriteLine("\n❌ Pas assez d'argent !");
                        Console.ReadKey(true);
                    }
                    break;
                case 'x':
                    enCours = false;
                    break;
            }
        }
    }

    public override string ToString()
    {
        return Affichage;
    }

}

