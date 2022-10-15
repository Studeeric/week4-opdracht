using Microsoft.EntityFrameworkCore;

namespace Database;

abstract class Rapport
{
    public abstract string Naam();
    public abstract Task<string> Genereer();
    public async Task VoerUit() => await File.WriteAllTextAsync(Naam() + ".txt", await Genereer());
    public async Task VoerPeriodiekUit(Func<bool> stop)
    {
        while (!stop())
        {
            await VoerUit();
            await Task.Delay(1000);
        }
    }
}

class DemografischRapport : Rapport
{
    private DatabaseContext context;
    public DemografischRapport(DatabaseContext context) => this.context = context;
    public override string Naam() => "Demografie";
    public override async Task<string> Genereer()
    {
        string ret = "Dit is een demografisch rapport: \n";
        ret += $"Er zijn in totaal { await AantalGebruikers() } gebruikers van dit platform (dat zijn gasten en medewerkers)\n";
        var dateTime = new DateTime(2000, 1, 1);
        ret += $"Er zijn { await AantalSinds(dateTime) } bezoekers sinds { dateTime }\n";
        if (await AlleGastenHebbenReservering())
            ret += "Alle gasten hebben een reservering\n";
        else
            ret += "Niet alle gasten hebben een reservering\n";
        ret += $"Het percentage bejaarden is { await PercentageBejaarden() }%\n";

        ret += $"De oudste gast heeft een leeftijd van { await HoogsteLeeftijd() } \n";

        ret += "De verdeling van de gasten per dag is als volgt: \n";
        var dagAantallen = await VerdelingPerDag();
        var totaal = dagAantallen.Select(t => t.aantal).Max();
        foreach (var dagAantal in dagAantallen)
            ret += $"{ dagAantal.dag }: { new string('#', (int)(dagAantal.aantal / (double)totaal * 20)) }\n";

        // ret += $"{ await FavorietCorrect() } gasten hebben de favoriete attractie inderdaad het vaakst bezocht. \n";

        return ret;
    }
    private async Task<int> AantalGebruikers() => context.Gebruikers.Count();
    private async Task<bool> AlleGastenHebbenReservering() => context.Gasten.All(Gast => Gast.Reserveringen.Any());
    private async Task<int> AantalSinds(DateTime sinds) => context.Gasten.Where(x => x.EersteBezoek >= sinds).Count();
    private async Task<Gast> GastBijEmail(string email) => context.Gasten.Single(gast => gast.Email == email);
    private async Task<Gast?> GastBijGeboorteDatum(DateTime d) => context.Gasten.Single(x => x.GeboorteDatum == d);
    private async Task<double> PercentageBejaarden() => (double)context.Gasten.Where(g => (DateTime.Today.Year - g.GeboorteDatum.Year) > 80).Count() / (double)context.Gasten.Count() * 100.0;
    private async Task<int> HoogsteLeeftijd() => context.Gasten.Max(x => DateTime.Today.Year - x.GeboorteDatum.Year);
    private async Task<IEnumerable<Gast>> Blut(IEnumerable<Gast> g) => g.Where(x => x.Credits == 0);
    private async Task<(string dag, int aantal)[]> VerdelingPerDag() => context.Gasten.Select((g) => g.EersteBezoek).ToList().Select((g) => g.DayOfWeek).GroupBy((g) => g.ToString()).Select((g) => (g.Key, g.Count())).ToArray();
    private async Task<List<(Gast,int)>> GastenMetActiviteit(IEnumerable<Gast> g) => g.Select(g => (g, g.Reserveringen.Count())).ToList();
    // private async Task<int> FavorietCorrect() => /* ... */; 
}


public class MainClass
{
    private static async Task<T> Willekeurig<T>(DbContext c) where T : class => await c.Set<T>().OrderBy(r => EF.Functions.Random()).FirstAsync();
    public static async Task Main(string[] args)
    {
        Random random = new Random(1);
        using (DatabaseContext c = new DatabaseContext())
        {
            c.Attracties.RemoveRange(c.Attracties);
            c.Gebruikers.RemoveRange(c.Gebruikers);
            c.Gasten.RemoveRange(c.Gasten);
            c.Medewerkers.RemoveRange(c.Medewerkers);
            c.Reserveringen.RemoveRange(c.Reserveringen);
            c.Onderhoud.RemoveRange(c.Onderhoud);

            c.SaveChanges();

            foreach (string attractie in new string[] { "Reuzenrad", "Spookhuis", "Achtbaan 1", "Achtbaan 2", "Draaimolen 1", "Draaimolen 2" })
                c.Attracties.Add(new Attractie(attractie));

            c.SaveChanges();

            for (int i=0;i<40;i++)
                c.Medewerkers.Add(new Medewerker($"medewerker{i}@mail.com"));
            c.SaveChanges();

            for (int i = 0; i < 10000; i++)
            {
                var geboren = DateTime.Now.AddDays(-random.Next(36500));
                var nieuweGast = new Gast($"gast{i}@mail.com") { GeboorteDatum = geboren, EersteBezoek = geboren + (DateTime.Now - geboren) * random.NextDouble(), Credits = random.Next(5) };
                if (random.NextDouble() > .6)
                    nieuweGast.Favoriet = await Willekeurig<Attractie>(c);
                c.Gasten.Add(nieuweGast);
            }
            c.SaveChanges();

            for (int i = 0; i < 10; i++)
                (await Willekeurig<Gast>(c)).Begeleider = await Willekeurig<Gast>(c);
            c.SaveChanges();


            Console.WriteLine("Finished initialization");

            Console.Write(await new DemografischRapport(c).Genereer());
            Console.ReadLine();
        }
    }
}