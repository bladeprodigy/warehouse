using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseApi.Interfaces;
using WarehouseShared;

namespace WarehouseApi.Controllers;

[ApiController]
[Route("api/movements")]
[Authorize]
public class StockMovementsController(IStockMovementService svc) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<StockMovementDto>>> GetAll()
    {
        var list = await svc.GetAllAsync();
        return Ok(list);
    }
}