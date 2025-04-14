public class Maladies 
{
    Random random = new Random();
    public string Type { get; set; }

    Plante Plante { get; set; }

    public double Probabilite { get; set; }

    public int Duree { get; set; }

    public int DureeRestante { get; set; } = 0;

    public Maladies(string type, double probabilite, int duree, Plante plante)
    {
        Type = type;
        Probabilite = probabilite;
        Duree = duree;
        Plante = plante;
    }

    public void AppliquerMaladie()
    {
        double chance = random.NextDouble();
        if (DureeRestante > 0 && chance < Probabilite)
        {
            Plante.TauxCroissance -= 10; // Reset the plant's growth cycle
            DureeRestante--;
        }
    }
}