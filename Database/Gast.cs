using System.ComponentModel.DataAnnotations.Schema;

namespace Database;

public class Gast : Gebruiker
{
    // public int Id {get; set;}
    public int Credits {get; set;}
    public DateTime GeboorteDatum {get; set;}
    public DateTime EersteBezoek {get; set;}
    public List<Reservering> Reserveringen {get; set;} = new List<Reservering>();
    public Gast? Begeleider {get; set;}
    public Gast? Begeleidt {get; set;}
    public GastInfo gastInfo;
    public Attractie? Favoriet { get; set; }
    public Gast(string Email): base(Email){
        this.gastInfo = new GastInfo(this);
    }

}