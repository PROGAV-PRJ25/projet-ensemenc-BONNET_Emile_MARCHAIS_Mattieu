public class Rongeur
{
    Random rnd = new Random();
    public int PositionX { get; set; }

    public int PositionY { get; set; }
    public int PV {get; set;}

    public GrilleDeJeu Grille { get; set; }

    public string Affichage { get; set; } = " M ";
    public Rongeur(int x, int y, int pv = 3)
    {
        PositionX = x;
        PositionY = y;
        PV = pv;
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

        if (tempX >= 0 && tempX < Grille.TailleX && tempY >= 0 && tempY < Grille.TailleY)
        {
            PositionX = tempX;
            PositionY = tempY;
        }
    }
}

