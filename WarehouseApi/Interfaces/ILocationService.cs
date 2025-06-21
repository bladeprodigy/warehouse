using WarehouseShared;

namespace WarehouseApi.Interfaces;

public interface ILocationService
{
    Task<List<LocationDto>> GetAllAsync();
}