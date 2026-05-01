using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestAprilEducationRepository.API.Endpoints.ExceptionHandlerExamples;
using RestAprilEducationRepository.API.Endpoints.Metrics;
using RestAprilEducationRepository.API.Endpoints.Products;
using RestAprilEducationRepository.API.Endpoints.Users;
using RestAprilEducationRepository.API.Endpoints.Versioning;
using RestAprilEducationRepository.API.Metrics;
using RestAprilEducationRepository.API.ExceptionsHandlers;
using RestAprilEducationRepository.API.Extensions;
using RestAprilEducationRepository.Application;
using RestAprilEducationRepository.Application.Products;
using RestAprilEducationRepository.Application.Products.Create;
using RestAprilEducationRepository.Domain.Exceptions;
using RestAprilEducationRepository.Persistence;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

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