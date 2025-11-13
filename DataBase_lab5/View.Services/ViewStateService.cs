using Model.DTO;
using Model.Enums;
using View.Contract;

namespace View.Services;

public class ViewStateService(IViewManager viewManager) : IViewStateSimulation
{
    public async Task Run(long runId, double delay)
    {
        var delayMs = (long)(delay * 1000);
        var simulationDto = await viewManager.GetSimulationRun(runId);
        if (simulationDto.DurationMs < delayMs)
        {
            throw new ViewException($"Delay не корректен. Данная симуляция длится {simulationDto.DurationMs} мс.");
        }
        var philosopherEvents = new PhilosopherEventDto[simulationDto.PhilosopherCount];
        for (var i = 0; i < simulationDto.PhilosopherCount; ++i)
        {
            philosopherEvents[i] = await viewManager.GetPhilosopherEvent(runId, delayMs, i+1);
        }
        var forkEvents = new ForkEventDto[simulationDto.PhilosopherCount];
        for (var i = 0; i < simulationDto.PhilosopherCount; ++i)
        {
            forkEvents[i] = await viewManager.GetForkEvent(runId, delayMs, i+1);
        }
        
        Console.WriteLine($"==== ВРЕМЯ {delayMs} МС ====");
        Console.WriteLine("Философы:");
        foreach (var philEvt in philosopherEvents)
        {
            var state = philEvt.State == PhilosopherState.Hungry
                ? $"{philEvt.State} (Action = {philEvt.Action})"
                : $"{philEvt.State}";
            Console.WriteLine($"({philEvt.Index}) {philEvt.Name}: {state}, съедено: {philEvt.Eaten}");
        }
        Console.WriteLine("");
        Console.WriteLine("Вилки:");
        foreach (var forkEvt in forkEvents)
        {
            Console.WriteLine(forkEvt.Owner == null
                ? $"Fork-{forkEvt.Index}: {forkEvt.State}"
                : $"Fork-{forkEvt.Index}: {forkEvt.State} ({forkEvt.Owner})");
        }
        Console.WriteLine("");
        Console.WriteLine("Пропускная способность:");
        for (var i = 0; i < simulationDto.PhilosopherCount; ++i)
        {
            var philEvt = philosopherEvents[i];
            Console.WriteLine($"{philEvt.Name}: {((double)philEvt.Eaten / delayMs):F7} ед/мс");
        }
        Console.WriteLine("");
        Console.WriteLine("Время ожидания:");
        long sum = 0;
        long max = 0;
        var maxName = "";
        foreach (var philEvt in philosopherEvents)
        {
            Console.WriteLine($"{philEvt.Name}: {philEvt.WaitingTime}");
            sum += philEvt.WaitingTime;
            if (philEvt.WaitingTime <= max) continue;
            max = philEvt.WaitingTime;
            maxName = philEvt.Name;
        }
        Console.WriteLine($"Среднее: {sum / 5} мс; Максимальное: {max} мс у {maxName}");
        Console.WriteLine("");
    }
}