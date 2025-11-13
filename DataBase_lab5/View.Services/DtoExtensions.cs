using Model.DTO;
using Model.Entity;

namespace View.Services;

public static class DtoExtensions
{
    public static SimulationRunDto ToSimulationRunDto(this SimulationRun run)
        => new SimulationRunDto(run.DurationMs, run.PhilosophersCount);

    public static PhilosopherEventDto ToPhilosopherEventDto(this PhilosopherEvent evt)
        => new PhilosopherEventDto(evt.Index, evt.Name, evt.State, evt.Action, evt.Eaten, evt.WaitingTime);

    public static ForkEventDto ToForkEventDto(this ForkEvent evt)
        => new ForkEventDto(evt.Index, evt.Owner, evt.State);
}