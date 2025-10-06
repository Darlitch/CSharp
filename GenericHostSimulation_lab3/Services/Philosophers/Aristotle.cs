using IServices;
using StrategyInterface;

namespace Services.Philosophers;

public class Aristotle(IPhilosopherStrategy strategy, ITableManager tableManager)
    : PhilosopherHostedService(strategy, tableManager, 1, "Аристотель");