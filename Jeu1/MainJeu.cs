public class MainJeu
{
    Random rnd = new Random();
    public Joueur Joueur { get; set; }
    public EspaceDeJeu EspaceDeJeu { get; set; }
    public Rongeur Rongeur { get; set; }

    public MainJeu()
    {
        Joueur = new Joueur(0, 0);
        EspaceDeJeu = new EspaceDeJeu(10, 10, Joueur);
        Joueur.Grille = EspaceDeJeu;

        Rongeur = new Rongeur(0,0,EspaceDeJeu);
    }

    public void StartGame()
    {
        bool win = true;

        while (win)
        {
            while (!EspaceDeJeu.ModeUrgence)
            {
                var timer = new System.Diagnostics.Stopwatch();
                timer.Start();
                while (timer.ElapsedMilliseconds < 1000)
                {
                    EspaceDeJeu.DefinirGrille(Joueur.PositionX, Joueur.PositionY);
                    EspaceDeJeu.AfficherGrille();
                    Joueur.MoveJoueur(1000);
                }
                timer.Stop();
                EspaceDeJeu.UpdatePlantes();
                EspaceDeJeu.luminosity += 1; 
                if (EspaceDeJeu.luminosity == 16)
                {
                    EspaceDeJeu.Jours ++;
                    EspaceDeJeu.luminosity = 0;
                    if (EspaceDeJeu.Jours % 10 == 0)
                    {
                        EspaceDeJeu.ModeUrgence = true;
                    }
                }
                
            }
            while (EspaceDeJeu.ModeUrgence)
            {
                var timer = new System.Diagnostics.Stopwatch();
                timer.Start();
                while (timer.ElapsedMilliseconds < 400)
                {
                    EspaceDeJeu.DefinirGrille(Joueur.PositionX, Joueur.PositionY);
                    EspaceDeJeu.Grille[Rongeur.PositionY, Rongeur.PositionX] = Rongeur.Affichage;
                    EspaceDeJeu.AfficherGrille();
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
                    EspaceDeJeu.ModeUrgence = false;
                    Rongeur.PV = 3;
                }   
            }
        }
    }

}