using DevForABuck.Application.Interfaces;
using DevForABuck.Infrastructure.Services;
using Microsoft.Azure.Cosmos;
using Azure.Storage.Blobs;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cosmos
builder.Services.AddSingleton(s =>
{
    var config = s.GetRequiredService<IConfiguration>();
    return new CosmosClient(config["CosmosDb:Account"], config["CosmosDb:Key"]);
});

// Blob
builder.Services.AddSingleton(s =>
{
    var config = s.GetRequiredService<IConfiguration>();
    return new BlobServiceClient(config["BlobStorage:ConnectionString"]);
});

// BookingService
builder.Services.AddScoped<IBookingService, BookingService>();

var app = builder.Build();

// Swagger always on in dev
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();