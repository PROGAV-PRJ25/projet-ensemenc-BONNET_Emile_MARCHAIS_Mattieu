public class Joueur
{
    public int[] TableauRecolte { get; set; } // Tableau de récolte en foction de la plante
    public int PrixRetraite { get; set; } = 1000; // Prix de la victoire
    public int PositionX { get; set; } // Position du joueur sur la grille
    public int PositionY { get; set; } // Position du joueur sur la grille
    public int Argent { get; set; } = 5;

    public HashSet<string> AmeliorationsAchetées { get; set; } = new HashSet<string>(); // Liste des améliorations achetées

    public bool AFrappe { get; set; } // Indique si le joueur a frappé un rongeur
    public EspaceDeJeu Grille { get; set; } 

    public string Affichage { get; set; } = " J "; 


    public Joueur(int x, int y)
    {
        PositionX = x;
        PositionY = y;
        AFrappe = false;
        TableauRecolte = new int[] {0,0,0,0,0,0,0,0}; 
    }
    
    public void MoveJoueur(int tempsBoucle) // Déplace le joueur sur la grille 
    {
        char? action = null;
        var timer = new System.Diagnostics.Stopwatch(); // Timer pour permettre de faire avancer le temps même si le joueur ne fait rien
        timer.Start();

        while (timer.ElapsedMilliseconds < tempsBoucle) // On attend que le temps soit écoulé ou qu'un action soit effectuée
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
            
        int tempX = PositionX; //Variables temporaires
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

        if (tempX >= 0 && tempX < Grille.TailleY && tempY >= 0 && tempY < Grille.TailleX) // Une fois action prise on change la position du joueur avec certaines sécurités
        {
            PositionX = tempX;
            PositionY = tempY;
        }
    }


    public void Action(int selection) // Action du joueur en fonction de la sélection
    {
        if(Grille.ModeUrgence) // Si le mode urgence est activé, on ne peut faire aucune autre action
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
        int x = PositionX;
        int y = PositionY;

        if (AmeliorationsAchetées.Contains("Motoculteur")) 
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    int nx = x + dx;
                    int ny = y + dy;
                    if (nx >= 0 && nx < Grille.TailleY && ny >= 0 && ny < Grille.TailleX)
                        Grille.EstLaboure[ny, nx] = true;
                }
            }
        }
        else
        {
            Grille.EstLaboure[y, x] = true;
        }
    }

    public void PlacePlante(Plante plante)
    {
        if (Grille.EstLaboure[PositionY, PositionX])
        {
            Grille.Plantes.Add(plante);
            if (!AmeliorationsAchetées.Contains("Engrais"))
            {
                Grille.EstLaboure[PositionY, PositionX] = false;
            }
        }
    }


    public void Recolter()
    {
        Plante plante = Grille.SelectionnerPlante(PositionX, PositionY);
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
                TableauRecolte[0]++; // Incrémenter le compteur de chaque plante
                int gainBase = 5; //Indique la valeur de base de la plante
                double multiplicateur = 1;
                if (AmeliorationsAchetées.Contains("Insecticide"))
                {
                    multiplicateur = new Random().NextDouble() * 1.5 + 0.5;
                }
                Argent += (int)(gainBase * multiplicateur);
            }
            else if (plante.Type == "Tomate")
            {
                TableauRecolte[1]++;
                int gainBase = 10;
                double multiplicateur = 1;
                if (AmeliorationsAchetées.Contains("Insecticide"))
                {
                    multiplicateur = new Random().NextDouble() * 1.5 + 0.5;
                }
                Argent += (int)(gainBase * multiplicateur);
            }
            else if (plante.Type == "Radis")
            {
                TableauRecolte[2]++;
                int gainBase = 10;
                double multiplicateur = 1;
                if (AmeliorationsAchetées.Contains("Insecticide"))
                {
                    multiplicateur = new Random().NextDouble() * 1.5 + 0.5;
                }
                Argent += (int)(gainBase * multiplicateur);
            }
            else if (plante.Type == "Salade")
            {
                TableauRecolte[3]++;
                int gainBase = 12;
                double multiplicateur = 1;
                if (AmeliorationsAchetées.Contains("Insecticide"))
                {
                    multiplicateur = new Random().NextDouble() * 1.5 + 0.5;
                }
                Argent += (int)(gainBase * multiplicateur);
            }
            else if (plante.Type == "Piment")
            {
                TableauRecolte[4]++;
                int gainBase = 15;
                double multiplicateur = 1;
                if (AmeliorationsAchetées.Contains("Insecticide"))
                {
                    multiplicateur = new Random().NextDouble() * 1.5 + 0.5;
                }
                Argent += (int)(gainBase * multiplicateur);
            }
            else if (plante.Type == "Melon")
            {
                TableauRecolte[5]++;
                int gainBase = 17;
                double multiplicateur = 1;
                if (AmeliorationsAchetées.Contains("Insecticide"))
                {
                    multiplicateur = new Random().NextDouble() * 1.5 + 0.5;
                }
                Argent += (int)(gainBase * multiplicateur);
            }
            else if (plante.Type == "Citrouille")
            {
                TableauRecolte[6]++;
                int gainBase = 22;
                double multiplicateur = 1;
                if (AmeliorationsAchetées.Contains("Insecticide"))
                {
                    multiplicateur = new Random().NextDouble() * 1.5 + 0.5;
                }
                Argent += (int)(gainBase * multiplicateur);
            }
            else if (plante.Type == "Fraise")
            {
                TableauRecolte[7]++;
                int gainBase = 26;
                double multiplicateur = 1;
                if (AmeliorationsAchetées.Contains("Insecticide"))
                {
                    multiplicateur = new Random().NextDouble() * 1.5 + 0.5;
                }
                Argent += (int)(gainBase * multiplicateur);
            }
        }
    }
    public Plante ChoixPlante()
    {
        int x = PositionX;
        int y = PositionY;
        string type = Grille.PlantesDispo[Grille.SelectPlante].Trim('|');

        return type switch // Chaque plante est créée avec ses caractéristiques ici 
        {
            "Carotte" => new Plante(type, x, y, 50, 10, 4, Grille),
            "Tomate" => new Plante(type, x, y, 40, 30, 8, Grille),
            "Radis" => new Plante(type, x, y, 70, 70, 5, Grille),
            "Salade" => new Plante(type, x, y, 30, 60, 4, Grille),
            "Piment" => new Plante(type, x, y, 30, 30, 7, Grille),
            "Melon" => new Plante(type, x, y, 30, 50, 10, Grille),
            "Citrouille" => new Plante(type, x, y, 20, 30, 10, Grille),
            "Fraise" => new Plante(type, x, y, 20, 70, 12, Grille),
            _ => new Plante("Carotte", x, y, 50, 10, 4, Grille)
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

        var prixObjet = new Dictionary<string, int> // Chaque objet de la boutique avec son prix
        {
            {"Piment", 25}, {"Melon", 30}, {"Citrouille", 35}, {"Fraise", 40},
            {"Capitalisme", 10}, {"Engrais", 15},
            {"Motoculteur", 50}, {"Insecticide", 30}, {"Irrigation automatique", 100},
            {"Katana", 20}, {"SuperRepousse", 50}, {"Retraite", PrixRetraite}
        }; 
        var descriptions = new Dictionary<string, string> // Tous les objets de la boutique avec leur description permettant au joueur de savoir ce qu'il achète
        {
            // Plantes
            {"Piment", "Plante épicée nécessitant peu d'eau. Rapport moyen."},
            {"Melon", "Fruit juteux à moyenne espérance de vie. Rapport élevé."},
            {"Citrouille", "Plante qui demande peu mais qui ne vit pas longtemps. Rapport élevé."},
            {"Fraise", "Plante rapide et rentable mais très susceptible de mourir si arrosé trop peu."},

            // Améliorations
            {"Capitalisme", "Débloque l'accès à des améliorations puissantes."},
            {"Engrais", "Les cases labourées restent labourées après plantation."},
            {"Motoculteur", "Laboure automatiquement les 9 cases autour."},
            {"Insecticide", "Récolte boostée : gains entre x0.5 et x2."},
            {"Irrigation automatique", "L'hydratation ne descend jamais sous 20%."},
            {"Katana", "Double les dégâts infligés au rongeur."},
            {"SuperRepousse", "Les urgences arrivent moins souvent."},
            {"Retraite", "Permet de gagner le jeu si vous avez assez d'argent."}
        };

        var ObjetAchetables = new List<string>();

        // Ajouter les plantes non encore débloquées
        foreach (var plante in Grille.ObjetBoutique)
        {
            if (!Grille.PlantesDispo.Contains($"|{plante}|"))
                ObjetAchetables.Add(plante);
        }

        // Ajouter Capitalisme si pas encore achetée
        if (!AmeliorationsAchetées.Contains("Capitalisme"))
            ObjetAchetables.Add("Capitalisme");

        // Ajouter autres améliorations si Capitalisme est achetée
        string[] upgrades = { "Engrais", "Motoculteur", "Insecticide", "Irrigation automatique", "Katana", "SuperRepousse" };
        if (AmeliorationsAchetées.Contains("Capitalisme"))
        {
            foreach (var up in upgrades)
            {
                if (!AmeliorationsAchetées.Contains(up))
                    ObjetAchetables.Add(up);
            }
        }

        int selection = 0;
        bool enCours = true;

        while (enCours) // Dans la boutique, on peut acheter des plantes ou des améliorations et le temps ne passe pas
        {
            Console.Clear();
            Console.WriteLine(" Boutique - Plantes spéciales à débloquer :");
            Console.WriteLine($" Argent : {Argent} pièces\n");

            for (int i = 0; i < ObjetAchetables.Count; i++)
            {
                string objet = ObjetAchetables[i];

                bool estAmelioration = prixObjet.ContainsKey(objet) && !Grille.ObjetBoutique.Contains(objet);

                if (i == selection)
                    Console.BackgroundColor = ConsoleColor.DarkGray;

                string tag;
                if (objet == "Retraite") // On affiche la condition de victoire différamment des plantes et différament des améliorations
                {
                    tag = "[🏆 Victoire]";
                    Console.ForegroundColor = ConsoleColor.Magenta;
                }
                else if (estAmelioration)
                {
                    tag = "[Upgrade]";
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else
                {
                    tag = "[Plante]";
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                if (objet == "Retraite")
                    Console.WriteLine($"✨ {tag} {objet.ToUpper()} : {prixObjet[objet]} ✨");
                else
                    Console.WriteLine($"- {tag}{objet} : {prixObjet[objet]}");


                Console.ResetColor();
            }
            Console.WriteLine("\n──────────────────────────────────────────────");
            string selectionActuelle = ObjetAchetables[selection];
            if (descriptions.ContainsKey(selectionActuelle))
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"ℹ️  {descriptions[selectionActuelle]}");
                Console.ResetColor();
            }
            Console.WriteLine("──────────────────────────────────────────────");

            Console.WriteLine("\n\nUtilise 'z'/'s' pour naviguer, 'e' pour acheter, 'x' pour quitter.");

            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.KeyChar)
            {
                case 'z':
                    selection = (selection - 1 + ObjetAchetables.Count) % ObjetAchetables.Count;
                    break;
                case 's':
                    selection = (selection + 1) % ObjetAchetables.Count;
                    break;
                case 'e':
                    string choix = ObjetAchetables[selection];
                    int prix = prixObjet[choix];

                    if (Argent >= prix)
                    {
                        Argent -= prix;

                        if (Grille.ObjetBoutique.Contains(choix))
                        {
                            Grille.PlantesDispo.Add($"|{choix}|");
                            Console.WriteLine($"\n {choix} débloquée !");
                        }
                        else
                        {
                            AmeliorationsAchetées.Add(choix);
                            Console.WriteLine($"\n Amélioration {choix} achetée !");
                        }

                        ObjetAchetables.RemoveAt(selection);
                        if (ObjetAchetables.Count == 0) enCours = false;
                        else selection %= ObjetAchetables.Count;
                    }
                    else
                    {
                        Console.WriteLine("\n Pas assez d'argent !");
                    }
                    Console.ReadKey(true);
                    break;

                case 'x':
                    enCours = false;
                    break;
            }
        }
    }

    public override string ToString() // Affichage du joueur en fontion du caractère qui le représente
    {
        return Affichage;
    }

}

