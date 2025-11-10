using Model.DTO;

namespace Contract.Services;

public interface IManager
{
    public bool ReadyToStart { get; }
    public Task RecordSimulationRun();
    public Task RecordPhilosopherEvent(PhilosopherEventDto dto);
    public Task RecordForkEvent(ForkEventDto dto);
}