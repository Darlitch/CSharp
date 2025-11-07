using Contract.Repositories;
using Contract.Services;
using Data;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;
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
                services.AddSingleton<IObserver, Observer>();
                
                services.AddScoped<ISimulationRunRepository, SimulationRunRepository>();
                services.AddScoped<IForkEventRepository, ForkEventRepository>();
                services.AddScoped<IPhilosopherEventRepository, PhilosopherEventRepository>();

                services.AddHostedService(sp => ActivatorUtilities.CreateInstance<PhilosopherHostedService>(sp, 1, "Платон"));
                services.AddHostedService(sp => ActivatorUtilities.CreateInstance<PhilosopherHostedService>(sp, 2, "Аристотель"));
                services.AddHostedService(sp => ActivatorUtilities.CreateInstance<PhilosopherHostedService>(sp, 3, "Декарт"));
                services.AddHostedService(sp => ActivatorUtilities.CreateInstance<PhilosopherHostedService>(sp, 4, "Кант"));
                services.AddHostedService(sp => ActivatorUtilities.CreateInstance<PhilosopherHostedService>(sp, 5, "Сократ"));
                
                services.Configure<SimulationOptions>(context.Configuration.GetSection("Simulation"));
            });
        var host = builder.Build();
        var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
        var startedSource = new TaskCompletionSource();
        lifetime.ApplicationStarted.Register(() =>
        {
            startedSource.TrySetResult();
        });
        using (var scope = host.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<DataBaseContext>();
            await context.Database.MigrateAsync(); 
        }
        await host.StartAsync();
        await startedSource.Task;
        await host.Services.GetRequiredService<IObserver>().RecordSimulationRun();
        host.Services.GetRequiredService<ISimulation>().Run();
        await host.StopAsync();
    }
}