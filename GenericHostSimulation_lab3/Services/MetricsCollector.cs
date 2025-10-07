using IServices;
using Microsoft.Extensions.Hosting;
using Model.Enums;

namespace Services;

public class MetricsCollector(ITableManager tableManager, IEnumerable<IHostedService> philosophers) : IMetricsCollector
{
    // private readonly ITableManager tableManager = tableManager;
    // private readonly IEnumerable<IHostedService> philosophers = philosophers;
    // ИДЕ их не использует, видимо из-за оптимизации. Можно ли сделать так?
    
    public void PrintMetrics(long currTime)
    {
        Console.WriteLine($"==== ВРЕМЯ {currTime} МС ====");
        Console.WriteLine("Философы:");
        foreach (var philosopher in philosophers.OfType<PhilosopherHostedService>())
        {
            var state = philosopher.State == PhilosopherState.Hungry
                ? $"{philosopher.State} (Action = {philosopher.Action})"
                : $"{philosopher.State} ({philosopher.CurrentActionDuration} ms left)";
            Console.WriteLine($"{philosopher.Name}: {state}, съедено: {philosopher.Metrics.Eaten}");
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
        foreach (var philosopher in philosophers.OfType<PhilosopherHostedService>())
        {
            Console.WriteLine($"{philosopher.Name}: {((double)philosopher.Metrics.Eaten / currTime):F7} ед/мс");
        }
        Console.WriteLine("");
        Console.WriteLine("Время ожидания:");
        long sum = 0;
        long max = 0;
        var maxName = "";
        foreach (var philosopher in philosophers.OfType<PhilosopherHostedService>())
        {
            Console.WriteLine($"{philosopher.Name}: {philosopher.Metrics.WaitingTime}");
            sum += philosopher.Metrics.WaitingTime;
            if (philosopher.Metrics.WaitingTime <= max) continue;
            max = philosopher.Metrics.WaitingTime;
            maxName = philosopher.Name;
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

    public void PrintFinalMetrics(long currTime)
    {
        PrintMetrics(currTime);
        Console.WriteLine($"Всего съедено: {philosophers.OfType<PhilosopherHostedService>().Sum(philosopher => philosopher.Metrics.Eaten)}");
    }
}