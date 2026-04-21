using FluentValidation;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using RestAprilEducationRepository.API.Endpoints.ExceptionHandlerExamples;
using RestAprilEducationRepository.API.Endpoints.Products;
using RestAprilEducationRepository.API.Endpoints.Versioning;
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

// OpenTelemetry — Traces, Metrics, Logs → SigNoz üzerinden ClickHouse'a gönderilir
var otlpEndpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"] ?? "http://signoz-otel-collector:4317";

builder.Services.AddOpenTelemetry()
    .ConfigureResource(r => r.AddService(
        serviceName: builder.Configuration["OTEL_SERVICE_NAME"] ?? "RestAprilEducationRepository.API",
        serviceVersion: "1.0.0"))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddOtlpExporter(o => o.Endpoint = new Uri(otlpEndpoint)))
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddOtlpExporter(o => o.Endpoint = new Uri(otlpEndpoint)));

builder.Logging.AddOpenTelemetry(logging =>
{
    logging.AddOtlpExporter(o => o.Endpoint = new Uri(otlpEndpoint));
});

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


builder.Services.AddExceptionHandler<UserFriendlyExceptionHandler>().AddExceptionHandler<BusinessExceptionHandler>()
    .AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseExceptionHandler(options => { });

var apiVersionSet = app.AddVersionSetExt();
app.AddProductEndpoints(apiVersionSet);
app.AddVersionExampleEndpoints(apiVersionSet);

app.AddExceptionHandlerExampleEndpoint();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();