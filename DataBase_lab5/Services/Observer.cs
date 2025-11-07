using Contract.Repositories;
using Contract.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Model.Entity;
using Model.Enums;

namespace Services;

public class Observer(ISimulationTime simulationTime, IServiceScopeFactory scopeFactory, IOptions<SimulationOptions> options) : IObserver
{
    private long _runId = -1;
    
    public bool ReadyToStart => _runId != -1;

    public async Task RecordSimulationRun()
    {
        using var scope = scopeFactory.CreateScope(); 
        var repository = scope.ServiceProvider.GetRequiredService<ISimulationRunRepository>();
        var simulation = new SimulationRun(options.Value.DurationSeconds * 1000, options.Value.PhilosophersCount);
        _runId = await repository.AddAsync(simulation);
        Console.WriteLine($"Simulation run id : {_runId}");
    }
    
    public async Task RecordPhilosopherEvent(int index, string name, PhilosopherState state, PhilosopherAction action,
        int eaten, long waitingTime)
    {
        using var scope = scopeFactory.CreateScope(); 
        var repository = scope.ServiceProvider.GetRequiredService<IPhilosopherEventRepository>();
        var evt = new PhilosopherEvent(index, name, state, action, eaten, waitingTime, simulationTime.CurrentTimeMs, _runId);
        await repository.AddAsync(evt);
    }

    public async Task RecordForkEvent(int index, string? owner, ForkState state)
    {
        using var scope = scopeFactory.CreateScope(); 
        var repository = scope.ServiceProvider.GetRequiredService<IForkEventRepository>();
        var evt = new ForkEvent(index, owner, state, simulationTime.CurrentTimeMs, _runId);
        await repository.AddAsync(evt);
    }
}