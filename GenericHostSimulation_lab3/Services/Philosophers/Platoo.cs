using IServices;
using Microsoft.Extensions.Options;
using StrategyInterface;

namespace Services.Philosophers;

public class Platoo(IPhilosopherStrategy strategy, ITableManager tableManager, IOptions<SimulationOptions> options)
    : PhilosopherHostedService(strategy, tableManager, 4, "Платон", options);