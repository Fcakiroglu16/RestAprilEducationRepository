using RestAprilEducationRepository.API.Endpoints.Products;
using RestAprilEducationRepository.Application.Products;
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
builder.Services.AddScoped<IProductRepository, ProductRepositoryWithInMemory>();


var app = builder.Build();


app.AddProductEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();