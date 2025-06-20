using WarehouseShared;

namespace WarehouseApi.Interfaces;

public interface IItemService
{
    Task<List<ItemDtos.ItemDto>> GetAllAsync();
    Task<ItemDtos.ItemDto?> GetByIdAsync(int id);
    Task<ItemDtos.ItemDto> CreateAsync(ItemDtos.CreateItemDto dto);
    Task<bool> UpdateAsync(int id, ItemDtos.UpdateItemDto dto);
    Task<bool> DeleteAsync(int id);
}