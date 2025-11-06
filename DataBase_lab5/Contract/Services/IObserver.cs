using Model.Enums;

namespace Contract.Services;

public interface IObserver
{
    public bool ReadyToStart { get; }
    public Task RecordSimulationRun();
    public Task RecordPhilosopherEvent(int index, string name, PhilosopherState state, PhilosopherAction action,
        int eaten, long waitingTime);

    public Task RecordForkEvent(int index, string? owner, ForkState state);
}