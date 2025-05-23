
Console.OutputEncoding = System.Text.Encoding.UTF8; //Pour l'affichage des émoticônes
MainJeu mainJeu = new MainJeu();
mainJeu.StartGame();
mainJeu.AfficherFinJeu(mainJeu.ConditionFinDeJeu);