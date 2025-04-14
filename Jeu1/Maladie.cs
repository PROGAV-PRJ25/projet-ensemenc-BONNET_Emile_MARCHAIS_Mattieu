public class Maladies 
{
    Random random = new Random();
    public string Type { get; set; }

    Plante Plante { get; set; }

    public double Probabilite { get; set; }

    public int Duree { get; set; }

    public int DureeRestante { get; set; }

    public Maladies(string type, double probabilite, int duree, Plante plante)
    {
        Type = type;
        Probabilite = probabilite;
        Duree = duree;
        DureeRestante = duree;
        Plante = plante;
    }

    public void AppliquerMaladie()
    {
        if (DureeRestante > 0)
        {
            double chance = random.NextDouble();
            if (chance < Probabilite)
            {
                Plante.cycleStep --; // Reset the plant's cycle step to indicate it's affected by the disease
                DureeRestante--;
            }
        }
    }
}