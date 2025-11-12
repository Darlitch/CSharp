using Contract.Services;
using Contract.Services.Event;
using Contract.Services.PhilosopherMain;
using Contract.Services.Simulation;
using Microsoft.Extensions.Options;
using Services.Simulation;
using StrategyInterface;

namespace Services.PhilosopherMain;

public class PhilosopherServiceBundle(IPhilosopherStrategy strategy,
    ITableManager tableManager,
    IEventQueue eventQueue,
    ISimulationTime simulationTime) : IPhilosopherServiceBundle
{
    public IPhilosopherStrategy Strategy { get; } = strategy;
    public ITableManager TableManager { get; } = tableManager;
    public IEventQueue EventQueue { get; } = eventQueue;
    public ISimulationTime SimulationTime { get; } = simulationTime;
}