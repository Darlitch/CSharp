using Contract.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;
using Services.Philosophers;
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
                services.AddSingleton<ITableManager, TableManager>();
                services.AddSingleton<IPhilosopherStrategy, NaivePhilosopherStrategy>();
                services.AddSingleton<IMetricsCollector, MetricsCollector>();
                services.AddSingleton<ISimulation, Simulation>();
                services.AddSingleton<ISimulationTime, SimulationTime>();

                services.AddHostedService(sp => ActivatorUtilities.CreateInstance<Platoo>(sp, 1));
                services.AddHostedService(sp => ActivatorUtilities.CreateInstance<Aristotle>(sp, 2));
                services.AddHostedService(sp => ActivatorUtilities.CreateInstance<Decartes>(sp, 3));
                services.AddHostedService(sp => ActivatorUtilities.CreateInstance<Kant>(sp, 4));
                services.AddHostedService(sp => ActivatorUtilities.CreateInstance<Socrates>(sp, 5));
                
                services.Configure<SimulationOptions>(context.Configuration.GetSection("Simulation"));
            });
        var host = builder.Build();
        var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
        var startedSource = new TaskCompletionSource();
        lifetime.ApplicationStarted.Register(() =>
        {
            startedSource.TrySetResult();
        });
        
        await host.StartAsync();
        await startedSource.Task;
        
        host.Services.GetRequiredService<ISimulation>().Run();
        await host.StopAsync();
    }
}