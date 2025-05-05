public class Plante
{
    Random rnd = new Random();
    public string Type { get; set; }
    public string TypeAfficher { get; set; }
    public int EsperanceDeVie { get; set; }
    public int PlantePositionX { get; set; }
    public int PlantePositionY { get; set; }
    public int BesoinEau { get; set; }
    public int Hydratation { get; set; }
    public Maladie? MaladieActuelle { get; set; }

    public int BesoinLuminosite { get; set; } 
    public double TauxCroissance { get; set; }
    public int cycleStep = 0;
    private string[] EtatPlante = new string[6];
    public string Affichage => EtatPlante[cycleStep];

    public GrilleDeJeu Grille { get; set; }


    public Plante(string type, int x, int y, int esperanceDeVie, int besoinEau, int besoinLuminosite, GrilleDeJeu grille)
    {
        Type = type;
        TypeAfficher = $"{type.ToLower()[0]}";
        for (int i = 0; i < EtatPlante.Length; i++)
            EtatPlante[i] = (i < 3) ? TypeAfficher : TypeAfficher.ToUpper();

        PlantePositionX = x;
        PlantePositionY = y;
        EsperanceDeVie = esperanceDeVie;
        BesoinEau = besoinEau;
        Hydratation = 100; // Commence à 100%
        BesoinLuminosite = besoinLuminosite;
        Grille = grille;
    }

    public void MetAJour()
    {
        Hydratation -= 5;
        EsperanceDeVie--;

        // Vérifie si elle tombe malade (très faible chance)
        if (MaladieActuelle == null && rnd.NextDouble() < 0.01)
        {
            ContracterMaladie();
        }

        // Applique les effets si elle est déjà malade
        MaladieActuelle?.AppliquerEffet(this);

        // Tente de contaminer les voisines
        if (MaladieActuelle != null)
        {
            PropagerMaladie();
        }

        RecalculerTauxCroissance();

        int essaiMAJ = rnd.Next(1, 101);
        if ((essaiMAJ <= TauxCroissance) && (cycleStep < EtatPlante.Length - 1))
        {
            cycleStep++;
        }

        if (TauxCroissance < 50)
        {
            EsperanceDeVie = 0; // meurt si insatisfaite
        }
    }

    private void RecalculerTauxCroissance()
    {
        double taux = 100;

        // Impact de la luminosité
        int lumiDiff = Math.Abs(Grille.luminosity - BesoinLuminosite);
        if (lumiDiff <= 1)
            taux += 10;
        else if (lumiDiff <= 3)
            taux -= 10;
        else
            taux -= 30;

        // Impact de l'eau
        int eauDiff = Hydratation - BesoinEau;
        if (eauDiff >= 20)
            taux += 10;
        else if (eauDiff < 0)
            taux -= 30;
        else
            taux -= 10;

        // Ajustement par l’âge
        if (EsperanceDeVie < 10)
            taux -= 20;

        TauxCroissance = Math.Clamp(taux, 0, 100);
    }

    private void ContracterMaladie()
    {
        // Maladie unique par type
        switch (Type)
        {
            case "Tomate":
                MaladieActuelle = new Maladie("MildiouTomate", 3, 15);
                break;
            case "Carotte":
                MaladieActuelle = new Maladie("RouilleCarotte", 2, 10);
                break;
            case "Radis":
                MaladieActuelle = new Maladie("PourritureRadis", 3, 20);
                break;
            case "Salade":
                MaladieActuelle = new Maladie("TacheSalade", 2, 15);
                break;
        }
    }

    private void PropagerMaladie()
    {
        var offsets = new (int dx, int dy)[] { (-1, 0), (1, 0), (0, -1), (0, 1) };
        foreach (var (dx, dy) in offsets)
        {
            int nx = PlantePositionX + dx;
            int ny = PlantePositionY + dy;

            if (nx >= 0 && nx < Grille.TailleY && ny >= 0 && ny < Grille.TailleX)
            {
                var voisine = Grille.SelectionnerPlante(nx, ny);
                if (voisine.Type == this.Type && voisine.MaladieActuelle == null)
                {
                    if (rnd.NextDouble() < 0.3) // propagation élevée
                    {
                        voisine.MaladieActuelle = new Maladie(this.MaladieActuelle.Type, this.MaladieActuelle.Duree, this.MaladieActuelle.Severite);
                    }
                }
            }
        }
    }

    public override string ToString()
    {
        return $"Type: {Type}, Jauge de Satisfaction: {TauxCroissance}";
    }
}
