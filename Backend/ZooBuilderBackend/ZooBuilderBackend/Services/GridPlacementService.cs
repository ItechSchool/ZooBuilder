using Microsoft.EntityFrameworkCore;
using SharedNetwork.Dtos;
using ZooBuilderBackend.Data;
using ZooBuilderBackend.Models;

namespace ZooBuilderBackend.Services;

public class GridPlacementService(ApplicationDbContext context)
{
    /// <summary>In this method a building gets placed on the city grid.
    /// The method checks for certain things (money, spot where it should be placed, buildingId, and ZooId) and throws an exception when it fails.
    ///  Remember: Handle exceptions when calling this method!</summary>
    public GridPlacementDto PlaceBuilding(int buildingId, int x, int y, int zooId)
    {
        var building = context.Building.Find(buildingId);
        var zoo = context.Zoo.Find(zooId);

        if (building == null)
        {
            throw new Exception("Can not find building with id " + buildingId);
        }
        if (zoo == null)
        {
            throw new Exception("Zoo does not exist " + zooId);
        }
        if (!IsAreaFree(x, y, building, zooId))
        {
            throw new Exception("This spot is not free, please place building somewhere else.");
        }

        if (!HasEnoughMoney(zoo.Money, building.Costs))
        {
            throw new Exception("You don't have enough money to buy this building");
        }

        var entity = new GridPlacement
        { XCoordinate = x, YCoordinate = y, Connected = false, ZooId = zooId, BuildingId = buildingId };
        context.GridPlacement.Add(entity);
        zoo.Money -= building.Costs;
        context.SaveChanges();

        return CreateGridPlacementDto(entity);
    }

    public GridPlacementDto UpdateBuildingPosition(int gridPlacementId, int newX, int newY, int zooId)
    {
        var gridPlacement = context.GridPlacement.Find(gridPlacementId);
        if (gridPlacement == null)
        {
            throw new Exception("Not a valid building in the city.");
        }

        var building = context.Building.FirstOrDefault(b => b.Id == gridPlacement.BuildingId);
        if (building == null)
        {
            throw new Exception("Corresponding building from the catalogue could not be found");
        }
        if (!IsAreaFree(newX, newY, building, zooId, gridPlacementId))
        {
            throw new Exception("This spot is not free, please place building somewhere else.");
        }

        gridPlacement.XCoordinate = newX;
        gridPlacement.YCoordinate = newY;

        context.SaveChanges();

        return CreateGridPlacementDto(gridPlacement);
    }

    private GridPlacementDto CreateGridPlacementDto(GridPlacement gridPlacement)
    {
        return new GridPlacementDto
        {
            Id = gridPlacement.Id,
            AnimalCount = gridPlacement.AnimalCount,
            BuildingId = gridPlacement.BuildingId,
            Connected = gridPlacement.Connected,
            XCoordinate = gridPlacement.XCoordinate,
            YCoordinate = gridPlacement.YCoordinate,
            ZooId = gridPlacement.ZooId
        };
    }

    private bool IsAreaFree(int x, int y, Building building, int zooId, int? gridPlacementId = null)
    {
        var rectBx1 = x;
        var rectBx2 = x + building.SizeWidth;
        var rectBy1 = y;
        var rectBy2 = y + building.SizeHeight;

        var allBuildings = context.GridPlacement.Where(gp => gp.ZooId == zooId)
            .Include(gridPlacement => gridPlacement.Building).ToArray();
        foreach (var placement in allBuildings)
        {
            if (gridPlacementId != null && placement.Id == gridPlacementId)
            {
                continue;
            }
            var rectAx1 = placement.XCoordinate;
            var rectAx2 = rectAx1 + placement.Building.SizeWidth;
            var rectAy1 = placement.YCoordinate;
            var rectAy2 = rectAy1 + placement.Building.SizeHeight;

            if (RectanglesOverlap(rectBx1, rectBx2, rectBy1, rectBy2, rectAx1, rectAx2, rectAy1, rectAy2))
            {
                return false;
            }
        }
        return true;
    }

    private static bool RectanglesOverlap(int rectBx1, int rectBx2, int rectBy1, int rectBy2, int rectAx1, int rectAx2,
        int rectAy1, int rectAy2)
    {
        return rectAx1 < rectBx2 && rectAx2 > rectBx1 && rectAy1 < rectBy2 && rectAy2 > rectBy1;
    }

    private static bool HasEnoughMoney(int money, int buildingCosts)
    {
        return money >= buildingCosts;
    }
}