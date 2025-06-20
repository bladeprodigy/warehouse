using WarehouseApi.Db;
using WarehouseApi.Interfaces;
using WarehouseApi.Models;
using WarehouseShared;

namespace WarehouseApi.Services;

public class StockMovementService(AppDbContext ctx) : IStockMovementService
{
    public async Task<StockMovementDtos.StockMovementDto> AdjustStockAsync(int itemId,
        StockMovementDtos.CreateMovementDto dto)
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

        return new StockMovementDtos.StockMovementDto(
            movement.Id,
            movement.ItemId,
            movement.Change,
            movement.Reason,
            movement.MovedAt
        );
    }
}