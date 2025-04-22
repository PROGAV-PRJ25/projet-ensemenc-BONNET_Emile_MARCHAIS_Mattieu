public class Joueur
{
    public int[] TableauRecolte { get; set; } // 0 = Carotte; 1 = Tomate; 2 = Radis; 3 = Salade
    public int JoueurPositionX { get; set; }

    public int JoueurPositionY { get; set; }

    public GrilleDeJeu Grille { get; set; }

    public string Affichage { get; set; } = " J ";
    public Joueur(int x, int y)
    {
        JoueurPositionX = x;
        JoueurPositionY = y;
    }
    
    public void MoveJoueur()
    {
        char? action = null;
        var timer = new System.Diagnostics.Stopwatch();
        timer.Start();

        while (timer.ElapsedMilliseconds < 300)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                action = Char.ToLower(key.KeyChar);
                break;
            }
        }

        timer.Stop();
        Grille.luminosity += 1; 
        if (Grille.luminosity == 16)
        {
            Grille.Jours ++;
            Grille.luminosity = 0; // Reset luminosity after reaching a certain threshold
        }


        // If no input, do nothing
        if (action == null)
            return;

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
        if(plante == null)
        {
            Console.WriteLine("Il n'y à pas de plante à cette position");
        }
        else if (plante.cycleStep != 5)
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
        switch (selection)
        {
            case 0: Labourer(); break;
            case 1: PlacePlante(ChoixPlante()); break;
            case 2: Recolter(); break;
        }    
    }

    public Plante ChoixPlante()
    {
        if (Grille.SelectPlante == 0)
        {
           return new Plante("Carotte", JoueurPositionX, JoueurPositionY, 20, 50);
        }
        if (Grille.SelectPlante == 1)
        {
            return new Plante("Tomate", JoueurPositionX, JoueurPositionY, 30, 60);
        }
        if (Grille.SelectPlante == 2)
        {
            return new Plante("Radis", JoueurPositionX, JoueurPositionY, 40, 30);
        }
        if (Grille.SelectPlante == 3)
        {
            return new Plante("Salade", JoueurPositionX, JoueurPositionY, 10, 100);
        }
    
        return new Plante("Carotte", JoueurPositionX, JoueurPositionY, 20, 50); // Default case
    }

    public override string ToString()
    {
        return Affichage;
    }

}

