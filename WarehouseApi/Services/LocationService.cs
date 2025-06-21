using Microsoft.EntityFrameworkCore;
using WarehouseApi.Db;
using WarehouseApi.Interfaces;
using WarehouseShared;

namespace WarehouseApi.Services;

public class LocationService(AppDbContext ctx) : ILocationService
{
    public async Task<List<LocationDto>> GetAllAsync()
    {
        return await ctx.Locations
            .Select(l => new LocationDto(
                l.Id,
                l.Name,
                l.Description
            ))
            .ToListAsync();
    }
}