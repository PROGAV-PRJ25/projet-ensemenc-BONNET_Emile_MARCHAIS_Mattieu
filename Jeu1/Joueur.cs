public class Joueur
{
    public int[] TableauRecolte { get; set; }
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

    public void PlacePlante(Plante plante)
    {
        if (Grille.EstLaboure[JoueurPositionY, JoueurPositionX])
        {
            Grille.Plantes.Add(plante);
            Grille.EstLaboure[JoueurPositionY, JoueurPositionX] = false;
        }
        
    }
    public void Labourer()
    {
        Grille.EstLaboure[JoueurPositionY, JoueurPositionX] = true;
    }


    public void Action(int selection)
    {
        switch (selection)
        {
            case 0: Labourer(); break;
            case 1: PlacePlante(ChoixPlante()); break;
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

