using System.Data;
using Microsoft.EntityFrameworkCore;
using WarehouseApi.Db;
using WarehouseApi.Interfaces;
using WarehouseApi.Models;
using WarehouseShared;

namespace WarehouseApi.Services;

public class ItemService(AppDbContext ctx) : IItemService
{
    public async Task<List<ItemDto>> GetAllAsync(string? search = null, int pageNumber = 1,
        int pageSize = 10)
    {
        var query = ctx.Items
            .Include(i => i.Location)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(i =>
                i.Name.Contains(search) ||
                i.SKU.Contains(search));
        }

        return await query
            .OrderBy(i => i.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(i => new ItemDto(
                i.Id,
                i.Name,
                i.SKU,
                i.Quantity,
                new LocationDto(
                    i.Location.Id,
                    i.Location.Name,
                    i.Location.Description
                )
            ))
            .ToListAsync();
    }

    public async Task<ItemDto?> GetByIdAsync(int id) =>
        await ctx.Items
            .Include(i => i.Location)
            .Where(i => i.Id == id)
            .Select(i => new ItemDto(
                i.Id,
                i.Name,
                i.SKU,
                i.Quantity,
                new LocationDto(
                    i.Location.Id,
                    i.Location.Name,
                    i.Location.Description
                )
            ))
            .FirstOrDefaultAsync();


    public async Task<ItemDto> CreateAsync(CreateItemDto dto)
    {
        if (await ctx.Items.AnyAsync(i => i.Name == dto.Name))
            throw new DuplicateNameException($"An item with name '{dto.Name}' already exists.");
        if (await ctx.Items.AnyAsync(i => i.SKU == dto.Sku))
            throw new DuplicateNameException($"An item with SKU '{dto.Sku}' already exists.");

        var location = await ctx.Locations.FindAsync(dto.LocationId)
                       ?? throw new KeyNotFoundException($"Location {dto.LocationId} not found.");

        var entity = new Item
        {
            Name = dto.Name,
            SKU = dto.Sku,
            Quantity = 0,
            LocationId = dto.LocationId
        };
        ctx.Items.Add(entity);

        ctx.StockMovements.Add(new StockMovement
        {
            Item = entity,
            ValueBefore = 0,
            ValueAfter = 0,
            Reason =
                $"Added product '{entity.Name}' (SKU: {entity.SKU}) to location '{location.Name}'."
        });

        await ctx.SaveChangesAsync();

        var locDto = new LocationDto(
            location.Id,
            location.Name,
            location.Description
        );

        return new ItemDto(
            entity.Id,
            entity.Name,
            entity.SKU,
            entity.Quantity,
            locDto
        );
    }

    public async Task<bool> UpdateAsync(int id, CreateItemDto dto)
    {
        var entity = await ctx.Items
            .Include(i => i.Location)
            .FirstOrDefaultAsync(i => i.Id == id);
        if (entity == null)
            return false;

        if (await ctx.Items.AnyAsync(i => i.Name == dto.Name && i.Id != id))
            throw new DuplicateNameException($"An item with name '{dto.Name}' already exists.");
        if (await ctx.Items.AnyAsync(i => i.SKU == dto.Sku && i.Id != id))
            throw new DuplicateNameException($"An item with SKU '{dto.Sku}' already exists.");

        var nameChanged = entity.Name != dto.Name;
        var skuChanged = entity.SKU != dto.Sku;
        var locChanged = entity.LocationId != dto.LocationId;

        var oldName = entity.Name;
        var oldSku = entity.SKU;
        var oldLocationName = entity.Location.Name;
        var oldQuantity = entity.Quantity;

        entity.Name = dto.Name;
        entity.SKU = dto.Sku;
        entity.LocationId = dto.LocationId;

        if (nameChanged)
        {
            ctx.StockMovements.Add(new StockMovement
            {
                Item = entity,
                ValueBefore = oldQuantity,
                ValueAfter = oldQuantity,
                Reason = $"Renamed item from '{oldName}' to '{entity.Name}'."
            });
        }

        if (skuChanged)
        {
            ctx.StockMovements.Add(new StockMovement
            {
                Item = entity,
                ValueBefore = oldQuantity,
                ValueAfter = oldQuantity,
                Reason = $"Changed SKU from '{oldSku}' to '{entity.SKU}'."
            });
        }

        if (locChanged)
        {
            var newLoc = await ctx.Locations.FindAsync(dto.LocationId)
                         ?? throw new KeyNotFoundException($"Location {dto.LocationId} not found.");

            ctx.StockMovements.Add(new StockMovement
            {
                Item = entity,
                ValueBefore = oldQuantity,
                ValueAfter = oldQuantity,
                Reason = $"Moved {oldQuantity} units from '{oldLocationName}' to '{newLoc.Name}'."
            });
        }

        await ctx.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await ctx.Items
            .Include(i => i.Location)
            .FirstOrDefaultAsync(i => i.Id == id);
        if (entity == null)
            return false;

        var before = entity.Quantity;
        var locationName = entity.Location.Name;

        ctx.StockMovements.Add(new StockMovement
        {
            ItemId = id,
            ValueBefore = before,
            ValueAfter = 0,
            Reason =
                $"Deleted product '{entity.Name}' (SKU: {entity.SKU}) from location '{locationName}'."
        });

        ctx.Items.Remove(entity);

        await ctx.SaveChangesAsync();
        return true;
    }
}