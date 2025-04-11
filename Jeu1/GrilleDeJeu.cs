public class GrilleDeJeu
{
    public int[,] Grille { get; private set; }
    public int TailleX { get; private set; }
    public int TailleY { get; private set; }

    public GrilleDeJeu(int tailleX, int tailleY)
    {
        TailleX = tailleX;
        TailleY = tailleY;
        Grille = new int[TailleX, TailleY];
        InitialiserGrille();
    }

    private void InitialiserGrille()
    {
        for (int i = 0; i < TailleX; i++)
        {
            for (int j = 0; j < TailleY; j++)
            {
                Grille[i, j] = 0; // Initialiser chaque case à 0
            }
        }
    }

    public void AfficherGrille()
    {
        Console.Clear();
        for (int i = 0; i < TailleX; i++)
        {
            for (int j = 0; j < TailleY; j++)
            {
                Console.Write(Grille[i, j] + " ");
            }
            Console.WriteLine();
        }
    }
}