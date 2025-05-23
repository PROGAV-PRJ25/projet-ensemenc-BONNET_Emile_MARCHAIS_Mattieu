
Console.OutputEncoding = System.Text.Encoding.UTF8; //Pour l'affichage des émoticônes
MainJeu mainJeu = new MainJeu();
#if WINDOWS
var player = new System.Media.SoundPlayer("tadow.wav");
player.Play();
#endif
mainJeu.StartGame();
mainJeu.AfficherFinJeu(mainJeu.ConditionFinDeJeu);