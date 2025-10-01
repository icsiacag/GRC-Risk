using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using GRC.Audit.Entities;

namespace GRC.Audit.Services;

public class SecurityAuditService
{
    private readonly AuditDbContext _context;
    private readonly ILogger<SecurityAuditService> _logger;

    public SecurityAuditService(AuditDbContext context, ILogger<SecurityAuditService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task LogSecurityEventAsync(string userId, string action, string resource, 
        string ipAddress, string userAgent, bool success, string? details = null)
    {
        var auditLog = new SecurityAuditLog
        {
            UserId = userId,
            Action = action,
            Resource = resource,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            Success = success,
            Details = details
        };

        _context.SecurityAuditLogs.Add(auditLog);
        await _context.SaveChangesAsync();
    }

    public async Task<List<SecurityAuditLog>> GetAuditLogsAsync(string userId, DateTime? fromDate = null, DateTime? toDate = null)
    {
        var query = _context.SecurityAuditLogs.AsQueryable();

        if (!string.IsNullOrEmpty(userId))
            query = query.Where(log => log.UserId == userId);

        if (fromDate.HasValue)
            query = query.Where(log => log.Timestamp >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(log => log.Timestamp <= toDate.Value);

        return await query.OrderByDescending(log => log.Timestamp).ToListAsync();
    }
}
