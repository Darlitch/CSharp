using StepByStepSimulationNew.Enums;
using StepByStepSimulationNew.Models;
using StepByStepSimulationNew.Models.DTO;

namespace StepByStepSimulationNew;

public class Metrics
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
        for (int i = 0; i < metricDto.Forks.Count; ++i)
        {
            switch (metricDto.Forks[i])
            {
                case ForkState.InUse when metricDto.Philosophers[i].LeftForkState == ForkState.InUse:
                    Console.WriteLine($"Fork-{i+1}: {metricDto.Forks[i]} ({metricDto.Philosophers[i].Name})");
                    break;
                case ForkState.InUse:
                    Console.WriteLine($"Fork-{i+1}: {metricDto.Forks[i]} ({metricDto.Philosophers[(i-1 + metricDto.Philosophers.Count) % metricDto.Philosophers.Count].Name})");
                    break;
                default:
                    Console.WriteLine($"Fork-{i+1}: {metricDto.Forks[i]}");
                    break;
            }
        }
        Console.WriteLine("Пропускная способность:");
        foreach (var philosopher in metricDto.Philosophers)
        {
            Console.WriteLine($"{philosopher.Name}: {((double)philosopher.Eaten / metricDto.Steps * 1000):F2}");
        }
        Console.WriteLine("Время ожидания:");
        int sum = 0;
        int max = 0;
        string maxName = "";
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