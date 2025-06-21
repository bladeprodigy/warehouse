using WarehouseShared;

namespace WarehouseApi.Interfaces;

public interface IItemService
{
    Task<List<ItemDto>> GetAllAsync(
        string? searchName = null,
        string? searchSku = null,
        int pageNumber = 1,
        int pageSize = 10);

    Task<ItemDto?> GetByIdAsync(int id);
    Task<ItemDto> CreateAsync(CreateItemDto dto);
    Task<bool> UpdateAsync(int id, CreateItemDto dto);
    Task<bool> DeleteAsync(int id);
}