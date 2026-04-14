using FluentValidation;
using RestAprilEducationRepository.API.Endpoints.Products;
using RestAprilEducationRepository.API.Extensions;
using RestAprilEducationRepository.Application;
using RestAprilEducationRepository.Application.Products;
using RestAprilEducationRepository.Application.Products.Create;
using RestAprilEducationRepository.Persistence;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddVersioningExt();
var app = builder.Build();


app.AddProductEndpoints(app.AddVersionSetExt());
app.AddVersionSetExt();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();