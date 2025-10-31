using IServices;
using Microsoft.Extensions.Options;
using StrategyInterface;

namespace Services.Philosophers;

public class Socrates(IPhilosopherStrategy strategy, ITableManager tableManager, IOptions<SimulationOptions> options, int index)
    : PhilosopherHostedService(strategy, tableManager, options, index, "Сократ");