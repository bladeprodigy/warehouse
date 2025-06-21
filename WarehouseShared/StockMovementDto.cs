namespace WarehouseShared;

public record StockMovementDto(
    int Id,
    int ItemId,
    int Change,
    string Reason,
    DateTime MovedAt
);