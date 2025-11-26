using Model.DTO;
using Model.Entity;

namespace View.Contract;

public interface IViewManager
{
    public Task<SimulationRunDto> GetSimulationRun(long runId);
    public Task<PhilosopherEventDto> GetPhilosopherEvent(long runId, long delay, int index);
    public Task<ForkEventDto> GetForkEvent(long runId, long delay, int index);
}