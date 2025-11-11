using Contract.Repositories;
using Contract.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Model.DTO;
using Model.Entity;
using Model.Enums;

namespace Services;

public class RecordManager(IServiceScopeFactory scopeFactory, IOptions<SimulationOptions> options) : IRecordManager
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

    public async Task UpdateSimulationRun(SimulationRunDto dto)
    {
        using var scope = scopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<ISimulationRunRepository>();
        await repository.UpdateAsync(_runId, dto.TimestampMs);
    }
    
    public async Task RecordPhilosopherEvent(PhilosopherEventDto dto)
    {
        using var scope = scopeFactory.CreateScope(); 
        var repository = scope.ServiceProvider.GetRequiredService<IPhilosopherEventRepository>();
        var evt = new PhilosopherEvent(dto.Index, dto.Name, dto.State, dto.Action, dto.Eaten, dto.WaitingTime, dto.CurrentTimeMs, _runId);
        await repository.AddAsync(evt);
    }

    public async Task RecordForkEvent(ForkEventDto dto)
    {
        using var scope = scopeFactory.CreateScope(); 
        var repository = scope.ServiceProvider.GetRequiredService<IForkEventRepository>();
        Console.WriteLine($"{dto.Index} - {dto.Owner} : {dto.CurrentTimeMs} - {dto.State}");
        var evt = new ForkEvent(dto.Index, dto.Owner, dto.State, dto.CurrentTimeMs, _runId);
        await repository.AddAsync(evt);
    }
}