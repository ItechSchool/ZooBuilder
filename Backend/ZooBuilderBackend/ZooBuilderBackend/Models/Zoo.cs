namespace ZooBuilderBackend.Models;

public class Zoo
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Money { get; set; }
    public int Vegetables { get; set; }
    public int Meat { get; set; }

    public int PlayerId { get; set; }
    public Player Player { get; set; }

    public ICollection<GridPlacement> GridPlacements { get; set; }
}