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
    return new CosmosClient(config["CosmosDb:Account"], config["CosmosDb:Key"]);
});

builder.Services.AddSingleton(s =>
{
    var config = s.GetRequiredService<IConfiguration>();
    return new BlobServiceClient(config["BlobStorage:ConnectionString"]);
});

// Cors policty
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        b => b.WithOrigins(
                "https://dev.devforbuck.com", // ✅ your static app URL
                "https://www.devforbuck.com") // ✅ prod if needed
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

// ✅ Common middlewares
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
