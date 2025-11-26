using Model.DTO;

namespace Contract.Services;

public interface IRecordManager
{
    // bool ReadyToStart { get; }
    Task<long> RecordSimulationRun();
    Task UpdateSimulationRun(long runId, UpdateSimulationRunDto dto);
    Task RecordPhilosopherEvent(long runId, CreatePhilosopherEventDto dto);
    Task RecordForkEvent(long runId, CreateForkEventDto dto);
}