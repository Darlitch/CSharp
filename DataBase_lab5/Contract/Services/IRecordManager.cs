using Model.DTO;

namespace Contract.Services;

public interface IRecordManager
{
    bool ReadyToStart { get; }
    Task RecordSimulationRun();
    Task UpdateSimulationRun(UpdateSimulationRunDto dto);
    Task RecordPhilosopherEvent(CreatePhilosopherEventDto dto);
    Task RecordForkEvent(CreateForkEventDto dto);
}