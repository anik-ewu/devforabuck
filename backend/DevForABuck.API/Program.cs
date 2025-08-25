using DevForABuck.Application.Commands.CreateBooking;
using DevForABuck.Application.Interfaces;
using DevForABuck.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure.Cosmos;
using Microsoft.IdentityModel.Tokens;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using DevForABuck.API;


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

var authSection = builder.Configuration.GetSection("Auth");
builder.Services.Configure<AuthOptions>(authSection);
builder.Services.AddOptions<AuthOptions>()
    .ValidateDataAnnotations()
    .Validate(o => !string.IsNullOrWhiteSpace(o.TenantId), "Auth:TenantId is required")
    .Validate(o => !string.IsNullOrWhiteSpace(o.ClientId), "Auth:ClientId is required")
    .Validate(o => !string.IsNullOrWhiteSpace(o.Authority), "Auth:Authority is required")
    .ValidateOnStart();

// ✅ JWT Bearer Auth from strongly typed config
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();

builder.Services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
    .Configure<IOptions<AuthOptions>>((options, authOptions) =>
    {
        var auth = authOptions.Value;
        var issuer = $"{auth.Authority}/{auth.TenantId}/v2.0";
        options.Authority = issuer;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidIssuers = new[] { issuer, issuer + "/" },
            ValidateAudience = true,
            // Azure AD can issue tokens with either the bare ClientId or an
            // `api://{auth.ClientId}` prefix as the audience. Accept both formats so
            // locally issued tokens and those requested with a custom scope are
            // considered valid.
            ValidAudiences = new[] { $"api://{auth.ClientId}", auth.ClientId },
            ValidateLifetime = true,
            RoleClaimType = "roles"
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = ctx =>
            {
                var logger = ctx.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogError(ctx.Exception, "Authentication failed");
                ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            }
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
