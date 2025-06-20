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
    public Task<List<ItemDtos.ItemDto>> GetAll() => svc.GetAllAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ItemDtos.ItemDto>> Get(int id)
    {
        var item = await svc.GetByIdAsync(id);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<ItemDtos.ItemDto>> Create([FromBody] ItemDtos.CreateItemDto dto)
    {
        var created = await svc.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ItemDtos.UpdateItemDto dto)
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