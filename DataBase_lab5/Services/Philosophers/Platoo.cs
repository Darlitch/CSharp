using Contract.Services;
using Microsoft.Extensions.Options;
using StrategyInterface;

namespace Services.Philosophers;

public class Platoo(IPhilosopherStrategy strategy, ITableManager tableManager, IOptions<SimulationOptions> options, int index)
    : PhilosopherHostedService(strategy, tableManager, options, index, "Платон");