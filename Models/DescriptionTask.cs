namespace Models;

public class DescriptionTask
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<TaskParameterValue>? Values { get; set; }
}