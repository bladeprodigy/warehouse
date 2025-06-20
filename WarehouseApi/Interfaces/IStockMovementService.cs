using WarehouseShared;

namespace WarehouseApi.Interfaces;

public interface IStockMovementService
{
    Task<StockMovementDtos.StockMovementDto> AdjustStockAsync(int itemId,
        StockMovementDtos.CreateMovementDto dto);
}