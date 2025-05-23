public class MainJeu
{
    Random rnd = new Random();
    public Joueur Joueur { get; set; }
    public EspaceDeJeu EspaceDeJeu { get; set; }
    public Rongeur Rongeur { get; set; }
    public int ConditionFinDeJeu { get; set; }

    public string nextPrint { get; set; } = "";

    public int selectNumber = 1; // -> Permet de savoir quel écran de selection est affiché

    public MainJeu()
    {
        Joueur = new Joueur(0, 0);
        EspaceDeJeu = new EspaceDeJeu(15, 15, Joueur);
        Joueur.Grille = EspaceDeJeu;

        Rongeur = new Rongeur(0, 0, EspaceDeJeu);
    }

    public void StartGame()
    {
        InterfaceJeu();
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
        nextPrint = "ENSEMENC";
        Console.SetCursorPosition((Console.WindowWidth - nextPrint.Length) / 2, Console.CursorTop); // Placement du curseur au centre de la console
        Console.WriteLine(nextPrint);
        Console.WriteLine();
        Console.WriteLine();
        PrintAscii();
        Console.WriteLine();
        Console.WriteLine();

        nextPrint = "Une mystérieuse lettre est arrivée dans votre boîte aux lettres.";
        Console.SetCursorPosition((Console.WindowWidth - nextPrint.Length) / 2, Console.CursorTop);
        Console.WriteLine(nextPrint);

        nextPrint = "Votre grand père vous a laissé un héritage : un jardin.";
        Console.SetCursorPosition((Console.WindowWidth - nextPrint.Length) / 2, Console.CursorTop);
        Console.WriteLine(nextPrint);

        nextPrint = "Saurez vous le faire prospérer ?";
        Console.SetCursorPosition((Console.WindowWidth - nextPrint.Length) / 2, Console.CursorTop);
        Console.WriteLine(nextPrint);

        nextPrint = "Mais attention, il y a un rongeur qui rôde dans le jardin.";
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

    void PrintAscii() // -> Affiche une fleur imposante pour l'écran d'accueil
    {
        string[] lines = new string[]
        {
            "             .",
            "             .@.                                    .",
            "             @m@,.                                 .@",
            "            .@m%nm@,.                            .@m@",
            "           .@nvv%vnmm@,.                      .@mn%n@",
            "          .@mnvvv%vvnnmm@,.                .@mmnv%vn@,",
            "          @mmnnvvv%vvvvvnnmm@,.        .@mmnnvvv%vvnm@",
            "          @mmnnvvvvv%vvvvvvnnmm@, ;;;@mmnnvvvvv%vvvnm@,",
            "          `@mmnnvvvvvv%vvvvvnnmmm;;@mmnnvvvvvv%vvvvnmm@",
            "           `@mmmnnvvvvvv%vvvnnmmm;%mmnnvvvvvv%vvvvnnmm@",
            "             `@m%v%v%v%v%v;%;%;%;%;%;%;%%%vv%vvvvnnnmm@",
            "             .,mm@@@@@mm%;;@@m@m@@m@@m@mm;;%%vvvnnnmm@;@,.",
            "          .,@mmmmmmvv%%;;@@vmvvvvvvvvvmvm@@;;%%vvnnm@;%mmm@,",
            "       .,@mmnnvvvvv%%;;@@vvvvv%%%%%%%vvvvmm@@;;%%mm@;%%nnnnm@,",
            "    .,@mnnvv%v%v%v%%;;@mmvvvv%%;*;*;%%vvvvmmm@;;%m;%%v%v%v%vmm@,.",
            ",@mnnvv%v%v%v%v%v%v%;;@@vvvv%%;*;*;*;%%vvvvm@@;;m%%%v%v%v%v%v%vnnm@,",
            "`    `@mnnvv%v%v%v%%;;@mvvvvv%%;;*;;%%vvvmmmm@;;%m;%%v%v%v%vmm@'   '",
            "        `@mmnnvvvvv%%;;@@mvvvv%%%%%%%vvvvmm@@;;%%mm@;%%nnnnm@'",
            "           `@mmmmmmvv%%;;@@mvvvvvvvvvvmmm@@;;%%mmnmm@;%mmm@'",
            "              `mm@@@@@mm%;;@m@@m@m@m@@m@@;;%%vvvvvnmm@;@'",
            "             ,@m%v%v%v%v%v;%;%;%;%;%;%;%;%vv%vvvvvnnmm@",
            "           .@mmnnvvvvvvv%vvvvnnmm%mmnnvvvvvvv%vvvvnnmm@",
            "          .@mmnnvvvvvv%vvvvvvnnmm'`@mmnnvvvvvv%vvvnnmm@",
            "          @mmnnvvvvv%vvvvvvnnmm@':%::`@mmnnvvvv%vvvnm@'",
            "          @mmnnvvv%vvvvvnnmm@'`:::%%:::'`@mmnnvv%vvmm@",
            "          `@mnvvv%vvnnmm@'     `:;%%;:'     `@mvv%vm@'",
            "           `@mnv%vnnm@'          `;%;'         `@n%n@",
            "            `@m%mm@'              ;%;.           `@m@",
            "             @m@'                 `;%;             `@",
            "             `@'                   ;%;.             '",
            "              `                    `;%;",
        };

        int startX = Console.WindowWidth / 3;

        foreach (string line in lines)
        {
            Console.SetCursorPosition(startX, Console.CursorTop);
            Console.WriteLine(line);
        }
    }
    void PrintSelectScreen(int x)//-> Affiche l'écran de selection en fonction de l'entier x 
    {
        Console.Clear();
        nextPrint = "ENSEMENC";
        Console.SetCursorPosition((Console.WindowWidth - nextPrint.Length) / 2, Console.CursorTop);
        Console.WriteLine(nextPrint);
        Console.WriteLine();
        Console.WriteLine();
        PrintAscii();
        Console.WriteLine();
        Console.WriteLine();
        nextPrint = "Naviguer avec z pour monter et s pour descendre";
        Console.SetCursorPosition((Console.WindowWidth - nextPrint.Length) / 2, Console.CursorTop);
        Console.WriteLine(nextPrint);
        nextPrint = "Appuyer sur 'espace' pour confirmer votre choix";
        Console.SetCursorPosition((Console.WindowWidth - nextPrint.Length) / 2, Console.CursorTop);
        Console.WriteLine(nextPrint);
        Console.WriteLine();
        Console.WriteLine();


        if (x == 1)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            nextPrint = ">-  Jouer  -<";
            Console.SetCursorPosition((Console.WindowWidth - nextPrint.Length) / 2, Console.CursorTop);
            Console.WriteLine(nextPrint);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;

            nextPrint = "Règles";
            Console.SetCursorPosition((Console.WindowWidth - nextPrint.Length) / 2, Console.CursorTop);
            Console.WriteLine(nextPrint);
            Console.WriteLine();
        }
        else
        {
            nextPrint = "Jouer";
            Console.SetCursorPosition((Console.WindowWidth - nextPrint.Length) / 2, Console.CursorTop);
            Console.WriteLine(nextPrint);
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Green;
            nextPrint = ">-  Règles  -<";
            Console.SetCursorPosition((Console.WindowWidth - nextPrint.Length) / 2, Console.CursorTop);
            Console.WriteLine(nextPrint);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    void SelectScreen()//-> Permet la selection des différents écrans de selection
    {
        Console.Clear();
        bool again = true;
        while (again)
        {
            PrintSelectScreen(selectNumber);
            Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);
            char action = Console.ReadKey().KeyChar;
            switch (action)
            {
                case 'z':
                    if (selectNumber != 1)
                    {
                        selectNumber--;
                    }
                    break;
                case 's':
                    if (selectNumber != 4)
                    {
                        selectNumber++;
                    }
                    break;
                case ' ':
                    again = false;
                    break;
            }
        }
    }

    void PrintColoredText(string input)
    {
        string[] words = input.Split(' ');

        foreach (var word in words)
        {
            if (word.Contains("plante") || word.Contains("plantes"))
                Console.ForegroundColor = ConsoleColor.Green;
            else if (word.Contains("rongeur"))
                Console.ForegroundColor = ConsoleColor.Red;
            else if (word.Contains("eau") || word.Contains("arroser"))
                Console.ForegroundColor = ConsoleColor.Blue;
            else if (word.Contains("terrain") || word.Contains("terrains"))
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            else if (word.Contains("boutique") || word.Contains("argent"))
                Console.ForegroundColor = ConsoleColor.Yellow;
            else if (word.Contains("malade"))
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
            else
                Console.ForegroundColor = ConsoleColor.White;

            Console.Write(word + " ");
        }

        Console.ResetColor();
        Console.WriteLine();
    }


    void Regles()
    {
        Console.Clear();
        nextPrint = "Bienvenue dans ENSEMENC. Voici les règles du jeu :";
        Console.SetCursorPosition((Console.WindowWidth - nextPrint.Length) / 2, Console.CursorTop);
        Console.WriteLine(nextPrint);
        Console.WriteLine();

        PrintColoredText("Le but du jeu est de faire prospérer votre jardin tout en récoltant le plus de plantes possible.");
        Console.WriteLine();
        PrintColoredText("Vous incarnez un jardinier débutant ayant hérité d’un terrain rempli de potentiel.");
        Console.WriteLine();
        PrintColoredText("Chaque plante a besoin d’une quantité précise d’eau. Pensez à les arroser régulièrement.");
        Console.WriteLine();
        PrintColoredText("Les plantes ont également des terrains favoris : adaptez leurs positions.");
        Console.WriteLine();
        PrintColoredText("Certaines plantes tomberont malades et elles peuvent mourir.");
        Console.WriteLine();
        PrintColoredText("Un rongeur rôde dans votre potager. Lorsqu’il apparaît, entrez en mode urgence et chassez-le !");
        Console.WriteLine();
        PrintColoredText("Utilisez l'argent récolté en vendant vos plantes pour acheter de nouvelles graines dans la boutique.");
        Console.WriteLine();
        PrintColoredText("Le but ultime ? Économiser 500 pièces pour prendre votre retraite bien méritée !");
        Console.WriteLine();
        PrintColoredText("Si plus de 50 plantes meurent... c’est la fin.");
        Console.WriteLine();

        nextPrint = "(Appuyez sur une touche pour revenir au menu)";
        Console.SetCursorPosition((Console.WindowWidth - nextPrint.Length) / 2, Console.CursorTop);
        Console.WriteLine(nextPrint);
        Console.ReadKey(true);
        Console.Clear();
    }

    void InterfaceJeu()
    {
        PrintIntro();
        bool enCours = true;
        while (enCours)
        {
            SelectScreen();

            if (selectNumber == 1) // Jouer
            {
                enCours = false; // on quitte le menu
            }
            else if (selectNumber == 2) // Règles
            {
                Regles(); // puis on revient au menu
            }
        }
    }

}