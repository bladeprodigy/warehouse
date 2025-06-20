namespace WarehouseShared;

public class StockMovementDtos
{
    public record StockMovementDto(
        int Id,
        int ItemId,
        int Change,
        string Reason,
        DateTime MovedAt
    );

    public record CreateMovementDto(
        int Change,
        string Reason
    );
}