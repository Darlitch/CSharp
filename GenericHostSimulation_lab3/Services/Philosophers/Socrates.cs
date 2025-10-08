using IServices;
using Microsoft.Extensions.Options;
using StrategyInterface;

namespace Services.Philosophers;

public class Socrates(IPhilosopherStrategy strategy, ITableManager tableManager, IOptions<SimulationOptions> options)
    : PhilosopherHostedService(strategy, tableManager, 5, "Сократ", options);