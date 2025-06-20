using Microsoft.EntityFrameworkCore;
using WarehouseApi.Db;
using WarehouseApi.Interfaces;
using WarehouseApi.Models;
using WarehouseShared;

namespace WarehouseApi.Services;

public class ItemService(AppDbContext ctx) : IItemService
{
    public async Task<List<ItemDtos.ItemDto>> GetAllAsync() => await ctx.Items
        .Select(i => new ItemDtos.ItemDto(i.Id, i.Name, i.SKU, i.Quantity, i.LocationId))
        .ToListAsync();

    public async Task<ItemDtos.ItemDto?> GetByIdAsync(int id) => await ctx.Items
        .Where(i => i.Id == id)
        .Select(i => new ItemDtos.ItemDto(i.Id, i.Name, i.SKU, i.Quantity, i.LocationId))
        .FirstOrDefaultAsync();

    public async Task<ItemDtos.ItemDto> CreateAsync(ItemDtos.CreateItemDto dto)
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
        return new ItemDtos.ItemDto(entity.Id, entity.Name, entity.SKU, entity.Quantity,
            entity.LocationId);
    }

    public async Task<bool> UpdateAsync(int id, ItemDtos.UpdateItemDto dto)
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