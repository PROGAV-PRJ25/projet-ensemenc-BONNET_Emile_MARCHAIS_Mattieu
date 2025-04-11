public class GrilleDeJeu
{
    public int[,] Grille { get; private set; }
    public int Taille { get; private set; }

    public GrilleDeJeu(int taille)
    {
        Taille = taille;
        Grille = new int[Taille, Taille];
        InitialiserGrille();
    }

    private void InitialiserGrille()
    {
        for (int i = 0; i < Taille; i++)
        {
            for (int j = 0; j < Taille; j++)
            {
                Grille[i, j] = 0;
            }
        }
    }

    public void AfficherGrille()
    {
        for (int i = 0; i < Taille; i++)
        {
            for (int j = 0; j < Taille; j++)
            {
                Console.Write(Grille[i, j] + " ");
            }
            Console.WriteLine();
        }
    }
}