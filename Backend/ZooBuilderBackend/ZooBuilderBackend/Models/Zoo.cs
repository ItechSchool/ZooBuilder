using System.ComponentModel.DataAnnotations;

namespace ZooBuilderBackend.Models;

public class Zoo
{
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = "Zoo #" + new Random().Next(1, 99999);
    public int Money { get; set; }
    public int Vegetables { get; set; }
    public int Meat { get; set; }

    public int PlayerId { get; set; }
    public Player Player { get; set; }

    public ICollection<GridPlacement> GridPlacements { get; set; }
}