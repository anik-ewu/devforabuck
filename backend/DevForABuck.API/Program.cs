using DevForABuck.Application.Commands.CreateBooking;
using DevForABuck.Application.Interfaces;
using DevForABuck.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure.Cosmos;
using Microsoft.IdentityModel.Tokens;
using Azure.Storage.Blobs;


var builder = WebApplication.CreateBuilder(args);

// ✅ Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ DI for custom services
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<ISlotService, SlotService>();

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

// ✅ JWT Bearer Auth from config
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var config = builder.Configuration;
        var tenantId = config["Auth:TenantId"];
        var clientId = config["Auth:ClientId"];
        var authority = config["Auth:Authority"];

        options.Authority = $"{authority}/{tenantId}/v2.0/";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"{authority}/{tenantId}/v2.0/",
            ValidateAudience = true,
            // Azure AD can issue tokens with either the bare ClientId or an
            // `api://{clientId}` prefix as the audience. Accept both formats so
            // locally issued tokens and those requested with a custom scope are
            // considered valid.
            ValidAudiences = new[] { $"api://{clientId}", clientId },
            ValidateLifetime = true,
            RoleClaimType = "roles"
        };
    });

builder.Services.AddAuthorization();



// Cors policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",
                "https://dev.devforabuck.com",
                "https://www.devforabuck.com")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


var app = builder.Build();

// ✅ Common middlewares
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    if (context.User.Identity?.IsAuthenticated == false)
    {
        logger.LogWarning("Unauthenticated request to: {Path}", context.Request.Path);
    }
    await next();
});

app.UseRouting();
app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseAuthentication();
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
