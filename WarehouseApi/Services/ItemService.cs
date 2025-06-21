using Microsoft.EntityFrameworkCore;
using WarehouseApi.Db;
using WarehouseApi.Interfaces;
using WarehouseApi.Models;
using WarehouseShared;

namespace WarehouseApi.Services;

public class ItemService(AppDbContext ctx) : IItemService
{
    public async Task<List<ItemDto>> GetAllAsync(
        string? searchName = null,
        string? searchSku = null,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var query = ctx.Items.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchName))
            query = query.Where(i => i.Name.Contains(searchName));
        if (!string.IsNullOrWhiteSpace(searchSku))
            query = query.Where(i => i.SKU.Contains(searchSku));

        return await query
            .OrderBy(i => i.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(i => new ItemDto(i.Id, i.Name, i.SKU, i.Quantity, i.LocationId))
            .ToListAsync();
    }

    public async Task<ItemDto?> GetByIdAsync(int id) => await ctx.Items
        .Where(i => i.Id == id)
        .Select(i => new ItemDto(i.Id, i.Name, i.SKU, i.Quantity, i.LocationId))
        .FirstOrDefaultAsync();

    public async Task<ItemDto> CreateAsync(CreateItemDto dto)
    {
        var entity = new Item
        {
            Name = dto.Name,
            SKU = dto.SKU,
            Quantity = 0,
            LocationId = dto.LocationId
        };
        ctx.Items.Add(entity);
        await ctx.SaveChangesAsync();
        return new ItemDto(entity.Id, entity.Name, entity.SKU, entity.Quantity,
            entity.LocationId);
    }

    public async Task<bool> UpdateAsync(int id, CreateItemDto dto)
    {
        var entity = await ctx.Items.FindAsync(id);
        if (entity == null) return false;
        entity.Name = dto.Name;
        entity.SKU = dto.SKU;
        entity.LocationId = dto.LocationId;
        await ctx.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await ctx.Items.FindAsync(id);
        if (entity == null) return false;
        ctx.Items.Remove(entity);
        await ctx.SaveChangesAsync();
        return true;
    }
}