using Contract.Repositories;
using Contract.Services;
using Contract.Services.Event;
using Contract.Services.PhilosopherMain;
using Contract.Services.Simulation;
using Data;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;
using Services.Event;
using Services.PhilosopherMain;
using Services.Philosophers;
using Services.Simulation;
using Strategy;
using StrategyInterface;

namespace DataBase_lab5;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.json"), optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<DataBaseContext>(options =>
                    options.UseNpgsql(context.Configuration.GetConnectionString("DefaultConnection")));
                
                services.AddSingleton<ITableManager, TableManager>();
                services.AddSingleton<IPhilosopherStrategy, NaivePhilosopherStrategy>();
                services.AddSingleton<IMetricsCollector, MetricsCollector>();
                services.AddSingleton<ISimulation, Simulation>();
                services.AddSingleton<ISimulationTime, SimulationTime>();
                services.AddSingleton<IEventQueue, EventQueue>();
                services.AddSingleton<IPhilosopherServiceBundle, PhilosopherServiceBundle>();
                
                services.AddScoped<IRecordManager, RecordManager>();
                services.AddScoped<ISimulationRunRepository, SimulationRunRepository>();
                services.AddScoped<IForkEventRepository, ForkEventRepository>();
                services.AddScoped<IPhilosopherEventRepository, PhilosopherEventRepository>();

                services.AddHostedService(sp => ActivatorUtilities.CreateInstance<Platoo>(sp, 1));
                services.AddHostedService(sp => ActivatorUtilities.CreateInstance<Aristotle>(sp, 2));
                services.AddHostedService(sp => ActivatorUtilities.CreateInstance<Decart>(sp, 3));
                services.AddHostedService(sp => ActivatorUtilities.CreateInstance<Kant>(sp, 4));
                services.AddHostedService(sp => ActivatorUtilities.CreateInstance<Socrates>(sp, 5));
                services.AddHostedService(sp => ActivatorUtilities.CreateInstance<EventProcessorHostedService>(sp));
                
                services.Configure<SimulationOptions>(context.Configuration.GetSection("Simulation"));
            });
        var host = builder.Build();
        using (var scope = host.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<DataBaseContext>();
            await context.Database.MigrateAsync(); 
        }
        await host.StartAsync();
        // await host.Services.GetRequiredService<IRecordManager>().RecordSimulationRun();
        host.Services.GetRequiredService<ISimulation>().Run();
        await host.StopAsync();
    }
}