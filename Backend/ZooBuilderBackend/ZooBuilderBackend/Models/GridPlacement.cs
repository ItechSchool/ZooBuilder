namespace ZooBuilderBackend.Models;

public class GridPlacement
{
    public int Id { get; set; }
    public int XCoordinate { get; set; }
    public int YCoordinate { get; set; }
    public int AnimalCount { get; set; }
    public bool Connected { get; set; }

    public int ZooId { get; set; }
    public Zoo Zoo { get; set; }

    public int BuildingId { get; set; }
    public Building Building { get; set; }
}