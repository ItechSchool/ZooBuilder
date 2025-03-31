namespace ZooBuilderBackend.Models;

public class Player
{
    public int Id { get; set; }
    public string DeviceId { get; set; }
    
    public ICollection<Zoo> Zoos { get; set; }
}