using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseApi.Interfaces;
using WarehouseShared;

namespace WarehouseApi.Controllers;

[ApiController]
[Route("api/items/{itemId:int}/movements")]
[Authorize]
public class ItemStockMovementsController(IStockMovementService svc) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(int itemId,
        [FromBody] CreateMovementDto dto)
    {
        var result = await svc.AdjustStockAsync(itemId, dto);
        return CreatedAtAction(
            nameof(GetById),
            new { itemId, movementId = result.Id },
            result);
    }

    [HttpGet]
    public async Task<ActionResult<List<StockMovementDto>>> GetAllForItem(
        int itemId)
    {
        var list = await svc.GetForItemAsync(itemId);
        return Ok(list);
    }

    [HttpGet("{movementId:int}")]
    public async Task<ActionResult<StockMovementDto>> GetById(int itemId,
        int movementId)
    {
        var mv = await svc.GetByIdAsync(itemId, movementId);
        return mv is null ? NotFound() : Ok(mv);
    }
}