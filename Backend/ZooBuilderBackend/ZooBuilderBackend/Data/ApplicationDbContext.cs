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
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    
        modelBuilder.Entity<Animal>().HasData(
            new Animal { Id = 1, Species = "Lion", Attraction = 5, Costs = 15, Diet = "Meat", Hunger = 4 },
            new Animal { Id = 2, Species = "Giraffe", Attraction = 4, Costs = 12, Diet = "Vegetables", Hunger = 3 },
            new Animal { Id = 3, Species = "Elephant", Attraction = 5, Costs = 20, Diet = "Vegetables", Hunger = 4 },
            new Animal { Id = 4, Species = "Penguin", Attraction = 2, Costs = 8, Diet = "Fish", Hunger = 2 },
            new Animal { Id = 5, Species = "Kangaroo", Attraction = 4, Costs = 12, Diet = "Vegetables", Hunger = 2 },
            new Animal { Id = 6, Species = "Zebra", Attraction = 3, Costs = 10, Diet = "Vegetables", Hunger = 3 },
            new Animal { Id = 7, Species = "Panda", Attraction = 5, Costs = 18, Diet = "Bamboo", Hunger = 2 },
            new Animal { Id = 8, Species = "Rat", Attraction = 1, Costs = 3, Diet = "Meat", Hunger = 1 }
        );
        
        modelBuilder.Entity<Building>().HasData(
            new Building { Id = 1, Name = "Lion Den", Type = "Enclosure", SizeHeight = 4, SizeWidth = 4, Costs = 30, AnimalId = 1, Capacity = 5 },
            new Building { Id = 2, Name = "Giraffe Enclosure", Type = "Enclosure", SizeHeight = 6, SizeWidth = 6, Costs = 35, AnimalId = 2,  Capacity = 4 },
            new Building { Id = 3, Name = "Elephant Habitat", Type = "Enclosure", SizeHeight = 8, SizeWidth = 8, Costs = 50, AnimalId = 3, Capacity = 3 },
            new Building { Id = 4, Name = "Penguin Cove", Type = "Enclosure", SizeHeight = 3, SizeWidth = 3, Costs = 20, AnimalId = 4, Capacity = 10 },
            new Building { Id = 5, Name = "Kangaroo Pen", Type = "Enclosure", SizeHeight = 5, SizeWidth = 5, Costs = 25, AnimalId = 5, Capacity = 6 },
            new Building { Id = 6, Name = "Zebra Zone", Type = "Enclosure", SizeHeight = 5, SizeWidth = 5, Costs = 25, AnimalId = 6, Capacity = 6 },
            new Building { Id = 7, Name = "Panda Sanctuary", Type = "Enclosure", SizeHeight = 6, SizeWidth = 6, Costs = 40,  AnimalId = 7, Capacity = 2 },
            new Building { Id = 8, Name = "Rat Cage", Type = "Enclosure", SizeHeight = 1, SizeWidth = 1, Costs = 5,  AnimalId = 8, Capacity = 1 },
            
            new Building { Id = 9, Name = "Food Truck", Type = "Snack", SizeHeight = 2, SizeWidth = 1, Costs = 10, Capacity = 300, MaxRevenue = 30 },
            new Building { Id = 10, Name = "Ice Cream Stand", Type = "Snack", SizeHeight = 2, SizeWidth = 1, Costs = 8, Capacity = 100, MaxRevenue = 20 },
            new Building { Id = 11, Name = "Lemonade Stand", Type = "Snack", SizeHeight = 2, SizeWidth = 1, Costs = 7, Capacity = 100, MaxRevenue = 15 },
            new Building { Id = 12, Name = "Jungle Cafe", Type = "Snack", SizeHeight = 6, SizeWidth = 4, Costs = 20, Capacity = 300, MaxRevenue = 50 }
            );
    }
}