public class MainJeu
{
    public Joueur Joueur { get; set; }
    public GrilleDeJeu Grille { get; set; }

    public MainJeu()
    {
        Joueur = new Joueur(0, 0);
        Grille = new GrilleDeJeu(10, 10, Joueur);
        Joueur.Grille = Grille; 
    }

    public void StartGame()
    {
        bool win = true;
        while (win)
        {
            Grille.UpdatePlantes();
            Grille.InitialiserGrille();
            Grille.DefineGrille(Joueur.JoueurPositionX, Joueur.JoueurPositionY);
            Grille.AfficherGrille();
            Joueur.MoveJoueur();
            
        }
    }

}