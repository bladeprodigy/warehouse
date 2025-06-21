namespace WarehouseShared;

public record ItemDto(
    int Id,
    string Name,
    string SKU,
    int Quantity,
    int LocationId
);