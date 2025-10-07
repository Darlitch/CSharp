using IServices;
using StrategyInterface;

namespace Services.Philosophers;

public class Socrates(IPhilosopherStrategy strategy, ITableManager tableManager, SimulationOptions options)
    : PhilosopherHostedService(strategy, tableManager, 5, "Сократ", options);