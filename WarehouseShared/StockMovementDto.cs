namespace WarehouseShared;

public record StockMovementDto(
    int Id,
    ItemDto Item,
    int ValueBefore,
    int ValueAfter,
    string Reason,
    DateTime MovedAt
);