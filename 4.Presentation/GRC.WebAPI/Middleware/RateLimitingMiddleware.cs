using Microsoft.Extensions.Caching.Memory;

namespace GRC.WebAPI.Middleware;

public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMemoryCache _cache;
    private readonly ILogger<RateLimitingMiddleware> _logger;

    public RateLimitingMiddleware(RequestDelegate next, IMemoryCache cache, ILogger<RateLimitingMiddleware> logger)
    {
        _next = next;
        _cache = cache;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var ipAddress = context.Connection.RemoteIpAddress?.ToString();
        var cacheKey = $\"rate_limit_{ipAddress}";

        if (!string.IsNullOrEmpty(ipAddress))
        {
            var requestCount = _cache.GetOrCreate(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return 0;
            });

            if (requestCount >= 100) // 100 request per minute
            {
                _logger.LogWarning("Rate limit exceeded for IP: {IPAddress}", ipAddress);
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsync("Rate limit exceeded. Please try again later.");
                return;
            }

            _cache.Set(cacheKey, requestCount + 1, TimeSpan.FromMinutes(1));
        }

        await _next(context);
    }
}
