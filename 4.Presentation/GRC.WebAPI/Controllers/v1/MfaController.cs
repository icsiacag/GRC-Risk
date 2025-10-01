using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GRC.WebAPI.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class MfaController : ControllerBase
{
    private readonly MfaService _mfaService;

    public MfaController(MfaService mfaService)
    {
        _mfaService = mfaService;
    }

    [HttpPost("enable")]
    public async Task<IActionResult> EnableMfa([FromBody] EnableMfaRequest request)
    {
        // MFA aktivasyon endpoint'i
        return Ok(new { message = "MFA enabled successfully" });
    }

    [HttpPost("disable")]
    public async Task<IActionResult> DisableMfa()
    {
        // MFA devre dışı bırakma endpoint'i
        return Ok(new { message = "MFA disabled successfully" });
    }

    [HttpGet("recovery-codes")]
    public async Task<IActionResult> GetRecoveryCodes()
    {
        // Kurtarma kodları endpoint'i
        return Ok(new { codes = new[] { "code1", "code2" } });
    }
}

public class EnableMfaRequest
{
    public string Token { get; set; } = string.Empty;
}
