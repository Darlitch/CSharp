
using Application.Abstractions;
using Application.Configurations;
using Application.Services;
using Application.Workers;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHealthChecks();

builder.Services.Configure<TableOptions>(builder.Configuration.GetSection("Options"));

builder.Services.AddSingleton<ISimulationTime, SimulationTime>();
builder.Services.AddSingleton<IMetricReporter, MetricReporter>();
builder.Services.AddSingleton<ITableManager, TableManager>();

builder.Services.AddHostedService<SimulationWorker>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TableService API",
        Version = "v1",
        Description = "API для работы с TableService"
    }));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TableService API v1");
    c.RoutePrefix = "swagger";
});

app.MapControllers();
app.Run();
