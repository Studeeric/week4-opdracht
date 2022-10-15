using System.ComponentModel.DataAnnotations.Schema;

namespace Database;

public class Reservering
{
    public int Id {get; set;}
    public Gast? Gast {get; set;}
    public Attractie? Attractie {get; set;}
    public DateTimeBereik? Data {get; set;}
}