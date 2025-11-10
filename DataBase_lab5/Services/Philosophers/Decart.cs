using Contract.Services;
using Microsoft.Extensions.Options;
using StrategyInterface;

namespace Services.Philosophers;

public class Decart(
    IPhilosopherStrategy strategy,
    ITableManager tableManager,
    IOptions<SimulationOptions> options,
    IEventQueue eventQueue,
    ISimulationTime simulationTime,
    int ind) : PhilosopherHostedService(strategy, tableManager, options, eventQueue, simulationTime, ind, "Декарт");