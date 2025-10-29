using Model;
using Model.Enums;

namespace IServices;

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