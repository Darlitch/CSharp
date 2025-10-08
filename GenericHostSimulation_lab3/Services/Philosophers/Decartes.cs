using IServices;
using Microsoft.Extensions.Options;
using StrategyInterface;

namespace Services.Philosophers;

public class Decartes(IPhilosopherStrategy strategy, ITableManager tableManager, IOptions<SimulationOptions> options)
    : PhilosopherHostedService(strategy, tableManager, 2, "Декарт", options);