using IServices;
using StrategyInterface;

namespace Services.Philosophers;

public class Aristotle(IPhilosopherStrategy strategy, ITableManager tableManager, SimulationOptions options)
    : PhilosopherHostedService(strategy, tableManager, 1, "Аристотель", options);