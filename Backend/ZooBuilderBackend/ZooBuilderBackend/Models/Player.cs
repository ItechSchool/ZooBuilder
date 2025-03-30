namespace ZooBuilderBackend.Models;

public class Player
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    
    public ICollection<Zoo> Zoos { get; set; }
}