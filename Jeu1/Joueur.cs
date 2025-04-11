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
    public void MoveJoueur()
    {
        bool again = true;
        while (again)
        {
            Console.SetCursorPosition(Console.WindowWidth / 2, Console.CursorTop);

            char? action = null;
            var timer = new System.Diagnostics.Stopwatch();
            timer.Start();

            // Give the player a short time to act (e.g. 300 ms)
            while (timer.ElapsedMilliseconds < 300)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true); // true to not show the key on screen
                    action = Char.ToLower(key.KeyChar);
                    break;
                }
            }

            // Reset timer
            timer.Stop();

            // Handle "no input" case
            if (action == null)
            {
                // No input -> time passes
                again = true; // Or keep it true depending on your design
                continue;
            }

            // Handle movement
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
                default:
                    // Invalid input — you could skip or treat it as no action
                    continue;
            }

            // Assume movement is always valid for now
            JoueurPositionX = tempX;
            JoueurPositionY = tempY;
            again = true;
        }

        // Example update functions if needed
        // DefineOwen(JoueurPositionX, JoueurPositionY);
        // DefineGrid();
    }

}

