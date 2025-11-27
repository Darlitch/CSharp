using Contract.Dtos;
using PhilosopherService.Domain;

namespace PhilosopherService.Application.Extensions;

public static class DtoExtension
{
    public static PhilosopherMetricsDto ToPhilosopherMetricsDto(this PhilosopherMetrics metrics)
        => new PhilosopherMetricsDto(metrics.State, metrics.Action, metrics.CurrentActionDuration, metrics.Eaten,
            metrics.WaitingTime);
}