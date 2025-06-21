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
            .OrderByDescending(sm => sm.MovedAt)
            .Select(sm => new StockMovementDto(
                sm.Id, sm.ItemId, sm.Change, sm.Reason, sm.MovedAt))
            .ToListAsync();

    public async Task<List<StockMovementDto>> GetForItemAsync(int itemId) =>
        await ctx.StockMovements
            .Where(sm => sm.ItemId == itemId)
            .OrderByDescending(sm => sm.MovedAt)
            .Select(sm => new StockMovementDto(
                sm.Id, sm.ItemId, sm.Change, sm.Reason, sm.MovedAt))
            .ToListAsync();

    public async Task<StockMovementDto?>
        GetByIdAsync(int itemId, int movementId) =>
        await ctx.StockMovements
            .Where(sm => sm.ItemId == itemId && sm.Id == movementId)
            .Select(sm => new StockMovementDto(
                sm.Id, sm.ItemId, sm.Change, sm.Reason, sm.MovedAt))
            .FirstOrDefaultAsync();

    public async Task<StockMovementDto> AdjustStockAsync(int itemId,
        CreateMovementDto dto)
    {
        var item = await ctx.Items.FindAsync(itemId)
                   ?? throw new KeyNotFoundException($"Item {itemId} not found.");

        item.Quantity += dto.Change;

        var movement = new StockMovement
        {
            ItemId = itemId,
            Change = dto.Change,
            Reason = dto.Reason
        };
        ctx.StockMovements.Add(movement);
        await ctx.SaveChangesAsync();

        return new StockMovementDto(
            movement.Id, movement.ItemId, movement.Change, movement.Reason, movement.MovedAt);
    }
}