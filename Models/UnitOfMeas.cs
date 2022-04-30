namespace Models;

public class UnitOfMeas
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Parameter>? Parameters { get; set; }
}