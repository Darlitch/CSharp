using IServices;
using Microsoft.Extensions.Options;
using StrategyInterface;

namespace Services.Philosophers;

public class Aristotle(IPhilosopherStrategy strategy, ITableManager tableManager, IOptions<SimulationOptions> options)
    : PhilosopherHostedService(strategy, tableManager, 1, "Аристотель", options);