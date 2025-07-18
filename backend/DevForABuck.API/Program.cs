using DevForABuck.Application.Interfaces;
using DevForABuck.Infrastructure.Services;
using Microsoft.Azure.Cosmos;
using Azure.Storage.Blobs;
using DevForABuck.Application.Commands.CreateBooking;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// ✅ Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ DI for custom services
builder.Services.AddScoped<IBookingService, BookingService>();

builder.Services.AddSingleton(s =>
{
    var config = s.GetRequiredService<IConfiguration>();
    var account = config["CosmosDb:Account"];
    var key = config["CosmosDb:Key"];
    return new CosmosClient(account, key);
});

builder.Services.AddSingleton(s =>
{
    var config = s.GetRequiredService<IConfiguration>();
    var connectionString = config["BlobStorage:ConnectionString"] ?? throw new InvalidOperationException($"CosmosDb connection string not found");
    return new BlobServiceClient(connectionString);
});

builder.Services.AddMediatR(typeof(Program));
builder.Services.AddMediatR(typeof(CreateBookingCommandHandler).Assembly);

builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // This connects logs to App Service Log Stream!


// Cors policty
// Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", builder =>
    {
        builder.WithOrigins(
                "http://localhost:4200", // Local Angular dev
                "https://dev.devforabuck.com", // Dev environment
                "https://www.devforabuck.com") // Production
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetPreflightMaxAge(TimeSpan.FromSeconds(86400)); // 24 hours for preflight cache
    });
});

var app = builder.Build();

// Handle OPTIONS requests first
app.Use(async (context, next) =>
{
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.StatusCode = 204; // No Content
        await context.Response.CompleteAsync();
        return;
    }
    await next();
});

// ✅ Common middlewares
app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseAuthorization();

// ✅ Always enable Swagger in all envs
app.UseSwagger();
app.UseSwaggerUI();

// ✅ Short root redirect to Swagger
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

// ✅ API endpoints
app.MapControllers();

app.Run();
