using Database;
namespace Database.Tests;

public class UnitTest1
{
    [Fact]
    public async Task BoekReserveringWordtNietGemaakt()
    {
        // Arrange
        DatabaseContext dbcontext = new DatabaseContext();
        Gast gast = new Gast("email@email.com");
        Attractie attractie = new Attractie("Testnaam");
        DateTimeBereik bestaandeData = new DateTimeBereik();
            bestaandeData.Begin = DateTime.Now.AddDays(-5);
            bestaandeData.Eind = DateTime.Now.AddDays(5);
        Reservering reservering = new Reservering(){Gast = gast, Attractie = attractie, Data = bestaandeData};
        gast.Reserveringen.Add(reservering);
        attractie.Reserveringen.Add(reservering);
        DateTimeBereik aanvraagData = new DateTimeBereik();
            aanvraagData.Begin = DateTime.Now.AddDays(-1);
            aanvraagData.Eind = DateTime.Now.AddDays(1);
        dbcontext.Attracties.Add(attractie);
        dbcontext.Gasten.Add(gast);
        dbcontext.Reserveringen.Add(reservering);
        dbcontext.Entry(attractie).Collection(x => x.Reserveringen).Load();
        // Act
        var gelukt = await dbcontext.Boek(gast, attractie, aanvraagData);

        // Assert
        Assert.False(gelukt);
    }
}