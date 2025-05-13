public class Rongeur
{
    Random rnd = new Random();
    public EspaceDeJeu Grille { get; set; }
    public int PositionX { get; set; }

    public int PositionY { get; set; }
    public int PV {get; set;}

    public string Affichage { get; set; } = " E ";

    public Rongeur(int x, int y, EspaceDeJeu grille, int pv = 3)
    {
        PositionX = x;
        PositionY = y;
        PV = pv;
        Grille = grille;
    }

    public void MoveRongeur()
    {
        int direction = rnd.Next(1,5); // 1 -> Nord, 2 -> Est, 3 -> Sud, 4 -> Ouest
        int tempX = PositionX;
        int tempY = PositionY;

        switch (direction)
        {
            case 1 : tempY--; break;
            case 2 : tempX++; break;
            case 3 : tempY++; break;
            case 4 : tempX--; break;
            default : return;
        }        

        if (tempX >= 0 && tempX < Grille.TailleY && tempY >= 0 && tempY < Grille.TailleX)
        {
            PositionX = tempX;
            PositionY = tempY;
        }
    }


    public void MangerPlante()
    {
        for(int i = Grille.Plantes.Count - 1; i >= 0; i--)
        {
            if((PositionX == Grille.Plantes[i].PositionX) && (PositionY == Grille.Plantes[i].PositionY))
            {
                Grille.Plantes.Remove(Grille.Plantes[i]);
            }
        }

        Grille.EstLaboure[PositionY, PositionX] = false;

    }
}

