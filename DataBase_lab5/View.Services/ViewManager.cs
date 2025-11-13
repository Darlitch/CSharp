using Contract.Repositories;
using Microsoft.Extensions.Logging;
using Model.DTO;
using View.Contract;

namespace View.Services;

public class ViewManager(IPhilosopherEventRepository philosopherEventRepository, IForkEventRepository forkEventRepository, ISimulationRunRepository simulationRunRepository) : IViewManager
{
    public async Task<SimulationRunDto> GetSimulationRun(long runId)
    {
        var run = await simulationRunRepository.GetAsync(runId);
        if (run == null)
        {
            throw new ViewException($"Симуляция с runId={runId} не найдена.");
        }
        return new SimulationRunDto(run.DurationMs, run.PhilosophersCount);
    }
    
    public async Task<PhilosopherEventDto> GetPhilosopherEvent(long runId, long delay, int index)
    {
        var state = await philosopherEventRepository.GetAsync(runId, delay, index);
        if (state == null)
        {
            throw new ViewException(
                $"Философ с индексом {index} в симуляции с runId={runId} в момент времени {delay} не найден");
        }
        return state.ToPhilosopherEventDto();
    }

    public async Task<ForkEventDto> GetForkEvent(long runId, long delay, int index)
    {
        var state = await forkEventRepository.GetAsync(runId, delay, index);
        if (state == null)
        {
            throw new ViewException(
                $"Вилка с индексом {index} в симуляции с runId={runId} в момент времени {delay} не найдена");
        }
        return state.ToForkEventDto();
    }
}