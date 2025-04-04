using System.Text.RegularExpressions;
using SharedNetwork;
using ZooBuilderBackend.Data;
using ZooBuilderBackend.Models;

namespace ZooBuilderBackend.Services;

public class PlayerService(ApplicationDbContext context)
{
    public Player Login(string deviceId)
    {
        if (string.IsNullOrEmpty(deviceId))
        {
            throw new ArgumentException("Missing Device Id");
        }
        var player = context.Player.FirstOrDefault(b => b.DeviceId == deviceId);

        return player ?? CreatePlayer(deviceId);
    }

    private Player CreatePlayer(string deviceId)
    {
        var player = new Player { DeviceId = deviceId };


        context.Player.Add(player);
        context.SaveChanges();

        var zoo = new Zoo
        {
            Money = 100,
            Meat = 10,
            Vegetables = 10,
            PlayerId = player.Id
        };
        context.Zoo.Add(zoo);
        context.SaveChanges();

        return player;
    }
}