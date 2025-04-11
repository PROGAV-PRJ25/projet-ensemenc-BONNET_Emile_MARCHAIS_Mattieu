public class Joueur
{
    public string Nom { get; set; }
    public int Score { get; set; }

    public Joueur(string nom)
    {
        Nom = nom;
        Score = 0;
    }

    public void AjouterPoint(int points)
    {
        Score += points;
    }
}