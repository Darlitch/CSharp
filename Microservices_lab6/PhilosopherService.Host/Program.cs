using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhilosopherService.Application.Abstractions;
using PhilosopherService.Application.Configurations;
using PhilosopherService.Application.Services;
using PhilosopherService.Application.Workers;
using PhilosopherService.Infrastructure;
using PhilosopherService.Infrastructure.Request;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<PhilosopherOptions>(builder.Configuration.GetSection("PhilosopherOptions"));
builder.Services.Configure<RequestDomains>(builder.Configuration.GetSection("RequestDomains"));

builder.Services.AddSingleton<RequestFactory>();

builder.Services.AddSingleton<IStrategy, NaiveStrategy>();

builder.Services.AddHttpClient<ITableServiceClient, TableServiceClient>();

builder.Services.AddHostedService<PhilosopherWorker>();

var app = builder.Build();
app.Run();