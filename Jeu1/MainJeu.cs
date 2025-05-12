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
                var timer = new System.Diagnostics.Stopwatch();
                timer.Start();
                while (timer.ElapsedMilliseconds < 1000)
                {
                    Grille.DefineGrille(Joueur.JoueurPositionX, Joueur.JoueurPositionY);
                    Grille.AfficherGrille();
                    Joueur.MoveJoueur(1000);
                }
                timer.Stop();
                Grille.UpdatePlantes();
                Grille.luminosity += 1; 
                if (Grille.luminosity == 16)
                {
                    Grille.Jours ++;
                    Grille.luminosity = 0; // Reset luminosity after reaching a certain threshold
                    if (Grille.Jours % 3 == 0)
                    {
                        Grille.ModeUrgence = true;
                    }
                }
                
            }
            while (Grille.ModeUrgence)
            {
                var timer = new System.Diagnostics.Stopwatch();
                timer.Start();
                while (timer.ElapsedMilliseconds < 400)
                {
                    Grille.DefineGrille(Joueur.JoueurPositionX, Joueur.JoueurPositionY);
                    Grille.Grille[Rongeur.PositionY, Rongeur.PositionX] = Rongeur.Affichage;
                    Grille.AfficherGrille();
                    Joueur.MoveJoueur(300);
                    if(Joueur.AFrappe)
                    {
                        Rongeur.PV--;
                        Joueur.AFrappe = false;
                    }
                }
                timer.Stop();
                Rongeur.MangerPlante();
                Rongeur.MoveRongeur();    
                if (Rongeur.PV <= 0)
                {
                    Grille.ModeUrgence = false;
                    Rongeur.PV = 3;
                }   
            }
        }
    }

}