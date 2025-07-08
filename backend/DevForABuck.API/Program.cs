using Azure.Storage.Blobs;
using DevForABuck.Application.Interfaces;
using DevForABuck.Infrastructure.Services;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Services (Only the essentials)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Register Azure Services (No fancy config)
builder.Services.AddSingleton(_ => new CosmosClient(builder.Configuration["CosmosDb:ConnectionString"]));
builder.Services.AddSingleton(_ => new BlobServiceClient(builder.Configuration["BlobStorage:ConnectionString"]));

// ✅ 3️⃣ DI for your services
builder.Services.AddScoped<IBookingService, BookingService>();


var app = builder.Build();

// 4. Middleware Pipeline (Lean setup)
app.UseSwagger();
app.UseSwaggerUI(); // No customization needed
app.UseHttpsRedirection();
app.MapControllers();

app.Run();