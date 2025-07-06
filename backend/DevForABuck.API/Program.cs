using DevForABuck.Application.Interfaces;
using DevForABuck.Infrastructure.Services;
using Microsoft.Azure.Cosmos;
using Azure.Storage.Blobs;

var builder = WebApplication.CreateBuilder(args);

// ✅ 1️⃣ Add controllers
builder.Services.AddControllers();

// ✅ 2️⃣ Add Swagger and set version properly
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "DevForABuck API",
        Description = "API for booking resume reviews and career counseling."
    });
});

// ✅ 3️⃣ DI for your services
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

var app = builder.Build();

// ✅ 4️⃣ Use Swagger UI — make sure this is INSIDE the pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "DevForABuck API v1");
        options.RoutePrefix = string.Empty; // So it shows at root if you want
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();