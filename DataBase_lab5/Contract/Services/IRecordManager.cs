using Model.DTO;

namespace Contract.Services;

public interface IRecordManager
{
    public bool ReadyToStart { get; }
    public Task RecordSimulationRun();
    public Task UpdateSimulationRun(SimulationRunDto dto);
    public Task RecordPhilosopherEvent(PhilosopherEventDto dto);
    public Task RecordForkEvent(ForkEventDto dto);
}