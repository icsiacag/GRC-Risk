using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GRC.WebAPI.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        // Kullanıcı listesi endpoint'i
        return Ok(new { users = new[] { "user1", "user2" } });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        // Kullanıcı detay endpoint'i
        return Ok(new { user = new { Id = id, Name = "Test User" } });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserRequest request)
    {
        // Kullanıcı güncelleme endpoint'i
        return Ok(new { message = "User updated successfully" });
    }
}

public class UpdateUserRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
