namespace Database;

public class GastInfo
{
    public int Id {get; set;}
    public Gast gast = null!;
    public string? LaatstBezochteURL {get; set;}

    protected GastInfo(){}
    public GastInfo(Gast gast){
        this.gast = gast;
    }
}