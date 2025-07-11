using DevForABuck.Application.Interfaces;
using DevForABuck.Infrastructure.Services;
using Microsoft.Azure.Cosmos;
using Azure.Storage.Blobs;

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
    var account = config["CosmosDb:Account"] ?? throw new InvalidOperationException($"CosmosDb account not found");
    var key = config["CosmosDb:Key"] ?? throw new InvalidOperationException($"CosmosDb key not found");
    return new CosmosClient(account, key);
});

builder.Services.AddSingleton(s =>
{
    var config = s.GetRequiredService<IConfiguration>();
    var connectionString = config["BlobStorage:ConnectionString"] ?? throw new InvalidOperationException($"CosmosDb connection string not found");
    return new BlobServiceClient(connectionString);
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // This connects logs to App Service Log Stream!


// Cors policty
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", builder =>
    {
        builder.WithOrigins(
                "http://localhost:4200", // Local Angular dev
                "https://dev.devforbuck.com", // Dev environment
                "https://www.devforbuck.com") // Production
            .AllowAnyHeader()
            .AllowAnyMethod();
        // TODO: add later
        // .AllowCredentials();                  // Add this if using cookies/auth
    });
});

var app = builder.Build();

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
