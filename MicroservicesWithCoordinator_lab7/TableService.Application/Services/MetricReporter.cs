using Application.Abstractions;
using Contract.Enums;
using TableService.Domain.Enums;

namespace Application.Services;

public class MetricReporter(ITableManager tableManager) : IMetricReporter
{
    public void PrintMetrics(long currTime)
    {
        var philosophersMetrics = tableManager.GetMetricsSnapshots();
        var forksMetrics = tableManager.GetForksSnapshots();
        if (currTime == 0) return;
        Console.WriteLine($"==== ВРЕМЯ {currTime} МС ====");
        Console.WriteLine("Философы:");
        foreach (var metrics in philosophersMetrics)
        {
            var state = metrics.State == PhilosopherState.Hungry
                ? $"{metrics.State} (Action = {metrics.Action})"
                : $"{metrics.State} ({metrics.CurrentActionDuration} ms left)";
            Console.WriteLine($"({metrics.Index}) {metrics.Name}: {state}, съедено: {metrics.Eaten}");
        }
        Console.WriteLine("");
        Console.WriteLine("Вилки:");
        foreach (var metrics in forksMetrics)
        {
            Console.WriteLine(metrics.State == ForkState.InUse
                ? $"Fork-{metrics.Index}: {metrics.State} ({metrics.Owner})"
                : $"Fork-{metrics.Index}: {metrics.State}");
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
        Console.WriteLine($"Среднее: {sum / tableManager.PhilosophersCount} мс; Максимальное: {max} мс у {maxName}");
        Console.WriteLine("");
        Console.WriteLine("Коэффициент утилизации:");
        foreach (var metrics in forksMetrics)
        {
            Console.WriteLine($"Fork-{metrics.Index}: Вилка свободна {((double)metrics.FreeTime / currTime * 100):00.00}%; " +
                              $"Вилка занята {((double)metrics.BlockTime / currTime * 100):00.00}%");
        }
        Console.WriteLine("========================");
        Console.WriteLine("");
    }

    public void PrintFinalMetrics(long currTime)
    {
        PrintMetrics(currTime);
        Console.WriteLine($"Всего съедено: {tableManager.GetMetricsSnapshots().Sum(philosopher => philosopher.Eaten)}");
    }
}