
using CoordinatorService.Application.Abstractions;
using CoordinatorService.Application.Configurations;
using CoordinatorService.Application.Services;
using CoordinatorService.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<CoordinatorOptions>(builder.Configuration.GetSection("CoordinatorOptions"));

builder.Services.AddSingleton<ICoordinatorManager, CoordinatorManager>();

builder.Services.AddMassTransitConfigurator(builder.Configuration);

var host = builder.Build();
host.Run();