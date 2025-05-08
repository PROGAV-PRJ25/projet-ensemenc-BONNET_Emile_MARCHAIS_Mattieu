public class MainJeu
{
    Random rnd = new Random();
    public Joueur Joueur { get; set; }
    public GrilleDeJeu Grille { get; set; }
    public Rongeur Rongeur { get; set; }

    public MainJeu()
    {
        Joueur = new Joueur(0, 0);
        Grille = new GrilleDeJeu(10, 10, Joueur);
        Joueur.Grille = Grille;

        Rongeur = new Rongeur(0,0,Grille);
    }

    public void StartGame()
    {
        bool win = true;
        while (win)
        {
            while (!Grille.ModeUrgence)
            {
                Grille.DefineGrille(Joueur.JoueurPositionX, Joueur.JoueurPositionY);
                Grille.AfficherGrille();
                Joueur.MoveJoueur();
                
            }
            while (Grille.ModeUrgence)
            {
                var timer = new System.Diagnostics.Stopwatch();
                timer.Start();
                while (timer.ElapsedMilliseconds < 2000)
                {
                    Grille.DefineGrille(Joueur.JoueurPositionX, Joueur.JoueurPositionY);
                    Grille.AfficherGrille();
                    Joueur.MoveJoueur();
                }
                timer.Stop();
                Rongeur.MoveRongeur();    
                if (Rongeur.PV <= 0)
                {
                    Grille.ModeUrgence = false;
                }   
            }
        }
    }

}