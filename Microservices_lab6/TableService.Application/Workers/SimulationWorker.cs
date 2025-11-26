using Application.Abstractions;
using Application.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Application.Workers;

public class SimulationWorker(IOptions<TableServiceOptions> options, ITableManager tableManager,
    IMetricReporter metricReporter) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
}