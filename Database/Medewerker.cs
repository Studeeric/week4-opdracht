namespace Database;

public class Medewerker : Gebruiker
{
    // public int Id { get; set; }
    public List<Onderhoud>? Coordineert {get; set;}
    public List<Onderhoud>? Doet {get; set;}
    public Medewerker(string Email): base(Email){}
}