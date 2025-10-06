using IServices;
using StrategyInterface;

namespace Services.Philosophers;

public class Socrates(IPhilosopherStrategy strategy, ITableManager tableManager)
    : PhilosopherHostedService(strategy, tableManager, 5, "Сократ");