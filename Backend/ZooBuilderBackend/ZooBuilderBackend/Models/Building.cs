namespace ZooBuilderBackend.Models;

public class Building
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public int SizeHeight { get; set; }
    public int SizeWidth { get; set; }
    public decimal Costs { get; set; }
    public int AnimalCapacity { get; set; }
    public decimal Revenue { get; set; }
    
    public int? AnimalId { get; set; }
    public Animal Animal { get; set; }
}