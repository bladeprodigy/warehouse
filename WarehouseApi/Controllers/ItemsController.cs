using System.Data;
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
        [FromQuery] string? search,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
        => svc.GetAllAsync(search, pageNumber, pageSize);

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ItemDto>> Get(int id)
    {
        var item = await svc.GetByIdAsync(id);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> Create([FromBody] CreateItemDto dto)
    {
        try
        {
            var created = await svc.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }
        catch (DuplicateNameException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateItemDto dto)
    {
        try
        {
            return await svc.UpdateAsync(id, dto)
                ? NoContent()
                : NotFound();
        }
        catch (DuplicateNameException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        return await svc.DeleteAsync(id)
            ? NoContent()
            : NotFound();
    }
}