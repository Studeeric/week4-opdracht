namespace Database;

public class DateTimeBereik
{
    public int Id { get; set; }
    public DateTime Begin { get; set; }
    public DateTime? Eind { get; set; }

    public bool Eindigt()
    {
        return Eind != null;
    }

    public bool Overlapt(DateTimeBereik that)
    {
        if (that.Eindigt())
        {
            if (that.Begin < this.Eind && that.Eind > this.Begin)
            {
                return true;
            }
            return false;
        } else
        {
            if (that.Begin < this.Eind)
            {
                return true;
            }
            return false;
        }
    }
}