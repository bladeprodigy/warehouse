using WarehouseShared;

namespace WarehouseApi.Interfaces;

public interface IStockMovementService
{
    Task<List<StockMovementDto>> GetAllAsync();
    Task<List<StockMovementDto>> GetForItemAsync(int itemId);
    Task<StockMovementDto?> GetByIdAsync(int itemId, int movementId);

    Task<StockMovementDto> AdjustStockAsync(int itemId,
        CreateMovementDto dto);
}