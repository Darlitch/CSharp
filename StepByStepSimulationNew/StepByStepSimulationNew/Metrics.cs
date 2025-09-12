using StepByStepSimulationNew.Enums;
using StepByStepSimulationNew.Models;
using StepByStepSimulationNew.Models.DTO;

namespace StepByStepSimulationNew;

public class Metrics
{
    public static void PrintMetrics(MetricDto metricDto)
    {
        for (int i = 0; i < metricDto.Philosophers.Count; ++i)
        {
            var philosopher = metricDto.Philosophers[i];
            string state = philosopher.State == PhilosopherState.Hungry
                ? $"{philosopher.State} (Action = {philosopher.Action})"
                : $"{philosopher.State} ({philosopher.CurrentActionDuration} steps left)";
            Console.WriteLine($"{philosopher.Name}: {philosopher.Action}");
        }
    }
}