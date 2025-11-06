using Contract.Services;
using Microsoft.Extensions.Options;
using StrategyInterface;

namespace Services.Philosophers;

public class Decartes(IPhilosopherStrategy strategy, ITableManager tableManager, IOptions<SimulationOptions> options, int index)
    : PhilosopherHostedService(strategy, tableManager, options, index, "Декарт");