public class MainJeu
{
    public Joueur Joueur { get; set; }
    public GrilleDeJeu Grille { get; set; }

    public MainJeu()
    {
        Grille = new GrilleDeJeu(10, 10); // Example size, adjust as needed
        Joueur = new Joueur(0, 0); // Starting position of the player
    }

    public void StartGame()
    {
        bool win = true;
        while (win)
        {
            Joueur.MoveJoueur();            
        }
    }
}