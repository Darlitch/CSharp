using System.CommandLine;
using System.Globalization;
using Contract.Repositories;
using Data;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using View.Contract;
using View.Services;

namespace View;

public class Program
{
    public static async Task Main(string[] args)
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        var runIdOption = new Option<long>("--runId")
        {
            Description = "Id симуляции.",
            Required = true
        };
        var delayOption = new Option<double>("--delay")
        {
            Description = "Смещение в секундах относительно начала симуляции.",
            Required = true
        };
        var rootCommand = new RootCommand("Просмотр состояния стола в симуляции обедающих философов.");
        rootCommand.Options.Add(runIdOption);
        rootCommand.Options.Add(delayOption);
    
        rootCommand.SetAction(async (ParseResult parseResult, CancellationToken ct) =>
        {
            var runId = parseResult.GetValue(runIdOption);
            var delay = parseResult.GetValue(delayOption);
            await RunCommandAsync(runId, delay, ct);
        });
        var parseResult = rootCommand.Parse(args);
        await parseResult.InvokeAsync();
    }
    private static async Task RunCommandAsync(long runId, double delay, CancellationToken ct = default)
    {
        var builder = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.json"), optional: false,
                    reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<DataBaseContext>(options =>
                    options.UseNpgsql(context.Configuration.GetConnectionString("DefaultConnection")));

                services.AddScoped<IViewManager, ViewManager>();
                services.AddScoped<ISimulationRunRepository, SimulationRunRepository>();
                services.AddScoped<IForkEventRepository, ForkEventRepository>();
                services.AddScoped<IPhilosopherEventRepository, PhilosopherEventRepository>();
                services.AddScoped<IViewStateSimulation, ViewStateService>();
            }).Build();
        using var scope = builder.Services.CreateScope();
        await scope.ServiceProvider.GetRequiredService<IViewStateSimulation>().Run(runId, delay);
    }
    
}