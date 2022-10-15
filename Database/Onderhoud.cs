namespace Database;

public class Onderhoud
{
    public int Id {get; set;}
    public DateTimeBereik? Data {get; set;}
    public Attractie reparatieAttractie {get; set;}
    public string? Probleem {get; set;}
    public List<Medewerker> WordtGecoordineerdDoor {get; set;} = new List<Medewerker>();
    public List<Medewerker> WordtGedaanDoor {get; set;} = new List<Medewerker>();
}