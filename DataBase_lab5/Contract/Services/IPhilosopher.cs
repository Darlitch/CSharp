using Model;
using Model.Enums;

namespace Contract.Services;

public interface IPhilosopher
{
    int Index { get; }
    string Name { get; }
    PhilosopherMetrics Metrics { get; }
    
    int CurrentActionDuration { get; }
    PhilosopherState State { get; }
    PhilosopherAction Action { get; }
    bool IsHungry { get; }
}