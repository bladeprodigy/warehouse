using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseApi.Interfaces;
using WarehouseShared;

namespace WarehouseApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LocationsController(ILocationService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<LocationDtos.LocationDto>>> GetAll()
    {
        var locations = await service.GetAllAsync();
        return Ok(locations);
    }
}