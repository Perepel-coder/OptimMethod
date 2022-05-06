namespace Models;

public class Parameter
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string Notation { get; set; }
    public int UnitOfMeasId { get; set; }
    public UnitOfMeas UnitOfMeas { get; set; }
    public ICollection<TaskParameterValue>? Values { get; set; }
}