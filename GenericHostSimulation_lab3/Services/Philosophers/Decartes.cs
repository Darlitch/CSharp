using IServices;
using StrategyInterface;

namespace Services.Philosophers;

public class Decartes(IPhilosopherStrategy strategy, ITableManager tableManager, SimulationOptions options)
    : PhilosopherHostedService(strategy, tableManager, 2, "Декарт", options);