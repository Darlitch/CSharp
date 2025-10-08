using IServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;
using Services.Philosophers;
using Strategy;
using StrategyInterface;

namespace GenericHostSimulation_lab3;

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

                services.AddHostedService<Platoo>();
                services.AddHostedService<Aristotle>();
                services.AddHostedService<Decartes>();
                services.AddHostedService<Kant>();
                services.AddHostedService<Socrates>();
                
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