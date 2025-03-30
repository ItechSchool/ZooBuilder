using Microsoft.EntityFrameworkCore;
using ZooBuilderBackend.Models;

namespace ZooBuilderBackend.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Player> Player { get; set; }
    public DbSet<Zoo> Zoo { get; set; }
    public DbSet<GridPlacement> GridPlacement { get; set; }
    public DbSet<Animal> Animal { get; set; }
    public DbSet<Building> Building { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ZooDb;Username=user;Password=password;");
    }
}