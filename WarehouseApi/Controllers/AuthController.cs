using Microsoft.AspNetCore.Mvc;
using WarehouseApi.Interfaces;
using WarehouseShared;

namespace WarehouseApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        var token = await authService.AuthenticateAsync(req.Username, req.Password);
        if (token == null)
            return Unauthorized(new LoginResponse { Message = "Invalid credentials" });

        return Ok(new LoginResponse { Token = token, Message = "Login successful" });
    }
}