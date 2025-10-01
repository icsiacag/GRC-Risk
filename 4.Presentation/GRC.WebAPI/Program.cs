using GRC.Infrastructure.Identity;
using GRC.Infrastructure.Data;
using GRC.Audit;
using GRC.WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Infrastructure Services
builder.Services.AddInfrastructureIdentity(builder.Configuration);
builder.Services.AddInfrastructureData(builder.Configuration);
builder.Services.AddAuditServices(builder.Configuration);

// Add Background Jobs
builder.Services.AddHostedService<SessionCleanupJob>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

// Custom Middleware
app.UseMiddleware<RateLimitingMiddleware>();
app.UseMiddleware<SecurityHeadersMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
