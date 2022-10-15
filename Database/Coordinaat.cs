using Microsoft.EntityFrameworkCore;

namespace Database;

[Owned]
public class Coordinaat
{
    public int X {get; set;}
    public int Y {get; set;}
}