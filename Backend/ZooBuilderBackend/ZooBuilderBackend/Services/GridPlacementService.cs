using Microsoft.EntityFrameworkCore;
using ZooBuilderBackend.Data;
using ZooBuilderBackend.Models;

namespace ZooBuilderBackend.Services;

public class GridPlacementService
{
    private readonly ApplicationDbContext _db = new();
    /// <summary>In this method a building gets placed on the city grid.</summary>
    /// <summary>The method checks for certain things (money, spot where it should be placed, buildingId, and ZooId) and throws an exception when it fails.</summary>
    /// <summary> Remember: Handle the exceptions when calling this method!</summary>
    public void PlaceBuilding(int buildingId, int x, int y, int zooId)
    {
        var building = _db.Building.Find(buildingId);
        var zoo = _db.Zoo.Find(zooId);
        
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
            throw new Exception("This spot is not free, please building somewhere else.");
        }

        if (!HasEnoughMoney(zoo.Money, building.Costs))
        {
            throw new Exception("Zoo with id " + zooId + " does not have enough money to buy building " + buildingId);
        }

        var entity = new GridPlacement
            { XCoordinate = x, YCoordinate = y, Connected = false, ZooId = zooId, BuildingId = buildingId };
        _db.GridPlacement.Add(entity);
        zoo.Money -= building.Costs;
        _db.SaveChanges();
    }

    private static bool HasEnoughMoney(int money, int buildingCosts)
    {
        return money >= buildingCosts;
    }

    private bool IsAreaFree(int x, int y, Building building, int zooId)
    {
        var rectBx1 = x;
        var rectBx2 = x + building.SizeWidth;
        var rectBy1 = y;
        var rectBy2 = y + building.SizeHeight;

        var allBuildings = _db.GridPlacement.Where(gp => gp.ZooId == zooId)
            .Include(gridPlacement => gridPlacement.Building).ToArray();
        foreach (var placement in allBuildings)
        {
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
}