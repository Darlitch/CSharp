using IServices;
using Microsoft.Extensions.Options;
using StrategyInterface;

namespace Services.Philosophers;

public class Kant(IPhilosopherStrategy strategy, ITableManager tableManager, IOptions<SimulationOptions> options)
    : PhilosopherHostedService(strategy, tableManager, 3, "Кант", options);