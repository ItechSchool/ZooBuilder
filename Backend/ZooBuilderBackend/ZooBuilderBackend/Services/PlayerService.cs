using ZooBuilderBackend.Data;
using ZooBuilderBackend.Models;

namespace ZooBuilderBackend.Services;

public class PlayerService
{
    private ApplicationDbContext db = new ApplicationDbContext();
    
    public PlayerService(ApplicationDbContext context)
    {
        db = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public void CreatePlayer(string deviceId)
    {
        if (string.IsNullOrEmpty(deviceId))
        {
            throw new Exception("Missing Device Id");
        }

        if (PlayerExists(deviceId))
        {
            throw new Exception("This player already exists!");
        }
        var player = new Player { DeviceId = deviceId };

        db.Player.Add(player);
        db.SaveChanges();
    }

    private bool PlayerExists(string deviceId)
    {
        return db.Player.Any(b => b.DeviceId == deviceId);
    }
}