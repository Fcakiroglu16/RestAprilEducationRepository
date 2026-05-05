using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestAprilEducationRepository.API.Authorization;
using RestAprilEducationRepository.API.Endpoints.ExceptionHandlerExamples;
using RestAprilEducationRepository.API.Endpoints.Metrics;
using RestAprilEducationRepository.API.Endpoints.Products;
using RestAprilEducationRepository.API.Endpoints.Users;
using RestAprilEducationRepository.API.Endpoints.Versioning;
using RestAprilEducationRepository.API.ExceptionsHandlers;
using RestAprilEducationRepository.API.Extensions;
using RestAprilEducationRepository.API.Metrics;
using RestAprilEducationRepository.Application;
using RestAprilEducationRepository.Application.Products;
using RestAprilEducationRepository.Application.Products.Create;
using RestAprilEducationRepository.Domain.Exceptions;
using RestAprilEducationRepository.Persistence;
using Scalar.AspNetCore;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("sms-policy", context =>
    {
        var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "anonymous";

        var userId = context.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)!.Value!;

        var tenantId = context.Request.Headers.First(x => x.Key == "tenantId").Value.First();
        return RateLimitPartition.GetFixedWindowLimiter(ipAddress, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 30,
            Window = TimeSpan.FromMinutes(1),
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 5
        });


        return RateLimitPartition.GetFixedWindowLimiter(userId, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 30,
            Window = TimeSpan.FromMinutes(1),
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 5
        });

        return RateLimitPartition.GetFixedWindowLimiter(tenantId, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 30,
            Window = TimeSpan.FromMinutes(1),
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 5
        });
    });
    options.AddFixedWindowLimiter("fixed-windows-limiter", options =>
    {
        options.PermitLimit = 10;
        options.Window = TimeSpan.FromMinutes(1);
        options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 2;
    });


    options.AddSlidingWindowLimiter("sliding-windows-limiter", options =>
    {
        options.PermitLimit = 15;
        options.Window = TimeSpan.FromMinutes(1);
        options.SegmentsPerWindow = 6; // Divide window into 6 segments (10 seconds each)
        options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 3;
    });

    options.AddTokenBucketLimiter("token-bucket", options =>
    {
        options.TokenLimit = 20;
        options.TokensPerPeriod = 5;
        options.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
        options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 5;
        options.AutoReplenishment = true;
    });


    options.AddConcurrencyLimiter("concurrency-limit", options =>
    {
        options.PermitLimit = 5;
        options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 0;
    });
});


builder.AddServiceDefaults();


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();


// Transient > Scoped > Singleton
builder.Services.AddSingleton<ICalculateService, CalculateService>();
builder.Services.AddScoped<IProductsApplication, ProductsApplication>();
builder.Services.AddScoped<UserApplication>();
builder.Services.AddPersistenceExt(builder.Configuration);


builder.Services.AddValidatorsFromAssemblyContaining<ApplicationAssembly>();
builder.Services.AddSingleton<AppMetrics>();
builder.Services.AddVersioningExt();


builder.Services.AddExceptionHandler<UserFriendlyExceptionHandler>().AddExceptionHandler<BusinessExceptionHandler>()
    .AddExceptionHandler<GlobalExceptionHandler>();

var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(congiure =>
    {
        congiure.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        congiure.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSection["SecretKey"]!))
        };
    }).AddJwtBearer("branch-schema", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSection["SecretKey"]!))
        };
    });


builder.Services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();

builder.Services.AddAuthorization(options =>
{
    //role-based authorization
    options.AddPolicy("editor-role-policy", configurePolicy =>
    {
        configurePolicy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);

        configurePolicy.RequireRole("editor");
    });

    options.AddPolicy("city-policy", configurePolicy =>
    {
        configurePolicy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);

        configurePolicy.RequireClaim("city", "istanbul");
    });


    options.AddPolicy("branch-policy",
        configurePolicy =>
        {
            configurePolicy.AuthenticationSchemes.Add("branch-schema");
            configurePolicy.RequireClaim("branch-id");
        });

    options.AddPolicy("min-age-policy", configurePolicy =>
    {
        configurePolicy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        configurePolicy.AddRequirements(new MinimumAgeRequirement(minimumAge: 18));
    });
});


var app = builder.Build();

app.MapDefaultEndpoints();

app.UseExceptionHandler(options => { });

app.UseAuthentication();
app.UseAuthorization();

var apiVersionSet = app.AddVersionSetExt();
app.AddProductEndpoints(apiVersionSet);
app.AddVersionExampleEndpoints(apiVersionSet);

app.AddExceptionHandlerExampleEndpoint();
app.AddMetricsEndpoints();
app.AddUserEndpoints();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();