public class Maladie
{
    public string Type { get; set; }
    public int Duree { get; set; } 
    public int Gravite { get; set; }

    public Maladie(string type, int duree, int gravite)
    {
        Type = type;
        Duree = duree;
        Gravite = gravite;
    }

    public void AppliquerEffet(Plante plante)
    /*Applique les effets d'une maladie (affecte le taux de croissance et permet la guérison */
    {
        if (Duree > 0)
        {
            plante.TauxCroissance -= Gravite;
            Duree--;
        }
        if (Duree <= 0)
        {
            plante.MaladieActuelle = null; // Guérison automatique
        }
    }

    public void ContracterMaladie(Plante plante)
    /*Fonction qui applique une maladie selon une certaine probabilité à une plante*/
    {
        // Maladie unique par type
        switch (Type)
        {
            case "Tomate":
                plante.MaladieActuelle = new Maladie("MildiouTomate", 10, 15);
                break;
            case "Carotte":
                plante.MaladieActuelle = new Maladie("RouilleCarotte", 20, 10);
                break;
            case "Radis":
                plante.MaladieActuelle = new Maladie("PourritureRadis", 15, 20);
                break;
            case "Salade":
                plante.MaladieActuelle = new Maladie("TacheSalade", 5, 15);
                break;
            case "Piment":
                plante.MaladieActuelle = new Maladie("Virus de la mosaïque", 15, 5);
                break;
            case "Melon":
                plante.MaladieActuelle = new Maladie("Oïdium", 5, 20);
                break;
            case "Citrouille":
                plante.MaladieActuelle = new Maladie("Anthracnose", 30, 5);
                break;
            case "Fraise":
                plante.MaladieActuelle = new Maladie("Botrytis", 2, 30);
                break;
        }
    }
    public void PropagerMaladie(Plante plante, Random rnd, EspaceDeJeu grille)
    /*Fonction permettant d'appliquer une maladie selon une certaine probabilité aux plantes voisine*/
    {
        var offsets = new (int dx, int dy)[] { (-1, 0), (1, 0), (0, -1), (0, 1) };
        foreach (var (dx, dy) in offsets)
        {
            int nx = plante.PositionX + dx;
            int ny = plante.PositionY + dy;

            if (nx >= 0 && nx < grille.TailleY && ny >= 0 && ny < grille.TailleX)
            {
                var voisine = grille.SelectionnerPlante(nx, ny);
                if (voisine.Type == plante.Type && voisine.MaladieActuelle == null)
                {
                    if (rnd.NextDouble() < 0.3) // propagation élevée
                    {
                        voisine.MaladieActuelle = new Maladie(plante.MaladieActuelle.Type, plante.MaladieActuelle.Duree, plante.MaladieActuelle.Gravite);
                    }
                }
            }
        }
    }
}
