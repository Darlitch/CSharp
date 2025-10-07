using IServices;
using StrategyInterface;

namespace Services.Philosophers;

public class Platoo(IPhilosopherStrategy strategy, ITableManager tableManager, SimulationOptions options)
    : PhilosopherHostedService(strategy, tableManager, 4, "Платон", options);