public class Plante
{
    public string Type { get; set; }
    public int PlantePositionX { get; set; }
    public int PlantePositionY { get; set; }
    public string Affichage { get; set; } = "P";
    public Plante(string type, int x, int y) 
    {
        Type = type;
        PlantePositionX = x;
        PlantePositionY = y;
    }
}