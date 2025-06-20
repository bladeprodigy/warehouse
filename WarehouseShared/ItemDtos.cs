namespace WarehouseShared;

public class ItemDtos
{
    public record ItemDto(
        int Id,
        string Name,
        string SKU,
        int Quantity,
        int LocationId
    );

    public record CreateItemDto(
        string Name,
        string SKU,
        int LocationId
    );

    public record UpdateItemDto(
        string Name,
        string SKU,
        int LocationId
    );
}