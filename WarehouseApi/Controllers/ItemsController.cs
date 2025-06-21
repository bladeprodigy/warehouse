using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseApi.Interfaces;
using WarehouseShared;

namespace WarehouseApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ItemsController(IItemService svc) : ControllerBase
{
    [HttpGet]
    public Task<List<ItemDto>> GetAll(
        [FromQuery] string? searchName,
        [FromQuery] string? searchSku,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        return svc.GetAllAsync(searchName, searchSku, pageNumber, pageSize);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ItemDto>> Get(int id)
    {
        var item = await svc.GetByIdAsync(id);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> Create([FromBody] CreateItemDto dto)
    {
        var created = await svc.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateItemDto dto)
    {
        return await svc.UpdateAsync(id, dto)
            ? NoContent()
            : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        return await svc.DeleteAsync(id)
            ? NoContent()
            : NotFound();
    }
}