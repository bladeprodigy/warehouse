using Microsoft.EntityFrameworkCore;
using WarehouseApi.Db;
using WarehouseApi.Models;

namespace WarehouseApi.Services;

public class DataSeeder(AppDbContext context)
{
    public async Task SeedAsync()
    {
        if (!await context.Users.AnyAsync())
        {
            context.Users.Add(new User
            {
                Username = "test",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("test"),
                Role = "Employee"
            });
        }

        if (!await context.Locations.AnyAsync())
        {
            context.Locations.AddRange(
                new Location { Name = "Aisle 1", Description = "Front section" },
                new Location { Name = "Aisle 2", Description = "Back section" }
            );
        }

        await context.SaveChangesAsync();
    }
}