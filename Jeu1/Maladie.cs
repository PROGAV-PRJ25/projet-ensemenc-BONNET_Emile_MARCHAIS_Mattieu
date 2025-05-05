public class Maladie
{
    public string Type { get; set; } // Nom unique (ex: "MildiouTomate")
    public int Duree { get; set; } // Nb de jours restants
    public int Severite { get; set; } // Impact négatif sur TauxCroissance

    public Maladie(string type, int duree, int severite)
    {
        Type = type;
        Duree = duree;
        Severite = severite;
    }

    public void AppliquerEffet(Plante plante)
    {
        if (Duree > 0)
        {
            plante.TauxCroissance -= Severite;
            Duree--;
        }
        if (Duree <= 0)
        {
            plante.MaladieActuelle = null; // Guérison automatique
        }
    }
}
