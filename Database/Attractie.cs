namespace Database;

public class Attractie
{
    public int Id {get; set;}
    public string Naam {get; set;}
    public List<Reservering> Reserveringen {get; set;} = new List<Reservering>();
    public List<Onderhoud> Onderhouds {get; set;} = new List<Onderhoud>();
    public readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);
    
    
    public Attractie (string Naam)
    {
        this.Naam = Naam;
    }

    public async Task<bool> OnderhoudBezig(DatabaseContext c)
    {
        foreach (Onderhoud o in Onderhouds)
        {
            if (o.Data.Eind > DateTime.Now)
            {
                return true;
            }
        }
        return false;
    }

    public async Task<bool> Vrij(DatabaseContext c, DateTimeBereik d)
    {
        c.Entry(this).Collection(x => x.Reserveringen).Load();
        var reseveringen = c.Entry(this).Collection(x => x.Reserveringen).Query().Where(x => x.Attractie.Id == this.Id);
        foreach (Reservering r in reseveringen)
        {
            if (d.Overlapt(r.Data))
            {
                return false;
            }
        }
        return true;
    }
}