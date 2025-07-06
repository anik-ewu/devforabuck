using DevForABuck.Application.Interfaces;
using DevForABuck.Infrastructure.Services;
using Microsoft.Azure.Cosmos;
using Azure.Storage.Blobs;

var builder = WebApplication.CreateBuilder(args);

// ✅ Add Controllers
builder.Services.AddControllers();

// ✅ Add Swagger (always on in dev)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ Add CosmosClient singleton
builder.Services.AddSingleton(s =>
{
    var config = s.GetRequiredService<IConfiguration>();
    var account = config["CosmosDb:Account"];
    var key = config["CosmosDb:Key"];
    return new CosmosClient(account, key);
});

// ✅ Add BlobServiceClient singleton
builder.Services.AddSingleton(s =>
{
    var config = s.GetRequiredService<IConfiguration>();
    var connStr = config["BlobStorage:ConnectionString"];
    return new BlobServiceClient(connStr);
});

// ✅ Register your BookingService
builder.Services.AddScoped<IBookingService, BookingService>();

var app = builder.Build();

// ✅ Use Swagger
app.UseSwagger();
app.UseSwaggerUI();

// ✅ Add HTTPS & routing
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();