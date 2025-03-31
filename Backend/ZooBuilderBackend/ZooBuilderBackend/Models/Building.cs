namespace ZooBuilderBackend.Models;

public class Building
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public int SizeHeight { get; set; }
    public int SizeWidth { get; set; }
    public int Costs { get; set; }
    public int Capacity { get; set; }
    public int MaxRevenue { get; set; }
    
    public int? AnimalId { get; set; }
    public Animal? Animal { get; set; }
}