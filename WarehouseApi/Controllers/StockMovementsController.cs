using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseApi.Interfaces;
using WarehouseShared;

namespace WarehouseApi.Controllers;

[ApiController]
[Route("api/items/{itemId:int}/movements")]
[Authorize]
public class StockMovementsController(IStockMovementService svc) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<StockMovementDtos.StockMovementDto>> Post(
        int itemId,
        [FromBody] StockMovementDtos.CreateMovementDto dto)
    {
        var result = await svc.AdjustStockAsync(itemId, dto);
        return CreatedAtAction(
            nameof(GetById),
            new { itemId, movementId = result.Id },
            result
        );
    }

    [HttpGet("{movementId}")]
    public async Task<ActionResult<StockMovementDtos.StockMovementDto>> GetById(
        int itemId, int movementId)
    {
        // (Implement retrieval if you need it; otherwise return NoContent)
        return NoContent();
    }
}