public class MainJeu
{
    Random rnd = new Random();
    public Joueur Joueur { get; set; }
    public EspaceDeJeu EspaceDeJeu { get; set; }
    public Rongeur Rongeur { get; set; }
    public int ConditionFinDeJeu { get; set; }

    public string nextPrint { get; set; } = "";

    public MainJeu()
    {
        Joueur = new Joueur(0, 0);
        EspaceDeJeu = new EspaceDeJeu(15, 15, Joueur);
        Joueur.Grille = EspaceDeJeu;

        Rongeur = new Rongeur(0, 0, EspaceDeJeu);
    }

    public void StartGame()
    {
        bool win = true;
        while (!EspaceDeJeu.ModeUrgence && win)
        {
            var timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            while (timer.ElapsedMilliseconds < 1000)
            {
                EspaceDeJeu.DefinirGrille(Joueur.PositionX, Joueur.PositionY);
                EspaceDeJeu.AfficherJeu();
                Joueur.MoveJoueur(1000);
            }
            timer.Stop();
            EspaceDeJeu.UpdatePlantes();
            EspaceDeJeu.luminosité += 1;
            if (EspaceDeJeu.luminosité == 16)
            {
                EspaceDeJeu.Jours++;
                EspaceDeJeu.luminosité = 0;
                if (EspaceDeJeu.Jours % 10 == 0)
                {
                    EspaceDeJeu.ModeUrgence = true;
                }
            }
            if (EspaceDeJeu.NombrePlanteMorte >= 2)
            {
                win = false;
                ConditionFinDeJeu = 1;
            }
            if (EspaceDeJeu.Retraite)
            {
                win = false;
                ConditionFinDeJeu = 2;
            }
        }
        while (EspaceDeJeu.ModeUrgence && win)
        {
            var timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            while (timer.ElapsedMilliseconds < 400)
            {
                EspaceDeJeu.DefinirGrille(Joueur.PositionX, Joueur.PositionY);
                EspaceDeJeu.Grille[Rongeur.PositionY, Rongeur.PositionX] = Rongeur.Affichage;
                EspaceDeJeu.AfficherJeu();
                Joueur.MoveJoueur(300);
                if (Joueur.AFrappe)
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

    public void AfficherFinJeu(int condition)
    {
        Console.Clear();
        if (condition == 1)
        {
            Console.WriteLine("Vous avez tué toute vos plantes...");
        }
        if (condition == 2)
        {
            Console.WriteLine("Bravo, vous avez accumulé assez d'argent pour prendre votre retraite !!!");
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Au total, vous avez ammasé:");
            Console.WriteLine($"{Joueur.TableauRecolte[0]} Carottes, ");
            Console.WriteLine($"{Joueur.TableauRecolte[1]} Tomates, ");
            Console.WriteLine($"{Joueur.TableauRecolte[2]} Radis, ");
            Console.WriteLine($"{Joueur.TableauRecolte[3]} Salade, ");
            Console.WriteLine($"{Joueur.TableauRecolte[4]} Piment, ");
            Console.WriteLine($"{Joueur.TableauRecolte[5]} Melon, ");
            Console.WriteLine($"{Joueur.TableauRecolte[6]} Citrouille, ");
            Console.WriteLine($"{Joueur.TableauRecolte[7]} Fraise, ");

            Console.WriteLine();
            Console.WriteLine("Reposez vous bien :)");

        }
    }
    void PrintIntro()//-> Affiche l'introduction du jeu
    {
        nextPrint = "JURENSIC WORLD";
        Console.SetCursorPosition((Console.WindowWidth - nextPrint.Length) / 2, Console.CursorTop); // Placement du curseur au centre de la console
        Console.WriteLine(nextPrint);
        Console.WriteLine();
        Console.WriteLine();
        PrintAscii();
        Console.WriteLine();
        Console.WriteLine();

        nextPrint = "Le plus grand prédateur de l'histoire est à vos trousses !";
        Console.SetCursorPosition((Console.WindowWidth - nextPrint.Length) / 2, Console.CursorTop);
        Console.WriteLine(nextPrint);

        nextPrint = "Saurez-vous l'enfermer dans sa cage ?";
        Console.SetCursorPosition((Console.WindowWidth - nextPrint.Length) / 2, Console.CursorTop);
        Console.WriteLine(nextPrint);

        Console.WriteLine();

        nextPrint = "(Appuyer sur n'importe quelle touche pour jouer.)";
        Console.SetCursorPosition((Console.WindowWidth - nextPrint.Length) / 2, Console.CursorTop);
        Console.WriteLine(nextPrint);

        Console.SetCursorPosition((Console.WindowWidth) / 2, Console.CursorTop);
        char suite = Console.ReadKey().KeyChar;
        Console.Clear();
    }

    void PrintAscii() // -> Affiche un motif de dinosaure
    {
        string[] lines = new string[]
        {
            "                      ,",
            "               ,  ;:._.-`''.",
            "             ;.;'.;`      _ `.",
            "              ',;`       ( \\ ,`-.  ",
            "           `:.`,         (_/ ;\\  `-.",
            "            ';:              / `.   `-._",
            "          `;.;'              `-,/ .     `-.",
            "          ';;'              _    `^`       `.",
            "         ';;            ,'-' `--._          ;",
            "':      `;;        ,;     `.    ':`,,.__,,_ /",
            " `;`:;`;:`       ,;  '.    ;,      ';';':';;`",
            "              .,; '    '-._ `':.;   ",
            "            .:; `          '._ `';;,",
            "          ;:` `    :'`'       ',__.)",
            "        `;:;:.,...;'`'",
            "      ';. '`'::'`''  .'`'",
            "    ,'jgs`';;:,..::;`'`'",
            ", .;`      `'::''`",
            ",`;`."
        };

        int startX = Console.WindowWidth / 3;

        foreach (string line in lines)
        {
            Console.SetCursorPosition(startX, Console.CursorTop);
            Console.WriteLine(line);
        }
    }


}