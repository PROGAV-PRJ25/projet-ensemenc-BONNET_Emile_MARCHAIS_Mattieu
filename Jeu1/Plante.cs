public class Plante
{
    public string Type { get; set; }
    public int PlantePositionX { get; set; }
    public int PlantePositionY { get; set; }

    private int cycleStep = 0;
    private string[] affichageCycle = new string[] { "P", "p", "." };
    public string Affichage => affichageCycle[cycleStep];

    public Plante(string type, int x, int y)
    {
        Type = type;
        PlantePositionX = x;
        PlantePositionY = y;
    }

    public void MetAJour()
    {
        cycleStep = (cycleStep + 1) % affichageCycle.Length;
    }
}
