using SharedNetwork.Dtos;
using ZooBuilderBackend.Data;

namespace ZooBuilderBackend.Services;

public class StartUpService(ApplicationDbContext context)
{
    private readonly ApplicationDbContext context = new();

    /// <summary>This method creates all the Data needed at Start Up.
    /// The Method throws Exceptions when the player or the zoo could not be found.
    /// Remember: Handle exceptions when calling this method!</summary>
    public StartUpDataDto LoadStartUpData(string deviceId)
    {
        var player = context.Player.SingleOrDefault(p => p.DeviceId == deviceId);
        if (player == null)
        {
            throw new Exception("Could not find Player with deviceId " + deviceId);
        }

        var zoo = context.Zoo.SingleOrDefault(z => z.PlayerId == player.Id);
        if (zoo == null)
        {
            throw new Exception("Could not find an existing Zoo for Player with id " + player.Id);
        }

        var zooDto = new ZooDto
        {
            Id = zoo.Id,
            Name = zoo.Name,
            Money = zoo.Money,
            Vegetables = zoo.Vegetables,
            Meat = zoo.Meat,
            PlayerId = zoo.PlayerId
        };

        var buildingDtos = context.Building.Select(b => new BuildingDto
        {
            Id = b.Id,
            Name = b.Name,
            Type = b.Type,
            SizeWidth = b.SizeWidth,
            SizeHeight = b.SizeHeight,
            Costs = b.Costs,
            Capacity = b.Capacity,
            MaxRevenue = b.MaxRevenue,
            AnimalId = b.AnimalId
        }).ToArray();

        var animalDtos = context.Animal.Select(a => new AnimalDto
        {
            Id = a.Id,
            Species = a.Species,
            Costs = a.Costs,
            Diet = a.Diet,
            Attraction = a.Attraction,
            Hunger = a.Hunger
        }).ToArray();

        var gridPlacements = context.GridPlacement.Where(gp => gp.ZooId == zoo.Id).ToList();
        var gridPlacementDtos = Array.Empty<GridPlacementDto>();
        if (gridPlacements.Count > 0)
        {
            gridPlacementDtos = gridPlacements.Select(gp => new GridPlacementDto
            {
                Id = gp.Id,
                BuildingId = gp.BuildingId,
                ZooId = gp.ZooId,
                XCoordinate = gp.XCoordinate,
                YCoordinate = gp.YCoordinate,
                AnimalCount = gp.AnimalCount,
                Connected = gp.Connected

            }).ToArray();
        }

        return new StartUpDataDto
        {
            Buildings = buildingDtos,
            Animals = animalDtos,
            GridPlacements = gridPlacementDtos,
            Zoo = zooDto
        };
    }
}