using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Text;

namespace GRC.Infrastructure.Identity.Services;

public class MfaService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<MfaService> _logger;

    public MfaService(UserManager<ApplicationUser> userManager, ILogger<MfaService> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<bool> EnableMfaAsync(ApplicationUser user, string token)
    {
        // MFA aktivasyonu
        return await _userManager.SetTwoFactorEnabledAsync(user, true);
    }

    public async Task<bool> DisableMfaAsync(ApplicationUser user)
    {
        // MFA devre dışı bırakma
        return await _userManager.SetTwoFactorEnabledAsync(user, false);
    }

    public async Task<string> GenerateMfaRecoveryCodesAsync(ApplicationUser user)
    {
        // Kurtarma kodları oluştur
        var codes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
        return string.Join(",", codes);
    }
}
