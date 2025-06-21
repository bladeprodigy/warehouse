namespace WarehouseShared;

public record CreateItemDto(
    string Name,
    string SKU,
    int LocationId
);