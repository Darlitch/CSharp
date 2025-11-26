using Contract.Repositories;
using Contract.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Model.DTO;
using Model.Entity;
using Model.Enums;
using Services.Simulation;

namespace Services;

public class RecordManager(ISimulationRunRepository simulationRunRepository,
    IPhilosopherEventRepository philosopherEventRepository,
    IForkEventRepository forkEventRepository,
    IOptions<SimulationOptions> options) : IRecordManager
{
    // private long _runId = -1;
    
    // public bool ReadyToStart => _runId != -1;
    
    public async Task<long> RecordSimulationRun()
    {
        // using var scope = scopeFactory.CreateScope(); 
        // var repository = scope.ServiceProvider.GetRequiredService<ISimulationRunRepository>();
        var simulation = new SimulationRun(options.Value.DurationSeconds * 1000, options.Value.PhilosophersCount);
        var runId = await simulationRunRepository.AddAsync(simulation);
        // Console.WriteLine($"Simulation run id : {runId}");
        return runId;
    }

    public async Task UpdateSimulationRun(long runId, UpdateSimulationRunDto dto)
    {
        // using var scope = scopeFactory.CreateScope();
        // var repository = scope.ServiceProvider.GetRequiredService<ISimulationRunRepository>();
        await simulationRunRepository.UpdateAsync(runId, dto.TimestampMs);
    }
    
    public async Task RecordPhilosopherEvent(long runId, CreatePhilosopherEventDto dto)
    {
        // using var scope = scopeFactory.CreateScope(); 
        // var repository = scope.ServiceProvider.GetRequiredService<IPhilosopherEventRepository>();
        var evt = new PhilosopherEvent(dto.Index, dto.Name, dto.State, dto.Action, dto.Eaten, dto.WaitingTime, dto.CurrentTimeMs, runId);
        await philosopherEventRepository.AddAsync(evt);
    }

    public async Task RecordForkEvent(long runId, CreateForkEventDto dto)
    {
        // using var scope = scopeFactory.CreateScope(); 
        // var repository = scope.ServiceProvider.GetRequiredService<IForkEventRepository>();
        // Console.WriteLine($"{dto.Index} - {dto.Owner} : {dto.CurrentTimeMs} - {dto.State}");
        var evt = new ForkEvent(dto.Index, dto.Owner, dto.State, dto.CurrentTimeMs, runId);
        await forkEventRepository.AddAsync(evt);
    }
}