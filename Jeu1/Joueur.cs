public class Joueur
{
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
            case 'e': PlacePlante(new Plante("Carotte", JoueurPositionX, JoueurPositionY)); break;
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
        Grille.Plantes.Add(plante);
    }



    public override string ToString()
    {
        return Affichage;
    }

}

