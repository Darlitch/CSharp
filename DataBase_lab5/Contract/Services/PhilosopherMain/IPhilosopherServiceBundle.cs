using Contract.Services.Event;
using Contract.Services.Simulation;
using StrategyInterface;

namespace Contract.Services.PhilosopherMain;

public interface IPhilosopherServiceBundle
{
    IPhilosopherStrategy Strategy { get; }
    ITableManager TableManager { get; }
    IEventQueue EventQueue { get; }
    ISimulationTime SimulationTime { get; }
}