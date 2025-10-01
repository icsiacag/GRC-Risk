using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GRC.Infrastructure.Identity.Services;

public class UserSessionService
{
    private readonly IdentityDbContext _context;
    private readonly ILogger<UserSessionService> _logger;

    public UserSessionService(IdentityDbContext context, ILogger<UserSessionService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task TrackUserSessionAsync(string userId, string ipAddress, string userAgent)
    {
        var session = new UserSession
        {
            UserId = userId,
            LoginTime = DateTime.UtcNow,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            IsActive = true
        };

        _context.UserSessions.Add(session);
        await _context.SaveChangesAsync();
    }

    public async Task EndUserSessionAsync(string sessionId)
    {
        var session = await _context.UserSessions.FindAsync(sessionId);
        if (session != null)
        {
            session.IsActive = false;
            session.LogoutTime = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task CleanupExpiredSessionsAsync()
    {
        var expiredSessions = await _context.UserSessions
            .Where(s => s.IsActive && s.LoginTime < DateTime.UtcNow.AddDays(-30))
            .ToListAsync();

        foreach (var session in expiredSessions)
        {
            session.IsActive = false;
            session.LogoutTime = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Cleaned up {Count} expired sessions", expiredSessions.Count);
    }
}
