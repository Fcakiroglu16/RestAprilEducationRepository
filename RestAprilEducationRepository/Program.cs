using FluentValidation;
using RestAprilEducationRepository.API.Endpoints.ExceptionHandlerExamples;
using RestAprilEducationRepository.API.Endpoints.Metrics;
using RestAprilEducationRepository.API.Endpoints.Products;
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

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//DI Container Framework ( Library ) IoC Container Framework

//  DI+ IoC =>  DI Pattern

//Singleton
//Scoped
//Transient
//builder.Services.AddSingleton<CalculateService>();

// Transient > Scoped > Singleton
builder.Services.AddSingleton<ICalculateService, CalculateService>();
builder.Services.AddScoped<IProductsApplication, ProductsApplication>();
builder.Services.AddRepositoriesExt();




builder.Services.AddValidatorsFromAssemblyContaining<ApplicationAssembly>();
builder.Services.AddSingleton<AppMetrics>();
builder.Services.AddVersioningExt();


builder.Services.AddExceptionHandler<UserFriendlyExceptionHandler>().AddExceptionHandler<BusinessExceptionHandler>()
    .AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseExceptionHandler(options => { });

var apiVersionSet = app.AddVersionSetExt();
app.AddProductEndpoints(apiVersionSet);
app.AddVersionExampleEndpoints(apiVersionSet);

app.AddExceptionHandlerExampleEndpoint();
app.AddMetricsEndpoints();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();