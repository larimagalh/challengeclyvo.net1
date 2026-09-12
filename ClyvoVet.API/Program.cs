using Microsoft.EntityFrameworkCore;
using ClyvoVet.API.Data;
using ClyvoVet.API.Infraestrutura.Health;
using ClyvoVet.API.Aplicacao.Middlewares;
using ClyvoVet.API.Infraestrutura.Observabilidade;
using Serilog;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File("logs/app-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(
        builder.Configuration.GetConnectionString("OracleConnection")));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks()
    .AddCheck<BancoDadosHealthCheck>("banco_dados") ;

builder.Services.AddOpenTelemetry()
.ConfigureResource(resource => resource.AddService(AplicacaoMetricas.NomeServico))
.WithTracing(tracing =>
{
    tracing
.AddAspNetCoreInstrumentation()
.AddHttpClientInstrumentation()
.AddSource(AplicacaoMetricas.NomeServico)
.AddConsoleExporter();
})
.WithMetrics(metrics =>
{
    metrics
    .AddAspNetCoreInstrumentation()
    .AddMeter(AplicacaoMetricas.NomeServico)
    .AddConsoleExporter();
});


var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();


app.UseSerilogRequestLogging();

app.UseMiddleware<CorrelationIdMiddleware>();

app.MapHealthChecks("/health");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();