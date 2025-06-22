using Microsoft.EntityFrameworkCore;
using WarehouseApi.Db;
using WarehouseApi.Interfaces;
using WarehouseApi.Models;
using WarehouseShared;

namespace WarehouseApi.Services;

public class StockMovementService(AppDbContext ctx) : IStockMovementService
{
    public async Task<List<StockMovementDto>> GetAllAsync() =>
        await ctx.StockMovements
            .Include(sm => sm.Item)
            .ThenInclude(i => i.Location)
            .OrderByDescending(sm => sm.MovedAt)
            .Select(sm => new StockMovementDto(
                sm.Id,
                new ItemDto(
                    sm.Item.Id,
                    sm.Item.Name,
                    sm.Item.SKU,
                    sm.Item.Quantity,
                    new LocationDto(
                        sm.Item.Location.Id,
                        sm.Item.Location.Name,
                        sm.Item.Location.Description
                    )
                ),
                sm.ValueBefore,
                sm.ValueAfter,
                sm.Reason,
                sm.MovedAt
            ))
            .ToListAsync();

    public async Task<List<StockMovementDto>> GetForItemAsync(int itemId) =>
        await ctx.StockMovements
            .Where(sm => sm.ItemId == itemId)
            .Include(sm => sm.Item)
            .ThenInclude(i => i.Location)
            .OrderByDescending(sm => sm.MovedAt)
            .Select(sm => new StockMovementDto(
                sm.Id,
                new ItemDto(
                    sm.Item.Id,
                    sm.Item.Name,
                    sm.Item.SKU,
                    sm.Item.Quantity,
                    new LocationDto(
                        sm.Item.Location.Id,
                        sm.Item.Location.Name,
                        sm.Item.Location.Description
                    )
                ),
                sm.ValueBefore,
                sm.ValueAfter,
                sm.Reason,
                sm.MovedAt
            ))
            .ToListAsync();

    public async Task<StockMovementDto?> GetByIdAsync(int itemId, int movementId) =>
        await ctx.StockMovements
            .Where(sm => sm.ItemId == itemId && sm.Id == movementId)
            .Include(sm => sm.Item)
            .ThenInclude(i => i.Location)
            .Select(sm => new StockMovementDto(
                sm.Id,
                new ItemDto(
                    sm.Item.Id,
                    sm.Item.Name,
                    sm.Item.SKU,
                    sm.Item.Quantity,
                    new LocationDto(
                        sm.Item.Location.Id,
                        sm.Item.Location.Name,
                        sm.Item.Location.Description
                    )
                ),
                sm.ValueBefore,
                sm.ValueAfter,
                sm.Reason,
                sm.MovedAt
            ))
            .FirstOrDefaultAsync();


    public async Task<StockMovementDto> AdjustStockAsync(int itemId, CreateMovementDto dto)
    {
        var item = await ctx.Items.FindAsync(itemId)
                   ?? throw new KeyNotFoundException($"Item {itemId} not found.");

        var before = item.Quantity;
        var after = before + dto.Change;

        if (after < 0)
            throw new InvalidOperationException(
                $"Cannot reduce stock below zero. Current stock: {before}, change: {dto.Change}.");

        item.Quantity = after;

        var movement = new StockMovement
        {
            ItemId = itemId,
            ValueBefore = before,
            ValueAfter = after,
            Reason = dto.Reason
        };
        ctx.StockMovements.Add(movement);

        await ctx.SaveChangesAsync();

        await ctx.Entry(movement).Reference(m => m.Item).LoadAsync();
        await ctx.Entry(movement.Item).Reference(i => i.Location).LoadAsync();

        return new StockMovementDto(
            movement.Id,
            new ItemDto(
                movement.Item.Id,
                movement.Item.Name,
                movement.Item.SKU,
                movement.Item.Quantity,
                new LocationDto(
                    movement.Item.Location.Id,
                    movement.Item.Location.Name,
                    movement.Item.Location.Description
                )
            ),
            movement.ValueBefore,
            movement.ValueAfter,
            movement.Reason,
            movement.MovedAt
        );
    }
}