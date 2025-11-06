using Contract.Repositories;
using Contract.Services;
using Microsoft.Extensions.Options;
using Model.Entity;
using Model.Enums;

namespace Services;

public class Observer(ISimulationTime simulationTime, IForkEventRepository forkEventRepository, ISimulationRunRepository simulationRunRepository,
    IPhilosopherEventRepository philosopherEventRepository, IOptions<SimulationOptions> options) : IObserver
{
    private long _runId = -1;
    
    public bool ReadyToStart => _runId != -1;

    public async Task RecordSimulationRun()
    {
        var simulation = new SimulationRun(options.Value.DurationSeconds * 1000, options.Value.PhilosophersCount);
        _runId = await simulationRunRepository.AddAsync(simulation);
        Console.WriteLine($"Simulation run id : {_runId}");
    }
    
    public async Task RecordPhilosopherEvent(int index, string name, PhilosopherState state, PhilosopherAction action,
        int eaten, long waitingTime)
    {
        var evt = new PhilosopherEvent(index, name, state, action, eaten, waitingTime, simulationTime.CurrentTimeMs, _runId);
        await philosopherEventRepository.AddAsync(evt);
    }

    public async Task RecordForkEvent(int index, string? owner, ForkState state)
    {
        var evt = new ForkEvent(index, owner, state, simulationTime.CurrentTimeMs, _runId);
        await forkEventRepository.AddAsync(evt);
    }
}