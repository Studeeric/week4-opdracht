namespace Database;
using Microsoft.EntityFrameworkCore;

public class DatabaseContext : DbContext
{
    public virtual DbSet<Attractie>? Attracties { get; set; }
    public virtual DbSet<Gebruiker>? Gebruikers { get; set; }
    public virtual DbSet<Gast>? Gasten { get; set; }
    public virtual DbSet<Medewerker>? Medewerkers { get; set; }
    public virtual DbSet<Reservering>? Reserveringen { get; set; }
    public virtual DbSet<Onderhoud>? Onderhoud { get; set; }

    public async Task<bool> Boek(Gast g, Attractie a, DateTimeBereik d)
    {
        using var transaction = this.Database.BeginTransaction();
        await a.Semaphore.WaitAsync();
        try
        {
            if (!a.Reserveringen.Any(x => x.Data.Overlapt(d)))
            // if (await a.Vrij(this, d))
            {
                await Reserveringen.AddAsync(new Reservering() { Gast = g, Attractie = a, Data = d });
                g.Credits--;
                this.SaveChanges();
                transaction.Commit();
                return true;
            }
            return false;
        }
        finally { a.Semaphore.Release(); }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Attractie>().ToTable("Attracties").HasKey(a => a.Id);
        builder.Entity<Gebruiker>().ToTable("Gebruikers").HasKey(g => g.Id);
        builder.Entity<Gast>().ToTable("Gasten");
        builder.Entity<GastInfo>().ToTable("GastInfo").HasKey(g => g.Id);
        builder.Entity<Medewerker>().ToTable("Medewerkers");
        builder.Entity<Reservering>().ToTable("Reserveringen").HasKey(r => r.Id);
        builder.Entity<Gast>().HasMany(x => x.Reserveringen).WithOne(x => x.Gast); // Reserveringen met gast koppelen
        builder.Entity<Reservering>().HasOne(x => x.Attractie).WithMany(x => x.Reserveringen); // Reserveringen met attractie gekoppeld
        builder.Entity<Onderhoud>().ToTable("Onderhoud").HasKey(o => o.Id);
        builder.Entity<Attractie>().HasMany(x => x.Onderhouds).WithOne(x => x.reparatieAttractie);
        builder.Entity<Medewerker>().HasMany(x => x.Doet).WithMany(x => x.WordtGedaanDoor);
        builder.Entity<Medewerker>().HasMany(x => x.Coordineert).WithMany(x => x.WordtGecoordineerdDoor);
        builder.Entity<Gast>()
        .HasOne(a => a.Begeleider)
        .WithOne(b => b.Begeleidt);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.UseSqlServer("Server=LAPTOP-ERIC-WIN\\SQLEXPRESS;Initial Catalog=YourDatabase;Integrated Security=true");
    }
}