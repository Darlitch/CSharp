using Model.Enums;
using StepByStepSimulationNew.DTO;

namespace StepByStepSimulationNew;

public static class Metrics
{
    public static void PrintMetrics(MetricDto metricDto)
    {
        Console.WriteLine($"==== ШАГ {metricDto.Steps} ====");
        Console.WriteLine("Философы:");
        foreach (var philosopher in metricDto.Philosophers)
        {
            string state = philosopher.State == PhilosopherState.Hungry
                ? $"{philosopher.State} (Action = {philosopher.Action})"
                : $"{philosopher.State} ({philosopher.CurrentActionDuration} steps left)";
            Console.WriteLine($"{philosopher.Name}: {state}, съедено: {philosopher.Eaten}");
        }
        Console.WriteLine("Вилки:");
        for (var i = 0; i < metricDto.Forks.Count; ++i)
        {
            Console.WriteLine(metricDto.Forks[i].State == ForkState.InUse
                ? $"Fork-{i + 1}: {metricDto.Forks[i].State} ({metricDto.Forks[i].Owner})"
                : $"Fork-{i + 1}: {metricDto.Forks[i].State}");
        }
        Console.WriteLine("Пропускная способность:");
        foreach (var philosopher in metricDto.Philosophers)
        {
            Console.WriteLine($"{philosopher.Name}: {((double)philosopher.Eaten / metricDto.Steps * 1000):F2}");
        }
        Console.WriteLine("Время ожидания:");
        var sum = 0;
        var max = 0;
        var maxName = "";
        foreach (var philosopher in metricDto.Philosophers)
        {
            Console.WriteLine($"{philosopher.Name}: {philosopher.WaitingTime}");
            sum += philosopher.WaitingTime;
            if (philosopher.WaitingTime > max)
            {
                max = philosopher.WaitingTime;
                maxName = philosopher.Name;
            }
        }
        Console.WriteLine($"Среднее: {sum / 5}; Максимальное: {max} у {maxName}");
        Console.WriteLine("");
    }

    public static void PrintFinalMetrics(MetricDto metricDto)
    {
        Console.WriteLine($"Всего съедено: {metricDto.Philosophers.Sum(philosopher => philosopher.Eaten)}");
        PrintMetrics(metricDto);
    }
}