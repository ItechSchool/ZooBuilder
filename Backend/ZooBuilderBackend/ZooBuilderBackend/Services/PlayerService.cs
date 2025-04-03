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

        Console.WriteLine(deviceId);

        // Regex pattern to match non-printable characters
        var regex = new Regex(@"[^\x20-\x7E]");

        if (regex.IsMatch(deviceId))
        {
            Console.WriteLine("The string contains non-printable characters.");
        }
        else
        {
            Console.WriteLine("The string does not contain non-printable characters.");
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