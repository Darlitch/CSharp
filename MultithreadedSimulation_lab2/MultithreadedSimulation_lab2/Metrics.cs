using Model.Enums;
using MultithreadedSimulation_lab2.DTO;

namespace MultithreadedSimulation_lab2;

public static class Metrics
{
    public static void PrintMetrics(MetricDto metricDto)
    {
        Console.WriteLine($"==== ВРЕМЯ {metricDto.CurrTime} МС ====");
        Console.WriteLine("Философы:");
        foreach (var philosopher in metricDto.Philosophers)
        {
            string state = philosopher.State == PhilosopherState.Hungry
                ? $"{philosopher.State} (Action = {philosopher.Action})"
                : $"{philosopher.State} ({philosopher.CurrentActionDuration} ms left)";
            Console.WriteLine($"{philosopher.Name}: {state}, съедено: {philosopher.Metrics.Eaten}");
        }
        Console.WriteLine("");
        Console.WriteLine("Вилки:");
        for (var i = 0; i < metricDto.Forks.Count; ++i)
        {
            Console.WriteLine(metricDto.Forks[i].State == ForkState.InUse
                ? $"Fork-{i + 1}: {metricDto.Forks[i].State} ({metricDto.Forks[i].Owner})"
                : $"Fork-{i + 1}: {metricDto.Forks[i].State}");
        }
        Console.WriteLine("");
        Console.WriteLine("Пропускная способность:");
        foreach (var philosopher in metricDto.Philosophers)
        {
            Console.WriteLine($"{philosopher.Name}: {((double)philosopher.Metrics.Eaten / metricDto.CurrTime):F7} ед/мс");
        }
        Console.WriteLine("");
        Console.WriteLine("Время ожидания:");
        long sum = 0;
        long max = 0;
        var maxName = "";
        foreach (var philosopher in metricDto.Philosophers)
        {
            Console.WriteLine($"{philosopher.Name}: {philosopher.Metrics.WaitingTime}");
            sum += philosopher.Metrics.WaitingTime;
            if (philosopher.Metrics.WaitingTime > max)
            {
                max = philosopher.Metrics.WaitingTime;
                maxName = philosopher.Name;
            }
        }
        Console.WriteLine($"Среднее: {sum / 5} мс; Максимальное: {max} мс у {maxName}");
        Console.WriteLine("");
        Console.WriteLine("Коэффициент утилизации:");
        for (var i = 0; i < metricDto.Forks.Count; ++i)
        {
            Console.WriteLine($"Fork-{i + 1}: Вилка свободна {((double)metricDto.Forks[i].FreeTime / metricDto.CurrTime * 100):00.00}%; " +
                              $"Вилка занята {((double)metricDto.Forks[i].BlockTime / metricDto.CurrTime * 100):00.00}%");
        }
        Console.WriteLine("========================");
        Console.WriteLine("");
    }

    public static void PrintFinalMetrics(MetricDto metricDto)
    {
        PrintMetrics(metricDto);
        Console.WriteLine($"Всего съедено: {metricDto.Philosophers.Sum(philosopher => philosopher.Metrics.Eaten)}");
    }
}