using WarehouseShared;

namespace WarehouseApi.Interfaces;

public interface ILocationService
{
    Task<List<LocationDtos.LocationDto>> GetAllAsync();
}