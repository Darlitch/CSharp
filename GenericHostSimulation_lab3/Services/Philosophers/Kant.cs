using IServices;
using StrategyInterface;

namespace Services.Philosophers;

public class Kant(IPhilosopherStrategy strategy, ITableManager tableManager)
    : PhilosopherHostedService(strategy, tableManager, 3, "Кант");