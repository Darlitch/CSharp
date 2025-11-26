using Application.Abstractions;
using Contract.Dtos;
using Contract.Enums;
using TableService.Domain.Enums;

namespace Application.Services;

public class MetricReporter(ITableManager tableManager) : IMetricReporter
{
    public void PrintMetrics(long currTime, List<PhilosopherMetricsDto> philosophersMetrics)
    {
        if (currTime == 0) return;
        Console.WriteLine($"==== ВРЕМЯ {currTime} МС ====");
        Console.WriteLine("Философы:");
        foreach (var metrics in philosophersMetrics)
        {
            var state = metrics.State == PhilosopherState.Hungry
                ? $"{metrics.State} (Action = {metrics.Action})"
                : $"{metrics.State} ({metrics.CurrentActionDuration} ms left)";
            Console.WriteLine($"{metrics.Name}: {state}, съедено: {metrics.Eaten}");
        }
        Console.WriteLine("");
        Console.WriteLine("Вилки:");
        for (var i = 0; i < tableManager.PhilosophersCount; ++i)
        {
            Console.WriteLine(tableManager.GetFork(i).State == ForkState.InUse
                ? $"Fork-{i + 1}: {tableManager.GetFork(i).State} ({tableManager.GetFork(i).Owner})"
                : $"Fork-{i + 1}: {tableManager.GetFork(i).State}");
        }
        Console.WriteLine("");
        Console.WriteLine("Пропускная способность:");
        foreach (var metrics in philosophersMetrics)
        {
            Console.WriteLine($"{metrics.Name}: {((double)metrics.Eaten / currTime):F7} ед/мс");
        }
        Console.WriteLine("");
        Console.WriteLine("Время ожидания:");
        long sum = 0;
        long max = 0;
        var maxName = "";
        foreach (var metrics in philosophersMetrics)
        {
            Console.WriteLine($"{metrics.Name}: {metrics.WaitingTime}");
            sum += metrics.WaitingTime;
            if (metrics.WaitingTime <= max) continue;
            max = metrics.WaitingTime;
            maxName = metrics.Name;
        }
        Console.WriteLine($"Среднее: {sum / 5} мс; Максимальное: {max} мс у {maxName}");
        Console.WriteLine("");
        Console.WriteLine("Коэффициент утилизации:");
        for (var i = 0; i < tableManager.PhilosophersCount; ++i)
        {
            Console.WriteLine($"Fork-{i + 1}: Вилка свободна {((double)tableManager.GetFork(i).FreeTime / currTime * 100):00.00}%; " +
                              $"Вилка занята {((double)tableManager.GetFork(i).BlockTime / currTime * 100):00.00}%");
        }
        Console.WriteLine("========================");
        Console.WriteLine("");
    }

    public void PrintFinalMetrics(long currTime, List<PhilosopherMetricsDto> philosophersMetrics)
    {
        PrintMetrics(currTime,  philosophersMetrics);
        Console.WriteLine($"Всего съедено: {philosophersMetrics.Sum(pM => pM.Eaten)}");
    }
}