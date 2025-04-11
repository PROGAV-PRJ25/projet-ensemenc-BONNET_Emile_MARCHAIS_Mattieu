public class Joueur
{
    public int JoueurPositionX { get; set; }

    public int JoueurPositionY { get; set; }

    public int Score { get; set; }

    public Joueur(int x, int y)
    {
        JoueurPositionX = x;
        JoueurPositionY = y;
        Score = 0;
    }
    public void MoveJoueur() //-> Pour faire bouger Owen et lancer ses grenades 
    {
        bool again = true;
        while (again) //Tant que l'on ne rentre pas un mouvement valide
        {
            Console.SetCursorPosition((Console.WindowWidth) / 2, Console.CursorTop);
            char action = Console.ReadKey().KeyChar;
            action = Char.ToLower(action);


            //On ajoute en variable temporaire les position X et Y
            int tempX = JoueurPositionX;
            int tempY = JoueurPositionY;
            
            switch (action)
            {
                case 'z':
                    tempY--;
                    break;
                case 'd':
                    tempX++;
                    break;
                case 's':
                    tempY++;
                    break;
                case 'q':
                    tempX--;
                    break;
                default: // Sécurité si le joueur appuie sur une touche non-valide. Cela recommence l'action.
                    again = true;
                    break;
            }

/*             if (IsMoveOwenBlueValid(tempX, tempY)) // Si le mouvement est valide, on actualise les positions temporaires
            {
                owenPositionX = tempX;
                owenPositionY = tempY;
            }
            else//Sinon on relance la boucle
            {
                again = true;
            } */
        }
/*         DefineOwen(owenPositionX, owenPositionY); // On redéfinit la position dans la grille
        DefineGrid();//On actualise la grille de crevasses si Owen a lancé une grenade */
    }
}

