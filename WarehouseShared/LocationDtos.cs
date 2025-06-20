namespace WarehouseShared;

public class LocationDtos
{
    public record LocationDto(
        int    Id,
        string Name,
        string? Description
    );
}